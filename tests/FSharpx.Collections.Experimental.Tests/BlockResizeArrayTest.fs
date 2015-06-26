module FSharpx.Collections.Experimental.Tests.BlockResizeArrayTest

open FSharpx.Collections.Experimental
open NUnit.Framework
open FsUnit
open FSharpx.Collections.TimeMeasurement

let arraySize = 2048*2048*10
let testIters = 10
let x = 1000UL

[<Test>]
let ``allocation performance`` () =
    
    averageTime testIters "ResizeArrayAlloc" (fun () -> 
        let a = new ResizeArray<uint64>()
        for i in 0..arraySize do a.Add x )    
    averageTime testIters "BlockResizeArrayAlloc" (fun () -> 
        let a = new BlockResizeArray<uint64>()
        for i in 0..arraySize do a.Add x )  

[<Test>]
let ``random access performance`` () =
    let rand = System.Random()
    let access = [|for i in 0..arraySize-1 -> rand.Next(0, arraySize - 1)|]
    let a = Array.init arraySize  (fun _ -> x)
    let ra = new ResizeArray<uint64>()
    for i in 0..arraySize do ra.Add x
    let bra = new BlockResizeArray<uint64>()
    for i in 0..arraySize do bra.Add x
    let b = ref 0UL
    averageTime testIters "ResizeArray random access" (fun () -> for i in access do ra.[i] <- 0UL) 
    averageTime testIters "Array random access" (fun () -> for i in access do a.[i] <- 0UL)       
    averageTime testIters "BlockResizeArray random access" (fun () -> for i in access do bra.[i] <- 0UL)

[<Test>]
let ``sequential access performance`` () =
    let rand = System.Random()
    let access = [|0..arraySize-1|]
    let a = Array.init arraySize  (fun _ -> x)
    let ra = new ResizeArray<uint64>()
    for i in 0..arraySize do ra.Add x
    let bra = new BlockResizeArray<uint64>()
    for i in 0..arraySize do bra.Add x
    let b = ref 0UL
    averageTime testIters "ResizeArray sequential access" (fun () -> for i in access do ra.[i] <- 0UL)    
    averageTime testIters "Array sequential access" (fun () -> for i in access do a.[i] <- 0UL)    
    averageTime testIters "BlockResize sequential access" (fun () -> for i in access do bra.[i] <- 0UL)

[<Test>]
let ``map function test`` () =
    let bra = BlockResizeArray.Init 1000000 (fun i -> i)
    let resArr = Array.init 1000000 (fun i -> i * 2)
    let bra = bra.Map (fun i -> i * 2)
    let mutable res = true 
    for i = 0 to 999999 do
        res <- res && resArr.[i] = bra.[i]
    Assert.AreEqual(res, true)


[<Test>]
let ``iter function test`` () =
    let bra = BlockResizeArray.Init 1000000 (fun i -> i)
    let resArr = Array.init 1000000 (fun i -> i)
    Array.iter (fun i -> bra.[i] <- i * 2) resArr
    bra.Iter (fun i -> bra.[i] <- i * 2)
    let mutable res = true 
    for i = 0 to 999999 do
        res <- res && resArr.[i] = bra.[i]
    Assert.AreEqual(res, true)

[<Test>]
let ``init function test`` () =
    let bra = BlockResizeArray.Init 1000000 (fun i -> i)
    let resArr = Array.init 1000000 (fun i -> i)
    let mutable res = true 
    let mutable tmp = 0
    for i = 0 to 999999 do
        res <- res && resArr.[i] = bra.[i]
    Assert.AreEqual((res && bra.Count = 1000000), true)

[<Test>]
let ``zeroCreate function test`` () =
    let bra = BlockResizeArray<_>.ZeroCreate 100000
    Assert.AreEqual(bra.Count, 100000)

[<Test>]
let ``find function test`` () =
    let bra = BlockResizeArray.Init 10000000 (fun i -> i)
    let res = bra.Find (fun i -> i <> 0 && i % 5000000 = 0)
    Assert.AreEqual(res, 5000000)

[<Test>]
let ``tryFind function test`` () =
    let bra = BlockResizeArray.Init 1000000 (fun i -> i)
    let res = bra.TryFind (fun i -> i <> 0 && i % 500000 = 0)
    Assert.AreEqual(res.IsSome, true)
    Assert.AreEqual(res.Value, 500000)

[<Test>]
let ``filter function test`` () =
    let a = Array.init 100000 (fun i -> i)
    let c = Array.filter (fun i -> i % 10000 = 0) a
    let bra = BlockResizeArray.Init 100000 (fun i -> i)
    let bra = bra.Filter (fun i -> i % 10000 = 0)
    Assert.AreEqual(bra.Count, 10)
    let mutable res = true 
    for i = 0 to 9 do
        res <- res && bra.[i] = i * 10000
    Assert.AreEqual(res, true)
        

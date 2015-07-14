module FSharpx.Collections.Experimental.Tests.BlockResizeArrayTest

open FSharpx.Collections.Experimental
open NUnit.Framework
open FsUnit
open FsCheck
open FSharpx.Collections.TimeMeasurement

let arraySize = 2048*2048*10
let testIters = 10
let x = 1000UL
let testLen = 1000000
let r = System.Random()

let compareByElems (bra : BlockResizeArray<'T>) (arr : 'T []) = 
    let mutable res = true 
    for i = 0 to bra.Count - 1 do
        res <- res && arr.[i] = bra.[i]
    res

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
let ``map performance`` () =
    let a = Array.init arraySize  (fun _ -> x)
    let ra = new ResizeArray<uint64>()
    for i in 0..arraySize do ra.Add x
    let bra = BlockResizeArray.Init arraySize (fun _ -> x)
    averageTime testIters "ResizeArray map" (fun () -> (Seq.map (fun x -> x*2UL) ra)) 
    averageTime testIters "Array map" (fun () -> (Array.map (fun x -> x*2UL) a))       
    averageTime testIters "BlockResizeArray map" (fun () -> bra.Map (fun x -> x*2UL))

[<Test>]
let ``map function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i * 2)
    let bra = bra.Map (fun i -> i * 2)
    Assert.AreEqual(compareByElems bra a, true)

[<Test>]
let ``iter function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i)
    Array.iter (fun i -> a.[i] <- i * 2) a
    bra.Iter (fun i -> bra.[i] <- i * 2)
    Assert.AreEqual(compareByElems bra a, true)

[<Test>]
let ``init function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i)
    Assert.AreEqual((compareByElems bra a && bra.Count = testLen), true)

[<Test>]
let ``zeroCreate function test`` () =
    let bra = BlockResizeArray<_>.ZeroCreate testLen
    Assert.AreEqual(bra.Count, testLen)

[<Test>]
let ``find function test`` () =
    let c = 500000
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let res = bra.Find (fun i -> i <> 0 && i % c = 0)
    Assert.AreEqual(res, c)

[<Test>]
let ``tryFind function test`` () =
    let c = 500000
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let res = bra.TryFind (fun i -> i <> 0 && i % c = 0)
    Assert.AreEqual(res.IsSome, true)
    Assert.AreEqual(res.Value, c)

[<Test>]
let ``filter function test`` () =
    let c = 10000
    let a = Array.init testLen (fun i -> i)
    let a = Array.filter (fun i -> i % c = 0) a
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let bra = bra.Filter (fun i -> i % c = 0)
    Assert.AreEqual(bra.Count, 100)
    Assert.AreEqual(compareByElems bra a, true)
        
[<Test>]
let ``fold function test`` () =
    let a = Array.init testLen (fun i -> i)
    let aRes = Array.fold (fun acc elem -> acc + elem) 0 a
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let braRes = bra.Fold (fun acc elem -> acc + elem) 0
    Assert.AreEqual(braRes, aRes)

let createBra count =
    BlockResizeArray.Init count (fun i -> i) 

let mapTest f с =    
    //let с = r.Next(0, testLen)
    let c = abs с
    let bra = createBra c
    let arr = Array.init с (fun i -> i)
    let b = bra.Map f
    let a = Array.map f arr
    compareByElems b a

[<Test>]
let ``Map2``() =
   Check.Verbose <| mapTest (fun e -> e * 2)
    
    

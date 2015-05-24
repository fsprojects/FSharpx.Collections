module FSharpx.Collections.Experimental.Tests.BlockResizeArrayTest

open FSharpx.Collections
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
    averageTime testIters "BlockResizeArray random access" (fun () -> for i in access do bra.Set i 0UL)

[<Test>]
let ``seqential access performance`` () =
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
    averageTime testIters "BlockResize sequential access" (fun () -> for i in access do bra.Set i 0UL)

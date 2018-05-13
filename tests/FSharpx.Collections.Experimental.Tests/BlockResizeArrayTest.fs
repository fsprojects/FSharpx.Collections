module FSharpx.Collections.Experimental.Tests.BlockResizeArrayTest

open FSharpx.Collections.Experimental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open FSharpx.Collections.TimeMeasurement

let arraySize = 2048*2048*10
let testIters = 10
let x = 1000UL
let testLen = 1000000
let rand = System.Random()

let compareByElems (bra : BlockResizeArray<'T>) (arr : 'T []) = 
    let mutable res = true   
    Assert.AreEqual(arr.Length, bra.Length, "Length of bra is not equal of array length")
    let diff = new ResizeArray<_>()
    for i = 0 to bra.Length - 1 do
        if arr.[i] <> bra.[i] then diff.Add (i,arr.[i],bra.[i])
        res <- res && arr.[i] = bra.[i]
    printfn "Content of arrays are not equals in position %A. Length of arr = %A " diff bra.Length
    Assert.IsTrue(res, "")

let compareByElems2 (bra : BlockResizeArray<'T>) (arr : 'T []) = 
    let mutable res = true   
    let diff = new ResizeArray<_>()
    for i = 0 to bra.Length - 1 do
        if arr.[i] <> bra.[i] then diff.Add (i,arr.[i],bra.[i])
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
    averageTime testIters "ResizeArray map" (fun () -> (Microsoft.FSharp.Collections.ResizeArray.map (fun x -> x*2UL) ra)) 
    averageTime testIters "Array map" (fun () -> (Array.map (fun x -> x*2UL) a))       
    averageTime testIters "BlockResizeArray map" (fun () -> bra.Map (fun x -> x*2UL))

[<Test>]
let ``iter performance`` () =
    let a = Array.init arraySize  (fun _ -> x)
    let ra = new ResizeArray<uint64>()
    for i in 0..arraySize do ra.Add x
    let bra = BlockResizeArray.Init arraySize (fun _ -> x)
    averageTime testIters "ResizeArray iter" (fun () -> (Microsoft.FSharp.Collections.ResizeArray.iter (fun i -> ()) ra)) 
    averageTime testIters "Array iter" (fun () -> (Array.iter (fun i -> ())))       
    averageTime testIters "BlockResizeArray iter" (fun () -> bra.Iter (fun i -> ()))

[<Test>]
let ``fold performance`` () =
    let a = Array.init arraySize  (fun _ -> x)
    let ra = new ResizeArray<uint64>()
    for i in 0..arraySize do ra.Add x
    let bra = BlockResizeArray.Init arraySize (fun _ -> x)
    averageTime testIters "ResizeArray fold" (fun () -> (Microsoft.FSharp.Collections.ResizeArray.fold (fun x acc -> acc + x*2UL) 0UL ra)) 
    averageTime testIters "Array fold" (fun () -> (Array.fold (fun x acc -> acc + x*2UL) 0UL a))       
    averageTime testIters "BlockResizeArray fold" (fun () -> (FSharpx.Collections.Experimental.BlockResizeArray.fold (fun x acc -> acc + x*2UL) 0UL bra))

[<Test>]
let ``find performance`` () =
    let x = 1000
    let a = Array.init arraySize  (fun i -> x + i)
    let ra = new ResizeArray<int>()
    let s = 1010000
    for i in 0..arraySize do ra.Add (x + i)
    let bra = BlockResizeArray.Init arraySize (fun i -> x + i)
    averageTime testIters "ResizeArray find" (fun () -> Microsoft.FSharp.Collections.ResizeArray.find (fun e -> e <> 0 && e % s = 0) ra)
    averageTime testIters "Array find" (fun () -> (Array.find (fun e -> e <> 0 && e % s = 0) a))       
    averageTime testIters "BlockResizeArray find" (fun () -> (FSharpx.Collections.Experimental.BlockResizeArray.find (fun e -> e <> 0 && e % s = 0) bra))

[<Test>]
let ``map function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i * 2)
    let bra = bra.Map (fun i -> i * 2)
    compareByElems bra a

[<Test>]
let ``iter function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i)
    Array.iter (fun i -> a.[i] <- i * 2) a
    bra.Iter (fun i -> bra.[i] <- i * 2)
    compareByElems bra a

[<Test>]
let ``init function test`` () =
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let a = Array.init testLen (fun i -> i)
    compareByElems bra a
    Assert.AreEqual(bra.Length, testLen)

[<Test>]
let ``zeroCreate function test`` () =
    let bra = BlockResizeArray<_>.ZeroCreate testLen
    Assert.AreEqual(bra.Length, testLen)

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
    let bra = bra.Filter (fun e -> e % c = 0)
    compareByElems bra a
        
[<Test>]
let ``fold function test`` () =
    let a = Array.init testLen (fun i -> i)
    let aRes = Array.fold (fun acc elem -> acc + elem) 0 a
    let bra = BlockResizeArray.Init testLen (fun i -> i)
    let braRes = bra.Fold (fun acc elem -> acc + elem) 0
    Assert.AreEqual(braRes, aRes) 

type ArbitraryModifiers =    
    static member BlockResizeArray() = 
        Arb.generate<int> 
        |> Gen.filter (fun i -> i >= 0)
        |> Gen.map (fun i -> BlockResizeArray.Init  (i * 10000) (fun i -> rand.Next()))
        |> Arb.fromGen

[<SetUp>]
let f () = Arb.register<ArbitraryModifiers>() |> ignore 

[<Test>]
let ``Random map``() =   
    let testFun f (bra:BlockResizeArray<int>) =        
        let arr = bra.ToArray()
        let b = bra.Map f
        let a = Array.map f arr
        compareByElems b a
    Check.VerboseThrowOnFailure <| testFun (fun e -> e * 2)
    
[<Test>]
let ``Random filter``() =   
    let testFun f (bra:BlockResizeArray<int>) =       
        let arr = bra.ToArray()
        let b = bra.Filter f
        let a = Array.filter f arr
        compareByElems b a        
    Check.VerboseThrowOnFailure <| testFun (fun e -> e % 3 = 2)
  
[<Test>]
let ``Random TryFind``() =   
    let testFun f (bra:BlockResizeArray<int>) =        
        let arr = bra.ToArray()
        let b = bra.TryFind f
        let a = Array.tryFind f arr
        Assert.IsTrue((b = a))
    Check.VerboseThrowOnFailure <| testFun (fun e -> e % 3 = 2)        

[<Test>]
let ``Random Find``() =   
    let testFun f (bra:BlockResizeArray<int>) =        
        let arr = bra.ToArray()
        let b = 
            try 
                bra.Find f |> Some
            with
            | :? System.Collections.Generic.KeyNotFoundException -> None
        let a = 
            try 
                Array.find f arr |> Some
            with
            | :? System.Collections.Generic.KeyNotFoundException -> None

        Assert.IsTrue((b = a))
    Check.VerboseThrowOnFailure <| testFun (fun e -> e % 3 = 2) 

[<Test>]
let ``Random ToArray``() =   
    let testFun (bra:BlockResizeArray<int>) =        
        let arr = bra.ToArray()
        compareByElems bra arr
    Check.VerboseThrowOnFailure <| testFun

[<Test>]
let ``Random iter``() =   
    let testFun f (bra:BlockResizeArray<int>) =
        let arr = bra.ToArray()
        let acc1 = ref 0
        let acc2 = ref 0
        bra.Iter (f acc1)
        Array.iter (f acc2) arr
        Assert.AreEqual(!acc1, !acc2)
    Check.VerboseThrowOnFailure <| testFun (fun acc -> (fun e -> acc := !acc + e))

[<Test>]
let ``Random fold``() =   
    let testFun f (bra:BlockResizeArray<int>) =
        let arr = bra.ToArray()        
        let r1 = bra.Fold f 0
        let r2 =  Array.fold f 0 arr
        Assert.AreEqual(r2, r1)
    Check.VerboseThrowOnFailure <| testFun (fun s e -> s + e)

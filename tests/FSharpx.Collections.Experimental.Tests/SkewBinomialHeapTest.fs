[<NUnit.Framework.TestFixture(Category = "SkewBinomialHeap, Heap, Priority Queue")>]
module FSharpx.Collections.Experimental.Tests.SkewBinomialHeapTest

open FSharpx.Collections.Experimental
open FsCheck
open FsCheck.NUnit
open NUnit.Framework
open FsUnit
open System

let ofList desc items = List.fold (fun h x -> SkewBinomialHeap.insert x h) (SkewBinomialHeap.empty desc) items

let sortList heap items = items |> List.sort |> if SkewBinomialHeap.isDescending heap then List.rev else id

let actualHead heap items = items |> if SkewBinomialHeap.isDescending heap then List.max else List.min

let actualTail heap = sortList heap >> List.tail

type ComparableGenerator<'T when 'T :> IComparable> =
    static member Generate () =
        gen{
            let! t = Arb.generate<'T>
            return (t :> System.IComparable)}
        |> Arb.fromGen

type HeapData<'T, 'L when 'T: comparison> = { Heap: 'T SkewBinomialHeap; Items: 'L list; Desc: bool }

type SkewBinomialHeapGenerators() =
    // Empty heaps only, some are empty after removing all its elements
    static member private EmptyOnly<'T when 'T: comparison> (): Gen<HeapData<'T, 'T>> = gen{
        let! desc = Gen.elements [true; false]
        let! s = Gen.listOf (Arb.generate<'T>)
        return { Heap = Seq.init (s.Length) id |> Seq.fold (fun heap _ -> heap |> SkewBinomialHeap.tail) (ofList desc s); Items = []; Desc = desc }}
    // Non-emtpy heaps only, after a sucession of insertions and deletions
    static member private NonemptyOnly<'T when 'T: comparison> (): Gen<HeapData<'T, 'T>> = gen{
        let! desc = Gen.elements [true; false]
        let! t = Gen.elements [true; false]
        let! s = Gen.nonEmptyListOf (Arb.generate<'T>)
        let! ndel = if t then Gen.constant Int32.MaxValue else Gen.choose (2, max 2 (s |> List.length))
        let (heap, list, _) =
            s 
            |> List.fold 
                (fun (heap, lst, k) item ->
                    let newHeap = heap |> SkewBinomialHeap.insert item
                    let newList = item::lst
                    if k + 1 = ndel then
                        (newHeap |> SkewBinomialHeap.tail, newList |> List.sort |> (if desc then List.rev else id) |> List.tail, 0)
                    else
                        (newHeap, newList, k + 1))
                (SkewBinomialHeap.empty desc, [], 0)
        return {Heap = heap; Items = list; Desc = desc}}
    
    static member private Mixed<'T when 'T: comparison> (): Gen<HeapData<'T, 'T>> =
        Gen.frequency [200, SkewBinomialHeapGenerators.EmptyOnly<'T> (); 800, SkewBinomialHeapGenerators.NonemptyOnly<'T> ()]

    static member private TwoMixed<'T when 'T: comparison> (): Gen<HeapData<'T, 'T> * HeapData<'T, 'T>> =
        Gen.frequency [
            200, SkewBinomialHeapGenerators.EmptyOnly<'T> () |> Gen.two; 
            400, SkewBinomialHeapGenerators.NonemptyOnly<'T> () |> Gen.two;
            200, gen{ 
                let! e = SkewBinomialHeapGenerators.EmptyOnly<'T>()
                let! n = SkewBinomialHeapGenerators.EmptyOnly<'T>()
                return (e, n)}
            200, gen{ 
                let! e = SkewBinomialHeapGenerators.EmptyOnly<'T>()
                let! n = SkewBinomialHeapGenerators.EmptyOnly<'T>()
                return (n, e)}]

    static member ComparableAndComparable() = SkewBinomialHeapGenerators.Mixed<IComparable> () |> Arb.fromGen

    static member ComparableAndComparablePair() = SkewBinomialHeapGenerators.TwoMixed<IComparable>() |> Arb.fromGen

    static member ComparableAndObject() = gen {
        let! data = SkewBinomialHeapGenerators.Mixed<IComparable> ()
        return { Heap = data.Heap; Items = data.Items |> Seq.cast<obj> |> Seq.toList; Desc = data.Desc }} |> Arb.fromGen
        
    static member ComparableAndObjectPair() = gen {
        let! data1, data2 = SkewBinomialHeapGenerators.TwoMixed<IComparable> ()
        return { Heap = data1.Heap; Items = data1.Items |> Seq.cast<obj> |> Seq.toList; Desc = data1.Desc },
               { Heap = data2.Heap; Items = data2.Items |> Seq.cast<obj> |> Seq.toList; Desc = data2.Desc }} |> Arb.fromGen

let register = 
    lazy (
        Arb.register<ComparableGenerator<string>>() |> ignore
        Arb.register<SkewBinomialHeapGenerators>() |> ignore)

[<TestFixtureSetUp>]
let setUp () =
    register.Force ()

let fail str = Assert.Fail str

let fsCheck a = fsCheck null a

//************* TESTS *****************

[<Test>]
let ``toSeq returns all the elements`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.toSeq |> Seq.toList |> List.sort = List.sort orig

[<Test>]
let ``toSeq returns the elements in the correct order`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.toSeq |> Seq.toList = sortList heap orig

[<Test>]
let ``toList returns the same as toSeq |> List.ofSeq`` () =
    fsCheck <| fun { Heap = heap } ->
        heap |> SkewBinomialHeap.toList = (heap |> SkewBinomialHeap.toSeq |> List.ofSeq)

[<Test>]
let ``isDescending returns correct value`` () =
    fsCheck <| fun { Heap = heap; Desc = desc } ->
        SkewBinomialHeap.isDescending heap = desc

[<Test>]
let ``isEmpty returns true if count = 0, false otherwise`` () =
    fsCheck <| fun { Heap = heap } ->
        SkewBinomialHeap.count heap = 0 = SkewBinomialHeap.isEmpty heap

[<Test>]
let ``isEmpty returns true if the heap is empty, false otherwise`` () =
    fsCheck <| fun { Heap = heap; Items = orig } ->
        SkewBinomialHeap.isEmpty heap = List.isEmpty orig

[<Test>]
let ``count returns the number of elements`` () =
    fsCheck <| fun { Heap = heap; Items = orig } ->
        heap |> SkewBinomialHeap.count = List.length orig

[<Test>]
let ``length is the same as count`` () =
    fsCheck <| fun { Heap = heap } ->
        heap |> SkewBinomialHeap.count = SkewBinomialHeap.length heap

[<Test>]
let ``head returns the first element when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.isEmpty |> not ==> lazy(heap |> SkewBinomialHeap.head |> should equal <| actualHead heap orig)

[<Test>]
let ``head throws when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } -> 
        heap |> SkewBinomialHeap.isEmpty ==> lazy(should throw typeof<Empty> <| fun () -> heap |> SkewBinomialHeap.head |> ignore)

[<Test>]
let ``tryHead returns Some head when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.isEmpty |> not ==> 
            lazy(
                match SkewBinomialHeap.tryHead heap with 
                | Some head -> head |> should equal <| actualHead heap orig
                | None -> fail "tryHead to a non-empty heap returned None")

[<Test>]
let ``tryHead returns None when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } ->
        heap |> SkewBinomialHeap.isEmpty ==> lazy (heap |> SkewBinomialHeap.tryHead |> Option.isNone)

[<Test>]
let ``tail returns a heap with the first element removed when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } ->
        heap |> SkewBinomialHeap.isEmpty |> not ==> lazy(heap |> SkewBinomialHeap.tail |> SkewBinomialHeap.toList |> should equal <| actualTail heap orig)

[<Test>]
let ``tail throws when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } -> 
        heap |> SkewBinomialHeap.isEmpty ==> lazy(should throw typeof<Empty> <| fun () -> heap |> SkewBinomialHeap.tail |> ignore)

[<Test>]
let ``tryTail returns Some tail when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.isEmpty |> not ==> 
            lazy(
                match SkewBinomialHeap.tryTail heap with 
                | Some tail -> tail |> SkewBinomialHeap.toList |> should equal <| actualTail heap orig
                | None -> fail "tryTail to a non-empty heap returned None")

[<Test>]
let ``tryTail returns None when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } ->
        heap |> SkewBinomialHeap.isEmpty ==> lazy(heap |> SkewBinomialHeap.tryTail |> Option.isNone)

[<Test>]
let ``uncons returns (head, tail) when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.isEmpty |> not ==> 
            lazy(
                let (h, t) = SkewBinomialHeap.uncons heap
                h |> should equal <| actualHead heap orig
                t |> SkewBinomialHeap.toList |> should equal <| actualTail heap orig)

[<Test>]
let ``uncons throws when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } -> 
        heap |> SkewBinomialHeap.isEmpty ==> lazy(should throw typeof<Empty> <| fun () -> heap |> SkewBinomialHeap.uncons |> ignore)

[<Test>]
let ``tryUncons returns Some (head, tail) when the heap is not empty`` () =
    fsCheck <| fun { Heap = heap; Items = orig } -> 
        heap |> SkewBinomialHeap.isEmpty |> not ==> 
            lazy(
                match SkewBinomialHeap.tryUncons heap with 
                | Some (h, t) ->
                    h |> should equal (actualHead heap orig)
                    t |> SkewBinomialHeap.toList |> should equal (actualTail heap orig)
                | None -> fail "tryUncons to a non-empty heap returned None")

[<Test>]
let ``tryUncons returns None when the heap is empty`` () =
    fsCheck <| fun { Heap = heap } ->
        heap |> SkewBinomialHeap.isEmpty ==> lazy(heap |> SkewBinomialHeap.tryUncons |> Option.isNone)
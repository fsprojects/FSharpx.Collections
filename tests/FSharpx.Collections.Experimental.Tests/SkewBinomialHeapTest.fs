namespace FSharpx.Collections.Experimental.Tests

open System

open FsCheck
open Expecto
open Expecto.Flip

open FSharpx.Collections.Experimental
open Properties

module SkewBinomialHeapTest =

    let ofList desc items =
        List.fold (fun h x -> SkewBinomialHeap.insert x h) (SkewBinomialHeap.empty desc) items

    //sorts a list with the same ordering as the heap
    let sortList heap items =
        items
        |> List.sort
        |> if SkewBinomialHeap.isDescending heap then List.rev else id

    let actualHead heap items =
        items
        |> if SkewBinomialHeap.isDescending heap then
               List.max
           else
               List.min

    let actualTail heap =
        sortList heap >> List.tail

    type ComparableGenerator<'T when 'T :> IComparable> =
        static member Generate() =
            gen {
                let! t =
                    Arb.generate<'T>
                    |> Gen.filter(fun x ->
                        match x :> obj with
                        | :? System.IComparable -> true
                        | _ -> false)

                return t :> System.IComparable
            }
            |> Arb.fromGen

    type HeapData<'T, 'L when 'T: comparison> = {
        Heap: 'T SkewBinomialHeap
        Items: 'L list
        Desc: bool
    }

    type SkewBinomialHeapGenerators() =
        static let genDesc d =
            match d with
            | Some v -> Gen.constant v
            | None -> Gen.elements [ true; false ]
        // Empty heaps only, some are empty after removing all of its elements
        static member private EmptyOnly<'T when 'T: comparison>(d) : Gen<HeapData<'T, 'T>> = gen {
            let! desc = genDesc d
            let! s = Gen.listOf(Arb.generate<'T>)

            return {
                Heap =
                    Seq.init (s.Length) id
                    |> Seq.fold (fun heap _ -> heap |> SkewBinomialHeap.tail) (ofList desc s)
                Items = []
                Desc = desc
            }
        }
        // Non-emtpy heaps only, after a sucession of insertions and deletions
        static member private NonemptyOnly<'T when 'T: comparison>(d) : Gen<HeapData<'T, 'T>> = gen {
            let! desc = genDesc d
            let! t = Gen.elements [ true; false ]
            let! s = Gen.nonEmptyListOf(Arb.generate<'T>)

            let! ndel =
                if t then
                    Gen.constant Int32.MaxValue
                else
                    Gen.choose(2, max 2 (s |> List.length))

            let (heap, list, _) =
                s
                |> List.fold
                    (fun (heap, lst, k) item ->
                        let newHeap = heap |> SkewBinomialHeap.insert item
                        let newList = item :: lst

                        if k + 1 = ndel then
                            (newHeap |> SkewBinomialHeap.tail, newList |> List.sort |> (if desc then List.rev else id) |> List.tail, 0)
                        else
                            (newHeap, newList, k + 1))
                    (SkewBinomialHeap.empty desc, [], 0)

            return {
                Heap = heap
                Items = list
                Desc = desc
            }
        }

        static member private Mixed<'T when 'T: comparison>() : Gen<HeapData<'T, 'T>> =
            Gen.frequency [
                200, SkewBinomialHeapGenerators.EmptyOnly<'T>(None)
                800, SkewBinomialHeapGenerators.NonemptyOnly<'T>(None)
            ]

        //Distribute the cases, so the tests that receive two heaps and need both to have the same isDescending don't exhaust the arguments, ex: the merge tests
        static member private TwoMixed<'T when 'T: comparison>() : Gen<HeapData<'T, 'T> * HeapData<'T, 'T>> =
            Gen.frequency [
                500,
                Gen.oneof [
                    SkewBinomialHeapGenerators.EmptyOnly<'T>(None)
                    SkewBinomialHeapGenerators.NonemptyOnly<'T>(None)
                ]
                |> Gen.two
                250,
                Gen.oneof [
                    SkewBinomialHeapGenerators.EmptyOnly<'T>(Some true)
                    SkewBinomialHeapGenerators.NonemptyOnly<'T>(Some true)
                ]
                |> Gen.two
                250,
                Gen.oneof [
                    SkewBinomialHeapGenerators.EmptyOnly<'T>(Some false)
                    SkewBinomialHeapGenerators.NonemptyOnly<'T>(Some false)
                ]
                |> Gen.two
            ]

        static member ComparableAndComparable() =
            SkewBinomialHeapGenerators.Mixed<IComparable>() |> Arb.fromGen

        static member ComparableAndComparablePair() =
            SkewBinomialHeapGenerators.TwoMixed<IComparable>() |> Arb.fromGen

        static member ComparableAndObject() =
            gen {
                let! data = SkewBinomialHeapGenerators.Mixed<IComparable>()

                return {
                    Heap = data.Heap
                    Items = data.Items |> Seq.cast<obj> |> Seq.toList
                    Desc = data.Desc
                }
            }
            |> Arb.fromGen

        static member ComparableAndObjectPair() =
            gen {
                let! data1, data2 = SkewBinomialHeapGenerators.TwoMixed<IComparable>()

                return
                    {
                        Heap = data1.Heap
                        Items = data1.Items |> Seq.cast<obj> |> Seq.toList
                        Desc = data1.Desc
                    },
                    {
                        Heap = data2.Heap
                        Items = data2.Items |> Seq.cast<obj> |> Seq.toList
                        Desc = data2.Desc
                    }
            }
            |> Arb.fromGen

    let heapIntGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listInt n
        let! b' = Gen.listBool 1
        let b = List.head b'
        return ((SkewBinomialHeap.ofSeq b x), x)
    }

    let heapStringGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listString n
        let! b' = Gen.listBool 1
        let b = List.head b'
        return ((SkewBinomialHeap.ofSeq b x), x)
    }

    let heapIntDescGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listInt n
        let! b' = Gen.listBool 1
        let b = List.head b'
        return ((SkewBinomialHeap.ofSeq b x), b)
    }

    let heapStringDescGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listString n
        let! b' = Gen.listBool 1
        let b = List.head b'
        return ((SkewBinomialHeap.ofSeq b x), b)
    }


    //@@@@@@@@@@@@@@@@@@@
    let iComparableGen() : Gen<IComparable> = gen {
        let! t =
            Arb.generate
            |> Gen.filter(fun x ->
                match x :> obj with
                | :? System.IComparable -> true
                | _ -> false)

        return t :> System.IComparable
    }
    //|> Arb.fromGen

    let genDesc d =
        match d with
        | Some v -> Gen.constant v
        | None -> Gen.elements [ true; false ]

    let emptyOnly d = gen {
        let! desc = genDesc d
        let! s = Gen.listOf(Arb.generate<'T>)

        return {
            Heap =
                Seq.init (s.Length) id
                |> Seq.fold (fun heap _ -> heap |> SkewBinomialHeap.tail) (ofList desc s)
            Items = []
            Desc = desc
        }
    }

    let nonEmptyOnly d = gen {
        let! desc = genDesc d
        let! t = Gen.elements [ true; false ]
        let! s = Gen.nonEmptyListOf <| iComparableGen()

        let! ndel =
            if t then
                Gen.constant Int32.MaxValue
            else
                Gen.choose(2, max 2 (s |> List.length))

        let (heap, list, _) =
            s
            |> List.fold
                (fun (heap, lst, k) item ->
                    let newHeap = heap |> SkewBinomialHeap.insert item
                    let newList = item :: lst

                    if k + 1 = ndel then
                        (newHeap |> SkewBinomialHeap.tail, newList |> List.sort |> (if desc then List.rev else id) |> List.tail, 0)
                    else
                        (newHeap, newList, k + 1))
                (SkewBinomialHeap.empty desc, [], 0)

        return {
            Heap = heap
            Items = list
            Desc = desc
        }
    }

    let differentOrdered() =
        gen {
            let! b = Arb.generate<bool>
            let! h1 = nonEmptyOnly(Some b)
            let! h2 = nonEmptyOnly(not b |> Some)
            return h1, h2
        }
        |> Arb.fromGen

    let mixedGen() =
        Gen.frequency [ 200, emptyOnly(None); 800, nonEmptyOnly(None) ]

    let comparableAndComparable() =
        mixedGen() |> Arb.fromGen

    //TODO: Test ofSeq

    //let comparableConfig = {config10k with arbitrary = ComparableGenerator::config10k.arbitrary}

    [<Tests>]
    let testSkewBinomialHeap =

        testList "Experimental SkewBinomialHeap" [
            testPropertyWithConfig
                config10k
                "toSeq returns all the int elements"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) -> heap |> SkewBinomialHeap.toSeq |> Seq.toList |> List.sort = List.sort orig)

            testPropertyWithConfig
                config10k
                "toSeq returns all the string elements"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) -> heap |> SkewBinomialHeap.toSeq |> Seq.toList |> List.sort = List.sort orig)

            testPropertyWithConfig
                config10k
                "toSeq returns the int elements in the correct order"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) -> heap |> SkewBinomialHeap.toSeq |> Seq.toList = sortList heap orig)

            testPropertyWithConfig
                config10k
                "toSeq returns the string elements in the correct order"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) -> heap |> SkewBinomialHeap.toSeq |> Seq.toList = sortList heap orig)

            ptestPropertyWithConfig
                config10k
                "toList returns the same as toSeq |> List.ofSeq"
                (Prop.forAll(comparableAndComparable())
                 <| fun { Heap = heap } -> heap |> SkewBinomialHeap.toList = (heap |> SkewBinomialHeap.toSeq |> List.ofSeq))

            testPropertyWithConfig
                config10k
                "int heap isDescending returns correct value"
                (Prop.forAll(Arb.fromGen heapIntDescGen)
                 <| fun (heap, desc) -> SkewBinomialHeap.isDescending heap = desc)

            testPropertyWithConfig
                config10k
                "string heap isDescending returns correct value"
                (Prop.forAll(Arb.fromGen heapStringDescGen)
                 <| fun (heap, desc) -> SkewBinomialHeap.isDescending heap = desc)

            //testPropertyWithConfig comparableConfig "isEmpty returns true if count = 0, false otherwise"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } -> (SkewBinomialHeap.count heap) = 0 = SkewBinomialHeap.isEmpty heap )

            testPropertyWithConfig
                config10k
                "isEmpty returns true if the int heap is empty, false otherwise"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) -> SkewBinomialHeap.isEmpty heap = List.isEmpty orig)

            testPropertyWithConfig
                config10k
                "isEmpty returns true if the string heap is empty, false otherwise"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) -> SkewBinomialHeap.isEmpty heap = List.isEmpty orig)

            testPropertyWithConfig
                config10k
                "int heap count returns the number of elements"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) -> (heap |> SkewBinomialHeap.count) = List.length orig)

            testPropertyWithConfig
                config10k
                "string heap count returns the number of elements"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) -> (heap |> SkewBinomialHeap.count) = List.length orig)

            //testPropertyWithConfig config10k "length is the same as count"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            (heap |> SkewBinomialHeap.count) = SkewBinomialHeap.length heap )

            testPropertyWithConfig
                config10k
                "int heap head returns the first element when the heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy (heap |> SkewBinomialHeap.head = actualHead heap orig))

            testPropertyWithConfig
                config10k
                "string heap head returns the first element when the heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy (heap |> SkewBinomialHeap.head = actualHead heap orig))

            //testPropertyWithConfig config10k "head throws when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> lazy(Expect.throwsT<InvalidOperationException> "" <| fun () -> heap |> SkewBinomialHeap.head |> ignore) )

            testPropertyWithConfig
                config10k
                "tryHead returns Some head when the int heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> match SkewBinomialHeap.tryHead heap with
                         | Some head -> head = actualHead heap orig
                         | None -> false)

            testPropertyWithConfig
                config10k
                "tryHead returns Some head when the string heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> match SkewBinomialHeap.tryHead heap with
                         | Some head -> head = actualHead heap orig
                         | None -> false)

            //testPropertyWithConfig config10k "tryHead returns None when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> (heap |> SkewBinomialHeap.tryHead |> Option.isNone) )

            testPropertyWithConfig
                config10k
                "tail returns a heap with the first element removed when the int heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy (heap |> SkewBinomialHeap.tail |> SkewBinomialHeap.toList = actualTail heap orig))

            testPropertyWithConfig
                config10k
                "tail returns a heap with the first element removed when the string heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy (heap |> SkewBinomialHeap.tail |> SkewBinomialHeap.toList = actualTail heap orig))

            //testPropertyWithConfig config10k "tail throws when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> lazy(Expect.throwsT<InvalidOperationException> "" <| fun () -> heap |> SkewBinomialHeap.tail |> ignore) )

            testPropertyWithConfig
                config10k
                "tryTail returns Some tail when the int heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (match SkewBinomialHeap.tryTail heap with
                          | Some tail -> tail |> SkewBinomialHeap.toList = actualTail heap orig
                          | None -> false))

            testPropertyWithConfig
                config10k
                "tryTail returns Some tail when the string heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (match SkewBinomialHeap.tryTail heap with
                          | Some tail -> tail |> SkewBinomialHeap.toList = actualTail heap orig
                          | None -> false))

            //testPropertyWithConfig config10k "tryTail returns None when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> (heap |> SkewBinomialHeap.tryTail |> Option.isNone) )

            testPropertyWithConfig
                config10k
                "uncons returns (head, tail) when the int heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (let (h, t) = SkewBinomialHeap.uncons heap

                          h = actualHead heap orig
                          && SkewBinomialHeap.toList t = actualTail heap orig))

            testPropertyWithConfig
                config10k
                "uncons returns (head, tail) when the string heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (let (h, t) = SkewBinomialHeap.uncons heap

                          h = actualHead heap orig
                          && SkewBinomialHeap.toList t = actualTail heap orig))

            //testPropertyWithConfig config10k "uncons throws when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> lazy(Expect.throwsT<InvalidOperationException> "" <| fun () -> heap |> SkewBinomialHeap.uncons |> ignore) )

            testPropertyWithConfig
                config10k
                "tryUncons returns Some (head, tail) when the int heap is not empty"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (match SkewBinomialHeap.tryUncons heap with
                          | Some(h, t) ->
                              h = actualHead heap orig
                              && SkewBinomialHeap.toList t = actualTail heap orig
                          | None -> false))

            testPropertyWithConfig
                config10k
                "tryUncons returns Some (head, tail) when the string heap is not empty"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) ->
                     heap |> SkewBinomialHeap.isEmpty |> not
                     ==> lazy
                         (match SkewBinomialHeap.tryUncons heap with
                          | Some(h, t) ->
                              h = actualHead heap orig
                              && SkewBinomialHeap.toList t = actualTail heap orig
                          | None -> false))

            //testPropertyWithConfig config10k "tryUncons returns None when the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==> lazy(heap |> SkewBinomialHeap.tryUncons |> Option.isNone) )

            testPropertyWithConfig
                config10k
                "int heap insert always insert"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap, orig) x ->
                     heap
                     |> SkewBinomialHeap.insert x
                     |> SkewBinomialHeap.toList
                     |> Expect.equal "" (x :: orig |> sortList heap))

            testPropertyWithConfig
                config10k
                "string heap insert always insert"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap, orig) x ->
                     heap
                     |> SkewBinomialHeap.insert x
                     |> SkewBinomialHeap.toList
                     |> Expect.equal "" (x :: orig |> sortList heap))

            //testPropertyWithConfig config10k "merge should merge if both heaps have the same ordering" <|
            //    fun ({ Heap = heap1; Items = orig1 }, { Heap = heap2; Items = orig2 }) ->
            //        heap1.IsDescending = heap2.IsDescending ==>
            //        lazy(
            //            SkewBinomialHeap.merge heap1 heap2
            //            |> SkewBinomialHeap.toList
            //            |> Expect.equal "" (orig1 |> List.append orig2 |> sortList heap1))

            ptestPropertyWithConfig
                config10k
                "merge throws when both heaps have diferent ordering"
                (Prop.forAll(differentOrdered())
                 <| fun ({ Heap = heap1 }, { Heap = heap2 }) ->
                     heap1.IsDescending <> heap2.IsDescending
                     ==> lazy
                         (Expect.throwsT<IncompatibleMerge> ""
                          <| fun () -> SkewBinomialHeap.merge heap1 heap2 |> ignore))

            //testPropertyWithConfig config10k "tryMerge returns Some merged heap when both heaps have the same ordering" <|
            //    fun ({ Heap = heap1; Items = orig1 }, { Heap = heap2; Items = orig2 }) ->
            //        heap1.IsDescending = heap2.IsDescending ==>
            //        lazy(
            //            match SkewBinomialHeap.tryMerge heap1 heap2 |> Option.map SkewBinomialHeap.toList with
            //            | Some list -> list = (List.append orig1 orig2 |> sortList heap1)
            //            | None -> false)

            //testPropertyWithConfig config10k "tryMerge returns None when both heaps have diferent ordering" <|
            //    fun ({ Heap = heap1; Items = orig1 }, { Heap = heap2; Items = orig2 }) ->
            //        heap1.IsDescending <> heap2.IsDescending ==> lazy(SkewBinomialHeap.tryMerge heap1 heap2 |> Option.isNone)

            //testPropertyWithConfig config10k "Cons pattern always match if the heap is not empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty |> not ==>
            //            match heap with
            //            | SkewBinomialHeap.Cons (_, _) -> true
            //            | _ -> false )

            //testPropertyWithConfig config10k "Nil pattern always match if the heap is empty"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty ==>
            //            match heap with
            //            | SkewBinomialHeap.Cons (_, _) -> false
            //            | _ -> true )

            //testPropertyWithConfig config10k "Cons pattern return the same as uncons"
            //    (Prop.forAll (SkewBinomialHeapGenerators.ComparableAndComparable()) <|
            //        fun { Heap = heap } ->
            //            heap |> SkewBinomialHeap.isEmpty |> not ==>
            //            match heap with
            //            | SkewBinomialHeap.Cons (h, t) ->
            //                let (h', t') = heap |> SkewBinomialHeap.uncons
            //                h = h' && t = t'
            //            | _ -> false )

            //testPropertyWithConfig config10k "GetHashCode is the same for equal heaps" <|
            //    fun { Heap = heap } item ->
            //        let heap1 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        let heap2 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        Unchecked.hash heap1 = Unchecked.hash heap2

            ////Maybe the distribution of the hash should be checked
            ////to avoid bad hashes, I don't know if that should be done as part of unit testPropertyWithConfig config10king

            ptestPropertyWithConfig
                config10k
                "Equality reflexivity"
                (Prop.forAll(comparableAndComparable())
                 <| fun { Heap = heap } -> heap = heap)

            //testPropertyWithConfig config10k "Equality symmetry" <|
            //    fun { Heap = heap } item ->
            //        let heap1 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        let heap2 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        heap1 = heap2 ==> (heap2 = heap1)

            //testPropertyWithConfig config10k "Equality transitivity" <| //maybe this is too much, I guess It would be hard to write an Equals that violates this property and not the others
            //    fun { Heap = heap } item ->
            //        let heap1 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        let heap2 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        let heap3 = heap |> SkewBinomialHeap.insert item |> SkewBinomialHeap.tail
            //        (heap1 = heap2 && heap2 = heap3) ==> (heap1 = heap3)

            //testPropertyWithConfig config10k "Equals returns false when comparing two heaps with the same ordering but different items" <|
            //    fun ({ Heap = heap1; Items = orig1}, { Heap = heap2; Items = orig2}) ->
            //        (heap1.IsDescending = heap2.IsDescending && orig1 <> orig2) ==> (heap1 <> heap2 && not (heap1.Equals heap2))

            testPropertyWithConfig
                config10k
                "Equals returns false when comparing two int heaps with the same items but different ordering"
                (Prop.forAll(Arb.fromGen heapIntGen)
                 <| fun (heap1, orig) ->
                     let heap2 = ofList (not heap1.IsDescending) orig
                     heap1 <> heap2 && not(heap1.Equals heap2))

            testPropertyWithConfig
                config10k
                "Equals returns false when comparing two string heaps with the same items but different ordering"
                (Prop.forAll(Arb.fromGen heapStringGen)
                 <| fun (heap1, orig) ->
                     let heap2 = ofList (not heap1.IsDescending) orig
                     heap1 <> heap2 && not(heap1.Equals heap2))
        ]

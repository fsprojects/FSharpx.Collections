namespace FSharpx.Collections.Tests

open FSharpx
open FSharpx.Collections
open FSharpx.Collections.Heap
open FSharpx.Collections.Tests.Properties
open FsCheck
open Expecto
open Expecto.Flip

module HeapTests =

    [<Tests>]
    let testHeap =
        testList "Heap" [

            test "cons pattern discriminator" {
                let h = ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h1, t1 = uncons h 

                let h2, t2 = 
                    match t1 with
                    | Cons(h, t) -> h, t
                    | _ ->  "x", t1

                Expect.isTrue "cons pattern" ((h2 = "e") && ((length t2) = 4)) }

            test "cons pattern discriminator 2" {
                let h = ofSeq true ["f";"e";"d";"c";"b";"a"]

                let t2 = 
                    match h with
                    | Cons("f", Cons(_, t)) -> t
                    | _ ->  h

                let h1, t3 = uncons t2 

                Expect.isTrue "cons pattern" ((h1 = "d") && ((length t2) = 4)) }

            test "empty list should be empty" { 
                Expect.isTrue "empty" (Heap.empty true).IsEmpty }

            test "rev empty" {
                let h = empty true
                Expect.isTrue "" (h |> rev |> isEmpty)
                let h' = empty false
                Expect.isTrue "" (h' |> rev |> isEmpty) }

            test "insert works" {
                Expect.isFalse "" (((Heap.empty true).Insert 1).Insert 2).IsEmpty }

            test "length of empty is 0" {
                Expect.equal "empty" 0 (Heap.empty true).Length }

            test "tryHead on empty should return None" {
                Expect.isNone "tryHead" (Heap.empty true).TryHead }

            test "tryTail on empty should return None" {
                Expect.isNone "tryTail" <| (Heap.empty true).TryTail() }

            test "tryTail on len 1 should return Some empty" {
                let h = Heap.empty true |> insert 1 |> tryTail
                Expect.isTrue "tryTail" (h.Value |> isEmpty) }

            test "tryMerge max and min should be None" {
                let h1 = ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h2 = ofSeq false ["t";"u";"v";"w";"x";"y";"z"]

                Expect.isNone "tryMerge" <| tryMerge h1 h2 }

            test "structural equality" {
                let l1 = ofSeq true [1..100]
                let l2 = ofSeq true [1..100]

                Expect.equal "structural equality" l1 l2

                let l3 = ofSeq true [1..99] |> insert 7

                Expect.notEqual "structural equality" l1 l3 }

            test "toSeq to list" {
                let l = ["f";"e";"d";"c";"b";"a"] 
                let h = ofSeq true l

                Expect.equal "toSeq to list" l  (h |> toSeq |> List.ofSeq) }

            test "tryUncons empty" {
                Expect.isNone "TryUncons" <| (Heap.empty true).TryUncons() }

            test "Tail of large heap does not result in stackoverflow" {
                let rnd = new System.Random()
                let h = 
                    [1..1000000] 
                    |> Seq.sortBy (fun x -> rnd.Next())
                    |> Heap.ofSeq false
    
                Heap.tail h |> ignore 
                Expect.isTrue "" true }
        ]

    //[<Tests>]
    //let propertyTestHeap =
    //    //only going up to 5 elements is probably sufficient to test all edge cases

    //    let insertThruList l h =
    //        List.fold (fun (h' : Heap<'a>) x -> h'.Insert  x  ) h l

    //    let maxHeapIntGen =
    //        gen {   let! n = Gen.length2thru12
    //                let! n2 = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                let! y =  Gen.listInt n2
    //                return ( (Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev) ) }

    //    let maxHeapIntOfSeqGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                return ( (Heap.ofSeq true x), (x |> List.sort |> List.rev) ) }

    //    let maxHeapIntInsertGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                return ( (Heap.empty true |> insertThruList x), (x |> List.sort |> List.rev) ) }

    //    let maxHeapStringGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! n2 = Gen.length2thru12
    //                let! x =  Gen.listString n
    //                let! y =  Gen.listString n2
    //                return ( (Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev) ) }

    //    let minHeapIntGen =
    //        gen {   let! n = Gen.length2thru12
    //                let! n2 = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                let! y =  Gen.listInt n2
    //                return ( (Heap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort) ) }

    //    let minHeapIntOfSeqGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                return ( (Heap.ofSeq false x), (x |> List.sort) ) }

    //    let minHeapIntInsertGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x =  Gen.listInt n
    //                return ( (Heap.empty false |> insertThruList x), (x |> List.sort) ) }

    //    let minHeapStringGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! n2 = Gen.length2thru12
    //                let! x =  Gen.listString n
    //                let! y =  Gen.listString n2
    //                return ( (Heap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort) ) }

    //    // HACK: from when using NUnit TestCaseSource does not understand array of tuples at runtime
    //    let intGens start =
    //        let v = Array.create 6 (box (maxHeapIntGen, "max Heap int"))
    //        v.[1] <- box ((maxHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "max Heap OfSeq")
    //        v.[2] <- box ((maxHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "max Heap from Insert")
    //        v.[3] <- box (minHeapIntGen , "min Heap int")
    //        v.[4] <- box ((minHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "min Heap OfSeq")
    //        v.[5] <- box ((minHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "min Heap from Insert")
    //        v

    //    let stringGens =
    //        let v = Array.create 2 (box (maxHeapStringGen, "max Heap string"))
    //        v.[1] <- box (minHeapStringGen, "min Heap string")
    //        v

    //    let intGensStart1 =
    //        intGens 1  //this will accept all

    //    let intGensStart2 =
    //        intGens 2 // this will accept 11 out of 12

    //    testList "Heap property tests" [

    //        testPropertyWithConfig config10k "head should return" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((h : Heap<int>), (l : int list)) -> h.Head = l.Head )

    //        testPropertyWithConfig config10k "maxHeap seq enumerate matches build list" (Prop.forAll (Arb.fromGen maxHeapIntGen) <|
    //            fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length )

    //        testPropertyWithConfig config10k "minHeap seq enumerate matches build list" (Prop.forAll (Arb.fromGen minHeapIntGen) <|
    //            fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length )

    //        testPropertyWithConfig config10k "rev works max heap" (Prop.forAll (Arb.fromGen maxHeapIntGen) <|
    //            fun (h, l) -> h |> rev |> List.ofSeq = (h |> List.ofSeq |> List.rev) )

    //        testPropertyWithConfig config10k "rev works min  heap" (Prop.forAll (Arb.fromGen minHeapIntGen) <|
    //            fun (h, l) -> h |> rev |> List.ofSeq = (h |> List.ofSeq |> List.rev) )

    //        testPropertyWithConfig config10k "seq enumerate matches build list int" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
    //            fun (h : Heap<int>, l) -> h |> Seq.toList = l )

    //        testPropertyWithConfig config10k "seq enumerate matches build list string" (Prop.forAll (Arb.fromGen (fst <| unbox stringGens)) <|
    //            fun (h : Heap<string>, l) -> h |> Seq.toList = l )

    //        testPropertyWithConfig config10k "tail should return" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((h : Heap<int>), (l : int list)) ->    
    //                let tl = h.Tail()
    //                let tlHead =
    //                    if (tl.Length > 0) then tl.Head = l.Item(1)
    //                    else true
    //                tlHead && (tl.Length = l.Length - 1) )

    //        testPropertyWithConfig config10k "tryHead should return" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((h : Heap<int>), (l : int list)) -> h.TryHead.Value = l.Head )

    //        testPropertyWithConfig config10k "tryUncons 1 element" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((h : Heap<int>), (l : int list)) ->    
    //                let x, tl = h.TryUncons().Value
    //                x = l.Head && tl.Length = (l.Length - 1) )

    //        testPropertyWithConfig config10k "uncons 1 element" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((h : Heap<int>), (l : int list)) ->    
    //                let x, tl = h.Uncons()
    //                x = l.Head && tl.Length = (l.Length - 1) )

    //        //type HeapGen =
    //        //    static member Heap{
    //        //        let rec heapGen{ 
    //        //            gen {
    //        //                let! n = Gen.length1thru100
    //        //                let! xs =  Gen.listInt n
    //        //                return Heap.ofSeq true xs
    //        //            }
    //        //        Arb.fromGen (heapGen())

    //        //let registerGen = lazy (Arb.register<HeapGen>() |> ignore)

    ////fsCheck having trouble with Heap
    ////FSharpx.Tests.HeapTest.monoid law:
    ////System.Exception : Geneflect: type not handled FSharpx.Collections.Heap`1[System.IComparable]
    ////[<Test>]
    ////testPropertyWithConfig config10k "monoid law" {
    ////    registerGen.Force()
    ////    checkMonoid "Heap" (Heap.monoid)
    //    ]
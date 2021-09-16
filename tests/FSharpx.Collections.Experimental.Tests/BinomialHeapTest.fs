namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip
open HeapGen

//only going up to 5 elements is probably sufficient to test all edge cases

module BinomialHeapTest =

    [<Tests>]
    let testBinomialHeap =

        testList "Experimental BinomialHeap" [

            test "cons pattern discriminator" {
                let h = BinomialHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h1, t1 = BinomialHeap.uncons h 

                let h2, t2 = 
                    match t1 with
                    | BinomialHeap.Cons(h, t) -> h, t
                    | _ ->  "x", t1

                ((h2 = "e") && ((BinomialHeap.length t2) = 4)) |> Expect.isTrue "" }

            test "cons pattern discriminator 2" {
                let h = BinomialHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]

                let t2 = 
                    match h with
                    | BinomialHeap.Cons("f", BinomialHeap.Cons(_, t)) -> t
                    | _ ->  h

                let h1, t3 = BinomialHeap.uncons t2 

                ((h1 = "d") && ((BinomialHeap.length t2) = 4)) |> Expect.isTrue "" }

            test "empty list should be empty" { 
                (BinomialHeap.empty true).IsEmpty |> Expect.isTrue "" }

            test "BinomialHeap.length of empty is 0" {
                (BinomialHeap.empty true).Length() |> Expect.equal "" 0 }

            test "tryGetHead on empty should return None" {
                (BinomialHeap.empty true).TryGetHead() |> Expect.isNone "" }

            test "BinomialHeap.tryGetTail on empty should return None" {
                (BinomialHeap.empty true).TryGetTail() |> Expect.isNone "" }

            test "BinomialHeap.tryGetTail on len 1 should return Some empty" {
                let h = BinomialHeap.empty true |> BinomialHeap.insert 1 |> BinomialHeap.tryGetTail
                h.Value |> BinomialHeap.isEmpty |> Expect.isTrue "" }

            test "BinomialHeap.tryMerge max and mis should be None" {
                let h1 = BinomialHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h2 = BinomialHeap.ofSeq false ["t";"u";"v";"w";"x";"y";"z"]

                BinomialHeap.tryMerge h1 h2 |> Expect.isNone "" }

            test "BinomialHeap.insert works" {
                (((BinomialHeap.empty true).Insert 1).Insert 2).IsEmpty |> Expect.isFalse "" }

            test "tryUncons empty" {
                (BinomialHeap.empty true).TryUncons() |> Expect.isNone "" }
        ]

    [<Tests>]
    let propertyBinomialHeap =

        let intGens start =
            let v = Array.create 6 maxBinomialHeapIntGen
            v.[1] <- maxBinomialHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
            v.[2] <- maxBinomialHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
            v.[3] <- minBinomialHeapIntGen
            v.[4] <- minBinomialHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
            v.[5] <- minBinomialHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
            v

        let stringGens =
            let v = Array.create 2 maxBinomialHeapStringGen
            v.[1] <- minBinomialHeapStringGen
            v

        let intGensStart1 =
            intGens 1  //this will accept all

        let intGensStart2 =
            intGens 2 // this will accept 11 out of 12

        testList "Experimental BinomialHeap property tests" [

            testPropertyWithConfig config10k "head should return 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "head should return 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "head should return 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "head should return 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "head should return 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "head should return 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list max" (Prop.forAll (Arb.fromGen maxBinomialHeapIntGen) <|
                fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list min" (Prop.forAll (Arb.fromGen minBinomialHeapIntGen) <|
                fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 3" (Prop.forAll (Arb.fromGen intGensStart1.[3]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 4" (Prop.forAll (Arb.fromGen intGensStart1.[4]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list int 5" (Prop.forAll (Arb.fromGen intGensStart1.[5]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "seq enumerate matches build list string 0" (Prop.forAll (Arb.fromGen stringGens.[0]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "seq enumerate matches build list string 1" (Prop.forAll (Arb.fromGen stringGens.[1]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tail should return 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "tryGetHead should return 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryGetHead should return 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryGetHead should return 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryGetHead should return 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryGetHead should return 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryGetHead should return 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "tryUncons 1 element 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
               fun (h, l) ->    let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "BinomialHeap.uncons 1 element 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))
        ]
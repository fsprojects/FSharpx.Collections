namespace FSharpx.Collections.Experimental.Tests

open FSharpx
open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip
open HeapGen

//only going up to 5 elements is probably sufficient to test all edge cases

(*
Could not get IHeap<'c, 'a when 'c :> IHeap<'c, 'a> and 'a : comparison> interface working smoothly between shared code,
NUnit TestCaseSource(), FsCheck, and trying to pass around the tuple of heap generator and list. So need individual test
file for each heap type, unlike IQueue.

Even restricting only to this type, never got generic element type 'a to work. Need separate tests for int and string.
*)

// NUnit TestCaseSource does not understand array of tuples at runtime

module PairingHeapTest =

    let intGens start =
        let v = Array.create 6 maxPairingHeapIntGen
        v.[1] <- maxPairingHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[2] <- maxPairingHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[3] <- minPairingHeapIntGen
        v.[4] <- minPairingHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[5] <- minPairingHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)
        v

    let stringGens =
        let v = Array.create 2 maxPairingHeapStringGen
        v.[1] <- minPairingHeapStringGen
        v

    let intGensStart1 =
        intGens 1  //this will accept all

    let intGensStart2 =
        intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testPairingHeap =

        testList "Experimental PairingHeap" [
            test "cons pattern discriminator" {
                let h = PairingHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h1, t1 = PairingHeap.uncons h 

                let h2, t2 = 
                    match t1 with
                    | PairingHeap.Cons(h, t) -> h, t
                    | _ ->  "x", t1

                ((h2 = "e") && ((PairingHeap.length t2) = 4)) |> Expect.isTrue "" }

            test "cons pattern discriminator 2" {
                let h = PairingHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]

                let t2 = 
                    match h with
                    | PairingHeap.Cons("f", PairingHeap.Cons(_, t)) -> t
                    | _ ->  h

                let h1, t3 = PairingHeap.uncons t2 

                ((h1 = "d") && ((PairingHeap.length t2) = 4)) |> Expect.isTrue "" }

            test "empty list should be empty" { 
                (PairingHeap.empty true).IsEmpty |> Expect.isTrue "" }

            test "insert works" {
                (((PairingHeap.empty true).Insert 1).Insert 2).IsEmpty |> Expect.isFalse "" }

            test "length of empty is 0" {
                (PairingHeap.empty true).Length() |> Expect.equal "" 0 }

            test "tryGetHead on empty should return None" {
                (PairingHeap.empty true).TryGetHead() |> Expect.isNone "" }

            test "tryGetTail on empty should return None" {
                (PairingHeap.empty true).TryGetTail() |> Expect.isNone "" }

            test "tryGetTail on len 1 should return Some empty" {
                let h = PairingHeap.empty true |> PairingHeap.insert 1 |> PairingHeap.tryGetTail
                h.Value |> PairingHeap.isEmpty |> Expect.isTrue "" }

            test "tryMerge max and mis should be None" {
                let h1 = PairingHeap.ofSeq true ["f";"e";"d";"c";"b";"a"]
                let h2 = PairingHeap.ofSeq false ["t";"u";"v";"w";"x";"y";"z"]

                PairingHeap.tryMerge h1 h2 |> Expect.isNone "" }

            test "tryUncons empty" {
                (PairingHeap.empty true).TryUncons() |> Expect.isNone "" }
        ]

    [<Tests>]
    let testPairingHeapProperties =

        testList "Experimental PairingHeap properties" [

            testPropertyWithConfig config10k "max PairingHeap int head should return" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap OfSeq head should return" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap from Insert head should return" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap int head should return" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap OfSeq head should return" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap from Insert head should return" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) -> (h.Head() = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "maxPairingHeap seq enumerate matches build list" (Prop.forAll (Arb.fromGen maxPairingHeapIntGen) <|
                fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "minPairingHeap seq enumerate matches build list" (Prop.forAll (Arb.fromGen minPairingHeapIntGen) <|
                fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap int seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "max PairingHeap OfSeq seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "max PairingHeap from Insert seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "min PairingHeap int seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[3]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "min PairingHeap OfSeq seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[4]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "min PairingHeap from Insert seq enumerate matches build list int" (Prop.forAll (Arb.fromGen intGensStart1.[5]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "max PairingHeap string seq enumerate matches build list string" (Prop.forAll (Arb.fromGen stringGens.[0]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "min PairingHeap string seq enumerate matches build list string" (Prop.forAll (Arb.fromGen stringGens.[1]) <|
                fun (h, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length()) )

            testPropertyWithConfig config10k "max PairingHeap int tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap OfSeq tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap from Insert tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap int tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap OfSeq tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap from Insert tail should return" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) ->   let tl = h.Tail()
                                let tlHead =
                                    if (tl.Length() > 0) then (tl.Head() = l.Item(1))
                                    else true
                                (tlHead && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap int tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap OfSeq tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap from Insert tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap int tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap OfSeq tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap from Insert tryGetHead should return" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) -> (h.TryGetHead().Value = l.Head) |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap int tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap OfSeq tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap from Insert tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap int tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap OfSeq tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap from Insert tryUncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (h, l) ->   let x, tl = h.TryUncons().Value
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap int uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap OfSeq uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "max PairingHeap from Insert uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap int uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap OfSeq uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))

            testPropertyWithConfig config10k "min PairingHeap from Insert uncons 1 element" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (h, l) ->   let x, tl = h.Uncons()
                                ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
                                |> classifyCollect h (h.Length()))
        ]
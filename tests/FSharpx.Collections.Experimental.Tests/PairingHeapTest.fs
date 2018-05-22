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
        let v = Array.create 6 (box (maxPairingHeapIntGen, "max PairingHeap int"))
        v.[1] <- box ((maxPairingHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "max PairingHeap OfSeq")
        v.[2] <- box ((maxPairingHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "max PairingHeap from Insert")
        v.[3] <- box (minPairingHeapIntGen , "min PairingHeap int")
        v.[4] <- box ((minPairingHeapIntOfSeqGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "min PairingHeap OfSeq")
        v.[5] <- box ((minPairingHeapIntInsertGen  |> Gen.filter (fun (q, l) -> l.Length >= start)), "min PairingHeap from Insert")
        v

    let stringGens =
        let v = Array.create 2 (box (maxPairingHeapStringGen, "max PairingHeap string"))
        v.[1] <- box (minPairingHeapStringGen, "min PairingHeap string")
        v

    let intGensStart1 =
        intGens 1  //this will accept all

    let intGensStart2 =
        intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testRealTimeDeque =

        testList "Experimental RealTimeDeque" [
            //test "cons pattern discriminator" {
            //    let h = ofSeq true ["f";"e";"d";"c";"b";"a"]
            //    let h1, t1 = uncons h 

            //    let h2, t2 = 
            //        match t1 with
            //        | Cons(h, t) -> h, t
            //        | _ ->  "x", t1

            //    ((h2 = "e") && ((length t2) = 4)) |> Expect.isTrue "" }

            //test "cons pattern discriminator 2" {
            //    let h = ofSeq true ["f";"e";"d";"c";"b";"a"]

            //    let t2 = 
            //        match h with
            //        | Cons("f", Cons(_, t)) -> t
            //        | _ ->  h

            //    let h1, t3 = uncons t2 

            //    ((h1 = "d") && ((length t2) = 4)) |> Expect.isTrue "" }

            //test "empty list should be empty" { 
            //    (PairingHeap.empty true).IsEmpty |> Expect.isTrue "" }

            //[<TestCaseSource("intGensStart2")>]
            //test "head should return``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((h : PairingHeap<int>), (l : int list)) ->    
            //                                                                            (h.Head() = l.Head)     
            //                                                                            |> classifyCollect h (h.Length())))
            //test "insert works" {
            //    (((PairingHeap.empty true).Insert 1).Insert 2).IsEmpty |> Expect.isFalse "" }

            //test "seq enumerate matches build list" {

            //    fsCheck "maxPairingHeap" (Prop.forAll (Arb.fromGen maxPairingHeapIntGen) 
            //        (fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length())))

            //    fsCheck "minPairingHeap" (Prop.forAll (Arb.fromGen minPairingHeapIntGen) 
            //        (fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h (h.Length())))

            //test "length of empty is 0" {
            //    (PairingHeap.empty true).Length() |> Expect.equal "" } 0

            //[<TestCaseSource("intGensStart1")>]
            //test "seq enumerate matches build list int``(x : obj) =
            //    let genAndName = unbox x
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (h : PairingHeap<int>, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length())))

            //[<TestCaseSource("stringGens")>]
            //test "seq enumerate matches build list string``(x : obj) =
            //    let genAndName = unbox x
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (h : PairingHeap<string>, l) -> h |> Seq.toList = l |> classifyCollect h (h.Length())))

            //[<TestCaseSource("intGensStart2")>]
            //test "tail should return``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((h : PairingHeap<int>), (l : int list)) ->    
            //                                                                            let tl = h.Tail()
            //                                                                            let tlHead =
            //                                                                                if (tl.Length() > 0) then (tl.Head() = l.Item(1))
            //                                                                                else true
            //                                                                            (tlHead && (tl.Length() = (l.Length - 1)))     
            //                                                                            |> classifyCollect h (h.Length())))

            //test "tryGetHead on empty should return None" {
            //    (PairingHeap.empty true).TryGetHead() |> Expect.isNone "" }

            //[<TestCaseSource("intGensStart2")>]
            //test "tryGetHead should return``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((h : PairingHeap<int>), (l : int list)) ->    
            //                                                                            (h.TryGetHead().Value = l.Head)     
            //                                                                            |> classifyCollect h (h.Length())))

            //test "tryGetTail on empty should return None" {
            //    (PairingHeap.empty true).TryGetTail() |> Expect.isNone "" }

            //test "tryGetTail on len 1 should return Some empty" {
            //    let h = PairingHeap.empty true |> insert 1 |> tryGetTail
            //    h.Value |> isEmpty |> Expect.isTrue "" }

            //test "tryMerge max and mis should be None" {
            //    let h1 = ofSeq true ["f";"e";"d";"c";"b";"a"]
            //    let h2 = ofSeq false ["t";"u";"v";"w";"x";"y";"z"]

            //    tryMerge h1 h2 |> Expect.isNone "" }

            //[<TestCaseSource("intGensStart2")>]
            //test "tryUncons 1 element``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((h : PairingHeap<int>), (l : int list)) ->    
            //                                                                            let x, tl = h.TryUncons().Value
            //                                                                            ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
            //                                                                            |> classifyCollect h (h.Length())))

            //test "tryUncons empty" {
            //    (PairingHeap.empty true).TryUncons() |> Expect.isNone "" }

            //[<TestCaseSource("intGensStart2")>]
            //test "uncons 1 element``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((h : PairingHeap<int>), (l : int list)) ->    
            //                                                                            let x, tl = h.Uncons()
            //                                                                            ((x = l.Head) && (tl.Length() = (l.Length - 1)))     
            //                                                                            |> classifyCollect h (h.Length())))
        ]
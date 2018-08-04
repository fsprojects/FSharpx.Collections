namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip

module IQueueTest =

    let emptyIQueues =
        let v = Array.create 4 (BankersQueue.empty() :> IQueue<obj>)
        v.[1] <- (BatchedQueue.empty() :> IQueue<obj>)
        v.[2] <- (PhysicistQueue.empty() :> IQueue<obj>)
        v.[3] <- (HoodMelvilleQueue.empty() :> IQueue<obj>)
        v

    // NUnit TestCaseSource does not understand array of tuples at runtime
    let intGens start =
        let v = Array.create 12 QueueGen.bankersQueueIntGen
        v.[1] <- QueueGen.bankersQueueIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[2] <- QueueGen.bankersQueueIntSnocGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[3] <- QueueGen.batchedQueueIntGen
        v.[4] <- QueueGen.batchedQueueIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[5] <- QueueGen.batchedQueueIntSnocGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[6] <- QueueGen.hoodMelvilleQueueIntGen
        v.[7] <- QueueGen.hoodMelvilleQueueIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[8] <- QueueGen.hoodMelvilleQueueIntSnocGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[9] <- QueueGen.physicistQueueIntGen
        v.[10] <- QueueGen.physicistQueueIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v.[11] <- QueueGen.physicistQueueIntSnocGen |> Gen.filter (fun (q, l) -> l.Length >= start)
        v

    let objGens =
        let v = Array.create 4 QueueGen.bankersQueueObjGen
        v.[1] <- QueueGen.batchedQueueObjGen
        v.[2] <- QueueGen.hoodMelvilleQueueObjGen
        v.[3] <- QueueGen.physicistQueueObjGen
        v

    let stringGens =
        let v = Array.create 4 QueueGen.bankersQueueStringGen
        v.[1] <- QueueGen.batchedQueueStringGen
        v.[2] <- QueueGen.hoodMelvilleQueueStringGen
        v.[3] <- QueueGen.physicistQueueStringGen
        v

    let intGensStart1 =
        intGens 1  //this will accept all

    let intGensStart2 =
        intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testIQueue =

        testList "Experimental IQueue" [

            test "allow to dequeue" { 
                emptyIQueues
                |> Array.iter (fun eIQ  -> ((eIQ.Snoc 1).Tail).IsEmpty |> Expect.isTrue "") }

            test "allow to enqueue" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> ((eIQ.Snoc 1).Snoc 2).IsEmpty |> Expect.isFalse "") }

            test "cons pattern discriminator - BankersQueue" {
                let q = BankersQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | BankersQueue.Cons(h, t) -> h, t
                    | _ ->  "x", q

                ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            test "cons pattern discriminator - BatchedQueue" {
                let q = BatchedQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | BatchedQueue.Cons(h, t) -> h, t
                    | _ ->  "x", q

                ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            test "cons pattern discriminator - HoodMelvilleQueue" {
                let q = HoodMelvilleQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | HoodMelvilleQueue.Cons(h, t) -> h, t
                    | _ ->  "x", q

                ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            test "cons pattern discriminator - PhysicistQueue" {
                let q = PhysicistQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | PhysicistQueue.Cons(h, t) -> h, t
                    | _ ->  "x", q

                ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            test "empty queue should be empty" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> eIQ.IsEmpty |> Expect.isTrue "" ) }

            test "fail if there is no head in the queue" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> 
                    let ok = ref false
                    try
                        eIQ.Head |> ignore
                    with x when x = Exceptions.Empty -> ok := true
                    !ok |> Expect.isTrue "" ) }

            test "fail if there is no tail in the queue" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> 
                    let ok = ref false
                    try
                        eIQ.Tail |> ignore
                    with x when x = Exceptions.Empty -> ok := true
                    !ok |> Expect.isTrue "" ) }

            test "give None if there is no head in the queue" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> eIQ.TryGetHead |> Expect.isNone "" ) }

            test "give None if there is no tail in the queue" {
                emptyIQueues
                |> Array.iter (fun eIQ  -> eIQ.TryGetTail |> Expect.isNone "" ) }

            test "TryUncons wind-down to None" {
                let qBn = BankersQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
                let qBt = BatchedQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
                let qH = HoodMelvilleQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
                let qP = PhysicistQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>

                let rec loop (iq : IQueue<string>) = 
                    match (iq.TryUncons) with
                    | Some(hd, tl) ->  loop tl
                    | None -> ()

                loop qBn
                loop qBt
                loop qH
                loop qP

                true |> Expect.isTrue "" }
        ]

    [<Tests>]
    let testIQueueProperties =

        let rec nth l i =
            match i with
            | 0 -> List.head l
            | _ -> nth (List.tail l) (i-1)

        testList "Experimental IQueue properties" [

            testPropertyWithConfig config10k "fold matches build list rev" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntGen) <|
                fun (q, l) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "fold matches build list rev OfSeq" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntOfSeqGen) <|
                fun (q, l) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "fold matches build list rev Snoc" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntSnocGen) <|
                fun (q, l) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "fold matches build list rev HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntGen) <|
                fun (q, l) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "fold matches build list rev HoodMelvilleQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntOfSeqGen) <|
                fun (q, l) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "fold matches build list rev HoodMelvilleQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntSnocGen) <|
                fun (q, l) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "fold matches build list rev PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) <|
                fun (q, l) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "fold matches build list rev PhysicistQueue OfSeq" (Prop.forAll (Arb.fromGen (QueueGen.physicistQueueIntOfSeqGen)) <|
                fun (q, l) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "fold matches build list rev PhysicistQueue Snoc" (Prop.forAll (Arb.fromGen (QueueGen.physicistQueueIntSnocGen)) <| 
                fun (q, l) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntGen) <|
                fun (q, l) -> BatchedQueue.foldBack (fun elem l'  -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "foldback matches build list BatchedQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntOfSeqGen) <|
                fun (q, l) -> BatchedQueue.foldBack (fun elem l' -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list BatchedQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntSnocGen) <|
                fun (q, l) -> BatchedQueue.foldBack (fun elem l' -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntGen) <|
                fun (q, l) -> HoodMelvilleQueue.foldBack (fun elem l'  -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "foldback matches build list HoodMelvilleQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntOfSeqGen) <|
                fun (q, l) -> HoodMelvilleQueue.foldBack (fun elem l' -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list HoodMelvilleQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntSnocGen) <| 
                fun (q, l) -> HoodMelvilleQueue.foldBack (fun elem l' -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) <|
                fun (q, l) -> PhysicistQueue.foldBack (fun elem l'  -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length()))
              
            testPropertyWithConfig config10k "foldback matches build list PhysicistQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntOfSeqGen) <|
                fun (q, l) -> PhysicistQueue.foldBack (fun elem l' -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "foldback matches build list PhysicistQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntSnocGen) <|
                fun (q, l) -> PhysicistQueue.foldBack (fun elem l' -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get head from queue 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 3" (Prop.forAll (Arb.fromGen intGensStart1.[3]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 4" (Prop.forAll (Arb.fromGen intGensStart1.[4]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 5" (Prop.forAll (Arb.fromGen intGensStart1.[5]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 6" (Prop.forAll (Arb.fromGen intGensStart1.[6]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 7" (Prop.forAll (Arb.fromGen intGensStart1.[7]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 8" (Prop.forAll (Arb.fromGen intGensStart1.[8]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 9" (Prop.forAll (Arb.fromGen intGensStart1.[9]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 10" (Prop.forAll (Arb.fromGen intGensStart1.[10]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue 11" (Prop.forAll (Arb.fromGen intGensStart1.[11]) <|
                fun (q, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length()) ) 

            testPropertyWithConfig config10k "get head from queue safely 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 3" (Prop.forAll (Arb.fromGen intGensStart1.[3]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 4" (Prop.forAll (Arb.fromGen intGensStart1.[4]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 5" (Prop.forAll (Arb.fromGen intGensStart1.[5]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 6" (Prop.forAll (Arb.fromGen intGensStart1.[6]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 7" (Prop.forAll (Arb.fromGen intGensStart1.[7]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 8" (Prop.forAll (Arb.fromGen intGensStart1.[8]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 9" (Prop.forAll (Arb.fromGen intGensStart1.[9]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 10" (Prop.forAll (Arb.fromGen intGensStart1.[10]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get head from queue safely 11" (Prop.forAll (Arb.fromGen intGensStart1.[11]) <|
                fun (q, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length()) )

            testPropertyWithConfig config10k "get tail from queue 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 6" (Prop.forAll (Arb.fromGen intGensStart2.[6]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 7" (Prop.forAll (Arb.fromGen intGensStart2.[7]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 8" (Prop.forAll (Arb.fromGen intGensStart2.[8]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 9" (Prop.forAll (Arb.fromGen intGensStart2.[9]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 10" (Prop.forAll (Arb.fromGen intGensStart2.[10]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue 11" (Prop.forAll (Arb.fromGen intGensStart2.[11]) <|
                fun (q, l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))
            
            testPropertyWithConfig config10k "get tail from queue safely 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 3" (Prop.forAll (Arb.fromGen intGensStart2.[3]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 4" (Prop.forAll (Arb.fromGen intGensStart2.[4]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 5" (Prop.forAll (Arb.fromGen intGensStart2.[5]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 6" (Prop.forAll (Arb.fromGen intGensStart2.[6]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 7" (Prop.forAll (Arb.fromGen intGensStart2.[7]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 8" (Prop.forAll (Arb.fromGen intGensStart2.[8]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 9" (Prop.forAll (Arb.fromGen intGensStart2.[9]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 10" (Prop.forAll (Arb.fromGen intGensStart2.[10]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "get tail from queue safely 11" (Prop.forAll (Arb.fromGen intGensStart2.[11]) <|
                fun (q, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length()))
            
            testPropertyWithConfig config10k "int queue builds and serializes 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 3" (Prop.forAll (Arb.fromGen intGensStart1.[3]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 4" (Prop.forAll (Arb.fromGen intGensStart1.[4]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 5" (Prop.forAll (Arb.fromGen intGensStart1.[5]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 6" (Prop.forAll (Arb.fromGen intGensStart1.[6]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 7" (Prop.forAll (Arb.fromGen intGensStart1.[7]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 8" (Prop.forAll (Arb.fromGen intGensStart1.[8]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 9" (Prop.forAll (Arb.fromGen intGensStart1.[9]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 10" (Prop.forAll (Arb.fromGen intGensStart1.[10]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "int queue builds and serializes 11" (Prop.forAll (Arb.fromGen intGensStart1.[11]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "obj queue builds and serializes 0" (Prop.forAll (Arb.fromGen objGens.[0]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "obj queue builds and serializes 1" (Prop.forAll (Arb.fromGen objGens.[1]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "obj queue builds and serializes 2" (Prop.forAll (Arb.fromGen objGens.[2]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "obj queue builds and serializes 3" (Prop.forAll (Arb.fromGen objGens.[3]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "string queue builds and serializes 0" (Prop.forAll (Arb.fromGen stringGens.[0]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "string queue builds and serializes 1" (Prop.forAll (Arb.fromGen stringGens.[1]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "string queue builds and serializes 2" (Prop.forAll (Arb.fromGen stringGens.[2]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "string queue builds and serializes 3" (Prop.forAll (Arb.fromGen stringGens.[3]) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "reverse . reverse = id BankersQueue" (Prop.forAll (Arb.fromGen QueueGen.bankersQueueObjGen) <|
                fun (q, l) -> q :?> BankersQueue<obj> |> BankersQueue.rev |> BankersQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length()))
    
            testPropertyWithConfig config10k "reverse . reverse = id BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueObjGen) <|
                fun (q, l) -> q :?> BatchedQueue<obj> |> BatchedQueue.rev |> BatchedQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length()))
    
            testPropertyWithConfig config10k "reverse . reverse = idPhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) <|
                fun (q, l) -> q :?> PhysicistQueue<int> |> PhysicistQueue.rev |> PhysicistQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length()))

            testPropertyWithConfig config10k "ofList build and serializeBatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueOfListGen) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q q.Length)

            testPropertyWithConfig config10k "ofList build and serialize HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueOfListGen) <|
                fun (q, l) -> q |> Seq.toList = l |> classifyCollect q q.Length)

            testPropertyWithConfig config10k "ofList build and serializePhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueOfListqGen) <|
                fun (q , l) -> q |>  Seq.toList = l |> classifyCollect q q.Length)
        ]
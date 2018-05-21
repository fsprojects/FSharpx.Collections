namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.Tests.Properties
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

        testList "Experimental   IQueue" [
            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "allow to dequeue``(eIQ : IQueue<obj>) =
            //    ((eIQ.Snoc 1).Tail).IsEmpty |> Expect.isTrue "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "allow to enqueue``(eIQ : IQueue<obj>) =
            //    ((eIQ.Snoc 1).Snoc 2).IsEmpty |> Expect.isFalse "" }

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "cons pattern discriminator - BankersQueue" {
            //    let q = BankersQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
            //    let h1, t1 = 
            //        match q with
            //        | BankersQueue.Cons(h, t) -> h, t
            //        | _ ->  "x", q

            //    ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "cons pattern discriminator - BatchedQueue" {
            //    let q = BatchedQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
            //    let h1, t1 = 
            //        match q with
            //        | BatchedQueue.Cons(h, t) -> h, t
            //        | _ ->  "x", q

            //    ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "cons pattern discriminator - HoodMelvilleQueue" {
            //    let q = HoodMelvilleQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
            //    let h1, t1 = 
            //        match q with
            //        | HoodMelvilleQueue.Cons(h, t) -> h, t
            //        | _ ->  "x", q

            //    ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "cons pattern discriminator - PhysicistQueue" {
            //    let q = PhysicistQueue.ofSeq  ["f";"e";"d";"c";"b";"a"]
    
            //    let h1, t1 = 
            //        match q with
            //        | PhysicistQueue.Cons(h, t) -> h, t
            //        | _ ->  "x", q

            //    ((h1 = "f") && (t1.Length = 5)) |> Expect.isTrue "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "empty queue should be empty``(eIQ : IQueue<obj>) =
            //    eIQ.IsEmpty |> Expect.isTrue "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "fail if there is no head in the queue``(eIQ : IQueue<obj>) =
            //    let ok = ref false
            //    try
            //        eIQ.Head |> ignore
            //    with x when x = Exceptions.Empty -> ok := true
            //    !ok |> Expect.isTrue "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "fail if there is no tail in the queue``(eIQ : IQueue<obj>) =
            //    let ok = ref false
            //    try
            //        eIQ.Tail |> ignore
            //    with x when x = Exceptions.Empty -> ok := true
            //    !ok |> Expect.isTrue "" }

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "fold matches build list rev" {

            //    fsCheck "BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))
              
            //    fsCheck "BatchedQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntOfSeqGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //    fsCheck "BatchedQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntSnocGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> BatchedQueue<int> |> BatchedQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //    fsCheck "HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))
              
            //    fsCheck "HoodMelvilleQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntOfSeqGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //    fsCheck "HoodMelvilleQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntSnocGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> HoodMelvilleQueue<int> |> HoodMelvilleQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //    fsCheck "PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))
              
            //    fsCheck "PhysicistQueue OfSeq" (Prop.forAll (Arb.fromGen (QueueGen.physicistQueueIntOfSeqGen)) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //    fsCheck "PhysicistQueue Snoc" (Prop.forAll (Arb.fromGen (QueueGen.physicistQueueIntSnocGen)) 
            //        (fun ((q :IQueue<int>), (l : int list)) -> q :?> PhysicistQueue<int> |> PhysicistQueue.fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) |> classifyCollect q (q.Length())))

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "foldback matches build list" {

            //    fsCheck "BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> BatchedQueue.foldBack (fun (elem : int) (l' : int list)  -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length())))
              
            //    fsCheck "BatchedQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntOfSeqGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> BatchedQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //    fsCheck "BatchedQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueIntSnocGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> BatchedQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> BatchedQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //    fsCheck "HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> HoodMelvilleQueue.foldBack (fun (elem : int) (l' : int list)  -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length())))
              
            //    fsCheck "HoodMelvilleQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntOfSeqGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> HoodMelvilleQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //    fsCheck "HoodMelvilleQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueIntSnocGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> HoodMelvilleQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> HoodMelvilleQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //    fsCheck "PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> PhysicistQueue.foldBack (fun (elem : int) (l' : int list)  -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length())))
              
            //    fsCheck "PhysicistQueue OfSeq" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntOfSeqGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> PhysicistQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //    fsCheck "PhysicistQueue Snoc" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntSnocGen) 
            //        (fun ((q : IQueue<int>), (l : int list)) -> PhysicistQueue.foldBack (fun (elem : int) (l' : int list) -> elem::l') (q :?> PhysicistQueue<int>) [] = l |> classifyCollect q (q.Length())))

            //let rec nth l i =
            //    match i with
            //    | 0 -> List.head l
            //    | _ -> nth (List.tail l) (i-1)

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("intGensStart1")>]
            //test "get head from queue``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q :IQueue<int>, l) -> q.Head = (nth l 0) |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("intGensStart1")>]
            //test "get head from queue safely``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q :IQueue<int>, l) -> q.TryGetHead.Value = (nth l 0) |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("intGensStart2")>]
            //test "get tail from queue``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun ((q : IQueue<int>), l) -> q.Tail.Head = (nth l 1) |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("intGensStart2")>]
            //test "get tail from queue safely``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q : IQueue<int>, l) -> q.TryGetTail.Value.Head = (nth l 1) |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "give None if there is no head in the queue``(eIQ : IQueue<obj>) =
            //    eIQ.TryGetHead |> Expect.isNone "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("emptyIQueues")>]
            //test "give None if there is no tail in the queue``(eIQ  : IQueue<obj>) =
            //    eIQ.TryGetTail |> Expect.isNone "" }

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("intGensStart1")>]
            //test "int queue builds and serializes``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q :IQueue<int>, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("objGens")>]
            //test "obj queue builds and serializes``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q :IQueue<obj>, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length())))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //[<TestCaseSource("stringGens")>]
            //test "string queue builds and serializes``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (q :IQueue<string>, l) -> q |> Seq.toList = l |> classifyCollect q (q.Length())))

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "reverse . reverse = id" {

            //    fsCheck "BankersQueue" (Prop.forAll (Arb.fromGen QueueGen.bankersQueueObjGen) 
            //        (fun (q, l) -> q :?> BankersQueue<obj> |> BankersQueue.rev |> BankersQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length())))
    
            //    fsCheck "BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueObjGen) 
            //        (fun (q, l) -> q :?> BatchedQueue<obj> |> BatchedQueue.rev |> BatchedQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length())))
    
            //    fsCheck "PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueIntGen) 
            //        (fun (q, l) -> q :?> PhysicistQueue<int> |> PhysicistQueue.rev |> PhysicistQueue.rev |> Seq.toList = (q |> Seq.toList) |> classifyCollect q (q.Length())))

            //[<Category("nonIQueue")>]
            //[<Property("Category", "nonIQueue")>]
            //test "ofList build and serialize" {

            //    fsCheck "BatchedQueue" (Prop.forAll (Arb.fromGen QueueGen.batchedQueueOfListGen) 
            //        (fun ((q : BatchedQueue<int>), (l : int list)) -> q |> Seq.toList = l |> classifyCollect q q.Length))

            //    fsCheck "HoodMelvilleQueue" (Prop.forAll (Arb.fromGen QueueGen.hoodMelvilleQueueOfListGen) 
            //        (fun ((q : HoodMelvilleQueue<int>), (l : int list)) -> q |> Seq.toList = l |> classifyCollect q q.Length))

            //    fsCheck "PhysicistQueue" (Prop.forAll (Arb.fromGen QueueGen.physicistQueueOfListqGen) 
            //        (fun ((q : PhysicistQueue<int>), (l : int list)) -> q |>  Seq.toList = l |> classifyCollect q q.Length))

            //[<Category("IQueue")>]
            //[<Property("Category", "IQueue")>]
            //test "TryUncons wind-down to None" {
            //    let qBn = BankersQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
            //    let qBt = BatchedQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
            //    let qH = HoodMelvilleQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>
            //    let qP = PhysicistQueue.ofSeq  ["f";"e";"d";"c";"b";"a"] :> IQueue<string>

            //    let rec loop (iq : IQueue<string>) = 
            //        match (iq.TryUncons) with
            //        | Some(hd, tl) ->  loop tl
            //        | None -> ()

            //    loop qBn
            //    loop qBt
            //    loop qH
            //    loop qP

            //    true |> Expect.isTrue "" }
        ]
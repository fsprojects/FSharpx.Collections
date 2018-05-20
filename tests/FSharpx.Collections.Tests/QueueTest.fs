namespace FSharpx.Collections.Tests

open FSharpx.Collections
open FSharpx.Collections.Queue
open FSharpx.Collections.Tests.Properties
open FsCheck
open Expecto
open Expecto.Flip

module QueueTests =
    let emptyQueue = Queue.empty

    let testQueue =

        testList "Queue" [
            test "allow to dequeue" {
                Expect.isTrue "tail isEmpty" (emptyQueue |> conj 1 |> tail |> isEmpty) }

            test "allow to enqueue" {
                Expect.isFalse "tail" (emptyQueue |> conj 1 |> conj 2 |> isEmpty) }

            test "cons pattern discriminator - Queue" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | Cons(h, t) -> h, t
                    | _ ->  "x", q

                Expect.isTrue "cons pattern discriminator" ((h1 = "f") && (t1.Length = 5)) }

            test "empty queue should be empty" {
                Expect.isTrue "empty" (emptyQueue |> isEmpty) }

            test "fail if there is no head in the queue" {
                Expect.throwsT<System.Exception> "no head"  (fun () -> emptyQueue |> head |> ignore) }

            test "fail if there is no tail in the queue" {
                Expect.throwsT<System.Exception> "no tail"  (fun () -> emptyQueue |> tail |> ignore) }

            test "give None if there is no head in the queue" {
                Expect.isNone "no head" (emptyQueue |> tryHead) }

            test "give None if there is no tail in the queue" {
                Expect.isNone "no tail" (emptyQueue |> tryTail) }

            test "toSeq to list" {
                let l = ["f";"e";"d";"c";"b";"a"] 
                let q = ofSeq l

                Expect.equal "toSeq" l (q|> toSeq |> List.ofSeq) }

            test "TryUncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : Queue<string>) = 
                    match (q'.TryUncons) with
                    | Some(_, tl) ->  loop tl
                    | None -> None

                Expect.isNone "TryUncons" <| loop q }

            test "Uncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : Queue<string>) = 
                    match (q'.Uncons) with
                    | _, tl when tl.IsEmpty ->  true
                    | _, tl ->  loop tl

                Expect.isTrue "Uncons" <| loop q }

            test "structural equality" {
                let l1 = ofSeq [1..100]
                let l2 = ofSeq [1..100]

                Expect.equal "structural equality" l1 l2

                let l3 = ofSeq [1..99] |> conj 7

                Expect.notEqual "" l1 l3 }
        ]
        
    [<Tests>]
    let propertyTestQueue =

        let enqueueThruList l q  =
            let rec loop (q' : 'a Queue) (l' : 'a list) = 
                match l' with
                | hd :: [] -> q'.Conj hd
                | hd :: tl -> loop (q'.Conj hd) tl
                | [] -> q'
        
            loop q l

        //Queue
        (*
        non-IQueue generators from random ofList
        *)
        let queueOfListGen =
            gen {   let! n = Gen.length2thru12
                    let! x = Gen.listInt n
                    return ( (Queue.ofList x), x) }

        (*
        IQueue generators from random ofSeq and/or conj elements from random list 
        *)
        let queueIntGen =
            gen {   let! n = Gen.length1thru12
                    let! n2 = Gen.length2thru12
                    let! x =  Gen.listInt n
                    let! y =  Gen.listInt n2
                    return ( (Queue.ofSeq x |> enqueueThruList y), (x @ y) ) }

        let queueIntOfSeqGen =
            gen {   let! n = Gen.length1thru12
                    let! x = Gen.listInt n
                    return ( (Queue.ofSeq x), x) }

        let queueIntConjGen =
            gen {   let! n = Gen.length1thru12
                    let! x = Gen.listInt n
                    return ( (Queue.empty |> enqueueThruList x), x) }

        let queueObjGen =
            gen {   let! n = Gen.length2thru12
                    let! n2 = Gen.length1thru12
                    let! x =  Gen.listObj n
                    let! y =  Gen.listObj n2
                    return ( (Queue.ofSeq x |> enqueueThruList y), (x @ y) ) }

        let queueStringGen =
            gen {   let! n = Gen.length1thru12
                    let! n2 = Gen.length2thru12
                    let! x =  Gen.listString n
                    let! y =  Gen.listString n2  
                    return ( (Queue.ofSeq x |> enqueueThruList y), (x @ y) ) }

        let intGens start =
            let v = Array.create 3 queueIntGen
            v.[1] <- queueIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
            v.[2] <-queueIntConjGen |> Gen.filter (fun (q, l) -> l.Length >= start)
            v

        let intGensStart1 =
            intGens 1  //this will accept all

        let intGensStart2 =
            intGens 2 // this will accept 11 out of 12

        testList "Queue property tests" [

            testPropertyWithConfig config10k "fold matches build list rev" (Prop.forAll (Arb.fromGen queueIntGen) <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = (List.rev l) |> classifyCollect q q.Length)
              
            testPropertyWithConfig config10k "Queue OfSeq fold matches build list rev"  (Prop.forAll (Arb.fromGen queueIntOfSeqGen) <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k "Queue Conj fold matches build list rev" (Prop.forAll (Arb.fromGen queueIntConjGen) <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k "Queue foldback matches build list" (Prop.forAll (Arb.fromGen queueIntGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )
              
            testPropertyWithConfig config10k " Queue OfSeqfoldback matches build list" (Prop.forAll (Arb.fromGen queueIntOfSeqGen)  <|
                fun (q, l) -> foldBack (fun elem l'  -> elem::l') q [] = l |> classifyCollect q q.Length)

            testPropertyWithConfig config10k "Queue Conj foldback matches build list" (Prop.forAll (Arb.fromGen queueIntConjGen) <|
                fun (q, l) -> foldBack (fun elem l'  -> elem::l') q [] = l )

            testPropertyWithConfig config10k "get head from queue 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> (head q) = (List.item 0 l) )

            testPropertyWithConfig config10k "get head from queue 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> (head q) = (List.item 0 l) )

            testPropertyWithConfig config10k "get head from queue 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> (head q) = (List.item 0 l) )

            testPropertyWithConfig config10k "get head from queue safely 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> (tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k "get head from queue safely 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> (tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k "get head from queue safely 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> (tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k "get tail from queue 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "get tail from queue 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "get tail from queue 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "get tail from queue safely 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "get tail from queue safely 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "get tail from queue safely 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k "int queue builds and serializes 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "int queue builds and serializes 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "int queue builds and serializes 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "obj queue builds and serializes" (Prop.forAll (Arb.fromGen queueObjGen) <|
                fun (q : Queue<obj>, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "string queue builds and serializes" (Prop.forAll (Arb.fromGen queueStringGen) <|
                fun (q : Queue<string>, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "reverse . reverse = id" (Prop.forAll (Arb.fromGen queueObjGen) <|
                fun (q, l) -> q |> rev |> rev |> Seq.toList = (q |> Seq.toList) )

            testPropertyWithConfig config10k "ofList build and serialize" (Prop.forAll (Arb.fromGen queueOfListGen) <|
                fun (q, l) -> q |> Seq.toList = l )
        ]
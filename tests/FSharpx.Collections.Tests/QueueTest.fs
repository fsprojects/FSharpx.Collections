namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Properties
open FsCheck
open Expecto
open Expecto.Flip

module QueueTests =
    let emptyQueue = Queue.empty

    [<Tests>]
    let testQueue =

        testList
            "Queue"
            [ test "allow to dequeue" { Expect.isTrue "tail Queue.isEmpty" (emptyQueue |> Queue.conj 1 |> Queue.tail |> Queue.isEmpty) }

              test "allow to enqueue" { Expect.isFalse "tail" (emptyQueue |> Queue.conj 1 |> Queue.conj 2 |> Queue.isEmpty) }

              test "cons pattern discriminator - Queue" {
                  let q = Queue.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let h1, t1 =
                      match q with
                      | Queue.Cons(h, t) -> h, t
                      | _ -> "x", q

                  Expect.isTrue "cons pattern discriminator" ((h1 = "f") && (t1.Length = 5))
              }

              test "empty queue should be empty" { Expect.isTrue "empty" (emptyQueue |> Queue.isEmpty) }

              test "fail if there is no head in the queue" {
                  Expect.throwsT<System.Exception> "no head" (fun () -> emptyQueue |> Queue.head |> ignore)
              }

              test "fail if there is no tail in the queue" {
                  Expect.throwsT<System.Exception> "no tail" (fun () -> emptyQueue |> Queue.tail |> ignore)
              }

              test "give None if there is no head in the queue" { Expect.isNone "no head" (emptyQueue |> Queue.tryHead) }

              test "give None if there is no tail in the queue" { Expect.isNone "no tail" (emptyQueue |> Queue.tryTail) }

              test "toSeq to list" {
                  let l = [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let q = Queue.ofSeq l

                  Expect.equal "toSeq" l (q |> Queue.toSeq |> List.ofSeq)
              }

              test "TryUncons wind-down to None" {
                  let q = Queue.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let rec loop(q': Queue<string>) =
                      match (q'.TryUncons) with
                      | Some(_, tl) -> loop tl
                      | None -> None

                  Expect.isNone "TryUncons" <| loop q
              }

              test "Uncons wind-down to None" {
                  let q = Queue.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let rec loop(q': Queue<string>) =
                      match (q'.Uncons) with
                      | _, tl when tl.IsEmpty -> true
                      | _, tl -> loop tl

                  Expect.isTrue "Uncons" <| loop q
              }

              test "structural equality" {
                  let l1 = Queue.ofSeq [ 1..100 ]
                  let l2 = Queue.ofSeq [ 1..100 ]

                  Expect.equal "structural equality" l1 l2

                  let l3 = Queue.ofSeq [ 1..99 ] |> Queue.conj 7

                  Expect.notEqual "" l1 l3
              } ]

    [<Tests>]
    let propertyTestQueue =

        let enqueueThruList l q =
            let rec loop (q': 'a Queue) (l': 'a list) =
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
            gen {
                let! n = Gen.length2thru12
                let! x = Gen.listInt n
                return ((Queue.ofList x), x)
            }

        (*
        IQueue generators from random Queue.ofSeq and/or Queue.conj elements from random list 
        *)
        let queueIntGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listInt n
                let! y = Gen.listInt n2
                return ((Queue.ofSeq x |> enqueueThruList y), (x @ y))
            }

        let queueIntOfSeqGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Queue.ofSeq x), x)
            }

        let queueIntConjGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Queue.empty |> enqueueThruList x), x)
            }

        let queueObjGen =
            gen {
                let! n = Gen.length2thru12
                let! n2 = Gen.length1thru12
                let! x = Gen.listObj n
                let! y = Gen.listObj n2
                return ((Queue.ofSeq x |> enqueueThruList y), (x @ y))
            }

        let queueStringGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listString n
                let! y = Gen.listString n2
                return ((Queue.ofSeq x |> enqueueThruList y), (x @ y))
            }

        let intGens start =
            let v = Array.create 3 queueIntGen
            v.[1] <- queueIntOfSeqGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[2] <- queueIntConjGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v

        let intGensStart1 = intGens 1 //this will accept all

        let intGensStart2 = intGens 2 // this will accept 11 out of 12

        testList
            "Queue property tests"
            [

              testPropertyWithConfig
                  config10k
                  "fold matches build list rev"
                  (Prop.forAll(Arb.fromGen queueIntGen)
                   <| fun (q, l) ->
                       q |> Queue.fold (fun l' elem -> elem :: l') [] = (List.rev l)
                       |> classifyCollect q q.Length)

              testPropertyWithConfig
                  config10k
                  "Queue OfSeq fold matches build list rev"
                  (Prop.forAll(Arb.fromGen queueIntOfSeqGen)
                   <| fun (q, l) -> q |> Queue.fold (fun l' elem -> elem :: l') [] = (List.rev l))

              testPropertyWithConfig
                  config10k
                  "Queue Conj fold matches build list rev"
                  (Prop.forAll(Arb.fromGen queueIntConjGen)
                   <| fun (q, l) -> q |> Queue.fold (fun l' elem -> elem :: l') [] = (List.rev l))

              testPropertyWithConfig
                  config10k
                  "Queue foldback matches build list"
                  (Prop.forAll(Arb.fromGen queueIntGen)
                   <| fun (q, l) -> Queue.foldBack (fun elem l' -> elem :: l') q [] = l)

              testPropertyWithConfig
                  config10k
                  " Queue OfSeqfoldback matches build list"
                  (Prop.forAll(Arb.fromGen queueIntOfSeqGen)
                   <| fun (q, l) ->
                       Queue.foldBack (fun elem l' -> elem :: l') q [] = l
                       |> classifyCollect q q.Length)

              testPropertyWithConfig
                  config10k
                  "Queue Conj foldback matches build list"
                  (Prop.forAll(Arb.fromGen queueIntConjGen)
                   <| fun (q, l) -> Queue.foldBack (fun elem l' -> elem :: l') q [] = l)

              testPropertyWithConfig
                  config10k
                  "get head from queue 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> (Queue.head q) = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get head from queue 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> (Queue.head q) = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get head from queue 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> (Queue.head q) = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get head from queue safely 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> (Queue.tryHead q).Value = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get head from queue safely 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> (Queue.tryHead q).Value = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get head from queue safely 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> (Queue.tryHead q).Value = (List.item 0 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun ((q), l) -> q.Tail.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun ((q), l) -> q.Tail.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun ((q), l) -> q.Tail.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue safely 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue safely 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "get tail from queue safely 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

              testPropertyWithConfig
                  config10k
                  "int queue builds and serializes 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "int queue builds and serializes 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "int queue builds and serializes 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "obj queue builds and serializes"
                  (Prop.forAll(Arb.fromGen queueObjGen)
                   <| fun (q: Queue<obj>, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "string queue builds and serializes"
                  (Prop.forAll(Arb.fromGen queueStringGen)
                   <| fun (q: Queue<string>, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "reverse . reverse = id"
                  (Prop.forAll(Arb.fromGen queueObjGen)
                   <| fun (q, l) -> q |> Queue.rev |> Queue.rev |> Seq.toList = (q |> Seq.toList))

              testPropertyWithConfig
                  config10k
                  "ofList build and serialize"
                  (Prop.forAll(Arb.fromGen queueOfListGen)
                   <| fun (q, l) -> q |> Seq.toList = l)

              test "toList preserves FIFO order" {
                  let q = Queue.ofSeq [ 1; 2; 3; 4; 5 ]
                  Expect.equal "toList" [ 1; 2; 3; 4; 5 ] (Queue.toList q)
              }

              test "toList empty queue" { Expect.equal "toList empty" [] (Queue.toList Queue.empty) }

              test "toArray preserves FIFO order" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  Expect.equal "toArray" [| 1; 2; 3 |] (Queue.toArray q)
              }

              test "map transforms elements" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  Expect.equal "map" [ 2; 4; 6 ] (Queue.map ((*) 2) q |> Queue.toList)
              }

              test "map preserves FIFO order across front/rBack boundary" {
                  let q = Queue.ofSeq [ "a"; "b"; "c" ] |> Queue.conj "d" |> Queue.conj "e"

                  Expect.equal "map order" [ "A"; "B"; "C"; "D"; "E" ] (Queue.map (fun (s: string) -> s.ToUpper()) q |> Queue.toList)
              }

              test "filter keeps matching elements" {
                  let q = Queue.ofSeq [ 1; 2; 3; 4; 5; 6 ]
                  Expect.equal "filter even" [ 2; 4; 6 ] (Queue.filter (fun x -> x % 2 = 0) q |> Queue.toList)
              }

              test "filter preserves order" {
                  let q = Queue.ofSeq [ 1; 2; 3; 4; 5 ]
                  Expect.equal "filter preserves order" [ 1; 3; 5 ] (Queue.filter (fun x -> x % 2 <> 0) q |> Queue.toList)
              }

              test "filter all out gives empty" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  Expect.isTrue "filter all out" (Queue.filter (fun _ -> false) q |> Queue.isEmpty)
              }

              test "iter visits each element in FIFO order" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  let result = System.Collections.Generic.List<int>()
                  Queue.iter result.Add q
                  Expect.equal "iter order" [ 1; 2; 3 ] (List.ofSeq result)
              }

              test "exists returns true when element satisfies predicate" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  Expect.isTrue "exists" (Queue.exists ((=) 2) q)
              }

              test "exists returns false when no element satisfies predicate" {
                  let q = Queue.ofSeq [ 1; 2; 3 ]
                  Expect.isFalse "exists false" (Queue.exists ((=) 99) q)
              }

              test "forall returns true when all elements satisfy predicate" {
                  let q = Queue.ofSeq [ 2; 4; 6 ]
                  Expect.isTrue "forall" (Queue.forall (fun x -> x % 2 = 0) q)
              }

              test "forall returns false when some elements do not satisfy predicate" {
                  let q = Queue.ofSeq [ 2; 3; 6 ]
                  Expect.isFalse "forall false" (Queue.forall (fun x -> x % 2 = 0) q)
              } ]

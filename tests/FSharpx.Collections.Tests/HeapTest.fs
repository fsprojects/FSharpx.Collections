namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Properties
open FsCheck
open Expecto
open Expecto.Flip

module HeapTests =

    [<Tests>]
    let testHeap =
        testList
            "Heap"
            [

              test "cons pattern discriminator" {
                  let h = Heap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let h1, t1 = Heap.uncons h

                  let h2, t2 =
                      match t1 with
                      | Heap.Cons(h, t) -> h, t
                      | _ -> "x", t1

                  Expect.isTrue "cons pattern" ((h2 = "e") && ((Heap.length t2) = 4))
              }

              test "cons pattern discriminator 2" {
                  let h = Heap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let t2 =
                      match h with
                      | Heap.Cons("f", Heap.Cons(_, t)) -> t
                      | _ -> h

                  let h1, t3 = Heap.uncons t2

                  Expect.isTrue "cons pattern" ((h1 = "d") && ((Heap.length t2) = 4))
              }

              test "empty list should be empty" { Expect.isTrue "empty" (Heap.empty true).IsEmpty }

              test "rev empty" {
                  let h = Heap.empty true
                  Expect.isTrue "" (h |> Heap.rev |> Heap.isEmpty)
                  let h' = Heap.empty false
                  Expect.isTrue "" (h' |> Heap.rev |> Heap.isEmpty)
              }

              test "insert works" { Expect.isFalse "" (((Heap.empty true).Insert 1).Insert 2).IsEmpty }

              test "length of empty is 0" { Expect.equal "empty" 0 (Heap.empty true).Length }

              test "tryHead on empty should return None" { Expect.isNone "tryHead" (Heap.empty true).TryHead }

              test "tryTail on empty should return None" { Expect.isNone "tryTail" <| (Heap.empty true).TryTail() }

              test "tryTail on len 1 should return Some empty" {
                  let h = Heap.empty true |> Heap.insert 1 |> Heap.tryTail
                  Expect.isTrue "tryTail" (h.Value |> Heap.isEmpty)
              }

              test "tryMerge max and min should be None" {
                  let h1 = Heap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let h2 = Heap.ofSeq false [ "t"; "u"; "v"; "w"; "x"; "y"; "z" ]

                  Expect.isNone "tryMerge" <| Heap.tryMerge h1 h2
              }

              test "structural equality" {
                  let l1 = Heap.ofSeq true [ 1..100 ]
                  let l2 = Heap.ofSeq true [ 1..100 ]

                  Expect.equal "structural equality" l1 l2

                  let l3 = Heap.ofSeq true [ 1..99 ] |> Heap.insert 7

                  Expect.notEqual "structural equality" l1 l3
              }

              test "toSeq to list" {
                  let l = [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let h = Heap.ofSeq true l

                  Expect.equal "toSeq to list" l (h |> Heap.toSeq |> List.ofSeq)
              }

              test "tryUncons empty" { Expect.isNone "TryUncons" <| (Heap.empty true).TryUncons() }

              test "Tail of large heap does not result in stackoverflow" {
                  let rnd = new System.Random()
                  let h = [ 1..1000000 ] |> Seq.sortBy(fun x -> rnd.Next()) |> Heap.ofSeq false

                  Heap.tail h |> ignore
                  Expect.isTrue "" true
              } ]

    [<Tests>]
    let propertyTestHeap =
        //only going up to 5 elements is probably sufficient to test all edge cases

        let insertThruList l h =
            List.fold (fun (h': Heap<'a>) x -> h'.Insert x) h l

        let maxHeapIntGen =
            gen {
                let! n = Gen.length2thru12
                let! n2 = Gen.length1thru12
                let! x = Gen.listInt n
                let! y = Gen.listInt n2
                return ((Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
            }

        let maxHeapIntOfSeqGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Heap.ofSeq true x), (x |> List.sort |> List.rev))
            }

        let maxHeapIntInsertGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Heap.empty true |> insertThruList x), (x |> List.sort |> List.rev))
            }

        let maxHeapStringGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listString n
                let! y = Gen.listString n2
                return ((Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
            }

        let minHeapIntGen =
            gen {
                let! n = Gen.length2thru12
                let! n2 = Gen.length1thru12
                let! x = Gen.listInt n
                let! y = Gen.listInt n2
                return ((Heap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
            }

        let minHeapIntOfSeqGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Heap.ofSeq false x), (x |> List.sort))
            }

        let minHeapIntInsertGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Heap.empty false |> insertThruList x), (x |> List.sort))
            }

        let minHeapStringGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listString n
                let! y = Gen.listString n2
                return ((Heap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
            }

        let intGens start =
            let v = Array.create 6 maxHeapIntGen
            v.[1] <- maxHeapIntOfSeqGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[2] <- maxHeapIntInsertGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[3] <- minHeapIntGen
            v.[4] <- minHeapIntOfSeqGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[5] <- minHeapIntInsertGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v

        let stringGens =
            let v = Array.create 2 maxHeapStringGen
            v.[1] <- minHeapStringGen
            v

        let intGensStart1 = intGens 1 //this will accept all

        let intGensStart2 = intGens 2 // this will accept 11 out of 12

        testList
            "Heap property tests"
            [

              testPropertyWithConfig
                  config10k
                  "head should return 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "head should return 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "head should return 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "head should return 3"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "head should return 4"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "head should return 5"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) -> h.Head = l.Head)

              testPropertyWithConfig
                  config10k
                  "maxHeap seq enumerate matches build list"
                  (Prop.forAll(Arb.fromGen maxHeapIntGen)
                   <| fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "minHeap seq enumerate matches build list"
                  (Prop.forAll(Arb.fromGen minHeapIntGen)
                   <| fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "rev works max heap"
                  (Prop.forAll(Arb.fromGen maxHeapIntGen)
                   <| fun (h, l) -> h |> Heap.rev |> List.ofSeq = (h |> List.ofSeq |> List.rev))

              testPropertyWithConfig
                  config10k
                  "rev works min  heap"
                  (Prop.forAll(Arb.fromGen minHeapIntGen)
                   <| fun (h, l) -> h |> Heap.rev |> List.ofSeq = (h |> List.ofSeq |> List.rev))

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 3"
                  (Prop.forAll(Arb.fromGen intGensStart1.[3])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 4"
                  (Prop.forAll(Arb.fromGen intGensStart1.[4])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list int 5"
                  (Prop.forAll(Arb.fromGen intGensStart1.[5])
                   <| fun (h, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list string 0"
                  (Prop.forAll(Arb.fromGen stringGens.[0])
                   <| fun (h: Heap<string>, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list string 1"
                  (Prop.forAll(Arb.fromGen stringGens.[1])
                   <| fun (h: Heap<string>, l) -> h |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "tail should return 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tail should return 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tail should return 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tail should return 3"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))


              testPropertyWithConfig
                  config10k
                  "tail should return 4"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tail should return 5"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then tl.Head = l.Item(1) else true
                       tlHead && (tl.Length = l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryHead should return 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryHead should return 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryHead should return 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryHead should return 3"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryHead should return 4"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryHead should return 5"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) -> h.TryHead.Value = l.Head)

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 3"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 4"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "tryUncons 1 element 5"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 3"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 4"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              testPropertyWithConfig
                  config10k
                  "Heap.uncons 1 element 5"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()
                       x = l.Head && tl.Length = (l.Length - 1))

              test "toList ascending returns sorted list" {
                  let h = Heap.ofSeq false [ 3; 1; 4; 1; 5; 9; 2; 6 ]
                  Expect.equal "toList asc" [ 1; 1; 2; 3; 4; 5; 6; 9 ] (Heap.toList h)
              }

              test "toList descending returns reverse-sorted list" {
                  let h = Heap.ofSeq true [ 3; 1; 4; 1; 5; 9; 2; 6 ]
                  Expect.equal "toList desc" [ 9; 6; 5; 4; 3; 2; 1; 1 ] (Heap.toList h)
              }

              test "toArray ascending returns sorted array" {
                  let h = Heap.ofSeq false [ 5; 3; 1; 4; 2 ]
                  Expect.equal "toArray asc" [| 1; 2; 3; 4; 5 |] (Heap.toArray h)
              }

              test "ofList round-trips through toList" {
                  let xs = [ 7; 3; 5; 1; 9; 2; 4; 8; 6 ]
                  let h = Heap.ofList false xs
                  Expect.equal "ofList round-trip" (List.sort xs) (Heap.toList h)
              }

              test "ofArray round-trips through toArray" {
                  let xs = [| 7; 3; 5; 1; 9; 2; 4; 8; 6 |]
                  let h = Heap.ofArray false xs
                  Expect.equal "ofArray round-trip" (Array.sort xs) (Heap.toArray h)
              }

              test "fold accumulates in sorted order" {
                  let h = Heap.ofSeq false [ 3; 1; 2 ]
                  let result = Heap.fold (fun acc x -> acc @ [ x ]) [] h
                  Expect.equal "fold ascending" [ 1; 2; 3 ] result
              }

              test "iter visits elements in sorted order" {
                  let h = Heap.ofSeq false [ 3; 1; 2 ]
                  let mutable result = []
                  Heap.iter (fun x -> result <- result @ [ x ]) h
                  Expect.equal "iter ascending" [ 1; 2; 3 ] result
              }

              test "exists returns true when predicate matches" {
                  let h = Heap.ofSeq false [ 1; 2; 3; 4; 5 ]
                  Expect.isTrue "exists" (Heap.exists (fun x -> x = 3) h)
              }

              test "exists returns false when predicate does not match" {
                  let h = Heap.ofSeq false [ 1; 2; 3; 4; 5 ]
                  Expect.isFalse "exists" (Heap.exists (fun x -> x = 6) h)
              }

              test "forall returns true when all match" {
                  let h = Heap.ofSeq false [ 2; 4; 6; 8 ]
                  Expect.isTrue "forall" (Heap.forall (fun x -> x % 2 = 0) h)
              }

              test "forall returns false when some do not match" {
                  let h = Heap.ofSeq false [ 2; 3; 4 ]
                  Expect.isFalse "forall" (Heap.forall (fun x -> x % 2 = 0) h)
              }

              test "map transforms elements" {
                  let h = Heap.ofSeq false [ 1; 2; 3; 4; 5 ]
                  let mapped = Heap.map (fun x -> x * 2) h
                  Expect.equal "map" [ 2; 4; 6; 8; 10 ] (Heap.toList mapped)
              }

              test "map preserves sort direction" {
                  let h = Heap.ofSeq true [ 1; 2; 3 ]
                  let mapped = Heap.map (fun x -> x * 10) h
                  Expect.equal "map desc" [ 30; 20; 10 ] (Heap.toList mapped)
              }

              test "filter keeps only matching elements" {
                  let h = Heap.ofSeq false [ 1; 2; 3; 4; 5; 6 ]
                  let filtered = Heap.filter (fun x -> x % 2 = 0) h
                  Expect.equal "filter" [ 2; 4; 6 ] (Heap.toList filtered)
              }

              test "filter empty heap returns empty heap" {
                  let h = Heap.empty false
                  let filtered = Heap.filter (fun x -> x > 0) h
                  Expect.isTrue "filter empty" (Heap.isEmpty filtered)
              }

              test "choose keeps Some values" {
                  let h = Heap.ofSeq false [ 1; 2; 3; 4; 5 ]
                  let chosen = Heap.choose (fun x -> if x % 2 = 0 then Some(x * 10) else None) h
                  Expect.equal "choose" [ 20; 40 ] (Heap.toList chosen)
              }

              test "choose all None returns empty heap" {
                  let h = Heap.ofSeq false [ 1; 3; 5 ]
                  let chosen = Heap.choose (fun _ -> None) h
                  Expect.isTrue "choose none" (Heap.isEmpty chosen)
              } ]

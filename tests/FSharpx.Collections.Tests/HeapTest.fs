namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Properties
open FsCheck
open Expecto
open Expecto.Flip

module HeapTests =

    [<Tests>]
    let testHeap =
        testList "Heap" [

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
            }
        ]

    [<Tests>]
    let propertyTestHeap =
        //only going up to 5 elements is probably sufficient to test all edge cases

        let insertThruList l h =
            List.fold (fun (h': Heap<'a>) x -> h'.Insert x) h l

        let maxHeapIntGen = gen {
            let! n = Gen.length2thru12
            let! n2 = Gen.length1thru12
            let! x = Gen.listInt n
            let! y = Gen.listInt n2
            return ((Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
        }

        let maxHeapIntOfSeqGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((Heap.ofSeq true x), (x |> List.sort |> List.rev))
        }

        let maxHeapIntInsertGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((Heap.empty true |> insertThruList x), (x |> List.sort |> List.rev))
        }

        let maxHeapStringGen = gen {
            let! n = Gen.length1thru12
            let! n2 = Gen.length2thru12
            let! x = Gen.listString n
            let! y = Gen.listString n2
            return ((Heap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
        }

        let minHeapIntGen = gen {
            let! n = Gen.length2thru12
            let! n2 = Gen.length1thru12
            let! x = Gen.listInt n
            let! y = Gen.listInt n2
            return ((Heap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
        }

        let minHeapIntOfSeqGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((Heap.ofSeq false x), (x |> List.sort))
        }

        let minHeapIntInsertGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((Heap.empty false |> insertThruList x), (x |> List.sort))
        }

        let minHeapStringGen = gen {
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

        testList "Heap property tests" [

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
        ]

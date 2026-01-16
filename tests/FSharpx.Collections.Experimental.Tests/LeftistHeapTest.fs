namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip
open HeapGen

//only going up to 5 elements is probably sufficient to test all edge cases

module LeftistHeapTest =

    let intGens start =
        let v = Array.create 6 maxLeftistHeapIntGen

        v.[1] <-
            maxLeftistHeapIntOfSeqGen
            |> Gen.filter(fun (q, l) -> l.Length >= start) // "max LeftistHeap OfSeq")

        v.[2] <-
            maxLeftistHeapIntInsertGen
            |> Gen.filter(fun (q, l) -> l.Length >= start) // "max LeftistHeap from Insert")

        v.[3] <- minLeftistHeapIntGen // "min LeftistHeap int")

        v.[4] <-
            minLeftistHeapIntOfSeqGen
            |> Gen.filter(fun (q, l) -> l.Length >= start) // "min LeftistHeap OfSeq")

        v.[5] <-
            minLeftistHeapIntInsertGen
            |> Gen.filter(fun (q, l) -> l.Length >= start) // "min LeftistHeap from Insert")

        v

    let stringGens =
        let v = Array.create 2 maxLeftistHeapStringGen
        v.[1] <- minLeftistHeapStringGen
        v

    let intGensStart1 = intGens 1 //this will accept all

    let intGensStart2 = intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testLeftistHeap =

        testList
            "Experimental LeftistHeap"
            [

              test "cons pattern discriminator" {
                  let h = LeftistHeap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let h1, t1 = LeftistHeap.uncons h

                  let h2, t2 =
                      match t1 with
                      | LeftistHeap.Cons(h, t) -> h, t
                      | _ -> "x", t1

                  ((h2 = "e") && ((LeftistHeap.length t2) = 4)) |> Expect.isTrue ""
              }

              test "cons pattern discriminator 2" {
                  let h = LeftistHeap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let t2 =
                      match h with
                      | LeftistHeap.Cons("f", LeftistHeap.Cons(_, t)) -> t
                      | _ -> h

                  let h1, t3 = LeftistHeap.uncons t2

                  ((h1 = "d") && ((LeftistHeap.length t2) = 4)) |> Expect.isTrue ""
              }

              test "empty list should be empty" { (LeftistHeap.empty true).IsEmpty |> Expect.isTrue "" }

              test "IHeap insert works" {
                  let h =
                      LeftistHeap.empty true
                      |> LeftistHeap.insert "a"
                      |> LeftistHeap.insert "b"
                      |> LeftistHeap.insert "c"
                      |> LeftistHeap.insert "d"
                      |> LeftistHeap.insert "e"
                      |> LeftistHeap.insert "f"
                      |> LeftistHeap.insert "g"
                      |> LeftistHeap.insert "h"
                      |> LeftistHeap.insert "i"
                      |> LeftistHeap.insert "j"

                  ((h :> IHeap<_, string>).Insert "zz").Head |> Expect.equal "" "zz"
              }

              test "insert works" {
                  (((LeftistHeap.empty true).Insert 1).Insert 2).IsEmpty
                  |> Expect.isFalse ""
              }

              test "length of empty is 0" { (LeftistHeap.empty true).Length |> Expect.equal "" 0 }

              test "structure pattern match and merge" {
                  let h = LeftistHeap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let x, h1, h2 =
                      match h with
                      | LeftistHeap.T(_, _, _, x', h1', h2') -> x', h1', h2'
                      | _ -> "zz", h, h

                  let h3 = LeftistHeap.merge h1 h2

                  let x2, t3 = LeftistHeap.uncons h3

                  ((x = "f") && (x2 = "e") && ((LeftistHeap.length t3) = 4))
                  |> Expect.isTrue ""
              }

              test "tryGetHead on empty should return None" { (LeftistHeap.empty true).TryGetHead |> Expect.isNone "" }

              test "tryGetTail on empty should return None" { (LeftistHeap.empty true).TryGetTail() |> Expect.isNone "" }

              test "tryGetTail on len 1 should return Some empty" {
                  (LeftistHeap.empty true
                   |> LeftistHeap.insert 1
                   |> LeftistHeap.tryGetTail)
                      .Value
                  |> LeftistHeap.isEmpty
                  |> Expect.isTrue ""
              }

              test "tryMerge max and mis should be None" {
                  let h1 = LeftistHeap.ofSeq true [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let h2 = LeftistHeap.ofSeq false [ "t"; "u"; "v"; "w"; "x"; "y"; "z" ]

                  LeftistHeap.tryMerge h1 h2 |> Expect.isNone ""
              }

              test "tryUncons empty" { (LeftistHeap.empty true).TryUncons() |> Expect.isNone "" } ]

    [<Tests>]
    let testLeftistHeapProperties =

        testList
            "Experimental LeftistHeap properties"
            [

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert head should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) -> (h.Head = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list maxLeftistHeap"
                  (Prop.forAll(Arb.fromGen maxLeftistHeapIntGen)
                   <| fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "seq enumerate matches build list minLeftistHeap"
                  (Prop.forAll(Arb.fromGen minLeftistHeapIntGen)
                   <| fun (h, l) -> h |> List.ofSeq = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[3])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[4])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert seq enumerate matches build list int"
                  (Prop.forAll(Arb.fromGen intGensStart1.[5])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)


              testPropertyWithConfig
                  config10k
                  "max LeftistHeap string seq enumerate matches build list string"
                  (Prop.forAll(Arb.fromGen stringGens.[0])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap string seq enumerate matches build list string"
                  (Prop.forAll(Arb.fromGen stringGens.[1])
                   <| fun (h, l) -> h |> Seq.toList = l |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert tail should return"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) ->
                       let tl = h.Tail()
                       let tlHead = if (tl.Length > 0) then (tl.Head = l.Item(1)) else true
                       (tlHead && (tl.Length = (l.Length - 1))) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert tryGetHead should return`"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) -> (h.TryGetHead.Value = l.Head) |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)
              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[3])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[4])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert tryUncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[5])
                   <| fun (h, l) ->
                       let x, tl = h.TryUncons().Value

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap int uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap OfSeq uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "max LeftistHeap from Insert uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap int uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap OfSeq uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length)

              testPropertyWithConfig
                  config10k
                  "min LeftistHeap from Insert uncons 1 element"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (h, l) ->
                       let x, tl = h.Uncons()

                       ((x = l.Head) && (tl.Length = (l.Length - 1)))
                       |> classifyCollect h h.Length) ]

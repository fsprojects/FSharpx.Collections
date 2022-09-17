namespace FSharpx.Collections.Tests
// First version copied from the F# Power Pack
// https://raw.github.com/fsharp/powerpack/master/src/FSharp.PowerPack.Unittests/LazyListTests.fs

open FSharpx.Collections
open Expecto
open Expecto.Flip
open System.Collections.Generic

#nowarn "40"

module LazyList =

    let rec pairReduce xs =
        match xs with
        | LazyList.Cons(x, LazyList.Cons(y, ys)) -> LazyList.consDelayed (x + y) (fun () -> pairReduce ys)
        | LazyList.Cons(x, LazyList.Nil) -> LazyList.cons x LazyList.empty
        | LazyList.Nil -> LazyList.empty

    let rec inf = LazyList.consDelayed 0 (fun () -> LazyList.map (fun x -> x + 1) inf)

    let countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators (seq: seq<'a>) =
        let enumerator() =
            let disposed = ref false in
            let endReached = ref false in
            let ie = seq.GetEnumerator() in
            numActiveEnumerators := !numActiveEnumerators + 1

            { new System.Collections.Generic.IEnumerator<'a> with
                member x.Current =
                    if !endReached then
                        failwith "end reached in Generic.IEnumerator.Current"

                    if !disposed then
                        failwith "disposed in Generic.IEnumerator.Current"

                    ie.Current

                member x.Dispose() =
                    if !disposed then
                        failwith "disposed in Generic.IEnumerator.Dispose"

                    numActiveEnumerators := !numActiveEnumerators - 1
                    disposed := true
                    ie.Dispose()
              interface System.Collections.IEnumerator with
                  member x.MoveNext() =
                      if !endReached then
                          failwith "end reached in IEnumerator.MoveNext"

                      if !disposed then
                          failwith "disposed in IEnumerator.MoveNext"

                      endReached := not(ie.MoveNext())
                      not !endReached

                  member x.Current =
                      if !endReached then
                          failwith "end reached in IEnumerator.Current"

                      if !disposed then
                          failwith "disposed in IEnumerator.Current"

                      box ie.Current

                  member x.Reset() =
                      ie.Reset() } in

        { new seq<'a> with
            member x.GetEnumerator() =
                enumerator()
          interface System.Collections.IEnumerable with
              member x.GetEnumerator() =
                  (enumerator() :> _) }

    let countEnumeratorsAndCheckedDisposedAtMostOnceAtEnd numActiveEnumerators (seq: seq<'a>) =
        let enumerator() =
            numActiveEnumerators := !numActiveEnumerators + 1
            let disposed = ref false in
            let endReached = ref false in
            let ie = seq.GetEnumerator() in

            { new System.Collections.Generic.IEnumerator<'a> with
                member x.Current =
                    if !endReached then
                        failwith "end reached in Generic.IEnumerator.Current"

                    if !disposed then
                        failwith "disposed in Generic.IEnumerator.Current"

                    ie.Current

                member x.Dispose() =
                    if (not !endReached) then
                        failwith "end reached in Generic.IEnumerator.Dispose"

                    if !disposed then
                        failwith "disposed in Generic.IEnumerator.Dispose"

                    numActiveEnumerators := !numActiveEnumerators - 1
                    disposed := true
                    ie.Dispose()
              interface System.Collections.IEnumerator with
                  member x.MoveNext() =
                      if !endReached then
                          failwith "end reached in IEnumerator.MoveNext"

                      if !disposed then
                          failwith "disposed in IEnumerator.MoveNext"

                      endReached := not(ie.MoveNext())
                      not !endReached

                  member x.Current =
                      if !endReached then
                          failwith "end reached in IEnumerator.Current"

                      if !disposed then
                          failwith "disposed in IEnumerator.Current"

                      box ie.Current

                  member x.Reset() =
                      ie.Reset() } in

        { new seq<'a> with
            member x.GetEnumerator() =
                enumerator()
          interface System.Collections.IEnumerable with
              member x.GetEnumerator() =
                  (enumerator() :> _) }

    [<Tests>]
    let testLazyList =

        let nats = LazyList.unfold (fun z -> Some(z, z + 1)) 0
        let rec diverge() = diverge()

        testList "LazyList" [

                              test "test2398984: LazyList.toSeq" {
                                  let l = LazyList.ofList [ 1; 2; 3 ] in
                                  let res = ref 2 in

                                  for i in (LazyList.toSeq l) do
                                      res := !res + i

                                  Expect.equal "LazyList.toSeq" 8 !res
                              }

                              test "test2398984: LazyList.toSeq 2" {
                                  let l = LazyList.ofList [ 1; 2; 3 ] in
                                  let res = ref 2 in

                                  for i in LazyList.toSeq l do
                                      res := !res + i

                                  Expect.equal "LazyList.toSeq" 8 !res
                              }

                              test "test2398994: foreach, LazyList.toSeq" {
                                  let l = LazyList.ofList [ 1; 2; 3 ] in
                                  let res = ref 2 in
                                  Seq.iter (fun i -> res := !res + i) (LazyList.toSeq l)
                                  Expect.equal "LazyList.toSeq" 8 !res
                              }

                              test "se1" { Expect.isTrue "isEmpty" <| LazyList.isEmpty LazyList.empty }
                              test "se2" {
                                  Expect.isFalse "cons"
                                  <| LazyList.isEmpty(LazyList.cons 1 LazyList.empty)
                              }
                              test "se3" { Expect.isFalse "repeat" <| LazyList.isEmpty(LazyList.repeat 1) }
                              test "se4" {
                                  Expect.isFalse "unfold"
                                  <| LazyList.isEmpty(LazyList.unfold (fun z -> Some(z, z + 1)) 0)
                              }

                              test "seq1" { Expect.equal "cons" 1 <| LazyList.head(LazyList.cons 1 LazyList.empty) }
                              test "seq2" {
                                  Expect.equal "cons" 1
                                  <| LazyList.head(LazyList.cons 1 (LazyList.cons 2 LazyList.empty))
                              }
                              test "seq3" {
                                  Expect.equal "cons" 2
                                  <| LazyList.head(LazyList.tail(LazyList.cons 1 (LazyList.cons 2 LazyList.empty)))
                              }

                              test "tryHead empty" { Expect.isNone "tryHead" <| LazyList.tryHead LazyList.empty }
                              test "tryHead seq1" { Expect.equal "tryHead" 1 (LazyList.tryHead(LazyList.cons 1 LazyList.empty)).Value }
                              test "tryHead seq2" {
                                  Expect.equal "tryHead" 1 (LazyList.tryHead(LazyList.cons 1 (LazyList.cons 2 LazyList.empty))).Value
                              }

                              test "tryTail empty" { Expect.isNone "tryTail" (LazyList.tryTail LazyList.empty) }
                              test "tryTail seq1" {
                                  Expect.isTrue "tryTail"
                                  <| LazyList.isEmpty (LazyList.tryTail(LazyList.cons 1 LazyList.empty)).Value
                              }
                              test "tryTail seq2" {
                                  Expect.equal "tryTail" [ 2 ]
                                  <| LazyList.toList((LazyList.tryTail(LazyList.cons 1 (LazyList.cons 2 LazyList.empty))).Value)
                              }
                              test "tryTail seq3" {
                                  Expect.equal "tryTail" [ 2; 3 ]
                                  <| LazyList.toList(
                                      (LazyList.tryTail(LazyList.cons 1 (LazyList.cons 2 (LazyList.cons 3 LazyList.empty))))
                                          .Value
                                  )
                              }

                              test "take1" {
                                  Expect.equal "take1" [ 0; 1; 2; 3 ]
                                  <| LazyList.toList(LazyList.take 4 nats)
                              }
                              test "skip 4" { Expect.equal "drop1" 4 <| LazyList.head(LazyList.skip 4 nats) }
                              test "skip 0" { Expect.equal "drop1" 0 <| LazyList.head(LazyList.skip 0 nats) }

                              test "tryTake empty" { Expect.isNone "tryTake" <| LazyList.tryTake 4 LazyList.empty }
                              test "tryTake0" {
                                  Expect.isTrue "tryTake0"
                                  <| LazyList.isEmpty (LazyList.tryTake 0 LazyList.empty).Value
                              }
                              test "tryTake1" {
                                  Expect.equal "tryTake1" [ 0 ]
                                  <| LazyList.toList (LazyList.tryTake 1 nats).Value
                              }
                              test "tryTake4" {
                                  Expect.equal "tryTake4" [ 0; 1; 2; 3 ]
                                  <| LazyList.toList (LazyList.tryTake 4 nats).Value
                              }
                              test "tryTake4 from list of 3" {
                                  Expect.equal "" [ 0; 1; 2 ]
                                  <| LazyList.toList (LazyList.tryTake 4 (LazyList.tryTake 3 nats).Value).Value
                              }

                              test "trySkip 0" { Expect.equal "" 0 <| LazyList.head (LazyList.trySkip 0 nats).Value }
                              test "trySkip 4" { Expect.equal "" 4 <| LazyList.head (LazyList.trySkip 4 nats).Value }
                              test "trySkip -1" { Expect.isNone "" <| LazyList.trySkip -1 nats }
                              test "trySkip 4 from list of 3" { Expect.isNone "" <| LazyList.trySkip 4 (LazyList.tryTake 3 nats).Value }

                              test "tryUncons empty" { Expect.isNone "" <| LazyList.tryUncons LazyList.empty }

                              test "tryUncons 1" {
                                  let x, y = (LazyList.tryUncons(LazyList.take 1 nats)).Value
                                  Expect.equal "tryUncons" (0, true) (x, (LazyList.isEmpty y))
                              }

                              test "tryUncons take 2" {
                                  let x2, y2 = (LazyList.tryUncons(LazyList.take 2 nats)).Value
                                  Expect.equal "tryUncons" (0, [ 1 ]) (x2, (LazyList.toList y2))
                              }

                              test "tryUncons take 3" {
                                  let x3, y3 = (LazyList.tryUncons(LazyList.take 3 nats)).Value
                                  Expect.equal "tryUncons" (0, [ 1; 2 ]) (x3, (LazyList.toList y3))
                              }

                              test "uncons 1" {
                                  let xa, ya = LazyList.uncons(LazyList.take 1 nats)
                                  Expect.equal "uncons" (0, true) (xa, (LazyList.isEmpty ya))
                              }

                              test "uncons take 2" {
                                  let xa2, ya2 = LazyList.uncons(LazyList.take 2 nats)
                                  Expect.equal "uncons" (0, [ 1 ]) (xa2, (LazyList.toList ya2))
                              }

                              test "uncons take 3" {
                                  let xa3, ya3 = LazyList.uncons(LazyList.take 3 nats)
                                  Expect.equal "uncons" (0, [ 1; 2 ]) (xa3, (LazyList.toList ya3))
                              }

                              test "mapAccum" {
                                  let ll = LazyList.ofList [ -5 .. -1 ]
                                  let expected = (15, [ 5; 4; 3; 2; 1 ])
                                  let x, y = LazyList.mapAccum (fun a b -> let c = abs b in (a + c, c)) 0 ll
                                  Expect.equal "mapAccum" expected (x, (LazyList.toList y))
                              }

                              test "fold" {
                                  let ll = LazyList.ofList [ -5 .. -1 ]
                                  let expected = 15

                                  Expect.equal "fold" expected
                                  <| LazyList.fold (fun a b -> a + abs b) 0 ll
                              }

                              test "repeat" {
                                  Expect.equal "take repeat" [ 1; 1; 1; 1 ]
                                  <| LazyList.toList(LazyList.take 4 (LazyList.repeat 1))
                              }
                              test "append" {
                                  Expect.equal "append" [ 77; 0; 1; 2 ]
                                  <| LazyList.toList(LazyList.take 4 (LazyList.append (LazyList.cons 77 (LazyList.empty)) nats))
                              }
                              test "zip" {
                                  Expect.equal "zip" [ 0, 6; 1, 7; 2, 8 ]
                                  <| LazyList.toList(LazyList.take 3 (LazyList.zip nats (LazyList.skip 6 nats)))
                              }
                              test "firstS" {
                                  Expect.equal "tryFind" (Some 8)
                                  <| LazyList.tryFind (fun x -> x >= 8) nats
                              }
                              test "firstN" {
                                  Expect.isNone "take"
                                  <| LazyList.tryFind (fun x -> x >= 8) (LazyList.take 5 nats)
                              }
                              test "find S" { Expect.equal "find" 8 <| LazyList.find (fun x -> x >= 8) nats }

                              test "find N" {
                                  Expect.throwsT<KeyNotFoundException> "find N" (fun () ->
                                      LazyList.find (fun x -> x >= 8) (LazyList.take 5 nats) |> ignore)
                              }

                              test "consfA" {
                                  Expect.equal "consDelayed" 1
                                  <| LazyList.head(LazyList.consDelayed 1 diverge)
                              }
                              test "consfB" {
                                  Expect.isTrue "divergence" (let ss = LazyList.tail(LazyList.consDelayed 1 diverge) in true)
                              } (* testing for lazy divergence *)
                              test "dropDiverge1" {
                                  Expect.isTrue "divergence" (let ss = LazyList.skip 1 (LazyList.consDelayed 1 diverge) in true)
                              } (* testing for lazy divergence *)
                              test "dropDiverge0" {
                                  Expect.isTrue "divergence" (let ss = LazyList.skip 0 (LazyList.delayed(fun () -> failwith "errors")) in true)
                              } (* testing for lazy divergence *)

                              test "takedrop" {
                                  Expect.equal "takedrop" [ 4; 5; 6 ]
                                  <| LazyList.toList(LazyList.take 3 (LazyList.skip 4 nats))
                              }
                              test "filter" {
                                  Expect.equal "filter" [ 0; 2; 4; 6 ]
                                  <| LazyList.toList(LazyList.take 4 (LazyList.filter (fun x -> x % 2 = 0) nats))
                              }
                              test "map" {
                                  Expect.equal "map" [ 1; 2; 3; 4 ]
                                  <| LazyList.toList(LazyList.take 4 (LazyList.map (fun x -> x + 1) nats))
                              }
                              test "map2" {
                                  Expect.equal "map2" [ 0 * 1; 1 * 2; 2 * 3; 3 * 4 ]
                                  <| LazyList.toList(LazyList.take 4 (LazyList.map2 (fun x y -> x * y) nats (LazyList.tail nats)))
                              }

                              test "array take6" {
                                  Expect.equal "array" (LazyList.toList(LazyList.take 6 nats))
                                  <| Array.toList(LazyList.toArray(LazyList.take 6 nats))
                              }
                              test "array list" {
                                  Expect.equal "array" (LazyList.toList(LazyList.ofArray [| 1; 2; 3; 4 |]))
                                  <| LazyList.toList(LazyList.ofList [ 1; 2; 3; 4 ])
                              }

                              test "equalsWith" {
                                  Expect.isTrue "equalsWith"
                                  <| LazyList.equalsWith (=) (LazyList.ofList [ 1; 2; 3; 4; 5 ]) (LazyList.ofList [ 1..5 ])
                              }
                              test "compareWith" {
                                  Expect.equal "compareWith" -1
                                  <| LazyList.compareWith Unchecked.compare (LazyList.ofList [ 1; 2; 3; 4 ]) (LazyList.ofList [ 1..5 ])
                              }

                              // This checks that LazyList.map, LazyList.length etc. are tail recursive
                              test "LazyList.length 100" { Expect.equal "length" 100 (LazyList.ofSeq(Seq.init 100 (fun c -> c)) |> LazyList.length) }
                              test "LazyList.length 1000000" {
                                  Expect.equal "length" 1000000 (LazyList.ofSeq(Seq.init 1000000 (fun c -> c)) |> LazyList.length)
                              }
                              test "LazyList.length 0" { Expect.equal "length" 0 (LazyList.ofSeq(Seq.init 0 (fun c -> c)) |> LazyList.length) }
                              test "LazyList.map" {
                                  Expect.equal
                                      "map"
                                      1000000
                                      (LazyList.map (fun x -> x + 1) (LazyList.ofSeq(Seq.init 1000000 (fun c -> c)))
                                       |> Seq.length)
                              }
                              test "LazyList.filter" {
                                  Expect.equal
                                      "filter"
                                      500000
                                      (LazyList.filter (fun x -> x % 2 = 0) (LazyList.ofSeq(Seq.init 1000000 (fun c -> c)))
                                       |> Seq.length)
                              }
                              test "LazyList.iter 0" {
                                  Expect.equal
                                      "iter"
                                      0
                                      (let count = ref 0 in
                                       LazyList.iter (fun x -> incr count) (LazyList.ofSeq(Seq.init 0 (fun c -> c)))
                                       !count)
                              }
                              test "LazyList.iter" {
                                  Expect.equal
                                      "iter"
                                      1000000
                                      (let count = ref 0 in
                                       LazyList.iter (fun x -> incr count) (LazyList.ofSeq(Seq.init 1000000 (fun c -> c)))
                                       !count)
                              }
                              test "LazyList.toList" {
                                  Expect.equal
                                      "toList"
                                      200000
                                      (LazyList.toList(LazyList.ofSeq(Seq.init 200000 (fun c -> c)))
                                       |> Seq.length)
                              }
                              test "LazyList.toArray" {
                                  Expect.equal
                                      "toArray"
                                      200000
                                      (LazyList.toArray(LazyList.ofSeq(Seq.init 200000 (fun c -> c)))
                                       |> Seq.length)
                              }

                              // check exists on an infinite stream terminates
                              test "IEnumerableTest.exists 1" {
                                  Expect.isTrue "exists" (Seq.exists ((=) "a") (LazyList.repeat "a" |> LazyList.toSeq))
                              }
                              // test a succeeding 'exists' on a concat of an infinite number of finite streams terminates
                              test "IEnumerableTest.exists 2" {
                                  Expect.isTrue "exists" (Seq.exists ((=) "a") (Seq.concat(LazyList.repeat [| "a"; "b" |] |> LazyList.toSeq)))
                              }
                              // test a succeeding 'exists' on a concat of an infinite number of infinite streams terminates
                              test "IEnumerableTest.exists 3" {
                                  Expect.isTrue
                                      "exists"
                                      (Seq.exists
                                          ((=) "a")
                                          (Seq.concat(
                                              LazyList.repeat(LazyList.repeat "a" |> LazyList.toSeq)
                                              |> LazyList.toSeq
                                          )))
                              }
                              // test a failing for_all on an infinite stream terminates
                              test "IEnumerableTest.exists 4" {
                                  Expect.isFalse "exists" (Seq.forall ((=) "a" >> not) (LazyList.repeat "a" |> LazyList.toSeq))
                              }
                              // test a failing for_all on a concat of an infinite number of finite streams terminates
                              test "IEnumerableTest.exists 5" {
                                  Expect.isFalse "exists" (Seq.forall ((=) "a" >> not) (Seq.concat(LazyList.repeat [| "a"; "b" |] |> LazyList.toSeq)))
                              }
                              test "IEnumerableTest.append, infinite, infinite, then take 2" {
                                  Expect.equal
                                      "append, infinite, infinite, then take"
                                      [ "a"; "a" ]
                                      (Seq.take 2 (Seq.append (LazyList.repeat "a" |> LazyList.toSeq) (LazyList.repeat "b" |> LazyList.toSeq))
                                       |> Seq.toList)
                              }

                              // test exists on an infinite stream terminates
                              test "IEnumerableTest.exists 6" {
                                  let numActiveEnumerators = ref 0

                                  Expect.isTrue
                                      "exists"
                                      (Seq.exists
                                          ((=) "a")
                                          (LazyList.repeat "a"
                                           |> LazyList.toSeq
                                           |> countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators))

                                  Expect.equal "<dispoal>" 0 !numActiveEnumerators
                              }

                              // test a succeeding 'exists' on a concat of an infinite number of finite streams terminates
                              test "IEnumerableTest.exists 7" {
                                  let numActiveEnumerators = ref 0

                                  Expect.isTrue
                                      ""
                                      (Seq.exists
                                          ((=) "a")
                                          (Seq.concat(
                                              LazyList.repeat [| "a"; "b" |]
                                              |> LazyList.toSeq
                                              |> countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators
                                          )))

                                  Expect.equal "<dispoal>" 0 !numActiveEnumerators
                              }

                              // test a succeeding 'exists' on a concat of an infinite number of infinite streams terminates
                              test "IEnumerableTest.exists 8" {
                                  let numActiveEnumerators = ref 0

                                  Expect.isTrue
                                      ""
                                      (Seq.exists
                                          ((=) "a")
                                          (Seq.concat(
                                              LazyList.repeat(LazyList.repeat "a" |> LazyList.toSeq)
                                              |> LazyList.toSeq
                                              |> countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators
                                          )))

                                  Expect.equal "<dispoal>" 0 !numActiveEnumerators
                              }

                              // test a failing for_all on an infinite stream terminates
                              test "IEnumerableTest.exists 9" {
                                  let numActiveEnumerators = ref 0

                                  Expect.isFalse
                                      "exists"
                                      (Seq.forall
                                          ((=) "a" >> not)
                                          (LazyList.repeat "a"
                                           |> LazyList.toSeq
                                           |> countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators))

                                  Expect.equal "" 0 !numActiveEnumerators
                              }

                              // check a failing for_all on a concat of an infinite number of finite streams terminates
                              test "IEnumerableTest.exists 10" {
                                  let numActiveEnumerators = ref 0

                                  Expect.isFalse
                                      "exists"
                                      (Seq.forall
                                          ((=) "a" >> not)
                                          (Seq.concat(
                                              LazyList.repeat [| "a"; "b" |]
                                              |> LazyList.toSeq
                                              |> countEnumeratorsAndCheckedDisposedAtMostOnce numActiveEnumerators
                                          )))

                                  Expect.equal "exists" 0 !numActiveEnumerators
                              }

                              test "IEnumerableTest.append, repeat" {
                                  let numActiveEnumerators = ref 0

                                  Expect.equal
                                      ""
                                      [ "a"; "a" ]
                                      (Seq.take 2 (Seq.append (LazyList.repeat "a" |> LazyList.toSeq) (LazyList.repeat "b" |> LazyList.toSeq))
                                       |> countEnumeratorsAndCheckedDisposedAtMostOnceAtEnd numActiveEnumerators
                                       |> Seq.toList)

                                  Expect.equal "exists" 0 !numActiveEnumerators
                              }

                              test "pairReduce" {
                                  Expect.equal "pairReduce" [ 1; 5; 9; 13; 17; 21; 25; 29; 33; 37 ]
                                  <| LazyList.toList(LazyList.take 10 (pairReduce inf))
                              }

                              test "scan 3" { Expect.equal "scan" [ 0; 1; 3 ] (LazyList.scan (+) 0 (LazyList.ofList [ 1; 2 ]) |> LazyList.toList) }
                              test "scan 0" { Expect.equal "scan" [ 0 ] (LazyList.scan (+) 0 (LazyList.ofList []) |> LazyList.toList) } ]

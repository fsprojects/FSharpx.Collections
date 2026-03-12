namespace FSharpx.Collections.Tests

open FSharpx.Collections
open FSharpx.Collections.Tests.Properties
open Expecto
open Expecto.Flip

module ListExtensionsTests =

    [<Tests>]
    let testListExtensions =
        testList
            "ListExtensions"
            [

              test "test List_split correctly breaks the list on the specified predicate" {
                  let str = List.ofSeq "Howdy! Want to play?"
                  let expected = (List.ofSeq "Howdy!", List.ofSeq " Want to play?")
                  Expect.equal "split" expected <| List.split (fun c -> c = ' ') str
              }

              test "test List_splitAt correctly breaks the list on the specified index" {
                  let str = List.ofSeq "Howdy! Want to play?"
                  let expected = (List.ofSeq "Howdy!", List.ofSeq " Want to play?")
                  Expect.equal "splitAt" expected <| List.splitAt 6 str
              }

              test "Can splitAt 3" {
                  let list = [ 1..5 ]
                  let expected = [ 1; 2; 3 ], [ 4; 5 ]
                  Expect.equal "splitAt" expected <| List.splitAt 3 list
              }

              test "Can split at 3" {
                  let list = [ 1..5 ]
                  let expected = [ 1; 2 ], [ 3; 4; 5 ]
                  Expect.equal "split" expected <| List.split ((=) 3) list
              }

              test "Can split at 0" {
                  let l1, l2 = List.split ((=) 0) [ 1..5 ]
                  Expect.equal "split" [ 1..5 ] l1
                  Expect.equal "split" [] l2
              }

              test "test List_span correctly breaks the list on the specified predicate" {
                  let str = List.ofSeq "Howdy! Want to play?"
                  let expected = (List.ofSeq "Howdy!", List.ofSeq " Want to play?")
                  Expect.equal "span" expected <| List.span (fun c -> c <> ' ') str
              }

              test "lift2" {
                  Expect.equal "lift2" [ 0; 2; 1; 3 ]
                  <| List.lift2 (+) [ 0; 1 ] [ 0; 2 ]
              }

              test "mapAccum" {
                  let list = [ -5 .. -1 ]
                  let expected = (15, [ 5; 4; 3; 2; 1 ])

                  Expect.equal "mapAccum" expected
                  <| List.mapAccum (fun a b -> let c = abs b in (a + c, c)) 0 list
              }

              test "Should be able to merge to lists" {
                  let a, b = [ 1; 2; 3; 4; 5 ], [ 6; 7; 8; 9; 10 ]

                  Expect.equal "merge" [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ]
                  <| List.mergeBy id a b
              }

              test "Should be able to merge two lists II" {
                  let a, b = [ 1; 2; 3; 4; 5 ], [ 6; 7; 8; 9; 10 ]

                  Expect.equal "merge" [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ]
                  <| List.merge a b
              }

              test "I should be able to transpose a list" {
                  let a = [ [ 1; 2; 3 ]; [ 4; 5; 6 ] ]
                  let expected = [ [ 1; 4 ]; [ 2; 5 ]; [ 3; 6 ] ]
                  Expect.equal "transpose" expected (a |> List.transpose)
              }

              test "singleton creates a one-element list" { Expect.equal "singleton" [ 42 ] (List.singleton 42) }

              test "cons prepends an element" { Expect.equal "cons" [ 1; 2; 3 ] (List.cons 1 [ 2; 3 ]) }

              test "findExactlyOne returns the sole matching element" {
                  Expect.equal "findExactlyOne" 3 (List.findExactlyOne ((=) 3) [ 1; 2; 3; 4; 5 ])
              }

              test "findExactlyOne throws when no element matches" {
                  Expect.throws "findExactlyOne no match" (fun () -> List.findExactlyOne ((=) 99) [ 1; 2; 3 ] |> ignore)
              }

              test "findExactlyOne throws when multiple elements match" {
                  Expect.throws "findExactlyOne multiple" (fun () -> List.findExactlyOne ((=) 1) [ 1; 1; 2 ] |> ignore)
              }

              test "skip removes first n elements" { Expect.equal "skip" [ 3; 4; 5 ] (List.skip 2 [ 1; 2; 3; 4; 5 ]) }

              test "skip 0 returns whole list" { Expect.equal "skip 0" [ 1; 2; 3 ] (List.skip 0 [ 1; 2; 3 ]) }

              test "take returns first n elements" { Expect.equal "take" [ 1; 2 ] (List.take 2 [ 1; 2; 3; 4; 5 ]) }

              test "take 0 returns empty list" { Expect.equal "take 0" [] (List.take 0 [ 1; 2; 3 ]) }

              test "skipWhile skips leading elements satisfying predicate" {
                  Expect.equal "skipWhile" [ 3; 4; 5 ] (List.skipWhile (fun x -> x < 3) [ 1; 2; 3; 4; 5 ])
              }

              test "skipUntil skips until predicate is satisfied" {
                  Expect.equal "skipUntil" [ 3; 4; 5 ] (List.skipUntil (fun x -> x = 3) [ 1; 2; 3; 4; 5 ])
              }

              test "takeWhile takes leading elements satisfying predicate" {
                  Expect.equal "takeWhile" [ 1; 2 ] (List.takeWhile (fun x -> x < 3) [ 1; 2; 3; 4; 5 ])
              }

              test "takeUntil takes until predicate is satisfied" {
                  Expect.equal "takeUntil" [ 1; 2 ] (List.takeUntil (fun x -> x = 3) [ 1; 2; 3; 4; 5 ])
              }

              test "groupNeighboursBy groups consecutive equal keys" {
                  let result = List.groupNeighboursBy id [ 1; 1; 2; 2; 1 ]
                  Expect.equal "groupNeighboursBy" [ (1, [ 1; 1 ]); (2, [ 2; 2 ]); (1, [ 1 ]) ] result
              }

              test "groupNeighboursBy on empty list" { Expect.equal "groupNeighboursBy empty" [] (List.groupNeighboursBy id []) }

              test "mapIf maps elements matching predicate, leaves others unchanged" {
                  let result = List.mapIf (fun x -> x % 2 = 0) ((*) 10) [ 1; 2; 3; 4; 5 ]
                  Expect.equal "mapIf" [ 1; 20; 3; 40; 5 ] result
              }

              test "catOptions extracts Some values from list of options" {
                  Expect.equal "catOptions" [ 1; 3 ] (List.catOptions [ Some 1; None; Some 3; None ])
              }

              test "catOptions on all-None list" { Expect.equal "catOptions all-None" [] (List.catOptions [ None; None ]) }

              test "choice1s extracts Choice1Of2 values" {
                  let xs = [ Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" ]
                  Expect.equal "choice1s" [ 1; 2 ] (List.choice1s xs)
              }

              test "choice2s extracts Choice2Of2 values" {
                  let xs = [ Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" ]
                  Expect.equal "choice2s" [ "a"; "b" ] (List.choice2s xs)
              }

              test "partitionChoices separates Choice1Of2 and Choice2Of2" {
                  let xs = [ Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" ]
                  let c1s, c2s = List.partitionChoices xs
                  Expect.equal "partitionChoices c1s" [ 1; 2 ] c1s
                  Expect.equal "partitionChoices c2s" [ "a"; "b" ] c2s
              }

              test "equalsWith returns true for element-wise equal lists" {
                  Expect.isTrue "equalsWith true" (List.equalsWith (=) [ 1; 2; 3 ] [ 1; 2; 3 ])
              }

              test "equalsWith returns false for unequal lists" { Expect.isFalse "equalsWith false" (List.equalsWith (=) [ 1; 2; 3 ] [ 1; 2; 4 ]) }

              test "equalsWith returns false for lists of different length" {
                  Expect.isFalse "equalsWith length" (List.equalsWith (=) [ 1; 2 ] [ 1; 2; 3 ])
              } ]

    [<Tests>]
    let propertyTestListExtensions =
        let fill (total: int) (elem: 'a) (list: 'a list) =
            let padded = List.fill total elem list

            if total > 0 && total > List.length list then
                List.length padded = total
            else
                List.length padded = List.length list

        let pad (total: int) (elem: 'a) (list: 'a list) =
            let padded = List.pad total elem list

            if total > 0 then
                List.length padded = total + List.length list
            else
                List.length padded = List.length list

        testList
            "ListExtensions property tests"
            [ testPropertyWithConfig config10k "fill a list" fill

              testPropertyWithConfig config10k "pad a list" pad ]

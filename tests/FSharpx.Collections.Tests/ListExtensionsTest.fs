namespace FSharpx.Collections.Tests

open FSharpx.Collections
open FSharpx.Collections.Tests.Properties
open Expecto
open Expecto.Flip

module ListExtensionsTests =

    [<Tests>]
    let testListExtensions =
        testList "ListExtensions" [

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

        testList "ListExtensions property tests" [ testPropertyWithConfig config10k "fill a list" fill

                                                   testPropertyWithConfig config10k "pad a list" pad ]

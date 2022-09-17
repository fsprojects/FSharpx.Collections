namespace FSharpx.Collections.Tests

// originally from https://bitbucket.org/colinbul/fsharpent

open FSharpx.Collections
open Expecto
open Expecto.Flip

module MapTests =
    let data =
        [
            (1, 1)
            (2, 2)
            (3, 3)
            (4, 4)
            (5, 5)
            (6, 6)
            (7, 7)
            (8, 8)
            (9, 9)
            (10, 10)
        ]
        |> Map.ofList

    [<Tests>]
    let testMap =
        testList "Map" [

            test "I should be able to choose elements from a map" {
                let expected = [ (2, 2); (4, 4); (6, 6); (8, 8); (10, 10) ] |> Map.ofList

                Expect.equal "choose" expected
                <| Map.choose (fun k _ -> if k % 2 = 0 then Some k else None) data
            }

            test "I should be able to remove elements by key" {
                let toRemove = [ 1; 2; 3; 4; 5 ]
                let expected = [ (6, 6); (7, 7); (8, 8); (9, 9); (10, 10) ] |> Map.ofList
                Expect.equal "removeMany" expected <| Map.removeMany toRemove data
            }

            test "I should be able to extract values from a map" {
                let expected = [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ] |> Seq.ofList
                Expect.sequenceEqual "values" expected <| Map.values data
            }

            test "I should be able to extract keys from a map" {
                let expected = [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ] |> Seq.ofList
                Expect.sequenceEqual "keys" expected <| Map.keys data
            }
        ]

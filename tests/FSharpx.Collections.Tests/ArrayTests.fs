namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module ArrayTests =

    let data = [|1.;2.;3.;4.;5.;6.;7.;8.;9.;10.|]

    [<Tests>]
    let testArray =
        testList "Array" [
            testCase "I should be able to part of an array to a target array" <| fun () ->
                let a, b = [|1;2;3;4;5|], [|10;11;12;13;14|]
                (Array.copyTo 0 2 a b) 
                Expect.equal "expect arrays equal" [|10;11;1;2;3|] b


            testCase "I should be able to convert a tuple to an array" <| fun () ->
                (1,2) |> Array.ofTuple
                |> (Expect.equal "expect arrays equal" [|1;2|])

            testCase "I should be able to convert an array to a tuple" <| fun () ->
                let result : (int*int) = [|1;2|] |> Array.toTuple
                result |> (Expect.equal "expect tuples equal" (1,2))

            testCase "I should be able to create a centered window from a seq" <| fun () ->
                let result = Array.centeredWindow 3 data
                let expected = [|
                                    [|1.;2.;3.;4.|]
                                    [|1.;2.;3.;4.;5.|]
                                    [|1.;2.;3.;4.;5.;6.|]
                                    [|1.;2.;3.;4.;5.;6.;7.|]
                                    [|2.;3.;4.;5.;6.;7.;8.|]
                                    [|3.;4.;5.;6.;7.;8.;9.|]
                                    [|4.;5.;6.;7.;8.;9.;10.|]
                                    [|5.;6.;7.;8.;9.;10.|]
                                    [|6.;7.;8.;9.;10.|]
                                    [|7.;8.;9.;10.|]
                                |]
                Expect.equal "expect arrays equal" expected result

            testCase "I should be able to compute the central moving average of a seq" <| fun () ->
                let result = Array.centralMovingAverage 3 data
                let expected = [|
                                    2.5; 3.; 3.5; 4.; 
                                    5.; 6.; 7.;
                                    7.5; 8.; 8.5
                                |]
                Expect.equal "expect arrays equal" expected result
        ]

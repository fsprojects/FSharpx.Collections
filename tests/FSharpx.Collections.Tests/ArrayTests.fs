namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module ArrayTests =

    let data = [| 1.; 2.; 3.; 4.; 5.; 6.; 7.; 8.; 9.; 10. |]

    [<Tests>]
    let testArray =
        testList
            "Array"
            [ test "I should be able to part of an array to a target array" {
                  let a, b = [| 1; 2; 3; 4; 5 |], [| 10; 11; 12; 13; 14 |]
                  Array.copyTo 0 2 a b
                  Expect.equal "expect arrays equal" [| 10; 11; 1; 2; 3 |] b
              }

              test "I should be able to convert a tuple to an array" {
                  (1, 2)
                  |> Array.ofTuple
                  |> (Expect.equal "expect arrays equal" [| 1; 2 |])
              }

              test "I should be able to convert an array to a tuple" {
                  let result: (int * int) = [| 1; 2 |] |> Array.toTuple
                  Expect.equal "expect tuples equal" (1, 2) result
              }

              test "I should be able to create a centered window from a seq" {
                  let expected =
                      [| [| 1.; 2.; 3.; 4. |]
                         [| 1.; 2.; 3.; 4.; 5. |]
                         [| 1.; 2.; 3.; 4.; 5.; 6. |]
                         [| 1.; 2.; 3.; 4.; 5.; 6.; 7. |]
                         [| 2.; 3.; 4.; 5.; 6.; 7.; 8. |]
                         [| 3.; 4.; 5.; 6.; 7.; 8.; 9. |]
                         [| 4.; 5.; 6.; 7.; 8.; 9.; 10. |]
                         [| 5.; 6.; 7.; 8.; 9.; 10. |]
                         [| 6.; 7.; 8.; 9.; 10. |]
                         [| 7.; 8.; 9.; 10. |] |]

                  Expect.equal "expect arrays equal" expected
                  <| Array.centeredWindow 3 data
              }

              test "I should be able to compute the central moving average of a seq" {
                  let expected = [| 2.5; 3.; 3.5; 4.; 5.; 6.; 7.; 7.5; 8.; 8.5 |]

                  Expect.equal "expect arrays equal" expected
                  <| Array.centralMovingAverage 3 data
              }

              test "nth returns element at index" { Expect.equal "nth" 3 (Array.nth 2 [| 1; 2; 3; 4; 5 |]) }

              test "setAt mutates element at index and returns array" {
                  let a = [| 1; 2; 3 |]
                  let result = Array.setAt 1 99 a
                  Expect.equal "setAt value" 99 result.[1]
                  Expect.isTrue "setAt same ref" (obj.ReferenceEquals(a, result))
              }

              test "findExactlyOne returns the sole matching element" {
                  Expect.equal "findExactlyOne" 3 (Array.findExactlyOne ((=) 3) [| 1; 2; 3; 4; 5 |])
              }

              test "findExactlyOne throws when no element matches" {
                  Expect.throwsT<System.ArgumentException> "findExactlyOne no match"
                  <| fun () -> Array.findExactlyOne ((=) 99) [| 1; 2; 3 |] |> ignore
              }

              test "centralMovingAverageOfOption handles None entries" {
                  let a = [| Some 1.0; None; Some 3.0 |]
                  let result = Array.centralMovingAverageOfOption 1 a
                  Expect.equal "centralMovingAverageOfOption" [| None; None; None |] result
              }

              test "centralMovingAverageOfOption all Some" {
                  let a = [| Some 1.0; Some 2.0; Some 3.0 |]
                  let result = Array.centralMovingAverageOfOption 1 a
                  Expect.equal "centralMovingAverageOfOption all Some" [| Some 1.5; Some 2.0; Some 2.5 |] result
              }

              test "catOptions extracts Some values from array of options" {
                  Expect.equal "catOptions" [| 1; 3 |] (Array.catOptions [| Some 1; None; Some 3; None |])
              }

              test "choice1s extracts Choice1Of2 values" {
                  let xs = [| Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" |]
                  Expect.equal "choice1s" [| 1; 2 |] (Array.choice1s xs)
              }

              test "choice2s extracts Choice2Of2 values" {
                  let xs = [| Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" |]
                  Expect.equal "choice2s" [| "a"; "b" |] (Array.choice2s xs)
              }

              test "partitionChoices separates Choice1Of2 and Choice2Of2" {
                  let xs = [| Choice1Of2 1; Choice2Of2 "a"; Choice1Of2 2; Choice2Of2 "b" |]
                  let c1s, c2s = Array.partitionChoices xs
                  Expect.equal "partitionChoices c1s" [| 1; 2 |] c1s
                  Expect.equal "partitionChoices c2s" [| "a"; "b" |] c2s
              }

              test "equalsWith returns true for element-wise equal arrays" {
                  Expect.isTrue "equalsWith true" (Array.equalsWith (=) [| 1; 2; 3 |] [| 1; 2; 3 |])
              }

              test "equalsWith returns false for unequal arrays" {
                  Expect.isFalse "equalsWith false" (Array.equalsWith (=) [| 1; 2; 3 |] [| 1; 2; 4 |])
              }

              test "equalsWith returns false for arrays of different length" {
                  Expect.isFalse "equalsWith length" (Array.equalsWith (=) [| 1; 2 |] [| 1; 2; 3 |])
              } ]

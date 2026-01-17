module NonEmptyListTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList
        "NonEmptyListTests"
        [ test "NonEmptyList works" {
              let xs = NonEmptyList.create "a" []

              Expect.equal (NonEmptyList.length xs) 1 "should have single entry"
              Expect.equal (NonEmptyList.head xs) "a" "should read head"

              let xs = xs |> NonEmptyList.cons "b" |> NonEmptyList.cons "c"

              Expect.equal (NonEmptyList.head xs) "c" "should update head"
              Expect.equal (NonEmptyList.length xs) 3 "should track length"
          } ]

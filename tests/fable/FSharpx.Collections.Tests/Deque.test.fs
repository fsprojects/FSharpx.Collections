module DequeTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList
        "DequeTests"
        [ test "NonEmptyList works" {
              let d = Deque.empty

              Expect.equal (Deque.isEmpty d) true "should be empty"

              let d = d |> Deque.conj "b" |> Deque.conj "a"

              Expect.equal (Deque.isEmpty d) false "should not be empty anymore"
              Expect.equal (Deque.head d) "b" "b should be at top"
              Expect.equal (Deque.length d) 2 "Should have length 2"
          } ]

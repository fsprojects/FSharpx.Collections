module QueueTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList "QueueTests" [
        test "Queue works" {
            let q = Queue.empty

            Expect.equal (Queue.isEmpty q) true "should start empty"

            let q = Queue.conj "a" q

            Expect.equal (Queue.isEmpty q) false "should no longer be empty"
            Expect.equal (Queue.tryHead q) (Some "a") "should read first element"

            let q = Queue.conj "b" q

            Expect.equal (Queue.tryHead q) (Some "a") "should preserve head"

            let q = Queue.conj "c" q

            Expect.equal (Queue.tryTail q) (Queue.empty |> Queue.conj "b" |> Queue.conj "c" |> Some) "should drop head"

            let popped, q = Queue.uncons q

            Expect.equal popped "a" "should pop in order"

            let popped, _ = Queue.uncons q

            Expect.equal popped "b" "should pop second"
        }
    ]

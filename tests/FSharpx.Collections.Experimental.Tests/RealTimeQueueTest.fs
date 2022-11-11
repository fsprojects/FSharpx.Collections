namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module RealTimeQueueTest =

    [<Tests>]
    let testRealTimeQueue =

        testList "Experimental RealTimeQueue" [
            test "RealTimeQueue.empty queue should be RealTimeQueue.empty" { RealTimeQueue.isEmpty RealTimeQueue.empty |> Expect.isTrue "" }

            test "it should allow to enqueue" {
                RealTimeQueue.empty
                |> RealTimeQueue.snoc 1
                |> RealTimeQueue.snoc 2
                |> RealTimeQueue.isEmpty
                |> Expect.isFalse ""
            }

            test "it should allow to dequeue" {
                RealTimeQueue.empty
                |> RealTimeQueue.snoc 1
                |> RealTimeQueue.tail
                |> RealTimeQueue.isEmpty
                |> Expect.isTrue ""
            }

            test "it should fail if there is no RealTimeQueue.head in the queue" {
                let ok = ref false

                try
                    RealTimeQueue.empty |> RealTimeQueue.head |> ignore
                with x when x = Exceptions.Empty ->
                    ok := true

                !ok |> Expect.isTrue ""
            }

            test "it should give None if there is no RealTimeQueue.head in the queue" {
                RealTimeQueue.empty |> RealTimeQueue.tryGetHead |> Expect.isNone ""
            }

            test "it should fail if there is no RealTimeQueue.tail the queue" {
                let ok = ref false

                try
                    RealTimeQueue.empty |> RealTimeQueue.tail |> ignore
                with x when x = Exceptions.Empty ->
                    ok := true

                !ok |> Expect.isTrue ""
            }

            test "it should give None if there is no RealTimeQueue.tail in the queue" {
                RealTimeQueue.empty |> RealTimeQueue.tryGetTail |> Expect.isNone ""
            }

            test "it should allow to get the RealTimeQueue.head from a queue" {
                RealTimeQueue.empty
                |> RealTimeQueue.snoc 1
                |> RealTimeQueue.snoc 2
                |> RealTimeQueue.head
                |> Expect.equal "" 1
            }

            test "it should allow to get the RealTimeQueue.head from a queue safely" {
                RealTimeQueue.empty
                |> RealTimeQueue.snoc 1
                |> RealTimeQueue.snoc 2
                |> RealTimeQueue.tryGetHead
                |> Expect.equal "" (Some 1)
            }

            test "it should allow to get the RealTimeQueue.tail from the queue" {
                RealTimeQueue.empty
                |> RealTimeQueue.snoc "a"
                |> RealTimeQueue.snoc "b"
                |> RealTimeQueue.snoc "c"
                |> RealTimeQueue.tail
                |> RealTimeQueue.head
                |> Expect.equal "" "b"
            }

            test "it should allow to get the RealTimeQueue.tail from a queue safely" {
                let value =
                    RealTimeQueue.empty
                    |> RealTimeQueue.snoc 1
                    |> RealTimeQueue.snoc 2
                    |> RealTimeQueue.tryGetTail

                value.Value |> RealTimeQueue.head |> Expect.equal "" 2
            }
        ]

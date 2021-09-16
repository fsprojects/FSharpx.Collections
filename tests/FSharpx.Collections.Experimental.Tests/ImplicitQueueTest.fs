namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module ImplicitQueueTest =

    [<Tests>]
    let testImplicitQueue =

        testList "Experimental ImplicitQueue" [
            test "ImplicitQueue.empty queue should be ImplicitQueue.empty" {
                ImplicitQueue.isEmpty ImplicitQueue.empty |> Expect.isTrue "" }

            test "it should allow to enqueue" {
                ImplicitQueue.empty |> ImplicitQueue.snoc 1 |> ImplicitQueue.snoc 2 |> ImplicitQueue.isEmpty |> Expect.isFalse "" }

            test "it should allow to dequeue" {
                ImplicitQueue.empty |> ImplicitQueue.snoc 1 |> ImplicitQueue.tail |> ImplicitQueue.isEmpty |> Expect.isTrue "" }

            test "it should fail if there is no ImplicitQueue.head in the queue" {
                let ok = ref false
                try
                    ImplicitQueue.empty |> ImplicitQueue.head |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no ImplicitQueue.head in the queue" {
                ImplicitQueue.empty |> ImplicitQueue.tryGetHead |> Expect.isNone "" }

            test "it should fail if there is no ImplicitQueue.tail the queue" {
                let ok = ref false
                try
                    ImplicitQueue.empty |> ImplicitQueue.tail |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no ImplicitQueue.tail in the queue" {
                ImplicitQueue.empty |> ImplicitQueue.tryGetTail |> Expect.isNone "" }

            test "it should allow to get the ImplicitQueue.head from a queue" {
                ImplicitQueue.empty |> ImplicitQueue.snoc 1 |> ImplicitQueue.snoc 2 |> ImplicitQueue.head |> Expect.equal "" 1 } 

            test "it should allow to get the ImplicitQueue.head from a queue safely" {
                ImplicitQueue.empty |> ImplicitQueue.snoc 1 |> ImplicitQueue.snoc 2 |> ImplicitQueue.tryGetHead |> Expect.equal "" (Some 1) } 

            test "it should allow to get the ImplicitQueue.tail from the queue" {
                ImplicitQueue.empty |> ImplicitQueue.snoc "a" |> ImplicitQueue.snoc "b" |> ImplicitQueue.snoc "c" |> ImplicitQueue.tail |> ImplicitQueue.head |> Expect.equal "" "b" } 

            test "it should allow to get the ImplicitQueue.tail from a queue safely" {
                let value = ImplicitQueue.empty |> ImplicitQueue.snoc 1 |> ImplicitQueue.snoc 2 |> ImplicitQueue.tryGetTail
                value.Value |> ImplicitQueue.head |> Expect.equal "" 2 } 
        ]
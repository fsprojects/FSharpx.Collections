namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module BootstrappedQueueTest =

    [<Tests>]
    let testBootstrappedQueue =

        testList "Experimental BootstrappedQueue" [
            test "BootstrappedQueue.empty queue should be BootstrappedQueue.empty" {
                BootstrappedQueue.isEmpty BootstrappedQueue.empty |> Expect.isTrue "" }

            test "it should allow to enqueue" {
                BootstrappedQueue.empty |> BootstrappedQueue.snoc 1 |> BootstrappedQueue.snoc 2 |> BootstrappedQueue.isEmpty |> Expect.isFalse "" }

            test "it should allow to dequeue" {
                BootstrappedQueue.empty |> BootstrappedQueue.snoc 1 |> BootstrappedQueue.tail |> BootstrappedQueue.isEmpty |> Expect.isTrue "" }

            test "it should fail if there is no BootstrappedQueue.head in the queue" {
                let ok = ref false
                try
                    BootstrappedQueue.empty |> BootstrappedQueue.head |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no BootstrappedQueue.head in the queue" {
                BootstrappedQueue.empty |> BootstrappedQueue.tryGetHead |> Expect.isNone "" }

            test "it should fail if there is no BootstrappedQueue.tail the queue" {
                let ok = ref false
                try
                    BootstrappedQueue.empty |> BootstrappedQueue.tail |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no BootstrappedQueue.tail in the queue" {
                BootstrappedQueue.empty |> BootstrappedQueue.tryGetTail |> Expect.isNone "" }

            test "it should allow to get the BootstrappedQueue.head from a queue" {
                BootstrappedQueue.empty |> BootstrappedQueue.snoc 1 |> BootstrappedQueue.snoc 2 |> BootstrappedQueue.head |> Expect.equal "" 1 } 

            test "it should allow to get the BootstrappedQueue.head from a queue safely" {
                BootstrappedQueue.empty |> BootstrappedQueue.snoc 1 |> BootstrappedQueue.snoc 2 |> BootstrappedQueue.tryGetHead |> Expect.equal "" (Some 1) } 

            test "it should allow to get the BootstrappedQueue.tail from the queue" {
                BootstrappedQueue.empty |> BootstrappedQueue.snoc "a" |> BootstrappedQueue.snoc "b" |> BootstrappedQueue.snoc "c" |> BootstrappedQueue.tail |> BootstrappedQueue.head |> Expect.equal "" "b" } 

            test "it should allow to get the BootstrappedQueue.tail from a queue safely" {
                let value = BootstrappedQueue.empty |> BootstrappedQueue.snoc 1 |> BootstrappedQueue.snoc 2 |> BootstrappedQueue.tryGetTail
                value.Value |> BootstrappedQueue.head |> Expect.equal "" 2 } 

            test "it should initialize from a list" {
                BootstrappedQueue.ofList [1..10] |> BootstrappedQueue.snoc 11 |> BootstrappedQueue.snoc 12 |> BootstrappedQueue.tail |> BootstrappedQueue.length |> Expect.equal "" 11 } 
        ]
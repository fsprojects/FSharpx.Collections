namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.BootstrappedQueue
open Expecto
open Expecto.Flip

module BootstrappedQueueTest =

    [<Tests>]
    let testBootstrappedQueue =

        testList "Experimental BootstrappedQueue" [
            test "empty queue should be empty" {
                isEmpty empty |> Expect.isTrue "" }

            test "it should allow to enqueue" {
                empty |> snoc 1 |> snoc 2 |> isEmpty |> Expect.isFalse "" }

            test "it should allow to dequeue" {
                empty |> snoc 1 |> tail |> isEmpty |> Expect.isTrue "" }

            test "it should fail if there is no head in the queue" {
                let ok = ref false
                try
                    empty |> head |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no head in the queue" {
                empty |> tryGetHead |> Expect.isNone "" }

            test "it should fail if there is no tail the queue" {
                let ok = ref false
                try
                    empty |> tail |> ignore
                with x when x = Exceptions.Empty -> ok := true
                !ok |> Expect.isTrue "" }

            test "it should give None if there is no tail in the queue" {
                empty |> tryGetTail |> Expect.isNone "" }

            test "it should allow to get the head from a queue" {
                empty |> snoc 1 |> snoc 2 |> head |> Expect.equal "" 1 } 

            test "it should allow to get the head from a queue safely" {
                empty |> snoc 1 |> snoc 2 |> tryGetHead |> Expect.equal "" (Some 1) } 

            test "it should allow to get the tail from the queue" {
                empty |> snoc "a" |> snoc "b" |> snoc "c" |> tail |> head |> Expect.equal "" "b" } 

            test "it should allow to get the tail from a queue safely" {
                let value = empty |> snoc 1 |> snoc 2 |> tryGetTail
                value.Value |> head |> Expect.equal "" 2 } 

            test "it should initialize from a list" {
                ofList [1..10] |> snoc 11 |> snoc 12 |> tail |> length |> Expect.equal "" 11 } 
        ]
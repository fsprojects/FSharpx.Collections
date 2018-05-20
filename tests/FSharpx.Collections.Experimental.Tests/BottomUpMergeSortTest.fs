namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.BottomUpMergeSort
open Expecto
open Expecto.Flip

module BottomUpMergeSortTest =

    [<Tests>]
    let testBottomUpMergeSort =

        testList "Experimental BottomUpMergeSort" [
            test "empty list should be empty" {
                empty |> isEmpty |> Expect.isTrue "" }

            test "empty list should be empty after sort" {
                sort empty |> Expect.equal "" [] } 

            test "singleton list should be the same after sort" {
                sort (singleton 1) |> Expect.equal "" [1] } 

            test "adding a element to an empty list" {
                empty |> add 1 |> sort |> Expect.equal "" [1] } 

            test "adding multiple elements to an empty list" {
                empty |> add 100 |> add 1 |> add 3 |> add 42 |> sort |> Expect.equal "" [1; 3; 42; 100] } 

            test "adding multiple strings to an empty list" {
                empty |> add "100" |> add "1" |> add "3" |> add "42" |> sort |> Expect.equal "" ["1"; "100"; "3"; "42"] } 
        ]
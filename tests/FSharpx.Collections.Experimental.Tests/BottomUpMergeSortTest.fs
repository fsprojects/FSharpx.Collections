namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module BottomUpMergeSortTest =

    [<Tests>]
    let testBottomUpMergeSort =

        testList "Experimental BottomUpMergeSort" [ test "empty list should be empty" {
                                                        BottomUpMergeSort.empty
                                                        |> BottomUpMergeSort.isEmpty
                                                        |> Expect.isTrue ""
                                                    }

                                                    test "empty list should be empty after sort" {
                                                        BottomUpMergeSort.sort BottomUpMergeSort.empty |> Expect.equal "" []
                                                    }

                                                    test "singleton list should be the same after sort" {
                                                        BottomUpMergeSort.sort(BottomUpMergeSort.singleton 1)
                                                        |> Expect.equal "" [ 1 ]
                                                    }

                                                    test "adding a element to an empty list" {
                                                        BottomUpMergeSort.empty
                                                        |> BottomUpMergeSort.add 1
                                                        |> BottomUpMergeSort.sort
                                                        |> Expect.equal "" [ 1 ]
                                                    }

                                                    test "adding multiple elements to an empty list" {
                                                        BottomUpMergeSort.empty
                                                        |> BottomUpMergeSort.add 100
                                                        |> BottomUpMergeSort.add 1
                                                        |> BottomUpMergeSort.add 3
                                                        |> BottomUpMergeSort.add 42
                                                        |> BottomUpMergeSort.sort
                                                        |> Expect.equal "" [ 1; 3; 42; 100 ]
                                                    }

                                                    test "adding multiple strings to an empty list" {
                                                        BottomUpMergeSort.empty
                                                        |> BottomUpMergeSort.add "100"
                                                        |> BottomUpMergeSort.add "1"
                                                        |> BottomUpMergeSort.add "3"
                                                        |> BottomUpMergeSort.add "42"
                                                        |> BottomUpMergeSort.sort
                                                        |> Expect.equal "" [ "1"; "100"; "3"; "42" ]
                                                    } ]

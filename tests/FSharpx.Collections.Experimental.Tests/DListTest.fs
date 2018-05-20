namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.DList
open Expecto
open Expecto.Flip

module DListTest =

    [<Tests>]
    let testDList =

        let expected = Join(Unit 0, Join(Unit 1, Join(Unit 2, Join(Unit 3, Unit 4, 2), 3), 4), 5)
        let expected2 = Join(Unit 1, Join(Unit 2, Join(Unit 3, Unit 4, 2), 3), 4)

        testList "Experimental DList" [
            test "test should verify empty is Nil" {
              empty<_> |> Expect.equal "" DList.Nil } 

            test "test length should return 5" {
              length expected |> Expect.equal "" 5 } 

            test "test ofSeq should create a DList from a seq" {
              let test = seq { for i in 0..4 -> i }
              DList.ofSeq test |> Expect.equal "" expected } 

            test "test ofSeq should create a DList from a list" {
              let test = [ for i in 0..4 -> i ]
              DList.ofSeq test |> Expect.equal "" expected } 

            test "test ofSeq should create a DList from an array" {
              let test = [| for i in 0..4 -> i |]
              DList.ofSeq test |> Expect.equal "" expected } 

            test "test cons should prepend 10 to the front of the original list" {
              cons 10 expected |> Expect.equal "" (Join(Unit 10, expected, 6)) } 

            test "test singleton should return a Unit containing the solo value" {
              singleton 1 |> Expect.equal "" (Unit 1) } 

            test "test cons should return a Unit when the tail is Nil" {
              cons 1 DList.empty |> Expect.equal "" (Unit 1) } 

            test "test subsequent cons should create a DList just as the constructor functions" {
              cons 0 (cons 1 (cons 2 (cons 3 (cons 4 empty)))) |> Expect.equal "" expected } 

            test "test append should join two DLists together" {
              append expected expected2 |> Expect.equal "" (Join(expected, expected2, 9)) } 

            test "test snoc should join DList and one element together" {
              snoc expected 5 |> Expect.equal "" (Join(expected, Unit 5, 6)) } 

            test "test head should return the first item in the DList" {
              head (append expected expected) |> Expect.equal "" 0 } 

            test "test tail should return all items except the head" {
              tail (append expected expected) |> Expect.equal "" (Join(cons 1 (cons 2 (cons 3 (cons 4 empty))), expected, 9)) } 

            test "test DList should respond to Seq functions such as map" {
              let testmap x = x*x
              let actual = Seq.map testmap (append expected expected)
              let expected = seq { yield 0; yield 1; yield 2; yield 3; yield 4; yield 0; yield 1; yield 2; yield 3; yield 4 } |> Seq.map testmap
              Expect.sequenceEqual ""  expected actual }
        ]
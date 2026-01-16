module PersistentVectorTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList
        "PersistentVectorTests"
        [ test "PersistentVector.empty works as expected" {
              let v = PersistentVector.empty

              Expect.equal (PersistentVector.isEmpty v) true "should be empty"
              Expect.equal (PersistentVector.length v) 0 "should have zero length"
          }

          test "PersistentVector.conj works as expected" {
              let v = PersistentVector.empty |> PersistentVector.conj "a"

              Expect.equal (PersistentVector.nth 0 v) "a" "should hold first value"

              let v = v |> PersistentVector.conj "b" |> PersistentVector.conj "c"

              Expect.equal (PersistentVector.nth 0 v) "a" "should preserve first element"
              Expect.equal (PersistentVector.nth 1 v) "b" "should add second element"
              Expect.equal (PersistentVector.nth 2 v) "c" "should add third element"
          }

          test "PersistentVector implements seq as expected" {
              let v =
                  PersistentVector.empty
                  |> PersistentVector.conj 1
                  |> PersistentVector.conj 4
                  |> PersistentVector.conj 25

              Expect.equal (Seq.toList v) [ 1; 4; 25 ] "should convert to list"
          }

          test "PersistentVector.map works as expected" {
              let v =
                  PersistentVector.ofSeq [ 1..4 ]
                  |> PersistentVector.map(fun x -> x * 2)

              Expect.equal (Seq.toList v) [ 2; 4; 6; 8 ] "should map over items"
          } ]

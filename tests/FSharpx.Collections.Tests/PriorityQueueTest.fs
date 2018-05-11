namespace FSharpx.Collections.Tests

open FSharpx.Collections.PriorityQueue
open Expecto
open Expecto.Flip

module PriorityQueueTests =
    let testPriorityQueue =

        testList "PriorityQueue" [
            test "empty queue should be empty" {
                let pq = empty false

                Expect.isTrue "empty" <| isEmpty pq
                Expect.isNone "empty" <| tryPeek pq
                Expect.isNone "empty" <| tryPop pq }

            test "After adding an element to the PQ it shouldn't be empty" {
                let pq = empty false |> insert 1
                Expect.isFalse "insert"  <| isEmpty pq }

            test "After adding an element to the PQ the element should be the smallest" {
                let pq = empty false |> insert 1

                Expect.equal "insert" (Some 1) <| tryPeek pq
                Expect.equal "insert" 1 <| peek pq }

            test "After adding an element to the PQ and popping it the PQ should be empty" {
                let pq = empty false |> insert 1

                let element,newPQ = pop pq
                Expect.equal "pop" 1 element
                Expect.isTrue "pop"  <| isEmpty newPQ

                let element,newPQ = (tryPop pq).Value
                Expect.equal "tryPop" 1 element
                Expect.isTrue "tryPop"  <| isEmpty newPQ }

            test "Adding multiple elements to the PQ should allow to pop the smallest" {
                let pq = empty false |> insert 1 |> insert 3 |> insert 0 |> insert 4 |> insert -3

                let element,newPQ = pop pq
                Expect.equal "pop" -3 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 0 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 1 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 3 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 4 element

                Expect.isTrue "pop"  <| isEmpty newPQ }

            test "Adding multiple elements to a MaxPriorityQueue should allow to pop the smallest" {
                let pq = empty true |> insert 1 |> insert 3 |> insert 0 |> insert 4 |> insert -3

                let element,newPQ = pop pq
                Expect.equal "pop" 4 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 3 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 1 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" 0 element

                let element,newPQ = pop newPQ
                Expect.equal "pop" -3 element

                Expect.isTrue "pop"  <| isEmpty newPQ }

            test "Can use a PQ as a seq" {
                let pq = empty false |> insert 15 |> insert 3 |> insert 0 |> insert 4 |> insert -3
                Expect.equal "" [-3;0;3;4;15] (pq |> Seq.toList) }
        ]
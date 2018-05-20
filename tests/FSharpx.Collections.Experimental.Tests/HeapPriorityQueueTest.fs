namespace FSharpx.Collections.Experimental.Tests

open FSharpx
open FSharpx.Collections.Experimental.HeapPriorityQueue
open Expecto
open Expecto.Flip

module HeapPriorityQueueTest =

    [<Tests>]
    let testHeapPriorityQueue =

        testList "Experimental HeapPriorityQueue" [
            test "empty queue should be empty" {
                let pq = empty false

                isEmpty pq |> Expect.isTrue "" 
                tryPeek pq |> Expect.isNone "" 
                tryPop pq |> Expect.isNone "" }

            test "After adding an element to the PQ it shouldn't be empty" {
                let pq = empty false |> insert 1

                isEmpty pq |> Expect.isFalse "" }
    

            test "After adding an element to the PQ the element should be the smallest" {
                let pq = empty false |> insert 1

                tryPeek pq |> Expect.equal "" (Some 1)
                peek pq |> Expect.equal "" 1 } 

            test "After adding an element to the PQ and popping it the PQ should be empty" {
                let pq = empty false |> insert 1

                let element,newPQ = pop pq
                element |> Expect.equal "" 1
                isEmpty newPQ |> Expect.isTrue "" 

                let element,newPQ = (tryPop pq).Value
                element |> Expect.equal "" 1
                isEmpty newPQ |> Expect.isTrue "" }

            test "Adding multiple elements to the PQ should allow to pop the smallest" {
                let pq = empty false |> insert 1 |> insert 3 |> insert 0 |> insert 4 |> insert -3

                let element,newPQ = pop pq
                element |> Expect.equal "" -3 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 0 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 1  

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 3 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 4 

                isEmpty newPQ |> Expect.isTrue "" }

            test "Adding multiple elements to a MaxPriorityQueue should allow to pop the smallest" {
                let pq = empty true |> insert 1 |> insert 3 |> insert 0 |> insert 4 |> insert -3

                let element,newPQ = pop pq
                element |> Expect.equal "" 4 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 3

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 1 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" 0 

                let element,newPQ = pop newPQ
                element |> Expect.equal "" -3 } 

            test "Can use a PQ as a seq" {
                let pq = empty false |> insert 15 |> insert 3 |> insert 0 |> insert 4 |> insert -3

                pq |> Seq.toList |> Expect.equal "" [-3;0;3;4;15] } 
        ]
namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module PriorityQueueTests =
    [<Tests>]
    let testPriorityQueue =

        testList
            "PriorityQueue"
            [ test "empty queue should be empty" {
                  let pq = PriorityQueue.empty false

                  Expect.isTrue "empty" <| PriorityQueue.isEmpty pq
                  Expect.isNone "empty" <| PriorityQueue.tryPeek pq
                  Expect.isNone "empty" <| PriorityQueue.tryPop pq
              }

              test "After adding an element to the PQ it shouldn't be empty" {
                  let pq = PriorityQueue.empty false |> PriorityQueue.insert 1
                  Expect.isFalse "PriorityQueue.insert" <| PriorityQueue.isEmpty pq
              }

              test "After adding an element to the PQ the element should be the smallest" {
                  let pq = PriorityQueue.empty false |> PriorityQueue.insert 1

                  Expect.equal "PriorityQueue.insert" (Some 1)
                  <| PriorityQueue.tryPeek pq

                  Expect.equal "PriorityQueue.insert" 1 <| PriorityQueue.peek pq
              }

              test "After adding an element to the PQ and popping it the PQ should be empty" {
                  let pq = PriorityQueue.empty false |> PriorityQueue.insert 1

                  let element, newPQ = PriorityQueue.pop pq
                  Expect.equal "PriorityQueue.pop" 1 element
                  Expect.isTrue "PriorityQueue.pop" <| PriorityQueue.isEmpty newPQ

                  let element, newPQ = (PriorityQueue.tryPop pq).Value
                  Expect.equal "PriorityQueue.tryPop" 1 element
                  Expect.isTrue "PriorityQueue.tryPop" <| PriorityQueue.isEmpty newPQ
              }

              test "Adding multiple elements to the PQ should allow to PriorityQueue.pop the smallest" {
                  let pq =
                      PriorityQueue.empty false
                      |> PriorityQueue.insert 1
                      |> PriorityQueue.insert 3
                      |> PriorityQueue.insert 0
                      |> PriorityQueue.insert 4
                      |> PriorityQueue.insert -3

                  let element, newPQ = PriorityQueue.pop pq
                  Expect.equal "PriorityQueue.pop" -3 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 0 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 1 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 3 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 4 element

                  Expect.isTrue "PriorityQueue.pop" <| PriorityQueue.isEmpty newPQ
              }

              test "Adding multiple elements to a MaxPriorityQueue should allow to PriorityQueue.pop the smallest" {
                  let pq =
                      PriorityQueue.empty true
                      |> PriorityQueue.insert 1
                      |> PriorityQueue.insert 3
                      |> PriorityQueue.insert 0
                      |> PriorityQueue.insert 4
                      |> PriorityQueue.insert -3

                  let element, newPQ = PriorityQueue.pop pq
                  Expect.equal "PriorityQueue.pop" 4 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 3 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 1 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" 0 element

                  let element, newPQ = PriorityQueue.pop newPQ
                  Expect.equal "PriorityQueue.pop" -3 element

                  Expect.isTrue "PriorityQueue.pop" <| PriorityQueue.isEmpty newPQ
              }

              test "Can use a PQ as a seq" {
                  let pq =
                      PriorityQueue.empty false
                      |> PriorityQueue.insert 15
                      |> PriorityQueue.insert 3
                      |> PriorityQueue.insert 0
                      |> PriorityQueue.insert 4
                      |> PriorityQueue.insert -3

                  Expect.equal "" [ -3; 0; 3; 4; 15 ] (pq |> Seq.toList)
              } ]

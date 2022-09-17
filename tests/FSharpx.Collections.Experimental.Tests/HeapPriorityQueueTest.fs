namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module HeapPriorityQueueTest =

    [<Tests>]
    let testHeapPriorityQueue =

        testList "Experimental HeapPriorityQueue" [ test "HeapPriorityQueue.empty queue should be HeapPriorityQueue.empty" {
                                                        let pq = HeapPriorityQueue.empty false

                                                        HeapPriorityQueue.isEmpty pq |> Expect.isTrue ""
                                                        HeapPriorityQueue.tryPeek pq |> Expect.isNone ""
                                                        HeapPriorityQueue.tryPop pq |> Expect.isNone ""
                                                    }

                                                    test "After adding an element to the PQ it shouldn't be HeapPriorityQueue.empty" {
                                                        let pq = HeapPriorityQueue.empty false |> HeapPriorityQueue.insert 1

                                                        HeapPriorityQueue.isEmpty pq |> Expect.isFalse ""
                                                    }


                                                    test "After adding an element to the PQ the element should be the smallest" {
                                                        let pq = HeapPriorityQueue.empty false |> HeapPriorityQueue.insert 1

                                                        HeapPriorityQueue.tryPeek pq |> Expect.equal "" (Some 1)
                                                        HeapPriorityQueue.peek pq |> Expect.equal "" 1
                                                    }

                                                    test "After adding an element to the PQ and popping it the PQ should be HeapPriorityQueue.empty" {
                                                        let pq = HeapPriorityQueue.empty false |> HeapPriorityQueue.insert 1

                                                        let element, newPQ = HeapPriorityQueue.pop pq
                                                        element |> Expect.equal "" 1
                                                        HeapPriorityQueue.isEmpty newPQ |> Expect.isTrue ""

                                                        let element, newPQ = (HeapPriorityQueue.tryPop pq).Value
                                                        element |> Expect.equal "" 1
                                                        HeapPriorityQueue.isEmpty newPQ |> Expect.isTrue ""
                                                    }

                                                    test "Adding multiple elements to the PQ should allow to HeapPriorityQueue.pop the smallest" {
                                                        let pq =
                                                            HeapPriorityQueue.empty false
                                                            |> HeapPriorityQueue.insert 1
                                                            |> HeapPriorityQueue.insert 3
                                                            |> HeapPriorityQueue.insert 0
                                                            |> HeapPriorityQueue.insert 4
                                                            |> HeapPriorityQueue.insert -3

                                                        let element, newPQ = HeapPriorityQueue.pop pq
                                                        element |> Expect.equal "" -3

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 0

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 1

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 3

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 4

                                                        HeapPriorityQueue.isEmpty newPQ |> Expect.isTrue ""
                                                    }

                                                    test
                                                        "Adding multiple elements to a MaxPriorityQueue should allow to HeapPriorityQueue.pop the smallest" {
                                                        let pq =
                                                            HeapPriorityQueue.empty true
                                                            |> HeapPriorityQueue.insert 1
                                                            |> HeapPriorityQueue.insert 3
                                                            |> HeapPriorityQueue.insert 0
                                                            |> HeapPriorityQueue.insert 4
                                                            |> HeapPriorityQueue.insert -3

                                                        let element, newPQ = HeapPriorityQueue.pop pq
                                                        element |> Expect.equal "" 4

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 3

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 1

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" 0

                                                        let element, newPQ = HeapPriorityQueue.pop newPQ
                                                        element |> Expect.equal "" -3
                                                    }

                                                    test "Can use a PQ as a seq" {
                                                        let pq =
                                                            HeapPriorityQueue.empty false
                                                            |> HeapPriorityQueue.insert 15
                                                            |> HeapPriorityQueue.insert 3
                                                            |> HeapPriorityQueue.insert 0
                                                            |> HeapPriorityQueue.insert 4
                                                            |> HeapPriorityQueue.insert -3

                                                        pq |> Seq.toList |> Expect.equal "" [ -3; 0; 3; 4; 15 ]
                                                    } ]

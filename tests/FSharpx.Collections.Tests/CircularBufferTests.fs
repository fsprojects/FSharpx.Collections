namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections.Mutable
open Expecto
open Expecto.Flip

module CircularBufferTests =

    [<Tests>]
    let testCircularBuffer =
        testList
            "CircularBuffer"
            [ test "Printing from a queue 1..5, all" {
                  let circularBuffer = CircularBuffer 5

                  circularBuffer.Enqueue(1)
                  circularBuffer.Enqueue(2)
                  circularBuffer.Enqueue(3)
                  circularBuffer.Enqueue(4)
                  circularBuffer.Enqueue 5
                  Expect.equal "" [| 1; 2; 3; 4; 5 |] <| circularBuffer.Dequeue 5
              }

              test "Printing from a queue 1..8, twice, all" {
                  let circularBuffer = CircularBuffer 5

                  circularBuffer.Enqueue(1)
                  circularBuffer.Enqueue(2)
                  circularBuffer.Enqueue(3)
                  circularBuffer.Enqueue(4)
                  circularBuffer.Enqueue 5 // <---
                  circularBuffer.Enqueue(6)
                  circularBuffer.Enqueue(7)
                  circularBuffer.Enqueue(8)
                  circularBuffer.Enqueue(1)
                  circularBuffer.Enqueue(2) // <---
                  circularBuffer.Enqueue(3)
                  circularBuffer.Enqueue(4)
                  circularBuffer.Enqueue 5
                  circularBuffer.Enqueue(6)
                  circularBuffer.Enqueue(7) // <---
                  circularBuffer.Enqueue(8)
                  Expect.equal "" [| 4; 5; 6; 7; 8 |] <| circularBuffer.Dequeue 5
              }

              test "Printing from a queue 1..5, partial" {
                  let circularBuffer = CircularBuffer 5

                  circularBuffer.Enqueue 1
                  circularBuffer.Enqueue 2
                  circularBuffer.Enqueue 3
                  circularBuffer.Enqueue 4
                  circularBuffer.Enqueue 5
                  Expect.equal "buffer" [| 1; 2; 3 |] <| circularBuffer.Dequeue 3
                  Expect.equal "count" 2 <| circularBuffer.Count
              }


              test "Printing from a queue 1..8 and dequeue 5, then enqueue 1..3 and dequeue 3" {
                  let circularBuffer = CircularBuffer 5

                  circularBuffer.Enqueue(1)
                  circularBuffer.Enqueue(2)
                  circularBuffer.Enqueue(3)
                  circularBuffer.Enqueue(4)
                  circularBuffer.Enqueue 5 // <---
                  circularBuffer.Enqueue(6)
                  circularBuffer.Enqueue(7)
                  circularBuffer.Enqueue(8)
                  Expect.equal "buffer" [| 4; 5; 6; 7; 8 |] <| circularBuffer.Dequeue 5
                  circularBuffer.Enqueue(1)
                  circularBuffer.Enqueue(2)
                  circularBuffer.Enqueue(3)
                  Expect.equal "buffer 2" [| 1; 2; 3 |] <| circularBuffer.Dequeue(3)
              }

              test "fail on overflow buffer" {
                  let f =
                      fun _ ->
                          let circularBuffer = CircularBuffer<int> 5
                          circularBuffer.Enqueue [| 1; 2; 3; 4; 5; 6; 7; 8; 1; 2; 3; 4; 5; 6; 7; 8 |]

                  Expect.throwsT<System.InvalidOperationException> "" f
              }

              ptest "Printing after multiple enqueue circles" {
                  let circularBuffer = CircularBuffer<int> 5

                  circularBuffer.Enqueue [| 1; 2; 3; 4; 5 |]
                  circularBuffer.Enqueue [| 6; 7; 8 |]
                  circularBuffer.Enqueue [| 1; 2; 3; 4; 5 |]
                  circularBuffer.Enqueue [| 6; 7; 8 |]
                  Expect.equal "buffer" [| 4; 5; 6; 7; 8 |] <| circularBuffer.Dequeue 5
              }



              ptest "Printing from a queue 1..8 and dequeue 5, then enqueue 1..3 and dequeue 3, from array" {
                  let circularBuffer = CircularBuffer<int> 5

                  circularBuffer.Enqueue([| 1; 2; 3; 4; 5 |])
                  circularBuffer.Enqueue([| 6; 7; 8 |])
                  Expect.equal "buffer" [| 4; 5; 6; 7; 8 |] <| circularBuffer.Dequeue 5
                  circularBuffer.Enqueue([| 1; 2; 3 |])
                  Expect.equal "buffer" [| 1; 2; 3 |] <| circularBuffer.Dequeue 3
              }

              ptest "Consider a large array with various, incoming array segments" {
                  let circularBuffer = CircularBuffer<int> 5

                  let source =
                      [| 1
                         2
                         3
                         4
                         5
                         1
                         2
                         3
                         4
                         5
                         6
                         7
                         8
                         1
                         2
                         3
                         4
                         5
                         6
                         7
                         8
                         1
                         2
                         3
                         4
                         5
                         1
                         2
                         3
                         1
                         2
                         3
                         4
                         5
                         6
                         7
                         8
                         1
                         2
                         3 |]

                  let incoming =
                      let generator =
                          seq {
                              yield ArraySegment<_>(source, 0, 5)
                              yield ArraySegment<_>(source, 5, 5)
                              yield ArraySegment<_>(source, 10, 3)
                              yield ArraySegment<_>(source, 13, 5)
                              yield ArraySegment<_>(source, 18, 3)
                              yield ArraySegment<_>(source, 21, 5)
                              yield ArraySegment<_>(source, 26, 3)
                              yield ArraySegment<_>(source, 29, 5)
                              yield ArraySegment<_>(source, 34, 3)
                              yield ArraySegment<_>(source, 37, 3)
                          } in

                      generator.GetEnumerator()

                  let enqueueNext() =
                      incoming.MoveNext() |> ignore
                      circularBuffer.Enqueue(incoming.Current)

                  // Printing from a queue 1..5
                  enqueueNext()

                  Expect.equal "buffer 1" [| 1; 2; 3; 4; 5 |]
                  <| circularBuffer.Dequeue 5

                  // Printing from a queue 1..8, twice
                  enqueueNext()
                  enqueueNext()
                  enqueueNext()
                  enqueueNext()

                  Expect.equal "buffer 2" [| 4; 5; 6; 7; 8 |]
                  <| circularBuffer.Dequeue 5

                  // Printing from a queue 1..5
                  enqueueNext()
                  Expect.equal "buffer 3" [| 1; 2; 3 |] <| circularBuffer.Dequeue 3

                  // Clear out the rest
                  circularBuffer.Dequeue 2 |> ignore

                  // Printing from a queue 1..3
                  enqueueNext()
                  Expect.equal "buffer 4" [| 1; 2; 3 |] <| circularBuffer.Dequeue 3

                  // Printing from a queue 1..8 and dequeue 5, then enqueue 1..3 and dequeue 3
                  enqueueNext()
                  enqueueNext()

                  Expect.equal "buffer 5" [| 4; 5; 6; 7; 8 |]
                  <| circularBuffer.Dequeue 5

                  enqueueNext()
                  Expect.equal "buffer 6" [| 1; 2; 3 |] <| circularBuffer.Dequeue 3
              }

              //printfn "Enqueue(array) tests passed in %d ms" stopwatch.ElapsedMilliseconds

              //let data = [|1;2;3;4;5|]
              //circularBuffer.Enqueue(data)
              //assert ((data |> Array.toList) = (queue |> Seq.toList))

              //printfn "Seq.toList matches enqueued data."

              //// [/snippet]

              //stopwatch.Reset()
              //stopwatch.Start()
              //// [snippet: Using CircularQueueAgent]
              //let buffer = new CircularQueueAgent<int>(3)

              //// The sample uses two workflows that add/take elements
              //// from the buffer with the following timeouts. When the producer
              //// timout is larger, consumer will be blocked. Otherwise, producer
              //// will be blocked.
              //let producerTimeout = 500
              //let consumerTimeout = 1000

              //async {
              //  for i in 0 .. 10 do
              //    // Sleep for some time and then add value
              //    do! Async.Sleep(producerTimeout)
              //    buffer.Enqueue([|i|])
              //    printfn "Added %d" i }
              //|> Async.Start

              //async {
              //  while true do
              //    // Sleep for some time and then get value
              //    do! Async.Sleep(consumerTimeout)
              //    let! v = buffer.AsyncDequeue(1)
              //    printfn "Got %d" v.[0] }
              //|> Async.Start
              //// [/snippet]
              //printfn "CircularQueueAgent.Enqueue(array) tests passed in %d ms" stopwatch.ElapsedMilliseconds

              //stopwatch.Reset()
              //stopwatch.Start()
              //// [snippet: Using CircularQueueAgent with AsyncEnqueue]
              //async {
              //  for i in 0 .. 10 do
              //    // Sleep for some time and then add value
              //    do! Async.Sleep(producerTimeout)
              //    do! buffer.AsyncEnqueue([|i|])
              //    printfn "Added %d" i }
              //|> Async.Start

              //async {
              //  while true do
              //    // Sleep for some time and then get value
              //    do! Async.Sleep(consumerTimeout)
              //    let! v = buffer.AsyncDequeue(1)
              //    printfn "Got %d" v.[0] }
              //|> Async.Start
              //// [/snippet]
              //printfn "CircularQueueAgent.AsyncEnqueue(array) tests passed in %d ms" stopwatch.ElapsedMilliseconds

              //stopwatch.Reset()
              //stopwatch.Start()
              //// [snippet: Using CircularStream]
              //let stream = new CircularStream(3)

              //async {
              //  for i in 0uy .. 10uy do
              //    // Sleep for some time and then add value
              //    do! Async.Sleep(producerTimeout)
              //    do! stream.AsyncWrite([|i|], 0, 1)
              //    printfn "Wrote %d" i }
              //|> Async.Start

              //async {
              //  let buffer = Array.zeroCreate<byte> 1
              //  while true do
              //    // Sleep for some time and then get value
              //    do! Async.Sleep(consumerTimeout)
              //    let! v = stream.AsyncRead(buffer, 0, 1)
              //    printfn "Read %d bytes with value %A" v buffer.[0] }
              //|> Async.Start
              //// [/snippet]
              //printfn "CircularStream.AsyncWrite(array) tests passed in %d ms" stopwatch.ElapsedMilliseconds
              ]

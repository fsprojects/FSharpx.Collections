namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module RingBufferTest =

    [<Tests>]
    let testRingBuffer =

        testList
            "Experimental RingBuffer"
            [ test "I can create a ring buffer with a given size" {
                  let actual = new RingBuffer<int>(10)
                  let expected = Array.init 10 (fun _ -> 0)
                  actual.ToArray() |> Expect.equal "" expected
              }

              test "I can create a ring buffer with a given set of values" {
                  let actual = new RingBuffer<int>([| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |])
                  let expected = Array.init 10 (fun i -> i + 1)
                  actual.ToArray() |> Expect.equal "" expected
              }

              test "I can normalise a ring buffer" {
                  let actual = new RingBuffer<int>([| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |])
                  actual.Advance(5) |> ignore
                  actual.Position |> Expect.equal "" 5
                  actual.Normalize()
                  actual.Position |> Expect.equal "" 0

                  actual.ToArray()
                  |> Expect.equal "" [| 6; 7; 8; 9; 10; 0; 0; 0; 0; 0 |]
              }

              test "I can insert values at 0 offset" {
                  let buffer = new RingBuffer<int>(10)
                  buffer.Insert(0, [| 1; 2; 3; 4; 5 |])
                  buffer.ToArray() |> Expect.equal "" [| 1; 2; 3; 4; 5; 0; 0; 0; 0; 0 |]
              }

              test "I should not be able to insert more values than the size of the buffer" {
                  let buffer = new RingBuffer<int>(10)
                  buffer.Insert(0, [| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11 |])

                  buffer.ToArray()
                  |> Expect.equal "" [| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |]
              }

              test "Insert should not fail if an empty sequence is inserted" {
                  let actual = new RingBuffer<int>(10)
                  actual.Insert(0, [||])
                  let expected = Array.init 10 (fun _ -> 0)
                  actual.ToArray() |> Expect.equal "" expected
              }

              test "I should be able to move the start index of the buffer" {
                  let buffer = new RingBuffer<int>([| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |])
                  buffer.Advance(5) |> ignore

                  buffer.ToArray()
                  |> Expect.equal "" [| 6; 7; 8; 9; 10; 0; 0; 0; 0; 0 |]
              }

              test "Advancing past the end of the buffer should cause it to wrap" {
                  let buffer = new RingBuffer<int>([| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |])
                  buffer.Advance(11) |> ignore
                  buffer.ToArray() |> Expect.equal "" [| 0; 0; 0; 0; 0; 0; 0; 0; 0; 0 |]
                  buffer.Position |> Expect.equal "" 1
              }

              test "I should be able to clone a ring buffer" {
                  let buffer = new RingBuffer<int>([| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 |])
                  let clone = buffer.Clone()
                  buffer.ToArray() |> Expect.equal "" <| clone.ToArray()
                  clone.Advance(2) |> ignore
                  Expect.notEqual "" (buffer.ToArray()) <| clone.ToArray()
                  buffer.Position |> Expect.equal "" 0
                  clone.Position |> Expect.equal "" 2
              }

              test "I can insert with an operation into the buffer" {
                  let buffer = new RingBuffer<int>(10)
                  buffer.Insert(0, [| 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11 |])
                  buffer.Insert((+), 5, [ 100; 100; 100; 100; 100; 100 ])

                  buffer.ToArray()
                  |> Expect.equal "" [| 1; 2; 3; 4; 5; 106; 107; 108; 109; 110 |]
              } ]

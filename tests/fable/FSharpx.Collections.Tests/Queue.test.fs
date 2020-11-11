module QueueTests

open Fable.Jester
open Fable.FastCheck
open Fable.FastCheck.Jest
open FSharpx.Collections

Jest.test("Queue works", async {
  let q = Queue.empty

  Jest.expect(Queue.isEmpty q).toEqual(true)

  let q = Queue.conj "a" q

  Jest.expect(Queue.isEmpty q).toEqual(false)
  Jest.expect(Queue.tryHead q).toEqual(Some "a")

  let q = Queue.conj "b" q

  Jest.expect(Queue.tryHead q).toEqual(Some "a")

  let q = Queue.conj "c" q

  Jest.expect(Queue.tryTail q).toEqual(Queue.empty |> Queue.conj "b" |> Queue.conj "c")

  let popped, q = Queue.uncons q

  Jest.expect(popped).toEqual("a")

  let popped, q = Queue.uncons q

  Jest.expect(popped).toEqual("b")
})

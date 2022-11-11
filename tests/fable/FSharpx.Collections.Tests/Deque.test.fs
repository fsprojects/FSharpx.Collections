module DequeTests

open Fable.Jester
open Fable.FastCheck
open Fable.FastCheck.Jest
open FSharpx.Collections

Jest.test(
    "NonEmptyList works",
    async {
        let d = Deque.empty

        Jest.expect(Deque.isEmpty d).toEqual(true)

        let d = d |> Deque.conj "b" |> Deque.conj "a"

        Jest.expect(Deque.isEmpty d).toEqual(false)
        Jest.expect(Deque.head d).toEqual("b")
        Jest.expect(Deque.length d).toEqual(2)
    }
)

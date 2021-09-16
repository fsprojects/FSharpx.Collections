module LazyListTests

open Fable.Jester
open Fable.FastCheck
open Fable.FastCheck.Jest
open FSharpx.Collections

Jest.test("LazyList works", async {
  let xs = LazyList.empty

  Jest.expect(LazyList.isEmpty xs).toEqual(true)

  let xs = LazyList.ofList [ 1; 2; 3 ]

  Jest.expect(LazyList.isEmpty xs).toEqual(false)
  Jest.expect(LazyList.tryHead xs).toEqual(1)

  let xs = xs |> LazyList.tail |> LazyList.tail

  Jest.expect(LazyList.tryHead xs).toEqual(3)
})

module NonEmptyListTests

open Fable.Jester
open Fable.FastCheck
open Fable.FastCheck.Jest
open FSharpx.Collections

Jest.test("NonEmptyList works", async {
  let xs = NonEmptyList.create "a" []

  Jest.expect(NonEmptyList.length xs).toEqual(1)
  Jest.expect(NonEmptyList.head xs).toEqual("a")

  let xs =
    xs
    |> NonEmptyList.cons "b"
    |> NonEmptyList.cons "c"

  Jest.expect(NonEmptyList.head xs).toEqual("c")
  Jest.expect(NonEmptyList.length xs).toEqual(3)
})

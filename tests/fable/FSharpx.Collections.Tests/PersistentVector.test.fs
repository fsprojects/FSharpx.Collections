module PersistentVectorTests

open Fable.Jester
open Fable.FastCheck
open Fable.FastCheck.Jest
open FSharpx.Collections

Jest.test(
    "PersistentVector.empty works as expected",
    fun () ->
        let v = PersistentVector.empty

        Jest.expect(PersistentVector.isEmpty v).toEqual(true)
        Jest.expect(PersistentVector.length v).toEqual(0)
)

Jest.test(
    "PersistentVector.conj works as expected",
    fun () ->
        let v = PersistentVector.empty |> PersistentVector.conj "a"

        Jest.expect(PersistentVector.nth 0 v).toEqual("a")

        let v = v |> PersistentVector.conj "b" |> PersistentVector.conj "c"

        Jest.expect(PersistentVector.nth 0 v).toEqual("a")
        Jest.expect(PersistentVector.nth 1 v).toEqual("b")
        Jest.expect(PersistentVector.nth 2 v).toEqual("c")
)

Jest.test(
    "PersistentVector implements seq as expected",
    fun () ->
        let v =
            PersistentVector.empty
            |> PersistentVector.conj 1
            |> PersistentVector.conj 4
            |> PersistentVector.conj 25

        Jest.expect(Seq.toList v = [ 1; 4; 25 ]).toBe(true)
)

Jest.test(
    "PersistentVector.map works as expected",
    fun () ->
        let v =
            PersistentVector.ofSeq [ 1..4 ]
            |> PersistentVector.map(fun x -> x * 2)

        Jest.expect(Seq.toList v = [ 2; 4; 6; 8 ]).toBe(true)
)

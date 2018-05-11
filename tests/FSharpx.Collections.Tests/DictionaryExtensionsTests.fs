namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module DictionaryExtensionsTests =

    [<Tests>]
    let testDictionaryExtensions =
        testList "DictionaryExtensions" [

            test "dictionary tryfind with some" {
              let a = dict [1,"one"; 2,"two"]
              Expect.equal "tryfind" (Some "one") (a |> Dictionary.tryFind 1) }

            test "dictionary tryfind with none" {
              let a = dict [1,"one"; 2,"two"]
              Expect.isNone  "tryfind" (a |> Dictionary.tryFind 3) }
        ]

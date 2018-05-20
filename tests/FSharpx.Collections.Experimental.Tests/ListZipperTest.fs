namespace FSharpx.Collections.Experimental.Tests

open FSharpx
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.ListZipper
open Expecto
open Expecto.Flip
   
module ListZipperTest =

    let chars = ['a'..'z']
    let digits = ['0'..'9']

    [<Tests>]
    let testListZipper =

        testList "Experimental ListZipper" [
            test "Can move forward" {
                let z = chars |> zipper |> forward |> forward
                Expect.equal "" 'c' <| focus z }

            test "Can move back" {
                let z = chars |> zipper |> forward |> forward |> back
                Expect.equal "" 'b' <| focus z }

            test "Can move to the front" {
                let z = chars |> zipper |> forward |> forward |> front
                Expect.equal "" 'a' <| focus z }

            test "Can modify an element" {
                let z = chars |> zipper |> forward |> forward |> modify 'e' |> back |> forward
                Expect.equal "" 'e' <| focus z }
        ]
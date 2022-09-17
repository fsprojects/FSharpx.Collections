namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module ListZipperTest =

    let chars = [ 'a' .. 'z' ]
    let digits = [ '0' .. '9' ]

    [<Tests>]
    let testListZipper =

        testList "Experimental ListZipper" [ test "Can move forward" {
                                                 let z = chars |> ListZipper.zipper |> ListZipper.forward |> ListZipper.forward
                                                 Expect.equal "" 'c' <| ListZipper.focus z
                                             }

                                             test "Can move back" {
                                                 let z =
                                                     chars
                                                     |> ListZipper.zipper
                                                     |> ListZipper.forward
                                                     |> ListZipper.forward
                                                     |> ListZipper.back

                                                 Expect.equal "" 'b' <| ListZipper.focus z
                                             }

                                             test "Can move to the front" {
                                                 let z =
                                                     chars
                                                     |> ListZipper.zipper
                                                     |> ListZipper.forward
                                                     |> ListZipper.forward
                                                     |> ListZipper.front

                                                 Expect.equal "" 'a' <| ListZipper.focus z
                                             }

                                             test "Can modify an element" {
                                                 let z =
                                                     chars
                                                     |> ListZipper.zipper
                                                     |> ListZipper.forward
                                                     |> ListZipper.forward
                                                     |> ListZipper.modify 'e'
                                                     |> ListZipper.back
                                                     |> ListZipper.forward

                                                 Expect.equal "" 'e' <| ListZipper.focus z
                                             } ]

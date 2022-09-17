namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module EditDistanceTest =

    type BS = ByteString

    [<Tests>]
    let testEditDistance =

        testList "Experimental EditDistance" [ test "distance example" {
                                                   BKTree.ByteString.distance (BS "kitten"B) (BS "sitting"B)
                                                   |> Expect.equal "" 3
                                               }

                                               test "toListDistance example" {
                                                   [ BS "kitten"B; BS "setting"B; BS "getting"B ]
                                                   |> BKTree.ByteString.ofList
                                                   |> BKTree.ByteString.toListDistance 2 (BS "sitting"B)
                                                   |> Expect.equal "" [ BS "setting"B; BS "getting"B ]
                                               }

                                               test "String distance example" {
                                                   let inline toBS(text: string) =
                                                       ByteString(System.Text.Encoding.ASCII.GetBytes text)

                                                   let calcEditDistance text1 text2 =
                                                       BKTree.ByteString.distance (toBS text1) (toBS text2)

                                                   calcEditDistance "meilenstein" "levenshtein" |> Expect.equal "" 4
                                               } ]

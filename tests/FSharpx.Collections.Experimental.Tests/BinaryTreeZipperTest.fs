namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.BinaryTreeZipper
open Expecto
open Expecto.Flip

module BinaryTreeZipperTest =
    let tree  = Branch("a", Branch("b", Leaf, Branch("c", Leaf, Leaf)), Branch("d", Leaf, Leaf))

    [<Tests>]
    let testBinaryTreeZipper =

        testList "Experimental BinaryTreeZipper" [

            test "Can create zipper from tree" {      
               let z1 = tree |> zipper
               Expect.equal "" tree z1.Focus }

            test "Can move down to the left inside the zipper" {      
               let z1 = tree |> zipper |> left
               Expect.equal "" (Branch("b", Leaf, Branch("c", Leaf, Leaf))) z1.Focus }

            test "Can move down to the right inside the zipper" {      
               let z1 = tree |> zipper |> right
               Expect.equal "" (Branch("d", Leaf, Leaf)) z1.Focus }

            test "Can move down to the left and the right inside the zipper" {      
               let z1 = tree |> zipper |> move [Left;Right]
               Expect.equal "" (Branch("c", Leaf, Leaf)) z1.Focus }

            test "Can move up inside the zipper" {      
               let z1 = tree |> zipper |> move [Left;Right;Right;Up;Up;Up]
               Expect.equal "" tree z1.Focus }

            test "Can move to the top from inside the zipper" {      
               let z1 = tree |> zipper |> move [Left;Right;Right] |> top
               Expect.equal "" tree z1.Focus }

            test "Can modify inside the zipper" { 
               let z1 = tree |> zipper |> right |> setFocus (branch "e") |> top

               Expect.equal "" (Branch("a", Branch("b", Leaf, Branch("c", Leaf, Leaf)), Branch("e", Leaf, Leaf))) z1.Focus }
        ]
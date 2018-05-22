namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module BinaryTreeZipperTest =
    let tree  = BinaryTreeZipper.Branch("a", BinaryTreeZipper.Branch("b", BinaryTreeZipper.Leaf, BinaryTreeZipper.Branch("c", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf)), BinaryTreeZipper.Branch("d", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf))

    [<Tests>]
    let testBinaryTreeZipper =

        testList "Experimental BinaryTreeZipper" [

            test "Can create BinaryTreeZipper.zipper from tree" {      
               let z1 = tree |> BinaryTreeZipper.zipper
               Expect.equal "" tree z1.Focus }

            test "Can BinaryTreeZipper.move down to the BinaryTreeZipper.left inside the BinaryTreeZipper.zipper" {      
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.left
               Expect.equal "" (BinaryTreeZipper.Branch("b", BinaryTreeZipper.Leaf, BinaryTreeZipper.Branch("c", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf))) z1.Focus }

            test "Can BinaryTreeZipper.move down to the BinaryTreeZipper.right inside the BinaryTreeZipper.zipper" {      
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.right
               Expect.equal "" (BinaryTreeZipper.Branch("d", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf)) z1.Focus }

            test "Can BinaryTreeZipper.move down to the BinaryTreeZipper.left and the BinaryTreeZipper.right inside the BinaryTreeZipper.zipper" {      
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.move [BinaryTreeZipper.TreeZipperDirection.Left;BinaryTreeZipper.TreeZipperDirection.Right]
               Expect.equal "" (BinaryTreeZipper.Branch("c", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf)) z1.Focus }

            test "Can BinaryTreeZipper.move up inside the BinaryTreeZipper.zipper" {      
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.move [BinaryTreeZipper.TreeZipperDirection.Left;BinaryTreeZipper.TreeZipperDirection.Right;BinaryTreeZipper.TreeZipperDirection.Right;BinaryTreeZipper.Up;BinaryTreeZipper.Up;BinaryTreeZipper.Up]
               Expect.equal "" tree z1.Focus }

            test "Can BinaryTreeZipper.move to the top from inside the BinaryTreeZipper.zipper" {      
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.move [BinaryTreeZipper.TreeZipperDirection.Left;BinaryTreeZipper.TreeZipperDirection.Right;BinaryTreeZipper.TreeZipperDirection.Right] |> BinaryTreeZipper.top
               Expect.equal "" tree z1.Focus }

            test "Can modify inside the BinaryTreeZipper.zipper" { 
               let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.right |> BinaryTreeZipper.setFocus (BinaryTreeZipper.branch "e") |> BinaryTreeZipper.top

               Expect.equal "" (BinaryTreeZipper.Branch("a", BinaryTreeZipper.Branch("b", BinaryTreeZipper.Leaf, BinaryTreeZipper.Branch("c", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf)), BinaryTreeZipper.Branch("e", BinaryTreeZipper.Leaf, BinaryTreeZipper.Leaf))) z1.Focus }
        ]
namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module BinaryTreeZipperTest =
    let tree =
        Branch("a", Branch("b", BinaryTree.Leaf, Branch("c", BinaryTree.Leaf, BinaryTree.Leaf)), Branch("d", BinaryTree.Leaf, BinaryTree.Leaf))

    [<Tests>]
    let testBinaryTreeZipper =

        testList "Experimental BinaryTreeZipper" [

                                                   test "Can create BinaryTreeZipper.zipper from tree" {
                                                       let z1 = tree |> BinaryTreeZipper.zipper
                                                       Expect.equal "" tree z1.Focus
                                                   }

                                                   test
                                                       "Can BinaryTreeZipper.move down to the BinaryTreeZipper.left inside the BinaryTreeZipper.zipper" {
                                                       let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.left

                                                       Expect.equal
                                                           ""
                                                           (Branch("b", BinaryTree.Leaf, Branch("c", BinaryTree.Leaf, BinaryTree.Leaf)))
                                                           z1.Focus
                                                   }

                                                   test
                                                       "Can BinaryTreeZipper.move down to the BinaryTreeZipper.right inside the BinaryTreeZipper.zipper" {
                                                       let z1 = tree |> BinaryTreeZipper.zipper |> BinaryTreeZipper.right
                                                       Expect.equal "" (Branch("d", BinaryTree.Leaf, BinaryTree.Leaf)) z1.Focus
                                                   }

                                                   test
                                                       "Can BinaryTreeZipper.move down to the BinaryTreeZipper.left and the BinaryTreeZipper.right inside the BinaryTreeZipper.zipper" {
                                                       let z1 =
                                                           tree
                                                           |> BinaryTreeZipper.zipper
                                                           |> BinaryTreeZipper.move [ BinaryTreeZipper.TreeZipperDirection.Left
                                                                                      BinaryTreeZipper.TreeZipperDirection.Right ]

                                                       Expect.equal "" (Branch("c", BinaryTree.Leaf, BinaryTree.Leaf)) z1.Focus
                                                   }

                                                   test "Can BinaryTreeZipper.move up inside the BinaryTreeZipper.zipper" {
                                                       let z1 =
                                                           tree
                                                           |> BinaryTreeZipper.zipper
                                                           |> BinaryTreeZipper.move [ BinaryTreeZipper.TreeZipperDirection.Left
                                                                                      BinaryTreeZipper.TreeZipperDirection.Right
                                                                                      BinaryTreeZipper.TreeZipperDirection.Right
                                                                                      BinaryTreeZipper.Up
                                                                                      BinaryTreeZipper.Up
                                                                                      BinaryTreeZipper.Up ]

                                                       Expect.equal "" tree z1.Focus
                                                   }

                                                   test "Can BinaryTreeZipper.move to the top from inside the BinaryTreeZipper.zipper" {
                                                       let z1 =
                                                           tree
                                                           |> BinaryTreeZipper.zipper
                                                           |> BinaryTreeZipper.move [ BinaryTreeZipper.TreeZipperDirection.Left
                                                                                      BinaryTreeZipper.TreeZipperDirection.Right
                                                                                      BinaryTreeZipper.TreeZipperDirection.Right ]
                                                           |> BinaryTreeZipper.top

                                                       Expect.equal "" tree z1.Focus
                                                   }

                                                   test "Can modify inside the BinaryTreeZipper.zipper" {
                                                       let z1 =
                                                           tree
                                                           |> BinaryTreeZipper.zipper
                                                           |> BinaryTreeZipper.right
                                                           |> BinaryTreeZipper.setFocus(BinaryTreeZipper.branch "e")
                                                           |> BinaryTreeZipper.top

                                                       Expect.equal
                                                           ""
                                                           (Branch(
                                                               "a",
                                                               Branch("b", BinaryTree.Leaf, Branch("c", BinaryTree.Leaf, BinaryTree.Leaf)),
                                                               Branch("e", BinaryTree.Leaf, BinaryTree.Leaf)
                                                           ))
                                                           z1.Focus
                                                   } ]

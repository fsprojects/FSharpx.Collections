namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module AaTreeTest =
    [<Tests>]
    let testAaTree = 
        testList "AaTree" [

        (* Existence tests. *)
        test "test isEmpty" {
            Expect.isTrue <| AaTree.isEmpty AaTree.empty <| ""
            Expect.isFalse <| AaTree.isEmpty (AaTree.ofList [9]) <| ""            
        }

        test "test exists" {
            let tree = AaTree.ofList [9]
            Expect.isTrue <| AaTree.exists 9 tree <| ""
            Expect.isFalse <| AaTree.exists 10 tree <| ""
        }

        test "test notExists" {
            let tree = AaTree.ofList [9]
            Expect.isFalse <| AaTree.notExists 9 tree <| ""
            Expect.isTrue <| AaTree.notExists 10 tree <| ""
        }

        test "test tryFind" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.equal (Some("hello")) <| AaTree.tryFind "hello" tree <| ""
            Expect.isNone <| AaTree.tryFind "goodbye" tree <| ""
        }

        test "test find" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.equal "hello" <| AaTree.find "hello" tree <| ""
            Expect.throws (fun () -> AaTree.find "goodbye" tree |> ignore) ""
        }

        (* Conversion from tests. *)
        test "test ofList" {
            let list = ['a'; 'b'; 'c'; 'd'; 'e']
            let tree = AaTree.ofList list
            for i in list do
                Expect.isTrue <| AaTree.exists i tree <| ""
        }

        test "test ofArray" {
            let array = [|1; 2; 3; 4; 5|]
            let tree = AaTree.ofArray array
            for i in array do
                Expect.isTrue <| AaTree.exists i tree <| ""
        }

        test "test ofSeq" {
            let seq = Seq.ofList ["hello"; "yellow"; "bye"; "try"]
            let tree = AaTree.ofSeq seq
            for i in seq do
                Expect.isTrue <| AaTree.exists i tree <| ""
        }

        (* Conversion to tests. *)
        test "test toList" {
            let inputList = [0;1;2;3]
            let tree = AaTree.ofList inputList
            let outputList = AaTree.toList tree
            Expect.equal outputList inputList ""
        }

        test "test toArray" {
            let inputArray = [|0;1;2;3|]
            let tree = AaTree.ofArray inputArray
            let outputArray = AaTree.toArray tree
            Expect.equal outputArray inputArray ""
        }

        test "test toSeq" { 
            let inputSeq = Seq.ofList ["hi";"why";"try"]
            let tree = AaTree.ofSeq inputSeq
            let outputSeq = AaTree.toSeq tree
            Expect.containsAll outputSeq inputSeq ""
        }

        (* Fold and foldback tests. 
         * We will try building two lists using fold/foldback, 
         * because that is an operation where order matters. *)
        test "test fold" {
            let tree = AaTree.ofList [1;2;3]
            let foldBackResult = AaTree.fold (fun a e -> e::a) [] tree
            Expect.equal foldBackResult [3;2;1] ""
        }

        test "test foldBack" {
            let tree = AaTree.ofList [1;2;3]
            let foldResult = AaTree.foldBack (fun a e -> e::a) [] tree
            Expect.equal foldResult [1;2;3] ""
        }

        (* Insert and delete tests. *)
        test "test insert" {
            let numsToInsert = [1;2;3;4;5]
            // Insert items into tree from list via AaTree.Insert in lambda.
            let tree = List.fold (fun tree el -> AaTree.insert el tree) AaTree.empty numsToInsert
            
            // Test that each item in the list is in the tree.
            for i in numsToInsert do
                Expect.isTrue <| AaTree.exists i tree <| ""
        }

        test "test delete" {
            // We have to insert items into a tree before we can delete them.
            let numsToInsert = [1;2;3;4;5]
            let tree = List.fold (fun tree el -> AaTree.insert el tree) AaTree.empty numsToInsert

            // Define numbers to delete and use List.fold to perform AaTree.delete on all
            let numsToDelete = [1;2;4;5]
            let tree = List.fold (fun tree el -> AaTree.delete el tree) tree numsToDelete

            // Test that none of the deleted items exist
            for i in numsToDelete do
                Expect.isFalse <| AaTree.exists i tree <| ""

            // Test that the one element we did not delete still exists in the tree.
            Expect.isTrue <| AaTree.exists 3 tree <| ""
        }
      ]

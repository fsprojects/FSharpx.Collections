namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto

module AaTreeTest =
    [<Tests>]
    let testAaTree = 
        testList "AaTree" [

        (* Existence tests. *)
        test "test isEmpty returns true for an empty AaTree" {
            let tree = AaTree.empty
            Expect.isTrue <| AaTree.isEmpty tree <| "expect isEmpty returns true"
        }

        test "test isEmpty returns false for an AaTree with at least one value" {
            let tree = AaTree.ofList [9]
            Expect.isFalse <| AaTree.isEmpty tree <| "expect isEmpty returns false"            
        }

        test "test isEmpty returns true when we delete an AaTree's last element " {
            let tree = AaTree.ofList [1]
            let tree = AaTree.delete 1 tree
            Expect.isTrue <| AaTree.isEmpty tree <| "expect isEmpty returns true"
        }

        test "test exists returns true when item exists in AaTree" {
            let tree = AaTree.ofList [9]
            Expect.isTrue <| AaTree.exists 9 tree <| "expect exists returns true"
        }

        test "test exists returns false when item does not exist in AaTree" {
            let tree = AaTree.ofList [9]
            Expect.isFalse <| AaTree.exists 10 tree <| "expect exists returns false"
        }

        test "test notExists returns true when item does not exist in AaTree" {
            let tree = AaTree.ofList [9]
            Expect.isTrue <| AaTree.notExists 10 tree <| "expect notExists returns true"
        }

        test "test notExists returns false when item exists in AaTree" {
            let tree = AaTree.ofList [9]
            Expect.isFalse <| AaTree.notExists 9 tree <| "expect notExists returns false"
        }

        test "test tryFind returns Some Item when Item exists in AaTree" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.equal (Some("hello")) <| AaTree.tryFind "hello" tree <| "expect tryFind returns Some Item"
        }

        test "test tryFind returns None when Item does not exist in AaTree" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.isNone <| AaTree.tryFind "goodbye" tree <| "expect tryFind returns None"
        }
        
        test "test find returns Item when Item exists in AaTree" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.equal "hello" <| AaTree.find "hello" tree <| "expect find returns item"
        }

        test "test find throws error when Item does not exist in AaTree" {
            let tree = AaTree.ofList ["hello"; "bye"]
            Expect.throws (fun () -> AaTree.find "goodbye" tree |> ignore) "expect find throws error"
        }

        (* Conversion from tests. *)
        test "test ofList returns AaTree where all elements in list exist" {
            let list = ['a'; 'b'; 'c'; 'd'; 'e']
            let tree = AaTree.ofList list
            let returnList = AaTree.toList tree
            for i in list do
                Expect.isTrue <| AaTree.exists i tree <| "expect AaTee.exists returns true on each item"
        }

        test "test ofArray returns AaTree where all elements in array exist" {
            let array = [|1; 2; 3; 4; 5|]
            let tree = AaTree.ofArray array
            for i in array do
                Expect.isTrue <| AaTree.exists i tree <| "expect AaTee.exists returns true on each item"
        }

        test "test ofSeq returns AaTree where all elements in seq exist" {
            let seq = Seq.ofList ["hello"; "yellow"; "bye"; "try"]
            let tree = AaTree.ofSeq seq
            for i in seq do
                Expect.isTrue <| AaTree.exists i tree <| "expect AaTee.exists returns true on each item"
        }

        (* Conversion to tests. *)
        test "test toList returns list equal to input list" {
            let inputList = [0;1;2;3]
            let tree = AaTree.ofList inputList
            let outputList = AaTree.toList tree
            Expect.equal outputList inputList "expect lists are equal"
        }

        test "test toArray returns array equal to input array" {
            let inputArray = [|0;1;2;3|]
            let tree = AaTree.ofArray inputArray
            let outputArray = AaTree.toArray tree
            Expect.equal outputArray inputArray "expect arrays are equal"
        }

        test "test toSeq returns seq equal to input seq" { 
            let inputSeq = Seq.ofList ["hi";"why";"try"]
            let tree = AaTree.ofSeq inputSeq
            let outputSeq = AaTree.toSeq tree
            Expect.containsAll outputSeq inputSeq "expect seqs are equal"
        }

        (* Fold and foldback tests. 
         * We will try building two lists using fold/foldback, 
         * because that is an operation where order matters. *)
        test "test fold operates on values in sorted order" {
            let inputList = [1;2;3]
            let expectList = List.rev inputList
            let tree = AaTree.ofList inputList
            // List :: cons operator used in folder puts the new value
            // at the start of the list, so it should be in reverse order.
            // For example, initial list: [1]
            // 2 :: [1] = [2;1] and finally 3 :: [2;1] = [3;2;1]
            // We start operating with the first element in sorted order.
            let foldBackResult = AaTree.fold (fun a e -> e::a) [] tree
            Expect.equal foldBackResult expectList "expect result is equal to reverse of input"
        }

        test "test foldBack operates on values in reverse order" {
            let inputList = [1; 2; 3]
            let tree = AaTree.ofList inputList
            // Exact same logic and folder in fold test abovem
            // but expecting reverse .
            // 3 :: [] = 3 -> 2 :: [3] = [2;3] -> 1 :: [2;3] = [1;2;3]
            let foldResult = AaTree.foldBack (fun a e -> e::a) [] tree
            Expect.equal foldResult inputList "expect result is equal to input"
        }

        (* Insert and delete tests. *)
        test "test inserted elements exist in AaTree" {
            let numsToInsert = [1;2;3;4;5]
            // Insert items into tree from list via AaTree.Insert in lambda.
            let tree = List.fold (fun tree el -> AaTree.insert el tree) AaTree.empty numsToInsert
            
            // Test that each item in the list is in the tree.
            for i in numsToInsert do
                Expect.isTrue <| AaTree.exists i tree <| "expect existence of inserted elements is true"
        }

        test "test deleted elements do not exist in AaTree" {
            // We have to insert items into a tree before we can delete them.
            let numsToInsert = [1;2;3;4;5]
            let tree = List.fold (fun tree el -> AaTree.insert el tree) AaTree.empty numsToInsert

            // Define numbers to delete and use List.fold to perform AaTree.delete on all
            let numsToDelete = [1;2;4;5]
            let tree = List.fold (fun tree el -> AaTree.delete el tree) tree numsToDelete

            // Test that none of the deleted items exist
            for i in numsToDelete do
                Expect.isFalse <| AaTree.exists i tree <| "expect existence of delete elements is false"
        }

        test "test element we did not delete exists in AaTree when we delete all others" {
            // We have to insert items into a tree before we can delete them.
            let numsToInsert = [1;2;3;4;5]
            let tree = List.fold (fun tree el -> AaTree.insert el tree) AaTree.empty numsToInsert

            // Define numbers to delete and use List.fold to perform AaTree.delete on all
            let numsToDelete = [1;2;4;5]
            let tree = List.fold (fun tree el -> AaTree.delete el tree) tree numsToDelete

            // Test that the one element we did not delete still exists in the tree.
            Expect.isTrue <| AaTree.exists 3 tree <| "expect existence of element we did not delete is still true"

        }
      ]

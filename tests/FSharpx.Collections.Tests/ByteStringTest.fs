namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Expecto
open Expecto.Flip

module ByteStringTests =
    type BS = ByteString

    let comparisonTests = [
          // When the base array is different
          "empty1",   ByteString.create ""B,   ByteString.create ""B,    0
          "empty2",   ByteString.create ""B,   ByteString.create "a"B,  -1  
          "empty3",   ByteString.create "a"B,  ByteString.create ""B,    1
          
          "same1",    ByteString.create "a"B,  ByteString.create "a"B,   0
          "smaller1", ByteString.create "a"B,  ByteString.create "b"B,  -1
          "bigger1",  ByteString.create "b"B,  ByteString.create "a"B,   1
          
          "longer1",  ByteString.create "aa"B, ByteString.create "a"B,   1
          "shorter1", ByteString.create "b"B,  ByteString.create "aa"B, -1
          
          //when the base array is the same
          let x = "baab"B
          "same2",    ByteString(x,0,1), ByteString(x,0,1),  0 
          "same3",    ByteString(x,0,1), ByteString(x,3,1),  0 
          "shorter2", ByteString(x,0,1), ByteString(x,0,2), -1 
          "shorter3", ByteString(x,0,1), ByteString(x,2,2), -1 
          "longer2",  ByteString(x,0,2), ByteString(x,0,1),  1 
          "longet3",  ByteString(x,1,2), ByteString(x,0,1),  1 
          "longer4",  ByteString(x,2,2), ByteString(x,0,1),  1 
        ]
  
    let spanAndSplitTests = [|
            "Howdy! Want to play?"B, ' 'B,   6
            "Howdy! Want to play?"B, '?'B,  19
            "Howdy! Want to play?"B, '\r'B, 20
        |]

    [<Tests>]
    let testByteString =
        testList "ByteString" [
            
            testList "test ByteString comparison should correctly return -1, 0, or 1"
                    (comparisonTests
                    |> List.map (fun (name, x, y, expectedResult) -> test name { BS.Compare(x, y) |> Expect.equal "comparison" expectedResult  }))
                

            test "test ByteString_length should return the length of the byte string" {
              let input = ByteString.create "Hello, world!"B
              Expect.equal "length" 13 <| ByteString.length input }

            test "test ByteString_span correctly breaks the ByteString on the specified predicate" {
                spanAndSplitTests
                |> Array.iter (fun (input, breakChar, breakIndex) -> 
                    let str = ByteString.create input
                    let expected = if input.Length = breakIndex then str, ByteString.empty
                                    else BS(input, 0, breakIndex), BS(input, breakIndex, input.Length - breakIndex)
                    Expect.equal "ByteString * ByteString" expected <| ByteString.span ((<>) breakChar) str ) }

            test "test ByteString_split correctly breaks the ByteString on the specified predicate" {
                spanAndSplitTests
                |> Array.iter (fun (input, breakChar, breakIndex) -> 
                    let str = ByteString.create input
                    let expected = if input.Length = breakIndex then str, ByteString.empty
                                    else BS(input, 0, breakIndex), BS(input, breakIndex, input.Length - breakIndex)
                    Expect.equal "ByteString * ByteString" expected <| ByteString.split ((=) breakChar) str ) }

            test "test ByteString_span correctly breaks the ByteString on \r" {
                let input = "test\r\ntest"B
                let str = ByteString.create input
                let expected = BS(input, 0, 4), BS(input, 4, 6)
                Expect.equal "ByteString * ByteString" expected <| ByteString.span (fun c -> c <> '\r'B && c <> '\n'B) str }

            test "test ByteString_split correctly breaks the ByteString on \r" {
                let input = "test\r\ntest"B
                let str = ByteString.create input
                let expected = BS(input, 0, 4), BS(input, 4, 6)
                Expect.equal "ByteString * ByteString" expected <| ByteString.split (fun c -> c = '\r'B || c = '\n'B) str }

            test "test ByteString_splitAt correctly breaks the ByteString on the specified index" {
                let input = "Howdy! Want to play?"B
                let str = ByteString.create input
                let expected = BS(input, 0, 6), BS(input, 6, 14)
                Expect.equal "ByteString * ByteString" expected <| ByteString.splitAt 6 str }

            test "test ByteString_fold should concatenate bytes into a string" {
                Expect.equal "string" "Howdy" 
                    <| (ByteString.create "Howdy"B
                        |> ByteString.fold (fun a b -> a + (char b).ToString()) "" ) }

            test "test ByteString_take correctly truncates the ByteString at the selected index" {
                let input = "Howdy! Want to play?"B
                let str = ByteString.create input
                let expected = BS(input, 0, 6)
                Expect.equal "ByteString" expected <| ByteString.take 6 str }

           
            test "test drop should drop the first n items" {
                let input = "Howdy! Want to play?"B
                Expect.equal "ByteString"  (BS(input,7,13)) <|ByteString. skip 7 (ByteString.create input) }

            test "test dropWhile should drop anything before the first space" {
                let input = ByteString.create "Howdy! Want to play?"B
                let dropWhile2Head = ByteString.skipWhile ((<>) ' 'B) >> ByteString.head
                Expect.equal "Byte" ' 'B <| dropWhile2Head input }

            test "test take should return an empty ArraySegment when asked to take 0" {
                Expect.equal "empty ByteString" ByteString.empty <| ByteString.take 0 (ByteString.create "Nothing should be taken"B) }

            test "test take should return an empty ArraySegment when given an empty ArraySegment" {
                Expect.equal "empty ByteString" ByteString.empty <| ByteString.take 4 ByteString.empty }

            test "test take should take the first n items" {
                let input = [|0uy..9uy|]

                [1;2;3;4;5;6;7;8;9;10]
                |> List.iter (fun x ->
                    Expect.equal "ByteString" (BS(input,0,x)) <| ByteString.take x (ByteString.create input) ) }

            test "test takeWhile should return an empty ArraySegment when given an empty ArraySegment" {
                Expect.equal "empty ByteString" ByteString.empty <| ByteString.takeWhile ((<>) ' 'B) ByteString.empty }

            test "test takeWhile should take anything before the first space" {
                let input = "Hello world"B
                Expect.equal "ByteString" (BS(input, 0, 5)) <| (ByteString.takeWhile ((<>) ' 'B) (ByteString.create input)) }

            test "test takeUntil should return an empty ArraySegment when given an empty ArraySegment" {
                Expect.equal "empty ByteString" ByteString.empty <| ByteString.takeUntil ((=) ' 'B) ByteString.empty }

            test "test takeUntil should correctly split the input" {
                let input = "abcde"B
                Expect.equal "ByteString" (BS(input, 0, 2)) <| ByteString.takeUntil ((=) 'c'B) (ByteString.create input) }
            
            testProperty "test if ByteString Compare follows lexicographic order" <| fun xs ys ->
                BS.Compare(ByteString xs, ByteString ys) = LanguagePrimitives.GenericComparison xs ys
            
            testSequenced <| testList "performance" [
                test "Comparision should only compare relevent part of Array" {
                    let veryLargeArray1 = Array.init 2000 byte |> ByteString.create
                    let veryLargeArray2 = Array.init 2000 byte |> ByteString.create
                    let compareVeryLargeArray () = BS.Compare(veryLargeArray1, veryLargeArray2) 
                    
                    let theExactSameByteString1 = ByteString.take 2000 veryLargeArray1
                    let theExactSameByteString2 = ByteString.take 2000 veryLargeArray1
                    let compareExactSameByteArray () = BS.Compare(theExactSameByteString1, theExactSameByteString2) 

                    let smallByteArray1 = ByteString.take 100 veryLargeArray1
                    let smallByteArray2 = ByteString.take 100 veryLargeArray2
                    let compareSmallByteArray () = BS.Compare(smallByteArray1, smallByteArray2) 
                    
                    Expect.equal "" (compareVeryLargeArray()) (compareExactSameByteArray())
                    compareExactSameByteArray |> Expect.isFasterThan "" compareVeryLargeArray
                    compareExactSameByteArray |> Expect.isFasterThan "" compareSmallByteArray
                    compareSmallByteArray |> Expect.isFasterThan "" compareVeryLargeArray
                }
            ]
        ]
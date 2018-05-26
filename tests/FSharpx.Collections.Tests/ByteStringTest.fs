namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Expecto
open Expecto.Flip

module ByteStringTests =
    type BS = ByteString

    let comparisonTests = [|
          [| box (ByteString.create ""B); box (ByteString.create ""B); box 0 |]
          [| box (ByteString.create "a"B); box (ByteString.create "a"B); box 0 |]
          [| box (ByteString.create "a"B); box (ByteString.create "b"B); box -1 |]
          [| box (ByteString.create "b"B); box (ByteString.create "a"B); box 1 |]
        |]
  
    let spanAndSplitTests = [|
            [| box "Howdy! Want to play?"B; box ' 'B; box 6 |]
            [| box "Howdy! Want to play?"B; box '?'B; box 19 |]
            [| box "Howdy! Want to play?"B; box '\r'B; box 20 |]
        |]

    [<Tests>]
    let testByteString =
        testList "ByteString" [
            test "test ByteString comparison should correctly return -1, 0, or 1" {
                comparisonTests
                |> Array.iter (fun x -> BS.Compare(unbox x.[0], unbox x.[1]) |> (Expect.equal "comparison" <| unbox x.[2]) ) }

            test "test ByteString_length should return the length of the byte string" {
              let input = ByteString.create "Hello, world!"B
              Expect.equal "length" 13 <| ByteString.length input }

            test "test ByteString_span correctly breaks the ByteString on the specified predicate" {
                spanAndSplitTests
                |> Array.iter (fun x -> 
                    let input = unbox x.[0]
                    let breakChar = unbox x.[1]
                    let breakIndex = unbox x.[2]
                    let str = ByteString.create input
                    let expected = if input.Length = breakIndex then str, ByteString.empty
                                    else BS(input, 0, breakIndex), BS(input, breakIndex, input.Length - breakIndex)
                    Expect.equal "ByteString * ByteString" expected <| ByteString.span ((<>) breakChar) str ) }

            test "test ByteString_split correctly breaks the ByteString on the specified predicate" {
                spanAndSplitTests
                |> Array.iter (fun x -> 
                    let input = unbox x.[0]
                    let breakChar = unbox x.[1]
                    let breakIndex = unbox x.[2]
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
        ]
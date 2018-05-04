namespace FSharpx.Collections.Tests

open System
open FSharpx
open FSharpx.Collections
open FSharpx.Collections.ByteString
open Expecto
open Expecto.Flip

module ByteStringTests =
    type BS = ByteString

    let comparisonTests = [|
          [| box (create ""B); box (create ""B); box 0 |]
          [| box (create "a"B); box (create "a"B); box 0 |]
          [| box (create "a"B); box (create "b"B); box -1 |]
          [| box (create "b"B); box (create "a"B); box 1 |]
        |]
  
    let spanAndSplitTests = [|
            [| box "Howdy! Want to play?"B; box ' 'B; box 6 |]
            [| box "Howdy! Want to play?"B; box '?'B; box 19 |]
            [| box "Howdy! Want to play?"B; box '\r'B; box 20 |]
        |]

    [<Tests>]
    let testByteString =
        testList "ByteString" [
            testCase "test ByteString comparison should correctly return -1, 0, or 1"  <| fun () ->
                comparisonTests
                |> Array.iter (fun x -> BS.Compare(unbox x.[0], unbox x.[1]) |> (Expect.equal "comparison" <| unbox x.[3]) )

            testCase "test ByteString_length should return the length of the byte string" <| fun () ->
              let input = create "Hello, world!"B
              length input |> (Expect.equal "length" 13)

            testCase "test ByteString_span correctly breaks the ByteString on the specified predicate" <| fun () ->
                spanAndSplitTests
                |> Array.iter (fun x -> 
                    let input = unbox x.[0]
                    let breakChar = unbox x.[1]
                    let breakIndex = unbox x.[2]
                    let str = create input
                    let expected = if input.Length = breakIndex then str, empty
                                    else BS(input, 0, breakIndex), BS(input, breakIndex, input.Length - breakIndex)
                    let actual = span ((<>) breakChar) str
                    actual |> (Expect.equal "ByteString * ByteString" expected) )

            testCase "test ByteString_split correctly breaks the ByteString on the specified predicate"  <| fun () ->
                spanAndSplitTests
                |> Array.iter (fun x -> 
                    let input = unbox x.[0]
                    let breakChar = unbox x.[1]
                    let breakIndex = unbox x.[2]
                    let str = create input
                    let expected = if input.Length = breakIndex then str, empty
                                    else BS(input, 0, breakIndex), BS(input, breakIndex, input.Length - breakIndex)
                    let actual = split ((=) breakChar) str
                    actual |> (Expect.equal "ByteString * ByteString" expected) )

            //[<Test>]
            //testCase "test ByteString_span correctly breaks the ByteString on \r" <| fun () ->
            //  let input = "test\r\ntest"B
            //  let str = create input
            //  let expected = BS(input, 0, 4), BS(input, 4, 6)
            //  let actual = span (fun c -> c <> '\r'B && c <> '\n'B) str
            //  actual |> should equal expected

            //[<Test>]
            //testCase "test ByteString_split correctly breaks the ByteString on \r" <| fun () ->
            //  let input = "test\r\ntest"B
            //  let str = create input
            //  let expected = BS(input, 0, 4), BS(input, 4, 6)
            //  let actual = split (fun c -> c = '\r'B || c = '\n'B) str
            //  actual |> should equal expected

            //[<Test>]
            //testCase "test ByteString_splitAt correctly breaks the ByteString on the specified index" <| fun () ->
            //  let input = "Howdy! Want to play?"B
            //  let str = create input
            //  let expected = BS(input, 0, 6), BS(input, 6, 14)
            //  let actual = splitAt 6 str
            //  actual |> should equal expected

            //[<Test>]
            //testCase "test ByteString_fold should concatenate bytes into a string" <| fun () ->
            //  create "Howdy"B
            //  |> fold (fun a b -> a + (char b).ToString()) ""
            //  |> should equal "Howdy"

            //[<Test>]
            //testCase "test ByteString_take correctly truncates the ByteString at the selected index" <| fun () ->
            //  let input = "Howdy! Want to play?"B
            //  let str = create input
            //  let expected = BS(input, 0, 6)
            //  let actual = take 6 str
            //  actual |> should equal expected

            //[<Test>]
            //[<Sequential>]
            //testCase "test drop should drop the first n items" <| fun () ->
            //([<Values(0,1,2,3,4,5,6,7,8,9)>] x) =
            //  let input = "Howdy! Want to play?"B
            //  let actual = skip 7 (create input)
            //  actual |> should equal (BS(input,7,13))

            //[<Test>]
            //testCase "test dropWhile should drop anything before the first space" <| fun () ->
            //  let input = create "Howdy! Want to play?"B
            //  let dropWhile2Head = skipWhile ((<>) ' 'B) >> head
            //  let actual = dropWhile2Head input
            //  actual |> should equal ' 'B

            //[<Test>]
            //testCase "test take should return an empty ArraySegment when asked to take 0" <| fun () ->
            //  let actual = take 0 (create "Nothing should be taken"B)
            //  actual |> should equal empty

            //[<Test>]
            //testCase "test take should return an empty ArraySegment when given an empty ArraySegment" <| fun () ->
            //  let actual = take 4 empty
            //  actual |> should equal empty

            //[<Test>]
            //[<Sequential>]
            //testCase "test take should take the first n items" <| fun () ->
            //([<Values(1,2,3,4,5,6,7,8,9,10)>] x) =
            //  let input = [|0uy..9uy|]
            //  let expected = BS(input,0,x)
            //  let actual = take x (create input)
            //  actual |> should equal expected

            //[<Test>]
            //testCase "test takeWhile should return an empty ArraySegment when given an empty ArraySegment" <| fun () ->
            //  let actual = takeWhile ((<>) ' 'B) empty
            //  actual |> should equal empty

            //[<Test>]
            //testCase "test takeWhile should take anything before the first space" <| fun () ->
            //  let input = "Hello world"B
            //  let actual = takeWhile ((<>) ' 'B) (create input)
            //  actual |> should equal (BS(input, 0, 5))

            //[<Test>]
            //testCase "test takeUntil should return an empty ArraySegment when given an empty ArraySegment" <| fun () ->
            //  let actual = takeUntil ((=) ' 'B) empty
            //  actual |> should equal empty

            //[<Test>]
            //testCase "test takeUntil should correctly split the input" <| fun () ->
            //  let input = "abcde"B
            //  let actual = takeUntil ((=) 'c'B) (create input)
            //  actual |> should equal (BS(input, 0, 2))

        ]
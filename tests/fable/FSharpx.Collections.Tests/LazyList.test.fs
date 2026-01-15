module LazyListTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList "LazyListTests" [
        test "LazyList works" {
            let xs = LazyList.empty

            Expect.equal (LazyList.isEmpty xs) true "should be empty"

            let xs = LazyList.ofList [ 1; 2; 3 ]

            Expect.equal (LazyList.isEmpty xs) false "should not be empty"
            Expect.equal (LazyList.tryHead xs) (Some 1) "should get first element"

            let xs = xs |> LazyList.tail |> LazyList.tail

            Expect.equal (LazyList.tryHead xs) (Some 3) "should reach third element"
        }
    ]

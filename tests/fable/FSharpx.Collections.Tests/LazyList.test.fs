module LazyListTests

open Fable.Mocha
open FSharpx.Collections

let tests =
    testList
        "LazyListTests"
        [ test "LazyList works" {
              let xs = LazyList.empty

              Expect.equal (LazyList.isEmpty xs) true "should be empty"

              let xs = LazyList.ofList [ 1; 2; 3 ]

              Expect.equal (LazyList.isEmpty xs) false "should not be empty"
              Expect.equal (LazyList.tryHead xs) (Some 1) "should get first element"

              let xs = xs |> LazyList.tail |> LazyList.tail

              Expect.equal (LazyList.tryHead xs) (Some 3) "should reach third element"
          }

          test "@@ head" {
              let tail = lazy (LazyList.ofList [ 2; 3 ])
              Expect.equal (LazyList.head(1 @@ tail)) 1 "@@ head"
          }

          test "@@ toList" {
              let tail = lazy (LazyList.ofList [ 2; 3 ])
              Expect.equal (LazyList.toList(1 @@ tail)) [ 1; 2; 3 ] "@@ toList"
          }

          test "@@ infinite" {
              let rec ones: LazyList<int> = 1 @@ lazy ones
              Expect.equal (LazyList.take 5 ones |> LazyList.toList) [ 1; 1; 1; 1; 1 ] "@@ infinite"
          } ]

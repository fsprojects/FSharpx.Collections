namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module DListTest =

    [<Tests>]
    let testDList =

        let expected =
            Join(Unit 0, Join(Unit 1, Join(Unit 2, Join(Unit 3, Unit 4, 2), 3), 4), 5)

        let expected2 = Join(Unit 1, Join(Unit 2, Join(Unit 3, Unit 4, 2), 3), 4)

        testList "Experimental DList" [
            test "test should verify DList.empty is Nil" { DList.empty<_> |> Expect.equal "" DList.Nil }

            test "test DList.length should return 5" { DList.length expected |> Expect.equal "" 5 }

            ptest "test ofSeq should create a DList from a seq" {
                let test = seq { for i in 0..4 -> i }
                DList.ofSeq test |> Expect.equal "" expected
            }

            ptest "test ofSeq should create a DList from a list" {
                let test = [ for i in 0..4 -> i ]
                DList.ofSeq test |> Expect.equal "" expected
            }

            ptest "test ofSeq should create a DList from an array" {
                let test = [| for i in 0..4 -> i |]
                DList.ofSeq test |> Expect.equal "" expected
            }

            test "test DList.cons should prepend 10 to the front of the original list" {
                DList.cons 10 expected |> Expect.equal "" (Join(Unit 10, expected, 6))
            }

            test "test DList.singleton should return a Unit containing the solo value" { DList.singleton 1 |> Expect.equal "" (Unit 1) }

            test "test DList.cons should return a Unit when the tail is Nil" { DList.cons 1 DList.empty |> Expect.equal "" (Unit 1) }

            test "test subsequent DList.cons should create a DList just as the constructor functions" {
                DList.cons 0 (DList.cons 1 (DList.cons 2 (DList.cons 3 (DList.cons 4 DList.empty))))
                |> Expect.equal "" expected
            }

            test "test DList.append should join two DLists together" {
                DList.append expected expected2
                |> Expect.equal "" (Join(expected, expected2, 9))
            }

            test "test DList.snoc should join DList and one element together" { DList.snoc expected 5 |> Expect.equal "" (Join(expected, Unit 5, 6)) }

            test "test head should return the first item in the DList" { DList.head(DList.append expected expected) |> Expect.equal "" 0 }

            test "test tail should return all items except the head" {
                DList.tail(DList.append expected expected)
                |> Expect.equal "" (Join(DList.cons 1 (DList.cons 2 (DList.cons 3 (DList.cons 4 DList.empty))), expected, 9))
            }

            test "test DList should respond to Seq functions such as map" {
                let testmap x = x * x
                let actual = Seq.map testmap (DList.append expected expected)

                let expected =
                    seq {
                        yield 0
                        yield 1
                        yield 2
                        yield 3
                        yield 4
                        yield 0
                        yield 1
                        yield 2
                        yield 3
                        yield 4
                    }
                    |> Seq.map testmap

                Expect.sequenceEqual "" expected actual
            }
        ]

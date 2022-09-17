namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip


//quite a lot going on and difficult to reason about edge cases
//testing up to RealTimeDeque.length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)
//6 makes front and back lists 3 long when C = 2

module RealTimeDequeTest =

    let len1 = RealTimeDeque.singleton "a"
    let len2 = RealTimeDeque.singleton "a" |> RealTimeDeque.cons "b"

    let len3 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"

    let len4 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"

    let len5 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"

    let len6 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"

    let len7 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"

    let len8 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"

    let len9 =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"
        |> RealTimeDeque.cons "i"

    let lena =
        RealTimeDeque.singleton "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"
        |> RealTimeDeque.cons "i"
        |> RealTimeDeque.cons "j"

    let len1snoc = RealTimeDeque.singleton "a"
    let len2snoc = RealTimeDeque.singleton "b" |> RealTimeDeque.snoc "a"

    let len3snoc =
        RealTimeDeque.singleton "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len4snoc =
        RealTimeDeque.singleton "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len5snoc =
        RealTimeDeque.singleton "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len6snoc =
        RealTimeDeque.singleton "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len7snoc =
        RealTimeDeque.singleton "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len8snoc =
        RealTimeDeque.singleton "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len9snoc =
        RealTimeDeque.singleton "i"
        |> RealTimeDeque.snoc "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let lenasnoc =
        RealTimeDeque.singleton "j"
        |> RealTimeDeque.snoc "i"
        |> RealTimeDeque.snoc "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len1C3 = RealTimeDeque.empty 3 |> RealTimeDeque.cons "a"

    let len2C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"

    let len3C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"

    let len4C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"

    let len5C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"

    let len6C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"

    let len7C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"

    let len8C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"

    let len9C3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"
        |> RealTimeDeque.cons "i"

    let lenaC3 =
        RealTimeDeque.empty 3
        |> RealTimeDeque.cons "a"
        |> RealTimeDeque.cons "b"
        |> RealTimeDeque.cons "c"
        |> RealTimeDeque.cons "d"
        |> RealTimeDeque.cons "e"
        |> RealTimeDeque.cons "f"
        |> RealTimeDeque.cons "g"
        |> RealTimeDeque.cons "h"
        |> RealTimeDeque.cons "i"
        |> RealTimeDeque.cons "j"

    let len1C3snoc = RealTimeDeque.empty 3 |> RealTimeDeque.snoc "a"

    let len2C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len3C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len4C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len5C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len6C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len7C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len8C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let len9C3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "i"
        |> RealTimeDeque.snoc "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    let lenaC3snoc =
        RealTimeDeque.empty 3
        |> RealTimeDeque.snoc "j"
        |> RealTimeDeque.snoc "i"
        |> RealTimeDeque.snoc "h"
        |> RealTimeDeque.snoc "g"
        |> RealTimeDeque.snoc "f"
        |> RealTimeDeque.snoc "e"
        |> RealTimeDeque.snoc "d"
        |> RealTimeDeque.snoc "c"
        |> RealTimeDeque.snoc "b"
        |> RealTimeDeque.snoc "a"

    [<Tests>]
    let testRealTimeDeque =

        testList "Experimental RealTimeDeque" [ test "RealTimeDeque.empty dqueue should be RealTimeDeque.empty" {
                                                    RealTimeDeque.isEmpty(RealTimeDeque.empty 2) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.cons works" {
                                                    ((len2 |> RealTimeDeque.isEmpty) && (len2C3 |> RealTimeDeque.isEmpty))
                                                    |> Expect.isFalse ""
                                                }

                                                test "RealTimeDeque.snoc works" {
                                                    ((len2snoc |> RealTimeDeque.isEmpty)
                                                     && (len2C3snoc |> RealTimeDeque.isEmpty))
                                                    |> Expect.isFalse ""
                                                }

                                                test "RealTimeDeque.singleton RealTimeDeque.head works" {
                                                    (((RealTimeDeque.head len1) = "a")
                                                     && ((len1C3 |> RealTimeDeque.isEmpty)) = false)
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.singleton RealTimeDeque.last works" {
                                                    len1 |> RealTimeDeque.last |> Expect.equal "" "a"
                                                }

                                                test "RealTimeDeque.tail of RealTimeDeque.singleton RealTimeDeque.empty" {
                                                    len1
                                                    |> RealTimeDeque.tail
                                                    |> RealTimeDeque.isEmpty
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tail of RealTimeDeque.tail of 2 RealTimeDeque.empty" {
                                                    (len2
                                                     |> RealTimeDeque.tail
                                                     |> RealTimeDeque.tail
                                                     |> RealTimeDeque.isEmpty)
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.init of RealTimeDeque.singleton RealTimeDeque.empty" {
                                                    ((RealTimeDeque.init len1) |> RealTimeDeque.isEmpty)
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 1" {
                                                    let t1 = RealTimeDeque.tail len2
                                                    let t1C = RealTimeDeque.tail len2C3
                                                    let t1s = RealTimeDeque.tail len2snoc
                                                    let t1Cs = RealTimeDeque.tail len2C3snoc
                                                    let ht1 = RealTimeDeque.head t1
                                                    let ht1C = RealTimeDeque.head t1C
                                                    let ht1s = RealTimeDeque.head t1s
                                                    let ht1Cs = RealTimeDeque.head t1Cs

                                                    (((RealTimeDeque.length t1) = 1)
                                                     && ((RealTimeDeque.length t1C) = 1)
                                                     && ((RealTimeDeque.length t1s) = 1)
                                                     && ((RealTimeDeque.length t1Cs) = 1)
                                                     && (ht1 = "a")
                                                     && (ht1C = "a")
                                                     && (ht1s = "a")
                                                     && (ht1Cs = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 2" {
                                                    let t1 = RealTimeDeque.tail len3
                                                    let t1C = RealTimeDeque.tail len3C3
                                                    let t1s = RealTimeDeque.tail len3snoc
                                                    let t1Cs = RealTimeDeque.tail len3C3snoc

                                                    let t1_1 = RealTimeDeque.tail t1
                                                    let t1C_1 = RealTimeDeque.tail t1C
                                                    let t1_1s = RealTimeDeque.tail t1s
                                                    let t1C_1s = RealTimeDeque.tail t1Cs

                                                    (((RealTimeDeque.length t1) = 2)
                                                     && ((RealTimeDeque.length t1C) = 2)
                                                     && ((RealTimeDeque.length t1s) = 2)
                                                     && ((RealTimeDeque.length t1Cs) = 2)
                                                     && ((RealTimeDeque.head t1) = "b")
                                                     && ((RealTimeDeque.head t1C) = "b")
                                                     && ((RealTimeDeque.head t1s) = "b")
                                                     && ((RealTimeDeque.head t1Cs) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 3" {
                                                    let t1 = RealTimeDeque.tail len4
                                                    let t1C = RealTimeDeque.tail len4C3
                                                    let t1s = RealTimeDeque.tail len4snoc
                                                    let t1Cs = RealTimeDeque.tail len4C3snoc

                                                    let t1_2 = RealTimeDeque.tail t1
                                                    let t1C_2 = RealTimeDeque.tail t1C
                                                    let t1_2s = RealTimeDeque.tail t1s
                                                    let t1C_2s = RealTimeDeque.tail t1Cs

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 3)
                                                     && ((RealTimeDeque.length t1C) = 3)
                                                     && ((RealTimeDeque.length t1s) = 3)
                                                     && ((RealTimeDeque.length t1Cs) = 3)
                                                     && ((RealTimeDeque.head t1) = "c")
                                                     && ((RealTimeDeque.head t1C) = "c")
                                                     && ((RealTimeDeque.head t1s) = "c")
                                                     && ((RealTimeDeque.head t1Cs) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 4" {
                                                    let t1 = RealTimeDeque.tail len5
                                                    let t1C = RealTimeDeque.tail len5C3
                                                    let t1s = RealTimeDeque.tail len5snoc
                                                    let t1Cs = RealTimeDeque.tail len5C3snoc

                                                    let t1_3 = RealTimeDeque.tail t1
                                                    let t1C_3 = RealTimeDeque.tail t1C
                                                    let t1_3s = RealTimeDeque.tail t1s
                                                    let t1C_3s = RealTimeDeque.tail t1Cs

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 4)
                                                     && ((RealTimeDeque.length t1C) = 4)
                                                     && ((RealTimeDeque.length t1s) = 4)
                                                     && ((RealTimeDeque.length t1Cs) = 4)
                                                     && ((RealTimeDeque.head t1) = "d")
                                                     && ((RealTimeDeque.head t1C) = "d")
                                                     && ((RealTimeDeque.head t1s) = "d")
                                                     && ((RealTimeDeque.head t1Cs) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 5" {
                                                    let t1 = RealTimeDeque.tail len6
                                                    let t1C = RealTimeDeque.tail len6C3
                                                    let t1s = RealTimeDeque.tail len6snoc
                                                    let t1Cs = RealTimeDeque.tail len6C3snoc

                                                    let t1_4 = RealTimeDeque.tail t1
                                                    let t1C_4 = RealTimeDeque.tail t1C
                                                    let t1_4s = RealTimeDeque.tail t1s
                                                    let t1C_4s = RealTimeDeque.tail t1Cs

                                                    let t1_3 = RealTimeDeque.tail t1_4
                                                    let t1C_3 = RealTimeDeque.tail t1C_4
                                                    let t1_3s = RealTimeDeque.tail t1_4s
                                                    let t1C_3s = RealTimeDeque.tail t1C_4s

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 5)
                                                     && ((RealTimeDeque.length t1C) = 5)
                                                     && ((RealTimeDeque.length t1s) = 5)
                                                     && ((RealTimeDeque.length t1Cs) = 5)
                                                     && ((RealTimeDeque.head t1) = "e")
                                                     && ((RealTimeDeque.head t1C) = "e")
                                                     && ((RealTimeDeque.head t1s) = "e")
                                                     && ((RealTimeDeque.head t1Cs) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.length t1_4s) = 4)
                                                     && ((RealTimeDeque.length t1C_4s) = 4)
                                                     && ((RealTimeDeque.head t1_4) = "d")
                                                     && ((RealTimeDeque.head t1C_4) = "d")
                                                     && ((RealTimeDeque.head t1_4s) = "d")
                                                     && ((RealTimeDeque.head t1C_4s) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 6" {
                                                    let t1 = RealTimeDeque.tail len7
                                                    let t1C = RealTimeDeque.tail len7C3
                                                    let t1s = RealTimeDeque.tail len7snoc
                                                    let t1Cs = RealTimeDeque.tail len7C3snoc

                                                    let t1_5 = RealTimeDeque.tail t1
                                                    let t1C_5 = RealTimeDeque.tail t1C
                                                    let t1_5s = RealTimeDeque.tail t1s
                                                    let t1C_5s = RealTimeDeque.tail t1Cs

                                                    let t1_4 = RealTimeDeque.tail t1_5
                                                    let t1C_4 = RealTimeDeque.tail t1C_5
                                                    let t1_4s = RealTimeDeque.tail t1_5s
                                                    let t1C_4s = RealTimeDeque.tail t1C_5s

                                                    let t1_3 = RealTimeDeque.tail t1_4
                                                    let t1C_3 = RealTimeDeque.tail t1C_4
                                                    let t1_3s = RealTimeDeque.tail t1_4s
                                                    let t1C_3s = RealTimeDeque.tail t1C_4s

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 6)
                                                     && ((RealTimeDeque.length t1C) = 6)
                                                     && ((RealTimeDeque.length t1s) = 6)
                                                     && ((RealTimeDeque.length t1Cs) = 6)
                                                     && ((RealTimeDeque.head t1) = "f")
                                                     && ((RealTimeDeque.head t1C) = "f")
                                                     && ((RealTimeDeque.head t1s) = "f")
                                                     && ((RealTimeDeque.head t1Cs) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 5)
                                                     && ((RealTimeDeque.length t1C_5) = 5)
                                                     && ((RealTimeDeque.length t1_5s) = 5)
                                                     && ((RealTimeDeque.length t1C_5s) = 5)
                                                     && ((RealTimeDeque.head t1_5) = "e")
                                                     && ((RealTimeDeque.head t1C_5) = "e")
                                                     && ((RealTimeDeque.head t1_5s) = "e")
                                                     && ((RealTimeDeque.head t1C_5s) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.length t1_4s) = 4)
                                                     && ((RealTimeDeque.length t1C_4s) = 4)
                                                     && ((RealTimeDeque.head t1_4) = "d")
                                                     && ((RealTimeDeque.head t1C_4) = "d")
                                                     && ((RealTimeDeque.head t1_4s) = "d")
                                                     && ((RealTimeDeque.head t1C_4s) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 7" {
                                                    let t1 = RealTimeDeque.tail len8
                                                    let t1C = RealTimeDeque.tail len8C3
                                                    let t1s = RealTimeDeque.tail len8snoc
                                                    let t1Cs = RealTimeDeque.tail len8C3snoc

                                                    let t1_6 = RealTimeDeque.tail t1
                                                    let t1C_6 = RealTimeDeque.tail t1C
                                                    let t1_6s = RealTimeDeque.tail t1s
                                                    let t1C_6s = RealTimeDeque.tail t1Cs

                                                    let t1_5 = RealTimeDeque.tail t1_6
                                                    let t1C_5 = RealTimeDeque.tail t1C_6
                                                    let t1_5s = RealTimeDeque.tail t1_6s
                                                    let t1C_5s = RealTimeDeque.tail t1C_6s

                                                    let t1_4 = RealTimeDeque.tail t1_5
                                                    let t1C_4 = RealTimeDeque.tail t1C_5
                                                    let t1_4s = RealTimeDeque.tail t1_5s
                                                    let t1C_4s = RealTimeDeque.tail t1C_5s

                                                    let t1_3 = RealTimeDeque.tail t1_4
                                                    let t1C_3 = RealTimeDeque.tail t1C_4
                                                    let t1_3s = RealTimeDeque.tail t1_4s
                                                    let t1C_3s = RealTimeDeque.tail t1C_4s

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 7)
                                                     && ((RealTimeDeque.length t1C) = 7)
                                                     && ((RealTimeDeque.length t1s) = 7)
                                                     && ((RealTimeDeque.length t1Cs) = 7)
                                                     && ((RealTimeDeque.head t1) = "g")
                                                     && ((RealTimeDeque.head t1C) = "g")
                                                     && ((RealTimeDeque.head t1s) = "g")
                                                     && ((RealTimeDeque.head t1Cs) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 6)
                                                     && ((RealTimeDeque.length t1C_6) = 6)
                                                     && ((RealTimeDeque.length t1_6s) = 6)
                                                     && ((RealTimeDeque.length t1C_6s) = 6)
                                                     && ((RealTimeDeque.head t1_6) = "f")
                                                     && ((RealTimeDeque.head t1C_6) = "f")
                                                     && ((RealTimeDeque.head t1_6s) = "f")
                                                     && ((RealTimeDeque.head t1C_6s) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 5)
                                                     && ((RealTimeDeque.length t1C_5) = 5)
                                                     && ((RealTimeDeque.length t1_5s) = 5)
                                                     && ((RealTimeDeque.length t1C_5s) = 5)
                                                     && ((RealTimeDeque.head t1_5) = "e")
                                                     && ((RealTimeDeque.head t1C_5) = "e")
                                                     && ((RealTimeDeque.head t1_5s) = "e")
                                                     && ((RealTimeDeque.head t1C_5s) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.length t1_4s) = 4)
                                                     && ((RealTimeDeque.length t1C_4s) = 4)
                                                     && ((RealTimeDeque.head t1_4) = "d")
                                                     && ((RealTimeDeque.head t1C_4) = "d")
                                                     && ((RealTimeDeque.head t1_4s) = "d")
                                                     && ((RealTimeDeque.head t1C_4s) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 8" {
                                                    let t1 = RealTimeDeque.tail len9
                                                    let t1C = RealTimeDeque.tail len9C3
                                                    let t1s = RealTimeDeque.tail len9snoc
                                                    let t1Cs = RealTimeDeque.tail len9C3snoc

                                                    let t1_7 = RealTimeDeque.tail t1
                                                    let t1C_7 = RealTimeDeque.tail t1C
                                                    let t1_7s = RealTimeDeque.tail t1s
                                                    let t1C_7s = RealTimeDeque.tail t1Cs

                                                    let t1_6 = RealTimeDeque.tail t1_7
                                                    let t1C_6 = RealTimeDeque.tail t1C_7
                                                    let t1_6s = RealTimeDeque.tail t1_7s
                                                    let t1C_6s = RealTimeDeque.tail t1C_7s

                                                    let t1_5 = RealTimeDeque.tail t1_6
                                                    let t1C_5 = RealTimeDeque.tail t1C_6
                                                    let t1_5s = RealTimeDeque.tail t1_6s
                                                    let t1C_5s = RealTimeDeque.tail t1C_6s

                                                    let t1_4 = RealTimeDeque.tail t1_5
                                                    let t1C_4 = RealTimeDeque.tail t1C_5
                                                    let t1_4s = RealTimeDeque.tail t1_5s
                                                    let t1C_4s = RealTimeDeque.tail t1C_5s

                                                    let t1_3 = RealTimeDeque.tail t1_4
                                                    let t1C_3 = RealTimeDeque.tail t1C_4
                                                    let t1_3s = RealTimeDeque.tail t1_4s
                                                    let t1C_3s = RealTimeDeque.tail t1C_4s

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 8)
                                                     && ((RealTimeDeque.length t1C) = 8)
                                                     && ((RealTimeDeque.length t1s) = 8)
                                                     && ((RealTimeDeque.length t1Cs) = 8)
                                                     && ((RealTimeDeque.head t1) = "h")
                                                     && ((RealTimeDeque.head t1C) = "h")
                                                     && ((RealTimeDeque.head t1s) = "h")
                                                     && ((RealTimeDeque.head t1Cs) = "h")
                                                     && ((RealTimeDeque.length t1_7) = 7)
                                                     && ((RealTimeDeque.length t1C_7) = 7)
                                                     && ((RealTimeDeque.length t1_7s) = 7)
                                                     && ((RealTimeDeque.length t1C_7s) = 7)
                                                     && ((RealTimeDeque.head t1_7) = "g")
                                                     && ((RealTimeDeque.head t1C_7) = "g")
                                                     && ((RealTimeDeque.head t1_7s) = "g")
                                                     && ((RealTimeDeque.head t1C_7s) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 6)
                                                     && ((RealTimeDeque.length t1C_6) = 6)
                                                     && ((RealTimeDeque.length t1_6s) = 6)
                                                     && ((RealTimeDeque.length t1C_6s) = 6)
                                                     && ((RealTimeDeque.head t1_6) = "f")
                                                     && ((RealTimeDeque.head t1C_6) = "f")
                                                     && ((RealTimeDeque.head t1_6s) = "f")
                                                     && ((RealTimeDeque.head t1C_6s) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 5)
                                                     && ((RealTimeDeque.length t1C_5) = 5)
                                                     && ((RealTimeDeque.length t1_5s) = 5)
                                                     && ((RealTimeDeque.length t1C_5s) = 5)
                                                     && ((RealTimeDeque.head t1_5) = "e")
                                                     && ((RealTimeDeque.head t1C_5) = "e")
                                                     && ((RealTimeDeque.head t1_5s) = "e")
                                                     && ((RealTimeDeque.head t1C_5s) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.length t1_4s) = 4)
                                                     && ((RealTimeDeque.length t1C_4s) = 4)
                                                     && ((RealTimeDeque.head t1_4) = "d")
                                                     && ((RealTimeDeque.head t1C_4) = "d")
                                                     && ((RealTimeDeque.head t1_4s) = "d")
                                                     && ((RealTimeDeque.head t1C_4s) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.head, RealTimeDeque.tail, and RealTimeDeque.length work test 9" {
                                                    let t1 = RealTimeDeque.tail lena
                                                    let t1C = RealTimeDeque.tail lenaC3
                                                    let t1s = RealTimeDeque.tail lenasnoc
                                                    let t1Cs = RealTimeDeque.tail lenaC3snoc

                                                    let t1_8 = RealTimeDeque.tail t1
                                                    let t1C_8 = RealTimeDeque.tail t1C
                                                    let t1_8s = RealTimeDeque.tail t1s
                                                    let t1C_8s = RealTimeDeque.tail t1Cs

                                                    let t1_7 = RealTimeDeque.tail t1_8
                                                    let t1C_7 = RealTimeDeque.tail t1C_8
                                                    let t1_7s = RealTimeDeque.tail t1_8s
                                                    let t1C_7s = RealTimeDeque.tail t1C_8s

                                                    let t1_6 = RealTimeDeque.tail t1_7
                                                    let t1C_6 = RealTimeDeque.tail t1C_7
                                                    let t1_6s = RealTimeDeque.tail t1_7s
                                                    let t1C_6s = RealTimeDeque.tail t1C_7s

                                                    let t1_5 = RealTimeDeque.tail t1_6
                                                    let t1C_5 = RealTimeDeque.tail t1C_6
                                                    let t1_5s = RealTimeDeque.tail t1_6s
                                                    let t1C_5s = RealTimeDeque.tail t1C_6s

                                                    let t1_4 = RealTimeDeque.tail t1_5
                                                    let t1C_4 = RealTimeDeque.tail t1C_5
                                                    let t1_4s = RealTimeDeque.tail t1_5s
                                                    let t1C_4s = RealTimeDeque.tail t1C_5s

                                                    let t1_3 = RealTimeDeque.tail t1_4
                                                    let t1C_3 = RealTimeDeque.tail t1C_4
                                                    let t1_3s = RealTimeDeque.tail t1_4s
                                                    let t1C_3s = RealTimeDeque.tail t1C_4s

                                                    let t1_2 = RealTimeDeque.tail t1_3
                                                    let t1C_2 = RealTimeDeque.tail t1C_3
                                                    let t1_2s = RealTimeDeque.tail t1_3s
                                                    let t1C_2s = RealTimeDeque.tail t1C_3s

                                                    let t1_1 = RealTimeDeque.tail t1_2
                                                    let t1C_1 = RealTimeDeque.tail t1C_2
                                                    let t1_1s = RealTimeDeque.tail t1_2s
                                                    let t1C_1s = RealTimeDeque.tail t1C_2s

                                                    (((RealTimeDeque.length t1) = 9)
                                                     && ((RealTimeDeque.length t1C) = 9)
                                                     && ((RealTimeDeque.length t1s) = 9)
                                                     && ((RealTimeDeque.length t1Cs) = 9)
                                                     && ((RealTimeDeque.head t1) = "i")
                                                     && ((RealTimeDeque.head t1C) = "i")
                                                     && ((RealTimeDeque.head t1s) = "i")
                                                     && ((RealTimeDeque.head t1Cs) = "i")
                                                     && ((RealTimeDeque.length t1_8) = 8)
                                                     && ((RealTimeDeque.length t1C_8) = 8)
                                                     && ((RealTimeDeque.length t1_8s) = 8)
                                                     && ((RealTimeDeque.length t1C_8s) = 8)
                                                     && ((RealTimeDeque.head t1_8) = "h")
                                                     && ((RealTimeDeque.head t1C_8) = "h")
                                                     && ((RealTimeDeque.head t1_8s) = "h")
                                                     && ((RealTimeDeque.head t1C_8s) = "h")
                                                     && ((RealTimeDeque.length t1_7) = 7)
                                                     && ((RealTimeDeque.length t1C_7) = 7)
                                                     && ((RealTimeDeque.length t1_7s) = 7)
                                                     && ((RealTimeDeque.length t1C_7s) = 7)
                                                     && ((RealTimeDeque.head t1_7) = "g")
                                                     && ((RealTimeDeque.head t1C_7) = "g")
                                                     && ((RealTimeDeque.head t1_7s) = "g")
                                                     && ((RealTimeDeque.head t1C_7s) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 6)
                                                     && ((RealTimeDeque.length t1C_6) = 6)
                                                     && ((RealTimeDeque.length t1_6s) = 6)
                                                     && ((RealTimeDeque.length t1C_6s) = 6)
                                                     && ((RealTimeDeque.head t1_6) = "f")
                                                     && ((RealTimeDeque.head t1C_6) = "f")
                                                     && ((RealTimeDeque.head t1_6s) = "f")
                                                     && ((RealTimeDeque.head t1C_6s) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 5)
                                                     && ((RealTimeDeque.length t1C_5) = 5)
                                                     && ((RealTimeDeque.length t1_5s) = 5)
                                                     && ((RealTimeDeque.length t1C_5s) = 5)
                                                     && ((RealTimeDeque.head t1_5) = "e")
                                                     && ((RealTimeDeque.head t1C_5) = "e")
                                                     && ((RealTimeDeque.head t1_5s) = "e")
                                                     && ((RealTimeDeque.head t1C_5s) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.length t1_4s) = 4)
                                                     && ((RealTimeDeque.length t1C_4s) = 4)
                                                     && ((RealTimeDeque.head t1_4) = "d")
                                                     && ((RealTimeDeque.head t1C_4) = "d")
                                                     && ((RealTimeDeque.head t1_4s) = "d")
                                                     && ((RealTimeDeque.head t1C_4s) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.length t1_3s) = 3)
                                                     && ((RealTimeDeque.length t1C_3s) = 3)
                                                     && ((RealTimeDeque.head t1_3) = "c")
                                                     && ((RealTimeDeque.head t1C_3) = "c")
                                                     && ((RealTimeDeque.head t1_3s) = "c")
                                                     && ((RealTimeDeque.head t1C_3s) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.length t1_2s) = 2)
                                                     && ((RealTimeDeque.length t1C_2s) = 2)
                                                     && ((RealTimeDeque.head t1_2) = "b")
                                                     && ((RealTimeDeque.head t1C_2) = "b")
                                                     && ((RealTimeDeque.head t1_2s) = "b")
                                                     && ((RealTimeDeque.head t1C_2s) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && ((RealTimeDeque.length t1_1s) = 1)
                                                     && ((RealTimeDeque.length t1C_1s) = 1)
                                                     && ((RealTimeDeque.head t1_1) = "a")
                                                     && ((RealTimeDeque.head t1C_1) = "a")
                                                     && ((RealTimeDeque.head t1_1s) = "a")
                                                     && ((RealTimeDeque.head t1C_1s) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                //the previous series thoroughly tested construction by RealTimeDeque.snoc, so we'll leave those out
                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 1" {
                                                    let t1 = RealTimeDeque.init len2
                                                    let t1C = RealTimeDeque.init len2C3

                                                    (((RealTimeDeque.length t1) = 1)
                                                     && ((RealTimeDeque.length t1C) = 1)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 2" {
                                                    let t1 = RealTimeDeque.init len3
                                                    let t1C = RealTimeDeque.init len3C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C

                                                    let lt1 = (RealTimeDeque.last t1)
                                                    let lt1C = (RealTimeDeque.last t1C)
                                                    let lt1_1 = (RealTimeDeque.last t1_1)
                                                    let lt1C_1 = (RealTimeDeque.last t1C_1)

                                                    (((RealTimeDeque.length t1) = 2)
                                                     && ((RealTimeDeque.length t1C) = 2)
                                                     && (lt1 = "b")
                                                     && (lt1C = "b")
                                                     && ((RealTimeDeque.length t1_1) = 1)
                                                     && ((RealTimeDeque.length t1C_1) = 1)
                                                     && (lt1_1 = "c")
                                                     && (lt1C_1 = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 3" {
                                                    let t1 = RealTimeDeque.init len4
                                                    let t1C = RealTimeDeque.init len4C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1

                                                    (((RealTimeDeque.length t1) = 3)
                                                     && ((RealTimeDeque.length t1C) = 3)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 2)
                                                     && ((RealTimeDeque.length t1C_1) = 2)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 1)
                                                     && ((RealTimeDeque.length t1C_2) = 1)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 4" {
                                                    let t1 = RealTimeDeque.init len5
                                                    let t1C = RealTimeDeque.init len5C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2

                                                    (((RealTimeDeque.length t1) = 4)
                                                     && ((RealTimeDeque.length t1C) = 4)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 3)
                                                     && ((RealTimeDeque.length t1C_1) = 3)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 2)
                                                     && ((RealTimeDeque.length t1C_2) = 2)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 1)
                                                     && ((RealTimeDeque.length t1C_3) = 1)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 5" {
                                                    let t1 = RealTimeDeque.init len6
                                                    let t1C = RealTimeDeque.init len6C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2
                                                    let t1_4 = RealTimeDeque.init t1_3
                                                    let t1C_4 = RealTimeDeque.init t1C_3

                                                    (((RealTimeDeque.length t1) = 5)
                                                     && ((RealTimeDeque.length t1C) = 5)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 4)
                                                     && ((RealTimeDeque.length t1C_1) = 4)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 3)
                                                     && ((RealTimeDeque.length t1C_2) = 3)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 2)
                                                     && ((RealTimeDeque.length t1C_3) = 2)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 1)
                                                     && ((RealTimeDeque.length t1C_4) = 1)
                                                     && ((RealTimeDeque.last t1_4) = "f")
                                                     && ((RealTimeDeque.last t1C_4) = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 6" {
                                                    let t1 = RealTimeDeque.init len7
                                                    let t1C = RealTimeDeque.init len7C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2
                                                    let t1_4 = RealTimeDeque.init t1_3
                                                    let t1C_4 = RealTimeDeque.init t1C_3
                                                    let t1_5 = RealTimeDeque.init t1_4
                                                    let t1C_5 = RealTimeDeque.init t1C_4

                                                    (((RealTimeDeque.length t1) = 6)
                                                     && ((RealTimeDeque.length t1C) = 6)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 5)
                                                     && ((RealTimeDeque.length t1C_1) = 5)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 4)
                                                     && ((RealTimeDeque.length t1C_2) = 4)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 3)
                                                     && ((RealTimeDeque.length t1C_3) = 3)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 2)
                                                     && ((RealTimeDeque.length t1C_4) = 2)
                                                     && ((RealTimeDeque.last t1_4) = "f")
                                                     && ((RealTimeDeque.last t1C_4) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 1)
                                                     && ((RealTimeDeque.length t1C_5) = 1)
                                                     && ((RealTimeDeque.last t1_5) = "g")
                                                     && ((RealTimeDeque.last t1C_5) = "g"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 7" {
                                                    let t1 = RealTimeDeque.init len8
                                                    let t1C = RealTimeDeque.init len8C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2
                                                    let t1_4 = RealTimeDeque.init t1_3
                                                    let t1C_4 = RealTimeDeque.init t1C_3
                                                    let t1_5 = RealTimeDeque.init t1_4
                                                    let t1C_5 = RealTimeDeque.init t1C_4
                                                    let t1_6 = RealTimeDeque.init t1_5
                                                    let t1C_6 = RealTimeDeque.init t1C_5

                                                    (((RealTimeDeque.length t1) = 7)
                                                     && ((RealTimeDeque.length t1C) = 7)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 6)
                                                     && ((RealTimeDeque.length t1C_1) = 6)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 5)
                                                     && ((RealTimeDeque.length t1C_2) = 5)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 4)
                                                     && ((RealTimeDeque.length t1C_3) = 4)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 3)
                                                     && ((RealTimeDeque.length t1C_4) = 3)
                                                     && ((RealTimeDeque.last t1_4) = "f")
                                                     && ((RealTimeDeque.last t1C_4) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 2)
                                                     && ((RealTimeDeque.length t1C_5) = 2)
                                                     && ((RealTimeDeque.last t1_5) = "g")
                                                     && ((RealTimeDeque.last t1C_5) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 1)
                                                     && ((RealTimeDeque.length t1C_6) = 1)
                                                     && ((RealTimeDeque.last t1_6) = "h")
                                                     && ((RealTimeDeque.last t1C_6) = "h"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 8" {
                                                    let t1 = RealTimeDeque.init len9
                                                    let t1C = RealTimeDeque.init len9C3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2
                                                    let t1_4 = RealTimeDeque.init t1_3
                                                    let t1C_4 = RealTimeDeque.init t1C_3
                                                    let t1_5 = RealTimeDeque.init t1_4
                                                    let t1C_5 = RealTimeDeque.init t1C_4
                                                    let t1_6 = RealTimeDeque.init t1_5
                                                    let t1C_6 = RealTimeDeque.init t1C_5
                                                    let t1_7 = RealTimeDeque.init t1_6
                                                    let t1C_7 = RealTimeDeque.init t1C_6

                                                    (((RealTimeDeque.length t1) = 8)
                                                     && ((RealTimeDeque.length t1C) = 8)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 7)
                                                     && ((RealTimeDeque.length t1C_1) = 7)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 6)
                                                     && ((RealTimeDeque.length t1C_2) = 6)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 5)
                                                     && ((RealTimeDeque.length t1C_3) = 5)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 4)
                                                     && ((RealTimeDeque.length t1C_4) = 4)
                                                     && ((RealTimeDeque.last t1_4) = "f")
                                                     && ((RealTimeDeque.last t1C_4) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 3)
                                                     && ((RealTimeDeque.length t1C_5) = 3)
                                                     && ((RealTimeDeque.last t1_5) = "g")
                                                     && ((RealTimeDeque.last t1C_5) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 2)
                                                     && ((RealTimeDeque.length t1C_6) = 2)
                                                     && ((RealTimeDeque.last t1_6) = "h")
                                                     && ((RealTimeDeque.last t1C_6) = "h")
                                                     && ((RealTimeDeque.length t1_7) = 1)
                                                     && ((RealTimeDeque.length t1C_7) = 1)
                                                     && ((RealTimeDeque.last t1_7) = "i")
                                                     && ((RealTimeDeque.last t1C_7) = "i"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.last, RealTimeDeque.init, and RealTimeDeque.length work test 9" {
                                                    let t1 = RealTimeDeque.init lena
                                                    let t1C = RealTimeDeque.init lenaC3
                                                    let t1_1 = RealTimeDeque.init t1
                                                    let t1C_1 = RealTimeDeque.init t1C
                                                    let t1_2 = RealTimeDeque.init t1_1
                                                    let t1C_2 = RealTimeDeque.init t1C_1
                                                    let t1_3 = RealTimeDeque.init t1_2
                                                    let t1C_3 = RealTimeDeque.init t1C_2
                                                    let t1_4 = RealTimeDeque.init t1_3
                                                    let t1C_4 = RealTimeDeque.init t1C_3
                                                    let t1_5 = RealTimeDeque.init t1_4
                                                    let t1C_5 = RealTimeDeque.init t1C_4
                                                    let t1_6 = RealTimeDeque.init t1_5
                                                    let t1C_6 = RealTimeDeque.init t1C_5
                                                    let t1_7 = RealTimeDeque.init t1_6
                                                    let t1C_7 = RealTimeDeque.init t1C_6
                                                    let t1_8 = RealTimeDeque.init t1_7
                                                    let t1C_8 = RealTimeDeque.init t1C_7

                                                    (((RealTimeDeque.length t1) = 9)
                                                     && ((RealTimeDeque.length t1C) = 9)
                                                     && ((RealTimeDeque.last t1) = "b")
                                                     && ((RealTimeDeque.last t1C) = "b")
                                                     && ((RealTimeDeque.length t1_1) = 8)
                                                     && ((RealTimeDeque.length t1C_1) = 8)
                                                     && ((RealTimeDeque.last t1_1) = "c")
                                                     && ((RealTimeDeque.last t1C_1) = "c")
                                                     && ((RealTimeDeque.length t1_2) = 7)
                                                     && ((RealTimeDeque.length t1C_2) = 7)
                                                     && ((RealTimeDeque.last t1_2) = "d")
                                                     && ((RealTimeDeque.last t1C_2) = "d")
                                                     && ((RealTimeDeque.length t1_3) = 6)
                                                     && ((RealTimeDeque.length t1C_3) = 6)
                                                     && ((RealTimeDeque.last t1_3) = "e")
                                                     && ((RealTimeDeque.last t1C_3) = "e")
                                                     && ((RealTimeDeque.length t1_4) = 5)
                                                     && ((RealTimeDeque.length t1C_4) = 5)
                                                     && ((RealTimeDeque.last t1_4) = "f")
                                                     && ((RealTimeDeque.last t1C_4) = "f")
                                                     && ((RealTimeDeque.length t1_5) = 4)
                                                     && ((RealTimeDeque.length t1C_5) = 4)
                                                     && ((RealTimeDeque.last t1_5) = "g")
                                                     && ((RealTimeDeque.last t1C_5) = "g")
                                                     && ((RealTimeDeque.length t1_6) = 3)
                                                     && ((RealTimeDeque.length t1C_6) = 3)
                                                     && ((RealTimeDeque.last t1_6) = "h")
                                                     && ((RealTimeDeque.last t1C_6) = "h")
                                                     && ((RealTimeDeque.length t1_7) = 2)
                                                     && ((RealTimeDeque.length t1C_7) = 2)
                                                     && ((RealTimeDeque.last t1_7) = "i")
                                                     && ((RealTimeDeque.last t1C_7) = "i")
                                                     && ((RealTimeDeque.length t1_8) = 1)
                                                     && ((RealTimeDeque.length t1C_8) = 1)
                                                     && ((RealTimeDeque.last t1_8) = "j")
                                                     && ((RealTimeDeque.last t1C_8) = "j"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "IEnumerable Seq" { (lena |> Seq.toArray).[5] |> Expect.equal "" "e" }

                                                test "IEnumerable Seq RealTimeDeque.length" { lena |> Seq.length |> Expect.equal "" 10 }

                                                test "type RealTimeDeque.cons works" { lena.Cons "zz" |> RealTimeDeque.head |> Expect.equal "" "zz" }

                                                test "IDeque RealTimeDeque.cons works" {
                                                    ((lena :> IDeque<string>).Cons "zz").Head |> Expect.equal "" "zz"
                                                }

                                                test "RealTimeDeque.ofCatLists and RealTimeDeque.uncons" {
                                                    let d = RealTimeDeque.ofCatLists [ "a"; "b"; "c" ] [ "d"; "e"; "f" ]
                                                    let h1, t1 = RealTimeDeque.uncons d
                                                    let h2, t2 = RealTimeDeque.uncons t1
                                                    let h3, t3 = RealTimeDeque.uncons t2
                                                    let h4, t4 = RealTimeDeque.uncons t3
                                                    let h5, t5 = RealTimeDeque.uncons t4
                                                    let h6, t6 = RealTimeDeque.uncons t5

                                                    ((h1 = "a")
                                                     && (h2 = "b")
                                                     && (h3 = "c")
                                                     && (h4 = "d")
                                                     && (h5 = "e")
                                                     && (h6 = "f")
                                                     && (RealTimeDeque.isEmpty t6))
                                                    |> Expect.isTrue ""
                                                }

                                                test "ofCatSeqs and RealTimeDeque.uncons" {
                                                    let d = RealTimeDeque.ofCatSeqs (seq { 'a' .. 'c' }) (seq { 'd' .. 'f' })
                                                    let h1, t1 = RealTimeDeque.uncons d
                                                    let h2, t2 = RealTimeDeque.uncons t1
                                                    let h3, t3 = RealTimeDeque.uncons t2
                                                    let h4, t4 = RealTimeDeque.uncons t3
                                                    let h5, t5 = RealTimeDeque.uncons t4
                                                    let h6, t6 = RealTimeDeque.uncons t5

                                                    ((h1 = 'a')
                                                     && (h2 = 'b')
                                                     && (h3 = 'c')
                                                     && (h4 = 'd')
                                                     && (h5 = 'e')
                                                     && (h6 = 'f')
                                                     && (RealTimeDeque.isEmpty t6))
                                                    |> Expect.isTrue ""
                                                }

                                                test "unsnoc works" {
                                                    let d = RealTimeDeque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ]
                                                    let i1, l1 = RealTimeDeque.unsnoc d
                                                    let i2, l2 = RealTimeDeque.unsnoc i1
                                                    let i3, l3 = RealTimeDeque.unsnoc i2
                                                    let i4, l4 = RealTimeDeque.unsnoc i3
                                                    let i5, l5 = RealTimeDeque.unsnoc i4
                                                    let i6, l6 = RealTimeDeque.unsnoc i5

                                                    ((l1 = "a")
                                                     && (l2 = "b")
                                                     && (l3 = "c")
                                                     && (l4 = "d")
                                                     && (l5 = "e")
                                                     && (l6 = "f")
                                                     && (RealTimeDeque.isEmpty i6))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.snoc pattern discriminator" {
                                                    let d = (RealTimeDeque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])
                                                    let i1, l1 = RealTimeDeque.unsnoc d

                                                    let i2, l2 =
                                                        match i1 with
                                                        | RealTimeDeque.Snoc(i, l) -> i, l
                                                        | _ -> i1, "x"

                                                    ((l2 = "b") && ((RealTimeDeque.length i2) = 4)) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.cons pattern discriminator" {
                                                    let d = (RealTimeDeque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])
                                                    let h1, t1 = RealTimeDeque.uncons d

                                                    let h2, t2 =
                                                        match t1 with
                                                        | RealTimeDeque.Cons(h, t) -> h, t
                                                        | _ -> "x", t1

                                                    ((h2 = "e") && ((RealTimeDeque.length t2) = 4)) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.cons and RealTimeDeque.snoc pattern discriminator" {
                                                    let d = (RealTimeDeque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])

                                                    let mid1 =
                                                        match d with
                                                        | RealTimeDeque.Cons(h, RealTimeDeque.Snoc(i, l)) -> i
                                                        | _ -> d

                                                    let head, last =
                                                        match mid1 with
                                                        | RealTimeDeque.Cons(h, RealTimeDeque.Snoc(i, l)) -> h, l
                                                        | _ -> "x", "x"

                                                    ((head = "e") && (last = "b")) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev RealTimeDeque.empty dqueue should be RealTimeDeque.empty" {
                                                    RealTimeDeque.isEmpty(RealTimeDeque.rev(RealTimeDeque.empty 2))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 1" {
                                                    ((RealTimeDeque.head(RealTimeDeque.rev len1) = "a")
                                                     && (RealTimeDeque.head(RealTimeDeque.rev len1C3) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 2" {
                                                    let r1 = RealTimeDeque.rev len2
                                                    let r1c = RealTimeDeque.rev len2C3
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c

                                                    ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 3" {
                                                    let r1 = RealTimeDeque.rev len3
                                                    let r1c = RealTimeDeque.rev len3C3
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 4" {
                                                    let r1 = RealTimeDeque.rev len4
                                                    let r1c = RealTimeDeque.rev len4C3
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 5" {
                                                    let r1 = RealTimeDeque.rev len5
                                                    let r1c = RealTimeDeque.rev len5C3
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                //RealTimeDeque.length 6 more than sufficient to test RealTimeDeque.rev
                                                test "RealTimeDeque.rev dqueue RealTimeDeque.length 6" {
                                                    let r1 = RealTimeDeque.rev len6
                                                    let r1c = RealTimeDeque.rev len6C3
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.empty" {
                                                    ((RealTimeDeque.isEmpty(RealTimeDeque.ofSeq []))
                                                     && (RealTimeDeque.isEmpty(RealTimeDeque.ofSeqC 3 [])))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 1" {
                                                    ((RealTimeDeque.head(RealTimeDeque.ofSeq [ "a" ]) = "a")
                                                     && (RealTimeDeque.head(RealTimeDeque.ofSeqC 3 [ "a" ]) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 2" {
                                                    let r1 = RealTimeDeque.ofSeq [ "a"; "b" ]
                                                    let r1c = RealTimeDeque.ofSeqC 3 [ "a"; "b" ]
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c

                                                    ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 3" {
                                                    let r1 = RealTimeDeque.ofSeq [ "a"; "b"; "c" ]
                                                    let r1c = RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c" ]
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 4" {
                                                    let r1 = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    let r1c = RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d" ]
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                //RealTimeDeque.length 5 more than sufficient to test RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC
                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 5" {
                                                    let r1 = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]
                                                    let r1c = RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e" ]
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                //RealTimeDeque.length 5 more than sufficient to test RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC
                                                test "RealTimeDeque.ofSeq and RealTimeDeque.ofSeqC RealTimeDeque.length 6" {
                                                    let r1 = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]
                                                    let r1c = RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e"; "f" ]
                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.empty dqueus" {
                                                    ((RealTimeDeque.isEmpty(RealTimeDeque.append (RealTimeDeque.ofSeq []) (RealTimeDeque.ofSeq [])))
                                                     && (RealTimeDeque.isEmpty(RealTimeDeque.append (RealTimeDeque.empty 3) (RealTimeDeque.empty 3))))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.empty and RealTimeDeque.length 1" {
                                                    ((RealTimeDeque.head(RealTimeDeque.append (RealTimeDeque.ofSeq []) len1) = "a")
                                                     && (RealTimeDeque.head(RealTimeDeque.append len1 (RealTimeDeque.empty 3)) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.empty and RealTimeDeque.length 2" {
                                                    let r1 =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq []) (RealTimeDeque.ofSeq [ "a"; "b" ])

                                                    let r1c =
                                                        RealTimeDeque.append (RealTimeDeque.empty 3) (RealTimeDeque.ofSeqC 3 [ "a"; "b" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c

                                                    let r1r =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a"; "b" ]) (RealTimeDeque.ofSeq [])

                                                    let r1cr =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeqC 3 [ "a"; "b" ]) (RealTimeDeque.empty 3)

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 1 and RealTimeDeque.length 2" {
                                                    let r1 =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a" ]) (RealTimeDeque.ofSeq [ "b"; "c" ])

                                                    let r1c =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeqC 3 [ "a" ]) (RealTimeDeque.ofSeqC 3 [ "b"; "c" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c

                                                    let r1r =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a"; "b" ]) (RealTimeDeque.ofSeq [ "c" ])

                                                    let r1cr =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeqC 3 [ "a"; "b" ]) (RealTimeDeque.ofSeqC 3 [ "c" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 1 and RealTimeDeque.length 3" {
                                                    let r1 =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a" ]) (RealTimeDeque.ofSeq [ "b"; "c"; "d" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "b"; "c"; "d" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    let r1r =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) (RealTimeDeque.ofSeq [ "d" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "d" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 1 and RealTimeDeque.length 4" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a" ])
                                                            (RealTimeDeque.ofSeq [ "b"; "c"; "d"; "e" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "b"; "c"; "d"; "e" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                            (RealTimeDeque.ofSeq [ "e" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "e" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr

                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 1 and RealTimeDeque.length 5" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a" ])
                                                            (RealTimeDeque.ofSeq [ "b"; "c"; "d"; "e"; "f" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "b"; "c"; "d"; "e"; "f" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                            (RealTimeDeque.ofSeq [ "f" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "f" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr
                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr
                                                    let t6r = RealTimeDeque.tail t5r
                                                    let t6cr = RealTimeDeque.tail t5cr
                                                    let h6r = RealTimeDeque.head t6r
                                                    let h6cr = RealTimeDeque.head t6cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e")
                                                     && (h6r = "f")
                                                     && (h6cr = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 2 and RealTimeDeque.length 2" {
                                                    let r1 =
                                                        RealTimeDeque.append (RealTimeDeque.ofSeq [ "a"; "b" ]) (RealTimeDeque.ofSeq [ "c"; "d" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "c"; "d" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 2 and RealTimeDeque.length 3" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeq [ "c"; "d"; "e" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "c"; "d"; "e" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c

                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c" ])
                                                            (RealTimeDeque.ofSeq [ "d"; "e" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "d"; "e" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr

                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 2 and RealTimeDeque.length 4" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeq [ "c"; "d"; "e"; "f" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "c"; "d"; "e"; "f" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                            (RealTimeDeque.ofSeq [ "e"; "f" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "e"; "f" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr
                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr
                                                    let t6r = RealTimeDeque.tail t5r
                                                    let t6cr = RealTimeDeque.tail t5cr
                                                    let h6r = RealTimeDeque.head t6r
                                                    let h6cr = RealTimeDeque.head t6cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e")
                                                     && (h6r = "f")
                                                     && (h6cr = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 2 and RealTimeDeque.length 5" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeq [ "c"; "d"; "e"; "f"; "g" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "c"; "d"; "e"; "f"; "g" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c
                                                    let t7 = RealTimeDeque.tail t6
                                                    let t7c = RealTimeDeque.tail t6c
                                                    let h7 = RealTimeDeque.head t7
                                                    let h7c = RealTimeDeque.head t7c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                            (RealTimeDeque.ofSeq [ "f"; "g" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "f"; "g" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr
                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr
                                                    let t6r = RealTimeDeque.tail t5r
                                                    let t6cr = RealTimeDeque.tail t5cr
                                                    let h6r = RealTimeDeque.head t6r
                                                    let h6cr = RealTimeDeque.head t6cr
                                                    let t7r = RealTimeDeque.tail t6r
                                                    let t7cr = RealTimeDeque.tail t6cr
                                                    let h7r = RealTimeDeque.head t7r
                                                    let h7cr = RealTimeDeque.head t7cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f")
                                                     && (h7 = "g")
                                                     && (h7c = "g")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e")
                                                     && (h6r = "f")
                                                     && (h6cr = "f")
                                                     && (h7r = "g")
                                                     && (h7cr = "g"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 3 and RealTimeDeque.length 3" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c" ])
                                                            (RealTimeDeque.ofSeq [ "d"; "e"; "f" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "d"; "e"; "f" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "appending RealTimeDeque.length 6 and RealTimeDeque.length 7" {
                                                    let r1 =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                            (RealTimeDeque.ofSeq [ "g"; "h"; "i"; "j"; "k"; "l"; "m" ])

                                                    let r1c =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "g"; "h"; "i"; "j"; "k"; "l"; "m" ])

                                                    let h1 = RealTimeDeque.head r1
                                                    let h1c = RealTimeDeque.head r1c
                                                    let t2 = RealTimeDeque.tail r1
                                                    let t2c = RealTimeDeque.tail r1c
                                                    let h2 = RealTimeDeque.head t2
                                                    let h2c = RealTimeDeque.head t2c
                                                    let t3 = RealTimeDeque.tail t2
                                                    let t3c = RealTimeDeque.tail t2c
                                                    let h3 = RealTimeDeque.head t3
                                                    let h3c = RealTimeDeque.head t3c
                                                    let t4 = RealTimeDeque.tail t3
                                                    let t4c = RealTimeDeque.tail t3c
                                                    let h4 = RealTimeDeque.head t4
                                                    let h4c = RealTimeDeque.head t4c
                                                    let t5 = RealTimeDeque.tail t4
                                                    let t5c = RealTimeDeque.tail t4c
                                                    let h5 = RealTimeDeque.head t5
                                                    let h5c = RealTimeDeque.head t5c
                                                    let t6 = RealTimeDeque.tail t5
                                                    let t6c = RealTimeDeque.tail t5c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c
                                                    let h6 = RealTimeDeque.head t6
                                                    let h6c = RealTimeDeque.head t6c
                                                    let t7 = RealTimeDeque.tail t6
                                                    let t7c = RealTimeDeque.tail t6c
                                                    let h7 = RealTimeDeque.head t7
                                                    let h7c = RealTimeDeque.head t7c
                                                    let h7 = RealTimeDeque.head t7
                                                    let h7c = RealTimeDeque.head t7c
                                                    let t8 = RealTimeDeque.tail t7
                                                    let t8c = RealTimeDeque.tail t7c
                                                    let h8 = RealTimeDeque.head t8
                                                    let h8c = RealTimeDeque.head t8c
                                                    let h8 = RealTimeDeque.head t8
                                                    let h8c = RealTimeDeque.head t8c
                                                    let t9 = RealTimeDeque.tail t8
                                                    let t9c = RealTimeDeque.tail t8c
                                                    let h9 = RealTimeDeque.head t9
                                                    let h9c = RealTimeDeque.head t9c
                                                    let h9 = RealTimeDeque.head t9
                                                    let h9c = RealTimeDeque.head t9c
                                                    let t10 = RealTimeDeque.tail t9
                                                    let t10c = RealTimeDeque.tail t9c
                                                    let h10 = RealTimeDeque.head t10
                                                    let h10c = RealTimeDeque.head t10c
                                                    let h10 = RealTimeDeque.head t10
                                                    let h10c = RealTimeDeque.head t10c
                                                    let t11 = RealTimeDeque.tail t10
                                                    let t11c = RealTimeDeque.tail t10c
                                                    let h11 = RealTimeDeque.head t11
                                                    let h11c = RealTimeDeque.head t11c
                                                    let h11 = RealTimeDeque.head t11
                                                    let h11c = RealTimeDeque.head t11c
                                                    let t12 = RealTimeDeque.tail t11
                                                    let t12c = RealTimeDeque.tail t11c
                                                    let h12 = RealTimeDeque.head t12
                                                    let h12c = RealTimeDeque.head t12c
                                                    let h12 = RealTimeDeque.head t12
                                                    let h12c = RealTimeDeque.head t12c
                                                    let t13 = RealTimeDeque.tail t12
                                                    let t13c = RealTimeDeque.tail t12c
                                                    let h13 = RealTimeDeque.head t13
                                                    let h13c = RealTimeDeque.head t13c
                                                    let h13 = RealTimeDeque.head t13
                                                    let h13c = RealTimeDeque.head t13c

                                                    let r1r =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f"; "g" ])
                                                            (RealTimeDeque.ofSeq [ "h"; "i"; "j"; "k"; "l"; "m" ])

                                                    let r1cr =
                                                        RealTimeDeque.append
                                                            (RealTimeDeque.ofSeqC 3 [ "a"; "b"; "c"; "d"; "e"; "f"; "g" ])
                                                            (RealTimeDeque.ofSeqC 3 [ "h"; "i"; "j"; "k"; "l"; "m" ])

                                                    let h1r = RealTimeDeque.head r1r
                                                    let h1cr = RealTimeDeque.head r1cr
                                                    let t2r = RealTimeDeque.tail r1r
                                                    let t2cr = RealTimeDeque.tail r1cr
                                                    let h2r = RealTimeDeque.head t2r
                                                    let h2cr = RealTimeDeque.head t2cr
                                                    let t3r = RealTimeDeque.tail t2r
                                                    let t3cr = RealTimeDeque.tail t2cr
                                                    let h3r = RealTimeDeque.head t3r
                                                    let h3cr = RealTimeDeque.head t3cr
                                                    let t4r = RealTimeDeque.tail t3r
                                                    let t4cr = RealTimeDeque.tail t3cr
                                                    let h4r = RealTimeDeque.head t4r
                                                    let h4cr = RealTimeDeque.head t4cr
                                                    let t5r = RealTimeDeque.tail t4r
                                                    let t5cr = RealTimeDeque.tail t4cr
                                                    let h5r = RealTimeDeque.head t5r
                                                    let h5cr = RealTimeDeque.head t5cr
                                                    let t6r = RealTimeDeque.tail t5r
                                                    let t6cr = RealTimeDeque.tail t5cr
                                                    let h6r = RealTimeDeque.head t6r
                                                    let h6cr = RealTimeDeque.head t6cr
                                                    let t7r = RealTimeDeque.tail t6r
                                                    let t7cr = RealTimeDeque.tail t6cr
                                                    let h7r = RealTimeDeque.head t7r
                                                    let h7cr = RealTimeDeque.head t7cr
                                                    let h7r = RealTimeDeque.head t7r
                                                    let h7cr = RealTimeDeque.head t7cr
                                                    let t8r = RealTimeDeque.tail t7r
                                                    let t8cr = RealTimeDeque.tail t7cr
                                                    let h8r = RealTimeDeque.head t8r
                                                    let h8cr = RealTimeDeque.head t8cr
                                                    let h8r = RealTimeDeque.head t8r
                                                    let h8cr = RealTimeDeque.head t8cr
                                                    let t9r = RealTimeDeque.tail t8r
                                                    let t9cr = RealTimeDeque.tail t8cr
                                                    let h9r = RealTimeDeque.head t9r
                                                    let h9cr = RealTimeDeque.head t9cr
                                                    let h9r = RealTimeDeque.head t9r
                                                    let h9cr = RealTimeDeque.head t9cr
                                                    let t10r = RealTimeDeque.tail t9r
                                                    let t10cr = RealTimeDeque.tail t9cr
                                                    let h10r = RealTimeDeque.head t10r
                                                    let h10cr = RealTimeDeque.head t10cr
                                                    let h10r = RealTimeDeque.head t10r
                                                    let h10cr = RealTimeDeque.head t10cr
                                                    let t11r = RealTimeDeque.tail t10r
                                                    let t11cr = RealTimeDeque.tail t10cr
                                                    let h11r = RealTimeDeque.head t11r
                                                    let h11cr = RealTimeDeque.head t11cr
                                                    let h11r = RealTimeDeque.head t11r
                                                    let h11cr = RealTimeDeque.head t11cr
                                                    let t12r = RealTimeDeque.tail t11r
                                                    let t12cr = RealTimeDeque.tail t11cr
                                                    let h12r = RealTimeDeque.head t12r
                                                    let h12cr = RealTimeDeque.head t12cr
                                                    let h12r = RealTimeDeque.head t12r
                                                    let h12cr = RealTimeDeque.head t12cr
                                                    let t13r = RealTimeDeque.tail t12r
                                                    let t13cr = RealTimeDeque.tail t12cr
                                                    let h13r = RealTimeDeque.head t13r
                                                    let h13cr = RealTimeDeque.head t13cr
                                                    let h13r = RealTimeDeque.head t13r
                                                    let h13cr = RealTimeDeque.head t13cr

                                                    ((h1 = "a")
                                                     && (h1c = "a")
                                                     && (h2 = "b")
                                                     && (h2c = "b")
                                                     && (h3 = "c")
                                                     && (h3c = "c")
                                                     && (h4 = "d")
                                                     && (h4c = "d")
                                                     && (h5 = "e")
                                                     && (h5c = "e")
                                                     && (h6 = "f")
                                                     && (h6c = "f")
                                                     && (h7 = "g")
                                                     && (h7c = "g")
                                                     && (h8 = "h")
                                                     && (h8c = "h")
                                                     && (h9 = "i")
                                                     && (h9c = "i")
                                                     && (h10 = "j")
                                                     && (h10c = "j")
                                                     && (h11 = "k")
                                                     && (h11c = "k")
                                                     && (h12 = "l")
                                                     && (h12c = "l")
                                                     && (h13 = "m")
                                                     && (h13c = "m")
                                                     && (h1r = "a")
                                                     && (h1cr = "a")
                                                     && (h2r = "b")
                                                     && (h2cr = "b")
                                                     && (h3r = "c")
                                                     && (h3cr = "c")
                                                     && (h4r = "d")
                                                     && (h4cr = "d")
                                                     && (h5r = "e")
                                                     && (h5cr = "e")
                                                     && (h6r = "f")
                                                     && (h6cr = "f")
                                                     && (h7r = "g")
                                                     && (h7cr = "g")
                                                     && (h8r = "h")
                                                     && (h8cr = "h")
                                                     && (h9r = "i")
                                                     && (h9cr = "i")
                                                     && (h10r = "j")
                                                     && (h10cr = "j")
                                                     && (h11r = "k")
                                                     && (h11cr = "k")
                                                     && (h12r = "l")
                                                     && (h12cr = "l")
                                                     && (h13r = "m")
                                                     && (h13cr = "m"))
                                                    |> Expect.isTrue ""
                                                }


                                                test "RealTimeDeque.lookup RealTimeDeque.length 1" {
                                                    len1 |> RealTimeDeque.lookup 0 |> Expect.equal "" "a"
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 2" {
                                                    (((len2 |> RealTimeDeque.lookup 0) = "b")
                                                     && ((len2 |> RealTimeDeque.lookup 1) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 3" {
                                                    (((len3 |> RealTimeDeque.lookup 0) = "c")
                                                     && ((len3 |> RealTimeDeque.lookup 1) = "b")
                                                     && ((len3 |> RealTimeDeque.lookup 2) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 4" {
                                                    (((len4 |> RealTimeDeque.lookup 0) = "d")
                                                     && ((len4 |> RealTimeDeque.lookup 1) = "c")
                                                     && ((len4 |> RealTimeDeque.lookup 2) = "b")
                                                     && ((len4 |> RealTimeDeque.lookup 3) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 5" {
                                                    (((len5 |> RealTimeDeque.lookup 0) = "e")
                                                     && ((len5 |> RealTimeDeque.lookup 1) = "d")
                                                     && ((len5 |> RealTimeDeque.lookup 2) = "c")
                                                     && ((len5 |> RealTimeDeque.lookup 3) = "b")
                                                     && ((len5 |> RealTimeDeque.lookup 4) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 6" {
                                                    (((len6 |> RealTimeDeque.lookup 0) = "f")
                                                     && ((len6 |> RealTimeDeque.lookup 1) = "e")
                                                     && ((len6 |> RealTimeDeque.lookup 2) = "d")
                                                     && ((len6 |> RealTimeDeque.lookup 3) = "c")
                                                     && ((len6 |> RealTimeDeque.lookup 4) = "b")
                                                     && ((len6 |> RealTimeDeque.lookup 5) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 7" {
                                                    (((len7 |> RealTimeDeque.lookup 0) = "g")
                                                     && ((len7 |> RealTimeDeque.lookup 1) = "f")
                                                     && ((len7 |> RealTimeDeque.lookup 2) = "e")
                                                     && ((len7 |> RealTimeDeque.lookup 3) = "d")
                                                     && ((len7 |> RealTimeDeque.lookup 4) = "c")
                                                     && ((len7 |> RealTimeDeque.lookup 5) = "b")
                                                     && ((len7 |> RealTimeDeque.lookup 6) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 8" {
                                                    (((len8 |> RealTimeDeque.lookup 0) = "h")
                                                     && ((len8 |> RealTimeDeque.lookup 1) = "g")
                                                     && ((len8 |> RealTimeDeque.lookup 2) = "f")
                                                     && ((len8 |> RealTimeDeque.lookup 3) = "e")
                                                     && ((len8 |> RealTimeDeque.lookup 4) = "d")
                                                     && ((len8 |> RealTimeDeque.lookup 5) = "c")
                                                     && ((len8 |> RealTimeDeque.lookup 6) = "b")
                                                     && ((len8 |> RealTimeDeque.lookup 7) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 9" {
                                                    (((len9 |> RealTimeDeque.lookup 0) = "i")
                                                     && ((len9 |> RealTimeDeque.lookup 1) = "h")
                                                     && ((len9 |> RealTimeDeque.lookup 2) = "g")
                                                     && ((len9 |> RealTimeDeque.lookup 3) = "f")
                                                     && ((len9 |> RealTimeDeque.lookup 4) = "e")
                                                     && ((len9 |> RealTimeDeque.lookup 5) = "d")
                                                     && ((len9 |> RealTimeDeque.lookup 6) = "c")
                                                     && ((len9 |> RealTimeDeque.lookup 7) = "b")
                                                     && ((len9 |> RealTimeDeque.lookup 8) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.lookup RealTimeDeque.length 10" {
                                                    (((lena |> RealTimeDeque.lookup 0) = "j")
                                                     && ((lena |> RealTimeDeque.lookup 1) = "i")
                                                     && ((lena |> RealTimeDeque.lookup 2) = "h")
                                                     && ((lena |> RealTimeDeque.lookup 3) = "g")
                                                     && ((lena |> RealTimeDeque.lookup 4) = "f")
                                                     && ((lena |> RealTimeDeque.lookup 5) = "e")
                                                     && ((lena |> RealTimeDeque.lookup 6) = "d")
                                                     && ((lena |> RealTimeDeque.lookup 7) = "c")
                                                     && ((lena |> RealTimeDeque.lookup 8) = "b")
                                                     && ((lena |> RealTimeDeque.lookup 9) = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 1" {
                                                    let a = len1 |> RealTimeDeque.tryLookup 0
                                                    (a.Value = "a") |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 2" {
                                                    let b = len2 |> RealTimeDeque.tryLookup 0
                                                    let a = len2 |> RealTimeDeque.tryLookup 1
                                                    ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 3" {
                                                    let c = len3 |> RealTimeDeque.tryLookup 0
                                                    let b = len3 |> RealTimeDeque.tryLookup 1
                                                    let a = len3 |> RealTimeDeque.tryLookup 2

                                                    ((c.Value = "c") && (b.Value = "b") && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 4" {
                                                    let d = len4 |> RealTimeDeque.tryLookup 0
                                                    let c = len4 |> RealTimeDeque.tryLookup 1
                                                    let b = len4 |> RealTimeDeque.tryLookup 2
                                                    let a = len4 |> RealTimeDeque.tryLookup 3

                                                    ((d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 5" {
                                                    let e = len5 |> RealTimeDeque.tryLookup 0
                                                    let d = len5 |> RealTimeDeque.tryLookup 1
                                                    let c = len5 |> RealTimeDeque.tryLookup 2
                                                    let b = len5 |> RealTimeDeque.tryLookup 3
                                                    let a = len5 |> RealTimeDeque.tryLookup 4

                                                    ((e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 6" {
                                                    let f = len6 |> RealTimeDeque.tryLookup 0
                                                    let e = len6 |> RealTimeDeque.tryLookup 1
                                                    let d = len6 |> RealTimeDeque.tryLookup 2
                                                    let c = len6 |> RealTimeDeque.tryLookup 3
                                                    let b = len6 |> RealTimeDeque.tryLookup 4
                                                    let a = len6 |> RealTimeDeque.tryLookup 5

                                                    ((f.Value = "f")
                                                     && (e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 7" {
                                                    let g = len7 |> RealTimeDeque.tryLookup 0
                                                    let f = len7 |> RealTimeDeque.tryLookup 1
                                                    let e = len7 |> RealTimeDeque.tryLookup 2
                                                    let d = len7 |> RealTimeDeque.tryLookup 3
                                                    let c = len7 |> RealTimeDeque.tryLookup 4
                                                    let b = len7 |> RealTimeDeque.tryLookup 5
                                                    let a = len7 |> RealTimeDeque.tryLookup 6

                                                    ((g.Value = "g")
                                                     && (f.Value = "f")
                                                     && (e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 8" {
                                                    let h = len8 |> RealTimeDeque.tryLookup 0
                                                    let g = len8 |> RealTimeDeque.tryLookup 1
                                                    let f = len8 |> RealTimeDeque.tryLookup 2
                                                    let e = len8 |> RealTimeDeque.tryLookup 3
                                                    let d = len8 |> RealTimeDeque.tryLookup 4
                                                    let c = len8 |> RealTimeDeque.tryLookup 5
                                                    let b = len8 |> RealTimeDeque.tryLookup 6
                                                    let a = len8 |> RealTimeDeque.tryLookup 7

                                                    ((h.Value = "h")
                                                     && (g.Value = "g")
                                                     && (f.Value = "f")
                                                     && (e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 9" {
                                                    let i = len9 |> RealTimeDeque.tryLookup 0
                                                    let h = len9 |> RealTimeDeque.tryLookup 1
                                                    let g = len9 |> RealTimeDeque.tryLookup 2
                                                    let f = len9 |> RealTimeDeque.tryLookup 3
                                                    let e = len9 |> RealTimeDeque.tryLookup 4
                                                    let d = len9 |> RealTimeDeque.tryLookup 5
                                                    let c = len9 |> RealTimeDeque.tryLookup 6
                                                    let b = len9 |> RealTimeDeque.tryLookup 7
                                                    let a = len9 |> RealTimeDeque.tryLookup 8

                                                    ((i.Value = "i")
                                                     && (h.Value = "h")
                                                     && (g.Value = "g")
                                                     && (f.Value = "f")
                                                     && (e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup RealTimeDeque.length 10" {
                                                    let j = lena |> RealTimeDeque.tryLookup 0
                                                    let i = lena |> RealTimeDeque.tryLookup 1
                                                    let h = lena |> RealTimeDeque.tryLookup 2
                                                    let g = lena |> RealTimeDeque.tryLookup 3
                                                    let f = lena |> RealTimeDeque.tryLookup 4
                                                    let e = lena |> RealTimeDeque.tryLookup 5
                                                    let d = lena |> RealTimeDeque.tryLookup 6
                                                    let c = lena |> RealTimeDeque.tryLookup 7
                                                    let b = lena |> RealTimeDeque.tryLookup 8
                                                    let a = lena |> RealTimeDeque.tryLookup 9

                                                    ((j.Value = "j")
                                                     && (i.Value = "i")
                                                     && (h.Value = "h")
                                                     && (g.Value = "g")
                                                     && (f.Value = "f")
                                                     && (e.Value = "e")
                                                     && (d.Value = "d")
                                                     && (c.Value = "c")
                                                     && (b.Value = "b")
                                                     && (a.Value = "a"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryLookup not found" { lena |> RealTimeDeque.tryLookup 10 |> Expect.isNone "" }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 1" {
                                                    len1
                                                    |> RealTimeDeque.remove 0
                                                    |> RealTimeDeque.isEmpty
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 2" {
                                                    let a = len2 |> RealTimeDeque.remove 0 |> RealTimeDeque.head
                                                    let b = len2 |> RealTimeDeque.remove 1 |> RealTimeDeque.head
                                                    ((a = "a") && (b = "b")) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 3" {
                                                    let r0 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.remove 0
                                                    let b0 = RealTimeDeque.head r0
                                                    let t0 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t0

                                                    let r1 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.remove 1
                                                    let a1 = RealTimeDeque.head r1
                                                    let t1 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t1

                                                    let r2 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.remove 2
                                                    let a2 = RealTimeDeque.head r2
                                                    let t2 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t2

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (a2 = "a")
                                                     && (b2 = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 4" {
                                                    let r0 = (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]) |> RealTimeDeque.remove 0
                                                    let b0 = RealTimeDeque.head r0
                                                    let t0 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t0
                                                    let t01 = RealTimeDeque.tail t0
                                                    let d0 = RealTimeDeque.head t01

                                                    let r1 = (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]) |> RealTimeDeque.remove 1
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12

                                                    let r2 = (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]) |> RealTimeDeque.remove 2
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22

                                                    let r3 = (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]) |> RealTimeDeque.remove 3
                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 5" {
                                                    let r0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.remove 0

                                                    let b0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let d0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let e0 = RealTimeDeque.head t03

                                                    let r1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.remove 1

                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let e1 = RealTimeDeque.head t13

                                                    let r2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.remove 2

                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let e2 = RealTimeDeque.head t23

                                                    let r3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.remove 3

                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let e3 = RealTimeDeque.head t33

                                                    let r4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.remove 4

                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (e2 = "e")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (e3 = "e")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.remove elements RealTimeDeque.length 6" {
                                                    let r0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 0

                                                    let b0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let d0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let e0 = RealTimeDeque.head t03
                                                    let t04 = RealTimeDeque.tail t03
                                                    let f0 = RealTimeDeque.head t04

                                                    let r1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 1

                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let e1 = RealTimeDeque.head t13
                                                    let t14 = RealTimeDeque.tail t13
                                                    let f1 = RealTimeDeque.head t14

                                                    let r2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 2

                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let e2 = RealTimeDeque.head t23
                                                    let t24 = RealTimeDeque.tail t23
                                                    let f2 = RealTimeDeque.head t24

                                                    let r3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 3

                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let e3 = RealTimeDeque.head t33
                                                    let t34 = RealTimeDeque.tail t33
                                                    let f3 = RealTimeDeque.head t34

                                                    let r4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 4

                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43
                                                    let t44 = RealTimeDeque.tail t43
                                                    let f4 = RealTimeDeque.head t44

                                                    let r5 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.remove 5

                                                    let a5 = RealTimeDeque.head r5
                                                    let t51 = RealTimeDeque.tail r5
                                                    let b5 = RealTimeDeque.head t51
                                                    let t52 = RealTimeDeque.tail t51
                                                    let c5 = RealTimeDeque.head t52
                                                    let t53 = RealTimeDeque.tail t52
                                                    let d5 = RealTimeDeque.head t53
                                                    let t54 = RealTimeDeque.tail t53
                                                    let e5 = RealTimeDeque.head t54

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (f0 = "f")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (f1 = "f")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (e2 = "e")
                                                     && (f2 = "f")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (e3 = "e")
                                                     && (f3 = "f")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d")
                                                     && (f4 = "f")
                                                     && (a5 = "a")
                                                     && (b5 = "b")
                                                     && (c5 = "c")
                                                     && (d5 = "d")
                                                     && (e5 = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "tryRemoveempty" {
                                                    (RealTimeDeque.empty 3)
                                                    |> RealTimeDeque.tryRemove 0
                                                    |> Expect.isNone ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 1" {
                                                    let a = len1 |> RealTimeDeque.tryRemove 0
                                                    a.Value |> RealTimeDeque.isEmpty |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 2" {
                                                    let a = len2 |> RealTimeDeque.tryRemove 0
                                                    let a1 = RealTimeDeque.head a.Value
                                                    let b = len2 |> RealTimeDeque.tryRemove 1
                                                    let b1 = RealTimeDeque.head b.Value
                                                    ((a1 = "a") && (b1 = "b")) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 3" {
                                                    let x0 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.tryRemove 0
                                                    let r0 = x0.Value
                                                    let b0 = RealTimeDeque.head r0
                                                    let t0 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t0

                                                    let x1 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.tryRemove 1
                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t1 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t1

                                                    let x2 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.tryRemove 2
                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t2 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t2

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (a2 = "a")
                                                     && (b2 = "b"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 4" {
                                                    let x0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryRemove 0

                                                    let r0 = x0.Value
                                                    let b0 = RealTimeDeque.head r0
                                                    let t0 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t0
                                                    let t01 = RealTimeDeque.tail t0
                                                    let d0 = RealTimeDeque.head t01

                                                    let x1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryRemove 1

                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12

                                                    let x2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryRemove 2

                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22

                                                    let x3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryRemove 3

                                                    let r3 = x3.Value
                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 5" {
                                                    let x0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.tryRemove 0

                                                    let r0 = x0.Value
                                                    let b0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let d0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let e0 = RealTimeDeque.head t03

                                                    let x1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.tryRemove 1

                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let e1 = RealTimeDeque.head t13

                                                    let x2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.tryRemove 2

                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let e2 = RealTimeDeque.head t23

                                                    let x3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.tryRemove 3

                                                    let r3 = x3.Value
                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let e3 = RealTimeDeque.head t33

                                                    let x4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.tryRemove 4

                                                    let r4 = x4.Value
                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (e2 = "e")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (e3 = "e")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryRemove elements RealTimeDeque.length 6" {
                                                    let x0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 0

                                                    let r0 = x0.Value
                                                    let b0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let c0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let d0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let e0 = RealTimeDeque.head t03
                                                    let t04 = RealTimeDeque.tail t03
                                                    let f0 = RealTimeDeque.head t04

                                                    let x1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 1

                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let c1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let d1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let e1 = RealTimeDeque.head t13
                                                    let t14 = RealTimeDeque.tail t13
                                                    let f1 = RealTimeDeque.head t14

                                                    let x2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 2

                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let e2 = RealTimeDeque.head t23
                                                    let t24 = RealTimeDeque.tail t23
                                                    let f2 = RealTimeDeque.head t24

                                                    let x3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 3

                                                    let r3 = x3.Value
                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let e3 = RealTimeDeque.head t33
                                                    let t34 = RealTimeDeque.tail t33
                                                    let f3 = RealTimeDeque.head t34

                                                    let x4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 4

                                                    let r4 = x4.Value
                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43
                                                    let t44 = RealTimeDeque.tail t43
                                                    let f4 = RealTimeDeque.head t44

                                                    let x5 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.tryRemove 5

                                                    let r5 = x5.Value
                                                    let a5 = RealTimeDeque.head r5
                                                    let t51 = RealTimeDeque.tail r5
                                                    let b5 = RealTimeDeque.head t51
                                                    let t52 = RealTimeDeque.tail t51
                                                    let c5 = RealTimeDeque.head t52
                                                    let t53 = RealTimeDeque.tail t52
                                                    let d5 = RealTimeDeque.head t53
                                                    let t54 = RealTimeDeque.tail t53
                                                    let e5 = RealTimeDeque.head t54

                                                    ((b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (f0 = "f")
                                                     && (a1 = "a")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (f1 = "f")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "d")
                                                     && (e2 = "e")
                                                     && (f2 = "f")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (e3 = "e")
                                                     && (f3 = "f")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d")
                                                     && (f4 = "f")
                                                     && (a5 = "a")
                                                     && (b5 = "b")
                                                     && (c5 = "c")
                                                     && (d5 = "d")
                                                     && (e5 = "e"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 1" {
                                                    len1
                                                    |> RealTimeDeque.update 0 "aa"
                                                    |> RealTimeDeque.head
                                                    |> Expect.equal "" "aa"
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 2" {
                                                    let r0 = (RealTimeDeque.ofSeq [ "a"; "b" ]) |> RealTimeDeque.update 0 "zz"
                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01

                                                    let r1 = (RealTimeDeque.ofSeq [ "a"; "b" ]) |> RealTimeDeque.update 1 "zz"
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11

                                                    ((a0 = "zz") && (b0 = "b") && (a1 = "a") && (b1 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 3" {
                                                    let r0 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.update 0 "zz"
                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02

                                                    let r1 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.update 1 "zz"
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12

                                                    let r2 = (RealTimeDeque.ofSeq [ "a"; "b"; "c" ]) |> RealTimeDeque.update 2 "zz"
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 4" {
                                                    let r0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.update 0 "zz"

                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let d0 = RealTimeDeque.head t03

                                                    let r1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.update 1 "zz"

                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let d1 = RealTimeDeque.head t13

                                                    let r2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.update 2 "zz"

                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let d2 = RealTimeDeque.head t23

                                                    let r3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.update 3 "zz"

                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let d3 = RealTimeDeque.head t33

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz")
                                                     && (d2 = "d")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (d3 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 5" {
                                                    let r0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.update 0 "zz"

                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let d0 = RealTimeDeque.head t03
                                                    let t04 = RealTimeDeque.tail t03
                                                    let e0 = RealTimeDeque.head t04

                                                    let r1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.update 1 "zz"

                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let d1 = RealTimeDeque.head t13
                                                    let t14 = RealTimeDeque.tail t13
                                                    let e1 = RealTimeDeque.head t14

                                                    let r2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.update 2 "zz"

                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let d2 = RealTimeDeque.head t23
                                                    let t24 = RealTimeDeque.tail t23
                                                    let e2 = RealTimeDeque.head t24

                                                    let r3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.update 3 "zz"

                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let d3 = RealTimeDeque.head t33
                                                    let t34 = RealTimeDeque.tail t33
                                                    let e3 = RealTimeDeque.head t34

                                                    let r4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e" ])
                                                        |> RealTimeDeque.update 4 "zz"

                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43
                                                    let t44 = RealTimeDeque.tail t43
                                                    let e4 = RealTimeDeque.head t44

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz")
                                                     && (d2 = "d")
                                                     && (e2 = "e")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (d3 = "zz")
                                                     && (e3 = "e")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d")
                                                     && (e4 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.update elements RealTimeDeque.length 6" {
                                                    let r0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 0 "zz"

                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let d0 = RealTimeDeque.head t03
                                                    let t04 = RealTimeDeque.tail t03
                                                    let e0 = RealTimeDeque.head t04
                                                    let t05 = RealTimeDeque.tail t04
                                                    let f0 = RealTimeDeque.head t05

                                                    let r1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 1 "zz"

                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let d1 = RealTimeDeque.head t13
                                                    let t14 = RealTimeDeque.tail t13
                                                    let e1 = RealTimeDeque.head t14
                                                    let t15 = RealTimeDeque.tail t14
                                                    let f1 = RealTimeDeque.head t15

                                                    let r2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 2 "zz"

                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let d2 = RealTimeDeque.head t23
                                                    let t24 = RealTimeDeque.tail t23
                                                    let e2 = RealTimeDeque.head t24
                                                    let t25 = RealTimeDeque.tail t24
                                                    let f2 = RealTimeDeque.head t25

                                                    let r3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 3 "zz"

                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let d3 = RealTimeDeque.head t33
                                                    let t34 = RealTimeDeque.tail t33
                                                    let e3 = RealTimeDeque.head t34
                                                    let t35 = RealTimeDeque.tail t34
                                                    let f3 = RealTimeDeque.head t35

                                                    let r4 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 4 "zz"

                                                    let a4 = RealTimeDeque.head r4
                                                    let t41 = RealTimeDeque.tail r4
                                                    let b4 = RealTimeDeque.head t41
                                                    let t42 = RealTimeDeque.tail t41
                                                    let c4 = RealTimeDeque.head t42
                                                    let t43 = RealTimeDeque.tail t42
                                                    let d4 = RealTimeDeque.head t43
                                                    let t44 = RealTimeDeque.tail t43
                                                    let e4 = RealTimeDeque.head t44
                                                    let t45 = RealTimeDeque.tail t44
                                                    let f4 = RealTimeDeque.head t45

                                                    let r5 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ])
                                                        |> RealTimeDeque.update 5 "zz"

                                                    let a5 = RealTimeDeque.head r5
                                                    let t51 = RealTimeDeque.tail r5
                                                    let b5 = RealTimeDeque.head t51
                                                    let t52 = RealTimeDeque.tail t51
                                                    let c5 = RealTimeDeque.head t52
                                                    let t53 = RealTimeDeque.tail t52
                                                    let d5 = RealTimeDeque.head t53
                                                    let t54 = RealTimeDeque.tail t53
                                                    let e5 = RealTimeDeque.head t54
                                                    let t55 = RealTimeDeque.tail t54
                                                    let f5 = RealTimeDeque.head t55

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (e0 = "e")
                                                     && (f0 = "f")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (e1 = "e")
                                                     && (f1 = "f")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz")
                                                     && (d2 = "d")
                                                     && (e2 = "e")
                                                     && (f2 = "f")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (d3 = "zz")
                                                     && (e3 = "e")
                                                     && (f3 = "f")
                                                     && (a4 = "a")
                                                     && (b4 = "b")
                                                     && (c4 = "c")
                                                     && (d4 = "d")
                                                     && (e4 = "zz")
                                                     && (f4 = "f")
                                                     && (a5 = "a")
                                                     && (b5 = "b")
                                                     && (c5 = "c")
                                                     && (d5 = "d")
                                                     && (e5 = "e")
                                                     && (f5 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUpdate elements RealTimeDeque.length 1" {
                                                    let a = len1 |> RealTimeDeque.tryUpdate 0 "aa"
                                                    a.Value |> RealTimeDeque.head |> Expect.equal "" "aa"
                                                }

                                                test "RealTimeDeque.tryUpdate elements RealTimeDeque.length 2" {
                                                    let x0 = (RealTimeDeque.ofSeq [ "a"; "b" ]) |> RealTimeDeque.tryUpdate 0 "zz"
                                                    let r0 = x0.Value
                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01

                                                    let x1 = (RealTimeDeque.ofSeq [ "a"; "b" ]) |> RealTimeDeque.tryUpdate 1 "zz"
                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11

                                                    ((a0 = "zz") && (b0 = "b") && (a1 = "a") && (b1 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUpdate elements RealTimeDeque.length 3" {
                                                    let x0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c" ])
                                                        |> RealTimeDeque.tryUpdate 0 "zz"

                                                    let r0 = x0.Value
                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02

                                                    let x1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c" ])
                                                        |> RealTimeDeque.tryUpdate 1 "zz"

                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12

                                                    let x2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c" ])
                                                        |> RealTimeDeque.tryUpdate 2 "zz"

                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUpdate elements RealTimeDeque.length 4" {
                                                    let x0 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryUpdate 0 "zz"

                                                    let r0 = x0.Value
                                                    let a0 = RealTimeDeque.head r0
                                                    let t01 = RealTimeDeque.tail r0
                                                    let b0 = RealTimeDeque.head t01
                                                    let t02 = RealTimeDeque.tail t01
                                                    let c0 = RealTimeDeque.head t02
                                                    let t03 = RealTimeDeque.tail t02
                                                    let d0 = RealTimeDeque.head t03

                                                    let x1 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryUpdate 1 "zz"

                                                    let r1 = x1.Value
                                                    let a1 = RealTimeDeque.head r1
                                                    let t11 = RealTimeDeque.tail r1
                                                    let b1 = RealTimeDeque.head t11
                                                    let t12 = RealTimeDeque.tail t11
                                                    let c1 = RealTimeDeque.head t12
                                                    let t13 = RealTimeDeque.tail t12
                                                    let d1 = RealTimeDeque.head t13

                                                    let x2 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryUpdate 2 "zz"

                                                    let r2 = x2.Value
                                                    let a2 = RealTimeDeque.head r2
                                                    let t21 = RealTimeDeque.tail r2
                                                    let b2 = RealTimeDeque.head t21
                                                    let t22 = RealTimeDeque.tail t21
                                                    let c2 = RealTimeDeque.head t22
                                                    let t23 = RealTimeDeque.tail t22
                                                    let d2 = RealTimeDeque.head t23

                                                    let x3 =
                                                        (RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ])
                                                        |> RealTimeDeque.tryUpdate 3 "zz"

                                                    let r3 = x3.Value
                                                    let a3 = RealTimeDeque.head r3
                                                    let t31 = RealTimeDeque.tail r3
                                                    let b3 = RealTimeDeque.head t31
                                                    let t32 = RealTimeDeque.tail t31
                                                    let c3 = RealTimeDeque.head t32
                                                    let t33 = RealTimeDeque.tail t32
                                                    let d3 = RealTimeDeque.head t33

                                                    ((a0 = "zz")
                                                     && (b0 = "b")
                                                     && (c0 = "c")
                                                     && (d0 = "d")
                                                     && (a1 = "a")
                                                     && (b1 = "zz")
                                                     && (c1 = "c")
                                                     && (d1 = "d")
                                                     && (a2 = "a")
                                                     && (b2 = "b")
                                                     && (c2 = "zz")
                                                     && (d2 = "d")
                                                     && (a3 = "a")
                                                     && (b3 = "b")
                                                     && (c3 = "c")
                                                     && (d3 = "zz"))
                                                    |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUncons on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryUncons q = None) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUncons on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    let x, xs = (RealTimeDeque.tryUncons q).Value
                                                    x |> Expect.equal "" "a"
                                                }

                                                test "RealTimeDeque.tryUnsnoc on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryUnsnoc q = None) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryUnsnoc on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    let xs, x = (RealTimeDeque.tryUnsnoc q).Value
                                                    x |> Expect.equal "" "d"
                                                }

                                                test "RealTimeDeque.tryGetHead on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryGetHead q = None) |> Expect.isTrue ""
                                                }

                                                test "RealTimeDeque.tryGetHead on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    (RealTimeDeque.tryGetHead q).Value |> Expect.equal "" "a"
                                                }

                                                test "tryGetInit on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryGetInit q = None) |> Expect.isTrue ""
                                                }

                                                test "tryGetInit on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    let x = (RealTimeDeque.tryGetInit q).Value
                                                    let x2 = x |> RealTimeDeque.last
                                                    x2 |> Expect.equal "" "c"
                                                }

                                                test "tryGetLast on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryGetLast q = None) |> Expect.isTrue ""
                                                }

                                                test "tryGetLast on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]
                                                    (RealTimeDeque.tryGetLast q).Value |> Expect.equal "" "d"
                                                }


                                                test "tryGetTail on RealTimeDeque.empty" {
                                                    let q = RealTimeDeque.empty 2
                                                    (RealTimeDeque.tryGetTail q = None) |> Expect.isTrue ""
                                                }

                                                test "tryGetTail on q" {
                                                    let q = RealTimeDeque.ofSeq [ "a"; "b"; "c"; "d" ]

                                                    (RealTimeDeque.tryGetTail q).Value
                                                    |> RealTimeDeque.head
                                                    |> Expect.equal "" "b"
                                                } ]

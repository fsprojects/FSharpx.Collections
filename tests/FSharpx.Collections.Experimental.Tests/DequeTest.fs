namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//quite a lot going on and difficult to reason about edge cases
//testing up to Deque.length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)

module DequeTest =

    let len1 = Deque.singleton "a"
    let len2 = Deque.singleton "a" |> Deque.cons "b"
    let len3 = Deque.singleton "a" |> Deque.cons "b" |> Deque.cons "c"

    let len4 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"

    let len5 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"

    let len6 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"
        |> Deque.cons "f"

    let len7 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"
        |> Deque.cons "f"
        |> Deque.cons "g"

    let len8 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"
        |> Deque.cons "f"
        |> Deque.cons "g"
        |> Deque.cons "h"

    let len9 =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"
        |> Deque.cons "f"
        |> Deque.cons "g"
        |> Deque.cons "h"
        |> Deque.cons "i"

    let lena =
        Deque.singleton "a"
        |> Deque.cons "b"
        |> Deque.cons "c"
        |> Deque.cons "d"
        |> Deque.cons "e"
        |> Deque.cons "f"
        |> Deque.cons "g"
        |> Deque.cons "h"
        |> Deque.cons "i"
        |> Deque.cons "j"

    let len2snoc = Deque.singleton "b" |> Deque.snoc "a"
    let len3snoc = Deque.singleton "c" |> Deque.snoc "b" |> Deque.snoc "a"

    let len4snoc =
        Deque.singleton "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let len5snoc =
        Deque.singleton "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let len6snoc =
        Deque.singleton "f"
        |> Deque.snoc "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let len7snoc =
        Deque.singleton "g"
        |> Deque.snoc "f"
        |> Deque.snoc "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let len8snoc =
        Deque.singleton "h"
        |> Deque.snoc "g"
        |> Deque.snoc "f"
        |> Deque.snoc "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let len9snoc =
        Deque.singleton "i"
        |> Deque.snoc "h"
        |> Deque.snoc "g"
        |> Deque.snoc "f"
        |> Deque.snoc "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    let lenasnoc =
        Deque.singleton "j"
        |> Deque.snoc "i"
        |> Deque.snoc "h"
        |> Deque.snoc "g"
        |> Deque.snoc "f"
        |> Deque.snoc "e"
        |> Deque.snoc "d"
        |> Deque.snoc "c"
        |> Deque.snoc "b"
        |> Deque.snoc "a"

    [<Tests>]
    let testDeque =

        testList "Experimental Deque" [ test "empty dqueue should be empty" { Expect.isTrue "empty is empty" (Deque.empty() |> Deque.isEmpty) }

                                        test "Deque.cons works" { Expect.isFalse "not empty" (len2 |> Deque.isEmpty) }

                                        test "Deque.snoc works" { Expect.isFalse "" (len2snoc |> Deque.isEmpty) }

                                        test "Deque.singleton Deque.head works" { Expect.equal "Deque.singleton" "a" (len1 |> Deque.head) }

                                        test "Deque.singleton Deque.last works" { Expect.equal "" "a" (len1 |> Deque.last) }

                                        test "Deque.tail of Deque.singleton empty" {
                                            Expect.isTrue "Deque.isEmpty" (len1 |> Deque.tail |> Deque.isEmpty)
                                        }

                                        test "Deque.tail of Deque.tail of 2 empty" {
                                            Expect.isTrue "Deque.isEmpty" (len2 |> Deque.tail |> Deque.tail |> Deque.isEmpty)
                                            Expect.isTrue "Deque.isEmpty" (len2snoc |> Deque.tail |> Deque.tail |> Deque.isEmpty)
                                        }

                                        test "initial of Deque.singleton empty" {
                                            Expect.isTrue "Deque.isEmpty" (len1 |> Deque.init |> Deque.isEmpty)
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 1" {
                                            let t1 = Deque.tail len2
                                            let t1s = Deque.tail len2snoc

                                            Expect.isTrue
                                                "Deque.head, Deque.tail, and Deque.length"
                                                (((Deque.length t1) = 1)
                                                 && ((Deque.length t1s) = 1)
                                                 && ((Deque.head t1) = "a")
                                                 && ((Deque.head t1s) = "a"))
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 2" {
                                            let t1 = Deque.tail len3
                                            let t1s = Deque.tail len3snoc

                                            let t1_1 = Deque.tail t1
                                            let t1_1s = Deque.tail t1s

                                            (((Deque.length t1) = 2)
                                             && ((Deque.length t1s) = 2)
                                             && ((Deque.head t1) = "b")
                                             && ((Deque.head t1s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 3" {
                                            let t1 = Deque.tail len4
                                            let t1s = Deque.tail len4snoc

                                            let t1_2 = Deque.tail t1
                                            let t1_2s = Deque.tail t1s

                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 3)
                                             && ((Deque.length t1s) = 3)
                                             && ((Deque.head t1) = "c")
                                             && ((Deque.head t1s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 4" {
                                            let t1 = Deque.tail len5
                                            let t1s = Deque.tail len5snoc

                                            let t1_3 = Deque.tail t1
                                            let t1_3s = Deque.tail t1s

                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s

                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 4)
                                             && ((Deque.length t1s) = 4)
                                             && ((Deque.head t1) = "d")
                                             && ((Deque.head t1s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 5" {
                                            let t1 = Deque.tail len6
                                            let t1s = Deque.tail len6snoc

                                            let t1_4 = Deque.tail t1
                                            let t1_4s = Deque.tail t1s

                                            let t1_3 = Deque.tail t1_4
                                            let t1_3s = Deque.tail t1_4s

                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s

                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 5)
                                             && ((Deque.length t1s) = 5)
                                             && ((Deque.head t1) = "e")
                                             && ((Deque.head t1s) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.length t1_4s) = 4)
                                             && ((Deque.head t1_4) = "d")
                                             && ((Deque.head t1_4s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 6" {
                                            let t1 = Deque.tail len7
                                            let t1s = Deque.tail len7snoc

                                            let t1_5 = Deque.tail t1
                                            let t1_5s = Deque.tail t1s

                                            let t1_4 = Deque.tail t1_5
                                            let t1_4s = Deque.tail t1_5s

                                            let t1_3 = Deque.tail t1_4
                                            let t1_3s = Deque.tail t1_4s

                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s

                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 6)
                                             && ((Deque.length t1s) = 6)
                                             && ((Deque.head t1) = "f")
                                             && ((Deque.head t1s) = "f")
                                             && ((Deque.length t1_5) = 5)
                                             && ((Deque.length t1_5s) = 5)
                                             && ((Deque.head t1_5) = "e")
                                             && ((Deque.head t1_5s) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.length t1_4s) = 4)
                                             && ((Deque.head t1_4) = "d")
                                             && ((Deque.head t1_4s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 7" {
                                            let t1 = Deque.tail len8
                                            let t1s = Deque.tail len8snoc
                                            let t1_6 = Deque.tail t1
                                            let t1_6s = Deque.tail t1s
                                            let t1_5 = Deque.tail t1_6
                                            let t1_5s = Deque.tail t1_6s
                                            let t1_4 = Deque.tail t1_5
                                            let t1_4s = Deque.tail t1_5s
                                            let t1_3 = Deque.tail t1_4
                                            let t1_3s = Deque.tail t1_4s
                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s
                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 7)
                                             && ((Deque.length t1s) = 7)
                                             && ((Deque.head t1) = "g")
                                             && ((Deque.head t1s) = "g")
                                             && ((Deque.length t1_6) = 6)
                                             && ((Deque.length t1_6s) = 6)
                                             && ((Deque.head t1_6) = "f")
                                             && ((Deque.head t1_6s) = "f")
                                             && ((Deque.length t1_5) = 5)
                                             && ((Deque.length t1_5s) = 5)
                                             && ((Deque.head t1_5) = "e")
                                             && ((Deque.head t1_5s) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.length t1_4s) = 4)
                                             && ((Deque.head t1_4) = "d")
                                             && ((Deque.head t1_4s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 8" {
                                            let t1 = Deque.tail len9
                                            let t1s = Deque.tail len9snoc
                                            let t1_7 = Deque.tail t1
                                            let t1_7s = Deque.tail t1s
                                            let t1_6 = Deque.tail t1_7
                                            let t1_6s = Deque.tail t1_7s
                                            let t1_5 = Deque.tail t1_6
                                            let t1_5s = Deque.tail t1_6s
                                            let t1_4 = Deque.tail t1_5
                                            let t1_4s = Deque.tail t1_5s
                                            let t1_3 = Deque.tail t1_4
                                            let t1_3s = Deque.tail t1_4s
                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s
                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 8)
                                             && ((Deque.length t1s) = 8)
                                             && ((Deque.head t1) = "h")
                                             && ((Deque.head t1s) = "h")
                                             && ((Deque.length t1_7) = 7)
                                             && ((Deque.length t1_7s) = 7)
                                             && ((Deque.head t1_7) = "g")
                                             && ((Deque.head t1_7s) = "g")
                                             && ((Deque.length t1_6) = 6)
                                             && ((Deque.length t1_6s) = 6)
                                             && ((Deque.head t1_6) = "f")
                                             && ((Deque.head t1_6s) = "f")
                                             && ((Deque.length t1_5) = 5)
                                             && ((Deque.length t1_5s) = 5)
                                             && ((Deque.head t1_5) = "e")
                                             && ((Deque.head t1_5s) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.length t1_4s) = 4)
                                             && ((Deque.head t1_4) = "d")
                                             && ((Deque.head t1_4s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        test "Deque.head, Deque.tail, and Deque.length work test 9" {
                                            let t1 = Deque.tail lena
                                            let t1s = Deque.tail lenasnoc
                                            let t1_8 = Deque.tail t1
                                            let t1_8s = Deque.tail t1s
                                            let t1_7 = Deque.tail t1_8
                                            let t1_7s = Deque.tail t1_8s
                                            let t1_6 = Deque.tail t1_7
                                            let t1_6s = Deque.tail t1_7s
                                            let t1_5 = Deque.tail t1_6
                                            let t1_5s = Deque.tail t1_6s
                                            let t1_4 = Deque.tail t1_5
                                            let t1_4s = Deque.tail t1_5s
                                            let t1_3 = Deque.tail t1_4
                                            let t1_3s = Deque.tail t1_4s
                                            let t1_2 = Deque.tail t1_3
                                            let t1_2s = Deque.tail t1_3s
                                            let t1_1 = Deque.tail t1_2
                                            let t1_1s = Deque.tail t1_2s

                                            (((Deque.length t1) = 9)
                                             && ((Deque.length t1s) = 9)
                                             && ((Deque.head t1) = "i")
                                             && ((Deque.head t1s) = "i")
                                             && ((Deque.length t1_8) = 8)
                                             && ((Deque.length t1_8s) = 8)
                                             && ((Deque.head t1_8) = "h")
                                             && ((Deque.head t1_8s) = "h")
                                             && ((Deque.length t1_7) = 7)
                                             && ((Deque.length t1_7s) = 7)
                                             && ((Deque.head t1_7) = "g")
                                             && ((Deque.head t1_7s) = "g")
                                             && ((Deque.length t1_6) = 6)
                                             && ((Deque.length t1_6s) = 6)
                                             && ((Deque.head t1_6) = "f")
                                             && ((Deque.head t1_6s) = "f")
                                             && ((Deque.length t1_5) = 5)
                                             && ((Deque.length t1_5s) = 5)
                                             && ((Deque.head t1_5) = "e")
                                             && ((Deque.head t1_5s) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.length t1_4s) = 4)
                                             && ((Deque.head t1_4) = "d")
                                             && ((Deque.head t1_4s) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.length t1_3s) = 3)
                                             && ((Deque.head t1_3) = "c")
                                             && ((Deque.head t1_3s) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.length t1_2s) = 2)
                                             && ((Deque.head t1_2) = "b")
                                             && ((Deque.head t1_2s) = "b")
                                             && ((Deque.length t1_1) = 1)
                                             && ((Deque.length t1_1s) = 1)
                                             && ((Deque.head t1_1) = "a")
                                             && ((Deque.head t1_1s) = "a"))
                                            |> Expect.isTrue "Deque.head, Deque.tail, and Deque.length"
                                        }

                                        ////the previous series thoroughly tested construction by Deque.snoc, so we'll leave those out

                                        test "Deque.last, Deque.init, and Deque.length work test 1" {
                                            let t1 = Deque.init len2

                                            Expect.isTrue
                                                "Deque.last, Deque.init, and Deque.length"
                                                (((Deque.length t1) = 1) && ((Deque.last t1) = "b"))
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 2" {
                                            let t1 = Deque.init len3
                                            let t1_1 = Deque.init t1

                                            Expect.isTrue
                                                "Deque.last, Deque.init, and Deque.length"
                                                (((Deque.length t1) = 2)
                                                 && ((Deque.last t1) = "b")
                                                 && ((Deque.length t1_1) = 1)
                                                 && ((Deque.last t1_1) = "c"))
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 3" {
                                            let t1 = Deque.init len4
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1

                                            (((Deque.length t1) = 3)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 2)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 1)
                                             && ((Deque.last t1_2) = "d"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 4" {
                                            let t1 = Deque.init len5
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2

                                            (((Deque.length t1) = 4)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 3)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 2)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 1)
                                             && ((Deque.last t1_3) = "e"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 5" {
                                            let t1 = Deque.init len6
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2
                                            let t1_4 = Deque.init t1_3

                                            (((Deque.length t1) = 5)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 4)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 3)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 2)
                                             && ((Deque.last t1_3) = "e")
                                             && ((Deque.length t1_4) = 1)
                                             && ((Deque.last t1_4) = "f"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 6" {
                                            let t1 = Deque.init len7
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2
                                            let t1_4 = Deque.init t1_3
                                            let t1_5 = Deque.init t1_4

                                            (((Deque.length t1) = 6)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 5)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 4)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 3)
                                             && ((Deque.last t1_3) = "e")
                                             && ((Deque.length t1_4) = 2)
                                             && ((Deque.last t1_4) = "f")
                                             && ((Deque.length t1_5) = 1)
                                             && ((Deque.last t1_5) = "g"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 7" {
                                            let t1 = Deque.init len8
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2
                                            let t1_4 = Deque.init t1_3
                                            let t1_5 = Deque.init t1_4
                                            let t1_6 = Deque.init t1_5

                                            (((Deque.length t1) = 7)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 6)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 5)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 4)
                                             && ((Deque.last t1_3) = "e")
                                             && ((Deque.length t1_4) = 3)
                                             && ((Deque.last t1_4) = "f")
                                             && ((Deque.length t1_5) = 2)
                                             && ((Deque.last t1_5) = "g")
                                             && ((Deque.length t1_6) = 1)
                                             && ((Deque.last t1_6) = "h"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 8" {
                                            let t1 = Deque.init len9
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2
                                            let t1_4 = Deque.init t1_3
                                            let t1_5 = Deque.init t1_4
                                            let t1_6 = Deque.init t1_5
                                            let t1_7 = Deque.init t1_6

                                            (((Deque.length t1) = 8)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 7)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 6)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 5)
                                             && ((Deque.last t1_3) = "e")
                                             && ((Deque.length t1_4) = 4)
                                             && ((Deque.last t1_4) = "f")
                                             && ((Deque.length t1_5) = 3)
                                             && ((Deque.last t1_5) = "g")
                                             && ((Deque.length t1_6) = 2)
                                             && ((Deque.last t1_6) = "h")
                                             && ((Deque.length t1_7) = 1)
                                             && ((Deque.last t1_7) = "i"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "Deque.last, Deque.init, and Deque.length work test 9" {
                                            let t1 = Deque.init lena
                                            let t1_1 = Deque.init t1
                                            let t1_2 = Deque.init t1_1
                                            let t1_3 = Deque.init t1_2
                                            let t1_4 = Deque.init t1_3
                                            let t1_5 = Deque.init t1_4
                                            let t1_6 = Deque.init t1_5
                                            let t1_7 = Deque.init t1_6
                                            let t1_8 = Deque.init t1_7

                                            (((Deque.length t1) = 9)
                                             && ((Deque.last t1) = "b")
                                             && ((Deque.length t1_1) = 8)
                                             && ((Deque.last t1_1) = "c")
                                             && ((Deque.length t1_2) = 7)
                                             && ((Deque.last t1_2) = "d")
                                             && ((Deque.length t1_3) = 6)
                                             && ((Deque.last t1_3) = "e")
                                             && ((Deque.length t1_4) = 5)
                                             && ((Deque.last t1_4) = "f")
                                             && ((Deque.length t1_5) = 4)
                                             && ((Deque.last t1_5) = "g")
                                             && ((Deque.length t1_6) = 3)
                                             && ((Deque.last t1_6) = "h")
                                             && ((Deque.length t1_7) = 2)
                                             && ((Deque.last t1_7) = "i")
                                             && ((Deque.length t1_8) = 1)
                                             && ((Deque.last t1_8) = "j"))
                                            |> Expect.isTrue "Deque.last, Deque.init, and Deque.length"
                                        }

                                        test "IEnumerable Seq nth" { Expect.equal "IEnumerable nth" "e" (lena |> Seq.item 5) }

                                        test "IEnumerable Seq Deque.length" { Expect.equal "IEnumerable Deque.length" 10 (lena |> Seq.length) }

                                        test "type Deque.cons works" { Expect.equal "Deque.cons" "zz" (lena.Cons "zz" |> Deque.head) }

                                        test "IDeque Deque.cons works" { (lena :> IDeque<string>).Cons("zz").Head |> Expect.equal "" "zz" }

                                        test "Deque.ofCatLists and Deque.uncons" {
                                            let d = Deque.ofCatLists [ "a"; "b"; "c" ] [ "d"; "e"; "f" ]
                                            let h1, t1 = Deque.uncons d
                                            let h2, t2 = Deque.uncons t1
                                            let h3, t3 = Deque.uncons t2
                                            let h4, t4 = Deque.uncons t3
                                            let h5, t5 = Deque.uncons t4
                                            let h6, t6 = Deque.uncons t5

                                            Expect.isTrue
                                                "Deque.ofCatLists and Deque.uncons"
                                                ((h1 = "a")
                                                 && (h2 = "b")
                                                 && (h3 = "c")
                                                 && (h4 = "d")
                                                 && (h5 = "e")
                                                 && (h6 = "f")
                                                 && (Deque.isEmpty t6))
                                        }

                                        test "Deque.unsnoc works" {
                                            let d = Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ]
                                            let i1, l1 = Deque.unsnoc d
                                            let i2, l2 = Deque.unsnoc i1
                                            let i3, l3 = Deque.unsnoc i2
                                            let i4, l4 = Deque.unsnoc i3
                                            let i5, l5 = Deque.unsnoc i4
                                            let i6, l6 = Deque.unsnoc i5

                                            Expect.isTrue
                                                "Deque.unsnoc"
                                                ((l1 = "a")
                                                 && (l2 = "b")
                                                 && (l3 = "c")
                                                 && (l4 = "d")
                                                 && (l5 = "e")
                                                 && (l6 = "f")
                                                 && (Deque.isEmpty i6))
                                        }

                                        test "Deque.snoc pattern discriminator" {
                                            let d = (Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])
                                            let i1, l1 = Deque.unsnoc d

                                            let i2, l2 =
                                                match i1 with
                                                | Deque.Snoc(i, l) -> i, l
                                                | _ -> i1, "x"

                                            Expect.isTrue "Deque.snoc" ((l2 = "b") && ((Deque.length i2) = 4))
                                        }

                                        test "Deque.cons pattern discriminator" {
                                            let d = (Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])
                                            let h1, t1 = Deque.uncons d

                                            let h2, t2 =
                                                match t1 with
                                                | Deque.Cons(h, t) -> h, t
                                                | _ -> "x", t1

                                            Expect.isTrue "Deque.cons" ((h2 = "e") && ((Deque.length t2) = 4))
                                        }

                                        test "Deque.cons and Deque.snoc pattern discriminator" {
                                            let d = (Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])

                                            let mid1 =
                                                match d with
                                                | Deque.Cons(h, Deque.Snoc(i, l)) -> i
                                                | _ -> d

                                            let head, last =
                                                match mid1 with
                                                | Deque.Cons(h, Deque.Snoc(i, l)) -> h, l
                                                | _ -> "x", "x"

                                            Expect.isTrue "Deque.cons and Deque.snoc" ((head = "e") && (last = "b"))
                                        }

                                        test "Deque.rev empty dqueue should be empty" {
                                            Deque.empty() |> Deque.rev |> Deque.isEmpty |> Expect.isTrue ""
                                        }

                                        test "Deque.rev deque Deque.length 1" { Expect.equal "Deque.length" "a" (Deque.rev len1 |> Deque.head) }

                                        test "Deque.rev deque Deque.length 2" {
                                            let r1 = Deque.rev len2
                                            let h1 = Deque.head r1
                                            let t2 = Deque.tail r1
                                            let h2 = Deque.head t2

                                            Expect.isTrue "Deque.length" ((h1 = "a") && (h2 = "b"))
                                        }

                                        test "Deque.rev deque Deque.length 3" {
                                            let r1 = Deque.rev len3
                                            let h1 = Deque.head r1
                                            let t2 = Deque.tail r1
                                            let h2 = Deque.head t2
                                            let t3 = Deque.tail t2
                                            let h3 = Deque.head t3

                                            Expect.isTrue "Deque.length" ((h1 = "a") && (h2 = "b") && (h3 = "c"))
                                        }

                                        test "Deque.rev deque Deque.length 4" {
                                            let r1 = Deque.rev len4
                                            let h1 = Deque.head r1
                                            let t2 = Deque.tail r1
                                            let h2 = Deque.head t2
                                            let t3 = Deque.tail t2
                                            let h3 = Deque.head t3
                                            let t4 = Deque.tail t3
                                            let h4 = Deque.head t4

                                            Expect.isTrue "Deque.length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d"))
                                        }

                                        test "Deque.rev deque Deque.length 5" {
                                            let r1 = Deque.rev len5
                                            let h1 = Deque.head r1
                                            let t2 = Deque.tail r1
                                            let h2 = Deque.head t2
                                            let t3 = Deque.tail t2
                                            let h3 = Deque.head t3
                                            let t4 = Deque.tail t3
                                            let h4 = Deque.head t4
                                            let t5 = Deque.tail t4
                                            let h5 = Deque.head t5

                                            Expect.isTrue "Deque.length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e"))
                                        }

                                        test "Deque.rev deque Deque.length 6" {
                                            let r1 = Deque.rev len6
                                            let h1 = Deque.head r1
                                            let t2 = Deque.tail r1
                                            let h2 = Deque.head t2
                                            let t3 = Deque.tail t2
                                            let h3 = Deque.head t3
                                            let t4 = Deque.tail t3
                                            let h4 = Deque.head t4
                                            let t5 = Deque.tail t4
                                            let h5 = Deque.head t5
                                            let t6 = Deque.tail t5
                                            let h6 = Deque.head t6

                                            Expect.isTrue
                                                "Deque.rev Deque.length"
                                                ((h1 = "a")
                                                 && (h2 = "b")
                                                 && (h3 = "c")
                                                 && (h4 = "d")
                                                 && (h5 = "e")
                                                 && (h6 = "f"))
                                        }

                                        test "Deque.lookup Deque.length 1" { Expect.equal "" "a" (len1 |> Deque.lookup 0) }

                                        test "Deque.lookup Deque.length 2" {
                                            (((len2 |> Deque.lookup 0) = "b") && ((len2 |> Deque.lookup 1) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 3" {
                                            (((len3 |> Deque.lookup 0) = "c")
                                             && ((len3 |> Deque.lookup 1) = "b")
                                             && ((len3 |> Deque.lookup 2) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 4" {
                                            (((len4 |> Deque.lookup 0) = "d")
                                             && ((len4 |> Deque.lookup 1) = "c")
                                             && ((len4 |> Deque.lookup 2) = "b")
                                             && ((len4 |> Deque.lookup 3) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 5" {
                                            (((len5 |> Deque.lookup 0) = "e")
                                             && ((len5 |> Deque.lookup 1) = "d")
                                             && ((len5 |> Deque.lookup 2) = "c")
                                             && ((len5 |> Deque.lookup 3) = "b")
                                             && ((len5 |> Deque.lookup 4) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 6" {
                                            (((len6 |> Deque.lookup 0) = "f")
                                             && ((len6 |> Deque.lookup 1) = "e")
                                             && ((len6 |> Deque.lookup 2) = "d")
                                             && ((len6 |> Deque.lookup 3) = "c")
                                             && ((len6 |> Deque.lookup 4) = "b")
                                             && ((len6 |> Deque.lookup 5) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 7" {
                                            (((len7 |> Deque.lookup 0) = "g")
                                             && ((len7 |> Deque.lookup 1) = "f")
                                             && ((len7 |> Deque.lookup 2) = "e")
                                             && ((len7 |> Deque.lookup 3) = "d")
                                             && ((len7 |> Deque.lookup 4) = "c")
                                             && ((len7 |> Deque.lookup 5) = "b")
                                             && ((len7 |> Deque.lookup 6) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 8" {
                                            (((len8 |> Deque.lookup 0) = "h")
                                             && ((len8 |> Deque.lookup 1) = "g")
                                             && ((len8 |> Deque.lookup 2) = "f")
                                             && ((len8 |> Deque.lookup 3) = "e")
                                             && ((len8 |> Deque.lookup 4) = "d")
                                             && ((len8 |> Deque.lookup 5) = "c")
                                             && ((len8 |> Deque.lookup 6) = "b")
                                             && ((len8 |> Deque.lookup 7) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 9" {
                                            (((len9 |> Deque.lookup 0) = "i")
                                             && ((len9 |> Deque.lookup 1) = "h")
                                             && ((len9 |> Deque.lookup 2) = "g")
                                             && ((len9 |> Deque.lookup 3) = "f")
                                             && ((len9 |> Deque.lookup 4) = "e")
                                             && ((len9 |> Deque.lookup 5) = "d")
                                             && ((len9 |> Deque.lookup 6) = "c")
                                             && ((len9 |> Deque.lookup 7) = "b")
                                             && ((len9 |> Deque.lookup 8) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.lookup Deque.length 10" {
                                            (((lena |> Deque.lookup 0) = "j")
                                             && ((lena |> Deque.lookup 1) = "i")
                                             && ((lena |> Deque.lookup 2) = "h")
                                             && ((lena |> Deque.lookup 3) = "g")
                                             && ((lena |> Deque.lookup 4) = "f")
                                             && ((lena |> Deque.lookup 5) = "e")
                                             && ((lena |> Deque.lookup 6) = "d")
                                             && ((lena |> Deque.lookup 7) = "c")
                                             && ((lena |> Deque.lookup 8) = "b")
                                             && ((lena |> Deque.lookup 9) = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 1" {
                                            let a = len1 |> Deque.tryLookup 0
                                            (a.Value = "a") |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 2" {
                                            let b = len2 |> Deque.tryLookup 0
                                            let a = len2 |> Deque.tryLookup 1
                                            ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 3" {
                                            let c = len3 |> Deque.tryLookup 0
                                            let b = len3 |> Deque.tryLookup 1
                                            let a = len3 |> Deque.tryLookup 2

                                            ((c.Value = "c") && (b.Value = "b") && (a.Value = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 4" {
                                            let d = len4 |> Deque.tryLookup 0
                                            let c = len4 |> Deque.tryLookup 1
                                            let b = len4 |> Deque.tryLookup 2
                                            let a = len4 |> Deque.tryLookup 3

                                            ((d.Value = "d")
                                             && (c.Value = "c")
                                             && (b.Value = "b")
                                             && (a.Value = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 5" {
                                            let e = len5 |> Deque.tryLookup 0
                                            let d = len5 |> Deque.tryLookup 1
                                            let c = len5 |> Deque.tryLookup 2
                                            let b = len5 |> Deque.tryLookup 3
                                            let a = len5 |> Deque.tryLookup 4

                                            ((e.Value = "e")
                                             && (d.Value = "d")
                                             && (c.Value = "c")
                                             && (b.Value = "b")
                                             && (a.Value = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 6" {
                                            let f = len6 |> Deque.tryLookup 0
                                            let e = len6 |> Deque.tryLookup 1
                                            let d = len6 |> Deque.tryLookup 2
                                            let c = len6 |> Deque.tryLookup 3
                                            let b = len6 |> Deque.tryLookup 4
                                            let a = len6 |> Deque.tryLookup 5

                                            ((f.Value = "f")
                                             && (e.Value = "e")
                                             && (d.Value = "d")
                                             && (c.Value = "c")
                                             && (b.Value = "b")
                                             && (a.Value = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 7" {
                                            let g = len7 |> Deque.tryLookup 0
                                            let f = len7 |> Deque.tryLookup 1
                                            let e = len7 |> Deque.tryLookup 2
                                            let d = len7 |> Deque.tryLookup 3
                                            let c = len7 |> Deque.tryLookup 4
                                            let b = len7 |> Deque.tryLookup 5
                                            let a = len7 |> Deque.tryLookup 6

                                            ((g.Value = "g")
                                             && (f.Value = "f")
                                             && (e.Value = "e")
                                             && (d.Value = "d")
                                             && (c.Value = "c")
                                             && (b.Value = "b")
                                             && (a.Value = "a"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryLookup Deque.length 8" {
                                            let h = len8 |> Deque.tryLookup 0
                                            let g = len8 |> Deque.tryLookup 1
                                            let f = len8 |> Deque.tryLookup 2
                                            let e = len8 |> Deque.tryLookup 3
                                            let d = len8 |> Deque.tryLookup 4
                                            let c = len8 |> Deque.tryLookup 5
                                            let b = len8 |> Deque.tryLookup 6
                                            let a = len8 |> Deque.tryLookup 7

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

                                        test "Deque.tryLookup Deque.length 9" {
                                            let i = len9 |> Deque.tryLookup 0
                                            let h = len9 |> Deque.tryLookup 1
                                            let g = len9 |> Deque.tryLookup 2
                                            let f = len9 |> Deque.tryLookup 3
                                            let e = len9 |> Deque.tryLookup 4
                                            let d = len9 |> Deque.tryLookup 5
                                            let c = len9 |> Deque.tryLookup 6
                                            let b = len9 |> Deque.tryLookup 7
                                            let a = len9 |> Deque.tryLookup 8

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

                                        test "Deque.tryLookup Deque.length 10" {
                                            let j = lena |> Deque.tryLookup 0
                                            let i = lena |> Deque.tryLookup 1
                                            let h = lena |> Deque.tryLookup 2
                                            let g = lena |> Deque.tryLookup 3
                                            let f = lena |> Deque.tryLookup 4
                                            let e = lena |> Deque.tryLookup 5
                                            let d = lena |> Deque.tryLookup 6
                                            let c = lena |> Deque.tryLookup 7
                                            let b = lena |> Deque.tryLookup 8
                                            let a = lena |> Deque.tryLookup 9

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

                                        test "Deque.tryLookup not found" { lena |> Deque.tryLookup 10 |> Expect.isNone "" }

                                        test "Deque.remove elements Deque.length 1" { len1 |> Deque.remove 0 |> Deque.isEmpty |> Expect.isTrue "" }

                                        test "Deque.remove elements Deque.length 2" {
                                            let a = len2 |> Deque.remove 0 |> Deque.head
                                            let b = len2 |> Deque.remove 1 |> Deque.head
                                            ((a = "a") && (b = "b")) |> Expect.isTrue ""
                                        }

                                        test "Deque.remove elements Deque.length 3" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.remove 0
                                            let b0 = Deque.head r0
                                            let t0 = Deque.tail r0
                                            let c0 = Deque.head t0

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.remove 1
                                            let a1 = Deque.head r1
                                            let t1 = Deque.tail r1
                                            let c1 = Deque.head t1

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.remove 2
                                            let a2 = Deque.head r2
                                            let t2 = Deque.tail r2
                                            let b2 = Deque.head t2

                                            ((b0 = "b")
                                             && (c0 = "c")
                                             && (a1 = "a")
                                             && (c1 = "c")
                                             && (a2 = "a")
                                             && (b2 = "b"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.remove elements Deque.length 4" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.remove 0
                                            let b0 = Deque.head r0
                                            let t0 = Deque.tail r0
                                            let c0 = Deque.head t0
                                            let t01 = Deque.tail t0
                                            let d0 = Deque.head t01

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.remove 1
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.remove 2
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.remove 3
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32

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

                                        test "Deque.remove elements Deque.length 5" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.remove 0
                                            let b0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let c0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let d0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let e0 = Deque.head t03

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.remove 1
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let e1 = Deque.head t13

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.remove 2
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let e2 = Deque.head t23

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.remove 3
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let e3 = Deque.head t33

                                            let r4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.remove 4
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43

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

                                        test "Deque.remove elements Deque.length 6" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 0
                                            let b0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let c0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let d0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let e0 = Deque.head t03
                                            let t04 = Deque.tail t03
                                            let f0 = Deque.head t04

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 1
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let e1 = Deque.head t13
                                            let t14 = Deque.tail t13
                                            let f1 = Deque.head t14

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 2
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let e2 = Deque.head t23
                                            let t24 = Deque.tail t23
                                            let f2 = Deque.head t24

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 3
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let e3 = Deque.head t33
                                            let t34 = Deque.tail t33
                                            let f3 = Deque.head t34

                                            let r4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 4
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43
                                            let t44 = Deque.tail t43
                                            let f4 = Deque.head t44

                                            let r5 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.remove 5
                                            let a5 = Deque.head r5
                                            let t51 = Deque.tail r5
                                            let b5 = Deque.head t51
                                            let t52 = Deque.tail t51
                                            let c5 = Deque.head t52
                                            let t53 = Deque.tail t52
                                            let d5 = Deque.head t53
                                            let t54 = Deque.tail t53
                                            let e5 = Deque.head t54

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

                                        test "tryRemoveempty" { Deque.empty() |> Deque.tryRemove 0 |> Expect.isNone "" }

                                        test "Deque.tryRemove elements Deque.length 1" {
                                            let a = len1 |> Deque.tryRemove 0
                                            a.Value |> Deque.isEmpty |> Expect.isTrue ""
                                        }

                                        test "Deque.tryRemove elements Deque.length 2" {
                                            let a = len2 |> Deque.tryRemove 0
                                            let a1 = Deque.head a.Value
                                            let b = len2 |> Deque.tryRemove 1
                                            let b1 = Deque.head b.Value
                                            ((a1 = "a") && (b1 = "b")) |> Expect.isTrue ""
                                        }

                                        test "Deque.tryRemove elements Deque.length 3" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryRemove 0
                                            let r0 = x0.Value
                                            let b0 = Deque.head r0
                                            let t0 = Deque.tail r0
                                            let c0 = Deque.head t0

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryRemove 1
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t1 = Deque.tail r1
                                            let c1 = Deque.head t1

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryRemove 2
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t2 = Deque.tail r2
                                            let b2 = Deque.head t2

                                            ((b0 = "b")
                                             && (c0 = "c")
                                             && (a1 = "a")
                                             && (c1 = "c")
                                             && (a2 = "a")
                                             && (b2 = "b"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryRemove elements Deque.length 4" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryRemove 0
                                            let r0 = x0.Value
                                            let b0 = Deque.head r0
                                            let t0 = Deque.tail r0
                                            let c0 = Deque.head t0
                                            let t01 = Deque.tail t0
                                            let d0 = Deque.head t01

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryRemove 1
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryRemove 2
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22

                                            let x3 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryRemove 3
                                            let r3 = x3.Value
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32

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

                                        test "Deque.tryRemove elements Deque.length 5" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.tryRemove 0
                                            let r0 = x0.Value
                                            let b0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let c0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let d0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let e0 = Deque.head t03

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.tryRemove 1
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let e1 = Deque.head t13

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.tryRemove 2
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let e2 = Deque.head t23

                                            let x3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.tryRemove 3
                                            let r3 = x3.Value
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let e3 = Deque.head t33

                                            let x4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.tryRemove 4
                                            let r4 = x4.Value
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43

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

                                        test "Deque.tryRemove elements Deque.length 6" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 0
                                            let r0 = x0.Value
                                            let b0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let c0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let d0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let e0 = Deque.head t03
                                            let t04 = Deque.tail t03
                                            let f0 = Deque.head t04

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 1
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let c1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let d1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let e1 = Deque.head t13
                                            let t14 = Deque.tail t13
                                            let f1 = Deque.head t14

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 2
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let e2 = Deque.head t23
                                            let t24 = Deque.tail t23
                                            let f2 = Deque.head t24

                                            let x3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 3
                                            let r3 = x3.Value
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let e3 = Deque.head t33
                                            let t34 = Deque.tail t33
                                            let f3 = Deque.head t34

                                            let x4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 4
                                            let r4 = x4.Value
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43
                                            let t44 = Deque.tail t43
                                            let f4 = Deque.head t44

                                            let x5 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.tryRemove 5
                                            let r5 = x5.Value
                                            let a5 = Deque.head r5
                                            let t51 = Deque.tail r5
                                            let b5 = Deque.head t51
                                            let t52 = Deque.tail t51
                                            let c5 = Deque.head t52
                                            let t53 = Deque.tail t52
                                            let d5 = Deque.head t53
                                            let t54 = Deque.tail t53
                                            let e5 = Deque.head t54

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

                                        test "Deque.update elements Deque.length 1" {
                                            len1 |> Deque.update 0 "aa" |> Deque.head |> Expect.equal "" "aa"
                                        }

                                        test "Deque.update elements Deque.length 2" {
                                            let r0 = (Deque.ofSeq [ "a"; "b" ]) |> Deque.update 0 "zz"
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01

                                            let r1 = (Deque.ofSeq [ "a"; "b" ]) |> Deque.update 1 "zz"
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11

                                            ((a0 = "zz") && (b0 = "b") && (a1 = "a") && (b1 = "zz"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.update elements Deque.length 3" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.update 0 "zz"
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.update 1 "zz"
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.update 2 "zz"
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22

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

                                        test "Deque.update elements Deque.length 4" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.update 0 "zz"
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let d0 = Deque.head t03

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.update 1 "zz"
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let d1 = Deque.head t13

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.update 2 "zz"
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let d2 = Deque.head t23

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.update 3 "zz"
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let d3 = Deque.head t33

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

                                        test "Deque.update elements Deque.length 5" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.update 0 "zz"
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let d0 = Deque.head t03
                                            let t04 = Deque.tail t03
                                            let e0 = Deque.head t04

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.update 1 "zz"
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let d1 = Deque.head t13
                                            let t14 = Deque.tail t13
                                            let e1 = Deque.head t14

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.update 2 "zz"
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let d2 = Deque.head t23
                                            let t24 = Deque.tail t23
                                            let e2 = Deque.head t24

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.update 3 "zz"
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let d3 = Deque.head t33
                                            let t34 = Deque.tail t33
                                            let e3 = Deque.head t34

                                            let r4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e" ]) |> Deque.update 4 "zz"
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43
                                            let t44 = Deque.tail t43
                                            let e4 = Deque.head t44

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

                                        test "Deque.update elements Deque.length 6" {
                                            let r0 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 0 "zz"
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let d0 = Deque.head t03
                                            let t04 = Deque.tail t03
                                            let e0 = Deque.head t04
                                            let t05 = Deque.tail t04
                                            let f0 = Deque.head t05

                                            let r1 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 1 "zz"
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let d1 = Deque.head t13
                                            let t14 = Deque.tail t13
                                            let e1 = Deque.head t14
                                            let t15 = Deque.tail t14
                                            let f1 = Deque.head t15

                                            let r2 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 2 "zz"
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let d2 = Deque.head t23
                                            let t24 = Deque.tail t23
                                            let e2 = Deque.head t24
                                            let t25 = Deque.tail t24
                                            let f2 = Deque.head t25

                                            let r3 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 3 "zz"
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let d3 = Deque.head t33
                                            let t34 = Deque.tail t33
                                            let e3 = Deque.head t34
                                            let t35 = Deque.tail t34
                                            let f3 = Deque.head t35

                                            let r4 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 4 "zz"
                                            let a4 = Deque.head r4
                                            let t41 = Deque.tail r4
                                            let b4 = Deque.head t41
                                            let t42 = Deque.tail t41
                                            let c4 = Deque.head t42
                                            let t43 = Deque.tail t42
                                            let d4 = Deque.head t43
                                            let t44 = Deque.tail t43
                                            let e4 = Deque.head t44
                                            let t45 = Deque.tail t44
                                            let f4 = Deque.head t45

                                            let r5 = (Deque.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f" ]) |> Deque.update 5 "zz"
                                            let a5 = Deque.head r5
                                            let t51 = Deque.tail r5
                                            let b5 = Deque.head t51
                                            let t52 = Deque.tail t51
                                            let c5 = Deque.head t52
                                            let t53 = Deque.tail t52
                                            let d5 = Deque.head t53
                                            let t54 = Deque.tail t53
                                            let e5 = Deque.head t54
                                            let t55 = Deque.tail t54
                                            let f5 = Deque.head t55

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

                                        test "Deque.tryUpdate elements Deque.length 1" {
                                            let a = len1 |> Deque.tryUpdate 0 "aa"
                                            a.Value |> Deque.head |> Expect.equal "" "aa"
                                        }

                                        test "Deque.tryUpdate elements Deque.length 2" {
                                            let x0 = (Deque.ofSeq [ "a"; "b" ]) |> Deque.tryUpdate 0 "zz"
                                            let r0 = x0.Value
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01

                                            let x1 = (Deque.ofSeq [ "a"; "b" ]) |> Deque.tryUpdate 1 "zz"
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11

                                            ((a0 = "zz") && (b0 = "b") && (a1 = "a") && (b1 = "zz"))
                                            |> Expect.isTrue ""
                                        }

                                        test "Deque.tryUpdate elements Deque.length 3" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryUpdate 0 "zz"
                                            let r0 = x0.Value
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryUpdate 1 "zz"
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c" ]) |> Deque.tryUpdate 2 "zz"
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22

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

                                        test "Deque.tryUpdate elements Deque.length 4" {
                                            let x0 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryUpdate 0 "zz"
                                            let r0 = x0.Value
                                            let a0 = Deque.head r0
                                            let t01 = Deque.tail r0
                                            let b0 = Deque.head t01
                                            let t02 = Deque.tail t01
                                            let c0 = Deque.head t02
                                            let t03 = Deque.tail t02
                                            let d0 = Deque.head t03

                                            let x1 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryUpdate 1 "zz"
                                            let r1 = x1.Value
                                            let a1 = Deque.head r1
                                            let t11 = Deque.tail r1
                                            let b1 = Deque.head t11
                                            let t12 = Deque.tail t11
                                            let c1 = Deque.head t12
                                            let t13 = Deque.tail t12
                                            let d1 = Deque.head t13

                                            let x2 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryUpdate 2 "zz"
                                            let r2 = x2.Value
                                            let a2 = Deque.head r2
                                            let t21 = Deque.tail r2
                                            let b2 = Deque.head t21
                                            let t22 = Deque.tail t21
                                            let c2 = Deque.head t22
                                            let t23 = Deque.tail t22
                                            let d2 = Deque.head t23

                                            let x3 = (Deque.ofSeq [ "a"; "b"; "c"; "d" ]) |> Deque.tryUpdate 3 "zz"
                                            let r3 = x3.Value
                                            let a3 = Deque.head r3
                                            let t31 = Deque.tail r3
                                            let b3 = Deque.head t31
                                            let t32 = Deque.tail t31
                                            let c3 = Deque.head t32
                                            let t33 = Deque.tail t32
                                            let d3 = Deque.head t33

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

                                        test "Deque.tryUncons on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "Deque.tryUncons" <| Deque.tryUncons q
                                        }

                                        test "Deque.tryUncons on q" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            let x, _ = (Deque.tryUncons q).Value
                                            Expect.equal "Deque.tryUncons" "a" x
                                        }

                                        test "Deque.tryUnsnoc on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "Deque.tryUnsnoc" <| Deque.tryUnsnoc q
                                        }

                                        test "Deque.tryUnsnoc on q" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            let _, x = (Deque.tryUnsnoc q).Value
                                            Expect.equal "Deque.tryUnsnoc" "d" x
                                        }

                                        test "Deque.tryGetHead on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "tryHead" <| Deque.tryGetHead q
                                        }

                                        test "Deque.tryGetHead on q" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            Expect.equal "tryHead" "a" (Deque.tryGetHead q).Value
                                        }

                                        test "Deque.tryGetInit on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "tryInitial" <| Deque.tryGetInit q
                                        }

                                        test "Deque.tryGetInit on q" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            let x = (Deque.tryGetInit q).Value
                                            let x2 = x |> Deque.last
                                            Expect.equal "tryinitial" "c" x2
                                        }

                                        test "Deque.tryGetLast on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "tryLast" <| Deque.tryGetLast q
                                        }

                                        test "Deque.tryGetLast on deque" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            Expect.equal "tryLast" "d" (Deque.tryGetLast q).Value
                                            Expect.equal "tryLast" "a" (len2 |> Deque.tryGetLast).Value
                                            Expect.equal "tryLast" "a" (len2snoc |> Deque.tryGetLast).Value
                                        }

                                        test "Deque.tryGetTail on empty" {
                                            let q = Deque.empty()
                                            Expect.isNone "tryTail" <| Deque.tryGetTail q
                                        }

                                        test "Deque.tryGetTail on q" {
                                            let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                                            Expect.equal "tryTail" "b" ((Deque.tryGetTail q).Value |> Deque.head)
                                        }

                                        test "deprecated structure still works" {
                                            let q = Deque.empty()
                                            (Deque.tryGetTail q = None) |> Expect.isTrue ""
                                        } ]

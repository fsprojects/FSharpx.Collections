namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Properties
open FsCheck
open Expecto
open Expecto.Flip

module DequeTests =

    [<Tests>]
    let testDeque =

        //quite a lot going on and difficult to reason about edge cases
        //testing up to length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)

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

        let len1conj = Deque.empty |> Deque.conj "a"
        let len2conj = Deque.empty |> Deque.conj "b" |> Deque.conj "a"
        let len3conj = Deque.empty |> Deque.conj "c" |> Deque.conj "b" |> Deque.conj "a"

        let len4conj =
            Deque.empty
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let len5conj =
            Deque.empty
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let len6conj =
            Deque.empty
            |> Deque.conj "f"
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let len7conj =
            Deque.empty
            |> Deque.conj "g"
            |> Deque.conj "f"
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let len8conj =
            Deque.empty
            |> Deque.conj "h"
            |> Deque.conj "g"
            |> Deque.conj "f"
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let len9conj =
            Deque.empty
            |> Deque.conj "i"
            |> Deque.conj "h"
            |> Deque.conj "g"
            |> Deque.conj "f"
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        let lenaconj =
            Deque.empty
            |> Deque.conj "j"
            |> Deque.conj "i"
            |> Deque.conj "h"
            |> Deque.conj "g"
            |> Deque.conj "f"
            |> Deque.conj "e"
            |> Deque.conj "d"
            |> Deque.conj "c"
            |> Deque.conj "b"
            |> Deque.conj "a"

        testList
            "Deque"
            [ test "empty dqueue should be empty" { Expect.isTrue "empty is empty" (Deque.empty |> Deque.isEmpty) }

              test "Deque.cons works" { Expect.isFalse "not empty" (len2 |> Deque.isEmpty) }

              test "Deque.conj works" { Expect.isFalse "" (len2conj |> Deque.isEmpty) }

              test "Deque.singleton head works" { Expect.equal "Deque.singleton" "a" (len1 |> Deque.head) }

              test "Deque.singleton last works" { Expect.equal "" "a" (len1 |> Deque.last) }

              test "TryUncons wind-down to None" {
                  let q = Deque.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let rec loop(q': Deque<string>) =
                      match (q'.TryUncons) with
                      | Some(hd, tl) -> loop tl
                      | None -> ()

                  Expect.equal "unit" () <| loop q
              }

              test "Uncons wind-down to None" {
                  let q = Deque.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                  let rec loop(q': Deque<string>) =
                      match (q'.Uncons) with
                      | hd, tl when tl.Length = 0 -> ()
                      | hd, tl -> loop tl

                  Expect.equal "unit" () <| loop q
              }

              test "toSeq works" {
                  let q = Deque.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]
                  let l = List.ofSeq q
                  Expect.equal "toSeq" l <| List.ofSeq(Deque.toSeq q)
              }

              test "Deque.tail of Deque.singleton empty" {
                  Expect.isTrue "Deque.isEmpty" (len1 |> Deque.tail |> Deque.isEmpty)
                  Expect.isTrue "Deque.isEmpty" (len1conj |> Deque.tail |> Deque.isEmpty)
              }

              test "Deque.tail of Deque.tail of 2 empty" {
                  Expect.isTrue "Deque.isEmpty" (len2 |> Deque.tail |> Deque.tail |> Deque.isEmpty)
                  Expect.isTrue "Deque.isEmpty" (len2conj |> Deque.tail |> Deque.tail |> Deque.isEmpty)
              }

              test "Deque.initial of Deque.singleton empty" {
                  Expect.isTrue "Deque.isEmpty" (len1 |> Deque.initial |> Deque.isEmpty)
                  Expect.isTrue "Deque.isEmpty" (len1conj |> Deque.initial |> Deque.isEmpty)
              }

              test "head, Deque.tail, and length work test 1" {
                  let t1 = Deque.tail len2
                  let t1s = Deque.tail len2conj

                  Expect.isTrue
                      "head, Deque.tail, and length"
                      (((Deque.length t1) = 1)
                       && ((Deque.length t1s) = 1)
                       && ((Deque.head t1) = "a")
                       && ((Deque.head t1s) = "a"))
              }

              test "head, Deque.tail, and length work test 2" {
                  let t1 = Deque.tail len3
                  let t1s = Deque.tail len3conj

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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 3" {
                  let t1 = Deque.tail len4
                  let t1s = Deque.tail len4conj

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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 4" {
                  let t1 = Deque.tail len5
                  let t1s = Deque.tail len5conj

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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 5" {
                  let t1 = Deque.tail len6
                  let t1s = Deque.tail len6conj

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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 6" {
                  let t1 = Deque.tail len7
                  let t1s = Deque.tail len7conj

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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 7" {
                  let t1 = Deque.tail len8
                  let t1s = Deque.tail len8conj
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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 8" {
                  let t1 = Deque.tail len9
                  let t1s = Deque.tail len9conj
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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              test "head, Deque.tail, and length work test 9" {
                  let t1 = Deque.tail lena
                  let t1s = Deque.tail lenaconj
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
                  |> Expect.isTrue "head, Deque.tail, and length"
              }

              //the previous series thoroughly tested construction by Deque.conj, so we'll leave those out
              test "last, init, and length work test 1" {
                  let t1 = Deque.initial len2
                  Expect.isTrue "last, init, and length" (((Deque.length t1) = 1) && ((Deque.last t1) = "b"))
              }

              test "last, init, and length work test 2" {
                  let t1 = Deque.initial len3
                  let t1_1 = Deque.initial t1

                  Expect.isTrue
                      "last, init, and length"
                      (((Deque.length t1) = 2)
                       && ((Deque.last t1) = "b")
                       && ((Deque.length t1_1) = 1)
                       && ((Deque.last t1_1) = "c"))
              }

              test "last, init, and length work test 3" {
                  let t1 = Deque.initial len4
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1

                  (((Deque.length t1) = 3)
                   && ((Deque.last t1) = "b")
                   && ((Deque.length t1_1) = 2)
                   && ((Deque.last t1_1) = "c")
                   && ((Deque.length t1_2) = 1)
                   && ((Deque.last t1_2) = "d"))
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 4" {
                  let t1 = Deque.initial len5
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2

                  (((Deque.length t1) = 4)
                   && ((Deque.last t1) = "b")
                   && ((Deque.length t1_1) = 3)
                   && ((Deque.last t1_1) = "c")
                   && ((Deque.length t1_2) = 2)
                   && ((Deque.last t1_2) = "d")
                   && ((Deque.length t1_3) = 1)
                   && ((Deque.last t1_3) = "e"))
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 5" {
                  let t1 = Deque.initial len6
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2
                  let t1_4 = Deque.initial t1_3

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
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 6" {
                  let t1 = Deque.initial len7
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2
                  let t1_4 = Deque.initial t1_3
                  let t1_5 = Deque.initial t1_4

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
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 7" {
                  let t1 = Deque.initial len8
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2
                  let t1_4 = Deque.initial t1_3
                  let t1_5 = Deque.initial t1_4
                  let t1_6 = Deque.initial t1_5

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
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 8" {
                  let t1 = Deque.initial len9
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2
                  let t1_4 = Deque.initial t1_3
                  let t1_5 = Deque.initial t1_4
                  let t1_6 = Deque.initial t1_5
                  let t1_7 = Deque.initial t1_6

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
                  |> Expect.isTrue "last, init, and length"
              }

              test "last, init, and length work test 9" {
                  let t1 = Deque.initial lena
                  let t1_1 = Deque.initial t1
                  let t1_2 = Deque.initial t1_1
                  let t1_3 = Deque.initial t1_2
                  let t1_4 = Deque.initial t1_3
                  let t1_5 = Deque.initial t1_4
                  let t1_6 = Deque.initial t1_5
                  let t1_7 = Deque.initial t1_6
                  let t1_8 = Deque.initial t1_7

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
                  |> Expect.isTrue "last, init, and length"
              }

              test "IEnumerable Seq nth" { Expect.equal "IEnumerable nth" "e" (lena |> Seq.item 5) }

              test "IEnumerable Seq length" { Expect.equal "IEnumerable length" 10 (lena |> Seq.length) }

              test "type Deque.cons works" { Expect.equal "Deque.cons" "zz" (lena.Cons "zz" |> Deque.head) }

              test "ofCatLists and Deque.uncons" {
                  let d = Deque.ofCatLists [ "a"; "b"; "c" ] [ "d"; "e"; "f" ]
                  let h1, t1 = Deque.uncons d
                  let h2, t2 = Deque.uncons t1
                  let h3, t3 = Deque.uncons t2
                  let h4, t4 = Deque.uncons t3
                  let h5, t5 = Deque.uncons t4
                  let h6, t6 = Deque.uncons t5

                  Expect.isTrue
                      "ofCatLists and Deque.uncons"
                      ((h1 = "a")
                       && (h2 = "b")
                       && (h3 = "c")
                       && (h4 = "d")
                       && (h5 = "e")
                       && (h6 = "f")
                       && (Deque.isEmpty t6))
              }

              test "Deque.unconj works" {
                  let d = Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ]
                  let i1, l1 = Deque.unconj d
                  let i2, l2 = Deque.unconj i1
                  let i3, l3 = Deque.unconj i2
                  let i4, l4 = Deque.unconj i3
                  let i5, l5 = Deque.unconj i4
                  let i6, l6 = Deque.unconj i5

                  Expect.isTrue
                      "Deque.unconj"
                      ((l1 = "a")
                       && (l2 = "b")
                       && (l3 = "c")
                       && (l4 = "d")
                       && (l5 = "e")
                       && (l6 = "f")
                       && (Deque.isEmpty i6))
              }

              test "Deque.conj pattern discriminator" {
                  let d = (Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])
                  let i1, l1 = Deque.unconj d

                  let i2, l2 =
                      match i1 with
                      | Deque.Conj(i, l) -> i, l
                      | _ -> i1, "x"

                  Expect.isTrue "Deque.conj" ((l2 = "b") && ((Deque.length i2) = 4))
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

              test "Deque.cons and Deque.conj pattern discriminator" {
                  let d = (Deque.ofCatLists [ "f"; "e"; "d" ] [ "c"; "b"; "a" ])

                  let mid1 =
                      match d with
                      | Deque.Cons(h, Deque.Conj(i, l)) -> i
                      | _ -> d

                  let head, last =
                      match mid1 with
                      | Deque.Cons(h, Deque.Conj(i, l)) -> h, l
                      | _ -> "x", "x"

                  Expect.isTrue "Deque.cons and Deque.conj" ((head = "e") && (last = "b"))
              }

              test "rev deque length 1" { Expect.equal "length" "a" (Deque.rev len1 |> Deque.head) }

              test "rev deque length 2" {
                  let r1 = Deque.rev len2
                  let h1 = Deque.head r1
                  let t2 = Deque.tail r1
                  let h2 = Deque.head t2

                  Expect.isTrue "length" ((h1 = "a") && (h2 = "b"))
              }

              test "rev deque length 3" {
                  let r1 = Deque.rev len3
                  let h1 = Deque.head r1
                  let t2 = Deque.tail r1
                  let h2 = Deque.head t2
                  let t3 = Deque.tail t2
                  let h3 = Deque.head t3

                  Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c"))
              }

              test "rev deque length 4" {
                  let r1 = Deque.rev len4
                  let h1 = Deque.head r1
                  let t2 = Deque.tail r1
                  let h2 = Deque.head t2
                  let t3 = Deque.tail t2
                  let h3 = Deque.head t3
                  let t4 = Deque.tail t3
                  let h4 = Deque.head t4

                  Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d"))
              }

              test "rev deque length 5" {
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

                  Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e"))
              }

              test "rev deque length 6" {
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
                      "rev length"
                      ((h1 = "a")
                       && (h2 = "b")
                       && (h3 = "c")
                       && (h4 = "d")
                       && (h5 = "e")
                       && (h6 = "f"))
              }

              test "Deque.tryUncons on empty" {
                  let q = Deque.empty
                  Expect.isNone "Deque.tryUncons" <| Deque.tryUncons q
              }

              test "Deque.tryUncons on q" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  let x, _ = (Deque.tryUncons q).Value
                  Expect.equal "Deque.tryUncons" "a" x
              }

              test "tryUnconj on empty" {
                  let q = Deque.empty
                  Expect.isNone "tryUnconj" <| Deque.tryUnconj q
              }

              test "tryUnconj on q" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  let _, x = (Deque.tryUnconj q).Value
                  Expect.equal "tryUnconj" "d" x
              }

              test "tryHead on empty" {
                  let q = Deque.empty
                  Expect.isNone "tryHead" <| Deque.tryHead q
              }

              test "tryHead on q" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  Expect.equal "tryHead" "a" (Deque.tryHead q).Value
              }

              test "tryInitial on empty" {
                  let q = Deque.empty
                  Expect.isNone "tryInitial" <| Deque.tryInitial q
              }

              test "tryinitial on q" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  let x = (Deque.tryInitial q).Value
                  let x2 = x |> Deque.last
                  Expect.equal "tryinitial" "c" x2
              }

              test "tryLast on empty" {
                  let q = Deque.empty
                  Expect.isNone "tryLast" <| Deque.tryLast q
              }

              test "tryLast on deque" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  Expect.equal "tryLast" "d" (Deque.tryLast q).Value
                  Expect.equal "tryLast" "a" (len2 |> Deque.tryLast).Value
                  Expect.equal "tryLast" "a" (len2conj |> Deque.tryLast).Value
              }

              test "tryTail on empty" {
                  let q = Deque.empty
                  Expect.isNone "tryTail" <| Deque.tryTail q
              }

              test "tryTail on q" {
                  let q = Deque.ofSeq [ "a"; "b"; "c"; "d" ]
                  Expect.equal "tryTail" "b" ((Deque.tryTail q).Value |> Deque.head)
              }

              test "structural equality" {

                  let l1 = Deque.ofSeq [ 1..100 ]
                  let l2 = Deque.ofSeq [ 1..100 ]

                  Expect.equal "equality" l1 l2

                  let l3 = Deque.ofSeq [ 1..99 ] |> Deque.conj 7

                  Expect.notEqual "equality" l1 l3
              } ]

    [<Tests>]
    let propertyTestDeque =

        let conjThruList l q =
            let rec loop (q': 'a Deque) (l': 'a list) =
                match l' with
                | hd :: tl -> loop (q'.Conj hd) tl
                | [] -> q'

            loop q l
        (*
        non-Deque generators from random ofList
        *)
        let dequeOfListGen =
            gen {
                let! n = Gen.length2thru12
                let! x = Gen.listInt n
                return ((Deque.ofList x), x)
            }

        (*
        Deque generators from random ofSeq and/or Deque.conj elements from random list 
        *)
        let dequeIntGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listInt n
                let! y = Gen.listInt n2
                return ((Deque.ofSeq x |> conjThruList y), (x @ y))
            }

        let dequeIntOfSeqGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Deque.ofSeq x), x)
            }

        let dequeIntConjGen =
            gen {
                let! n = Gen.length1thru12
                let! x = Gen.listInt n
                return ((Deque.empty |> conjThruList x), x)
            }

        let dequeObjGen =
            gen {
                let! n = Gen.length2thru12
                let! n2 = Gen.length1thru12
                let! x = Gen.listObj n
                let! y = Gen.listObj n2
                return ((Deque.ofSeq x |> conjThruList y), (x @ y))
            }

        let dequeStringGen =
            gen {
                let! n = Gen.length1thru12
                let! n2 = Gen.length2thru12
                let! x = Gen.listString n
                let! y = Gen.listString n2
                return ((Deque.ofSeq x |> conjThruList y), (x @ y))
            }

        let intGens start =
            let v = Array.create 3 dequeIntGen
            v.[1] <- dequeIntOfSeqGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[2] <- dequeIntConjGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v

        let intGensStart1 = intGens 1 //this will accept all

        let intGensStart2 = intGens 2 // this will accept 11 out of 12

        testList
            "Deque property tests"
            [

              testPropertyWithConfig
                  config10k
                  "Deque fold matches build list rev"
                  (Prop.forAll(Arb.fromGen dequeIntGen)
                   <| fun (q, l) -> q |> Deque.fold (fun l' elem -> elem :: l') [] = List.rev l)

              testPropertyWithConfig
                  config10k
                  "Deque OfSeq fold matches build list rev"
                  (Prop.forAll(Arb.fromGen dequeIntOfSeqGen)
                   <| fun (q, l) -> q |> Deque.fold (fun l' elem -> elem :: l') [] = List.rev l)

              testPropertyWithConfig
                  config10k
                  "Deque Conj fold matches build list rev"
                  (Prop.forAll(Arb.fromGen dequeIntConjGen)
                   <| fun (q, l) -> q |> Deque.fold (fun l' elem -> elem :: l') [] = List.rev l)

              testPropertyWithConfig
                  config10k
                  "Deque foldback matches build list"
                  (Prop.forAll(Arb.fromGen dequeIntGen)
                   <| fun (q, l) -> Deque.foldBack (fun elem l' -> elem :: l') q [] = l)

              testPropertyWithConfig
                  config10k
                  "Deque OfSeq foldback matches build list"
                  (Prop.forAll(Arb.fromGen dequeIntOfSeqGen)
                   <| fun (q, l) -> Deque.foldBack (fun elem l' -> elem :: l') q [] = l)

              testPropertyWithConfig
                  config10k
                  "Deque Conj foldback matches build list"
                  (Prop.forAll(Arb.fromGen dequeIntConjGen)
                   <| fun (q, l) -> Deque.foldBack (fun elem l' -> elem :: l') q [] = l)

              testPropertyWithConfig
                  config10k
                  "int deque builds and serializes 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "int deque builds and serializes 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "int deque builds and serializes 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "obj deque builds and serializes"
                  (Prop.forAll(Arb.fromGen dequeObjGen)
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "string deque builds and serializes"
                  (Prop.forAll(Arb.fromGen dequeStringGen)
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "obj Deque reverse . reverse = id"
                  (Prop.forAll(Arb.fromGen dequeObjGen)
                   <| fun (q, l) -> q |> Deque.rev |> Deque.rev |> Seq.toList = (q |> Seq.toList))

              testPropertyWithConfig
                  config10k
                  "Deque ofList build and serialize"
                  (Prop.forAll(Arb.fromGen dequeOfListGen)
                   <| fun (q, l) -> q |> Seq.toList = l)

              testPropertyWithConfig
                  config10k
                  "get head from deque 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> Deque.head q = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get head from deque 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> Deque.head q = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get head from deque 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> Deque.head q = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get head from deque safely 0"
                  (Prop.forAll(Arb.fromGen intGensStart1.[0])
                   <| fun (q, l) -> (Deque.tryHead q).Value = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get head from deque safely 1"
                  (Prop.forAll(Arb.fromGen intGensStart1.[1])
                   <| fun (q, l) -> (Deque.tryHead q).Value = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get head from deque safely 2"
                  (Prop.forAll(Arb.fromGen intGensStart1.[2])
                   <| fun (q, l) -> (Deque.tryHead q).Value = List.item 0 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (q, l) -> q.Tail.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (q, l) -> q.Tail.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (q, l) -> q.Tail.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque safely 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (q, l) -> q.TryTail.Value.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque safely 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (q, l) -> q.TryTail.Value.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.tail from deque safely 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (q, l) -> q.TryTail.Value.Head = List.item 1 l)

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (q, l) -> List.ofSeq(Deque.initial q) = (List.rev l |> List.tail |> List.rev))

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (q, l) -> List.ofSeq(Deque.initial q) = (List.rev l |> List.tail |> List.rev))

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (q, l) -> List.ofSeq(Deque.initial q) = (List.rev l |> List.tail |> List.rev))

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque safely 0"
                  (Prop.forAll(Arb.fromGen intGensStart2.[0])
                   <| fun (q, l) -> List.ofSeq q.TryInitial.Value = (List.rev l |> List.tail |> List.rev))

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque safely 1"
                  (Prop.forAll(Arb.fromGen intGensStart2.[1])
                   <| fun (q, l) -> List.ofSeq q.TryInitial.Value = (List.rev l |> List.tail |> List.rev))

              testPropertyWithConfig
                  config10k
                  "get Deque.initial from deque safely 2"
                  (Prop.forAll(Arb.fromGen intGensStart2.[2])
                   <| fun (q, l) -> List.ofSeq q.TryInitial.Value = (List.rev l |> List.tail |> List.rev)) ]

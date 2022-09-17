namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//only going up to len5 is probably sufficient to test all edge cases
//but better too many unit tests than too few

module AltBinaryRandomAccessListTest =

    let len1 = AltBinaryRandomAccessList.empty |> AltBinaryRandomAccessList.cons "a"

    let len2 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"

    let len3 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"

    let len4 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"

    let len5 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"

    let len6 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"
        |> AltBinaryRandomAccessList.cons "f"

    let len7 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"
        |> AltBinaryRandomAccessList.cons "f"
        |> AltBinaryRandomAccessList.cons "g"

    let len8 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"
        |> AltBinaryRandomAccessList.cons "f"
        |> AltBinaryRandomAccessList.cons "g"
        |> AltBinaryRandomAccessList.cons "h"

    let len9 =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"
        |> AltBinaryRandomAccessList.cons "f"
        |> AltBinaryRandomAccessList.cons "g"
        |> AltBinaryRandomAccessList.cons "h"
        |> AltBinaryRandomAccessList.cons "i"

    let lena =
        AltBinaryRandomAccessList.empty
        |> AltBinaryRandomAccessList.cons "a"
        |> AltBinaryRandomAccessList.cons "b"
        |> AltBinaryRandomAccessList.cons "c"
        |> AltBinaryRandomAccessList.cons "d"
        |> AltBinaryRandomAccessList.cons "e"
        |> AltBinaryRandomAccessList.cons "f"
        |> AltBinaryRandomAccessList.cons "g"
        |> AltBinaryRandomAccessList.cons "h"
        |> AltBinaryRandomAccessList.cons "i"
        |> AltBinaryRandomAccessList.cons "j"

    [<Tests>]
    let testAltBinaryRandomAccessList =

        testList "Experimental AltBinaryRandomAccessList" [ test "AltBinaryRandomAccessList.empty list should be AltBinaryRandomAccessList.empty" {
                                                                AltBinaryRandomAccessList.isEmpty AltBinaryRandomAccessList.empty
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.cons works" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.cons 1
                                                                |> AltBinaryRandomAccessList.cons 2
                                                                |> AltBinaryRandomAccessList.isEmpty
                                                                |> Expect.isFalse ""
                                                            }

                                                            test "AltBinaryRandomAccessList.uncons 1 element" {
                                                                let x, _ =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.uncons

                                                                (x = 1) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.uncons 2 elements" {
                                                                let x, _ =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.uncons

                                                                (x = 2) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.uncons 3 elements" {
                                                                let x, _ =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.cons 3
                                                                    |> AltBinaryRandomAccessList.uncons

                                                                (x = 3) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUncons 1 element" {
                                                                let x =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.tryUncons

                                                                (fst(x.Value) = 1) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUncons 2 elements" {
                                                                let x =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.tryUncons

                                                                (fst(x.Value) = 2) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUncons 3 elements" {
                                                                let x =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.cons 3
                                                                    |> AltBinaryRandomAccessList.tryUncons

                                                                (fst(x.Value) = 3) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUncons AltBinaryRandomAccessList.empty" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.tryUncons
                                                                |> Expect.isNone ""
                                                            }

                                                            test "AltBinaryRandomAccessList.head should return" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.cons 1
                                                                |> AltBinaryRandomAccessList.cons 2
                                                                |> AltBinaryRandomAccessList.head
                                                                |> Expect.equal "" 2
                                                            }

                                                            test "AltBinaryRandomAccessList.tryGetHead should return" {
                                                                let x =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.tryGetHead

                                                                x.Value |> Expect.equal "" 2
                                                            }

                                                            test
                                                                "AltBinaryRandomAccessList.tryGetHead on AltBinaryRandomAccessList.empty should return None" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.tryGetHead
                                                                |> Expect.isNone ""
                                                            }

                                                            test
                                                                "AltBinaryRandomAccessList.tryGetTail on AltBinaryRandomAccessList.empty should return None" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.tryGetTail
                                                                |> Expect.isNone ""
                                                            }

                                                            test
                                                                "AltBinaryRandomAccessList.tryGetTail on len 1 should return Some AltBinaryRandomAccessList.empty" {
                                                                let x =
                                                                    (AltBinaryRandomAccessList.empty
                                                                     |> AltBinaryRandomAccessList.cons 1
                                                                     |> AltBinaryRandomAccessList.tryGetTail)
                                                                        .Value

                                                                x |> AltBinaryRandomAccessList.isEmpty |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tail on len 2 should return" {
                                                                AltBinaryRandomAccessList.empty
                                                                |> AltBinaryRandomAccessList.cons 1
                                                                |> AltBinaryRandomAccessList.cons 2
                                                                |> AltBinaryRandomAccessList.tail
                                                                |> AltBinaryRandomAccessList.head
                                                                |> Expect.equal "" 1
                                                            }

                                                            test "AltBinaryRandomAccessList.tryGetTail on len 2 should return" {
                                                                let a =
                                                                    AltBinaryRandomAccessList.empty
                                                                    |> AltBinaryRandomAccessList.cons 1
                                                                    |> AltBinaryRandomAccessList.cons 2
                                                                    |> AltBinaryRandomAccessList.tryGetTail

                                                                ((AltBinaryRandomAccessList.head a.Value) = 1) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 1" {
                                                                len1 |> AltBinaryRandomAccessList.lookup 0 |> Expect.equal "" "a"
                                                            }

                                                            test "AltBinaryRandomAccessList.rev AltBinaryRandomAccessList.empty" {
                                                                AltBinaryRandomAccessList.isEmpty(
                                                                    AltBinaryRandomAccessList.empty |> AltBinaryRandomAccessList.rev
                                                                )
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.rev elements length 5" {
                                                                let a =
                                                                    match (len5 |> AltBinaryRandomAccessList.rev) with
                                                                    | One("a", Zero(One((("b", "c"), ("d", "e")), Nil))) -> true
                                                                    | _ -> false

                                                                a |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.append 2 AltBinaryRandomAccessList.empty lists" {
                                                                AltBinaryRandomAccessList.isEmpty(
                                                                    AltBinaryRandomAccessList.append
                                                                        AltBinaryRandomAccessList.empty
                                                                        AltBinaryRandomAccessList.empty
                                                                )
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.append left AltBinaryRandomAccessList.empty right 5" {
                                                                let a =
                                                                    match (AltBinaryRandomAccessList.append AltBinaryRandomAccessList.empty len5) with
                                                                    | One("e", Zero(One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                a |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.append left 6 right AltBinaryRandomAccessList.empty" {
                                                                let a =
                                                                    match (AltBinaryRandomAccessList.append len6 AltBinaryRandomAccessList.empty) with
                                                                    | Zero(One(("f", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                a |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.append left 6 right3" {
                                                                let a =
                                                                    match
                                                                        (AltBinaryRandomAccessList.append
                                                                            len6
                                                                            (AltBinaryRandomAccessList.empty
                                                                             |> AltBinaryRandomAccessList.cons "3"
                                                                             |> AltBinaryRandomAccessList.cons "2"
                                                                             |> AltBinaryRandomAccessList.cons "1"))
                                                                    with
                                                                    | One("f",
                                                                          Zero(Zero(One(((("e", "d"), ("c", "b")), (("a", "1"), ("2", "3"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                a |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 2" {
                                                                (((len2 |> AltBinaryRandomAccessList.lookup 0) = "b")
                                                                 && ((len2 |> AltBinaryRandomAccessList.lookup 1) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 3" {
                                                                (((len3 |> AltBinaryRandomAccessList.lookup 0) = "c")
                                                                 && ((len3 |> AltBinaryRandomAccessList.lookup 1) = "b")
                                                                 && ((len3 |> AltBinaryRandomAccessList.lookup 2) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 4" {
                                                                (((len4 |> AltBinaryRandomAccessList.lookup 0) = "d")
                                                                 && ((len4 |> AltBinaryRandomAccessList.lookup 1) = "c")
                                                                 && ((len4 |> AltBinaryRandomAccessList.lookup 2) = "b")
                                                                 && ((len4 |> AltBinaryRandomAccessList.lookup 3) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 5" {
                                                                (((len5 |> AltBinaryRandomAccessList.lookup 0) = "e")
                                                                 && ((len5 |> AltBinaryRandomAccessList.lookup 1) = "d")
                                                                 && ((len5 |> AltBinaryRandomAccessList.lookup 2) = "c")
                                                                 && ((len5 |> AltBinaryRandomAccessList.lookup 3) = "b")
                                                                 && ((len5 |> AltBinaryRandomAccessList.lookup 4) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 6" {
                                                                (((len6 |> AltBinaryRandomAccessList.lookup 0) = "f")
                                                                 && ((len6 |> AltBinaryRandomAccessList.lookup 1) = "e")
                                                                 && ((len6 |> AltBinaryRandomAccessList.lookup 2) = "d")
                                                                 && ((len6 |> AltBinaryRandomAccessList.lookup 3) = "c")
                                                                 && ((len6 |> AltBinaryRandomAccessList.lookup 4) = "b")
                                                                 && ((len6 |> AltBinaryRandomAccessList.lookup 5) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 7" {
                                                                (((len7 |> AltBinaryRandomAccessList.lookup 0) = "g")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 1) = "f")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 2) = "e")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 3) = "d")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 4) = "c")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 5) = "b")
                                                                 && ((len7 |> AltBinaryRandomAccessList.lookup 6) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 8" {
                                                                (((len8 |> AltBinaryRandomAccessList.lookup 0) = "h")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 1) = "g")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 2) = "f")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 3) = "e")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 4) = "d")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 5) = "c")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 6) = "b")
                                                                 && ((len8 |> AltBinaryRandomAccessList.lookup 7) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 9" {
                                                                (((len9 |> AltBinaryRandomAccessList.lookup 0) = "i")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 1) = "h")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 2) = "g")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 3) = "f")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 4) = "e")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 5) = "d")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 6) = "c")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 7) = "b")
                                                                 && ((len9 |> AltBinaryRandomAccessList.lookup 8) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.lookup length 10" {
                                                                (((lena |> AltBinaryRandomAccessList.lookup 0) = "j")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 1) = "i")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 2) = "h")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 3) = "g")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 4) = "f")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 5) = "e")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 6) = "d")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 7) = "c")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 8) = "b")
                                                                 && ((lena |> AltBinaryRandomAccessList.lookup 9) = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 1" {
                                                                let a = len1 |> AltBinaryRandomAccessList.tryLookup 0
                                                                (a.Value = "a") |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 2" {
                                                                let b = len2 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let a = len2 |> AltBinaryRandomAccessList.tryLookup 1
                                                                ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 3" {
                                                                let c = len3 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let b = len3 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let a = len3 |> AltBinaryRandomAccessList.tryLookup 2

                                                                ((c.Value = "c") && (b.Value = "b") && (a.Value = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 4" {
                                                                let d = len4 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let c = len4 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let b = len4 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let a = len4 |> AltBinaryRandomAccessList.tryLookup 3

                                                                ((d.Value = "d")
                                                                 && (c.Value = "c")
                                                                 && (b.Value = "b")
                                                                 && (a.Value = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 5" {
                                                                let e = len5 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let d = len5 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let c = len5 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let b = len5 |> AltBinaryRandomAccessList.tryLookup 3
                                                                let a = len5 |> AltBinaryRandomAccessList.tryLookup 4

                                                                ((e.Value = "e")
                                                                 && (d.Value = "d")
                                                                 && (c.Value = "c")
                                                                 && (b.Value = "b")
                                                                 && (a.Value = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 6" {
                                                                let f = len6 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let e = len6 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let d = len6 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let c = len6 |> AltBinaryRandomAccessList.tryLookup 3
                                                                let b = len6 |> AltBinaryRandomAccessList.tryLookup 4
                                                                let a = len6 |> AltBinaryRandomAccessList.tryLookup 5

                                                                ((f.Value = "f")
                                                                 && (e.Value = "e")
                                                                 && (d.Value = "d")
                                                                 && (c.Value = "c")
                                                                 && (b.Value = "b")
                                                                 && (a.Value = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 7" {
                                                                let g = len7 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let f = len7 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let e = len7 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let d = len7 |> AltBinaryRandomAccessList.tryLookup 3
                                                                let c = len7 |> AltBinaryRandomAccessList.tryLookup 4
                                                                let b = len7 |> AltBinaryRandomAccessList.tryLookup 5
                                                                let a = len7 |> AltBinaryRandomAccessList.tryLookup 6

                                                                ((g.Value = "g")
                                                                 && (f.Value = "f")
                                                                 && (e.Value = "e")
                                                                 && (d.Value = "d")
                                                                 && (c.Value = "c")
                                                                 && (b.Value = "b")
                                                                 && (a.Value = "a"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryLookup length 8" {
                                                                let h = len8 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let g = len8 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let f = len8 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let e = len8 |> AltBinaryRandomAccessList.tryLookup 3
                                                                let d = len8 |> AltBinaryRandomAccessList.tryLookup 4
                                                                let c = len8 |> AltBinaryRandomAccessList.tryLookup 5
                                                                let b = len8 |> AltBinaryRandomAccessList.tryLookup 6
                                                                let a = len8 |> AltBinaryRandomAccessList.tryLookup 7

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

                                                            test "AltBinaryRandomAccessList.tryLookup length 9" {
                                                                let i = len9 |> AltBinaryRandomAccessList.tryLookup 0
                                                                let h = len9 |> AltBinaryRandomAccessList.tryLookup 1
                                                                let g = len9 |> AltBinaryRandomAccessList.tryLookup 2
                                                                let f = len9 |> AltBinaryRandomAccessList.tryLookup 3
                                                                let e = len9 |> AltBinaryRandomAccessList.tryLookup 4
                                                                let d = len9 |> AltBinaryRandomAccessList.tryLookup 5
                                                                let c = len9 |> AltBinaryRandomAccessList.tryLookup 6
                                                                let b = len9 |> AltBinaryRandomAccessList.tryLookup 7
                                                                let a = len9 |> AltBinaryRandomAccessList.tryLookup 8

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

                                                            test "AltBinaryRandomAccessList.tryLookup length 10" {
                                                                let j = lena |> AltBinaryRandomAccessList.tryLookup 0
                                                                let i = lena |> AltBinaryRandomAccessList.tryLookup 1
                                                                let h = lena |> AltBinaryRandomAccessList.tryLookup 2
                                                                let g = lena |> AltBinaryRandomAccessList.tryLookup 3
                                                                let f = lena |> AltBinaryRandomAccessList.tryLookup 4
                                                                let e = lena |> AltBinaryRandomAccessList.tryLookup 5
                                                                let d = lena |> AltBinaryRandomAccessList.tryLookup 6
                                                                let c = lena |> AltBinaryRandomAccessList.tryLookup 7
                                                                let b = lena |> AltBinaryRandomAccessList.tryLookup 8
                                                                let a = lena |> AltBinaryRandomAccessList.tryLookup 9

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

                                                            test "AltBinaryRandomAccessList.tryLookup not found" {
                                                                lena |> AltBinaryRandomAccessList.tryLookup 10 |> Expect.isNone ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 1" {
                                                                len1
                                                                |> AltBinaryRandomAccessList.update 0 "aa"
                                                                |> AltBinaryRandomAccessList.lookup 0
                                                                |> Expect.equal "" "aa"
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 2" {
                                                                (((len2
                                                                   |> AltBinaryRandomAccessList.update 0 "bb"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "bb")
                                                                 && ((len2
                                                                      |> AltBinaryRandomAccessList.update 1 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 3" {
                                                                (((len3
                                                                   |> AltBinaryRandomAccessList.update 0 "cc"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "cc")
                                                                 && ((len3
                                                                      |> AltBinaryRandomAccessList.update 1 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "bb")
                                                                 && ((len3
                                                                      |> AltBinaryRandomAccessList.update 2 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 4" {
                                                                (((len4
                                                                   |> AltBinaryRandomAccessList.update 0 "dd"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "dd")
                                                                 && ((len4
                                                                      |> AltBinaryRandomAccessList.update 1 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "cc")
                                                                 && ((len4
                                                                      |> AltBinaryRandomAccessList.update 2 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "bb")
                                                                 && ((len4
                                                                      |> AltBinaryRandomAccessList.update 3 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 5" {
                                                                (((len5
                                                                   |> AltBinaryRandomAccessList.update 0 "ee"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "ee")
                                                                 && ((len5
                                                                      |> AltBinaryRandomAccessList.update 1 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "dd")
                                                                 && ((len5
                                                                      |> AltBinaryRandomAccessList.update 2 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "cc")
                                                                 && ((len5
                                                                      |> AltBinaryRandomAccessList.update 3 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "bb")
                                                                 && ((len5
                                                                      |> AltBinaryRandomAccessList.update 4 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 6" {
                                                                (((len6
                                                                   |> AltBinaryRandomAccessList.update 0 "ff"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "ff")
                                                                 && ((len6
                                                                      |> AltBinaryRandomAccessList.update 1 "ee"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "ee")
                                                                 && ((len6
                                                                      |> AltBinaryRandomAccessList.update 2 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "dd")
                                                                 && ((len6
                                                                      |> AltBinaryRandomAccessList.update 3 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "cc")
                                                                 && ((len6
                                                                      |> AltBinaryRandomAccessList.update 4 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "bb")
                                                                 && ((len6
                                                                      |> AltBinaryRandomAccessList.update 5 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 5) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 7" {
                                                                (((len7
                                                                   |> AltBinaryRandomAccessList.update 0 "gg"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "gg")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 1 "ff"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "ff")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 2 "ee"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "ee")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 3 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "dd")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 4 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "cc")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 5 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 5) = "bb")
                                                                 && ((len7
                                                                      |> AltBinaryRandomAccessList.update 6 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 6) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 8" {
                                                                (((len8
                                                                   |> AltBinaryRandomAccessList.update 0 "hh"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "hh")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 1 "gg"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "gg")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 2 "ff"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "ff")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 3 "ee"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "ee")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 4 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "dd")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 5 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 5) = "cc")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 6 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 6) = "bb")
                                                                 && ((len8
                                                                      |> AltBinaryRandomAccessList.update 7 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 7) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 9" {
                                                                (((len9
                                                                   |> AltBinaryRandomAccessList.update 0 "ii"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "ii")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 1 "hh"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "hh")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 2 "gg"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "gg")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 3 "ff"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "ff")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 4 "ee"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "ee")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 5 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 5) = "dd")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 6 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 6) = "cc")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 7 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 7) = "bb")
                                                                 && ((len9
                                                                      |> AltBinaryRandomAccessList.update 8 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 8) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.update length 10" {
                                                                (((lena
                                                                   |> AltBinaryRandomAccessList.update 0 "jj"
                                                                   |> AltBinaryRandomAccessList.lookup 0) = "jj")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 1 "ii"
                                                                      |> AltBinaryRandomAccessList.lookup 1) = "ii")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 2 "hh"
                                                                      |> AltBinaryRandomAccessList.lookup 2) = "hh")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 3 "gg"
                                                                      |> AltBinaryRandomAccessList.lookup 3) = "gg")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 4 "ff"
                                                                      |> AltBinaryRandomAccessList.lookup 4) = "ff")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 5 "ee"
                                                                      |> AltBinaryRandomAccessList.lookup 5) = "ee")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 6 "dd"
                                                                      |> AltBinaryRandomAccessList.lookup 6) = "dd")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 7 "cc"
                                                                      |> AltBinaryRandomAccessList.lookup 7) = "cc")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 8 "bb"
                                                                      |> AltBinaryRandomAccessList.lookup 8) = "bb")
                                                                 && ((lena
                                                                      |> AltBinaryRandomAccessList.update 9 "aa"
                                                                      |> AltBinaryRandomAccessList.lookup 9) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 1" {
                                                                let a = len1 |> AltBinaryRandomAccessList.tryUpdate 0 "aa"

                                                                ((a.Value |> AltBinaryRandomAccessList.lookup 0) = "aa")
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 2" {
                                                                let b = len2 |> AltBinaryRandomAccessList.tryUpdate 0 "bb"
                                                                let a = len2 |> AltBinaryRandomAccessList.tryUpdate 1 "aa"

                                                                (((b.Value |> AltBinaryRandomAccessList.lookup 0) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 1) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 3" {
                                                                let c = len3 |> AltBinaryRandomAccessList.tryUpdate 0 "cc"
                                                                let b = len3 |> AltBinaryRandomAccessList.tryUpdate 1 "bb"
                                                                let a = len3 |> AltBinaryRandomAccessList.tryUpdate 2 "aa"

                                                                (((c.Value |> AltBinaryRandomAccessList.lookup 0) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 1) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 2) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 4" {
                                                                let d = len4 |> AltBinaryRandomAccessList.tryUpdate 0 "dd"
                                                                let c = len4 |> AltBinaryRandomAccessList.tryUpdate 1 "cc"
                                                                let b = len4 |> AltBinaryRandomAccessList.tryUpdate 2 "bb"
                                                                let a = len4 |> AltBinaryRandomAccessList.tryUpdate 3 "aa"

                                                                (((d.Value |> AltBinaryRandomAccessList.lookup 0) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 1) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 2) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 3) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 5" {
                                                                let e = len5 |> AltBinaryRandomAccessList.tryUpdate 0 "ee"
                                                                let d = len5 |> AltBinaryRandomAccessList.tryUpdate 1 "dd"
                                                                let c = len5 |> AltBinaryRandomAccessList.tryUpdate 2 "cc"
                                                                let b = len5 |> AltBinaryRandomAccessList.tryUpdate 3 "bb"
                                                                let a = len5 |> AltBinaryRandomAccessList.tryUpdate 4 "aa"

                                                                (((e.Value |> AltBinaryRandomAccessList.lookup 0) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 1) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 2) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 3) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 4) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 6" {
                                                                let f = len6 |> AltBinaryRandomAccessList.tryUpdate 0 "ff"
                                                                let e = len6 |> AltBinaryRandomAccessList.tryUpdate 1 "ee"
                                                                let d = len6 |> AltBinaryRandomAccessList.tryUpdate 2 "dd"
                                                                let c = len6 |> AltBinaryRandomAccessList.tryUpdate 3 "cc"
                                                                let b = len6 |> AltBinaryRandomAccessList.tryUpdate 4 "bb"
                                                                let a = len6 |> AltBinaryRandomAccessList.tryUpdate 5 "aa"

                                                                (((f.Value |> AltBinaryRandomAccessList.lookup 0) = "ff")
                                                                 && ((e.Value |> AltBinaryRandomAccessList.lookup 1) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 2) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 3) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 4) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 5) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 7" {
                                                                let g = len7 |> AltBinaryRandomAccessList.tryUpdate 0 "gg"
                                                                let f = len7 |> AltBinaryRandomAccessList.tryUpdate 1 "ff"
                                                                let e = len7 |> AltBinaryRandomAccessList.tryUpdate 2 "ee"
                                                                let d = len7 |> AltBinaryRandomAccessList.tryUpdate 3 "dd"
                                                                let c = len7 |> AltBinaryRandomAccessList.tryUpdate 4 "cc"
                                                                let b = len7 |> AltBinaryRandomAccessList.tryUpdate 5 "bb"
                                                                let a = len7 |> AltBinaryRandomAccessList.tryUpdate 6 "aa"

                                                                (((g.Value |> AltBinaryRandomAccessList.lookup 0) = "gg")
                                                                 && ((f.Value |> AltBinaryRandomAccessList.lookup 1) = "ff")
                                                                 && ((e.Value |> AltBinaryRandomAccessList.lookup 2) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 3) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 4) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 5) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 6) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 8" {
                                                                let h = len8 |> AltBinaryRandomAccessList.tryUpdate 0 "hh"
                                                                let g = len8 |> AltBinaryRandomAccessList.tryUpdate 1 "gg"
                                                                let f = len8 |> AltBinaryRandomAccessList.tryUpdate 2 "ff"
                                                                let e = len8 |> AltBinaryRandomAccessList.tryUpdate 3 "ee"
                                                                let d = len8 |> AltBinaryRandomAccessList.tryUpdate 4 "dd"
                                                                let c = len8 |> AltBinaryRandomAccessList.tryUpdate 5 "cc"
                                                                let b = len8 |> AltBinaryRandomAccessList.tryUpdate 6 "bb"
                                                                let a = len8 |> AltBinaryRandomAccessList.tryUpdate 7 "aa"

                                                                (((h.Value |> AltBinaryRandomAccessList.lookup 0) = "hh")
                                                                 && ((g.Value |> AltBinaryRandomAccessList.lookup 1) = "gg")
                                                                 && ((f.Value |> AltBinaryRandomAccessList.lookup 2) = "ff")
                                                                 && ((e.Value |> AltBinaryRandomAccessList.lookup 3) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 4) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 5) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 6) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 7) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 9" {
                                                                let i = len9 |> AltBinaryRandomAccessList.tryUpdate 0 "ii"
                                                                let h = len9 |> AltBinaryRandomAccessList.tryUpdate 1 "hh"
                                                                let g = len9 |> AltBinaryRandomAccessList.tryUpdate 2 "gg"
                                                                let f = len9 |> AltBinaryRandomAccessList.tryUpdate 3 "ff"
                                                                let e = len9 |> AltBinaryRandomAccessList.tryUpdate 4 "ee"
                                                                let d = len9 |> AltBinaryRandomAccessList.tryUpdate 5 "dd"
                                                                let c = len9 |> AltBinaryRandomAccessList.tryUpdate 6 "cc"
                                                                let b = len9 |> AltBinaryRandomAccessList.tryUpdate 7 "bb"
                                                                let a = len9 |> AltBinaryRandomAccessList.tryUpdate 8 "aa"

                                                                (((i.Value |> AltBinaryRandomAccessList.lookup 0) = "ii")
                                                                 && ((h.Value |> AltBinaryRandomAccessList.lookup 1) = "hh")
                                                                 && ((g.Value |> AltBinaryRandomAccessList.lookup 2) = "gg")
                                                                 && ((f.Value |> AltBinaryRandomAccessList.lookup 3) = "ff")
                                                                 && ((e.Value |> AltBinaryRandomAccessList.lookup 4) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 5) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 6) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 7) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 8) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryUpdate length 10" {
                                                                let j = lena |> AltBinaryRandomAccessList.tryUpdate 0 "jj"
                                                                let i = lena |> AltBinaryRandomAccessList.tryUpdate 1 "ii"
                                                                let h = lena |> AltBinaryRandomAccessList.tryUpdate 2 "hh"
                                                                let g = lena |> AltBinaryRandomAccessList.tryUpdate 3 "gg"
                                                                let f = lena |> AltBinaryRandomAccessList.tryUpdate 4 "ff"
                                                                let e = lena |> AltBinaryRandomAccessList.tryUpdate 5 "ee"
                                                                let d = lena |> AltBinaryRandomAccessList.tryUpdate 6 "dd"
                                                                let c = lena |> AltBinaryRandomAccessList.tryUpdate 7 "cc"
                                                                let b = lena |> AltBinaryRandomAccessList.tryUpdate 8 "bb"
                                                                let a = lena |> AltBinaryRandomAccessList.tryUpdate 9 "aa"

                                                                (((j.Value |> AltBinaryRandomAccessList.lookup 0) = "jj")
                                                                 && ((i.Value |> AltBinaryRandomAccessList.lookup 1) = "ii")
                                                                 && ((h.Value |> AltBinaryRandomAccessList.lookup 2) = "hh")
                                                                 && ((g.Value |> AltBinaryRandomAccessList.lookup 3) = "gg")
                                                                 && ((f.Value |> AltBinaryRandomAccessList.lookup 4) = "ff")
                                                                 && ((e.Value |> AltBinaryRandomAccessList.lookup 5) = "ee")
                                                                 && ((d.Value |> AltBinaryRandomAccessList.lookup 6) = "dd")
                                                                 && ((c.Value |> AltBinaryRandomAccessList.lookup 7) = "cc")
                                                                 && ((b.Value |> AltBinaryRandomAccessList.lookup 8) = "bb")
                                                                 && ((a.Value |> AltBinaryRandomAccessList.lookup 9) = "aa"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 1" {
                                                                AltBinaryRandomAccessList.isEmpty(len1 |> AltBinaryRandomAccessList.remove 0)
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 2" {
                                                                let a =
                                                                    match (len2 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | One(("a"), Nil) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len2 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | One(("b"), Nil) -> true
                                                                    | _ -> false

                                                                (a && b) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 3" {
                                                                let a =
                                                                    match (len3 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | Zero(One(("b", "a"), Nil)) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len3 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | Zero(One(("c", "a"), Nil)) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len3 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | Zero(One(("c", "b"), Nil)) -> true
                                                                    | _ -> false

                                                                (a && b && c) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 4" {
                                                                let a =
                                                                    match (len4 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | One("c", One(("b", "a"), Nil)) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len4 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | One("d", One(("b", "a"), Nil)) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len4 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | One("d", One(("c", "a"), Nil)) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len4 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | One("d", One(("c", "b"), Nil)) -> true
                                                                    | _ -> false

                                                                (a && b && c && d) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 5" {
                                                                let a =
                                                                    match (len5 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | Zero(Zero(One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len5 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | Zero(Zero(One((("e", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len5 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | Zero(Zero(One((("e", "d"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len5 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | Zero(Zero(One((("e", "d"), ("c", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len5 |> AltBinaryRandomAccessList.remove 4) with
                                                                    | Zero(Zero(One((("e", "d"), ("c", "b")), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 6" {
                                                                let a =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | One("e", Zero(One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | One("f", Zero(One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | One("f", Zero(One((("e", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | One("f", Zero(One((("e", "d"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 4) with
                                                                    | One("f", Zero(One((("e", "d"), ("c", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len6 |> AltBinaryRandomAccessList.remove 5) with
                                                                    | One("f", Zero(One((("e", "d"), ("c", "b")), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 7" {
                                                                let a =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | Zero(One(("f", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | Zero(One(("g", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | Zero(One(("g", "f"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | Zero(One(("g", "f"), One((("e", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 4) with
                                                                    | Zero(One(("g", "f"), One((("e", "d"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 5) with
                                                                    | Zero(One(("g", "f"), One((("e", "d"), ("c", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len7 |> AltBinaryRandomAccessList.remove 6) with
                                                                    | Zero(One(("g", "f"), One((("e", "d"), ("c", "b")), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 8" {
                                                                let a =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | One("g", One(("f", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | One("h", One(("f", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | One("h", One(("g", "e"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | One("h", One(("g", "f"), One((("d", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 4) with
                                                                    | One("h", One(("g", "f"), One((("e", "c"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 5) with
                                                                    | One("h", One(("g", "f"), One((("e", "d"), ("b", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 6) with
                                                                    | One("h", One(("g", "f"), One((("e", "d"), ("c", "a")), Nil))) -> true
                                                                    | _ -> false

                                                                let h =
                                                                    match (len8 |> AltBinaryRandomAccessList.remove 7) with
                                                                    | One("h", One(("g", "f"), One((("e", "d"), ("c", "b")), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 9" {
                                                                let a =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 0) with
                                                                    | Zero(Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 1) with
                                                                    | Zero(Zero(Zero(One(((("i", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 2) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 3) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 4) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 5) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 6) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let h =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 7) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let i =
                                                                    match (len9 |> AltBinaryRandomAccessList.remove 8) with
                                                                    | Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "b"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h && i) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.remove elements length 10" {
                                                                let a =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 0) with
                                                                    | One("i",
                                                                          Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let b =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 1) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let c =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 2) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let d =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 3) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let e =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 4) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "e")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let f =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 5) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "f")), (("d", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let g =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 6) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "c"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let h =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 7) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("b", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let i =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 8) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "a"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                let j =
                                                                    match (lena |> AltBinaryRandomAccessList.remove 9) with
                                                                    | One("j",
                                                                          Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "b"))), Nil)))) ->
                                                                        true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h && i && j) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 1" {
                                                                let a = AltBinaryRandomAccessList.tryRemove 0 len1
                                                                (AltBinaryRandomAccessList.isEmpty a.Value) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 2" {
                                                                let a =
                                                                    match (len2 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(One(("a"), Nil)) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len2 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(One(("b"), Nil)) -> true
                                                                    | _ -> false

                                                                (a && b) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 3" {
                                                                let a =
                                                                    match (len3 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(Zero(One(("b", "a"), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len3 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(Zero(One(("c", "a"), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len3 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(Zero(One(("c", "b"), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 4" {
                                                                let a =
                                                                    match (len4 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(One("c", One(("b", "a"), Nil))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len4 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(One("d", One(("b", "a"), Nil))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len4 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(One("d", One(("c", "a"), Nil))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len4 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(One("d", One(("c", "b"), Nil))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 5" {
                                                                let a =
                                                                    match (len5 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(Zero(Zero(One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len5 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(Zero(Zero(One((("e", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len5 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(Zero(Zero(One((("e", "d"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len5 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(Zero(Zero(One((("e", "d"), ("c", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len5 |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(Zero(Zero(One((("e", "d"), ("c", "b")), Nil)))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 6" {
                                                                let a =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(One("e", Zero(One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(One("f", Zero(One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(One("f", Zero(One((("e", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(One("f", Zero(One((("e", "d"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(One("f", Zero(One((("e", "d"), ("c", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len6 |> AltBinaryRandomAccessList.tryRemove 5) with
                                                                    | Some(One("f", Zero(One((("e", "d"), ("c", "b")), Nil)))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 7" {
                                                                let a =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(Zero(One(("f", "e"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(Zero(One(("g", "e"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(Zero(One(("g", "f"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(Zero(One(("g", "f"), One((("e", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(Zero(One(("g", "f"), One((("e", "d"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 5) with
                                                                    | Some(Zero(One(("g", "f"), One((("e", "d"), ("c", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len7 |> AltBinaryRandomAccessList.tryRemove 6) with
                                                                    | Some(Zero(One(("g", "f"), One((("e", "d"), ("c", "b")), Nil)))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 8" {
                                                                let a =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(One("g", One(("f", "e"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(One("h", One(("f", "e"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(One("h", One(("g", "e"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(One("h", One(("g", "f"), One((("d", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(One("h", One(("g", "f"), One((("e", "c"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 5) with
                                                                    | Some(One("h", One(("g", "f"), One((("e", "d"), ("b", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 6) with
                                                                    | Some(One("h", One(("g", "f"), One((("e", "d"), ("c", "a")), Nil)))) -> true
                                                                    | _ -> false

                                                                let h =
                                                                    match (len8 |> AltBinaryRandomAccessList.tryRemove 7) with
                                                                    | Some(One("h", One(("g", "f"), One((("e", "d"), ("c", "b")), Nil)))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 9" {
                                                                let a =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(Zero(Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let b =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "g"), ("f", "e")), (("d", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let c =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("f", "e")), (("d", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let d =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "e")), (("d", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let e =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("d", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let f =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 5) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "c"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let g =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 6) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("b", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let h =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 7) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "a"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                let i =
                                                                    match (len9 |> AltBinaryRandomAccessList.tryRemove 8) with
                                                                    | Some(Zero(Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "b"))),
                                                                                              Nil))))) -> true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h && i) |> Expect.isTrue ""
                                                            }

                                                            test "AltBinaryRandomAccessList.tryRemove elements length 10" {
                                                                let a =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 0) with
                                                                    | Some(One("i",
                                                                               Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let b =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 1) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("h", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let c =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 2) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "g"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let d =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 3) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("f", "e")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let e =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 4) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "e")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let f =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 5) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "f")), (("d", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let g =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 6) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "c"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let h =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 7) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("b", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let i =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 8) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "a"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                let j =
                                                                    match (lena |> AltBinaryRandomAccessList.tryRemove 9) with
                                                                    | Some(One("j",
                                                                               Zero(Zero(One(((("i", "h"), ("g", "f")), (("e", "d"), ("c", "b"))), Nil))))) ->
                                                                        true
                                                                    | _ -> false

                                                                (a && b && c && d && e && f && g && h && i && j) |> Expect.isTrue ""
                                                            }

                                                            test "length of AltBinaryRandomAccessList.empty is 0" {
                                                                ((AltBinaryRandomAccessList.length AltBinaryRandomAccessList.empty) = 0)
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "length of 1 - 10 good" {
                                                                (((AltBinaryRandomAccessList.length len1) = 1)
                                                                 && ((AltBinaryRandomAccessList.length len2) = 2)
                                                                 && ((AltBinaryRandomAccessList.length len3) = 3)
                                                                 && ((AltBinaryRandomAccessList.length len4) = 4)
                                                                 && ((AltBinaryRandomAccessList.length len5) = 5)
                                                                 && ((AltBinaryRandomAccessList.length len6) = 6)
                                                                 && ((AltBinaryRandomAccessList.length len7) = 7)
                                                                 && ((AltBinaryRandomAccessList.length len8) = 8)
                                                                 && ((AltBinaryRandomAccessList.length len9) = 9)
                                                                 && ((AltBinaryRandomAccessList.length lena) = 10))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "ofSeq" {
                                                                let x =
                                                                    AltBinaryRandomAccessList.ofSeq [ "a"
                                                                                                      "b"
                                                                                                      "c"
                                                                                                      "d"
                                                                                                      "e"
                                                                                                      "f"
                                                                                                      "g"
                                                                                                      "h"
                                                                                                      "i"
                                                                                                      "j" ]

                                                                (((x |> AltBinaryRandomAccessList.lookup 0) = "a")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 1) = "b")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 2) = "c")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 3) = "d")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 4) = "e")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 5) = "f")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 6) = "g")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 7) = "h")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 8) = "i")
                                                                 && ((x |> AltBinaryRandomAccessList.lookup 9) = "j"))
                                                                |> Expect.isTrue ""
                                                            }

                                                            test "IRandomAccessList AltBinaryRandomAccessList.cons works" {
                                                                ((lena :> IRandomAccessList<string>).Cons "zz").Head
                                                                |> Expect.equal "" "zz"
                                                            } ]

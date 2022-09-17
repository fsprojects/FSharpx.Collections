namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//only going up to len5 is probably sufficient to test all edge cases
//but better too many unit tests than too few

module BinaryRandomAccessListTest =

    let len1 = BinaryRandomAccessList.empty() |> BinaryRandomAccessList.cons "a"

    let len2 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"

    let len3 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"

    let len4 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"

    let len5 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"

    let len6 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"
        |> BinaryRandomAccessList.cons "f"

    let len7 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"
        |> BinaryRandomAccessList.cons "f"
        |> BinaryRandomAccessList.cons "g"

    let len8 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"
        |> BinaryRandomAccessList.cons "f"
        |> BinaryRandomAccessList.cons "g"
        |> BinaryRandomAccessList.cons "h"

    let len9 =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"
        |> BinaryRandomAccessList.cons "f"
        |> BinaryRandomAccessList.cons "g"
        |> BinaryRandomAccessList.cons "h"
        |> BinaryRandomAccessList.cons "i"

    let lena =
        BinaryRandomAccessList.empty()
        |> BinaryRandomAccessList.cons "a"
        |> BinaryRandomAccessList.cons "b"
        |> BinaryRandomAccessList.cons "c"
        |> BinaryRandomAccessList.cons "d"
        |> BinaryRandomAccessList.cons "e"
        |> BinaryRandomAccessList.cons "f"
        |> BinaryRandomAccessList.cons "g"
        |> BinaryRandomAccessList.cons "h"
        |> BinaryRandomAccessList.cons "i"
        |> BinaryRandomAccessList.cons "j"

    [<Tests>]
    let testBinaryRandomAccessList =

        testList "Experimental BinaryRandomAccessList" [ test "empty list should be empty" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.isEmpty
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.cons works" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.cons 1
                                                             |> BinaryRandomAccessList.cons 2
                                                             |> BinaryRandomAccessList.isEmpty
                                                             |> Expect.isFalse ""
                                                         }

                                                         test "BinaryRandomAccessList.uncons 1 element" {
                                                             let x, _ =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.uncons

                                                             (x = 1) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.uncons 2 elements" {
                                                             let x, _ =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.uncons

                                                             (x = 2) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.uncons 3 elements" {
                                                             let x, _ =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.cons 3
                                                                 |> BinaryRandomAccessList.uncons

                                                             (x = 3) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUncons 1 element" {
                                                             let x =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.tryUncons

                                                             (fst(x.Value) = 1) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUncons 2 elements" {
                                                             let x =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.tryUncons

                                                             (fst(x.Value) = 2) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUncons 3 elements" {
                                                             let x =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.cons 3
                                                                 |> BinaryRandomAccessList.tryUncons

                                                             (fst(x.Value) = 3) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUncons empty" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.tryUncons
                                                             |> Expect.isNone ""
                                                         }

                                                         test "BinaryRandomAccessList.head should return" {
                                                             let x =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.head

                                                             x |> Expect.equal "" 2
                                                         }

                                                         test "BinaryRandomAccessList.tryGetHead should return" {
                                                             let x =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.tryGetHead

                                                             x.Value |> Expect.equal "" 2
                                                         }

                                                         test "BinaryRandomAccessList.tryGetHead on empty should return None" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.tryGetHead
                                                             |> Expect.isNone ""
                                                         }

                                                         test "BinaryRandomAccessList.tryGetTail on empty should return None" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.tryGetTail
                                                             |> Expect.isNone ""
                                                         }

                                                         test "BinaryRandomAccessList.tryGetTail on len 1 should return Some empty" {
                                                             let x =
                                                                 (BinaryRandomAccessList.empty()
                                                                  |> BinaryRandomAccessList.cons 1
                                                                  |> BinaryRandomAccessList.tryGetTail)
                                                                     .Value

                                                             x |> BinaryRandomAccessList.isEmpty |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tail on len 2 should return" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.cons 1
                                                             |> BinaryRandomAccessList.cons 2
                                                             |> BinaryRandomAccessList.tail
                                                             |> BinaryRandomAccessList.head
                                                             |> Expect.equal "" 1
                                                         }

                                                         test "BinaryRandomAccessList.tryGetTail on len 2 should return" {
                                                             let a =
                                                                 BinaryRandomAccessList.empty()
                                                                 |> BinaryRandomAccessList.cons 1
                                                                 |> BinaryRandomAccessList.cons 2
                                                                 |> BinaryRandomAccessList.tryGetTail

                                                             ((BinaryRandomAccessList.head a.Value) = 1) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 1" {
                                                             len1 |> BinaryRandomAccessList.lookup 0 |> Expect.equal "" "a"
                                                         }

                                                         test "BinaryRandomAccessList.rev empty" {
                                                             BinaryRandomAccessList.isEmpty(
                                                                 BinaryRandomAccessList.empty() |> BinaryRandomAccessList.rev
                                                             )
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.rev elements BinaryRandomAccessList.length 5" {
                                                             let a = BinaryRandomAccessList.ofSeq [ "a"; "b"; "c"; "d"; "e" ]

                                                             let b = BinaryRandomAccessList.rev a

                                                             let c = List.ofSeq b

                                                             c.Head |> Expect.equal "" "e"
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 2" {
                                                             (((len2 |> BinaryRandomAccessList.lookup 0) = "b")
                                                              && ((len2 |> BinaryRandomAccessList.lookup 1) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 3" {
                                                             (((len3 |> BinaryRandomAccessList.lookup 0) = "c")
                                                              && ((len3 |> BinaryRandomAccessList.lookup 1) = "b")
                                                              && ((len3 |> BinaryRandomAccessList.lookup 2) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 4" {
                                                             (((len4 |> BinaryRandomAccessList.lookup 0) = "d")
                                                              && ((len4 |> BinaryRandomAccessList.lookup 1) = "c")
                                                              && ((len4 |> BinaryRandomAccessList.lookup 2) = "b")
                                                              && ((len4 |> BinaryRandomAccessList.lookup 3) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 5" {
                                                             (((len5 |> BinaryRandomAccessList.lookup 0) = "e")
                                                              && ((len5 |> BinaryRandomAccessList.lookup 1) = "d")
                                                              && ((len5 |> BinaryRandomAccessList.lookup 2) = "c")
                                                              && ((len5 |> BinaryRandomAccessList.lookup 3) = "b")
                                                              && ((len5 |> BinaryRandomAccessList.lookup 4) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 6" {
                                                             (((len6 |> BinaryRandomAccessList.lookup 0) = "f")
                                                              && ((len6 |> BinaryRandomAccessList.lookup 1) = "e")
                                                              && ((len6 |> BinaryRandomAccessList.lookup 2) = "d")
                                                              && ((len6 |> BinaryRandomAccessList.lookup 3) = "c")
                                                              && ((len6 |> BinaryRandomAccessList.lookup 4) = "b")
                                                              && ((len6 |> BinaryRandomAccessList.lookup 5) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 7" {
                                                             (((len7 |> BinaryRandomAccessList.lookup 0) = "g")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 1) = "f")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 2) = "e")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 3) = "d")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 4) = "c")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 5) = "b")
                                                              && ((len7 |> BinaryRandomAccessList.lookup 6) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 8" {
                                                             (((len8 |> BinaryRandomAccessList.lookup 0) = "h")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 1) = "g")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 2) = "f")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 3) = "e")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 4) = "d")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 5) = "c")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 6) = "b")
                                                              && ((len8 |> BinaryRandomAccessList.lookup 7) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 9" {
                                                             (((len9 |> BinaryRandomAccessList.lookup 0) = "i")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 1) = "h")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 2) = "g")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 3) = "f")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 4) = "e")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 5) = "d")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 6) = "c")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 7) = "b")
                                                              && ((len9 |> BinaryRandomAccessList.lookup 8) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.lookup BinaryRandomAccessList.length 10" {
                                                             (((lena |> BinaryRandomAccessList.lookup 0) = "j")
                                                              && ((lena |> BinaryRandomAccessList.lookup 1) = "i")
                                                              && ((lena |> BinaryRandomAccessList.lookup 2) = "h")
                                                              && ((lena |> BinaryRandomAccessList.lookup 3) = "g")
                                                              && ((lena |> BinaryRandomAccessList.lookup 4) = "f")
                                                              && ((lena |> BinaryRandomAccessList.lookup 5) = "e")
                                                              && ((lena |> BinaryRandomAccessList.lookup 6) = "d")
                                                              && ((lena |> BinaryRandomAccessList.lookup 7) = "c")
                                                              && ((lena |> BinaryRandomAccessList.lookup 8) = "b")
                                                              && ((lena |> BinaryRandomAccessList.lookup 9) = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 1" {
                                                             let a = len1 |> BinaryRandomAccessList.tryLookup 0
                                                             (a.Value = "a") |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 2" {
                                                             let b = len2 |> BinaryRandomAccessList.tryLookup 0
                                                             let a = len2 |> BinaryRandomAccessList.tryLookup 1
                                                             ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 3" {
                                                             let c = len3 |> BinaryRandomAccessList.tryLookup 0
                                                             let b = len3 |> BinaryRandomAccessList.tryLookup 1
                                                             let a = len3 |> BinaryRandomAccessList.tryLookup 2

                                                             ((c.Value = "c") && (b.Value = "b") && (a.Value = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 4" {
                                                             let d = len4 |> BinaryRandomAccessList.tryLookup 0
                                                             let c = len4 |> BinaryRandomAccessList.tryLookup 1
                                                             let b = len4 |> BinaryRandomAccessList.tryLookup 2
                                                             let a = len4 |> BinaryRandomAccessList.tryLookup 3

                                                             ((d.Value = "d")
                                                              && (c.Value = "c")
                                                              && (b.Value = "b")
                                                              && (a.Value = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 5" {
                                                             let e = len5 |> BinaryRandomAccessList.tryLookup 0
                                                             let d = len5 |> BinaryRandomAccessList.tryLookup 1
                                                             let c = len5 |> BinaryRandomAccessList.tryLookup 2
                                                             let b = len5 |> BinaryRandomAccessList.tryLookup 3
                                                             let a = len5 |> BinaryRandomAccessList.tryLookup 4

                                                             ((e.Value = "e")
                                                              && (d.Value = "d")
                                                              && (c.Value = "c")
                                                              && (b.Value = "b")
                                                              && (a.Value = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 6" {
                                                             let f = len6 |> BinaryRandomAccessList.tryLookup 0
                                                             let e = len6 |> BinaryRandomAccessList.tryLookup 1
                                                             let d = len6 |> BinaryRandomAccessList.tryLookup 2
                                                             let c = len6 |> BinaryRandomAccessList.tryLookup 3
                                                             let b = len6 |> BinaryRandomAccessList.tryLookup 4
                                                             let a = len6 |> BinaryRandomAccessList.tryLookup 5

                                                             ((f.Value = "f")
                                                              && (e.Value = "e")
                                                              && (d.Value = "d")
                                                              && (c.Value = "c")
                                                              && (b.Value = "b")
                                                              && (a.Value = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 7" {
                                                             let g = len7 |> BinaryRandomAccessList.tryLookup 0
                                                             let f = len7 |> BinaryRandomAccessList.tryLookup 1
                                                             let e = len7 |> BinaryRandomAccessList.tryLookup 2
                                                             let d = len7 |> BinaryRandomAccessList.tryLookup 3
                                                             let c = len7 |> BinaryRandomAccessList.tryLookup 4
                                                             let b = len7 |> BinaryRandomAccessList.tryLookup 5
                                                             let a = len7 |> BinaryRandomAccessList.tryLookup 6

                                                             ((g.Value = "g")
                                                              && (f.Value = "f")
                                                              && (e.Value = "e")
                                                              && (d.Value = "d")
                                                              && (c.Value = "c")
                                                              && (b.Value = "b")
                                                              && (a.Value = "a"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 8" {
                                                             let h = len8 |> BinaryRandomAccessList.tryLookup 0
                                                             let g = len8 |> BinaryRandomAccessList.tryLookup 1
                                                             let f = len8 |> BinaryRandomAccessList.tryLookup 2
                                                             let e = len8 |> BinaryRandomAccessList.tryLookup 3
                                                             let d = len8 |> BinaryRandomAccessList.tryLookup 4
                                                             let c = len8 |> BinaryRandomAccessList.tryLookup 5
                                                             let b = len8 |> BinaryRandomAccessList.tryLookup 6
                                                             let a = len8 |> BinaryRandomAccessList.tryLookup 7

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

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 9" {
                                                             let i = len9 |> BinaryRandomAccessList.tryLookup 0
                                                             let h = len9 |> BinaryRandomAccessList.tryLookup 1
                                                             let g = len9 |> BinaryRandomAccessList.tryLookup 2
                                                             let f = len9 |> BinaryRandomAccessList.tryLookup 3
                                                             let e = len9 |> BinaryRandomAccessList.tryLookup 4
                                                             let d = len9 |> BinaryRandomAccessList.tryLookup 5
                                                             let c = len9 |> BinaryRandomAccessList.tryLookup 6
                                                             let b = len9 |> BinaryRandomAccessList.tryLookup 7
                                                             let a = len9 |> BinaryRandomAccessList.tryLookup 8

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

                                                         test "BinaryRandomAccessList.tryLookup BinaryRandomAccessList.length 10" {
                                                             let j = lena |> BinaryRandomAccessList.tryLookup 0
                                                             let i = lena |> BinaryRandomAccessList.tryLookup 1
                                                             let h = lena |> BinaryRandomAccessList.tryLookup 2
                                                             let g = lena |> BinaryRandomAccessList.tryLookup 3
                                                             let f = lena |> BinaryRandomAccessList.tryLookup 4
                                                             let e = lena |> BinaryRandomAccessList.tryLookup 5
                                                             let d = lena |> BinaryRandomAccessList.tryLookup 6
                                                             let c = lena |> BinaryRandomAccessList.tryLookup 7
                                                             let b = lena |> BinaryRandomAccessList.tryLookup 8
                                                             let a = lena |> BinaryRandomAccessList.tryLookup 9

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

                                                         test "BinaryRandomAccessList.tryLookup not found" {
                                                             lena |> BinaryRandomAccessList.tryLookup 10 |> Expect.isNone ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 1" {
                                                             len1
                                                             |> BinaryRandomAccessList.update 0 "aa"
                                                             |> BinaryRandomAccessList.lookup 0
                                                             |> Expect.equal "" "aa"
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 2" {
                                                             (((len2
                                                                |> BinaryRandomAccessList.update 0 "bb"
                                                                |> BinaryRandomAccessList.lookup 0) = "bb")
                                                              && ((len2
                                                                   |> BinaryRandomAccessList.update 1 "aa"
                                                                   |> BinaryRandomAccessList.lookup 1) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 3" {
                                                             (((len3
                                                                |> BinaryRandomAccessList.update 0 "cc"
                                                                |> BinaryRandomAccessList.lookup 0) = "cc")
                                                              && ((len3
                                                                   |> BinaryRandomAccessList.update 1 "bb"
                                                                   |> BinaryRandomAccessList.lookup 1) = "bb")
                                                              && ((len3
                                                                   |> BinaryRandomAccessList.update 2 "aa"
                                                                   |> BinaryRandomAccessList.lookup 2) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 4" {
                                                             (((len4
                                                                |> BinaryRandomAccessList.update 0 "dd"
                                                                |> BinaryRandomAccessList.lookup 0) = "dd")
                                                              && ((len4
                                                                   |> BinaryRandomAccessList.update 1 "cc"
                                                                   |> BinaryRandomAccessList.lookup 1) = "cc")
                                                              && ((len4
                                                                   |> BinaryRandomAccessList.update 2 "bb"
                                                                   |> BinaryRandomAccessList.lookup 2) = "bb")
                                                              && ((len4
                                                                   |> BinaryRandomAccessList.update 3 "aa"
                                                                   |> BinaryRandomAccessList.lookup 3) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 5" {
                                                             (((len5
                                                                |> BinaryRandomAccessList.update 0 "ee"
                                                                |> BinaryRandomAccessList.lookup 0) = "ee")
                                                              && ((len5
                                                                   |> BinaryRandomAccessList.update 1 "dd"
                                                                   |> BinaryRandomAccessList.lookup 1) = "dd")
                                                              && ((len5
                                                                   |> BinaryRandomAccessList.update 2 "cc"
                                                                   |> BinaryRandomAccessList.lookup 2) = "cc")
                                                              && ((len5
                                                                   |> BinaryRandomAccessList.update 3 "bb"
                                                                   |> BinaryRandomAccessList.lookup 3) = "bb")
                                                              && ((len5
                                                                   |> BinaryRandomAccessList.update 4 "aa"
                                                                   |> BinaryRandomAccessList.lookup 4) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 6" {
                                                             (((len6
                                                                |> BinaryRandomAccessList.update 0 "ff"
                                                                |> BinaryRandomAccessList.lookup 0) = "ff")
                                                              && ((len6
                                                                   |> BinaryRandomAccessList.update 1 "ee"
                                                                   |> BinaryRandomAccessList.lookup 1) = "ee")
                                                              && ((len6
                                                                   |> BinaryRandomAccessList.update 2 "dd"
                                                                   |> BinaryRandomAccessList.lookup 2) = "dd")
                                                              && ((len6
                                                                   |> BinaryRandomAccessList.update 3 "cc"
                                                                   |> BinaryRandomAccessList.lookup 3) = "cc")
                                                              && ((len6
                                                                   |> BinaryRandomAccessList.update 4 "bb"
                                                                   |> BinaryRandomAccessList.lookup 4) = "bb")
                                                              && ((len6
                                                                   |> BinaryRandomAccessList.update 5 "aa"
                                                                   |> BinaryRandomAccessList.lookup 5) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 7" {
                                                             (((len7
                                                                |> BinaryRandomAccessList.update 0 "gg"
                                                                |> BinaryRandomAccessList.lookup 0) = "gg")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 1 "ff"
                                                                   |> BinaryRandomAccessList.lookup 1) = "ff")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 2 "ee"
                                                                   |> BinaryRandomAccessList.lookup 2) = "ee")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 3 "dd"
                                                                   |> BinaryRandomAccessList.lookup 3) = "dd")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 4 "cc"
                                                                   |> BinaryRandomAccessList.lookup 4) = "cc")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 5 "bb"
                                                                   |> BinaryRandomAccessList.lookup 5) = "bb")
                                                              && ((len7
                                                                   |> BinaryRandomAccessList.update 6 "aa"
                                                                   |> BinaryRandomAccessList.lookup 6) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 8" {
                                                             (((len8
                                                                |> BinaryRandomAccessList.update 0 "hh"
                                                                |> BinaryRandomAccessList.lookup 0) = "hh")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 1 "gg"
                                                                   |> BinaryRandomAccessList.lookup 1) = "gg")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 2 "ff"
                                                                   |> BinaryRandomAccessList.lookup 2) = "ff")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 3 "ee"
                                                                   |> BinaryRandomAccessList.lookup 3) = "ee")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 4 "dd"
                                                                   |> BinaryRandomAccessList.lookup 4) = "dd")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 5 "cc"
                                                                   |> BinaryRandomAccessList.lookup 5) = "cc")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 6 "bb"
                                                                   |> BinaryRandomAccessList.lookup 6) = "bb")
                                                              && ((len8
                                                                   |> BinaryRandomAccessList.update 7 "aa"
                                                                   |> BinaryRandomAccessList.lookup 7) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 9" {
                                                             (((len9
                                                                |> BinaryRandomAccessList.update 0 "ii"
                                                                |> BinaryRandomAccessList.lookup 0) = "ii")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 1 "hh"
                                                                   |> BinaryRandomAccessList.lookup 1) = "hh")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 2 "gg"
                                                                   |> BinaryRandomAccessList.lookup 2) = "gg")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 3 "ff"
                                                                   |> BinaryRandomAccessList.lookup 3) = "ff")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 4 "ee"
                                                                   |> BinaryRandomAccessList.lookup 4) = "ee")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 5 "dd"
                                                                   |> BinaryRandomAccessList.lookup 5) = "dd")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 6 "cc"
                                                                   |> BinaryRandomAccessList.lookup 6) = "cc")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 7 "bb"
                                                                   |> BinaryRandomAccessList.lookup 7) = "bb")
                                                              && ((len9
                                                                   |> BinaryRandomAccessList.update 8 "aa"
                                                                   |> BinaryRandomAccessList.lookup 8) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.update BinaryRandomAccessList.length 10" {
                                                             (((lena
                                                                |> BinaryRandomAccessList.update 0 "jj"
                                                                |> BinaryRandomAccessList.lookup 0) = "jj")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 1 "ii"
                                                                   |> BinaryRandomAccessList.lookup 1) = "ii")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 2 "hh"
                                                                   |> BinaryRandomAccessList.lookup 2) = "hh")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 3 "gg"
                                                                   |> BinaryRandomAccessList.lookup 3) = "gg")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 4 "ff"
                                                                   |> BinaryRandomAccessList.lookup 4) = "ff")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 5 "ee"
                                                                   |> BinaryRandomAccessList.lookup 5) = "ee")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 6 "dd"
                                                                   |> BinaryRandomAccessList.lookup 6) = "dd")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 7 "cc"
                                                                   |> BinaryRandomAccessList.lookup 7) = "cc")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 8 "bb"
                                                                   |> BinaryRandomAccessList.lookup 8) = "bb")
                                                              && ((lena
                                                                   |> BinaryRandomAccessList.update 9 "aa"
                                                                   |> BinaryRandomAccessList.lookup 9) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 1" {
                                                             let a = len1 |> BinaryRandomAccessList.tryUpdate 0 "aa"

                                                             ((a.Value |> BinaryRandomAccessList.lookup 0) = "aa")
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 2" {
                                                             let b = len2 |> BinaryRandomAccessList.tryUpdate 0 "bb"
                                                             let a = len2 |> BinaryRandomAccessList.tryUpdate 1 "aa"

                                                             (((b.Value |> BinaryRandomAccessList.lookup 0) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 1) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 3" {
                                                             let c = len3 |> BinaryRandomAccessList.tryUpdate 0 "cc"
                                                             let b = len3 |> BinaryRandomAccessList.tryUpdate 1 "bb"
                                                             let a = len3 |> BinaryRandomAccessList.tryUpdate 2 "aa"

                                                             (((c.Value |> BinaryRandomAccessList.lookup 0) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 1) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 2) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 4" {
                                                             let d = len4 |> BinaryRandomAccessList.tryUpdate 0 "dd"
                                                             let c = len4 |> BinaryRandomAccessList.tryUpdate 1 "cc"
                                                             let b = len4 |> BinaryRandomAccessList.tryUpdate 2 "bb"
                                                             let a = len4 |> BinaryRandomAccessList.tryUpdate 3 "aa"

                                                             (((d.Value |> BinaryRandomAccessList.lookup 0) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 1) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 2) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 3) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 5" {
                                                             let e = len5 |> BinaryRandomAccessList.tryUpdate 0 "ee"
                                                             let d = len5 |> BinaryRandomAccessList.tryUpdate 1 "dd"
                                                             let c = len5 |> BinaryRandomAccessList.tryUpdate 2 "cc"
                                                             let b = len5 |> BinaryRandomAccessList.tryUpdate 3 "bb"
                                                             let a = len5 |> BinaryRandomAccessList.tryUpdate 4 "aa"

                                                             (((e.Value |> BinaryRandomAccessList.lookup 0) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 1) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 2) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 3) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 4) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 6" {
                                                             let f = len6 |> BinaryRandomAccessList.tryUpdate 0 "ff"
                                                             let e = len6 |> BinaryRandomAccessList.tryUpdate 1 "ee"
                                                             let d = len6 |> BinaryRandomAccessList.tryUpdate 2 "dd"
                                                             let c = len6 |> BinaryRandomAccessList.tryUpdate 3 "cc"
                                                             let b = len6 |> BinaryRandomAccessList.tryUpdate 4 "bb"
                                                             let a = len6 |> BinaryRandomAccessList.tryUpdate 5 "aa"

                                                             (((f.Value |> BinaryRandomAccessList.lookup 0) = "ff")
                                                              && ((e.Value |> BinaryRandomAccessList.lookup 1) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 2) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 3) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 4) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 5) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 7" {
                                                             let g = len7 |> BinaryRandomAccessList.tryUpdate 0 "gg"
                                                             let f = len7 |> BinaryRandomAccessList.tryUpdate 1 "ff"
                                                             let e = len7 |> BinaryRandomAccessList.tryUpdate 2 "ee"
                                                             let d = len7 |> BinaryRandomAccessList.tryUpdate 3 "dd"
                                                             let c = len7 |> BinaryRandomAccessList.tryUpdate 4 "cc"
                                                             let b = len7 |> BinaryRandomAccessList.tryUpdate 5 "bb"
                                                             let a = len7 |> BinaryRandomAccessList.tryUpdate 6 "aa"

                                                             (((g.Value |> BinaryRandomAccessList.lookup 0) = "gg")
                                                              && ((f.Value |> BinaryRandomAccessList.lookup 1) = "ff")
                                                              && ((e.Value |> BinaryRandomAccessList.lookup 2) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 3) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 4) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 5) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 6) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 8" {
                                                             let h = len8 |> BinaryRandomAccessList.tryUpdate 0 "hh"
                                                             let g = len8 |> BinaryRandomAccessList.tryUpdate 1 "gg"
                                                             let f = len8 |> BinaryRandomAccessList.tryUpdate 2 "ff"
                                                             let e = len8 |> BinaryRandomAccessList.tryUpdate 3 "ee"
                                                             let d = len8 |> BinaryRandomAccessList.tryUpdate 4 "dd"
                                                             let c = len8 |> BinaryRandomAccessList.tryUpdate 5 "cc"
                                                             let b = len8 |> BinaryRandomAccessList.tryUpdate 6 "bb"
                                                             let a = len8 |> BinaryRandomAccessList.tryUpdate 7 "aa"

                                                             (((h.Value |> BinaryRandomAccessList.lookup 0) = "hh")
                                                              && ((g.Value |> BinaryRandomAccessList.lookup 1) = "gg")
                                                              && ((f.Value |> BinaryRandomAccessList.lookup 2) = "ff")
                                                              && ((e.Value |> BinaryRandomAccessList.lookup 3) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 4) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 5) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 6) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 7) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 9" {
                                                             let i = len9 |> BinaryRandomAccessList.tryUpdate 0 "ii"
                                                             let h = len9 |> BinaryRandomAccessList.tryUpdate 1 "hh"
                                                             let g = len9 |> BinaryRandomAccessList.tryUpdate 2 "gg"
                                                             let f = len9 |> BinaryRandomAccessList.tryUpdate 3 "ff"
                                                             let e = len9 |> BinaryRandomAccessList.tryUpdate 4 "ee"
                                                             let d = len9 |> BinaryRandomAccessList.tryUpdate 5 "dd"
                                                             let c = len9 |> BinaryRandomAccessList.tryUpdate 6 "cc"
                                                             let b = len9 |> BinaryRandomAccessList.tryUpdate 7 "bb"
                                                             let a = len9 |> BinaryRandomAccessList.tryUpdate 8 "aa"

                                                             (((i.Value |> BinaryRandomAccessList.lookup 0) = "ii")
                                                              && ((h.Value |> BinaryRandomAccessList.lookup 1) = "hh")
                                                              && ((g.Value |> BinaryRandomAccessList.lookup 2) = "gg")
                                                              && ((f.Value |> BinaryRandomAccessList.lookup 3) = "ff")
                                                              && ((e.Value |> BinaryRandomAccessList.lookup 4) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 5) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 6) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 7) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 8) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.tryUpdate BinaryRandomAccessList.length 10" {
                                                             let j = lena |> BinaryRandomAccessList.tryUpdate 0 "jj"
                                                             let i = lena |> BinaryRandomAccessList.tryUpdate 1 "ii"
                                                             let h = lena |> BinaryRandomAccessList.tryUpdate 2 "hh"
                                                             let g = lena |> BinaryRandomAccessList.tryUpdate 3 "gg"
                                                             let f = lena |> BinaryRandomAccessList.tryUpdate 4 "ff"
                                                             let e = lena |> BinaryRandomAccessList.tryUpdate 5 "ee"
                                                             let d = lena |> BinaryRandomAccessList.tryUpdate 6 "dd"
                                                             let c = lena |> BinaryRandomAccessList.tryUpdate 7 "cc"
                                                             let b = lena |> BinaryRandomAccessList.tryUpdate 8 "bb"
                                                             let a = lena |> BinaryRandomAccessList.tryUpdate 9 "aa"

                                                             (((j.Value |> BinaryRandomAccessList.lookup 0) = "jj")
                                                              && ((i.Value |> BinaryRandomAccessList.lookup 1) = "ii")
                                                              && ((h.Value |> BinaryRandomAccessList.lookup 2) = "hh")
                                                              && ((g.Value |> BinaryRandomAccessList.lookup 3) = "gg")
                                                              && ((f.Value |> BinaryRandomAccessList.lookup 4) = "ff")
                                                              && ((e.Value |> BinaryRandomAccessList.lookup 5) = "ee")
                                                              && ((d.Value |> BinaryRandomAccessList.lookup 6) = "dd")
                                                              && ((c.Value |> BinaryRandomAccessList.lookup 7) = "cc")
                                                              && ((b.Value |> BinaryRandomAccessList.lookup 8) = "bb")
                                                              && ((a.Value |> BinaryRandomAccessList.lookup 9) = "aa"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.length of empty is 0" {
                                                             BinaryRandomAccessList.empty()
                                                             |> BinaryRandomAccessList.length
                                                             |> Expect.equal "" 0
                                                         }

                                                         test "BinaryRandomAccessList.length of 1 - 10 good" {

                                                             (((BinaryRandomAccessList.length len1) = 1)
                                                              && ((BinaryRandomAccessList.length len2) = 2)
                                                              && ((BinaryRandomAccessList.length len3) = 3)
                                                              && ((BinaryRandomAccessList.length len4) = 4)
                                                              && ((BinaryRandomAccessList.length len5) = 5)
                                                              && ((BinaryRandomAccessList.length len6) = 6)
                                                              && ((BinaryRandomAccessList.length len7) = 7)
                                                              && ((BinaryRandomAccessList.length len8) = 8)
                                                              && ((BinaryRandomAccessList.length len9) = 9)
                                                              && ((BinaryRandomAccessList.length lena) = 10))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "BinaryRandomAccessList.ofSeq" {
                                                             let x =
                                                                 BinaryRandomAccessList.ofSeq [ "a"; "b"; "c"; "d"; "e"; "f"; "g"; "h"; "i"; "j" ]

                                                             (((x |> BinaryRandomAccessList.lookup 0) = "a")
                                                              && ((x |> BinaryRandomAccessList.lookup 1) = "b")
                                                              && ((x |> BinaryRandomAccessList.lookup 2) = "c")
                                                              && ((x |> BinaryRandomAccessList.lookup 3) = "d")
                                                              && ((x |> BinaryRandomAccessList.lookup 4) = "e")
                                                              && ((x |> BinaryRandomAccessList.lookup 5) = "f")
                                                              && ((x |> BinaryRandomAccessList.lookup 6) = "g")
                                                              && ((x |> BinaryRandomAccessList.lookup 7) = "h")
                                                              && ((x |> BinaryRandomAccessList.lookup 8) = "i")
                                                              && ((x |> BinaryRandomAccessList.lookup 9) = "j"))
                                                             |> Expect.isTrue ""
                                                         }

                                                         test "IRandomAccessList BinaryRandomAccessList.cons works" {
                                                             ((lena :> IRandomAccessList<string>).Cons "zz").Head
                                                             |> Expect.equal "" "zz"
                                                         } ]

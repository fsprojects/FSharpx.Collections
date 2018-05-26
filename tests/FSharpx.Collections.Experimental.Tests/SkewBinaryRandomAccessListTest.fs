namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//only going up to len5 is probably sufficient to test all edge cases
//but better too many unit tests than too few

module SkewBinaryRandomAccessListTest =

    [<Tests>]
    let testSkewBinaryRandomAccessList =

        testList "Experimental SkewBinaryRandomAccessList" [
            test "empty list should be empty" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.isEmpty |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.cons works" {
                SkewBinaryRandomAccessList.empty()|> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.isEmpty |> Expect.isFalse "" }

            test "SkewBinaryRandomAccessList.uncons 1 element" {
                let x, _ = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |>  SkewBinaryRandomAccessList.uncons
                (x = 1) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.uncons 2 elements" {
                let x, _ = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.uncons 
                (x = 2) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.uncons 3 elements" {
                let x, _ = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.cons 3 |> SkewBinaryRandomAccessList.uncons 
                (x = 3) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUncons 1 element" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.tryUncons
                (fst(x.Value) = 1) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUncons 2 elements" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.tryUncons
                (fst(x.Value) = 2) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUncons 3 elements" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.cons 3 |> SkewBinaryRandomAccessList.tryUncons 
                (fst(x.Value) = 3) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUncons empty" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.tryUncons |> Expect.isNone "" }
    
            test "SkewBinaryRandomAccessList.head should return" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.head 
                x |> Expect.equal "" 2 } 

            test "SkewBinaryRandomAccessList.tryGetHead should return" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.tryGetHead 
                x.Value |> Expect.equal "" 2 } 

            test "SkewBinaryRandomAccessList.tryGetHead on empty should return None" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.tryGetHead |> Expect.isNone "" }

            test "SkewBinaryRandomAccessList.tryGetTail on empty should return None" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.tryGetTail |> Expect.isNone "" }

            test "SkewBinaryRandomAccessList.tryGetTail on len 1 should return Some empty" {
                let x = (SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |> SkewBinaryRandomAccessList.tryGetTail).Value
                x |> SkewBinaryRandomAccessList.isEmpty |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tail on len 2 should return" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |>  SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.tail |> SkewBinaryRandomAccessList.head |> Expect.equal "" 1 } 

            test "SkewBinaryRandomAccessList.tryGetTail on len 2 should return" {
                let a = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons 1 |>  SkewBinaryRandomAccessList.cons 2 |> SkewBinaryRandomAccessList.tryGetTail 
                ((SkewBinaryRandomAccessList.head a.Value) = 1) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 1" {
                let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" 
            //    let x = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" 
                let x' = x |> SkewBinaryRandomAccessList.lookup 0 
                x' |> Expect.equal "" "a" } 

            test "SkewBinaryRandomAccessList.rev empty" {
                SkewBinaryRandomAccessList.isEmpty (SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.rev) |> Expect.isTrue "" }
    
            test "SkewBinaryRandomAccessList.rev elements length 5" {
                let a = SkewBinaryRandomAccessList.ofSeq ["a";"b";"c";"d";"e"]

                let b = SkewBinaryRandomAccessList.rev a

                let c = List.ofSeq b

                c.Head |> Expect.equal "" "e" } 

            test "SkewBinaryRandomAccessList.lookup length 2" {
                (((SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.lookup 0) = "b") && ((SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.lookup 1) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 3" {
                let len3 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c"
                (((len3 |> SkewBinaryRandomAccessList.lookup 0) = "c") 
                && ((len3 |> SkewBinaryRandomAccessList.lookup 1) = "b") 
                && ((len3 |> SkewBinaryRandomAccessList.lookup 2) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 4" {
                let len4 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d"
                (((len4 |> SkewBinaryRandomAccessList.lookup 0) = "d") && ((len4 |> SkewBinaryRandomAccessList.lookup 1) = "c") && ((len4 |> SkewBinaryRandomAccessList.lookup 2) = "b") && ((len4 |> SkewBinaryRandomAccessList.lookup 3) = "a")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 5" {
                let len5 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e"
                (((len5 |> SkewBinaryRandomAccessList.lookup 0) = "e") && ((len5 |> SkewBinaryRandomAccessList.lookup 1) = "d") && ((len5 |> SkewBinaryRandomAccessList.lookup 2) = "c") && ((len5 |> SkewBinaryRandomAccessList.lookup 3) = "b") 
                && ((len5 |> SkewBinaryRandomAccessList.lookup 4) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 6" {
                let len6 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f"
                (((len6 |> SkewBinaryRandomAccessList.lookup 0) = "f") && ((len6 |> SkewBinaryRandomAccessList.lookup 1) = "e") && ((len6 |> SkewBinaryRandomAccessList.lookup 2) = "d") && ((len6 |> SkewBinaryRandomAccessList.lookup 3) = "c") 
                && ((len6 |> SkewBinaryRandomAccessList.lookup 4) = "b") && ((len6 |> SkewBinaryRandomAccessList.lookup 5) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 7" {
                let len7 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g"
                (((len7 |> SkewBinaryRandomAccessList.lookup 0) = "g") && ((len7 |> SkewBinaryRandomAccessList.lookup 1) = "f") && ((len7 |> SkewBinaryRandomAccessList.lookup 2) = "e") && ((len7 |> SkewBinaryRandomAccessList.lookup 3) = "d") 
                && ((len7 |> SkewBinaryRandomAccessList.lookup 4) = "c") && ((len7 |> SkewBinaryRandomAccessList.lookup 5) = "b") && ((len7 |> SkewBinaryRandomAccessList.lookup 6) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 8" {
                let len8 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h"
                (((len8 |> SkewBinaryRandomAccessList.lookup 0) = "h") && ((len8 |> SkewBinaryRandomAccessList.lookup 1) = "g") && ((len8 |> SkewBinaryRandomAccessList.lookup 2) = "f") && ((len8 |> SkewBinaryRandomAccessList.lookup 3) = "e") 
                && ((len8 |> SkewBinaryRandomAccessList.lookup 4) = "d") && ((len8 |> SkewBinaryRandomAccessList.lookup 5) = "c") && ((len8 |> SkewBinaryRandomAccessList.lookup 6) = "b") && ((len8 |> SkewBinaryRandomAccessList.lookup 7) = "a")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 9" {
                let len9 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i"
                (((len9 |> SkewBinaryRandomAccessList.lookup 0) = "i") && ((len9 |> SkewBinaryRandomAccessList.lookup 1) = "h") && ((len9 |> SkewBinaryRandomAccessList.lookup 2) = "g") && ((len9 |> SkewBinaryRandomAccessList.lookup 3) = "f") 
                && ((len9 |> SkewBinaryRandomAccessList.lookup 4) = "e") && ((len9 |> SkewBinaryRandomAccessList.lookup 5) = "d") && ((len9 |> SkewBinaryRandomAccessList.lookup 6) = "c") && ((len9 |> SkewBinaryRandomAccessList.lookup 7) = "b")
                && ((len9 |> SkewBinaryRandomAccessList.lookup 8) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.lookup length 10" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                (((lena |> SkewBinaryRandomAccessList.lookup 0) = "j") && ((lena |> SkewBinaryRandomAccessList.lookup 1) = "i") && ((lena |> SkewBinaryRandomAccessList.lookup 2) = "h") && ((lena |> SkewBinaryRandomAccessList.lookup 3) = "g") 
                && ((lena |> SkewBinaryRandomAccessList.lookup 4) = "f") && ((lena |> SkewBinaryRandomAccessList.lookup 5) = "e") && ((lena |> SkewBinaryRandomAccessList.lookup 6) = "d") && ((lena |> SkewBinaryRandomAccessList.lookup 7) = "c")
                && ((lena |> SkewBinaryRandomAccessList.lookup 8) = "b") && ((lena |> SkewBinaryRandomAccessList.lookup 9) = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 1" {
                let a = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.tryLookup 0 
                (a.Value = "a") |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 2" {
                let len2 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b"
                let b = len2 |> SkewBinaryRandomAccessList.tryLookup 0
                let a = len2 |> SkewBinaryRandomAccessList.tryLookup 1
                ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 3" {
                let len3 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c"
                let c = len3 |> SkewBinaryRandomAccessList.tryLookup 0
                let b = len3 |> SkewBinaryRandomAccessList.tryLookup 1
                let a = len3 |> SkewBinaryRandomAccessList.tryLookup 2
                ((c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 4" {
                let len4 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d"
                let d = len4 |> SkewBinaryRandomAccessList.tryLookup 0
                let c = len4 |> SkewBinaryRandomAccessList.tryLookup 1
                let b = len4 |> SkewBinaryRandomAccessList.tryLookup 2
                let a = len4 |> SkewBinaryRandomAccessList.tryLookup 3
                ((d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" } 

            test "SkewBinaryRandomAccessList.tryLookup length 5" {
                let len5 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e"
                let e = len5 |> SkewBinaryRandomAccessList.tryLookup 0
                let d = len5 |> SkewBinaryRandomAccessList.tryLookup 1
                let c = len5 |> SkewBinaryRandomAccessList.tryLookup 2
                let b = len5 |> SkewBinaryRandomAccessList.tryLookup 3
                let a = len5 |> SkewBinaryRandomAccessList.tryLookup 4
                ((e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 6" {
                let len6 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f"
                let f = len6 |> SkewBinaryRandomAccessList.tryLookup 0
                let e = len6 |> SkewBinaryRandomAccessList.tryLookup 1
                let d = len6 |> SkewBinaryRandomAccessList.tryLookup 2
                let c = len6 |> SkewBinaryRandomAccessList.tryLookup 3
                let b = len6 |> SkewBinaryRandomAccessList.tryLookup 4
                let a = len6 |> SkewBinaryRandomAccessList.tryLookup 5
                ((f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 7" {
                let len7 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g"
                let g = len7 |> SkewBinaryRandomAccessList.tryLookup 0
                let f = len7 |> SkewBinaryRandomAccessList.tryLookup 1
                let e = len7 |> SkewBinaryRandomAccessList.tryLookup 2
                let d = len7 |> SkewBinaryRandomAccessList.tryLookup 3
                let c = len7 |> SkewBinaryRandomAccessList.tryLookup 4
                let b = len7 |> SkewBinaryRandomAccessList.tryLookup 5
                let a = len7 |> SkewBinaryRandomAccessList.tryLookup 6
                ((g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") 
                && (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 8" {
                let len8 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h"
                let h = len8 |> SkewBinaryRandomAccessList.tryLookup 0
                let g = len8 |> SkewBinaryRandomAccessList.tryLookup 1
                let f = len8 |> SkewBinaryRandomAccessList.tryLookup 2
                let e = len8 |> SkewBinaryRandomAccessList.tryLookup 3
                let d = len8 |> SkewBinaryRandomAccessList.tryLookup 4
                let c = len8 |> SkewBinaryRandomAccessList.tryLookup 5
                let b = len8 |> SkewBinaryRandomAccessList.tryLookup 6
                let a = len8 |> SkewBinaryRandomAccessList.tryLookup 7
                ((h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c")  
                && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 9" {
                let len9 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i"
                let i = len9 |> SkewBinaryRandomAccessList.tryLookup 0
                let h = len9 |> SkewBinaryRandomAccessList.tryLookup 1
                let g = len9 |> SkewBinaryRandomAccessList.tryLookup 2
                let f = len9 |> SkewBinaryRandomAccessList.tryLookup 3
                let e = len9 |> SkewBinaryRandomAccessList.tryLookup 4
                let d = len9 |> SkewBinaryRandomAccessList.tryLookup 5
                let c = len9 |> SkewBinaryRandomAccessList.tryLookup 6
                let b = len9 |> SkewBinaryRandomAccessList.tryLookup 7
                let a = len9 |> SkewBinaryRandomAccessList.tryLookup 8
                ((i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") 
                && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup length 10" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                let j = lena |> SkewBinaryRandomAccessList.tryLookup 0
                let i = lena |> SkewBinaryRandomAccessList.tryLookup 1
                let h = lena |> SkewBinaryRandomAccessList.tryLookup 2
                let g = lena |> SkewBinaryRandomAccessList.tryLookup 3
                let f = lena |> SkewBinaryRandomAccessList.tryLookup 4
                let e = lena |> SkewBinaryRandomAccessList.tryLookup 5
                let d = lena |> SkewBinaryRandomAccessList.tryLookup 6
                let c = lena |> SkewBinaryRandomAccessList.tryLookup 7
                let b = lena |> SkewBinaryRandomAccessList.tryLookup 8
                let a = lena |> SkewBinaryRandomAccessList.tryLookup 9
                ((j.Value = "j") && (i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") 
                && (d.Value = "d") && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryLookup not found" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                lena |> SkewBinaryRandomAccessList.tryLookup 10 |> Expect.isNone "" }

            test "SkewBinaryRandomAccessList.update length 1" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.update 0 "aa"|> SkewBinaryRandomAccessList.lookup 0 |> Expect.equal "" "aa" } 

            test "SkewBinaryRandomAccessList.update length 2" {
                let len2 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b"
                (((len2 |> SkewBinaryRandomAccessList.update 0 "bb"|> SkewBinaryRandomAccessList.lookup 0) = "bb") && ((len2 |> SkewBinaryRandomAccessList.update 1 "aa"|> SkewBinaryRandomAccessList.lookup 1) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 3" {
                let len3 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c"
                (((len3 |> SkewBinaryRandomAccessList.update 0 "cc"|> SkewBinaryRandomAccessList.lookup 0) = "cc") && ((len3 |> SkewBinaryRandomAccessList.update 1 "bb"|> SkewBinaryRandomAccessList.lookup 1) = "bb") 
                && ((len3 |> SkewBinaryRandomAccessList.update 2 "aa"|> SkewBinaryRandomAccessList.lookup 2) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 4" {
                let len4 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d"
                (((len4 |> SkewBinaryRandomAccessList.update 0 "dd"|> SkewBinaryRandomAccessList.lookup 0) = "dd") && ((len4 |> SkewBinaryRandomAccessList.update 1 "cc"|> SkewBinaryRandomAccessList.lookup 1) = "cc") 
                && ((len4 |> SkewBinaryRandomAccessList.update 2 "bb"|> SkewBinaryRandomAccessList.lookup 2) = "bb") && ((len4 |> SkewBinaryRandomAccessList.update 3 "aa"|> SkewBinaryRandomAccessList.lookup 3) = "aa")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 5" {
                let len5 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e"
                (((len5 |> SkewBinaryRandomAccessList.update 0 "ee"|> SkewBinaryRandomAccessList.lookup 0) = "ee") && ((len5 |> SkewBinaryRandomAccessList.update 1 "dd"|> SkewBinaryRandomAccessList.lookup 1) = "dd") 
                && ((len5 |> SkewBinaryRandomAccessList.update 2 "cc"|> SkewBinaryRandomAccessList.lookup 2) = "cc") && ((len5 |> SkewBinaryRandomAccessList.update 3 "bb"|> SkewBinaryRandomAccessList.lookup 3) = "bb") 
                && ((len5 |> SkewBinaryRandomAccessList.update 4 "aa"|> SkewBinaryRandomAccessList.lookup 4) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 6" {
                let len6 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f"
                (((len6 |> SkewBinaryRandomAccessList.update 0 "ff"|> SkewBinaryRandomAccessList.lookup 0) = "ff") && ((len6 |> SkewBinaryRandomAccessList.update 1 "ee"|> SkewBinaryRandomAccessList.lookup 1) = "ee") 
                && ((len6 |> SkewBinaryRandomAccessList.update 2 "dd"|> SkewBinaryRandomAccessList.lookup 2) = "dd") && ((len6 |> SkewBinaryRandomAccessList.update 3 "cc"|> SkewBinaryRandomAccessList.lookup 3) = "cc") 
                && ((len6 |> SkewBinaryRandomAccessList.update 4 "bb"|> SkewBinaryRandomAccessList.lookup 4) = "bb") && ((len6 |> SkewBinaryRandomAccessList.update 5 "aa"|> SkewBinaryRandomAccessList.lookup 5) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 7" {
                let len7 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g"
                (((len7 |> SkewBinaryRandomAccessList.update 0 "gg"|> SkewBinaryRandomAccessList.lookup 0) = "gg") && ((len7 |> SkewBinaryRandomAccessList.update 1 "ff"|> SkewBinaryRandomAccessList.lookup 1) = "ff") 
                && ((len7 |> SkewBinaryRandomAccessList.update 2 "ee"|> SkewBinaryRandomAccessList.lookup 2) = "ee") && ((len7 |> SkewBinaryRandomAccessList.update 3 "dd"|> SkewBinaryRandomAccessList.lookup 3) = "dd") 
                && ((len7 |> SkewBinaryRandomAccessList.update 4 "cc"|> SkewBinaryRandomAccessList.lookup 4) = "cc") && ((len7 |> SkewBinaryRandomAccessList.update 5 "bb"|> SkewBinaryRandomAccessList.lookup 5) = "bb") 
                && ((len7 |> SkewBinaryRandomAccessList.update 6 "aa"|> SkewBinaryRandomAccessList.lookup 6) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 8" {
                let len8 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h"
                (((len8 |> SkewBinaryRandomAccessList.update 0 "hh"|> SkewBinaryRandomAccessList.lookup 0) = "hh") && ((len8 |> SkewBinaryRandomAccessList.update 1 "gg"|> SkewBinaryRandomAccessList.lookup 1) = "gg") 
                && ((len8 |> SkewBinaryRandomAccessList.update 2 "ff"|> SkewBinaryRandomAccessList.lookup 2) = "ff") && ((len8 |> SkewBinaryRandomAccessList.update 3 "ee"|> SkewBinaryRandomAccessList.lookup 3) = "ee") 
                && ((len8 |> SkewBinaryRandomAccessList.update 4 "dd"|> SkewBinaryRandomAccessList.lookup 4) = "dd") && ((len8 |> SkewBinaryRandomAccessList.update 5 "cc"|> SkewBinaryRandomAccessList.lookup 5) = "cc") 
                && ((len8 |> SkewBinaryRandomAccessList.update 6 "bb"|> SkewBinaryRandomAccessList.lookup 6) = "bb") && ((len8 |> SkewBinaryRandomAccessList.update 7 "aa"|> SkewBinaryRandomAccessList.lookup 7) = "aa")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 9" {
                let len9 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i"
                (((len9 |> SkewBinaryRandomAccessList.update 0 "ii"|> SkewBinaryRandomAccessList.lookup 0) = "ii") && ((len9 |> SkewBinaryRandomAccessList.update 1 "hh"|> SkewBinaryRandomAccessList.lookup 1) = "hh") 
                && ((len9 |> SkewBinaryRandomAccessList.update 2 "gg"|> SkewBinaryRandomAccessList.lookup 2) = "gg") && ((len9 |> SkewBinaryRandomAccessList.update 3 "ff"|> SkewBinaryRandomAccessList.lookup 3) = "ff") 
                && ((len9 |> SkewBinaryRandomAccessList.update 4 "ee"|> SkewBinaryRandomAccessList.lookup 4) = "ee") && ((len9 |> SkewBinaryRandomAccessList.update 5 "dd"|> SkewBinaryRandomAccessList.lookup 5) = "dd") 
                && ((len9 |> SkewBinaryRandomAccessList.update 6 "cc"|> SkewBinaryRandomAccessList.lookup 6) = "cc") && ((len9 |> SkewBinaryRandomAccessList.update 7 "bb"|> SkewBinaryRandomAccessList.lookup 7) = "bb")
                && ((len9 |> SkewBinaryRandomAccessList.update 8 "aa"|> SkewBinaryRandomAccessList.lookup 8) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.update length 10" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                (((lena |> SkewBinaryRandomAccessList.update 0 "jj"|> SkewBinaryRandomAccessList.lookup 0) = "jj") && ((lena |> SkewBinaryRandomAccessList.update 1 "ii"|> SkewBinaryRandomAccessList.lookup 1) = "ii") 
                && ((lena |> SkewBinaryRandomAccessList.update 2 "hh"|> SkewBinaryRandomAccessList.lookup 2) = "hh") && ((lena |> SkewBinaryRandomAccessList.update 3 "gg"|> SkewBinaryRandomAccessList.lookup 3) = "gg") 
                && ((lena |> SkewBinaryRandomAccessList.update 4 "ff"|> SkewBinaryRandomAccessList.lookup 4) = "ff") && ((lena |> SkewBinaryRandomAccessList.update 5 "ee"|> SkewBinaryRandomAccessList.lookup 5) = "ee") 
                && ((lena |> SkewBinaryRandomAccessList.update 6 "dd"|> SkewBinaryRandomAccessList.lookup 6) = "dd") && ((lena |> SkewBinaryRandomAccessList.update 7 "cc"|> SkewBinaryRandomAccessList.lookup 7) = "cc")
                && ((lena |> SkewBinaryRandomAccessList.update 8 "bb"|> SkewBinaryRandomAccessList.lookup 8) = "bb") && ((lena |> SkewBinaryRandomAccessList.update 9 "aa"|> SkewBinaryRandomAccessList.lookup 9) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 1" {
                let a = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.tryUpdate 0 "aa"
                ((a.Value |> SkewBinaryRandomAccessList.lookup 0) = "aa") |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 2" {
                let len2 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b"
                let b = len2 |> SkewBinaryRandomAccessList.tryUpdate 0 "bb"
                let a = len2 |> SkewBinaryRandomAccessList.tryUpdate 1 "aa"
                (((b.Value |> SkewBinaryRandomAccessList.lookup 0) = "bb") && ((a.Value |> SkewBinaryRandomAccessList.lookup 1) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 3" {
                let len3 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c"
                let c = len3 |> SkewBinaryRandomAccessList.tryUpdate 0 "cc"
                let b = len3 |> SkewBinaryRandomAccessList.tryUpdate 1 "bb"
                let a = len3 |> SkewBinaryRandomAccessList.tryUpdate 2 "aa"
                (((c.Value |> SkewBinaryRandomAccessList.lookup 0) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 1) = "bb") && ((a.Value |> SkewBinaryRandomAccessList.lookup 2) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 4" {
                let len4 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d"
                let d = len4 |> SkewBinaryRandomAccessList.tryUpdate 0 "dd"
                let c = len4 |> SkewBinaryRandomAccessList.tryUpdate 1 "cc"
                let b = len4 |> SkewBinaryRandomAccessList.tryUpdate 2 "bb"
                let a = len4 |> SkewBinaryRandomAccessList.tryUpdate 3 "aa"
                (((d.Value |> SkewBinaryRandomAccessList.lookup 0) = "dd") && ((c.Value |> SkewBinaryRandomAccessList.lookup 1) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 2) = "bb") 
                && ((a.Value |> SkewBinaryRandomAccessList.lookup 3) = "aa")) |> Expect.isTrue "" } 

            test "SkewBinaryRandomAccessList.tryUpdate length 5" {
                let len5 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e"
                let e = len5 |> SkewBinaryRandomAccessList.tryUpdate 0 "ee"
                let d = len5 |> SkewBinaryRandomAccessList.tryUpdate 1 "dd"
                let c = len5 |> SkewBinaryRandomAccessList.tryUpdate 2 "cc"
                let b = len5 |> SkewBinaryRandomAccessList.tryUpdate 3 "bb"
                let a = len5 |> SkewBinaryRandomAccessList.tryUpdate 4 "aa"
                (((e.Value |> SkewBinaryRandomAccessList.lookup 0) = "ee") && ((d.Value |> SkewBinaryRandomAccessList.lookup 1) = "dd") && ((c.Value |> SkewBinaryRandomAccessList.lookup 2) = "cc") 
                && ((b.Value |> SkewBinaryRandomAccessList.lookup 3) = "bb") && ((a.Value |> SkewBinaryRandomAccessList.lookup 4) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 6" {
                let len6 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f"
                let f = len6 |> SkewBinaryRandomAccessList.tryUpdate 0 "ff"
                let e = len6 |> SkewBinaryRandomAccessList.tryUpdate 1 "ee"
                let d = len6 |> SkewBinaryRandomAccessList.tryUpdate 2 "dd"
                let c = len6 |> SkewBinaryRandomAccessList.tryUpdate 3 "cc"
                let b = len6 |> SkewBinaryRandomAccessList.tryUpdate 4 "bb"
                let a = len6 |> SkewBinaryRandomAccessList.tryUpdate 5 "aa"
                (((f.Value |> SkewBinaryRandomAccessList.lookup 0) = "ff") && ((e.Value |> SkewBinaryRandomAccessList.lookup 1) = "ee") && ((d.Value |> SkewBinaryRandomAccessList.lookup 2) = "dd") 
                && ((c.Value |> SkewBinaryRandomAccessList.lookup 3) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 4) = "bb") && ((a.Value |> SkewBinaryRandomAccessList.lookup 5) = "aa")) 
                |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 7" {
                let len7 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g"
                let g = len7 |> SkewBinaryRandomAccessList.tryUpdate 0 "gg"
                let f = len7 |> SkewBinaryRandomAccessList.tryUpdate 1 "ff"
                let e = len7 |> SkewBinaryRandomAccessList.tryUpdate 2 "ee"
                let d = len7 |> SkewBinaryRandomAccessList.tryUpdate 3 "dd"
                let c = len7 |> SkewBinaryRandomAccessList.tryUpdate 4 "cc"
                let b = len7 |> SkewBinaryRandomAccessList.tryUpdate 5 "bb"
                let a = len7 |> SkewBinaryRandomAccessList.tryUpdate 6 "aa"
                (((g.Value |> SkewBinaryRandomAccessList.lookup 0) = "gg") && ((f.Value |> SkewBinaryRandomAccessList.lookup 1) = "ff") && ((e.Value |> SkewBinaryRandomAccessList.lookup 2) = "ee") 
                && ((d.Value |> SkewBinaryRandomAccessList.lookup 3) = "dd") && ((c.Value |> SkewBinaryRandomAccessList.lookup 4) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 5) = "bb") 
                && ((a.Value |> SkewBinaryRandomAccessList.lookup 6) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 8" {
                let len8 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h"
                let h = len8 |> SkewBinaryRandomAccessList.tryUpdate 0 "hh"
                let g = len8 |> SkewBinaryRandomAccessList.tryUpdate 1 "gg"
                let f = len8 |> SkewBinaryRandomAccessList.tryUpdate 2 "ff"
                let e = len8 |> SkewBinaryRandomAccessList.tryUpdate 3 "ee"
                let d = len8 |> SkewBinaryRandomAccessList.tryUpdate 4 "dd"
                let c = len8 |> SkewBinaryRandomAccessList.tryUpdate 5 "cc"
                let b = len8 |> SkewBinaryRandomAccessList.tryUpdate 6 "bb"
                let a = len8 |> SkewBinaryRandomAccessList.tryUpdate 7 "aa"
                (((h.Value |> SkewBinaryRandomAccessList.lookup 0) = "hh") && ((g.Value |> SkewBinaryRandomAccessList.lookup 1) = "gg") && ((f.Value |> SkewBinaryRandomAccessList.lookup 2) = "ff") 
                && ((e.Value |> SkewBinaryRandomAccessList.lookup 3) = "ee") && ((d.Value |> SkewBinaryRandomAccessList.lookup 4) = "dd") && ((c.Value |> SkewBinaryRandomAccessList.lookup 5) = "cc")  
                && ((b.Value |> SkewBinaryRandomAccessList.lookup 6) = "bb")&& ((a.Value |> SkewBinaryRandomAccessList.lookup 7) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 9" {
                let len9 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i"
                let i = len9 |> SkewBinaryRandomAccessList.tryUpdate 0 "ii"
                let h = len9 |> SkewBinaryRandomAccessList.tryUpdate 1 "hh"
                let g = len9 |> SkewBinaryRandomAccessList.tryUpdate 2 "gg"
                let f = len9 |> SkewBinaryRandomAccessList.tryUpdate 3 "ff"
                let e = len9 |> SkewBinaryRandomAccessList.tryUpdate 4 "ee"
                let d = len9 |> SkewBinaryRandomAccessList.tryUpdate 5 "dd"
                let c = len9 |> SkewBinaryRandomAccessList.tryUpdate 6 "cc"
                let b = len9 |> SkewBinaryRandomAccessList.tryUpdate 7 "bb"
                let a = len9 |> SkewBinaryRandomAccessList.tryUpdate 8 "aa"
                (((i.Value |> SkewBinaryRandomAccessList.lookup 0) = "ii") && ((h.Value |> SkewBinaryRandomAccessList.lookup 1) = "hh") && ((g.Value |> SkewBinaryRandomAccessList.lookup 2) = "gg") 
                && ((f.Value |> SkewBinaryRandomAccessList.lookup 3) = "ff") && ((e.Value |> SkewBinaryRandomAccessList.lookup 4) = "ee") && ((d.Value |> SkewBinaryRandomAccessList.lookup 5) = "dd") 
                && ((c.Value |> SkewBinaryRandomAccessList.lookup 6) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 7) = "bb")&& ((a.Value |> SkewBinaryRandomAccessList.lookup 8) = "aa")) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.tryUpdate length 10" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                let j = lena |> SkewBinaryRandomAccessList.tryUpdate 0 "jj"
                let i = lena |> SkewBinaryRandomAccessList.tryUpdate 1 "ii"
                let h = lena |> SkewBinaryRandomAccessList.tryUpdate 2 "hh"
                let g = lena |> SkewBinaryRandomAccessList.tryUpdate 3 "gg"
                let f = lena |> SkewBinaryRandomAccessList.tryUpdate 4 "ff"
                let e = lena |> SkewBinaryRandomAccessList.tryUpdate 5 "ee"
                let d = lena |> SkewBinaryRandomAccessList.tryUpdate 6 "dd"
                let c = lena |> SkewBinaryRandomAccessList.tryUpdate 7 "cc"
                let b = lena |> SkewBinaryRandomAccessList.tryUpdate 8 "bb"
                let a = lena |> SkewBinaryRandomAccessList.tryUpdate 9 "aa"
                (((j.Value |> SkewBinaryRandomAccessList.lookup 0) = "jj") && ((i.Value |> SkewBinaryRandomAccessList.lookup 1) = "ii") && ((h.Value |> SkewBinaryRandomAccessList.lookup 2) = "hh") 
                && ((g.Value |> SkewBinaryRandomAccessList.lookup 3) = "gg") && ((f.Value |> SkewBinaryRandomAccessList.lookup 4) = "ff") && ((e.Value |> SkewBinaryRandomAccessList.lookup 5) = "ee") 
                && ((d.Value |> SkewBinaryRandomAccessList.lookup 6) = "dd") && ((c.Value |> SkewBinaryRandomAccessList.lookup 7) = "cc") && ((b.Value |> SkewBinaryRandomAccessList.lookup 8) = "bb")
                && ((a.Value |> SkewBinaryRandomAccessList.lookup 9) = "aa")) |> Expect.isTrue "" }

            test "length of empty is 0" {
                SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.length |> Expect.equal "" 0 } 

            test "length of 1 - 10 good" {
                let len1 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a"
                let len2 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b"
                let len3 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c"
                let len4 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d"
                let len5 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e"
                let len6 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f"
                let len7 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g"
                let len8 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h"
                let len9 = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i"
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                (((SkewBinaryRandomAccessList.length len1) = 1) && ((SkewBinaryRandomAccessList.length len2) = 2) && ((SkewBinaryRandomAccessList.length len3) = 3) && ((SkewBinaryRandomAccessList.length len4) = 4) 
                && ((SkewBinaryRandomAccessList.length len5) = 5) && ((SkewBinaryRandomAccessList.length len6) = 6) && ((SkewBinaryRandomAccessList.length len7) = 7) && ((SkewBinaryRandomAccessList.length len8) = 8) 
                && ((SkewBinaryRandomAccessList.length len9) = 9) && ((SkewBinaryRandomAccessList.length lena) = 10)) |> Expect.isTrue "" }

            test "SkewBinaryRandomAccessList.ofSeq" {
                let x = SkewBinaryRandomAccessList.ofSeq ["a";"b";"c";"d";"e";"f";"g";"h";"i";"j"]

                (((x |> SkewBinaryRandomAccessList.lookup 0) = "a") && ((x |> SkewBinaryRandomAccessList.lookup 1) = "b") && ((x |> SkewBinaryRandomAccessList.lookup 2) = "c") && ((x |> SkewBinaryRandomAccessList.lookup 3) = "d") 
                && ((x |> SkewBinaryRandomAccessList.lookup 4) = "e") && ((x |> SkewBinaryRandomAccessList.lookup 5) = "f") && ((x |> SkewBinaryRandomAccessList.lookup 6) = "g") && ((x |> SkewBinaryRandomAccessList.lookup 7) = "h")
                && ((x |> SkewBinaryRandomAccessList.lookup 8) = "i") && ((x |> SkewBinaryRandomAccessList.lookup 9) = "j")) |> Expect.isTrue "" }

            test "IRandomAccessList SkewBinaryRandomAccessList.cons works" {
                let lena = SkewBinaryRandomAccessList.empty() |> SkewBinaryRandomAccessList.cons "a" |> SkewBinaryRandomAccessList.cons "b" |> SkewBinaryRandomAccessList.cons "c" |> SkewBinaryRandomAccessList.cons "d" |> SkewBinaryRandomAccessList.cons "e" |> SkewBinaryRandomAccessList.cons "f" |> SkewBinaryRandomAccessList.cons "g" |> SkewBinaryRandomAccessList.cons "h" |> SkewBinaryRandomAccessList.cons "i" |> SkewBinaryRandomAccessList.cons "j"
                ((lena :> IRandomAccessList<string>).Cons "zz").Head |> Expect.equal "" "zz" } 
        ]
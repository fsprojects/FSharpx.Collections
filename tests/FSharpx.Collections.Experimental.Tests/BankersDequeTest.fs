namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//quite a lot going on and difficult to reason about edge cases
//testing up to BankersDeque.length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)
//6 makes front and back lists 3 long when C = 2

module BankersDequeTest =

    let len1 = BankersDeque.singleton "a"
    let len2 = BankersDeque.singleton "a" |> BankersDeque.cons "b"
    let len3 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c"
    let len4 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d"
    let len5 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e"
    let len6 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f"
    let len7 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g"
    let len8 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h"
    let len9 = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h" |> BankersDeque.cons "i"
    let lena = BankersDeque.singleton "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h" |> BankersDeque.cons "i" |> BankersDeque.cons "j"

    let len1snoc = BankersDeque.singleton "a"
    let len2snoc = BankersDeque.singleton "b" |> BankersDeque.snoc "a"
    let len3snoc = BankersDeque.singleton "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len4snoc = BankersDeque.singleton "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len5snoc = BankersDeque.singleton "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len6snoc = BankersDeque.singleton "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len7snoc = BankersDeque.singleton "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len8snoc = BankersDeque.singleton "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len9snoc = BankersDeque.singleton "i" |> BankersDeque.snoc "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let lenasnoc = BankersDeque.singleton "j" |> BankersDeque.snoc "i" |> BankersDeque.snoc "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"

    let len1C3 = BankersDeque.empty 3 |> BankersDeque.cons "a"
    let len2C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b"
    let len3C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c"
    let len4C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d"
    let len5C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e"
    let len6C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f"
    let len7C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g"
    let len8C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h"
    let len9C3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h" |> BankersDeque.cons "i"
    let lenaC3 = BankersDeque.empty 3 |> BankersDeque.cons "a" |> BankersDeque.cons "b" |> BankersDeque.cons "c" |> BankersDeque.cons "d" |> BankersDeque.cons "e" |> BankersDeque.cons "f" |> BankersDeque.cons "g" |> BankersDeque.cons "h" |> BankersDeque.cons "i" |> BankersDeque.cons "j"

    let len1C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "a"
    let len2C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len3C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len4C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len5C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len6C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len7C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len8C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let len9C3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "i" |> BankersDeque.snoc "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"
    let lenaC3snoc = BankersDeque.empty 3 |> BankersDeque.snoc "j" |> BankersDeque.snoc "i" |> BankersDeque.snoc "h" |> BankersDeque.snoc "g" |> BankersDeque.snoc "f" |> BankersDeque.snoc "e" |> BankersDeque.snoc "d" |> BankersDeque.snoc "c" |> BankersDeque.snoc "b" |> BankersDeque.snoc "a"

    [<Tests>]
    let testBankersDeque =

        testList "Experimental BankersDeque" [
            test "BankersDeque.empty dqueue should be BankersDeque.empty" {

                BankersDeque.isEmpty (BankersDeque.empty 2) |> Expect.isTrue "" }

            test "BankersDeque.cons works" {
                ((len2 |> BankersDeque.isEmpty) && (len2C3 |> BankersDeque.isEmpty)) |> Expect.isFalse "" }

            test "BankersDeque.snoc works" {
                ((len2snoc |> BankersDeque.isEmpty) && (len2C3snoc |> BankersDeque.isEmpty)) |> Expect.isFalse "" }

            test "BankersDeque.singleton BankersDeque.head works" {
                (((BankersDeque.head len1) = "a") && ((len1C3 |> BankersDeque.isEmpty)) = false) |> Expect.isTrue "" }

            test "BankersDeque.singleton BankersDeque.last works" {
                len1 |> BankersDeque.last |> Expect.equal "" "a" } 

            test "BankersDeque.tail of BankersDeque.singleton BankersDeque.empty" {
                len1 |> BankersDeque.tail |> BankersDeque.isEmpty |> Expect.isTrue "" }

            test "BankersDeque.tail of BankersDeque.tail of 2 BankersDeque.empty" {
                 ( len2 |> BankersDeque.tail |> BankersDeque.tail |> BankersDeque.isEmpty) |> Expect.isTrue "" }

            test "BankersDeque.init of BankersDeque.singleton BankersDeque.empty" {
                ((BankersDeque.init len1)  |> BankersDeque.isEmpty) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 1" {
                let t1 = BankersDeque.tail len2
                let t1C = BankersDeque.tail len2C3
                let t1s = BankersDeque.tail len2snoc
                let t1Cs = BankersDeque.tail len2C3snoc
                (((BankersDeque.length t1) = 1) && ((BankersDeque.length t1C) = 1) && ((BankersDeque.length t1s) = 1) && ((BankersDeque.length t1Cs) = 1) 
                && ((BankersDeque.head t1) = "a") && ((BankersDeque.head t1C) = "a") && ((BankersDeque.head t1s) = "a") && ((BankersDeque.head t1Cs) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 2" {
                let t1 = BankersDeque.tail len3
                let t1C = BankersDeque.tail len3C3
                let t1s = BankersDeque.tail len3snoc
                let t1Cs = BankersDeque.tail len3C3snoc

                let t1_1 = BankersDeque.tail t1
                let t1C_1 = BankersDeque.tail t1C
                let t1_1s = BankersDeque.tail t1s
                let t1C_1s = BankersDeque.tail t1Cs

                (((BankersDeque.length t1) = 2) && ((BankersDeque.length t1C) = 2) && ((BankersDeque.length t1s) = 2) && ((BankersDeque.length t1Cs) = 2)
                && ((BankersDeque.head t1) = "b") && ((BankersDeque.head t1C) = "b") && ((BankersDeque.head t1s) = "b") && ((BankersDeque.head t1Cs) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 3" {
                let t1 = BankersDeque.tail len4
                let t1C = BankersDeque.tail len4C3
                let t1s = BankersDeque.tail len4snoc
                let t1Cs = BankersDeque.tail len4C3snoc

                let t1_2 = BankersDeque.tail t1
                let t1C_2 = BankersDeque.tail t1C
                let t1_2s = BankersDeque.tail t1s
                let t1C_2s = BankersDeque.tail t1Cs

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 3) && ((BankersDeque.length t1C) = 3) && ((BankersDeque.length t1s) = 3) && ((BankersDeque.length t1Cs) = 3)
                && ((BankersDeque.head t1) = "c") && ((BankersDeque.head t1C) = "c") && ((BankersDeque.head t1s) = "c") && ((BankersDeque.head t1Cs) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2)
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 4" {
                let t1 = BankersDeque.tail len5
                let t1C = BankersDeque.tail len5C3
                let t1s = BankersDeque.tail len5snoc
                let t1Cs = BankersDeque.tail len5C3snoc

                let t1_3 = BankersDeque.tail t1
                let t1C_3 = BankersDeque.tail t1C
                let t1_3s = BankersDeque.tail t1s
                let t1C_3s = BankersDeque.tail t1Cs

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 4) && ((BankersDeque.length t1C) = 4) && ((BankersDeque.length t1s) = 4) && ((BankersDeque.length t1Cs) = 4)
                && ((BankersDeque.head t1) = "d") && ((BankersDeque.head t1C) = "d") && ((BankersDeque.head t1s) = "d") && ((BankersDeque.head t1Cs) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 5" {
                let t1 = BankersDeque.tail len6
                let t1C = BankersDeque.tail len6C3
                let t1s = BankersDeque.tail len6snoc
                let t1Cs = BankersDeque.tail len6C3snoc

                let t1_4 = BankersDeque.tail t1
                let t1C_4 = BankersDeque.tail t1C
                let t1_4s = BankersDeque.tail t1s
                let t1C_4s = BankersDeque.tail t1Cs

                let t1_3 = BankersDeque.tail t1_4
                let t1C_3 = BankersDeque.tail t1C_4
                let t1_3s = BankersDeque.tail t1_4s
                let t1C_3s = BankersDeque.tail t1C_4s

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 5) && ((BankersDeque.length t1C) = 5) && ((BankersDeque.length t1s) = 5) && ((BankersDeque.length t1Cs) = 5)
                && ((BankersDeque.head t1) = "e") && ((BankersDeque.head t1C) = "e") && ((BankersDeque.head t1s) = "e") && ((BankersDeque.head t1Cs) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.length t1_4s) = 4) && ((BankersDeque.length t1C_4s) = 4)
                && ((BankersDeque.head t1_4) = "d") && ((BankersDeque.head t1C_4) = "d") && ((BankersDeque.head t1_4s) = "d") && ((BankersDeque.head t1C_4s) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 6" {
                let t1 = BankersDeque.tail len7
                let t1C = BankersDeque.tail len7C3
                let t1s = BankersDeque.tail len7snoc
                let t1Cs = BankersDeque.tail len7C3snoc

                let t1_5 = BankersDeque.tail t1
                let t1C_5 = BankersDeque.tail t1C
                let t1_5s = BankersDeque.tail t1s
                let t1C_5s = BankersDeque.tail t1Cs

                let t1_4 = BankersDeque.tail t1_5
                let t1C_4 = BankersDeque.tail t1C_5
                let t1_4s = BankersDeque.tail t1_5s
                let t1C_4s = BankersDeque.tail t1C_5s

                let t1_3 = BankersDeque.tail t1_4
                let t1C_3 = BankersDeque.tail t1C_4
                let t1_3s = BankersDeque.tail t1_4s
                let t1C_3s = BankersDeque.tail t1C_4s

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 6) && ((BankersDeque.length t1C) = 6) && ((BankersDeque.length t1s) = 6) && ((BankersDeque.length t1Cs) = 6)
                && ((BankersDeque.head t1) = "f") && ((BankersDeque.head t1C) = "f") && ((BankersDeque.head t1s) = "f") && ((BankersDeque.head t1Cs) = "f")
                && ((BankersDeque.length t1_5) = 5) && ((BankersDeque.length t1C_5) = 5) && ((BankersDeque.length t1_5s) = 5) && ((BankersDeque.length t1C_5s) = 5)
                && ((BankersDeque.head t1_5) = "e") && ((BankersDeque.head t1C_5) = "e") && ((BankersDeque.head t1_5s) = "e") && ((BankersDeque.head t1C_5s) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.length t1_4s) = 4) && ((BankersDeque.length t1C_4s) = 4)
                && ((BankersDeque.head t1_4) = "d") && ((BankersDeque.head t1C_4) = "d") && ((BankersDeque.head t1_4s) = "d") && ((BankersDeque.head t1C_4s) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 7" {
                let t1 = BankersDeque.tail len8
                let t1C = BankersDeque.tail len8C3
                let t1s = BankersDeque.tail len8snoc
                let t1Cs = BankersDeque.tail len8C3snoc

                let t1_6 = BankersDeque.tail t1
                let t1C_6 = BankersDeque.tail t1C
                let t1_6s = BankersDeque.tail t1s
                let t1C_6s = BankersDeque.tail t1Cs

                let t1_5 = BankersDeque.tail t1_6
                let t1C_5 = BankersDeque.tail t1C_6
                let t1_5s = BankersDeque.tail t1_6s
                let t1C_5s = BankersDeque.tail t1C_6s

                let t1_4 = BankersDeque.tail t1_5
                let t1C_4 = BankersDeque.tail t1C_5
                let t1_4s = BankersDeque.tail t1_5s
                let t1C_4s = BankersDeque.tail t1C_5s

                let t1_3 = BankersDeque.tail t1_4
                let t1C_3 = BankersDeque.tail t1C_4
                let t1_3s = BankersDeque.tail t1_4s
                let t1C_3s = BankersDeque.tail t1C_4s

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 7) && ((BankersDeque.length t1C) = 7) && ((BankersDeque.length t1s) = 7) && ((BankersDeque.length t1Cs) = 7)
                && ((BankersDeque.head t1) = "g") && ((BankersDeque.head t1C) = "g") && ((BankersDeque.head t1s) = "g") && ((BankersDeque.head t1Cs) = "g")
                && ((BankersDeque.length t1_6) = 6) && ((BankersDeque.length t1C_6) = 6) && ((BankersDeque.length t1_6s) = 6) && ((BankersDeque.length t1C_6s) = 6)
                && ((BankersDeque.head t1_6) = "f") && ((BankersDeque.head t1C_6) = "f") && ((BankersDeque.head t1_6s) = "f") && ((BankersDeque.head t1C_6s) = "f")
                && ((BankersDeque.length t1_5) = 5) && ((BankersDeque.length t1C_5) = 5) && ((BankersDeque.length t1_5s) = 5) && ((BankersDeque.length t1C_5s) = 5)
                && ((BankersDeque.head t1_5) = "e") && ((BankersDeque.head t1C_5) = "e") && ((BankersDeque.head t1_5s) = "e") && ((BankersDeque.head t1C_5s) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.length t1_4s) = 4) && ((BankersDeque.length t1C_4s) = 4)
                && ((BankersDeque.head t1_4) = "d") && ((BankersDeque.head t1C_4) = "d") && ((BankersDeque.head t1_4s) = "d") && ((BankersDeque.head t1C_4s) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 8" {
                let t1 = BankersDeque.tail len9
                let t1C = BankersDeque.tail len9C3
                let t1s = BankersDeque.tail len9snoc
                let t1Cs = BankersDeque.tail len9C3snoc

                let t1_7 = BankersDeque.tail t1
                let t1C_7 = BankersDeque.tail t1C
                let t1_7s = BankersDeque.tail t1s
                let t1C_7s = BankersDeque.tail t1Cs

                let t1_6 = BankersDeque.tail t1_7
                let t1C_6 = BankersDeque.tail t1C_7
                let t1_6s = BankersDeque.tail t1_7s
                let t1C_6s = BankersDeque.tail t1C_7s

                let t1_5 = BankersDeque.tail t1_6
                let t1C_5 = BankersDeque.tail t1C_6
                let t1_5s = BankersDeque.tail t1_6s
                let t1C_5s = BankersDeque.tail t1C_6s

                let t1_4 = BankersDeque.tail t1_5
                let t1C_4 = BankersDeque.tail t1C_5
                let t1_4s = BankersDeque.tail t1_5s
                let t1C_4s = BankersDeque.tail t1C_5s

                let t1_3 = BankersDeque.tail t1_4
                let t1C_3 = BankersDeque.tail t1C_4
                let t1_3s = BankersDeque.tail t1_4s
                let t1C_3s = BankersDeque.tail t1C_4s

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 8) && ((BankersDeque.length t1C) = 8) && ((BankersDeque.length t1s) = 8) && ((BankersDeque.length t1Cs) = 8)
                && ((BankersDeque.head t1) = "h") && ((BankersDeque.head t1C) = "h") && ((BankersDeque.head t1s) = "h") && ((BankersDeque.head t1Cs) = "h")
                && ((BankersDeque.length t1_7) = 7) && ((BankersDeque.length t1C_7) = 7) && ((BankersDeque.length t1_7s) = 7) && ((BankersDeque.length t1C_7s) = 7)
                && ((BankersDeque.head t1_7) = "g") && ((BankersDeque.head t1C_7) = "g") && ((BankersDeque.head t1_7s) = "g") && ((BankersDeque.head t1C_7s) = "g")
                && ((BankersDeque.length t1_6) = 6) && ((BankersDeque.length t1C_6) = 6) && ((BankersDeque.length t1_6s) = 6) && ((BankersDeque.length t1C_6s) = 6)
                && ((BankersDeque.head t1_6) = "f") && ((BankersDeque.head t1C_6) = "f") && ((BankersDeque.head t1_6s) = "f") && ((BankersDeque.head t1C_6s) = "f")
                && ((BankersDeque.length t1_5) = 5) && ((BankersDeque.length t1C_5) = 5) && ((BankersDeque.length t1_5s) = 5) && ((BankersDeque.length t1C_5s) = 5)
                && ((BankersDeque.head t1_5) = "e") && ((BankersDeque.head t1C_5) = "e") && ((BankersDeque.head t1_5s) = "e") && ((BankersDeque.head t1C_5s) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.length t1_4s) = 4) && ((BankersDeque.length t1C_4s) = 4)
                && ((BankersDeque.head t1_4) = "d") && ((BankersDeque.head t1C_4) = "d") && ((BankersDeque.head t1_4s) = "d") && ((BankersDeque.head t1C_4s) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.head, BankersDeque.tail, and BankersDeque.length work test 9" {
                let t1 = BankersDeque.tail lena
                let t1C = BankersDeque.tail lenaC3
                let t1s = BankersDeque.tail lenasnoc
                let t1Cs = BankersDeque.tail lenaC3snoc

                let t1_8 = BankersDeque.tail t1
                let t1C_8 = BankersDeque.tail t1C
                let t1_8s = BankersDeque.tail t1s
                let t1C_8s = BankersDeque.tail t1Cs

                let t1_7 = BankersDeque.tail t1_8
                let t1C_7 = BankersDeque.tail t1C_8
                let t1_7s = BankersDeque.tail t1_8s
                let t1C_7s = BankersDeque.tail t1C_8s

                let t1_6 = BankersDeque.tail t1_7
                let t1C_6 = BankersDeque.tail t1C_7
                let t1_6s = BankersDeque.tail t1_7s
                let t1C_6s = BankersDeque.tail t1C_7s

                let t1_5 = BankersDeque.tail t1_6
                let t1C_5 = BankersDeque.tail t1C_6
                let t1_5s = BankersDeque.tail t1_6s
                let t1C_5s = BankersDeque.tail t1C_6s

                let t1_4 = BankersDeque.tail t1_5
                let t1C_4 = BankersDeque.tail t1C_5
                let t1_4s = BankersDeque.tail t1_5s
                let t1C_4s = BankersDeque.tail t1C_5s

                let t1_3 = BankersDeque.tail t1_4
                let t1C_3 = BankersDeque.tail t1C_4
                let t1_3s = BankersDeque.tail t1_4s
                let t1C_3s = BankersDeque.tail t1C_4s

                let t1_2 = BankersDeque.tail t1_3
                let t1C_2 = BankersDeque.tail t1C_3
                let t1_2s = BankersDeque.tail t1_3s
                let t1C_2s = BankersDeque.tail t1C_3s

                let t1_1 = BankersDeque.tail t1_2
                let t1C_1 = BankersDeque.tail t1C_2
                let t1_1s = BankersDeque.tail t1_2s
                let t1C_1s = BankersDeque.tail t1C_2s

                (((BankersDeque.length t1) = 9) && ((BankersDeque.length t1C) = 9) && ((BankersDeque.length t1s) = 9) && ((BankersDeque.length t1Cs) = 9)
                && ((BankersDeque.head t1) = "i") && ((BankersDeque.head t1C) = "i") && ((BankersDeque.head t1s) = "i") && ((BankersDeque.head t1Cs) = "i")
                && ((BankersDeque.length t1_8) = 8) && ((BankersDeque.length t1C_8) = 8) && ((BankersDeque.length t1_8s) = 8) && ((BankersDeque.length t1C_8s) = 8)
                && ((BankersDeque.head t1_8) = "h") && ((BankersDeque.head t1C_8) = "h") && ((BankersDeque.head t1_8s) = "h") && ((BankersDeque.head t1C_8s) = "h")
                && ((BankersDeque.length t1_7) = 7) && ((BankersDeque.length t1C_7) = 7) && ((BankersDeque.length t1_7s) = 7) && ((BankersDeque.length t1C_7s) = 7)
                && ((BankersDeque.head t1_7) = "g") && ((BankersDeque.head t1C_7) = "g") && ((BankersDeque.head t1_7s) = "g") && ((BankersDeque.head t1C_7s) = "g")
                && ((BankersDeque.length t1_6) = 6) && ((BankersDeque.length t1C_6) = 6) && ((BankersDeque.length t1_6s) = 6) && ((BankersDeque.length t1C_6s) = 6)
                && ((BankersDeque.head t1_6) = "f") && ((BankersDeque.head t1C_6) = "f") && ((BankersDeque.head t1_6s) = "f") && ((BankersDeque.head t1C_6s) = "f")
                && ((BankersDeque.length t1_5) = 5) && ((BankersDeque.length t1C_5) = 5) && ((BankersDeque.length t1_5s) = 5) && ((BankersDeque.length t1C_5s) = 5)
                && ((BankersDeque.head t1_5) = "e") && ((BankersDeque.head t1C_5) = "e") && ((BankersDeque.head t1_5s) = "e") && ((BankersDeque.head t1C_5s) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.length t1_4s) = 4) && ((BankersDeque.length t1C_4s) = 4)
                && ((BankersDeque.head t1_4) = "d") && ((BankersDeque.head t1C_4) = "d") && ((BankersDeque.head t1_4s) = "d") && ((BankersDeque.head t1C_4s) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.length t1_3s) = 3) && ((BankersDeque.length t1C_3s) = 3)
                && ((BankersDeque.head t1_3) = "c") && ((BankersDeque.head t1C_3) = "c") && ((BankersDeque.head t1_3s) = "c") && ((BankersDeque.head t1C_3s) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.length t1_2s) = 2) && ((BankersDeque.length t1C_2s) = 2) 
                && ((BankersDeque.head t1_2) = "b") && ((BankersDeque.head t1C_2) = "b") && ((BankersDeque.head t1_2s) = "b") && ((BankersDeque.head t1C_2s) = "b")
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.length t1_1s) = 1) && ((BankersDeque.length t1C_1s) = 1) 
                && ((BankersDeque.head t1_1) = "a") && ((BankersDeque.head t1C_1) = "a") && ((BankersDeque.head t1_1s) = "a") && ((BankersDeque.head t1C_1s) = "a")) |> Expect.isTrue "" }

            //the previous series thoroughly tested construction by BankersDeque.snoc, so we'll leave those out
            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 1" {  
                let t1 = BankersDeque.init len2
                let t1C = BankersDeque.init len2C3
    
                (((BankersDeque.length t1) = 1) && ((BankersDeque.length t1C) = 1) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 2" {
                let t1 = BankersDeque.init len3
                let t1C = BankersDeque.init len3C3
    
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
    
                (((BankersDeque.length t1) = 2) && ((BankersDeque.length t1C) = 2) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 1) && ((BankersDeque.length t1C_1) = 1) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 3" {
                let t1 = BankersDeque.init len4
                let t1C = BankersDeque.init len4C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
    
                (((BankersDeque.length t1) = 3) && ((BankersDeque.length t1C) = 3) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 2) && ((BankersDeque.length t1C_1) = 2) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 1) && ((BankersDeque.length t1C_2) = 1) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 4" {
                let t1 = BankersDeque.init len5
                let t1C = BankersDeque.init len5C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
    
                (((BankersDeque.length t1) = 4) && ((BankersDeque.length t1C) = 4) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 3) && ((BankersDeque.length t1C_1) = 3) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 2) && ((BankersDeque.length t1C_2) = 2) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 1) && ((BankersDeque.length t1C_3) = 1) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 5" {
                let t1 = BankersDeque.init len6
                let t1C = BankersDeque.init len6C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
                let t1_4 = BankersDeque.init t1_3
                let t1C_4 = BankersDeque.init t1C_3
    
                (((BankersDeque.length t1) = 5) && ((BankersDeque.length t1C) = 5) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 4) && ((BankersDeque.length t1C_1) = 4) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 3) && ((BankersDeque.length t1C_2) = 3) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 2) && ((BankersDeque.length t1C_3) = 2) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")
                && ((BankersDeque.length t1_4) = 1) && ((BankersDeque.length t1C_4) = 1) && ((BankersDeque.last t1_4) = "f") && ((BankersDeque.last t1C_4) = "f")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 6" {
                let t1 = BankersDeque.init len7
                let t1C = BankersDeque.init len7C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
                let t1_4 = BankersDeque.init t1_3
                let t1C_4 = BankersDeque.init t1C_3
                let t1_5 = BankersDeque.init t1_4
                let t1C_5 = BankersDeque.init t1C_4
    
                (((BankersDeque.length t1) = 6) && ((BankersDeque.length t1C) = 6) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 5) && ((BankersDeque.length t1C_1) = 5) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 4) && ((BankersDeque.length t1C_2) = 4) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 3) && ((BankersDeque.length t1C_3) = 3) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")
                && ((BankersDeque.length t1_4) = 2) && ((BankersDeque.length t1C_4) = 2) && ((BankersDeque.last t1_4) = "f") && ((BankersDeque.last t1C_4) = "f")
                && ((BankersDeque.length t1_5) = 1) && ((BankersDeque.length t1C_5) = 1) && ((BankersDeque.last t1_5) = "g") && ((BankersDeque.last t1C_5) = "g")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 7" {
                let t1 = BankersDeque.init len8
                let t1C = BankersDeque.init len8C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
                let t1_4 = BankersDeque.init t1_3
                let t1C_4 = BankersDeque.init t1C_3
                let t1_5 = BankersDeque.init t1_4
                let t1C_5 = BankersDeque.init t1C_4
                let t1_6 = BankersDeque.init t1_5
                let t1C_6 = BankersDeque.init t1C_5
    
                (((BankersDeque.length t1) = 7) && ((BankersDeque.length t1C) = 7) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 6) && ((BankersDeque.length t1C_1) = 6) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 5) && ((BankersDeque.length t1C_2) = 5) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 4) && ((BankersDeque.length t1C_3) = 4) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")
                && ((BankersDeque.length t1_4) = 3) && ((BankersDeque.length t1C_4) = 3) && ((BankersDeque.last t1_4) = "f") && ((BankersDeque.last t1C_4) = "f")
                && ((BankersDeque.length t1_5) = 2) && ((BankersDeque.length t1C_5) = 2) && ((BankersDeque.last t1_5) = "g") && ((BankersDeque.last t1C_5) = "g")
                && ((BankersDeque.length t1_6) = 1) && ((BankersDeque.length t1C_6) = 1) && ((BankersDeque.last t1_6) = "h") && ((BankersDeque.last t1C_6) = "h")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 8" {
                let t1 = BankersDeque.init len9
                let t1C = BankersDeque.init len9C3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
                let t1_4 = BankersDeque.init t1_3
                let t1C_4 = BankersDeque.init t1C_3
                let t1_5 = BankersDeque.init t1_4
                let t1C_5 = BankersDeque.init t1C_4
                let t1_6 = BankersDeque.init t1_5
                let t1C_6 = BankersDeque.init t1C_5
                let t1_7 = BankersDeque.init t1_6
                let t1C_7 = BankersDeque.init t1C_6
    
                (((BankersDeque.length t1) = 8) && ((BankersDeque.length t1C) = 8) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 7) && ((BankersDeque.length t1C_1) = 7) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 6) && ((BankersDeque.length t1C_2) = 6) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 5) && ((BankersDeque.length t1C_3) = 5) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")
                && ((BankersDeque.length t1_4) = 4) && ((BankersDeque.length t1C_4) = 4) && ((BankersDeque.last t1_4) = "f") && ((BankersDeque.last t1C_4) = "f")
                && ((BankersDeque.length t1_5) = 3) && ((BankersDeque.length t1C_5) = 3) && ((BankersDeque.last t1_5) = "g") && ((BankersDeque.last t1C_5) = "g")
                && ((BankersDeque.length t1_6) = 2) && ((BankersDeque.length t1C_6) = 2) && ((BankersDeque.last t1_6) = "h") && ((BankersDeque.last t1C_6) = "h")
                && ((BankersDeque.length t1_7) = 1) && ((BankersDeque.length t1C_7) = 1) && ((BankersDeque.last t1_7) = "i") && ((BankersDeque.last t1C_7) = "i")) |> Expect.isTrue "" }

            test "BankersDeque.last, BankersDeque.init, and BankersDeque.length work test 9" {
                let t1 = BankersDeque.init lena
                let t1C = BankersDeque.init lenaC3
                let t1_1 = BankersDeque.init t1
                let t1C_1 = BankersDeque.init t1C
                let t1_2 = BankersDeque.init t1_1
                let t1C_2 = BankersDeque.init t1C_1
                let t1_3 = BankersDeque.init t1_2
                let t1C_3 = BankersDeque.init t1C_2
                let t1_4 = BankersDeque.init t1_3
                let t1C_4 = BankersDeque.init t1C_3
                let t1_5 = BankersDeque.init t1_4
                let t1C_5 = BankersDeque.init t1C_4
                let t1_6 = BankersDeque.init t1_5
                let t1C_6 = BankersDeque.init t1C_5
                let t1_7 = BankersDeque.init t1_6
                let t1C_7 = BankersDeque.init t1C_6
                let t1_8 = BankersDeque.init t1_7
                let t1C_8 = BankersDeque.init t1C_7
    
                (((BankersDeque.length t1) = 9) && ((BankersDeque.length t1C) = 9) && ((BankersDeque.last t1) = "b") && ((BankersDeque.last t1C) = "b") 
                && ((BankersDeque.length t1_1) = 8) && ((BankersDeque.length t1C_1) = 8) && ((BankersDeque.last t1_1) = "c") && ((BankersDeque.last t1C_1) = "c")
                && ((BankersDeque.length t1_2) = 7) && ((BankersDeque.length t1C_2) = 7) && ((BankersDeque.last t1_2) = "d") && ((BankersDeque.last t1C_2) = "d")
                && ((BankersDeque.length t1_3) = 6) && ((BankersDeque.length t1C_3) = 6) && ((BankersDeque.last t1_3) = "e") && ((BankersDeque.last t1C_3) = "e")
                && ((BankersDeque.length t1_4) = 5) && ((BankersDeque.length t1C_4) = 5) && ((BankersDeque.last t1_4) = "f") && ((BankersDeque.last t1C_4) = "f")
                && ((BankersDeque.length t1_5) = 4) && ((BankersDeque.length t1C_5) = 4) && ((BankersDeque.last t1_5) = "g") && ((BankersDeque.last t1C_5) = "g")
                && ((BankersDeque.length t1_6) = 3) && ((BankersDeque.length t1C_6) = 3) && ((BankersDeque.last t1_6) = "h") && ((BankersDeque.last t1C_6) = "h")
                && ((BankersDeque.length t1_7) = 2) && ((BankersDeque.length t1C_7) = 2) && ((BankersDeque.last t1_7) = "i") && ((BankersDeque.last t1C_7) = "i")
                && ((BankersDeque.length t1_8) = 1) && ((BankersDeque.length t1C_8) = 1) && ((BankersDeque.last t1_8) = "j") && ((BankersDeque.last t1C_8) = "j")) |> Expect.isTrue "" }

            test "IEnumerable Seq" {
                (lena |> Seq.toArray).[5] |> Expect.equal "" "e" } 

            test "IEnumerable Seq BankersDeque.length" {
                lena |> Seq.length |> Expect.equal "" 10 } 

            test "type BankersDeque.cons works" {
                lena.Cons "zz" |> BankersDeque.head |> Expect.equal "" "zz" } 

            test "IDeque BankersDeque.cons works" {
                ((lena :> IDeque<string>).Cons "zz").Head |> Expect.equal "" "zz" } 

            test "BankersDeque.ofCatLists and BankersDeque.uncons" {
                let d = BankersDeque.ofCatLists ["a";"b";"c"] ["d";"e";"f"]
                let h1, t1 = BankersDeque.uncons d
                let h2, t2 = BankersDeque.uncons t1
                let h3, t3 = BankersDeque.uncons t2
                let h4, t4 = BankersDeque.uncons t3
                let h5, t5 = BankersDeque.uncons t4
                let h6, t6 = BankersDeque.uncons t5

                ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e") && (h6 = "f") && (BankersDeque.isEmpty t6)) |> Expect.isTrue "" }

            test "BankersDeque.ofCatSeqs and BankersDeque.uncons" {
                let d = BankersDeque.ofCatSeqs (seq {'a'..'c'}) (seq {'d'..'f'})
                let h1, t1 = BankersDeque.uncons d
                let h2, t2 = BankersDeque.uncons t1
                let h3, t3 = BankersDeque.uncons t2
                let h4, t4 = BankersDeque.uncons t3
                let h5, t5 = BankersDeque.uncons t4
                let h6, t6 = BankersDeque.uncons t5

                ((h1 = 'a') && (h2 = 'b') && (h3 = 'c') && (h4 = 'd') && (h5 = 'e') && (h6 = 'f') && (BankersDeque.isEmpty t6)) |> Expect.isTrue "" }

            test "BankersDeque.unsnoc works" {
                let d = BankersDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]
                let i1, l1 = BankersDeque.unsnoc d
                let i2, l2 = BankersDeque.unsnoc i1
                let i3, l3 = BankersDeque.unsnoc i2
                let i4, l4 = BankersDeque.unsnoc i3
                let i5, l5 = BankersDeque.unsnoc i4
                let i6, l6 = BankersDeque.unsnoc i5

                ((l1 = "a") && (l2 = "b") && (l3 = "c") && (l4 = "d") && (l5 = "e") && (l6 = "f") && (BankersDeque.isEmpty i6)) |> Expect.isTrue "" }

            test "BankersDeque.snoc pattern discriminator" {
                let d = (BankersDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let i1, l1 = BankersDeque.unsnoc d 

                let i2, l2 = 
                    match i1 with
                    | BankersDeque.Snoc(i, l) -> i, l
                    | _ -> i1, "x"

                ((l2 = "b") && ((BankersDeque.length i2) = 4)) |> Expect.isTrue "" }

            test "BankersDeque.cons pattern discriminator" {
                let d = (BankersDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let h1, t1 = BankersDeque.uncons d 

                let h2, t2 = 
                    match t1 with
                    | BankersDeque.Cons(h, t) -> h, t
                    | _ ->  "x", t1

                ((h2 = "e") && ((BankersDeque.length t2) = 4)) |> Expect.isTrue "" }

            test "BankersDeque.cons and BankersDeque.snoc pattern discriminator" {
                let d = (BankersDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
    
                let mid1 = 
                    match d with
                    | BankersDeque.Cons(h, BankersDeque.Snoc(i, l)) -> i
                    | _ -> d

                let head, last = 
                    match mid1 with
                    | BankersDeque.Cons(h, BankersDeque.Snoc(i, l)) -> h, l
                    | _ -> "x", "x"

                ((head = "e") && (last = "b")) |> Expect.isTrue "" }

            test "BankersDeque.rev BankersDeque.empty dqueue should be BankersDeque.empty" {
                BankersDeque.isEmpty (BankersDeque.rev (BankersDeque.empty 2)) |> Expect.isTrue "" }

            test "BankersDeque.rev dqueue BankersDeque.length 1" {
                ((BankersDeque.head (BankersDeque.rev len1) = "a") && (BankersDeque.head (BankersDeque.rev len1C3) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.rev dqueue BankersDeque.length 2" {
                let r1 = BankersDeque.rev len2
                let r1c = BankersDeque.rev len2C3
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")) |> Expect.isTrue "" }

            test "BankersDeque.rev dqueue BankersDeque.length 3" {
                let r1 = BankersDeque.rev len3
                let r1c = BankersDeque.rev len3C3
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c")) |> Expect.isTrue "" }

            test "BankersDeque.rev dqueue BankersDeque.length 4" {
                let r1 = BankersDeque.rev len4
                let r1c = BankersDeque.rev len4C3
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")) |> Expect.isTrue "" }

            test "BankersDeque.rev dqueue BankersDeque.length 5" {
                let r1 = BankersDeque.rev len5
                let r1c = BankersDeque.rev len5C3
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")
                && (h5 = "e") && (h5c = "e")) |> Expect.isTrue "" }

            //BankersDeque.length 6 more than sufficient to test BankersDeque.rev
            test "BankersDeque.rev dqueue BankersDeque.length 6" {
                let r1 = BankersDeque.rev len6
                let r1c = BankersDeque.rev len6C3
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f")) |> Expect.isTrue "" }

            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.empty" {
                ((BankersDeque.isEmpty (BankersDeque.ofSeq [])) && (BankersDeque.isEmpty (BankersDeque.ofSeqC 3 []))) |> Expect.isTrue "" }

            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 1" {
                ((BankersDeque.head       (BankersDeque.ofSeq ["a"])      = "a") && (BankersDeque.head      (BankersDeque.ofSeqC 3 ["a"])      = "a")) |> Expect.isTrue "" }

            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 2" {
                let r1 = BankersDeque.ofSeq ["a";"b"]
                let r1c = BankersDeque.ofSeqC 3 ["a";"b"]
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")) |> Expect.isTrue "" }

            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 3" {
                let r1 = BankersDeque.ofSeq ["a";"b";"c"]
                let r1c = BankersDeque.ofSeqC 3 ["a";"b";"c"]
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c")) |> Expect.isTrue "" }

            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 4" {
                let r1 = BankersDeque.ofSeq ["a";"b";"c";"d"]
                let r1c = BankersDeque.ofSeqC 3 ["a";"b";"c";"d"]
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")) |> Expect.isTrue "" }

            //BankersDeque.length 5 more than sufficient to test BankersDeque.ofSeq and BankersDeque.ofSeqC
            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 5" {
                let r1 = BankersDeque.ofSeq ["a";"b";"c";"d";"e"]
                let r1c = BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e"]
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")
                && (h5 = "e") && (h5c = "e")) |> Expect.isTrue "" }

            //BankersDeque.length 5 more than sufficient to test BankersDeque.ofSeq and BankersDeque.ofSeqC
            test "BankersDeque.ofSeq and BankersDeque.ofSeqC BankersDeque.length 6" {
                let r1 = BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]
                let r1c = BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e";"f"]
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b") && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f")) |> Expect.isTrue "" }

            test "appending BankersDeque.empty dqueus" {
                ((BankersDeque.isEmpty (BankersDeque.append (BankersDeque.ofSeq []) (BankersDeque.ofSeq []) )) && (BankersDeque.isEmpty (BankersDeque.append (BankersDeque.empty 3) (BankersDeque.empty 3)))
                ) |> Expect.isTrue "" }

            test "appending BankersDeque.empty and BankersDeque.length 1" {
                ((BankersDeque.head (BankersDeque.append (BankersDeque.ofSeq []) len1) = "a") && (BankersDeque.head (BankersDeque.append len1 (BankersDeque.empty 3)) = "a")) |> Expect.isTrue "" }

            test "appending BankersDeque.empty and BankersDeque.length 2" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq []) (BankersDeque.ofSeq ["a";"b"])
                let r1c = BankersDeque.append (BankersDeque.empty 3) (BankersDeque.ofSeqC 3 ["a";"b"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq [])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.empty 3)
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 1 and BankersDeque.length 2" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a"]) (BankersDeque.ofSeq ["b";"c"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a"]) (BankersDeque.ofSeqC 3 ["b";"c"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq ["c"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.ofSeqC 3 ["c"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 1 and BankersDeque.length 3" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a"]) (BankersDeque.ofSeq ["b";"c";"d"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a"]) (BankersDeque.ofSeqC 3 ["b";"c";"d"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c"]) (BankersDeque.ofSeq ["d"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c"]) (BankersDeque.ofSeqC 3 ["d"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 1 and BankersDeque.length 4" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a"]) (BankersDeque.ofSeq ["b";"c";"d";"e"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a"]) (BankersDeque.ofSeqC 3 ["b";"c";"d";"e"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d"]) (BankersDeque.ofSeq ["e"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d"]) (BankersDeque.ofSeqC 3 ["e"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr

                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d") 
                && (h5r = "e") && (h5cr = "e")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 1 and BankersDeque.length 5" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a"]) (BankersDeque.ofSeq ["b";"c";"d";"e";"f"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a"]) (BankersDeque.ofSeqC 3 ["b";"c";"d";"e";"f"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) (BankersDeque.ofSeq ["f"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e"]) (BankersDeque.ofSeqC 3 ["f"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr
                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr
                let t6r = BankersDeque.tail t5r
                let t6cr = BankersDeque.tail t5cr
                let h6r = BankersDeque.head t6r
                let h6cr = BankersDeque.head t6cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d") 
                && (h5r = "e") && (h5cr = "e") && (h6r = "f") && (h6cr = "f")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 6 and BankersDeque.length 7" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) (BankersDeque.ofSeq ["g";"h";"i";"j";"k";"l";"m"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e";"f"]) (BankersDeque.ofSeqC 3 ["g";"h";"i";"j";"k";"l";"m"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c
                let t7 = BankersDeque.tail t6
                let t7c = BankersDeque.tail t6c
                let h7 = BankersDeque.head t7
                let h7c = BankersDeque.head t7c
                let h7 = BankersDeque.head t7
                let h7c = BankersDeque.head t7c
                let t8 = BankersDeque.tail t7
                let t8c = BankersDeque.tail t7c
                let h8 = BankersDeque.head t8
                let h8c = BankersDeque.head t8c
                let h8 = BankersDeque.head t8
                let h8c = BankersDeque.head t8c
                let t9 = BankersDeque.tail t8
                let t9c = BankersDeque.tail t8c
                let h9 = BankersDeque.head t9
                let h9c = BankersDeque.head t9c
                let h9 = BankersDeque.head t9
                let h9c = BankersDeque.head t9c
                let t10 = BankersDeque.tail t9
                let t10c = BankersDeque.tail t9c
                let h10 = BankersDeque.head t10
                let h10c = BankersDeque.head t10c
                let h10 = BankersDeque.head t10
                let h10c = BankersDeque.head t10c
                let t11 = BankersDeque.tail t10
                let t11c = BankersDeque.tail t10c
                let h11 = BankersDeque.head t11
                let h11c = BankersDeque.head t11c
                let h11 = BankersDeque.head t11
                let h11c = BankersDeque.head t11c
                let t12 = BankersDeque.tail t11
                let t12c = BankersDeque.tail t11c
                let h12 = BankersDeque.head t12
                let h12c = BankersDeque.head t12c
                let h12 = BankersDeque.head t12
                let h12c = BankersDeque.head t12c
                let t13 = BankersDeque.tail t12
                let t13c = BankersDeque.tail t12c
                let h13 = BankersDeque.head t13
                let h13c = BankersDeque.head t13c
                let h13 = BankersDeque.head t13
                let h13c = BankersDeque.head t13c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f";"g"]) (BankersDeque.ofSeq ["h";"i";"j";"k";"l";"m"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e";"f";"g"]) (BankersDeque.ofSeqC 3 ["h";"i";"j";"k";"l";"m"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr
                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr
                let t6r = BankersDeque.tail t5r
                let t6cr = BankersDeque.tail t5cr
                let h6r = BankersDeque.head t6r
                let h6cr = BankersDeque.head t6cr
                let t7r = BankersDeque.tail t6r
                let t7cr = BankersDeque.tail t6cr
                let h7r = BankersDeque.head t7r
                let h7cr = BankersDeque.head t7cr
                let h7r = BankersDeque.head t7r
                let h7cr = BankersDeque.head t7cr
                let t8r = BankersDeque.tail t7r
                let t8cr = BankersDeque.tail t7cr
                let h8r = BankersDeque.head t8r
                let h8cr = BankersDeque.head t8cr
                let h8r = BankersDeque.head t8r
                let h8cr = BankersDeque.head t8cr
                let t9r = BankersDeque.tail t8r
                let t9cr = BankersDeque.tail t8cr
                let h9r = BankersDeque.head t9r
                let h9cr = BankersDeque.head t9cr
                let h9r = BankersDeque.head t9r
                let h9cr = BankersDeque.head t9cr
                let t10r = BankersDeque.tail t9r
                let t10cr = BankersDeque.tail t9cr
                let h10r = BankersDeque.head t10r
                let h10cr = BankersDeque.head t10cr
                let h10r = BankersDeque.head t10r
                let h10cr = BankersDeque.head t10cr
                let t11r = BankersDeque.tail t10r
                let t11cr = BankersDeque.tail t10cr
                let h11r = BankersDeque.head t11r
                let h11cr = BankersDeque.head t11cr
                let h11r = BankersDeque.head t11r
                let h11cr = BankersDeque.head t11cr
                let t12r = BankersDeque.tail t11r
                let t12cr = BankersDeque.tail t11cr
                let h12r = BankersDeque.head t12r
                let h12cr = BankersDeque.head t12cr
                let h12r = BankersDeque.head t12r
                let h12cr = BankersDeque.head t12cr
                let t13r = BankersDeque.tail t12r
                let t13cr = BankersDeque.tail t12cr
                let h13r = BankersDeque.head t13r
                let h13cr = BankersDeque.head t13cr
                let h13r = BankersDeque.head t13r
                let h13cr = BankersDeque.head t13cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f") && (h7 = "g") && (h7c = "g") && (h8 = "h") && (h8c = "h") 
                && (h9 = "i") && (h9c = "i") && (h10 = "j") && (h10c = "j") && (h11 = "k") && (h11c = "k") && (h12 = "l") && (h12c = "l") 
                && (h13 = "m") && (h13c = "m") && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") 
                && (h4r = "d") && (h4cr = "d") && (h5r = "e") && (h5cr = "e") && (h6r = "f") && (h6cr = "f") && (h7r = "g") && (h7cr = "g") 
                && (h8r = "h") && (h8cr = "h") && (h9r = "i") && (h9cr = "i") && (h10r = "j") && (h10cr = "j") 
                && (h11r = "k") && (h11cr = "k") && (h12r = "l") && (h12cr = "l") && (h13r = "m") && (h13cr = "m")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 2 and BankersDeque.length 2" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq ["c";"d"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.ofSeqC 3 ["c";"d"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 2 and BankersDeque.length 3" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq ["c";"d";"e"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.ofSeqC 3 ["c";"d";"e"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c

                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c"]) (BankersDeque.ofSeq ["d";"e"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c"]) (BankersDeque.ofSeqC 3 ["d";"e"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr

                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d") 
                && (h5r = "e") && (h5cr = "e")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 2 and BankersDeque.length 4" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq ["c";"d";"e";"f"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.ofSeqC 3 ["c";"d";"e";"f"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d"]) (BankersDeque.ofSeq ["e";"f"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d"]) (BankersDeque.ofSeqC 3 ["e";"f"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr
                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr
                let t6r = BankersDeque.tail t5r
                let t6cr = BankersDeque.tail t5cr
                let h6r = BankersDeque.head t6r
                let h6cr = BankersDeque.head t6cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d") 
                && (h5r = "e") && (h5cr = "e") && (h6r = "f") && (h6cr = "f")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 2 and BankersDeque.length 5" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b"]) (BankersDeque.ofSeq ["c";"d";"e";"f";"g"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b"]) (BankersDeque.ofSeqC 3 ["c";"d";"e";"f";"g"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c
                let t7 = BankersDeque.tail t6
                let t7c = BankersDeque.tail t6c
                let h7 = BankersDeque.head t7
                let h7c = BankersDeque.head t7c

                let r1r = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) (BankersDeque.ofSeq ["f";"g"])
                let r1cr = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c";"d";"e"]) (BankersDeque.ofSeqC 3 ["f";"g"])
                let h1r = BankersDeque.head r1r
                let h1cr = BankersDeque.head r1cr
                let t2r = BankersDeque.tail r1r
                let t2cr = BankersDeque.tail r1cr
                let h2r = BankersDeque.head t2r
                let h2cr = BankersDeque.head t2cr
                let t3r = BankersDeque.tail t2r
                let t3cr = BankersDeque.tail t2cr
                let h3r = BankersDeque.head t3r
                let h3cr = BankersDeque.head t3cr
                let t4r = BankersDeque.tail t3r
                let t4cr = BankersDeque.tail t3cr
                let h4r = BankersDeque.head t4r
                let h4cr = BankersDeque.head t4cr
                let t5r = BankersDeque.tail t4r
                let t5cr = BankersDeque.tail t4cr
                let h5r = BankersDeque.head t5r
                let h5cr = BankersDeque.head t5cr
                let t6r = BankersDeque.tail t5r
                let t6cr = BankersDeque.tail t5cr
                let h6r = BankersDeque.head t6r
                let h6cr = BankersDeque.head t6cr
                let t7r = BankersDeque.tail t6r
                let t7cr = BankersDeque.tail t6cr
                let h7r = BankersDeque.head t7r
                let h7cr = BankersDeque.head t7cr

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f") && (h7 = "g") && (h7c = "g")
                && (h1r = "a") && (h1cr = "a") && (h2r = "b") && (h2cr = "b") && (h3r = "c") && (h3cr = "c") && (h4r = "d") && (h4cr = "d") 
                && (h5r = "e") && (h5cr = "e") && (h6r = "f") && (h6cr = "f") && (h7r = "g") && (h7cr = "g")) |> Expect.isTrue "" }

            test "appending BankersDeque.length 3 and BankersDeque.length 3" {
                let r1 = BankersDeque.append (BankersDeque.ofSeq ["a";"b";"c"]) (BankersDeque.ofSeq ["d";"e";"f"])
                let r1c = BankersDeque.append (BankersDeque.ofSeqC 3 ["a";"b";"c"]) (BankersDeque.ofSeqC 3 ["d";"e";"f"])
                let h1 = BankersDeque.head r1
                let h1c = BankersDeque.head r1c
                let t2 = BankersDeque.tail r1
                let t2c = BankersDeque.tail r1c
                let h2 = BankersDeque.head t2
                let h2c = BankersDeque.head t2c
                let t3 = BankersDeque.tail t2
                let t3c = BankersDeque.tail t2c
                let h3 = BankersDeque.head t3
                let h3c = BankersDeque.head t3c
                let t4 = BankersDeque.tail t3
                let t4c = BankersDeque.tail t3c
                let h4 = BankersDeque.head t4
                let h4c = BankersDeque.head t4c
                let t5 = BankersDeque.tail t4
                let t5c = BankersDeque.tail t4c
                let h5 = BankersDeque.head t5
                let h5c = BankersDeque.head t5c
                let t6 = BankersDeque.tail t5
                let t6c = BankersDeque.tail t5c
                let h6 = BankersDeque.head t6
                let h6c = BankersDeque.head t6c

                ((h1 = "a") && (h1c = "a") && (h2 = "b") && (h2c = "b")  && (h3 = "c") && (h3c = "c") && (h4 = "d") && (h4c = "d") 
                && (h5 = "e") && (h5c = "e") && (h6 = "f") && (h6c = "f")) |> Expect.isTrue "" }


            test "BankersDeque.lookup BankersDeque.length 1" {
                len1 |> BankersDeque.lookup 0 |> Expect.equal "" "a" } 

            test "BankersDeque.lookup BankersDeque.length 2" {
                (((len2 |> BankersDeque.lookup 0) = "b") && ((len2 |> BankersDeque.lookup 1) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 3" {
                (((len3 |> BankersDeque.lookup 0) = "c") && ((len3 |> BankersDeque.lookup 1) = "b") && ((len3 |> BankersDeque.lookup 2) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 4" {
                (((len4 |> BankersDeque.lookup 0) = "d") && ((len4 |> BankersDeque.lookup 1) = "c") && ((len4 |> BankersDeque.lookup 2) = "b") && ((len4 |> BankersDeque.lookup 3) = "a")) 
                |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 5" {
                (((len5 |> BankersDeque.lookup 0) = "e") && ((len5 |> BankersDeque.lookup 1) = "d") && ((len5 |> BankersDeque.lookup 2) = "c") && ((len5 |> BankersDeque.lookup 3) = "b") 
                && ((len5 |> BankersDeque.lookup 4) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 6" {
                (((len6 |> BankersDeque.lookup 0) = "f") && ((len6 |> BankersDeque.lookup 1) = "e") && ((len6 |> BankersDeque.lookup 2) = "d") && ((len6 |> BankersDeque.lookup 3) = "c") 
                && ((len6 |> BankersDeque.lookup 4) = "b") && ((len6 |> BankersDeque.lookup 5) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 7" {
                (((len7 |> BankersDeque.lookup 0) = "g") && ((len7 |> BankersDeque.lookup 1) = "f") && ((len7 |> BankersDeque.lookup 2) = "e") && ((len7 |> BankersDeque.lookup 3) = "d") 
                && ((len7 |> BankersDeque.lookup 4) = "c") && ((len7 |> BankersDeque.lookup 5) = "b") && ((len7 |> BankersDeque.lookup 6) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 8" {
                (((len8 |> BankersDeque.lookup 0) = "h") && ((len8 |> BankersDeque.lookup 1) = "g") && ((len8 |> BankersDeque.lookup 2) = "f") && ((len8 |> BankersDeque.lookup 3) = "e") 
                && ((len8 |> BankersDeque.lookup 4) = "d") && ((len8 |> BankersDeque.lookup 5) = "c") && ((len8 |> BankersDeque.lookup 6) = "b") && ((len8 |> BankersDeque.lookup 7) = "a")) 
                |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 9" {
                (((len9 |> BankersDeque.lookup 0) = "i") && ((len9 |> BankersDeque.lookup 1) = "h") && ((len9 |> BankersDeque.lookup 2) = "g") && ((len9 |> BankersDeque.lookup 3) = "f") 
                && ((len9 |> BankersDeque.lookup 4) = "e") && ((len9 |> BankersDeque.lookup 5) = "d") && ((len9 |> BankersDeque.lookup 6) = "c") && ((len9 |> BankersDeque.lookup 7) = "b")
                && ((len9 |> BankersDeque.lookup 8) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.lookup BankersDeque.length 10" {
                (((lena |> BankersDeque.lookup 0) = "j") && ((lena |> BankersDeque.lookup 1) = "i") && ((lena |> BankersDeque.lookup 2) = "h") && ((lena |> BankersDeque.lookup 3) = "g") 
                && ((lena |> BankersDeque.lookup 4) = "f") && ((lena |> BankersDeque.lookup 5) = "e") && ((lena |> BankersDeque.lookup 6) = "d") && ((lena |> BankersDeque.lookup 7) = "c")
                && ((lena |> BankersDeque.lookup 8) = "b") && ((lena |> BankersDeque.lookup 9) = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 1" {
                let a = len1 |> BankersDeque.tryLookup 0 
                (a.Value = "a") |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 2" {
                let b = len2 |> BankersDeque.tryLookup 0
                let a = len2 |> BankersDeque.tryLookup 1
                ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 3" {
                let c = len3 |> BankersDeque.tryLookup 0
                let b = len3 |> BankersDeque.tryLookup 1
                let a = len3 |> BankersDeque.tryLookup 2
                ((c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 4" {
                let d = len4 |> BankersDeque.tryLookup 0
                let c = len4 |> BankersDeque.tryLookup 1
                let b = len4 |> BankersDeque.tryLookup 2
                let a = len4 |> BankersDeque.tryLookup 3
                ((d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" } 

            test "BankersDeque.tryLookup BankersDeque.length 5" {
                let e = len5 |> BankersDeque.tryLookup 0
                let d = len5 |> BankersDeque.tryLookup 1
                let c = len5 |> BankersDeque.tryLookup 2
                let b = len5 |> BankersDeque.tryLookup 3
                let a = len5 |> BankersDeque.tryLookup 4
                ((e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 6" {
                let f = len6 |> BankersDeque.tryLookup 0
                let e = len6 |> BankersDeque.tryLookup 1
                let d = len6 |> BankersDeque.tryLookup 2
                let c = len6 |> BankersDeque.tryLookup 3
                let b = len6 |> BankersDeque.tryLookup 4
                let a = len6 |> BankersDeque.tryLookup 5
                ((f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) 
                |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 7" {
                let g = len7 |> BankersDeque.tryLookup 0
                let f = len7 |> BankersDeque.tryLookup 1
                let e = len7 |> BankersDeque.tryLookup 2
                let d = len7 |> BankersDeque.tryLookup 3
                let c = len7 |> BankersDeque.tryLookup 4
                let b = len7 |> BankersDeque.tryLookup 5
                let a = len7 |> BankersDeque.tryLookup 6
                ((g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") 
                && (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 8" {
                let h = len8 |> BankersDeque.tryLookup 0
                let g = len8 |> BankersDeque.tryLookup 1
                let f = len8 |> BankersDeque.tryLookup 2
                let e = len8 |> BankersDeque.tryLookup 3
                let d = len8 |> BankersDeque.tryLookup 4
                let c = len8 |> BankersDeque.tryLookup 5
                let b = len8 |> BankersDeque.tryLookup 6
                let a = len8 |> BankersDeque.tryLookup 7
                ((h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c")  
                && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 9" {
                let i = len9 |> BankersDeque.tryLookup 0
                let h = len9 |> BankersDeque.tryLookup 1
                let g = len9 |> BankersDeque.tryLookup 2
                let f = len9 |> BankersDeque.tryLookup 3
                let e = len9 |> BankersDeque.tryLookup 4
                let d = len9 |> BankersDeque.tryLookup 5
                let c = len9 |> BankersDeque.tryLookup 6
                let b = len9 |> BankersDeque.tryLookup 7
                let a = len9 |> BankersDeque.tryLookup 8
                ((i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") 
                && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup BankersDeque.length 10" {
                let j = lena |> BankersDeque.tryLookup 0
                let i = lena |> BankersDeque.tryLookup 1
                let h = lena |> BankersDeque.tryLookup 2
                let g = lena |> BankersDeque.tryLookup 3
                let f = lena |> BankersDeque.tryLookup 4
                let e = lena |> BankersDeque.tryLookup 5
                let d = lena |> BankersDeque.tryLookup 6
                let c = lena |> BankersDeque.tryLookup 7
                let b = lena |> BankersDeque.tryLookup 8
                let a = lena |> BankersDeque.tryLookup 9
                ((j.Value = "j") && (i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") 
                && (d.Value = "d") && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BankersDeque.tryLookup not found" {
                lena |> BankersDeque.tryLookup 10 |> Expect.isNone "" }

            test "BankersDeque.remove elements BankersDeque.length 1" {
                len1 |> BankersDeque.remove 0 |> BankersDeque.isEmpty |> Expect.isTrue "" }

            test "BankersDeque.remove elements BankersDeque.length 2" {
                let a = len2 |> BankersDeque.remove 0 |> BankersDeque.head 
                let b = len2 |> BankersDeque.remove 1 |> BankersDeque.head
                ((a = "a") && (b = "b")) |> Expect.isTrue "" }

            test "BankersDeque.remove elements BankersDeque.length 3" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.remove 0
                let b0 = BankersDeque.head r0
                let t0 = BankersDeque.tail r0
                let c0 = BankersDeque.head t0

                let r1 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.remove 1
                let a1 = BankersDeque.head r1
                let t1 = BankersDeque.tail r1
                let c1 = BankersDeque.head t1

                let r2 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.remove 2
                let a2 = BankersDeque.head r2
                let t2 = BankersDeque.tail r2
                let b2 = BankersDeque.head t2

                ((b0 = "b") && (c0 = "c") && (a1 = "a") && (c1 = "c") && (a2 = "a") && (b2 = "b")) |> Expect.isTrue "" }

            test "BankersDeque.remove elements BankersDeque.length 4" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.remove 0
                let b0 = BankersDeque.head r0
                let t0 = BankersDeque.tail r0
                let c0 = BankersDeque.head t0
                let t01 = BankersDeque.tail t0
                let d0 = BankersDeque.head t01

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.remove 1
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.remove 2
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.remove 3
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32

                ((b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (c1 = "c") && (d1 = "d")
                && (a2 = "a") && (b2 = "b") && (c2 = "d")
                && (a3 = "a") && (b3 = "b") && (c3 = "c")) |> Expect.isTrue "" }

            test "BankersDeque.remove elements BankersDeque.length 5" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.remove 0
                let b0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let c0 = BankersDeque.head t01
                let t02= BankersDeque.tail t01
                let d0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let e0 = BankersDeque.head t03

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.remove 1
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let e1 = BankersDeque.head t13

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.remove 2
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let e2 = BankersDeque.head t23

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.remove 3
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let e3 = BankersDeque.head t33

                let r4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.remove 4
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")) |> Expect.isTrue "" }

            test "BankersDeque.remove elements BankersDeque.length 6" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 0
                let b0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let c0 = BankersDeque.head t01
                let t02= BankersDeque.tail t01
                let d0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let e0 = BankersDeque.head t03
                let t04 = BankersDeque.tail t03
                let f0 = BankersDeque.head t04

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 1
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let e1 = BankersDeque.head t13
                let t14 = BankersDeque.tail t13
                let f1 = BankersDeque.head t14

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 2
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let e2 = BankersDeque.head t23
                let t24 = BankersDeque.tail t23
                let f2 = BankersDeque.head t24

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 3
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let e3 = BankersDeque.head t33
                let t34 = BankersDeque.tail t33
                let f3 = BankersDeque.head t34

                let r4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 4
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43
                let t44 = BankersDeque.tail t43
                let f4 = BankersDeque.head t44

                let r5 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.remove 5
                let a5 = BankersDeque.head r5
                let t51 = BankersDeque.tail r5
                let b5 = BankersDeque.head t51
                let t52 = BankersDeque.tail t51
                let c5 = BankersDeque.head t52
                let t53 = BankersDeque.tail t52
                let d5 = BankersDeque.head t53
                let t54 = BankersDeque.tail t53
                let e5 = BankersDeque.head t54

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d") && (e5 = "e")) |> Expect.isTrue "" }

            test "tryRemoveempty" {
                (BankersDeque.empty 3) |> BankersDeque.tryRemove 0 |> Expect.isNone "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 1" {
                let a = len1 |> BankersDeque.tryRemove 0 
                a.Value |> BankersDeque.isEmpty |> Expect.isTrue "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 2" {
                let a = len2 |> BankersDeque.tryRemove 0 
                let a1 =  BankersDeque.head a.Value
                let b = len2 |> BankersDeque.tryRemove 1 
                let b1 = BankersDeque.head b.Value
                ((a1 = "a") && (b1 = "b")) |> Expect.isTrue "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 3" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BankersDeque.head r0
                let t0 = BankersDeque.tail r0
                let c0 = BankersDeque.head t0

                let x1 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t1 = BankersDeque.tail r1
                let c1 = BankersDeque.head t1

                let x2 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BankersDeque.head r2
                let t2 = BankersDeque.tail r2
                let b2 = BankersDeque.head t2

                ((b0 = "b") && (c0 = "c") && (a1 = "a") && (c1 = "c") && (a2 = "a") && (b2 = "b")) |> Expect.isTrue "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 4" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BankersDeque.head r0
                let t0 = BankersDeque.tail r0
                let c0 = BankersDeque.head t0
                let t01 = BankersDeque.tail t0
                let d0 = BankersDeque.head t01

                let x1 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12
 
                let x2 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
     
                let x3 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32

                ((b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (c1 = "c") && (d1 = "d")
                && (a2 = "a") && (b2 = "b") && (c2 = "d")
                && (a3 = "a") && (b3 = "b") && (c3 = "c")) |> Expect.isTrue "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 5" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let c0 = BankersDeque.head t01
                let t02= BankersDeque.tail t01
                let d0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let e0 = BankersDeque.head t03

                let x1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let e1 = BankersDeque.head t13

                let x2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let e2 = BankersDeque.head t23

                let x3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let e3 = BankersDeque.head t33

                let x4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.tryRemove 4
                let r4 = x4.Value
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")) |> Expect.isTrue "" }

            test "BankersDeque.tryRemove elements BankersDeque.length 6" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let c0 = BankersDeque.head t01
                let t02= BankersDeque.tail t01
                let d0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let e0 = BankersDeque.head t03
                let t04 = BankersDeque.tail t03
                let f0 = BankersDeque.head t04

                let x1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 1
                let r1 =x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let c1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let d1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let e1 = BankersDeque.head t13
                let t14 = BankersDeque.tail t13
                let f1 = BankersDeque.head t14

                let x2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 2
                let r2 = x2.Value 
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let e2 = BankersDeque.head t23
                let t24 = BankersDeque.tail t23
                let f2 = BankersDeque.head t24

                let x3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let e3 = BankersDeque.head t33
                let t34 = BankersDeque.tail t33
                let f3 = BankersDeque.head t34

                let x4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 4
                let r4 = x4.Value
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43
                let t44 = BankersDeque.tail t43
                let f4 = BankersDeque.head t44

                let x5 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.tryRemove 5
                let r5 = x5.Value
                let a5 = BankersDeque.head r5
                let t51 = BankersDeque.tail r5
                let b5 = BankersDeque.head t51
                let t52 = BankersDeque.tail t51
                let c5 = BankersDeque.head t52
                let t53 = BankersDeque.tail t52
                let d5 = BankersDeque.head t53
                let t54 = BankersDeque.tail t53
                let e5 = BankersDeque.head t54

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d") && (e5 = "e")) |> Expect.isTrue "" }

            test "BankersDeque.update elements BankersDeque.length 1" {
                len1 |> BankersDeque.update 0 "aa" |> BankersDeque.head |> Expect.equal "" "aa" } 

            test "BankersDeque.update elements BankersDeque.length 2" {
                let r0 = (BankersDeque.ofSeq ["a";"b"]) |> BankersDeque.update 0 "zz"
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01

                let r1 = (BankersDeque.ofSeq ["a";"b"]) |> BankersDeque.update 1 "zz"
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11

                ((a0 = "zz") && (b0 = "b")  
                && (a1 = "a") && (b1 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.update elements BankersDeque.length 3" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.update 0 "zz"
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02

                let r1 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.update 1 "zz"
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12

                let r2 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.update 2 "zz"
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.update elements BankersDeque.length 4" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.update 0 "zz"
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let d0 = BankersDeque.head t03

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.update 1 "zz"
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let d1 = BankersDeque.head t13

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.update 2 "zz"
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let d2 = BankersDeque.head t23

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.update 3 "zz"
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let d3 = BankersDeque.head t33

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.update elements BankersDeque.length 5" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.update 0 "zz"
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let d0 = BankersDeque.head t03
                let t04 = BankersDeque.tail t03
                let e0 = BankersDeque.head t04

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.update 1 "zz"
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let d1 = BankersDeque.head t13
                let t14 = BankersDeque.tail t13
                let e1 = BankersDeque.head t14

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.update 2 "zz"
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let d2 = BankersDeque.head t23
                let t24 = BankersDeque.tail t23
                let e2 = BankersDeque.head t24

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.update 3 "zz"
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let d3 = BankersDeque.head t33
                let t34 = BankersDeque.tail t33
                let e3 = BankersDeque.head t34

                let r4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BankersDeque.update 4 "zz"
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43
                let t44 = BankersDeque.tail t43
                let e4 = BankersDeque.head t44

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d")  && (e0 = "e")
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") && (e1 = "e") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") && (e2 = "e") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")  && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")  && (e4 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.update elements BankersDeque.length 6" {
                let r0 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 0 "zz"
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let d0 = BankersDeque.head t03
                let t04 = BankersDeque.tail t03
                let e0 = BankersDeque.head t04
                let t05 = BankersDeque.tail t04
                let f0 = BankersDeque.head t05

                let r1 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 1 "zz"
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let d1 = BankersDeque.head t13
                let t14 = BankersDeque.tail t13
                let e1 = BankersDeque.head t14
                let t15 = BankersDeque.tail t14
                let f1 = BankersDeque.head t15

                let r2 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 2 "zz"
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let d2 = BankersDeque.head t23
                let t24 = BankersDeque.tail t23
                let e2 = BankersDeque.head t24
                let t25 = BankersDeque.tail t24
                let f2 = BankersDeque.head t25

                let r3 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 3 "zz"
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let d3 = BankersDeque.head t33
                let t34 = BankersDeque.tail t33
                let e3 = BankersDeque.head t34
                let t35 = BankersDeque.tail t34
                let f3 = BankersDeque.head t35

                let r4 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 4 "zz"
                let a4 = BankersDeque.head r4
                let t41 = BankersDeque.tail r4
                let b4 = BankersDeque.head t41
                let t42 = BankersDeque.tail t41
                let c4 = BankersDeque.head t42
                let t43 = BankersDeque.tail t42
                let d4 = BankersDeque.head t43
                let t44 = BankersDeque.tail t43
                let e4 = BankersDeque.head t44
                let t45 = BankersDeque.tail t44
                let f4 = BankersDeque.head t45

                let r5 = (BankersDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BankersDeque.update 5 "zz"
                let a5 = BankersDeque.head r5
                let t51 = BankersDeque.tail r5
                let b5 = BankersDeque.head t51
                let t52 = BankersDeque.tail t51
                let c5 = BankersDeque.head t52
                let t53 = BankersDeque.tail t52
                let d5 = BankersDeque.head t53
                let t54 = BankersDeque.tail t53
                let e5 = BankersDeque.head t54
                let t55 = BankersDeque.tail t54
                let f5 = BankersDeque.head t55

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d")  && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")  && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")  && (e4 = "zz") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d")  && (e5 = "e") && (f5 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.tryUpdate elements BankersDeque.length 1" {
                let a = len1 |> BankersDeque.tryUpdate 0 "aa" 
                a.Value |> BankersDeque.head |> Expect.equal "" "aa" } 

            test "BankersDeque.tryUpdate elements BankersDeque.length 2" {
                let x0 = (BankersDeque.ofSeq ["a";"b"]) |> BankersDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01

                let x1 = (BankersDeque.ofSeq ["a";"b"]) |> BankersDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11

                ((a0 = "zz") && (b0 = "b")  
                && (a1 = "a") && (b1 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.tryUpdate elements BankersDeque.length 3" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02

                let x1 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12

                let x2 = (BankersDeque.ofSeq ["a";"b";"c"]) |> BankersDeque.tryUpdate 2 "zz"
                let r2 = x2.Value
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.tryUpdate elements BankersDeque.length 4" {
                let x0 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BankersDeque.head r0
                let t01 = BankersDeque.tail r0
                let b0 = BankersDeque.head t01
                let t02 = BankersDeque.tail t01
                let c0 = BankersDeque.head t02
                let t03 = BankersDeque.tail t02
                let d0 = BankersDeque.head t03

                let x1 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BankersDeque.head r1
                let t11 = BankersDeque.tail r1
                let b1 = BankersDeque.head t11
                let t12 = BankersDeque.tail t11
                let c1 = BankersDeque.head t12
                let t13 = BankersDeque.tail t12
                let d1 = BankersDeque.head t13

                let x2 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryUpdate 2 "zz"
                let r2 = x2.Value
                let a2 = BankersDeque.head r2
                let t21 = BankersDeque.tail r2
                let b2 = BankersDeque.head t21
                let t22 = BankersDeque.tail t21
                let c2 = BankersDeque.head t22
                let t23 = BankersDeque.tail t22
                let d2 = BankersDeque.head t23

                let x3 = (BankersDeque.ofSeq ["a";"b";"c";"d"]) |> BankersDeque.tryUpdate 3 "zz"
                let r3 = x3.Value
                let a3 = BankersDeque.head r3
                let t31 = BankersDeque.tail r3
                let b3 = BankersDeque.head t31
                let t32 = BankersDeque.tail t31
                let c3 = BankersDeque.head t32
                let t33 = BankersDeque.tail t32
                let d3 = BankersDeque.head t33

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")) |> Expect.isTrue "" }

            test "BankersDeque.tryUncons on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryUncons q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryUncons on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                let x, xs = (BankersDeque.tryUncons q).Value 
                x |> Expect.equal "" "a" } 

            test "BankersDeque.tryUnsnoc on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryUnsnoc q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryUnsnoc on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                let xs, x = (BankersDeque.tryUnsnoc q).Value 
                x |> Expect.equal "" "d" } 

            test "BankersDeque.tryGetHead on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryGetHead q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryGetHead on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                (BankersDeque.tryGetHead q).Value |> Expect.equal "" "a" } 

            test "BankersDeque.tryGetInit on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryGetInit q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryGetInit on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                let x = (BankersDeque.tryGetInit q).Value 
                let x2 = x|> BankersDeque.last 
                x2 |> Expect.equal "" "c"} 

            test "BankersDeque.tryGetLast on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryGetLast q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryGetLast on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                (BankersDeque.tryGetLast q).Value |> Expect.equal "" "d" } 


            test "BankersDeque.tryGetTail on BankersDeque.empty" {
                let q = BankersDeque.empty 2
                (BankersDeque.tryGetTail q = None) |> Expect.isTrue "" }

            test "BankersDeque.tryGetTail on q" {
                let q = BankersDeque.ofSeq ["a";"b";"c";"d"]
                (BankersDeque.tryGetTail q).Value |> BankersDeque.head |> Expect.equal "" "b" } 
        ]
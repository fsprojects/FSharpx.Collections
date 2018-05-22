namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

//quite a lot going on and difficult to reason about edge cases
//testing up to BatchedDeque.length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)

module BatchDequeTest =

    let len1 = BatchedDeque.singleton "a"
    let len2 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b"
    let len3 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c"
    let len4 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d"
    let len5 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e"
    let len6 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e" |> BatchedDeque.cons "f"
    let len7 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e" |> BatchedDeque.cons "f" |> BatchedDeque.cons "g"
    let len8 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e" |> BatchedDeque.cons "f" |> BatchedDeque.cons "g" |> BatchedDeque.cons "h"
    let len9 = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e" |> BatchedDeque.cons "f" |> BatchedDeque.cons "g" |> BatchedDeque.cons "h" |> BatchedDeque.cons "i"
    let lena = BatchedDeque.singleton "a" |> BatchedDeque.cons "b" |> BatchedDeque.cons "c" |> BatchedDeque.cons "d" |> BatchedDeque.cons "e" |> BatchedDeque.cons "f" |> BatchedDeque.cons "g" |> BatchedDeque.cons "h" |> BatchedDeque.cons "i" |> BatchedDeque.cons "j"

    let len2snoc = BatchedDeque.singleton "b" |> BatchedDeque.snoc "a"
    let len3snoc = BatchedDeque.singleton "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len4snoc = BatchedDeque.singleton "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len5snoc = BatchedDeque.singleton "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len6snoc = BatchedDeque.singleton "f" |> BatchedDeque.snoc "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len7snoc = BatchedDeque.singleton "g" |> BatchedDeque.snoc "f" |> BatchedDeque.snoc "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len8snoc = BatchedDeque.singleton "h" |> BatchedDeque.snoc "g" |> BatchedDeque.snoc "f" |> BatchedDeque.snoc "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let len9snoc = BatchedDeque.singleton "i" |> BatchedDeque.snoc "h" |> BatchedDeque.snoc "g" |> BatchedDeque.snoc "f" |> BatchedDeque.snoc "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"
    let lenasnoc = BatchedDeque.singleton "j" |> BatchedDeque.snoc "i" |> BatchedDeque.snoc "h" |> BatchedDeque.snoc "g" |> BatchedDeque.snoc "f" |> BatchedDeque.snoc "e" |> BatchedDeque.snoc "d" |> BatchedDeque.snoc "c" |> BatchedDeque.snoc "b" |> BatchedDeque.snoc "a"

    [<Tests>]
    let testBatchDeque =

        testList "Experimental BatchDeque" [

            test "empty dqueue should be empty" {
                BatchedDeque.empty() |> BatchedDeque.isEmpty |> Expect.isTrue "" }

            test "BatchedDeque.cons works" {
                len2 |> BatchedDeque.isEmpty |> Expect.isFalse "" }

            test "BatchedDeque.snoc works" {
                len2snoc |> BatchedDeque.isEmpty |> Expect.isFalse "" }

            test "BatchedDeque.singleton BatchedDeque.head works" {
                len1 |> BatchedDeque.head |> Expect.equal "" "a" } 

            test "BatchedDeque.singleton BatchedDeque.last works" {
                len1 |> BatchedDeque.last |> Expect.equal "" "a" } 

            test "BatchedDeque.tail of BatchedDeque.singleton empty" {
                len1 |> BatchedDeque.tail |> BatchedDeque.isEmpty |> Expect.isTrue "" }

            test "BatchedDeque.tail of BatchedDeque.tail of 2 empty" {
                 ( len2 |> BatchedDeque.tail |> BatchedDeque.tail |> BatchedDeque.isEmpty) |> Expect.isTrue "" }

            test "BatchedDeque.init of BatchedDeque.singleton empty" {
                len1 |> BatchedDeque.init |> BatchedDeque.isEmpty |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 1" {
                let t1 = BatchedDeque.tail len2
                let t1s = BatchedDeque.tail len2snoc
                (((BatchedDeque.length t1) = 1) && ((BatchedDeque.length t1s) = 1) && ((BatchedDeque.head t1) = "a") && ((BatchedDeque.head t1s) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 2" {
                let t1 = BatchedDeque.tail len3
                let t1s = BatchedDeque.tail len3snoc

                let t1_1 = BatchedDeque.tail t1
                let t1_1s = BatchedDeque.tail t1s

                (((BatchedDeque.length t1) = 2) && ((BatchedDeque.length t1s) = 2) && ((BatchedDeque.head t1) = "b") && ((BatchedDeque.head t1s) = "b") && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1) 
                && ((BatchedDeque.head t1_1) = "a") && ((BatchedDeque.head t1_1s) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 3" {
                let t1 = BatchedDeque.tail len4
                let t1s = BatchedDeque.tail len4snoc

                let t1_2 = BatchedDeque.tail t1
                let t1_2s = BatchedDeque.tail t1s

                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 3) && ((BatchedDeque.length t1s) = 3) && ((BatchedDeque.head t1) = "c")&& ((BatchedDeque.head t1s) = "c") && ((BatchedDeque.length t1_2) = 2) && ((BatchedDeque.length t1_2s) = 2)
                && ((BatchedDeque.head t1_2) = "b") && ((BatchedDeque.head t1_2s) = "b") && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1) && ((BatchedDeque.head t1_1) = "a") 
                && ((BatchedDeque.head t1_1s) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 4" {
                let t1 = BatchedDeque.tail len5
                let t1s = BatchedDeque.tail len5snoc

                let t1_3 = BatchedDeque.tail t1
                let t1_3s = BatchedDeque.tail t1s

                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s

                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 4) && ((BatchedDeque.length t1s) = 4) && ((BatchedDeque.head t1) = "d") && ((BatchedDeque.head t1s) = "d") && ((BatchedDeque.length t1_3) = 3) && ((BatchedDeque.length t1_3s) = 3)
                && ((BatchedDeque.head t1_3) = "c") && ((BatchedDeque.head t1_3s) = "c") && ((BatchedDeque.length t1_2) = 2) && ((BatchedDeque.length t1_2s) = 2) && ((BatchedDeque.head t1_2) = "b") && ((BatchedDeque.head t1_2s) = "b")
                && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1) && ((BatchedDeque.head t1_1) = "a") && ((BatchedDeque.head t1_1s) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 5" {
                let t1 = BatchedDeque.tail len6
                let t1s = BatchedDeque.tail len6snoc

                let t1_4 = BatchedDeque.tail t1
                let t1_4s = BatchedDeque.tail t1s

                let t1_3 = BatchedDeque.tail t1_4
                let t1_3s = BatchedDeque.tail t1_4s

                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s

                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 5) && ((BatchedDeque.length t1s) = 5) && ((BatchedDeque.head t1) = "e") && ((BatchedDeque.head t1s) = "e") && ((BatchedDeque.length t1_4) = 4) && ((BatchedDeque.length t1_4s) = 4) 
                && ((BatchedDeque.head t1_4) = "d") && ((BatchedDeque.head t1_4s) = "d") && ((BatchedDeque.length t1_3) = 3) && ((BatchedDeque.length t1_3s) = 3) && ((BatchedDeque.head t1_3) = "c") && ((BatchedDeque.head t1_3s) = "c") 
                && ((BatchedDeque.length t1_2) = 2) && ((BatchedDeque.length t1_2s) = 2) && ((BatchedDeque.head t1_2) = "b") && ((BatchedDeque.head t1_2s) = "b") && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1)
                && ((BatchedDeque.head t1_1) = "a") && ((BatchedDeque.head t1_1s) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 6" {
                let t1 = BatchedDeque.tail len7
                let t1s = BatchedDeque.tail len7snoc

                let t1_5 = BatchedDeque.tail t1
                let t1_5s = BatchedDeque.tail t1s

                let t1_4 = BatchedDeque.tail t1_5
                let t1_4s = BatchedDeque.tail t1_5s

                let t1_3 = BatchedDeque.tail t1_4
                let t1_3s = BatchedDeque.tail t1_4s

                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s

                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 6) && ((BatchedDeque.length t1s) = 6)  
                && ((BatchedDeque.head t1) = "f") && ((BatchedDeque.head t1s) = "f") 
                && ((BatchedDeque.length t1_5) = 5) && ((BatchedDeque.length t1_5s) = 5) 
                && ((BatchedDeque.head t1_5) = "e") && ((BatchedDeque.head t1_5s) = "e") 
                && ((BatchedDeque.length t1_4) = 4) && ((BatchedDeque.length t1_4s) = 4) 
                && ((BatchedDeque.head t1_4) = "d") && ((BatchedDeque.head t1_4s) = "d") 
                && ((BatchedDeque.length t1_3) = 3) && ((BatchedDeque.length t1_3s) = 3) 
                && ((BatchedDeque.head t1_3) = "c") && ((BatchedDeque.head t1_3s) = "c") 
                && ((BatchedDeque.length t1_2) = 2) && ((BatchedDeque.length t1_2s) = 2) 
                && ((BatchedDeque.head t1_2) = "b") && ((BatchedDeque.head t1_2s) = "b") 
                && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1) 
                && ((BatchedDeque.head t1_1) = "a") && ((BatchedDeque.head t1_1s) = "a") ) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 7" {
                let t1 = BatchedDeque.tail len8
                let t1s = BatchedDeque.tail len8snoc
                let t1_6 = BatchedDeque.tail t1
                let t1_6s = BatchedDeque.tail t1s
                let t1_5 = BatchedDeque.tail t1_6
                let t1_5s = BatchedDeque.tail t1_6s
                let t1_4 = BatchedDeque.tail t1_5
                let t1_4s = BatchedDeque.tail t1_5s
                let t1_3 = BatchedDeque.tail t1_4
                let t1_3s = BatchedDeque.tail t1_4s
                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s
                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 7) && ((BatchedDeque.length t1s) = 7) 
                && ((BatchedDeque.head t1) = "g") && ((BatchedDeque.head t1s) = "g") 
                && ((BatchedDeque.length t1_6) = 6)  && ((BatchedDeque.length t1_6s) = 6) 
                && ((BatchedDeque.head t1_6) = "f")  && ((BatchedDeque.head t1_6s) = "f") 
                && ((BatchedDeque.length t1_5) = 5)  && ((BatchedDeque.length t1_5s) = 5) 
                && ((BatchedDeque.head t1_5) = "e")  && ((BatchedDeque.head t1_5s) = "e") 
                && ((BatchedDeque.length t1_4) = 4)  && ((BatchedDeque.length t1_4s) = 4) 
                && ((BatchedDeque.head t1_4) = "d")  && ((BatchedDeque.head t1_4s) = "d") 
                && ((BatchedDeque.length t1_3) = 3)  && ((BatchedDeque.length t1_3s) = 3) 
                && ((BatchedDeque.head t1_3) = "c")  && ((BatchedDeque.head t1_3s) = "c") 
                && ((BatchedDeque.length t1_2) = 2)  && ((BatchedDeque.length t1_2s) = 2) 
                && ((BatchedDeque.head t1_2) = "b")  && ((BatchedDeque.head t1_2s) = "b") 
                && ((BatchedDeque.length t1_1) = 1)  && ((BatchedDeque.length t1_1s) = 1) 
                && ((BatchedDeque.head t1_1) = "a")  && ((BatchedDeque.head t1_1s) = "a") ) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 8" {
                let t1 = BatchedDeque.tail len9
                let t1s = BatchedDeque.tail len9snoc
                let t1_7 = BatchedDeque.tail t1
                let t1_7s = BatchedDeque.tail t1s  
                let t1_6 = BatchedDeque.tail t1_7
                let t1_6s = BatchedDeque.tail t1_7s
                let t1_5 = BatchedDeque.tail t1_6
                let t1_5s = BatchedDeque.tail t1_6s
                let t1_4 = BatchedDeque.tail t1_5
                let t1_4s = BatchedDeque.tail t1_5s
                let t1_3 = BatchedDeque.tail t1_4
                let t1_3s = BatchedDeque.tail t1_4s
                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s
                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s

                (((BatchedDeque.length t1) = 8) && ((BatchedDeque.length t1s) = 8) 
                && ((BatchedDeque.head t1) = "h") && ((BatchedDeque.head t1s) = "h") 
                && ((BatchedDeque.length t1_7) = 7)  && ((BatchedDeque.length t1_7s) = 7) 
                && ((BatchedDeque.head t1_7) = "g")  && ((BatchedDeque.head t1_7s) = "g") 
                && ((BatchedDeque.length t1_6) = 6)  && ((BatchedDeque.length t1_6s) = 6) 
                && ((BatchedDeque.head t1_6) = "f")  && ((BatchedDeque.head t1_6s) = "f") 
                && ((BatchedDeque.length t1_5) = 5)  && ((BatchedDeque.length t1_5s) = 5) 
                && ((BatchedDeque.head t1_5) = "e")  && ((BatchedDeque.head t1_5s) = "e") 
                && ((BatchedDeque.length t1_4) = 4)  && ((BatchedDeque.length t1_4s) = 4) 
                && ((BatchedDeque.head t1_4) = "d")  && ((BatchedDeque.head t1_4s) = "d") 
                && ((BatchedDeque.length t1_3) = 3)  && ((BatchedDeque.length t1_3s) = 3) 
                && ((BatchedDeque.head t1_3) = "c")  && ((BatchedDeque.head t1_3s) = "c") 
                && ((BatchedDeque.length t1_2) = 2)  && ((BatchedDeque.length t1_2s) = 2) 
                && ((BatchedDeque.head t1_2) = "b")  && ((BatchedDeque.head t1_2s) = "b") 
                && ((BatchedDeque.length t1_1) = 1)  && ((BatchedDeque.length t1_1s) = 1) 
                && ((BatchedDeque.head t1_1) = "a")  && ((BatchedDeque.head t1_1s) = "a") ) |> Expect.isTrue "" }

            test "BatchedDeque.head, BatchedDeque.tail, and BatchedDeque.length work test 9" {
                let t1 = BatchedDeque.tail lena
                let t1s = BatchedDeque.tail lenasnoc
                let t1_8 = BatchedDeque.tail t1
                let t1_8s = BatchedDeque.tail t1s
                let t1_7 = BatchedDeque.tail t1_8
                let t1_7s = BatchedDeque.tail t1_8s
                let t1_6 = BatchedDeque.tail t1_7
                let t1_6s = BatchedDeque.tail t1_7s
                let t1_5 = BatchedDeque.tail t1_6
                let t1_5s = BatchedDeque.tail t1_6s
                let t1_4 = BatchedDeque.tail t1_5
                let t1_4s = BatchedDeque.tail t1_5s
                let t1_3 = BatchedDeque.tail t1_4
                let t1_3s = BatchedDeque.tail t1_4s
                let t1_2 = BatchedDeque.tail t1_3
                let t1_2s = BatchedDeque.tail t1_3s
                let t1_1 = BatchedDeque.tail t1_2
                let t1_1s = BatchedDeque.tail t1_2s
    
                (((BatchedDeque.length t1) = 9) && ((BatchedDeque.length t1s) = 9) && ((BatchedDeque.head t1) = "i")  && ((BatchedDeque.head t1s) = "i") 
                && ((BatchedDeque.length t1_8) = 8) && ((BatchedDeque.length t1_8s) = 8) && ((BatchedDeque.head t1_8) = "h") && ((BatchedDeque.head t1_8s) = "h") 
                && ((BatchedDeque.length t1_7) = 7) && ((BatchedDeque.length t1_7s) = 7) && ((BatchedDeque.head t1_7) = "g") && ((BatchedDeque.head t1_7s) = "g") 
                && ((BatchedDeque.length t1_6) = 6) && ((BatchedDeque.length t1_6s) = 6) && ((BatchedDeque.head t1_6) = "f") && ((BatchedDeque.head t1_6s) = "f") 
                && ((BatchedDeque.length t1_5) = 5) && ((BatchedDeque.length t1_5s) = 5) && ((BatchedDeque.head t1_5) = "e") && ((BatchedDeque.head t1_5s) = "e") 
                && ((BatchedDeque.length t1_4) = 4) && ((BatchedDeque.length t1_4s) = 4) && ((BatchedDeque.head t1_4) = "d") && ((BatchedDeque.head t1_4s) = "d") 
                && ((BatchedDeque.length t1_3) = 3) && ((BatchedDeque.length t1_3s) = 3) && ((BatchedDeque.head t1_3) = "c") && ((BatchedDeque.head t1_3s) = "c") 
                && ((BatchedDeque.length t1_2) = 2) && ((BatchedDeque.length t1_2s) = 2) && ((BatchedDeque.head t1_2) = "b") && ((BatchedDeque.head t1_2s) = "b") 
                && ((BatchedDeque.length t1_1) = 1) && ((BatchedDeque.length t1_1s) = 1) && ((BatchedDeque.head t1_1) = "a") && ((BatchedDeque.head t1_1s) = "a")) |> Expect.isTrue "" }

            //the previous series thoroughly tested construction by BatchedDeque.snoc, so we'll leave those out
            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 1" {  
                let t1 = BatchedDeque.init len2
    
                (((BatchedDeque.length t1) = 1) && ((BatchedDeque.last t1) = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 2" {
                let t1 = BatchedDeque.init len3
                let t1_1 = BatchedDeque.init t1
    
                (((BatchedDeque.length t1) = 2) && ((BatchedDeque.last t1) = "b") && ((BatchedDeque.length t1_1) = 1)  && ((BatchedDeque.last t1_1) = "c") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 3" {
                let t1 = BatchedDeque.init len4
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
    
                (((BatchedDeque.length t1) = 3) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 2)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 1)  && ((BatchedDeque.last t1_2) = "d") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 4" {
                let t1 = BatchedDeque.init len5
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
    
                (((BatchedDeque.length t1) = 4) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 3)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 2)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 1)  && ((BatchedDeque.last t1_3) = "e") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 5" {
                let t1 = BatchedDeque.init len6
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
                let t1_4 = BatchedDeque.init t1_3
    
                (((BatchedDeque.length t1) = 5) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 4)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 3)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 2)  && ((BatchedDeque.last t1_3) = "e") 
                && ((BatchedDeque.length t1_4) = 1)  && ((BatchedDeque.last t1_4) = "f") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 6" {
                let t1 = BatchedDeque.init len7
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
                let t1_4 = BatchedDeque.init t1_3
                let t1_5 = BatchedDeque.init t1_4
    
                (((BatchedDeque.length t1) = 6) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 5)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 4)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 3)  && ((BatchedDeque.last t1_3) = "e") 
                && ((BatchedDeque.length t1_4) = 2)  && ((BatchedDeque.last t1_4) = "f") 
                && ((BatchedDeque.length t1_5) = 1)  && ((BatchedDeque.last t1_5) = "g") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 7" {
                let t1 = BatchedDeque.init len8
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
                let t1_4 = BatchedDeque.init t1_3
                let t1_5 = BatchedDeque.init t1_4
                let t1_6 = BatchedDeque.init t1_5
    
                (((BatchedDeque.length t1) = 7) && ((BatchedDeque.last t1) = "b") 
                && ((BatchedDeque.length t1_1) = 6)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 5)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 4)  && ((BatchedDeque.last t1_3) = "e") 
                && ((BatchedDeque.length t1_4) = 3)  && ((BatchedDeque.last t1_4) = "f") 
                && ((BatchedDeque.length t1_5) = 2)  && ((BatchedDeque.last t1_5) = "g") 
                && ((BatchedDeque.length t1_6) = 1)  && ((BatchedDeque.last t1_6) = "h") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 8" {
                let t1 = BatchedDeque.init len9
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
                let t1_4 = BatchedDeque.init t1_3
                let t1_5 = BatchedDeque.init t1_4
                let t1_6 = BatchedDeque.init t1_5
                let t1_7 = BatchedDeque.init t1_6
    
                (((BatchedDeque.length t1) = 8) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 7)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 6)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 5)  && ((BatchedDeque.last t1_3) = "e") 
                && ((BatchedDeque.length t1_4) = 4)  && ((BatchedDeque.last t1_4) = "f") 
                && ((BatchedDeque.length t1_5) = 3)  && ((BatchedDeque.last t1_5) = "g") 
                && ((BatchedDeque.length t1_6) = 2)  && ((BatchedDeque.last t1_6) = "h") 
                && ((BatchedDeque.length t1_7) = 1)  && ((BatchedDeque.last t1_7) = "i") ) |> Expect.isTrue "" }

            test "BatchedDeque.last, BatchedDeque.init, and BatchedDeque.length work test 9" {
                let t1 = BatchedDeque.init lena
                let t1_1 = BatchedDeque.init t1
                let t1_2 = BatchedDeque.init t1_1
                let t1_3 = BatchedDeque.init t1_2
                let t1_4 = BatchedDeque.init t1_3
                let t1_5 = BatchedDeque.init t1_4
                let t1_6 = BatchedDeque.init t1_5
                let t1_7 = BatchedDeque.init t1_6
                let t1_8 = BatchedDeque.init t1_7
    
                (((BatchedDeque.length t1) = 9) && ((BatchedDeque.last t1) = "b")
                && ((BatchedDeque.length t1_1) = 8)  && ((BatchedDeque.last t1_1) = "c") 
                && ((BatchedDeque.length t1_2) = 7)  && ((BatchedDeque.last t1_2) = "d") 
                && ((BatchedDeque.length t1_3) = 6)  && ((BatchedDeque.last t1_3) = "e") 
                && ((BatchedDeque.length t1_4) = 5)  && ((BatchedDeque.last t1_4) = "f") 
                && ((BatchedDeque.length t1_5) = 4)  && ((BatchedDeque.last t1_5) = "g") 
                && ((BatchedDeque.length t1_6) = 3)  && ((BatchedDeque.last t1_6) = "h") 
                && ((BatchedDeque.length t1_7) = 2)  && ((BatchedDeque.last t1_7) = "i") 
                && ((BatchedDeque.length t1_8) = 1)  && ((BatchedDeque.last t1_8) = "j") ) |> Expect.isTrue "" }

            test "IEnumerable Seq" {
                (lena |> Seq.toArray).[5] |> Expect.equal "" "e" } 

            test "IEnumerable Seq BatchedDeque.length" {
                lena |> Seq.length |> Expect.equal "" 10 }

            test "type BatchedDeque.cons works" {
                lena.Cons "zz" |> BatchedDeque.head |> Expect.equal "" "zz" } 

            test "IDeque BatchedDeque.cons works" {
                ((lena :> IDeque<string>).Cons "zz").Head |> Expect.equal "" "zz" } 

            test "BatchedDeque.ofCatLists and BatchedDeque.uncons" {
                let d = BatchedDeque.ofCatLists ["a";"b";"c"] ["d";"e";"f"]
                let h1, t1 = BatchedDeque.uncons d
                let h2, t2 = BatchedDeque.uncons t1
                let h3, t3 = BatchedDeque.uncons t2
                let h4, t4 = BatchedDeque.uncons t3
                let h5, t5 = BatchedDeque.uncons t4
                let h6, t6 = BatchedDeque.uncons t5

                ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e") && (h6 = "f") && (BatchedDeque.isEmpty t6)) |> Expect.isTrue "" }

            test "BatchedDeque.unsnoc works" {
                let d = BatchedDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]
                let i1, l1 = BatchedDeque.unsnoc d
                let i2, l2 = BatchedDeque.unsnoc i1
                let i3, l3 = BatchedDeque.unsnoc i2
                let i4, l4 = BatchedDeque.unsnoc i3
                let i5, l5 = BatchedDeque.unsnoc i4
                let i6, l6 = BatchedDeque.unsnoc i5

                ((l1 = "a") && (l2 = "b") && (l3 = "c") && (l4 = "d") && (l5 = "e") && (l6 = "f") && (BatchedDeque.isEmpty i6)) |> Expect.isTrue "" }

            test "BatchedDeque.snoc pattern discriminator" {
                let d = (BatchedDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let i1, l1 = BatchedDeque.unsnoc d 

                let i2, l2 = 
                    match i1 with
                    | BatchedDeque.Snoc(i, l) -> i, l
                    | _ -> i1, "x"

                ((l2 = "b") && ((BatchedDeque.length i2) = 4)) |> Expect.isTrue "" }

            test "BatchedDeque.cons pattern discriminator" {
                let d = (BatchedDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let h1, t1 = BatchedDeque.uncons d 

                let h2, t2 = 
                    match t1 with
                    | BatchedDeque.Cons(h, t) -> h, t
                    | _ ->  "x", t1

                ((h2 = "e") && ((BatchedDeque.length t2) = 4)) |> Expect.isTrue "" }

            test "BatchedDeque.cons and BatchedDeque.snoc pattern discriminator" {
                let d = (BatchedDeque.ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
    
                let mid1 = 
                    match d with
                    | BatchedDeque.Cons(h, BatchedDeque.Snoc(i, l)) -> i
                    | _ -> d

                let head, last = 
                    match mid1 with
                    | BatchedDeque.Cons(h, BatchedDeque.Snoc(i, l)) -> h, l
                    | _ -> "x", "x"

                ((head = "e") && (last = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.rev dqueue BatchedDeque.length 1" {
                BatchedDeque.rev len1 |> BatchedDeque.head  |> Expect.equal "" "a" } 

            test "BatchedDeque.rev dqueue BatchedDeque.length 2" {
                let r1 = BatchedDeque.rev len2
                let h1 = BatchedDeque.head r1
                let t2 = BatchedDeque.tail r1
                let h2 = BatchedDeque.head t2

                ((h1 = "a")  && (h2 = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.rev dqueue BatchedDeque.length 3" {
                let r1 = BatchedDeque.rev len3
                let h1 = BatchedDeque.head r1
                let t2 = BatchedDeque.tail r1
                let h2 = BatchedDeque.head t2
                let t3 = BatchedDeque.tail t2
                let h3 = BatchedDeque.head t3

                ((h1 = "a") && (h2 = "b") && (h3 = "c")) |> Expect.isTrue "" }

            test "BatchedDeque.rev dqueue BatchedDeque.length 4" {
                let r1 = BatchedDeque.rev len4
                let h1 = BatchedDeque.head r1
                let t2 = BatchedDeque.tail r1
                let h2 = BatchedDeque.head t2
                let t3 = BatchedDeque.tail t2
                let h3 = BatchedDeque.head t3
                let t4 = BatchedDeque.tail t3
                let h4 = BatchedDeque.head t4

                ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d")) |> Expect.isTrue "" }

            test "BatchedDeque.rev dqueue BatchedDeque.length 5" {
                let r1 = BatchedDeque.rev len5
                let h1 = BatchedDeque.head r1
                let t2 = BatchedDeque.tail r1
                let h2 = BatchedDeque.head t2
                let t3 = BatchedDeque.tail t2
                let h3 = BatchedDeque.head t3
                let t4 = BatchedDeque.tail t3
                let h4 = BatchedDeque.head t4
                let t5 = BatchedDeque.tail t4
                let h5 = BatchedDeque.head t5

                ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e")) |> Expect.isTrue "" }

            //BatchedDeque.length 6 more than sufficient to test BatchedDeque.rev
            test "BatchedDeque.rev dqueue BatchedDeque.length 6" {
                let r1 = BatchedDeque.rev len6
                let h1 = BatchedDeque.head r1
                let t2 = BatchedDeque.tail r1
                let h2 = BatchedDeque.head t2
                let t3 = BatchedDeque.tail t2
                let h3 = BatchedDeque.head t3
                let t4 = BatchedDeque.tail t3
                let h4 = BatchedDeque.head t4
                let t5 = BatchedDeque.tail t4
                let h5 = BatchedDeque.head t5
                let t6 = BatchedDeque.tail t5
                let h6 = BatchedDeque.head t6

                ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e") && (h6 = "f")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 1" {
                len1 |> BatchedDeque.lookup 0 |> Expect.equal "" "a" } 

            test "BatchedDeque.lookup BatchedDeque.length 2" {
                (((len2 |> BatchedDeque.lookup 0) = "b") && ((len2 |> BatchedDeque.lookup 1) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 3" {
                (((len3 |> BatchedDeque.lookup 0) = "c") && ((len3 |> BatchedDeque.lookup 1) = "b") && ((len3 |> BatchedDeque.lookup 2) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 4" {
                (((len4 |> BatchedDeque.lookup 0) = "d") && ((len4 |> BatchedDeque.lookup 1) = "c") && ((len4 |> BatchedDeque.lookup 2) = "b") && ((len4 |> BatchedDeque.lookup 3) = "a")) 
                |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 5" {
                (((len5 |> BatchedDeque.lookup 0) = "e") && ((len5 |> BatchedDeque.lookup 1) = "d") && ((len5 |> BatchedDeque.lookup 2) = "c") && ((len5 |> BatchedDeque.lookup 3) = "b") 
                && ((len5 |> BatchedDeque.lookup 4) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 6" {
                (((len6 |> BatchedDeque.lookup 0) = "f") && ((len6 |> BatchedDeque.lookup 1) = "e") && ((len6 |> BatchedDeque.lookup 2) = "d") && ((len6 |> BatchedDeque.lookup 3) = "c") 
                && ((len6 |> BatchedDeque.lookup 4) = "b") && ((len6 |> BatchedDeque.lookup 5) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 7" {
                (((len7 |> BatchedDeque.lookup 0) = "g") && ((len7 |> BatchedDeque.lookup 1) = "f") && ((len7 |> BatchedDeque.lookup 2) = "e") && ((len7 |> BatchedDeque.lookup 3) = "d") 
                && ((len7 |> BatchedDeque.lookup 4) = "c") && ((len7 |> BatchedDeque.lookup 5) = "b") && ((len7 |> BatchedDeque.lookup 6) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 8" {
                (((len8 |> BatchedDeque.lookup 0) = "h") && ((len8 |> BatchedDeque.lookup 1) = "g") && ((len8 |> BatchedDeque.lookup 2) = "f") && ((len8 |> BatchedDeque.lookup 3) = "e") 
                && ((len8 |> BatchedDeque.lookup 4) = "d") && ((len8 |> BatchedDeque.lookup 5) = "c") && ((len8 |> BatchedDeque.lookup 6) = "b") && ((len8 |> BatchedDeque.lookup 7) = "a")) 
                |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 9" {
                (((len9 |> BatchedDeque.lookup 0) = "i") && ((len9 |> BatchedDeque.lookup 1) = "h") && ((len9 |> BatchedDeque.lookup 2) = "g") && ((len9 |> BatchedDeque.lookup 3) = "f") 
                && ((len9 |> BatchedDeque.lookup 4) = "e") && ((len9 |> BatchedDeque.lookup 5) = "d") && ((len9 |> BatchedDeque.lookup 6) = "c") && ((len9 |> BatchedDeque.lookup 7) = "b")
                && ((len9 |> BatchedDeque.lookup 8) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.lookup BatchedDeque.length 10" {
                (((lena |> BatchedDeque.lookup 0) = "j") && ((lena |> BatchedDeque.lookup 1) = "i") && ((lena |> BatchedDeque.lookup 2) = "h") && ((lena |> BatchedDeque.lookup 3) = "g") 
                && ((lena |> BatchedDeque.lookup 4) = "f") && ((lena |> BatchedDeque.lookup 5) = "e") && ((lena |> BatchedDeque.lookup 6) = "d") && ((lena |> BatchedDeque.lookup 7) = "c")
                && ((lena |> BatchedDeque.lookup 8) = "b") && ((lena |> BatchedDeque.lookup 9) = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 1" {
                let a = len1 |> BatchedDeque.tryLookup 0 
                (a.Value = "a") |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 2" {
                let b = len2 |> BatchedDeque.tryLookup 0
                let a = len2 |> BatchedDeque.tryLookup 1
                ((b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 3" {
                let c = len3 |> BatchedDeque.tryLookup 0
                let b = len3 |> BatchedDeque.tryLookup 1
                let a = len3 |> BatchedDeque.tryLookup 2
                ((c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 4" {
                let d = len4 |> BatchedDeque.tryLookup 0
                let c = len4 |> BatchedDeque.tryLookup 1
                let b = len4 |> BatchedDeque.tryLookup 2
                let a = len4 |> BatchedDeque.tryLookup 3
                ((d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" } 

            test "BatchedDeque.tryLookup BatchedDeque.length 5" {
                let e = len5 |> BatchedDeque.tryLookup 0
                let d = len5 |> BatchedDeque.tryLookup 1
                let c = len5 |> BatchedDeque.tryLookup 2
                let b = len5 |> BatchedDeque.tryLookup 3
                let a = len5 |> BatchedDeque.tryLookup 4
                ((e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 6" {
                let f = len6 |> BatchedDeque.tryLookup 0
                let e = len6 |> BatchedDeque.tryLookup 1
                let d = len6 |> BatchedDeque.tryLookup 2
                let c = len6 |> BatchedDeque.tryLookup 3
                let b = len6 |> BatchedDeque.tryLookup 4
                let a = len6 |> BatchedDeque.tryLookup 5
                ((f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) 
                |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 7" {
                let g = len7 |> BatchedDeque.tryLookup 0
                let f = len7 |> BatchedDeque.tryLookup 1
                let e = len7 |> BatchedDeque.tryLookup 2
                let d = len7 |> BatchedDeque.tryLookup 3
                let c = len7 |> BatchedDeque.tryLookup 4
                let b = len7 |> BatchedDeque.tryLookup 5
                let a = len7 |> BatchedDeque.tryLookup 6
                ((g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") 
                && (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 8" {
                let h = len8 |> BatchedDeque.tryLookup 0
                let g = len8 |> BatchedDeque.tryLookup 1
                let f = len8 |> BatchedDeque.tryLookup 2
                let e = len8 |> BatchedDeque.tryLookup 3
                let d = len8 |> BatchedDeque.tryLookup 4
                let c = len8 |> BatchedDeque.tryLookup 5
                let b = len8 |> BatchedDeque.tryLookup 6
                let a = len8 |> BatchedDeque.tryLookup 7
                ((h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c")  
                && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 9" {
                let i = len9 |> BatchedDeque.tryLookup 0
                let h = len9 |> BatchedDeque.tryLookup 1
                let g = len9 |> BatchedDeque.tryLookup 2
                let f = len9 |> BatchedDeque.tryLookup 3
                let e = len9 |> BatchedDeque.tryLookup 4
                let d = len9 |> BatchedDeque.tryLookup 5
                let c = len9 |> BatchedDeque.tryLookup 6
                let b = len9 |> BatchedDeque.tryLookup 7
                let a = len9 |> BatchedDeque.tryLookup 8
                ((i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") 
                && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup BatchedDeque.length 10" {
                let j = lena |> BatchedDeque.tryLookup 0
                let i = lena |> BatchedDeque.tryLookup 1
                let h = lena |> BatchedDeque.tryLookup 2
                let g = lena |> BatchedDeque.tryLookup 3
                let f = lena |> BatchedDeque.tryLookup 4
                let e = lena |> BatchedDeque.tryLookup 5
                let d = lena |> BatchedDeque.tryLookup 6
                let c = lena |> BatchedDeque.tryLookup 7
                let b = lena |> BatchedDeque.tryLookup 8
                let a = lena |> BatchedDeque.tryLookup 9
                ((j.Value = "j") && (i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") 
                && (d.Value = "d") && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "" }

            test "BatchedDeque.tryLookup not found" {
                lena |> BatchedDeque.tryLookup 10 |> Expect.isNone "" }

            test "BatchedDeque.remove elements BatchedDeque.length 1" {
                len1 |> BatchedDeque.remove 0 |> BatchedDeque.isEmpty |> Expect.isTrue "" }

            test "BatchedDeque.remove elements BatchedDeque.length 2" {
                let a = len2 |> BatchedDeque.remove 0 |> BatchedDeque.head 
                let b = len2 |> BatchedDeque.remove 1 |> BatchedDeque.head
                ((a = "a") && (b = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.remove elements BatchedDeque.length 3" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.remove 0
                let b0 = BatchedDeque.head r0
                let t0 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t0

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.remove 1
                let a1 = BatchedDeque.head r1
                let t1 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t1

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.remove 2
                let a2 = BatchedDeque.head r2
                let t2 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t2

                ((b0 = "b") && (c0 = "c") && (a1 = "a") && (c1 = "c") && (a2 = "a") && (b2 = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.remove elements BatchedDeque.length 4" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.remove 0
                let b0 = BatchedDeque.head r0
                let t0 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t0
                let t01 = BatchedDeque.tail t0
                let d0 = BatchedDeque.head t01

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.remove 1
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.remove 2
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.remove 3
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32

                ((b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (c1 = "c") && (d1 = "d")
                && (a2 = "a") && (b2 = "b") && (c2 = "d")
                && (a3 = "a") && (b3 = "b") && (c3 = "c")) |> Expect.isTrue "" }

            test "BatchedDeque.remove elements BatchedDeque.length 5" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.remove 0
                let b0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t01
                let t02= BatchedDeque.tail t01
                let d0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let e0 = BatchedDeque.head t03

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.remove 1
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let e1 = BatchedDeque.head t13

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.remove 2
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let e2 = BatchedDeque.head t23

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.remove 3
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let e3 = BatchedDeque.head t33

                let r4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.remove 4
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")) |> Expect.isTrue "" }

            test "BatchedDeque.remove elements BatchedDeque.length 6" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 0
                let b0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t01
                let t02= BatchedDeque.tail t01
                let d0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let e0 = BatchedDeque.head t03
                let t04 = BatchedDeque.tail t03
                let f0 = BatchedDeque.head t04

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 1
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let e1 = BatchedDeque.head t13
                let t14 = BatchedDeque.tail t13
                let f1 = BatchedDeque.head t14

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 2
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let e2 = BatchedDeque.head t23
                let t24 = BatchedDeque.tail t23
                let f2 = BatchedDeque.head t24

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 3
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let e3 = BatchedDeque.head t33
                let t34 = BatchedDeque.tail t33
                let f3 = BatchedDeque.head t34

                let r4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 4
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43
                let t44 = BatchedDeque.tail t43
                let f4 = BatchedDeque.head t44

                let r5 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.remove 5
                let a5 = BatchedDeque.head r5
                let t51 = BatchedDeque.tail r5
                let b5 = BatchedDeque.head t51
                let t52 = BatchedDeque.tail t51
                let c5 = BatchedDeque.head t52
                let t53 = BatchedDeque.tail t52
                let d5 = BatchedDeque.head t53
                let t54 = BatchedDeque.tail t53
                let e5 = BatchedDeque.head t54

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d") && (e5 = "e")) |> Expect.isTrue "" }

            test "tryRemoveempty" {
                BatchedDeque.empty() |>BatchedDeque.tryRemove 0 |> Expect.isNone "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 1" {
                let a = len1 |> BatchedDeque.tryRemove 0 
                a.Value |> BatchedDeque.isEmpty |> Expect.isTrue "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 2" {
                let a = len2 |> BatchedDeque.tryRemove 0 
                let a1 =  BatchedDeque.head a.Value
                let b = len2 |> BatchedDeque.tryRemove 1 
                let b1 = BatchedDeque.head b.Value
                ((a1 = "a") && (b1 = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 3" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BatchedDeque.head r0
                let t0 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t0

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t1 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t1

                let x2 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BatchedDeque.head r2
                let t2 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t2

                ((b0 = "b") && (c0 = "c") && (a1 = "a") && (c1 = "c") && (a2 = "a") && (b2 = "b")) |> Expect.isTrue "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 4" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BatchedDeque.head r0
                let t0 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t0
                let t01 = BatchedDeque.tail t0
                let d0 = BatchedDeque.head t01

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12
 
                let x2 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
     
                let x3 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32

                ((b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (c1 = "c") && (d1 = "d")
                && (a2 = "a") && (b2 = "b") && (c2 = "d")
                && (a3 = "a") && (b3 = "b") && (c3 = "c")) |> Expect.isTrue "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 5" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t01
                let t02= BatchedDeque.tail t01
                let d0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let e0 = BatchedDeque.head t03

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.tryRemove 1
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let e1 = BatchedDeque.head t13

                let x2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.tryRemove 2
                let r2 = x2.Value
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let e2 = BatchedDeque.head t23

                let x3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let e3 = BatchedDeque.head t33

                let x4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.tryRemove 4
                let r4 = x4.Value
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")) |> Expect.isTrue "" }

            test "BatchedDeque.tryRemove elements BatchedDeque.length 6" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 0
                let r0 = x0.Value
                let b0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let c0 = BatchedDeque.head t01
                let t02= BatchedDeque.tail t01
                let d0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let e0 = BatchedDeque.head t03
                let t04 = BatchedDeque.tail t03
                let f0 = BatchedDeque.head t04

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 1
                let r1 =x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let c1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let d1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let e1 = BatchedDeque.head t13
                let t14 = BatchedDeque.tail t13
                let f1 = BatchedDeque.head t14

                let x2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 2
                let r2 = x2.Value 
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let e2 = BatchedDeque.head t23
                let t24 = BatchedDeque.tail t23
                let f2 = BatchedDeque.head t24

                let x3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 3
                let r3 = x3.Value
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let e3 = BatchedDeque.head t33
                let t34 = BatchedDeque.tail t33
                let f3 = BatchedDeque.head t34

                let x4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 4
                let r4 = x4.Value
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43
                let t44 = BatchedDeque.tail t43
                let f4 = BatchedDeque.head t44

                let x5 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.tryRemove 5
                let r5 = x5.Value
                let a5 = BatchedDeque.head r5
                let t51 = BatchedDeque.tail r5
                let b5 = BatchedDeque.head t51
                let t52 = BatchedDeque.tail t51
                let c5 = BatchedDeque.head t52
                let t53 = BatchedDeque.tail t52
                let d5 = BatchedDeque.head t53
                let t54 = BatchedDeque.tail t53
                let e5 = BatchedDeque.head t54

                ((b0 = "b") && (c0 = "c") && (d0 = "d") && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d") && (e5 = "e")) |> Expect.isTrue "" }

            test "BatchedDeque.update elements BatchedDeque.length 1" {
                len1 |> BatchedDeque.update 0 "aa" |> BatchedDeque.head |> Expect.equal "" "aa" } 

            test "BatchedDeque.update elements BatchedDeque.length 2" {
                let r0 = (BatchedDeque.ofSeq ["a";"b"]) |> BatchedDeque.update 0 "zz"
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01

                let r1 = (BatchedDeque.ofSeq ["a";"b"]) |> BatchedDeque.update 1 "zz"
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11

                ((a0 = "zz") && (b0 = "b")  
                && (a1 = "a") && (b1 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.update elements BatchedDeque.length 3" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.update 0 "zz"
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.update 1 "zz"
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.update 2 "zz"
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.update elements BatchedDeque.length 4" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.update 0 "zz"
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let d0 = BatchedDeque.head t03

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.update 1 "zz"
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let d1 = BatchedDeque.head t13

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.update 2 "zz"
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let d2 = BatchedDeque.head t23

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.update 3 "zz"
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let d3 = BatchedDeque.head t33

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.update elements BatchedDeque.length 5" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.update 0 "zz"
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let d0 = BatchedDeque.head t03
                let t04 = BatchedDeque.tail t03
                let e0 = BatchedDeque.head t04

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.update 1 "zz"
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let d1 = BatchedDeque.head t13
                let t14 = BatchedDeque.tail t13
                let e1 = BatchedDeque.head t14

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.update 2 "zz"
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let d2 = BatchedDeque.head t23
                let t24 = BatchedDeque.tail t23
                let e2 = BatchedDeque.head t24

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.update 3 "zz"
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let d3 = BatchedDeque.head t33
                let t34 = BatchedDeque.tail t33
                let e3 = BatchedDeque.head t34

                let r4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e"]) |> BatchedDeque.update 4 "zz"
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43
                let t44 = BatchedDeque.tail t43
                let e4 = BatchedDeque.head t44

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d")  && (e0 = "e")
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") && (e1 = "e") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") && (e2 = "e") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")  && (e3 = "e")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")  && (e4 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.update elements BatchedDeque.length 6" {
                let r0 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 0 "zz"
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let d0 = BatchedDeque.head t03
                let t04 = BatchedDeque.tail t03
                let e0 = BatchedDeque.head t04
                let t05 = BatchedDeque.tail t04
                let f0 = BatchedDeque.head t05

                let r1 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 1 "zz"
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let d1 = BatchedDeque.head t13
                let t14 = BatchedDeque.tail t13
                let e1 = BatchedDeque.head t14
                let t15 = BatchedDeque.tail t14
                let f1 = BatchedDeque.head t15

                let r2 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 2 "zz"
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let d2 = BatchedDeque.head t23
                let t24 = BatchedDeque.tail t23
                let e2 = BatchedDeque.head t24
                let t25 = BatchedDeque.tail t24
                let f2 = BatchedDeque.head t25

                let r3 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 3 "zz"
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let d3 = BatchedDeque.head t33
                let t34 = BatchedDeque.tail t33
                let e3 = BatchedDeque.head t34
                let t35 = BatchedDeque.tail t34
                let f3 = BatchedDeque.head t35

                let r4 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 4 "zz"
                let a4 = BatchedDeque.head r4
                let t41 = BatchedDeque.tail r4
                let b4 = BatchedDeque.head t41
                let t42 = BatchedDeque.tail t41
                let c4 = BatchedDeque.head t42
                let t43 = BatchedDeque.tail t42
                let d4 = BatchedDeque.head t43
                let t44 = BatchedDeque.tail t43
                let e4 = BatchedDeque.head t44
                let t45 = BatchedDeque.tail t44
                let f4 = BatchedDeque.head t45

                let r5 = (BatchedDeque.ofSeq ["a";"b";"c";"d";"e";"f"]) |> BatchedDeque.update 5 "zz"
                let a5 = BatchedDeque.head r5
                let t51 = BatchedDeque.tail r5
                let b5 = BatchedDeque.head t51
                let t52 = BatchedDeque.tail t51
                let c5 = BatchedDeque.head t52
                let t53 = BatchedDeque.tail t52
                let d5 = BatchedDeque.head t53
                let t54 = BatchedDeque.tail t53
                let e5 = BatchedDeque.head t54
                let t55 = BatchedDeque.tail t54
                let f5 = BatchedDeque.head t55

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d")  && (e0 = "e") && (f0 = "f")
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") && (e1 = "e") && (f1 = "f")
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") && (e2 = "e") && (f2 = "f")
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")  && (e3 = "e") && (f3 = "f")
                && (a4 = "a") && (b4 = "b") && (c4 = "c") && (d4 = "d")  && (e4 = "zz") && (f4 = "f")
                && (a5 = "a") && (b5 = "b") && (c5 = "c") && (d5 = "d")  && (e5 = "e") && (f5 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.tryUpdate elements BatchedDeque.length 1" {
                let a = len1 |> BatchedDeque.tryUpdate 0 "aa" 
                a.Value |> BatchedDeque.head |> Expect.equal "" "aa" } 

            test "BatchedDeque.tryUpdate elements BatchedDeque.length 2" {
                let x0 = (BatchedDeque.ofSeq ["a";"b"]) |> BatchedDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01

                let x1 = (BatchedDeque.ofSeq ["a";"b"]) |> BatchedDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11

                ((a0 = "zz") && (b0 = "b")  
                && (a1 = "a") && (b1 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.tryUpdate elements BatchedDeque.length 3" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12

                let x2 = (BatchedDeque.ofSeq ["a";"b";"c"]) |> BatchedDeque.tryUpdate 2 "zz"
                let r2 = x2.Value
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.tryUpdate elements BatchedDeque.length 4" {
                let x0 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryUpdate 0 "zz"
                let r0 = x0.Value
                let a0 = BatchedDeque.head r0
                let t01 = BatchedDeque.tail r0
                let b0 = BatchedDeque.head t01
                let t02 = BatchedDeque.tail t01
                let c0 = BatchedDeque.head t02
                let t03 = BatchedDeque.tail t02
                let d0 = BatchedDeque.head t03

                let x1 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryUpdate 1 "zz"
                let r1 = x1.Value
                let a1 = BatchedDeque.head r1
                let t11 = BatchedDeque.tail r1
                let b1 = BatchedDeque.head t11
                let t12 = BatchedDeque.tail t11
                let c1 = BatchedDeque.head t12
                let t13 = BatchedDeque.tail t12
                let d1 = BatchedDeque.head t13

                let x2 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryUpdate 2 "zz"
                let r2 = x2.Value
                let a2 = BatchedDeque.head r2
                let t21 = BatchedDeque.tail r2
                let b2 = BatchedDeque.head t21
                let t22 = BatchedDeque.tail t21
                let c2 = BatchedDeque.head t22
                let t23 = BatchedDeque.tail t22
                let d2 = BatchedDeque.head t23

                let x3 = (BatchedDeque.ofSeq ["a";"b";"c";"d"]) |> BatchedDeque.tryUpdate 3 "zz"
                let r3 = x3.Value
                let a3 = BatchedDeque.head r3
                let t31 = BatchedDeque.tail r3
                let b3 = BatchedDeque.head t31
                let t32 = BatchedDeque.tail t31
                let c3 = BatchedDeque.head t32
                let t33 = BatchedDeque.tail t32
                let d3 = BatchedDeque.head t33

                ((a0 = "zz") && (b0 = "b") && (c0 = "c") && (d0 = "d") 
                && (a1 = "a") && (b1 = "zz") && (c1 = "c") && (d1 = "d") 
                && (a2 = "a") && (b2 = "b") && (c2 = "zz") && (d2 = "d") 
                && (a3 = "a") && (b3 = "b") && (c3 = "c") && (d3 = "zz")) |> Expect.isTrue "" }

            test "BatchedDeque.tryUncons on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryUncons q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryUncons on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                let x, xs = (BatchedDeque.tryUncons q).Value 
                x |> Expect.equal "" "a" } 

            test "BatchedDeque.tryUnsnoc on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryUnsnoc q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryUnsnoc on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                let xs, x = (BatchedDeque.tryUnsnoc q).Value 
                x |> Expect.equal "" "d" } 

            test "BatchedDeque.tryGetHead on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryGetHead q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryGetHead on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                (BatchedDeque.tryGetHead q).Value |> Expect.equal "" "a" } 

            test "BatchedDeque.tryGetInit on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryGetInit q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryGetInit on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                let x = (BatchedDeque.tryGetInit q).Value 
                let x2 = x|> BatchedDeque.last 
                x2 |> Expect.equal "" "c" } 

            test "BatchedDeque.tryGetLast on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryGetLast q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryGetLast on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                (BatchedDeque.tryGetLast q).Value |> Expect.equal "" "d" } 


            test "BatchedDeque.tryGetTail on empty" {
                let q = BatchedDeque.empty()
                (BatchedDeque.tryGetTail q = None) |> Expect.isTrue "" }

            test "BatchedDeque.tryGetTail on q" {
                let q = BatchedDeque.ofSeq ["a";"b";"c";"d"]
                (BatchedDeque.tryGetTail q).Value |> BatchedDeque.head |> Expect.equal "" "b" } 
        ]
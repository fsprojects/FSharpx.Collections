namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Deque
open FSharpx.Collections.Tests.Properties
open FsCheck
open Expecto
open Expecto.Flip

module DequeTests  =

    [<Tests>]
    let testDeque =

        //quite a lot going on and difficult to reason about edge cases
        //testing up to length of 6 is the likely minimum to satisfy any arbitrary test case (less for some cases)

        let len1 = singleton "a"
        let len2 = singleton "a" |> cons "b"
        let len3 = singleton "a" |> cons "b" |> cons "c"
        let len4 = singleton "a" |> cons "b" |> cons "c" |> cons "d"
        let len5 = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
        let len6 = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
        let len7 = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
        let len8 = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
        let len9 = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
        let lena = singleton "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"

        let len1conj = empty |> conj "a"
        let len2conj = empty |> conj "b" |> conj "a"
        let len3conj = empty |> conj "c" |> conj "b" |> conj "a"
        let len4conj = empty |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let len5conj = empty |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let len6conj = empty |> conj "f" |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let len7conj = empty |> conj "g" |> conj "f" |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let len8conj = empty |> conj "h" |> conj "g" |> conj "f" |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let len9conj = empty |> conj "i" |> conj "h" |> conj "g" |> conj "f" |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"
        let lenaconj = empty |> conj "j" |> conj "i" |> conj "h" |> conj "g" |> conj "f" |> conj "e" |> conj "d" |> conj "c" |> conj "b" |> conj "a"

        testList "Deque" [
            test "empty dqueue should be empty" {
                Expect.isTrue "empty is empty" (empty |> isEmpty) }

            test "cons works" {
                Expect.isFalse "not empty" (len2 |> isEmpty) }

            test "conj works" {
                Expect.isFalse "" (len2conj |> isEmpty) }

            test "singleton head works" {
                Expect.equal "singleton" "a" (len1 |> head) }

            test "singleton last works" {
                Expect.equal "" "a" (len1 |> last) }

            test "TryUncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : Deque<string>) = 
                    match (q'.TryUncons) with
                    | Some(hd, tl) ->  loop tl
                    | None -> ()

                Expect.equal "unit" () <| loop q }

            test "Uncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : Deque<string>) = 
                    match (q'.Uncons) with
                    | hd, tl when tl.Length = 0 ->  ()
                    | hd, tl ->  loop tl

                Expect.equal "unit" () <| loop q }

            test "toSeq works" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 
                let l = List.ofSeq q
                Expect.equal "toSeq" l <|List.ofSeq (toSeq q) }

            test "tail of singleton empty" {
                 Expect.isTrue "isEmpty" (len1 |> tail |> isEmpty)
                 Expect.isTrue "isEmpty" (len1conj |> tail |> isEmpty) }

            test "tail of tail of 2 empty" {
                Expect.isTrue "isEmpty" (len2 |> tail |> tail |> isEmpty)
                Expect.isTrue "isEmpty" (len2conj |> tail |> tail |> isEmpty) }

            test "initial of singleton empty" {
                Expect.isTrue "isEmpty" (len1 |> initial |> isEmpty)
                Expect.isTrue "isEmpty" (len1conj |> initial |> isEmpty) }

            test "head, tail, and length work test 1" {
                let t1 = tail len2
                let t1s = tail len2conj
                Expect.isTrue "head, tail, and length" (((length t1) = 1) && ((length t1s) = 1) && ((head t1) = "a") && ((head t1s) = "a")) }

            test "head, tail, and length work test 2" {
                let t1 = tail len3
                let t1s = tail len3conj

                let t1_1 = tail t1
                let t1_1s = tail t1s
                  
                (((length t1) = 2) && ((length t1s) = 2) && ((head t1) = "b") && ((head t1s) = "b") && ((length t1_1) = 1) && ((length t1_1s) = 1) 
                && ((head t1_1) = "a") && ((head t1_1s) = "a")) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 3" {
                let t1 = tail len4
                let t1s = tail len4conj

                let t1_2 = tail t1
                let t1_2s = tail t1s

                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 3) && ((length t1s) = 3) && ((head t1) = "c")&& ((head t1s) = "c") && ((length t1_2) = 2) && ((length t1_2s) = 2)
                && ((head t1_2) = "b") && ((head t1_2s) = "b") && ((length t1_1) = 1) && ((length t1_1s) = 1) && ((head t1_1) = "a") 
                && ((head t1_1s) = "a")) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 4" {
                let t1 = tail len5
                let t1s = tail len5conj

                let t1_3 = tail t1
                let t1_3s = tail t1s

                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s

                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 4) && ((length t1s) = 4) && ((head t1) = "d") && ((head t1s) = "d") && ((length t1_3) = 3) && ((length t1_3s) = 3)
                && ((head t1_3) = "c") && ((head t1_3s) = "c") && ((length t1_2) = 2) && ((length t1_2s) = 2) && ((head t1_2) = "b") && ((head t1_2s) = "b")
                && ((length t1_1) = 1) && ((length t1_1s) = 1) && ((head t1_1) = "a") && ((head t1_1s) = "a")) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 5" {
                let t1 = tail len6
                let t1s = tail len6conj

                let t1_4 = tail t1
                let t1_4s = tail t1s

                let t1_3 = tail t1_4
                let t1_3s = tail t1_4s

                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s

                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 5) && ((length t1s) = 5) && ((head t1) = "e") && ((head t1s) = "e") && ((length t1_4) = 4) && ((length t1_4s) = 4) 
                && ((head t1_4) = "d") && ((head t1_4s) = "d") && ((length t1_3) = 3) && ((length t1_3s) = 3) && ((head t1_3) = "c") && ((head t1_3s) = "c") 
                && ((length t1_2) = 2) && ((length t1_2s) = 2) && ((head t1_2) = "b") && ((head t1_2s) = "b") && ((length t1_1) = 1) && ((length t1_1s) = 1)
                && ((head t1_1) = "a") && ((head t1_1s) = "a")) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 6" {
                let t1 = tail len7
                let t1s = tail len7conj

                let t1_5 = tail t1
                let t1_5s = tail t1s

                let t1_4 = tail t1_5
                let t1_4s = tail t1_5s

                let t1_3 = tail t1_4
                let t1_3s = tail t1_4s

                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s

                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 6) && ((length t1s) = 6)  
                && ((head t1) = "f") && ((head t1s) = "f") 
                && ((length t1_5) = 5) && ((length t1_5s) = 5) 
                && ((head t1_5) = "e") && ((head t1_5s) = "e") 
                && ((length t1_4) = 4) && ((length t1_4s) = 4) 
                && ((head t1_4) = "d") && ((head t1_4s) = "d") 
                && ((length t1_3) = 3) && ((length t1_3s) = 3) 
                && ((head t1_3) = "c") && ((head t1_3s) = "c") 
                && ((length t1_2) = 2) && ((length t1_2s) = 2) 
                && ((head t1_2) = "b") && ((head t1_2s) = "b") 
                && ((length t1_1) = 1) && ((length t1_1s) = 1) 
                && ((head t1_1) = "a") && ((head t1_1s) = "a") ) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 7" {
                let t1 = tail len8
                let t1s = tail len8conj
                let t1_6 = tail t1
                let t1_6s = tail t1s
                let t1_5 = tail t1_6
                let t1_5s = tail t1_6s
                let t1_4 = tail t1_5
                let t1_4s = tail t1_5s
                let t1_3 = tail t1_4
                let t1_3s = tail t1_4s
                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s
                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 7) && ((length t1s) = 7) 
                && ((head t1) = "g") && ((head t1s) = "g") 
                && ((length t1_6) = 6)  && ((length t1_6s) = 6) 
                && ((head t1_6) = "f")  && ((head t1_6s) = "f") 
                && ((length t1_5) = 5)  && ((length t1_5s) = 5) 
                && ((head t1_5) = "e")  && ((head t1_5s) = "e") 
                && ((length t1_4) = 4)  && ((length t1_4s) = 4) 
                && ((head t1_4) = "d")  && ((head t1_4s) = "d") 
                && ((length t1_3) = 3)  && ((length t1_3s) = 3) 
                && ((head t1_3) = "c")  && ((head t1_3s) = "c") 
                && ((length t1_2) = 2)  && ((length t1_2s) = 2) 
                && ((head t1_2) = "b")  && ((head t1_2s) = "b") 
                && ((length t1_1) = 1)  && ((length t1_1s) = 1) 
                && ((head t1_1) = "a")  && ((head t1_1s) = "a") ) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 8" {
                let t1 = tail len9
                let t1s = tail len9conj
                let t1_7 = tail t1
                let t1_7s = tail t1s  
                let t1_6 = tail t1_7
                let t1_6s = tail t1_7s
                let t1_5 = tail t1_6
                let t1_5s = tail t1_6s
                let t1_4 = tail t1_5
                let t1_4s = tail t1_5s
                let t1_3 = tail t1_4
                let t1_3s = tail t1_4s
                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s
                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s

                (((length t1) = 8) && ((length t1s) = 8) 
                && ((head t1) = "h") && ((head t1s) = "h") 
                && ((length t1_7) = 7)  && ((length t1_7s) = 7) 
                && ((head t1_7) = "g")  && ((head t1_7s) = "g") 
                && ((length t1_6) = 6)  && ((length t1_6s) = 6) 
                && ((head t1_6) = "f")  && ((head t1_6s) = "f") 
                && ((length t1_5) = 5)  && ((length t1_5s) = 5) 
                && ((head t1_5) = "e")  && ((head t1_5s) = "e") 
                && ((length t1_4) = 4)  && ((length t1_4s) = 4) 
                && ((head t1_4) = "d")  && ((head t1_4s) = "d") 
                && ((length t1_3) = 3)  && ((length t1_3s) = 3) 
                && ((head t1_3) = "c")  && ((head t1_3s) = "c") 
                && ((length t1_2) = 2)  && ((length t1_2s) = 2) 
                && ((head t1_2) = "b")  && ((head t1_2s) = "b") 
                && ((length t1_1) = 1)  && ((length t1_1s) = 1) 
                && ((head t1_1) = "a")  && ((head t1_1s) = "a") ) |> Expect.isTrue "head, tail, and length" }

            test "head, tail, and length work test 9" {
                let t1 = tail lena
                let t1s = tail lenaconj
                let t1_8 = tail t1
                let t1_8s = tail t1s
                let t1_7 = tail t1_8
                let t1_7s = tail t1_8s
                let t1_6 = tail t1_7
                let t1_6s = tail t1_7s
                let t1_5 = tail t1_6
                let t1_5s = tail t1_6s
                let t1_4 = tail t1_5
                let t1_4s = tail t1_5s
                let t1_3 = tail t1_4
                let t1_3s = tail t1_4s
                let t1_2 = tail t1_3
                let t1_2s = tail t1_3s
                let t1_1 = tail t1_2
                let t1_1s = tail t1_2s
    
                (((length t1) = 9) && ((length t1s) = 9) && ((head t1) = "i")  && ((head t1s) = "i") 
                && ((length t1_8) = 8) && ((length t1_8s) = 8) && ((head t1_8) = "h") && ((head t1_8s) = "h") 
                && ((length t1_7) = 7) && ((length t1_7s) = 7) && ((head t1_7) = "g") && ((head t1_7s) = "g") 
                && ((length t1_6) = 6) && ((length t1_6s) = 6) && ((head t1_6) = "f") && ((head t1_6s) = "f") 
                && ((length t1_5) = 5) && ((length t1_5s) = 5) && ((head t1_5) = "e") && ((head t1_5s) = "e") 
                && ((length t1_4) = 4) && ((length t1_4s) = 4) && ((head t1_4) = "d") && ((head t1_4s) = "d") 
                && ((length t1_3) = 3) && ((length t1_3s) = 3) && ((head t1_3) = "c") && ((head t1_3s) = "c") 
                && ((length t1_2) = 2) && ((length t1_2s) = 2) && ((head t1_2) = "b") && ((head t1_2s) = "b") 
                && ((length t1_1) = 1) && ((length t1_1s) = 1) && ((head t1_1) = "a") && ((head t1_1s) = "a")) |> Expect.isTrue "head, tail, and length" } 

            //the previous series thoroughly tested construction by conj, so we'll leave those out
            test "last, init, and length work test 1" {  
                let t1 = initial len2
                Expect.isTrue "last, init, and length" (((length t1) = 1) && ((last t1) = "b")) }

            test "last, init, and length work test 2" {
                let t1 = initial len3
                let t1_1 = initial t1
    
                Expect.isTrue "last, init, and length" (((length t1) = 2) && ((last t1) = "b") && ((length t1_1) = 1)  && ((last t1_1) = "c") ) }

            test "last, init, and length work test 3" {
                let t1 = initial len4
                let t1_1 = initial t1
                let t1_2 = initial t1_1
    
                (((length t1) = 3) && ((last t1) = "b")
                && ((length t1_1) = 2)  && ((last t1_1) = "c") 
                && ((length t1_2) = 1)  && ((last t1_2) = "d") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 4" {
                let t1 = initial len5
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
    
                (((length t1) = 4) && ((last t1) = "b")
                && ((length t1_1) = 3)  && ((last t1_1) = "c") 
                && ((length t1_2) = 2)  && ((last t1_2) = "d") 
                && ((length t1_3) = 1)  && ((last t1_3) = "e") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 5" {
                let t1 = initial len6
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
                let t1_4 = initial t1_3
    
                (((length t1) = 5) && ((last t1) = "b")
                && ((length t1_1) = 4)  && ((last t1_1) = "c") 
                && ((length t1_2) = 3)  && ((last t1_2) = "d") 
                && ((length t1_3) = 2)  && ((last t1_3) = "e") 
                && ((length t1_4) = 1)  && ((last t1_4) = "f") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 6" {
                let t1 = initial len7
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
                let t1_4 = initial t1_3
                let t1_5 = initial t1_4
    
                (((length t1) = 6) && ((last t1) = "b")
                && ((length t1_1) = 5)  && ((last t1_1) = "c") 
                && ((length t1_2) = 4)  && ((last t1_2) = "d") 
                && ((length t1_3) = 3)  && ((last t1_3) = "e") 
                && ((length t1_4) = 2)  && ((last t1_4) = "f") 
                && ((length t1_5) = 1)  && ((last t1_5) = "g") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 7" {
                let t1 = initial len8
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
                let t1_4 = initial t1_3
                let t1_5 = initial t1_4
                let t1_6 = initial t1_5
    
                (((length t1) = 7) && ((last t1) = "b") 
                && ((length t1_1) = 6)  && ((last t1_1) = "c") 
                && ((length t1_2) = 5)  && ((last t1_2) = "d") 
                && ((length t1_3) = 4)  && ((last t1_3) = "e") 
                && ((length t1_4) = 3)  && ((last t1_4) = "f") 
                && ((length t1_5) = 2)  && ((last t1_5) = "g") 
                && ((length t1_6) = 1)  && ((last t1_6) = "h") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 8" {
                let t1 = initial len9
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
                let t1_4 = initial t1_3
                let t1_5 = initial t1_4
                let t1_6 = initial t1_5
                let t1_7 = initial t1_6
    
                (((length t1) = 8) && ((last t1) = "b")
                && ((length t1_1) = 7)  && ((last t1_1) = "c") 
                && ((length t1_2) = 6)  && ((last t1_2) = "d") 
                && ((length t1_3) = 5)  && ((last t1_3) = "e") 
                && ((length t1_4) = 4)  && ((last t1_4) = "f") 
                && ((length t1_5) = 3)  && ((last t1_5) = "g") 
                && ((length t1_6) = 2)  && ((last t1_6) = "h") 
                && ((length t1_7) = 1)  && ((last t1_7) = "i") ) |> Expect.isTrue "last, init, and length" }

            test "last, init, and length work test 9" {
                let t1 = initial lena
                let t1_1 = initial t1
                let t1_2 = initial t1_1
                let t1_3 = initial t1_2
                let t1_4 = initial t1_3
                let t1_5 = initial t1_4
                let t1_6 = initial t1_5
                let t1_7 = initial t1_6
                let t1_8 = initial t1_7
    
                (((length t1) = 9) && ((last t1) = "b")
                && ((length t1_1) = 8)  && ((last t1_1) = "c") 
                && ((length t1_2) = 7)  && ((last t1_2) = "d") 
                && ((length t1_3) = 6)  && ((last t1_3) = "e") 
                && ((length t1_4) = 5)  && ((last t1_4) = "f") 
                && ((length t1_5) = 4)  && ((last t1_5) = "g") 
                && ((length t1_6) = 3)  && ((last t1_6) = "h") 
                && ((length t1_7) = 2)  && ((last t1_7) = "i") 
                && ((length t1_8) = 1)  && ((last t1_8) = "j") ) |> Expect.isTrue "last, init, and length" }

            test "IEnumerable Seq nth" {
                Expect.equal "IEnumerable nth" "e" (lena |> Seq.item 5) }

            test "IEnumerable Seq length" {
                Expect.equal "IEnumerable length" 10 (lena |> Seq.length) }

            test "type cons works" {
                Expect.equal "cons" "zz" (lena.Cons "zz" |> head) }

            test "ofCatLists and uncons" {
                let d = ofCatLists ["a";"b";"c"] ["d";"e";"f"]
                let h1, t1 = uncons d
                let h2, t2 = uncons t1
                let h3, t3 = uncons t2
                let h4, t4 = uncons t3
                let h5, t5 = uncons t4
                let h6, t6 = uncons t5

                Expect.isTrue "ofCatLists and uncons" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e") && (h6 = "f") && (isEmpty t6)) }

            test "unconj works" {
                let d = ofCatLists ["f";"e";"d"] ["c";"b";"a"]
                let i1, l1 = unconj d
                let i2, l2 = unconj i1
                let i3, l3 = unconj i2
                let i4, l4 = unconj i3
                let i5, l5 = unconj i4
                let i6, l6 = unconj i5

                Expect.isTrue "unconj" ((l1 = "a") && (l2 = "b") && (l3 = "c") && (l4 = "d") && (l5 = "e") && (l6 = "f") && (isEmpty i6)) }

            test "conj pattern discriminator" {
                let d = (ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let i1, l1 = unconj d 

                let i2, l2 = 
                    match i1 with
                    | Conj(i, l) -> i, l
                    | _ -> i1, "x"

                Expect.isTrue "conj" ((l2 = "b") && ((length i2) = 4)) }

            test "cons pattern discriminator" {
                let d = (ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
                let h1, t1 = uncons d 

                let h2, t2 = 
                    match t1 with
                    | Cons(h, t) -> h, t
                    | _ ->  "x", t1

                Expect.isTrue "cons" ((h2 = "e") && ((length t2) = 4)) }

            test "cons and conj pattern discriminator" {
                let d = (ofCatLists ["f";"e";"d"] ["c";"b";"a"]) 
    
                let mid1 = 
                    match d with
                    | Cons(h, Conj(i, l)) -> i
                    | _ -> d

                let head, last = 
                    match mid1 with
                    | Cons(h, Conj(i, l)) -> h, l
                    | _ -> "x", "x"

                Expect.isTrue "cons and conj" ((head = "e") && (last = "b")) } 

            test "rev deque length 1" {
                Expect.equal "length" "a" (rev len1 |> head) } 

            test "rev deque length 2" {
                let r1 = rev len2
                let h1 = head r1
                let t2 = tail r1
                let h2 = head t2

                Expect.isTrue "length" ((h1 = "a")  && (h2 = "b")) }

            test "rev deque length 3" {
                let r1 = rev len3
                let h1 = head r1
                let t2 = tail r1
                let h2 = head t2
                let t3 = tail t2
                let h3 = head t3

                Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c")) }

            test "rev deque length 4" {
                let r1 = rev len4
                let h1 = head r1
                let t2 = tail r1
                let h2 = head t2
                let t3 = tail t2
                let h3 = head t3
                let t4 = tail t3
                let h4 = head t4

                Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d")) }

            test "rev deque length 5" {
                let r1 = rev len5
                let h1 = head r1
                let t2 = tail r1
                let h2 = head t2
                let t3 = tail t2
                let h3 = head t3
                let t4 = tail t3
                let h4 = head t4
                let t5 = tail t4
                let h5 = head t5

                Expect.isTrue "length" ((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e")) }

            test "rev deque length 6" {
                let r1 = rev len6
                let h1 = head r1
                let t2 = tail r1
                let h2 = head t2
                let t3 = tail t2
                let h3 = head t3
                let t4 = tail t3
                let h4 = head t4
                let t5 = tail t4
                let h5 = head t5
                let t6 = tail t5
                let h6 = head t6

                Expect.isTrue "rev length"((h1 = "a") && (h2 = "b") && (h3 = "c") && (h4 = "d") && (h5 = "e") && (h6 = "f")) }

            test "tryUncons on empty" {
                let q = empty
                Expect.isNone "tryUncons" <| tryUncons q }

            test "tryUncons on q" {
                let q = ofSeq ["a";"b";"c";"d"]
                let x, _ = (tryUncons q).Value 
                Expect.equal "tryUncons" "a" x }

            test "tryUnconj on empty" {
                let q = empty
                Expect.isNone "tryUnconj" <| tryUnconj q }

            test "tryUnconj on q" {
                let q = ofSeq ["a";"b";"c";"d"]
                let _, x = (tryUnconj q).Value 
                Expect.equal "tryUnconj" "d" x }

            test "tryHead on empty" {
                let q = empty
                Expect.isNone "tryHead" <| tryHead q }

            test "tryHead on q" {
                let q = ofSeq ["a";"b";"c";"d"]
                Expect.equal "tryHead" "a" (tryHead q).Value }

            test "tryInitial on empty" {
                let q = empty
                Expect.isNone "tryInitial" <| tryInitial q }

            test "tryinitial on q" {
                let q = ofSeq ["a";"b";"c";"d"]
                let x = (tryInitial q).Value 
                let x2 = x|> last 
                Expect.equal "tryinitial" "c" x2 }

            test "tryLast on empty" {
                let q = empty
                Expect.isNone "tryLast" <| tryLast q }

            test "tryLast on deque" {
                let q = ofSeq ["a";"b";"c";"d"]
                Expect.equal "tryLast" "d" (tryLast q).Value 
                Expect.equal "tryLast" "a" (len2 |> tryLast).Value 
                Expect.equal "tryLast" "a" (len2conj |> tryLast).Value }

            test "tryTail on empty" {
                let q = empty
                Expect.isNone "tryTail" <| tryTail q }

            test "tryTail on q" {
                let q = ofSeq ["a";"b";"c";"d"]
                Expect.equal "tryTail" "b" ((tryTail q).Value |> head) }

            test "structural equality" {

                let l1 = ofSeq [1..100]
                let l2 = ofSeq [1..100]

                Expect.equal "equality" l1 l2 

                let l3 = ofSeq [1..99] |> conj 7

                Expect.notEqual "equality" l1 l3 }
        ]

    [<Tests>]
    let propertyTestDeque =

        let conjThruList l q  =
            let rec loop (q' : 'a Deque) (l' : 'a list) = 
                match l' with
                | hd :: tl -> loop (q'.Conj hd) tl
                | [] -> q'
        
            loop q l 
        (*
        non-Deque generators from random ofList
        *)
        let dequeOfListGen =
            gen {   let! n = Gen.length2thru12
                    let! x = Gen.listInt n
                    return ( (Deque.ofList x), x) }

        (*
        Deque generators from random ofSeq and/or conj elements from random list 
        *)
        let dequeIntGen =
            gen {   let! n = Gen.length1thru12
                    let! n2 = Gen.length2thru12
                    let! x =  Gen.listInt n
                    let! y =  Gen.listInt n2
                    return ( (Deque.ofSeq x |> conjThruList y), (x @ y) ) }

        let dequeIntOfSeqGen =
            gen {   let! n = Gen.length1thru12
                    let! x = Gen.listInt n
                    return ( (Deque.ofSeq x), x) }

        let dequeIntConjGen =
            gen {   let! n = Gen.length1thru12
                    let! x = Gen.listInt n
                    return ( (Deque.empty |> conjThruList x), x) }

        let dequeObjGen =
            gen {   let! n = Gen.length2thru12
                    let! n2 = Gen.length1thru12
                    let! x =  Gen.listObj n
                    let! y =  Gen.listObj n2
                    return ( (Deque.ofSeq x |> conjThruList y), (x @ y) ) }

        let dequeStringGen =
            gen {   let! n = Gen.length1thru12
                    let! n2 = Gen.length2thru12
                    let! x =  Gen.listString n
                    let! y =  Gen.listString n2  
                    return ( (Deque.ofSeq x |> conjThruList y), (x @ y) ) }

        // HACK: from when using NUnit TestCaseSource does not understand array of tuples at runtime
        let intGens start =
            let v = Array.create 3 (box (dequeIntGen, "Deque"))
            v.[1] <- box ((dequeIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "Deque OfSeq")
            v.[2] <- box ((dequeIntConjGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "Deque Enqueue") 
            v

        let intGensStart1 =
            intGens 1  //this will accept all

        let intGensStart2 =
            intGens 2 // this will accept 11 out of 12

        testList "Deque property tests" [
        
            testPropertyWithConfig config10k "Deque fold matches build list rev" (Prop.forAll (Arb.fromGen dequeIntGen) <|
                fun (q, l) -> q |> fold (fun l' elem  -> elem::l') [] = List.rev l )
              
            testPropertyWithConfig config10k "Deque OfSeq fold matches build list rev" (Prop.forAll (Arb.fromGen dequeIntOfSeqGen)  <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = List.rev l )

            testPropertyWithConfig config10k "Deque Conj fold matches build list rev" (Prop.forAll (Arb.fromGen dequeIntConjGen) <|
                fun (q, l) -> q |> fold (fun l' elem  -> elem::l') [] = List.rev l )

            testPropertyWithConfig config10k "Deque foldback matches build list" (Prop.forAll (Arb.fromGen dequeIntGen) <|
                fun (q, l) -> foldBack (fun elem l'  -> elem::l') q [] = l )
              
            testPropertyWithConfig config10k "Deque OfSeq foldback matches build list" (Prop.forAll (Arb.fromGen dequeIntOfSeqGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k "Deque Conj foldback matches build list" (Prop.forAll (Arb.fromGen dequeIntConjGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k "int deque builds and serializes" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q : Deque<int>, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "obj deque builds and serializes" (Prop.forAll (Arb.fromGen dequeObjGen) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "string deque builds and serializes" (Prop.forAll (Arb.fromGen dequeStringGen) <|
                fun (q , l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "obj Deque reverse . reverse = id" (Prop.forAll (Arb.fromGen dequeObjGen) <|
                fun (q, l) -> q |> rev |> rev |> Seq.toList = (q |> Seq.toList) )
    
            testPropertyWithConfig config10k "Deque ofList build and serialize" (Prop.forAll (Arb.fromGen dequeOfListGen) <|
                fun (q, (l : int list)) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k "get head from deque" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q : Deque<int>, l) -> head q = List.item 0 l )

            testPropertyWithConfig config10k "get head from deque safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q : Deque<int>, l) -> (tryHead q).Value = List.item 0 l )

            testPropertyWithConfig config10k "get tail from deque" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun ((q : Deque<int>), l) -> q.Tail.Head = List.item 1 l )

            testPropertyWithConfig config10k "get tail from deque safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun ((q : Deque<int>), l) -> q.TryTail.Value.Head = List.item 1 l )

            testPropertyWithConfig config10k "get initial from deque" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun ((q : Deque<int>), l) -> List.ofSeq (initial q) = (List.rev l |> List.tail |> List.rev) )

            testPropertyWithConfig config10k "get initial from deque safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun (q : Deque<int>, l) -> List.ofSeq q.TryInitial.Value = (List.rev l |> List.tail |> List.rev) )
        ]
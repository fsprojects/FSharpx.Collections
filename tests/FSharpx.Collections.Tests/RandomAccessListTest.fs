namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.RandomAccessList
open FSharpx.Collections.Tests.Properties
open FsCheck
open Expecto
open Expecto.Flip

//there's a crap-load to test here :)
//vector blocksizej of 32, need to generate lists up to 100

module RandomAccessListTest =
    let emptyRandomAccessList = RandomAccessList.empty

    let testRandomAccessList =

        testList "RandomAccessList" [

            test "fail if there is no head in the RandomAccessList" {
                Expect.throwsT<System.Exception> "no head" (fun () -> emptyRandomAccessList |> head |> ignore) }

            test "fail if there is no tail in the RandomAccessList" {
                Expect.throwsT<System.Exception> "no tail" (fun () -> emptyRandomAccessList |> tail |> ignore) }

            test "foldBack matches build list 2" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
                Expect.equal "foldBack" (List.ofSeq q) <| foldBack (fun (elem : string) (l' : string list) -> elem::l') q [] }

            test "fold matches build list rev 2" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
                Expect.equal "fold" (List.rev (List.ofSeq q)) <| fold (fun (l' : string list) (elem : string) -> elem::l') [] q }

            test "TryUncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : RandomAccessList<string>) = 
                    match (q'.TryUncons) with
                    | Some(hd, tl) ->  loop tl
                    | None -> None

                Expect.isNone "TryUncons" <| loop q }

            test "Uncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : RandomAccessList<string>) = 
                    match (q'.Uncons) with
                    | hd, tl when tl.IsEmpty -> true
                    | hd, tl ->  loop tl

                Expect.isTrue "Uncons" <| loop q }

            test "empty list should be empty" {
                Expect.isTrue "empty" (empty |> isEmpty) }

            test "cons works" {
                 Expect.isFalse "cons" (empty|> cons 1 |> cons 2 |> isEmpty) }

            test "uncons 1 element" {
                let x, _ = empty |> cons 1 |>  uncons
                Expect.equal "uncons" 1 x }

            test "uncons 2 elements" {
                let x, _ = empty |> cons 1 |> cons 2 |> uncons 
                Expect.equal "uncons" 2 x }

            test "uncons 3 elements" {
                let x, _ = empty |> cons 1 |> cons 2 |> cons 3 |> uncons 
                Expect.equal "uncons" 3 x }

            test "tryUncons 1 element" {
                let x = empty |> cons 1 |> tryUncons
                Expect.equal "tryUncons" 1 <| fst x.Value }

            test "tryUncons 2 elements" {
                let x = empty |> cons 1 |> cons 2 |> tryUncons
                Expect.equal "tryUncons" 2  <| fst x.Value }

            test "tryUncons 3 elements" {
                let x = empty |> cons 1 |> cons 2 |> cons 3 |> tryUncons 
                Expect.equal "tryUncons" 3 <| fst x.Value }

            test "tryUncons empty" {
                Expect.isNone "tryUncons" (empty |> tryUncons) }

            test "head should return" {
                Expect.equal "head" 2 (empty |> cons 1 |> cons 2 |> head) }

            test "tryHead should return" {
                Expect.equal "tryHead" 2 (empty |> cons 1 |> cons 2 |> tryHead).Value }

            test "tryHead on empty should return None" {
                Expect.isNone "tryHead" (empty |> tryHead) }

            test "tryTail on empty should return None" {
                Expect.isNone "tryTail" (empty |> tryTail) }

            test "tryTail on len 1 should return Some empty" {
                let x = (empty |> cons 1 |> tryTail).Value
                Expect.isTrue "tryTail" (x |> isEmpty) }

            test "tail on len 2 should return" {
                Expect.equal "tail" 1 (empty |> cons 1 |>  cons 2 |> tail |> head) }

            test "tryTail on len 2 should return" {
                let a = empty |> cons 1 |>  cons 2 |> tryTail 
                Expect.equal "tryTail" 1 <| head a.Value }

            test "randomAccessList of randomAccessLists constructed by consing tail" {

                let windowFun windowLength = 
                    fun (v : RandomAccessList<RandomAccessList<int>>) t ->
                    if v.Head.Length = windowLength then RandomAccessList.cons (RandomAccessList.empty.Cons(t)) v
                    else RandomAccessList.tail v |> RandomAccessList.cons (RandomAccessList.cons t (RandomAccessList.head v))

                let windowed = 
                    seq{1..100}
                    |> Seq.fold (windowFun 5) (RandomAccessList.empty.Cons RandomAccessList.empty<int>)

                Expect.equal "windowed" 20 windowed.Length
                Expect.equal "windowed" 5  windowed.[2].Length }

            test "windowSeq should keep every value from its original list" {
                let seq30 = seq { for i in 1..30 do yield i }
                let fullVec = ofSeq seq30
                for i in 1..35 do
                    let lists = windowSeq i seq30
                    Expect.equal "windowSeq" fullVec (lists |> fold append empty)
                    Expect.equal "windowSeq" [1;2;3;4;5] (lists |> fold append empty |> toSeq |> Seq.take 5 |> Seq.toList) }

            test "windowSeq should return vectors of equal length if possible" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len3lists = windowSeq 3 seq30
                let len5lists = windowSeq 5 seq30
                let len6lists = windowSeq 6 seq30

                Expect.equal "windowSeq" 10 (len3lists |> length)
                Expect.equal "windowSeq" 6 (len5lists |> length)
                Expect.equal "windowSeq" 5 (len6lists |> length)
                Expect.equal "windowSeq" [3;3;3;3;3;3;3;3;3;3] (len3lists |> map length |> toSeq |> Seq.toList)
                Expect.equal "windowSeq" [5;5;5;5;5;5] (len5lists |> map length |> toSeq |> Seq.toList)
                Expect.equal "windowSeq" [6;6;6;6;6] (len6lists |> map length |> toSeq |> Seq.toList) }

            test "windowSeq should return vectors all of equal length except the first" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len4lists = windowSeq 4 seq30
                let len7lists = windowSeq 7 seq30
                let len8lists = windowSeq 8 seq30
                let len17lists = windowSeq 17 seq30

                Expect.equal "" 8 (len4lists |> length)
                Expect.equal "" 5 (len7lists |> length)
                Expect.equal "" 4 (len8lists |> length)
                Expect.equal "" 2 (len17lists |> length)
                Expect.equal "" [2;4;4;4;4;4;4;4]  (len4lists |> map length |> toSeq |> Seq.toList) (*[4;4;4;4;4;4;4;2]*)
                Expect.equal "" [2;7;7;7;7] (len7lists |> map length |> toSeq |> Seq.toList)
                Expect.equal "" [6;8;8;8] (len8lists |> map length |> toSeq |> Seq.toList)
                Expect.equal "" [13;17] (len17lists |> map length |> toSeq |> Seq.toList) }

            test "nth on empty list should throw" {
                Expect.throwsT<System.IndexOutOfRangeException> "empty list" (fun () -> empty |> nth 0 |> ignore) }

            test "nth length 1" {
                let x = empty |> cons "a" 
                Expect.equal "nth" "a" (x |> nth 0) }

            test "appending two lists keeps order of items" {
                let list1 = ref empty
                for i in 3 .. -1 .. 1 do
                    list1 := cons i (!list1)

                let list2 = ref empty
                for i in 9 .. -1 .. 7 do
                    list2 := cons i (!list2)

                Expect.equal "" [1;2;3;7;8;9] (append (!list1) (!list2) |> toSeq |> Seq.toList) }

            test "rev empty" {
                Expect.isTrue "rev empty" <| isEmpty (empty |> rev) }

            test "rev elements length 5" {
                let a = ofSeq ["a";"b";"c";"d";"e"]
                let b = rev a

                let c = List.ofSeq b
                Expect.equal "rev" "a" a.Head
                Expect.equal "rev" "e" b.Head
                Expect.equal "rev" "e" c.Head
                Expect.equal "rev" (["a";"b";"c";"d";"e"] |> List.rev) (b |> List.ofSeq) }

            test "rev elements length 15" {
                let a = ofSeq ["a";"b";"c";"d";"e"; "f"; "g"; "h"; "i"; "j"; "l"; "m"; "n"; "o"; "p"]
                let b = rev a
                Expect.equal "rev" (["a";"b";"c";"d";"e"; "f"; "g"; "h"; "i"; "j"; "l"; "m"; "n"; "o"; "p"] |> List.rev) (b |> List.ofSeq) }

            test "rev 300" {
                let x = ofSeq [1..300]
                Expect.equal "rev" (List.rev [1..300]) (x.Rev() |> List.ofSeq) }

            test "nth length 2" {
                Expect.isTrue "" (((empty |> cons "a" |> cons "b" |> nth 0) = "b") && ((empty |> cons "a" |> cons "b" |> nth 1) = "a")) }

            test "nth length 3" {
                let len3 = empty |> cons "a" |> cons "b" |> cons "c"
                (((len3 |> nth 0) = "c") 
                && ((len3 |> nth 1) = "b") 
                && ((len3 |> nth 2) = "a")) |> Expect.isTrue "nth" }

            test "nth length 4" {
                let len4 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d"
                Expect.isTrue "nth" (((len4 |> nth 0) = "d") && ((len4 |> nth 1) = "c") && ((len4 |> nth 2) = "b") && ((len4 |> nth 3) = "a")) }

            test "nth length 5" {
                let len5 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
                (((len5 |> nth 0) = "e") && ((len5 |> nth 1) = "d") && ((len5 |> nth 2) = "c") && ((len5 |> nth 3) = "b") 
                && ((len5 |> nth 4) = "a")) |> Expect.isTrue "nth" }

            test "nth length 6" {
                let len6 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
                (((len6 |> nth 0) = "f") && ((len6 |> nth 1) = "e") && ((len6 |> nth 2) = "d") && ((len6 |> nth 3) = "c") 
                && ((len6 |> nth 4) = "b") && ((len6 |> nth 5) = "a")) |> Expect.isTrue "nth" }

            test "nth length 7" {
                let len7 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
                (((len7 |> nth 0) = "g") && ((len7 |> nth 1) = "f") && ((len7 |> nth 2) = "e") && ((len7 |> nth 3) = "d") 
                && ((len7 |> nth 4) = "c") && ((len7 |> nth 5) = "b") && ((len7 |> nth 6) = "a")) |> Expect.isTrue "nth" }

            test "nth length 8" {
                let len8 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
                (((len8 |> nth 0) = "h") && ((len8 |> nth 1) = "g") && ((len8 |> nth 2) = "f") && ((len8 |> nth 3) = "e") 
                && ((len8 |> nth 4) = "d") && ((len8 |> nth 5) = "c") && ((len8 |> nth 6) = "b") && ((len8 |> nth 7) = "a")) 
                |> Expect.isTrue "nth" }

            test "nth length 9" {
                let len9 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
                (((len9 |> nth 0) = "i") && ((len9 |> nth 1) = "h") && ((len9 |> nth 2) = "g") && ((len9 |> nth 3) = "f") 
                && ((len9 |> nth 4) = "e") && ((len9 |> nth 5) = "d") && ((len9 |> nth 6) = "c") && ((len9 |> nth 7) = "b")
                && ((len9 |> nth 8) = "a")) |> Expect.isTrue "nth" }

            test "nth length 10" {
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                (((lena |> nth 0) = "j") && ((lena |> nth 1) = "i") && ((lena |> nth 2) = "h") && ((lena |> nth 3) = "g") 
                && ((lena |> nth 4) = "f") && ((lena |> nth 5) = "e") && ((lena |> nth 6) = "d") && ((lena |> nth 7) = "c")
                && ((lena |> nth 8) = "b") && ((lena |> nth 9) = "a")) |> Expect.isTrue "nth" }

            test "tryNth length 1" {
                let a = empty |> cons "a" |> tryNth 0 
                Expect.equal "tryNth" "a" a.Value}

            test "tryNth length 2" {
                let len2 = empty |> cons "a" |> cons "b"
                let b = len2 |> tryNth 0
                let a = len2 |> tryNth 1
                Expect.isTrue "tryNth" ((b.Value = "b") && (a.Value = "a")) }

            test "tryNth length 3" {
                let len3 = empty |> cons "a" |> cons "b" |> cons "c"
                let c = len3 |> tryNth 0
                let b = len3 |> tryNth 1
                let a = len3 |> tryNth 2
                Expect.isTrue "tryNth"((c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "tryNth length 4" {
                let len4 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d"
                let d = len4 |> tryNth 0
                let c = len4 |> tryNth 1
                let b = len4 |> tryNth 2
                let a = len4 |> tryNth 3
                Expect.isTrue "tryNth" ((d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "tryNth length 5" {
                let len5 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
                let e = len5 |> tryNth 0
                let d = len5 |> tryNth 1
                let c = len5 |> tryNth 2
                let b = len5 |> tryNth 3
                let a = len5 |> tryNth 4
                Expect.isTrue "tryNth" ((e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "tryNth length 6" {
                let len6 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
                let f = len6 |> tryNth 0
                let e = len6 |> tryNth 1
                let d = len6 |> tryNth 2
                let c = len6 |> tryNth 3
                let b = len6 |> tryNth 4
                let a = len6 |> tryNth 5
                
                Expect.isTrue "tryNth" ((f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "tryNth length 7" {
                let len7 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
                let g = len7 |> tryNth 0
                let f = len7 |> tryNth 1
                let e = len7 |> tryNth 2
                let d = len7 |> tryNth 3
                let c = len7 |> tryNth 4
                let b = len7 |> tryNth 5
                let a = len7 |> tryNth 6
                ((g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") 
                && (a.Value = "a")) |> Expect.isTrue "tryNth" }

            test "tryNth length 8" {
                let len8 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
                let h = len8 |> tryNth 0
                let g = len8 |> tryNth 1
                let f = len8 |> tryNth 2
                let e = len8 |> tryNth 3
                let d = len8 |> tryNth 4
                let c = len8 |> tryNth 5
                let b = len8 |> tryNth 6
                let a = len8 |> tryNth 7
                ((h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c")  
                && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "tryNth" }

            test "tryNth length 9" {
                let len9 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
                let i = len9 |> tryNth 0
                let h = len9 |> tryNth 1
                let g = len9 |> tryNth 2
                let f = len9 |> tryNth 3
                let e = len9 |> tryNth 4
                let d = len9 |> tryNth 5
                let c = len9 |> tryNth 6
                let b = len9 |> tryNth 7
                let a = len9 |> tryNth 8
                ((i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") 
                && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "tryNth" }

            test "tryNth length 10" {
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                let j = lena |> tryNth 0
                let i = lena |> tryNth 1
                let h = lena |> tryNth 2
                let g = lena |> tryNth 3
                let f = lena |> tryNth 4
                let e = lena |> tryNth 5
                let d = lena |> tryNth 6
                let c = lena |> tryNth 7
                let b = lena |> tryNth 8
                let a = lena |> tryNth 9
                ((j.Value = "j") && (i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") 
                && (d.Value = "d") && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "tryNth" }

            test "tryNth not found" {
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                Expect.isNone "tryNth" (lena |> tryNth 10) }

            test "list of lists can be accessed with nthNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.equal "nthNth" 3 (outer |> nthNth 0 2)
                Expect.equal "nthNth" 5 ( outer |> nthNth 1 4) }

            test "nthNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 2 2 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 1 5 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth -1 2 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 1 -2 outer |> ignore) }

            test "list of lists can be accessed with tryNthNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.equal "tryNthNth" (Some 3) (outer |> tryNthNth 0 2)
                Expect.equal "tryNthNth" (Some 5) (outer |> tryNthNth 1 4) }

            test "tryNthNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.isNone "tryNthNth" (outer |> tryNthNth 2 2)
                Expect.isNone "tryNthNth" (outer |> tryNthNth 1 5)
                Expect.isNone "tryNthNth" (outer |> tryNthNth -1 2)
                Expect.isNone "tryNthNth" (outer |> tryNthNth 1 -2) }

            test "list of lists can be updated with updateNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.equal "updateNth" 7 (outer |> updateNth 0 2 7 |> nthNth 0 2)
                Expect.equal "updateNth" 9 (outer |> updateNth 1 4 9 |> nthNth 1 4) }

            test "updateNth should not change the original list" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = ref empty
                outer := cons inner (!outer)
                outer := cons inner (!outer)

                Expect.equal "updateNth" 7 (!outer |> updateNth 0 2 7 |> nthNth 0 2)
                Expect.equal "updateNth" 3 (!outer |> nthNth 0 2)
                Expect.equal "updateNth" 9 (!outer |> updateNth 1 4 9 |> nthNth 1 4)
                Expect.equal "updateNth" 5 (!outer |> nthNth 1 4) }

            test "updateNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 0 6 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 9 2 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 1 -4 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth -1 4 7 outer |> ignore) }

            test "tryUpdateNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                Expect.isNone "tryUpdateNth" <| tryUpdateNth 0 6 7 outer
                Expect.isNone "tryUpdateNth" <| tryUpdateNth 9 2 7 outer
                Expect.isNone "tryUpdateNth" <| tryUpdateNth 1 -4 7 outer
                Expect.isNone "tryUpdateNth" <| tryUpdateNth -1 4 7 outer }

            test "tryUpdateNth is like updateNth but returns option" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> cons inner |> cons inner

                let result = outer |> tryUpdateNth 0 2 7
                Expect.isTrue "tryUpdateNth" (result |> Option.isSome)
                Expect.equal "tryUpdateNth" 7 (result |> Option.get |> nthNth 0 2)

                let result2 = outer |> tryUpdateNth 1 4 9
                Expect.isTrue "tryUpdateNth" (result2 |> Option.isSome)
                Expect.equal "tryUpdateNth" 9 (result2 |> Option.get |> nthNth 1 4) }

            test "tryUpdateNth should not change the original list" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = ref empty
                outer := cons inner (!outer)
                outer := cons inner (!outer)

                Expect.equal "" 7 (!outer |> tryUpdateNth 0 2 7 |> Option.get |> nthNth 0 2)
                Expect.equal "tryUpdateNth" 3 (!outer |> nthNth 0 2)
                Expect.equal "" 9 (!outer |> tryUpdateNth 1 4 9 |> Option.get |> nthNth 1 4)
                Expect.equal "tryUpdateNth" 5 (!outer |> nthNth 1 4) }

            test "update length 1" {
                Expect.equal "update" "aa" (empty |> cons "a" |> update 0 "aa"|> nth 0) }

            test "update length 2" {
                let len2 = empty |> cons "a" |> cons "b"
                Expect.isTrue "update" (((len2 |> update 0 "bb"|> nth 0) = "bb") && ((len2 |> update 1 "aa"|> nth 1) = "aa")) }

            test "update length 3" {
                let len3 = empty |> cons "a" |> cons "b" |> cons "c"
                (((len3 |> update 0 "cc"|> nth 0) = "cc") && ((len3 |> update 1 "bb"|> nth 1) = "bb") 
                && ((len3 |> update 2 "aa"|> nth 2) = "aa")) |> Expect.isTrue "update" }

            test "update length 4" {
                let len4 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d"
                (((len4 |> update 0 "dd"|> nth 0) = "dd") && ((len4 |> update 1 "cc"|> nth 1) = "cc") 
                && ((len4 |> update 2 "bb"|> nth 2) = "bb") && ((len4 |> update 3 "aa"|> nth 3) = "aa")) 
                |> Expect.isTrue "update" }

            test "update length 5" {
                let len5 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
                (((len5 |> update 0 "ee"|> nth 0) = "ee") && ((len5 |> update 1 "dd"|> nth 1) = "dd") 
                && ((len5 |> update 2 "cc"|> nth 2) = "cc") && ((len5 |> update 3 "bb"|> nth 3) = "bb") 
                && ((len5 |> update 4 "aa"|> nth 4) = "aa")) |> Expect.isTrue "update" }

            test "update length 6" {
                let len6 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
                (((len6 |> update 0 "ff"|> nth 0) = "ff") && ((len6 |> update 1 "ee"|> nth 1) = "ee") 
                && ((len6 |> update 2 "dd"|> nth 2) = "dd") && ((len6 |> update 3 "cc"|> nth 3) = "cc") 
                && ((len6 |> update 4 "bb"|> nth 4) = "bb") && ((len6 |> update 5 "aa"|> nth 5) = "aa")) |> Expect.isTrue "update" }

            test "update length 7" {
                let len7 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
                (((len7 |> update 0 "gg"|> nth 0) = "gg") && ((len7 |> update 1 "ff"|> nth 1) = "ff") 
                && ((len7 |> update 2 "ee"|> nth 2) = "ee") && ((len7 |> update 3 "dd"|> nth 3) = "dd") 
                && ((len7 |> update 4 "cc"|> nth 4) = "cc") && ((len7 |> update 5 "bb"|> nth 5) = "bb") 
                && ((len7 |> update 6 "aa"|> nth 6) = "aa")) |> Expect.isTrue "update" }

            test "update length 8" {
                let len8 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
                (((len8 |> update 0 "hh"|> nth 0) = "hh") && ((len8 |> update 1 "gg"|> nth 1) = "gg") 
                && ((len8 |> update 2 "ff"|> nth 2) = "ff") && ((len8 |> update 3 "ee"|> nth 3) = "ee") 
                && ((len8 |> update 4 "dd"|> nth 4) = "dd") && ((len8 |> update 5 "cc"|> nth 5) = "cc") 
                && ((len8 |> update 6 "bb"|> nth 6) = "bb") && ((len8 |> update 7 "aa"|> nth 7) = "aa")) 
                |> Expect.isTrue "update" }

            test "update length 9" {
                let len9 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
                (((len9 |> update 0 "ii"|> nth 0) = "ii") && ((len9 |> update 1 "hh"|> nth 1) = "hh") 
                && ((len9 |> update 2 "gg"|> nth 2) = "gg") && ((len9 |> update 3 "ff"|> nth 3) = "ff") 
                && ((len9 |> update 4 "ee"|> nth 4) = "ee") && ((len9 |> update 5 "dd"|> nth 5) = "dd") 
                && ((len9 |> update 6 "cc"|> nth 6) = "cc") && ((len9 |> update 7 "bb"|> nth 7) = "bb")
                && ((len9 |> update 8 "aa"|> nth 8) = "aa")) |> Expect.isTrue "update" }

            test "update length 10" {
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                (((lena |> update 0 "jj"|> nth 0) = "jj") && ((lena |> update 1 "ii"|> nth 1) = "ii") 
                && ((lena |> update 2 "hh"|> nth 2) = "hh") && ((lena |> update 3 "gg"|> nth 3) = "gg") 
                && ((lena |> update 4 "ff"|> nth 4) = "ff") && ((lena |> update 5 "ee"|> nth 5) = "ee") 
                && ((lena |> update 6 "dd"|> nth 6) = "dd") && ((lena |> update 7 "cc"|> nth 7) = "cc")
                && ((lena |> update 8 "bb"|> nth 8) = "bb") && ((lena |> update 9 "aa"|> nth 9) = "aa")) |> Expect.isTrue "update" }

            test "tryUpdate length 1" {
                let a = empty |> cons "a" |> tryUpdate 0 "aa"
                ((a.Value |> nth 0) = "aa") |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 2" {
                let len2 = empty |> cons "a" |> cons "b"
                let b = len2 |> tryUpdate 0 "bb"
                let a = len2 |> tryUpdate 1 "aa"
                (((b.Value |> nth 0) = "bb") && ((a.Value |> nth 1) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 3" {
                let len3 = empty |> cons "a" |> cons "b" |> cons "c"
                let c = len3 |> tryUpdate 0 "cc"
                let b = len3 |> tryUpdate 1 "bb"
                let a = len3 |> tryUpdate 2 "aa"
                (((c.Value |> nth 0) = "cc") && ((b.Value |> nth 1) = "bb") && ((a.Value |> nth 2) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 4" {
                let len4 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d"
                let d = len4 |> tryUpdate 0 "dd"
                let c = len4 |> tryUpdate 1 "cc"
                let b = len4 |> tryUpdate 2 "bb"
                let a = len4 |> tryUpdate 3 "aa"
                (((d.Value |> nth 0) = "dd") && ((c.Value |> nth 1) = "cc") && ((b.Value |> nth 2) = "bb") 
                && ((a.Value |> nth 3) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 5" {
                let len5 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
                let e = len5 |> tryUpdate 0 "ee"
                let d = len5 |> tryUpdate 1 "dd"
                let c = len5 |> tryUpdate 2 "cc"
                let b = len5 |> tryUpdate 3 "bb"
                let a = len5 |> tryUpdate 4 "aa"
                (((e.Value |> nth 0) = "ee") && ((d.Value |> nth 1) = "dd") && ((c.Value |> nth 2) = "cc") 
                && ((b.Value |> nth 3) = "bb") && ((a.Value |> nth 4) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 6" {
                let len6 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
                let f = len6 |> tryUpdate 0 "ff"
                let e = len6 |> tryUpdate 1 "ee"
                let d = len6 |> tryUpdate 2 "dd"
                let c = len6 |> tryUpdate 3 "cc"
                let b = len6 |> tryUpdate 4 "bb"
                let a = len6 |> tryUpdate 5 "aa"
                (((f.Value |> nth 0) = "ff") && ((e.Value |> nth 1) = "ee") && ((d.Value |> nth 2) = "dd") 
                && ((c.Value |> nth 3) = "cc") && ((b.Value |> nth 4) = "bb") && ((a.Value |> nth 5) = "aa")) 
                |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 7" {
                let len7 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
                let g = len7 |> tryUpdate 0 "gg"
                let f = len7 |> tryUpdate 1 "ff"
                let e = len7 |> tryUpdate 2 "ee"
                let d = len7 |> tryUpdate 3 "dd"
                let c = len7 |> tryUpdate 4 "cc"
                let b = len7 |> tryUpdate 5 "bb"
                let a = len7 |> tryUpdate 6 "aa"
                (((g.Value |> nth 0) = "gg") && ((f.Value |> nth 1) = "ff") && ((e.Value |> nth 2) = "ee") 
                && ((d.Value |> nth 3) = "dd") && ((c.Value |> nth 4) = "cc") && ((b.Value |> nth 5) = "bb") 
                && ((a.Value |> nth 6) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 8" {
                let len8 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
                let h = len8 |> tryUpdate 0 "hh"
                let g = len8 |> tryUpdate 1 "gg"
                let f = len8 |> tryUpdate 2 "ff"
                let e = len8 |> tryUpdate 3 "ee"
                let d = len8 |> tryUpdate 4 "dd"
                let c = len8 |> tryUpdate 5 "cc"
                let b = len8 |> tryUpdate 6 "bb"
                let a = len8 |> tryUpdate 7 "aa"
                (((h.Value |> nth 0) = "hh") && ((g.Value |> nth 1) = "gg") && ((f.Value |> nth 2) = "ff") 
                && ((e.Value |> nth 3) = "ee") && ((d.Value |> nth 4) = "dd") && ((c.Value |> nth 5) = "cc")  
                && ((b.Value |> nth 6) = "bb")&& ((a.Value |> nth 7) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 9" {
                let len9 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
                let i = len9 |> tryUpdate 0 "ii"
                let h = len9 |> tryUpdate 1 "hh"
                let g = len9 |> tryUpdate 2 "gg"
                let f = len9 |> tryUpdate 3 "ff"
                let e = len9 |> tryUpdate 4 "ee"
                let d = len9 |> tryUpdate 5 "dd"
                let c = len9 |> tryUpdate 6 "cc"
                let b = len9 |> tryUpdate 7 "bb"
                let a = len9 |> tryUpdate 8 "aa"
                (((i.Value |> nth 0) = "ii") && ((h.Value |> nth 1) = "hh") && ((g.Value |> nth 2) = "gg") 
                && ((f.Value |> nth 3) = "ff") && ((e.Value |> nth 4) = "ee") && ((d.Value |> nth 5) = "dd") 
                && ((c.Value |> nth 6) = "cc") && ((b.Value |> nth 7) = "bb")&& ((a.Value |> nth 8) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryUpdate length 10" {
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                let j = lena |> tryUpdate 0 "jj"
                let i = lena |> tryUpdate 1 "ii"
                let h = lena |> tryUpdate 2 "hh"
                let g = lena |> tryUpdate 3 "gg"
                let f = lena |> tryUpdate 4 "ff"
                let e = lena |> tryUpdate 5 "ee"
                let d = lena |> tryUpdate 6 "dd"
                let c = lena |> tryUpdate 7 "cc"
                let b = lena |> tryUpdate 8 "bb"
                let a = lena |> tryUpdate 9 "aa"
                (((j.Value |> nth 0) = "jj") && ((i.Value |> nth 1) = "ii") && ((h.Value |> nth 2) = "hh") 
                && ((g.Value |> nth 3) = "gg") && ((f.Value |> nth 4) = "ff") && ((e.Value |> nth 5) = "ee") 
                && ((d.Value |> nth 6) = "dd") && ((c.Value |> nth 7) = "cc") && ((b.Value |> nth 8) = "bb")
                && ((a.Value |> nth 9) = "aa")) |> Expect.isTrue "tryUpdate" }

            test "tryupdate of long RAL" {
                let v = ofSeq [1..100]
                Expect.equal "tryUpdate" 5 (v |> update 99 5 |> nth 99) }

            test "length of empty is 0" {
                Expect.equal "" 0 (empty |> length) }

            test "length of 1 - 10 good" {
                let len1 = empty |> cons "a"
                let len2 = empty |> cons "a" |> cons "b"
                let len3 = empty |> cons "a" |> cons "b" |> cons "c"
                let len4 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d"
                let len5 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e"
                let len6 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f"
                let len7 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g"
                let len8 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h"
                let len9 = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i"
                let lena = empty |> cons "a" |> cons "b" |> cons "c" |> cons "d" |> cons "e" |> cons "f" |> cons "g" |> cons "h" |> cons "i" |> cons "j"
                (((length len1) = 1) && ((length len2) = 2) && ((length len3) = 3) && ((length len4) = 4) 
                && ((length len5) = 5) && ((length len6) = 6) && ((length len7) = 7) && ((length len8) = 8) 
                && ((length len9) = 9) && ((length lena) = 10)) |> Expect.isTrue "length" }

            test "allow map" {
                let x = ofSeq [1..300]
                let randomAccessList2 = map (fun x -> x * 2) x
                Expect.equal "map" [for i in 1..300 -> i * 2] (randomAccessList2 |> Seq.toList) }

            test "cons pattern discriminator - randomAccessList" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | Cons(h, t) -> h, t
                    | _ ->  "x", q

                Expect.isTrue "cons pattern discriminato" ((h1 = "f") && (t1.Length = 5)) }

            test "structural equality" {
                let l1 = ofSeq [1..100]
                let l2 = ofSeq [1..100]

                Expect.isTrue "structural equality" (l1 = l2)

                let l3 = l2 |> update 98 7

                Expect.isFalse "structural equality" (l1 = l3) }

            test "ofSeq random access list" {
                let x = ofSeq ["a";"b";"c";"d";"e";"f";"g";"h";"i";"j"]

                (((x |> nth 0) = "a") && ((x |> nth 1) = "b") && ((x |> nth 2) = "c") && ((x |> nth 3) = "d") 
                && ((x |> nth 4) = "e") && ((x |> nth 5) = "f") && ((x |> nth 6) = "g") && ((x |> nth 7) = "h")
                && ((x |> nth 8) = "i") && ((x |> nth 9) = "j")) |> Expect.isTrue "ofSeq" }

            test "allow init" {
                let randomAccessList = init 5 (fun x -> x * 2) 
                let s = Seq.init 5 (fun x -> x * 2)

                s |> Seq.toList |> Expect.equal "" [0;2;4;6;8]
                Expect.equal "init" [0;2;4;6;8] (randomAccessList |> Seq.toList) }

            test "toSeq to list" {
                let l = ["f";"e";"d";"c";"b";"a"] 
                let rl = ofSeq l

                Expect.equal "toSeq" l (rl |> toSeq |> List.ofSeq) }

            test "enumerate empty" {
                for i in RandomAccessList.empty do
                    ignore()

                Expect.isTrue "enumerate empty" true }
        ]

    [<Tests>]
    let propertyTestRandomAccessList = 
        let consThruList l q  =
            let rec loop (q' : 'a RandomAccessList) (l' : 'a list) = 
                match l' with
                | hd :: [] -> q'.Cons hd
                | hd :: tl -> loop (q'.Cons hd) tl
                | [] -> q'
        
            loop q l

        //RandomAccessList
        (*
        non-IRandomAccessList generators from random ofList
        *)
        let RandomAccessListOfListGen =
            gen {   let! n = Gen.length2thru100
                    let! x = Gen.listInt n
                    return ( (RandomAccessList.ofSeq x), x) }

        (*
        IRandomAccessList generators from random ofSeq and/or conj elements from random list 
        *)
        let RandomAccessListIntGen =
            gen {   let! n = Gen.length1thru100
                    let! n2 = Gen.length2thru100
                    let! x =  Gen.listInt n
                    let! y =  Gen.listInt n2
                    return ( (RandomAccessList.ofSeq x |> consThruList y), ((List.rev y) @ x) ) }

        let RandomAccessListIntOfSeqGen =
            gen {   let! n = Gen.length1thru100
                    let! x = Gen.listInt n
                    return ( (RandomAccessList.ofSeq x), x) }

        let RandomAccessListIntConsGen =
            gen {   let! n = Gen.length1thru100
                    let! x = Gen.listInt n
                    return ( (RandomAccessList.empty |> consThruList x),  List.rev x) }

        let RandomAccessListObjGen =
            gen {   let! n = Gen.length2thru100
                    let! n2 = Gen.length1thru100
                    let! x =  Gen.listObj n
                    let! y =  Gen.listObj n2
                    return ( (RandomAccessList.ofSeq x |> consThruList y), ((List.rev y) @ x) ) }

        let RandomAccessListStringGen =
            gen {   let! n = Gen.length1thru100
                    let! n2 = Gen.length2thru100
                    let! x =  Gen.listString n
                    let! y =  Gen.listString n2  
                    return ( (RandomAccessList.ofSeq x |> consThruList y), ((List.rev y) @ x) ) }

        // NUnit TestCaseSource does not understand array of tuples at runtime
        let intGens start =
            let v = Array.create 3 (box (RandomAccessListIntGen, "RandomAccessList"))
            v.[1] <- box ((RandomAccessListIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "RandomAccessList OfSeq")
            v.[2] <- box ((RandomAccessListIntConsGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "RandomAccessList conjRandomAccessList") 
            v

        let intGensStart1 =
            intGens 1  //this will accept all

        let intGensStart2 =
            intGens 2 // this will accept 11 out of 12

        testList "RandomAccessList property tests" [
            testPropertyWithConfig config10k  "fold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = (List.rev l) )
              
            testPropertyWithConfig config10k  "RandomAccessList OfSeq fold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k  "andomAccessList Consfold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> q |> fold (fun l' elem  -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k  "foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )
              
            testPropertyWithConfig config10k  "OfSeq foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k  "Conj foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k  "get head from RandomAccessList" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q : RandomAccessList<int>, l) -> (head q) = (List.item 0 l) )

            testPropertyWithConfig config10k  "get head from RandomAccessList safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q : RandomAccessList<int>, l) -> (tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k  "get tail from RandomAccessList" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun ((q : RandomAccessList<int>), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get tail from RandomAccessList safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
                fun (q : RandomAccessList<int>, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "int RandomAccessList builds and serializes" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "obj RandomAccessList builds and serializes" (Prop.forAll (Arb.fromGen RandomAccessListObjGen) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "string RandomAccessList builds and serializes" (Prop.forAll (Arb.fromGen RandomAccessListStringGen) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> q |> rev |> List.ofSeq = (List.rev l) )
              
            testPropertyWithConfig config10k  "OfSeq rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> q |> rev |> List.ofSeq = (List.rev l) )

            testPropertyWithConfig config10k  "Cons rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> q |> rev |> List.ofSeq = (List.rev l) )
        ]
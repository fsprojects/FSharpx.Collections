namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Properties
open FsCheck
open Expecto
open Expecto.Flip

//there's a crap-load to test here :)
//vector blocksizej of 32, need to generate lists up to 100

module RandomAccessListTest =
    let emptyRandomAccessList = RandomAccessList.empty

    let testRandomAccessList =

        testList "RandomAccessList" [

            test "fail if there is no RandomAccessList.head in the RandomAccessList" {
                Expect.throwsT<System.Exception> "no RandomAccessList.head" (fun () -> emptyRandomAccessList |> RandomAccessList.head |> ignore) }

            test "fail if there is no RandomAccessList.tail in the RandomAccessList" {
                Expect.throwsT<System.Exception> "no RandomAccessList.tail" (fun () -> emptyRandomAccessList |> RandomAccessList.tail |> ignore) }

            test "RandomAccessList.foldBack matches build list 2" {
                let q = RandomAccessList.ofSeq ["f";"e";"d";"c";"b";"a"]
                Expect.equal "RandomAccessList.foldBack" (List.ofSeq q) <| RandomAccessList.foldBack (fun (elem : string) (l' : string list) -> elem::l') q [] }

            test "RandomAccessList.fold matches build list rev 2" {
                let q = RandomAccessList.ofSeq ["f";"e";"d";"c";"b";"a"]
                Expect.equal "RandomAccessList.fold" (List.rev (List.ofSeq q)) <| RandomAccessList.fold (fun (l' : string list) (elem : string) -> elem::l') [] q }

            test "TryUncons wind-down to None" {
                let q = RandomAccessList.ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : RandomAccessList<string>) = 
                    match (q'.TryUncons) with
                    | Some(hd, tl) ->  loop tl
                    | None -> None

                Expect.isNone "TryUncons" <| loop q }

            test "Uncons wind-down to None" {
                let q = RandomAccessList.ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : RandomAccessList<string>) = 
                    match (q'.Uncons) with
                    | hd, tl when tl.IsEmpty -> true
                    | hd, tl ->  loop tl

                Expect.isTrue "Uncons" <| loop q }

            test "RandomAccessList.empty list should be RandomAccessList.empty" {
                Expect.isTrue "RandomAccessList.empty" (RandomAccessList.empty |> RandomAccessList.isEmpty) }

            test "RandomAccessList.cons works" {
                 Expect.isFalse "RandomAccessList.cons" (RandomAccessList.empty|> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.isEmpty) }

            test "RandomAccessList.uncons 1 element" {
                let x, _ = RandomAccessList.empty |> RandomAccessList.cons 1 |>  RandomAccessList.uncons
                Expect.equal "RandomAccessList.uncons" 1 x }

            test "RandomAccessList.uncons 2 elements" {
                let x, _ = RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.uncons 
                Expect.equal "RandomAccessList.uncons" 2 x }

            test "RandomAccessList.uncons 3 elements" {
                let x, _ = RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.cons 3 |> RandomAccessList.uncons 
                Expect.equal "RandomAccessList.uncons" 3 x }

            test "RandomAccessList.tryUncons 1 element" {
                let x = RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.tryUncons
                Expect.equal "RandomAccessList.tryUncons" 1 <| fst x.Value }

            test "RandomAccessList.tryUncons 2 elements" {
                let x = RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.tryUncons
                Expect.equal "RandomAccessList.tryUncons" 2  <| fst x.Value }

            test "RandomAccessList.tryUncons 3 elements" {
                let x = RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.cons 3 |> RandomAccessList.tryUncons 
                Expect.equal "RandomAccessList.tryUncons" 3 <| fst x.Value }

            test "RandomAccessList.tryUncons RandomAccessList.empty" {
                Expect.isNone "RandomAccessList.tryUncons" (RandomAccessList.empty |> RandomAccessList.tryUncons) }

            test "RandomAccessList.head should return" {
                Expect.equal "RandomAccessList.head" 2 (RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.head) }

            test "RandomAccessList.tryHead should return" {
                Expect.equal "RandomAccessList.tryHead" 2 (RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.cons 2 |> RandomAccessList.tryHead).Value }

            test "RandomAccessList.tryHead on RandomAccessList.empty should return None" {
                Expect.isNone "RandomAccessList.tryHead" (RandomAccessList.empty |> RandomAccessList.tryHead) }

            test "RandomAccessList.tryTail on RandomAccessList.empty should return None" {
                Expect.isNone "RandomAccessList.tryTail" (RandomAccessList.empty |> RandomAccessList.tryTail) }

            test "RandomAccessList.tryTail on len 1 should return Some RandomAccessList.empty" {
                let x = (RandomAccessList.empty |> RandomAccessList.cons 1 |> RandomAccessList.tryTail).Value
                Expect.isTrue "RandomAccessList.tryTail" (x |> RandomAccessList.isEmpty) }

            test "RandomAccessList.tail on len 2 should return" {
                Expect.equal "RandomAccessList.tail" 1 (RandomAccessList.empty |> RandomAccessList.cons 1 |>  RandomAccessList.cons 2 |> RandomAccessList.tail |> RandomAccessList.head) }

            test "RandomAccessList.tryTail on len 2 should return" {
                let a = RandomAccessList.empty |> RandomAccessList.cons 1 |>  RandomAccessList.cons 2 |> RandomAccessList.tryTail 
                Expect.equal "RandomAccessList.tryTail" 1 <| RandomAccessList.head a.Value }

            test "randomAccessList of randomAccessLists constructed by consing RandomAccessList.tail" {

                let windowFun windowLength = 
                    fun (v : RandomAccessList<RandomAccessList<int>>) t ->
                    if v.Head.Length = windowLength then RandomAccessList.cons (RandomAccessList.empty.Cons(t)) v
                    else RandomAccessList.tail v |> RandomAccessList.cons (RandomAccessList.cons t (RandomAccessList.head v))

                let windowed = 
                    seq{1..100}
                    |> Seq.fold (windowFun 5) (RandomAccessList.empty.Cons RandomAccessList.empty<int>)

                Expect.equal "windowed" 20 windowed.Length
                Expect.equal "windowed" 5  windowed.[2].Length }

            test "RandomAccessList.windowSeq should keep every value from its original list" {
                let seq30 = seq { for i in 1..30 do yield i }
                let fullVec = RandomAccessList.ofSeq seq30
                for i in 1..35 do
                    let lists = RandomAccessList.windowSeq i seq30
                    Expect.equal "RandomAccessList.windowSeq" fullVec (lists |> RandomAccessList.fold RandomAccessList.append RandomAccessList.empty)
                    Expect.equal "RandomAccessList.windowSeq" [1;2;3;4;5] (lists |> RandomAccessList.fold RandomAccessList.append RandomAccessList.empty |> RandomAccessList.toSeq |> Seq.take 5 |> Seq.toList) }

            test "RandomAccessList.windowSeq should return vectors of equal RandomAccessList.length if possible" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len3lists = RandomAccessList.windowSeq 3 seq30
                let len5lists = RandomAccessList.windowSeq 5 seq30
                let len6lists = RandomAccessList.windowSeq 6 seq30

                Expect.equal "RandomAccessList.windowSeq" 10 (len3lists |> RandomAccessList.length)
                Expect.equal "RandomAccessList.windowSeq" 6 (len5lists |> RandomAccessList.length)
                Expect.equal "RandomAccessList.windowSeq" 5 (len6lists |> RandomAccessList.length)
                Expect.equal "RandomAccessList.windowSeq" [3;3;3;3;3;3;3;3;3;3] (len3lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList)
                Expect.equal "RandomAccessList.windowSeq" [5;5;5;5;5;5] (len5lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList)
                Expect.equal "RandomAccessList.windowSeq" [6;6;6;6;6] (len6lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList) }

            test "RandomAccessList.windowSeq should return vectors all of equal RandomAccessList.length except the first" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len4lists = RandomAccessList.windowSeq 4 seq30
                let len7lists = RandomAccessList.windowSeq 7 seq30
                let len8lists = RandomAccessList.windowSeq 8 seq30
                let len17lists = RandomAccessList.windowSeq 17 seq30

                Expect.equal "" 8 (len4lists |> RandomAccessList.length)
                Expect.equal "" 5 (len7lists |> RandomAccessList.length)
                Expect.equal "" 4 (len8lists |> RandomAccessList.length)
                Expect.equal "" 2 (len17lists |> RandomAccessList.length)
                Expect.equal "" [2;4;4;4;4;4;4;4]  (len4lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList) (*[4;4;4;4;4;4;4;2]*)
                Expect.equal "" [2;7;7;7;7] (len7lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList)
                Expect.equal "" [6;8;8;8] (len8lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList)
                Expect.equal "" [13;17] (len17lists |> RandomAccessList.map RandomAccessList.length |> RandomAccessList.toSeq |> Seq.toList) }

            test "RandomAccessList.nth on RandomAccessList.empty list should throw" {
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.empty list" (fun () -> RandomAccessList.empty |> RandomAccessList.nth 0 |> ignore) }

            test "RandomAccessList.nth RandomAccessList.length 1" {
                let x = RandomAccessList.empty |> RandomAccessList.cons "a" 
                Expect.equal "RandomAccessList.nth" "a" (x |> RandomAccessList.nth 0) }

            test "appending two lists keeps order of items" {
                let list1 = ref RandomAccessList.empty
                for i in 3 .. -1 .. 1 do
                    list1 := RandomAccessList.cons i (!list1)

                let list2 = ref RandomAccessList.empty
                for i in 9 .. -1 .. 7 do
                    list2 := RandomAccessList.cons i (!list2)

                Expect.equal "" [1;2;3;7;8;9] (RandomAccessList.append (!list1) (!list2) |> RandomAccessList.toSeq |> Seq.toList) }

            test "rev RandomAccessList.empty" {
                Expect.isTrue "rev RandomAccessList.empty" <| RandomAccessList.isEmpty (RandomAccessList.empty |> RandomAccessList.rev) }

            test "rev elements RandomAccessList.length 5" {
                let a = RandomAccessList.ofSeq ["a";"b";"c";"d";"e"]
                let b = RandomAccessList.rev a

                let c = List.ofSeq b
                Expect.equal "rev" "a" a.Head
                Expect.equal "rev" "e" b.Head
                Expect.equal "rev" "e" c.Head
                Expect.equal "rev" (["a";"b";"c";"d";"e"] |> List.rev) (b |> List.ofSeq) }

            test "rev elements RandomAccessList.length 15" {
                let a = RandomAccessList.ofSeq ["a";"b";"c";"d";"e"; "f"; "g"; "h"; "i"; "j"; "l"; "m"; "n"; "o"; "p"]
                let b = RandomAccessList.rev a
                Expect.equal "rev" (["a";"b";"c";"d";"e"; "f"; "g"; "h"; "i"; "j"; "l"; "m"; "n"; "o"; "p"] |> List.rev) (b |> List.ofSeq) }

            test "rev 300" {
                let x = RandomAccessList.ofSeq [1..300]
                Expect.equal "rev" (List.rev [1..300]) (x.Rev() |> List.ofSeq) }

            test "RandomAccessList.nth RandomAccessList.length 2" {
                Expect.isTrue "" (((RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.nth 0) = "b") && ((RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.nth 1) = "a")) }

            test "RandomAccessList.nth RandomAccessList.length 3" {
                let len3 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c"
                (((len3 |> RandomAccessList.nth 0) = "c") 
                && ((len3 |> RandomAccessList.nth 1) = "b") 
                && ((len3 |> RandomAccessList.nth 2) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 4" {
                let len4 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d"
                Expect.isTrue "RandomAccessList.nth" (((len4 |> RandomAccessList.nth 0) = "d") && ((len4 |> RandomAccessList.nth 1) = "c") && ((len4 |> RandomAccessList.nth 2) = "b") && ((len4 |> RandomAccessList.nth 3) = "a")) }

            test "RandomAccessList.nth RandomAccessList.length 5" {
                let len5 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e"
                (((len5 |> RandomAccessList.nth 0) = "e") && ((len5 |> RandomAccessList.nth 1) = "d") && ((len5 |> RandomAccessList.nth 2) = "c") && ((len5 |> RandomAccessList.nth 3) = "b") 
                && ((len5 |> RandomAccessList.nth 4) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 6" {
                let len6 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f"
                (((len6 |> RandomAccessList.nth 0) = "f") && ((len6 |> RandomAccessList.nth 1) = "e") && ((len6 |> RandomAccessList.nth 2) = "d") && ((len6 |> RandomAccessList.nth 3) = "c") 
                && ((len6 |> RandomAccessList.nth 4) = "b") && ((len6 |> RandomAccessList.nth 5) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 7" {
                let len7 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g"
                (((len7 |> RandomAccessList.nth 0) = "g") && ((len7 |> RandomAccessList.nth 1) = "f") && ((len7 |> RandomAccessList.nth 2) = "e") && ((len7 |> RandomAccessList.nth 3) = "d") 
                && ((len7 |> RandomAccessList.nth 4) = "c") && ((len7 |> RandomAccessList.nth 5) = "b") && ((len7 |> RandomAccessList.nth 6) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 8" {
                let len8 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h"
                (((len8 |> RandomAccessList.nth 0) = "h") && ((len8 |> RandomAccessList.nth 1) = "g") && ((len8 |> RandomAccessList.nth 2) = "f") && ((len8 |> RandomAccessList.nth 3) = "e") 
                && ((len8 |> RandomAccessList.nth 4) = "d") && ((len8 |> RandomAccessList.nth 5) = "c") && ((len8 |> RandomAccessList.nth 6) = "b") && ((len8 |> RandomAccessList.nth 7) = "a")) 
                |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 9" {
                let len9 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i"
                (((len9 |> RandomAccessList.nth 0) = "i") && ((len9 |> RandomAccessList.nth 1) = "h") && ((len9 |> RandomAccessList.nth 2) = "g") && ((len9 |> RandomAccessList.nth 3) = "f") 
                && ((len9 |> RandomAccessList.nth 4) = "e") && ((len9 |> RandomAccessList.nth 5) = "d") && ((len9 |> RandomAccessList.nth 6) = "c") && ((len9 |> RandomAccessList.nth 7) = "b")
                && ((len9 |> RandomAccessList.nth 8) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.nth RandomAccessList.length 10" {
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                (((lena |> RandomAccessList.nth 0) = "j") && ((lena |> RandomAccessList.nth 1) = "i") && ((lena |> RandomAccessList.nth 2) = "h") && ((lena |> RandomAccessList.nth 3) = "g") 
                && ((lena |> RandomAccessList.nth 4) = "f") && ((lena |> RandomAccessList.nth 5) = "e") && ((lena |> RandomAccessList.nth 6) = "d") && ((lena |> RandomAccessList.nth 7) = "c")
                && ((lena |> RandomAccessList.nth 8) = "b") && ((lena |> RandomAccessList.nth 9) = "a")) |> Expect.isTrue "RandomAccessList.nth" }

            test "RandomAccessList.tryNth RandomAccessList.length 1" {
                let a = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.tryNth 0 
                Expect.equal "RandomAccessList.tryNth" "a" a.Value}

            test "RandomAccessList.tryNth RandomAccessList.length 2" {
                let len2 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b"
                let b = len2 |> RandomAccessList.tryNth 0
                let a = len2 |> RandomAccessList.tryNth 1
                Expect.isTrue "RandomAccessList.tryNth" ((b.Value = "b") && (a.Value = "a")) }

            test "RandomAccessList.tryNth RandomAccessList.length 3" {
                let len3 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c"
                let c = len3 |> RandomAccessList.tryNth 0
                let b = len3 |> RandomAccessList.tryNth 1
                let a = len3 |> RandomAccessList.tryNth 2
                Expect.isTrue "RandomAccessList.tryNth"((c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "RandomAccessList.tryNth RandomAccessList.length 4" {
                let len4 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d"
                let d = len4 |> RandomAccessList.tryNth 0
                let c = len4 |> RandomAccessList.tryNth 1
                let b = len4 |> RandomAccessList.tryNth 2
                let a = len4 |> RandomAccessList.tryNth 3
                Expect.isTrue "RandomAccessList.tryNth" ((d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "RandomAccessList.tryNth RandomAccessList.length 5" {
                let len5 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e"
                let e = len5 |> RandomAccessList.tryNth 0
                let d = len5 |> RandomAccessList.tryNth 1
                let c = len5 |> RandomAccessList.tryNth 2
                let b = len5 |> RandomAccessList.tryNth 3
                let a = len5 |> RandomAccessList.tryNth 4
                Expect.isTrue "RandomAccessList.tryNth" ((e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "RandomAccessList.tryNth RandomAccessList.length 6" {
                let len6 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f"
                let f = len6 |> RandomAccessList.tryNth 0
                let e = len6 |> RandomAccessList.tryNth 1
                let d = len6 |> RandomAccessList.tryNth 2
                let c = len6 |> RandomAccessList.tryNth 3
                let b = len6 |> RandomAccessList.tryNth 4
                let a = len6 |> RandomAccessList.tryNth 5
                
                Expect.isTrue "RandomAccessList.tryNth" ((f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") && (a.Value = "a")) }

            test "RandomAccessList.tryNth RandomAccessList.length 7" {
                let len7 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g"
                let g = len7 |> RandomAccessList.tryNth 0
                let f = len7 |> RandomAccessList.tryNth 1
                let e = len7 |> RandomAccessList.tryNth 2
                let d = len7 |> RandomAccessList.tryNth 3
                let c = len7 |> RandomAccessList.tryNth 4
                let b = len7 |> RandomAccessList.tryNth 5
                let a = len7 |> RandomAccessList.tryNth 6
                ((g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c") && (b.Value = "b") 
                && (a.Value = "a")) |> Expect.isTrue "RandomAccessList.tryNth" }

            test "RandomAccessList.tryNth RandomAccessList.length 8" {
                let len8 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h"
                let h = len8 |> RandomAccessList.tryNth 0
                let g = len8 |> RandomAccessList.tryNth 1
                let f = len8 |> RandomAccessList.tryNth 2
                let e = len8 |> RandomAccessList.tryNth 3
                let d = len8 |> RandomAccessList.tryNth 4
                let c = len8 |> RandomAccessList.tryNth 5
                let b = len8 |> RandomAccessList.tryNth 6
                let a = len8 |> RandomAccessList.tryNth 7
                ((h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") && (c.Value = "c")  
                && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "RandomAccessList.tryNth" }

            test "RandomAccessList.tryNth RandomAccessList.length 9" {
                let len9 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i"
                let i = len9 |> RandomAccessList.tryNth 0
                let h = len9 |> RandomAccessList.tryNth 1
                let g = len9 |> RandomAccessList.tryNth 2
                let f = len9 |> RandomAccessList.tryNth 3
                let e = len9 |> RandomAccessList.tryNth 4
                let d = len9 |> RandomAccessList.tryNth 5
                let c = len9 |> RandomAccessList.tryNth 6
                let b = len9 |> RandomAccessList.tryNth 7
                let a = len9 |> RandomAccessList.tryNth 8
                ((i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") && (d.Value = "d") 
                && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "RandomAccessList.tryNth" }

            test "RandomAccessList.tryNth RandomAccessList.length 10" {
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                let j = lena |> RandomAccessList.tryNth 0
                let i = lena |> RandomAccessList.tryNth 1
                let h = lena |> RandomAccessList.tryNth 2
                let g = lena |> RandomAccessList.tryNth 3
                let f = lena |> RandomAccessList.tryNth 4
                let e = lena |> RandomAccessList.tryNth 5
                let d = lena |> RandomAccessList.tryNth 6
                let c = lena |> RandomAccessList.tryNth 7
                let b = lena |> RandomAccessList.tryNth 8
                let a = lena |> RandomAccessList.tryNth 9
                ((j.Value = "j") && (i.Value = "i") && (h.Value = "h") && (g.Value = "g") && (f.Value = "f") && (e.Value = "e") 
                && (d.Value = "d") && (c.Value = "c") && (b.Value = "b")&& (a.Value = "a")) |> Expect.isTrue "RandomAccessList.tryNth" }

            test "RandomAccessList.tryNth not found" {
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                Expect.isNone "RandomAccessList.tryNth" (lena |> RandomAccessList.tryNth 10) }

            test "list of lists can be accessed with RandomAccessList.nthNth" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.equal "RandomAccessList.nthNth" 3 (outer |> RandomAccessList.nthNth 0 2)
                Expect.equal "RandomAccessList.nthNth" 5 ( outer |> RandomAccessList.nthNth 1 4) }

            test "RandomAccessList.nthNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.nthNth" (fun () -> RandomAccessList.nthNth 2 2 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.nthNth" (fun () -> RandomAccessList.nthNth 1 5 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.nthNth" (fun () -> RandomAccessList.nthNth -1 2 outer |> ignore) 
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.nthNth" (fun () -> RandomAccessList.nthNth 1 -2 outer |> ignore) }

            test "list of lists can be accessed with RandomAccessList.tryNthNth" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.equal "RandomAccessList.tryNthNth" (Some 3) (outer |> RandomAccessList.tryNthNth 0 2)
                Expect.equal "RandomAccessList.tryNthNth" (Some 5) (outer |> RandomAccessList.tryNthNth 1 4) }

            test "RandomAccessList.tryNthNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.isNone "RandomAccessList.tryNthNth" (outer |> RandomAccessList.tryNthNth 2 2)
                Expect.isNone "RandomAccessList.tryNthNth" (outer |> RandomAccessList.tryNthNth 1 5)
                Expect.isNone "RandomAccessList.tryNthNth" (outer |> RandomAccessList.tryNthNth -1 2)
                Expect.isNone "RandomAccessList.tryNthNth" (outer |> RandomAccessList.tryNthNth 1 -2) }

            test "list of lists can be RandomAccessList.updated with RandomAccessList.updateNth" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.equal "RandomAccessList.updateNth" 7 (outer |> RandomAccessList.updateNth 0 2 7 |> RandomAccessList.nthNth 0 2)
                Expect.equal "RandomAccessList.updateNth" 9 (outer |> RandomAccessList.updateNth 1 4 9 |> RandomAccessList.nthNth 1 4) }

            test "RandomAccessList.updateNth should not change the original list" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = ref RandomAccessList.empty
                outer := RandomAccessList.cons inner (!outer)
                outer := RandomAccessList.cons inner (!outer)

                Expect.equal "RandomAccessList.updateNth" 7 (!outer |> RandomAccessList.updateNth 0 2 7 |> RandomAccessList.nthNth 0 2)
                Expect.equal "RandomAccessList.updateNth" 3 (!outer |> RandomAccessList.nthNth 0 2)
                Expect.equal "RandomAccessList.updateNth" 9 (!outer |> RandomAccessList.updateNth 1 4 9 |> RandomAccessList.nthNth 1 4)
                Expect.equal "RandomAccessList.updateNth" 5 (!outer |> RandomAccessList.nthNth 1 4) }

            test "RandomAccessList.updateNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.updateNth" (fun () -> RandomAccessList.updateNth 0 6 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.updateNth" (fun () -> RandomAccessList.updateNth 9 2 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.updateNth" (fun () -> RandomAccessList.updateNth 1 -4 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "RandomAccessList.updateNth" (fun () -> RandomAccessList.updateNth -1 4 7 outer |> ignore) }

            test "RandomAccessList.tryUpdateNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                Expect.isNone "RandomAccessList.tryUpdateNth" <| RandomAccessList.tryUpdateNth 0 6 7 outer
                Expect.isNone "RandomAccessList.tryUpdateNth" <| RandomAccessList.tryUpdateNth 9 2 7 outer
                Expect.isNone "RandomAccessList.tryUpdateNth" <| RandomAccessList.tryUpdateNth 1 -4 7 outer
                Expect.isNone "RandomAccessList.tryUpdateNth" <| RandomAccessList.tryUpdateNth -1 4 7 outer }

            test "RandomAccessList.tryUpdateNth is like RandomAccessList.updateNth but returns option" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = RandomAccessList.empty |> RandomAccessList.cons inner |> RandomAccessList.cons inner

                let result = outer |> RandomAccessList.tryUpdateNth 0 2 7
                Expect.isTrue "RandomAccessList.tryUpdateNth" (result |> Option.isSome)
                Expect.equal "RandomAccessList.tryUpdateNth" 7 (result |> Option.get |> RandomAccessList.nthNth 0 2)

                let result2 = outer |> RandomAccessList.tryUpdateNth 1 4 9
                Expect.isTrue "RandomAccessList.tryUpdateNth" (result2 |> Option.isSome)
                Expect.equal "RandomAccessList.tryUpdateNth" 9 (result2 |> Option.get |> RandomAccessList.nthNth 1 4) }

            test "RandomAccessList.tryUpdateNth should not change the original list" {
                let inner = [1; 2; 3; 4; 5] |> RandomAccessList.ofSeq
                let outer = ref RandomAccessList.empty
                outer := RandomAccessList.cons inner (!outer)
                outer := RandomAccessList.cons inner (!outer)

                Expect.equal "" 7 (!outer |> RandomAccessList.tryUpdateNth 0 2 7 |> Option.get |> RandomAccessList.nthNth 0 2)
                Expect.equal "RandomAccessList.tryUpdateNth" 3 (!outer |> RandomAccessList.nthNth 0 2)
                Expect.equal "" 9 (!outer |> RandomAccessList.tryUpdateNth 1 4 9 |> Option.get |> RandomAccessList.nthNth 1 4)
                Expect.equal "RandomAccessList.tryUpdateNth" 5 (!outer |> RandomAccessList.nthNth 1 4) }

            test "RandomAccessList.update RandomAccessList.length 1" {
                Expect.equal "RandomAccessList.update" "aa" (RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.update 0 "aa"|> RandomAccessList.nth 0) }

            test "RandomAccessList.update RandomAccessList.length 2" {
                let len2 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b"
                Expect.isTrue "RandomAccessList.update" (((len2 |> RandomAccessList.update 0 "bb"|> RandomAccessList.nth 0) = "bb") && ((len2 |> RandomAccessList.update 1 "aa"|> RandomAccessList.nth 1) = "aa")) }

            test "RandomAccessList.update RandomAccessList.length 3" {
                let len3 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c"
                (((len3 |> RandomAccessList.update 0 "cc"|> RandomAccessList.nth 0) = "cc") && ((len3 |> RandomAccessList.update 1 "bb"|> RandomAccessList.nth 1) = "bb") 
                && ((len3 |> RandomAccessList.update 2 "aa"|> RandomAccessList.nth 2) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 4" {
                let len4 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d"
                (((len4 |> RandomAccessList.update 0 "dd"|> RandomAccessList.nth 0) = "dd") && ((len4 |> RandomAccessList.update 1 "cc"|> RandomAccessList.nth 1) = "cc") 
                && ((len4 |> RandomAccessList.update 2 "bb"|> RandomAccessList.nth 2) = "bb") && ((len4 |> RandomAccessList.update 3 "aa"|> RandomAccessList.nth 3) = "aa")) 
                |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 5" {
                let len5 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e"
                (((len5 |> RandomAccessList.update 0 "ee"|> RandomAccessList.nth 0) = "ee") && ((len5 |> RandomAccessList.update 1 "dd"|> RandomAccessList.nth 1) = "dd") 
                && ((len5 |> RandomAccessList.update 2 "cc"|> RandomAccessList.nth 2) = "cc") && ((len5 |> RandomAccessList.update 3 "bb"|> RandomAccessList.nth 3) = "bb") 
                && ((len5 |> RandomAccessList.update 4 "aa"|> RandomAccessList.nth 4) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 6" {
                let len6 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f"
                (((len6 |> RandomAccessList.update 0 "ff"|> RandomAccessList.nth 0) = "ff") && ((len6 |> RandomAccessList.update 1 "ee"|> RandomAccessList.nth 1) = "ee") 
                && ((len6 |> RandomAccessList.update 2 "dd"|> RandomAccessList.nth 2) = "dd") && ((len6 |> RandomAccessList.update 3 "cc"|> RandomAccessList.nth 3) = "cc") 
                && ((len6 |> RandomAccessList.update 4 "bb"|> RandomAccessList.nth 4) = "bb") && ((len6 |> RandomAccessList.update 5 "aa"|> RandomAccessList.nth 5) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 7" {
                let len7 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g"
                (((len7 |> RandomAccessList.update 0 "gg"|> RandomAccessList.nth 0) = "gg") && ((len7 |> RandomAccessList.update 1 "ff"|> RandomAccessList.nth 1) = "ff") 
                && ((len7 |> RandomAccessList.update 2 "ee"|> RandomAccessList.nth 2) = "ee") && ((len7 |> RandomAccessList.update 3 "dd"|> RandomAccessList.nth 3) = "dd") 
                && ((len7 |> RandomAccessList.update 4 "cc"|> RandomAccessList.nth 4) = "cc") && ((len7 |> RandomAccessList.update 5 "bb"|> RandomAccessList.nth 5) = "bb") 
                && ((len7 |> RandomAccessList.update 6 "aa"|> RandomAccessList.nth 6) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 8" {
                let len8 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h"
                (((len8 |> RandomAccessList.update 0 "hh"|> RandomAccessList.nth 0) = "hh") && ((len8 |> RandomAccessList.update 1 "gg"|> RandomAccessList.nth 1) = "gg") 
                && ((len8 |> RandomAccessList.update 2 "ff"|> RandomAccessList.nth 2) = "ff") && ((len8 |> RandomAccessList.update 3 "ee"|> RandomAccessList.nth 3) = "ee") 
                && ((len8 |> RandomAccessList.update 4 "dd"|> RandomAccessList.nth 4) = "dd") && ((len8 |> RandomAccessList.update 5 "cc"|> RandomAccessList.nth 5) = "cc") 
                && ((len8 |> RandomAccessList.update 6 "bb"|> RandomAccessList.nth 6) = "bb") && ((len8 |> RandomAccessList.update 7 "aa"|> RandomAccessList.nth 7) = "aa")) 
                |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 9" {
                let len9 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i"
                (((len9 |> RandomAccessList.update 0 "ii"|> RandomAccessList.nth 0) = "ii") && ((len9 |> RandomAccessList.update 1 "hh"|> RandomAccessList.nth 1) = "hh") 
                && ((len9 |> RandomAccessList.update 2 "gg"|> RandomAccessList.nth 2) = "gg") && ((len9 |> RandomAccessList.update 3 "ff"|> RandomAccessList.nth 3) = "ff") 
                && ((len9 |> RandomAccessList.update 4 "ee"|> RandomAccessList.nth 4) = "ee") && ((len9 |> RandomAccessList.update 5 "dd"|> RandomAccessList.nth 5) = "dd") 
                && ((len9 |> RandomAccessList.update 6 "cc"|> RandomAccessList.nth 6) = "cc") && ((len9 |> RandomAccessList.update 7 "bb"|> RandomAccessList.nth 7) = "bb")
                && ((len9 |> RandomAccessList.update 8 "aa"|> RandomAccessList.nth 8) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.update RandomAccessList.length 10" {
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                (((lena |> RandomAccessList.update 0 "jj"|> RandomAccessList.nth 0) = "jj") && ((lena |> RandomAccessList.update 1 "ii"|> RandomAccessList.nth 1) = "ii") 
                && ((lena |> RandomAccessList.update 2 "hh"|> RandomAccessList.nth 2) = "hh") && ((lena |> RandomAccessList.update 3 "gg"|> RandomAccessList.nth 3) = "gg") 
                && ((lena |> RandomAccessList.update 4 "ff"|> RandomAccessList.nth 4) = "ff") && ((lena |> RandomAccessList.update 5 "ee"|> RandomAccessList.nth 5) = "ee") 
                && ((lena |> RandomAccessList.update 6 "dd"|> RandomAccessList.nth 6) = "dd") && ((lena |> RandomAccessList.update 7 "cc"|> RandomAccessList.nth 7) = "cc")
                && ((lena |> RandomAccessList.update 8 "bb"|> RandomAccessList.nth 8) = "bb") && ((lena |> RandomAccessList.update 9 "aa"|> RandomAccessList.nth 9) = "aa")) |> Expect.isTrue "RandomAccessList.update" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 1" {
                let a = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.tryUpdate 0 "aa"
                ((a.Value |> RandomAccessList.nth 0) = "aa") |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 2" {
                let len2 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b"
                let b = len2 |> RandomAccessList.tryUpdate 0 "bb"
                let a = len2 |> RandomAccessList.tryUpdate 1 "aa"
                (((b.Value |> RandomAccessList.nth 0) = "bb") && ((a.Value |> RandomAccessList.nth 1) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 3" {
                let len3 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c"
                let c = len3 |> RandomAccessList.tryUpdate 0 "cc"
                let b = len3 |> RandomAccessList.tryUpdate 1 "bb"
                let a = len3 |> RandomAccessList.tryUpdate 2 "aa"
                (((c.Value |> RandomAccessList.nth 0) = "cc") && ((b.Value |> RandomAccessList.nth 1) = "bb") && ((a.Value |> RandomAccessList.nth 2) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 4" {
                let len4 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d"
                let d = len4 |> RandomAccessList.tryUpdate 0 "dd"
                let c = len4 |> RandomAccessList.tryUpdate 1 "cc"
                let b = len4 |> RandomAccessList.tryUpdate 2 "bb"
                let a = len4 |> RandomAccessList.tryUpdate 3 "aa"
                (((d.Value |> RandomAccessList.nth 0) = "dd") && ((c.Value |> RandomAccessList.nth 1) = "cc") && ((b.Value |> RandomAccessList.nth 2) = "bb") 
                && ((a.Value |> RandomAccessList.nth 3) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 5" {
                let len5 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e"
                let e = len5 |> RandomAccessList.tryUpdate 0 "ee"
                let d = len5 |> RandomAccessList.tryUpdate 1 "dd"
                let c = len5 |> RandomAccessList.tryUpdate 2 "cc"
                let b = len5 |> RandomAccessList.tryUpdate 3 "bb"
                let a = len5 |> RandomAccessList.tryUpdate 4 "aa"
                (((e.Value |> RandomAccessList.nth 0) = "ee") && ((d.Value |> RandomAccessList.nth 1) = "dd") && ((c.Value |> RandomAccessList.nth 2) = "cc") 
                && ((b.Value |> RandomAccessList.nth 3) = "bb") && ((a.Value |> RandomAccessList.nth 4) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 6" {
                let len6 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f"
                let f = len6 |> RandomAccessList.tryUpdate 0 "ff"
                let e = len6 |> RandomAccessList.tryUpdate 1 "ee"
                let d = len6 |> RandomAccessList.tryUpdate 2 "dd"
                let c = len6 |> RandomAccessList.tryUpdate 3 "cc"
                let b = len6 |> RandomAccessList.tryUpdate 4 "bb"
                let a = len6 |> RandomAccessList.tryUpdate 5 "aa"
                (((f.Value |> RandomAccessList.nth 0) = "ff") && ((e.Value |> RandomAccessList.nth 1) = "ee") && ((d.Value |> RandomAccessList.nth 2) = "dd") 
                && ((c.Value |> RandomAccessList.nth 3) = "cc") && ((b.Value |> RandomAccessList.nth 4) = "bb") && ((a.Value |> RandomAccessList.nth 5) = "aa")) 
                |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 7" {
                let len7 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g"
                let g = len7 |> RandomAccessList.tryUpdate 0 "gg"
                let f = len7 |> RandomAccessList.tryUpdate 1 "ff"
                let e = len7 |> RandomAccessList.tryUpdate 2 "ee"
                let d = len7 |> RandomAccessList.tryUpdate 3 "dd"
                let c = len7 |> RandomAccessList.tryUpdate 4 "cc"
                let b = len7 |> RandomAccessList.tryUpdate 5 "bb"
                let a = len7 |> RandomAccessList.tryUpdate 6 "aa"
                (((g.Value |> RandomAccessList.nth 0) = "gg") && ((f.Value |> RandomAccessList.nth 1) = "ff") && ((e.Value |> RandomAccessList.nth 2) = "ee") 
                && ((d.Value |> RandomAccessList.nth 3) = "dd") && ((c.Value |> RandomAccessList.nth 4) = "cc") && ((b.Value |> RandomAccessList.nth 5) = "bb") 
                && ((a.Value |> RandomAccessList.nth 6) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 8" {
                let len8 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h"
                let h = len8 |> RandomAccessList.tryUpdate 0 "hh"
                let g = len8 |> RandomAccessList.tryUpdate 1 "gg"
                let f = len8 |> RandomAccessList.tryUpdate 2 "ff"
                let e = len8 |> RandomAccessList.tryUpdate 3 "ee"
                let d = len8 |> RandomAccessList.tryUpdate 4 "dd"
                let c = len8 |> RandomAccessList.tryUpdate 5 "cc"
                let b = len8 |> RandomAccessList.tryUpdate 6 "bb"
                let a = len8 |> RandomAccessList.tryUpdate 7 "aa"
                (((h.Value |> RandomAccessList.nth 0) = "hh") && ((g.Value |> RandomAccessList.nth 1) = "gg") && ((f.Value |> RandomAccessList.nth 2) = "ff") 
                && ((e.Value |> RandomAccessList.nth 3) = "ee") && ((d.Value |> RandomAccessList.nth 4) = "dd") && ((c.Value |> RandomAccessList.nth 5) = "cc")  
                && ((b.Value |> RandomAccessList.nth 6) = "bb")&& ((a.Value |> RandomAccessList.nth 7) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 9" {
                let len9 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i"
                let i = len9 |> RandomAccessList.tryUpdate 0 "ii"
                let h = len9 |> RandomAccessList.tryUpdate 1 "hh"
                let g = len9 |> RandomAccessList.tryUpdate 2 "gg"
                let f = len9 |> RandomAccessList.tryUpdate 3 "ff"
                let e = len9 |> RandomAccessList.tryUpdate 4 "ee"
                let d = len9 |> RandomAccessList.tryUpdate 5 "dd"
                let c = len9 |> RandomAccessList.tryUpdate 6 "cc"
                let b = len9 |> RandomAccessList.tryUpdate 7 "bb"
                let a = len9 |> RandomAccessList.tryUpdate 8 "aa"
                (((i.Value |> RandomAccessList.nth 0) = "ii") && ((h.Value |> RandomAccessList.nth 1) = "hh") && ((g.Value |> RandomAccessList.nth 2) = "gg") 
                && ((f.Value |> RandomAccessList.nth 3) = "ff") && ((e.Value |> RandomAccessList.nth 4) = "ee") && ((d.Value |> RandomAccessList.nth 5) = "dd") 
                && ((c.Value |> RandomAccessList.nth 6) = "cc") && ((b.Value |> RandomAccessList.nth 7) = "bb")&& ((a.Value |> RandomAccessList.nth 8) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "RandomAccessList.tryUpdate RandomAccessList.length 10" {
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                let j = lena |> RandomAccessList.tryUpdate 0 "jj"
                let i = lena |> RandomAccessList.tryUpdate 1 "ii"
                let h = lena |> RandomAccessList.tryUpdate 2 "hh"
                let g = lena |> RandomAccessList.tryUpdate 3 "gg"
                let f = lena |> RandomAccessList.tryUpdate 4 "ff"
                let e = lena |> RandomAccessList.tryUpdate 5 "ee"
                let d = lena |> RandomAccessList.tryUpdate 6 "dd"
                let c = lena |> RandomAccessList.tryUpdate 7 "cc"
                let b = lena |> RandomAccessList.tryUpdate 8 "bb"
                let a = lena |> RandomAccessList.tryUpdate 9 "aa"
                (((j.Value |> RandomAccessList.nth 0) = "jj") && ((i.Value |> RandomAccessList.nth 1) = "ii") && ((h.Value |> RandomAccessList.nth 2) = "hh") 
                && ((g.Value |> RandomAccessList.nth 3) = "gg") && ((f.Value |> RandomAccessList.nth 4) = "ff") && ((e.Value |> RandomAccessList.nth 5) = "ee") 
                && ((d.Value |> RandomAccessList.nth 6) = "dd") && ((c.Value |> RandomAccessList.nth 7) = "cc") && ((b.Value |> RandomAccessList.nth 8) = "bb")
                && ((a.Value |> RandomAccessList.nth 9) = "aa")) |> Expect.isTrue "RandomAccessList.tryUpdate" }

            test "tryRandomAccessList.update of long RAL" {
                let v = RandomAccessList.ofSeq [1..100]
                Expect.equal "RandomAccessList.tryUpdate" 5 (v |> RandomAccessList.update 99 5 |> RandomAccessList.nth 99) }

            test "RandomAccessList.length of RandomAccessList.empty is 0" {
                Expect.equal "" 0 (RandomAccessList.empty |> RandomAccessList.length) }

            test "RandomAccessList.length of 1 - 10 good" {
                let len1 = RandomAccessList.empty |> RandomAccessList.cons "a"
                let len2 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b"
                let len3 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c"
                let len4 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d"
                let len5 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e"
                let len6 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f"
                let len7 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g"
                let len8 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h"
                let len9 = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i"
                let lena = RandomAccessList.empty |> RandomAccessList.cons "a" |> RandomAccessList.cons "b" |> RandomAccessList.cons "c" |> RandomAccessList.cons "d" |> RandomAccessList.cons "e" |> RandomAccessList.cons "f" |> RandomAccessList.cons "g" |> RandomAccessList.cons "h" |> RandomAccessList.cons "i" |> RandomAccessList.cons "j"
                (((RandomAccessList.length len1) = 1) && ((RandomAccessList.length len2) = 2) && ((RandomAccessList.length len3) = 3) && ((RandomAccessList.length len4) = 4) 
                && ((RandomAccessList.length len5) = 5) && ((RandomAccessList.length len6) = 6) && ((RandomAccessList.length len7) = 7) && ((RandomAccessList.length len8) = 8) 
                && ((RandomAccessList.length len9) = 9) && ((RandomAccessList.length lena) = 10)) |> Expect.isTrue "RandomAccessList.length" }

            test "allow map" {
                let x = RandomAccessList.ofSeq [1..300]
                let randomAccessList2 = RandomAccessList.map (fun x -> x * 2) x
                Expect.equal "map" [for i in 1..300 -> i * 2] (randomAccessList2 |> Seq.toList) }

            test "RandomAccessList.cons pattern discriminator - randomAccessList" {
                let q = RandomAccessList.ofSeq ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | RandomAccessList.Cons(h, t) -> h, t
                    | _ ->  "x", q

                Expect.isTrue "RandomAccessList.cons pattern discriminato" ((h1 = "f") && (t1.Length = 5)) }

            test "structural equality" {
                let l1 = RandomAccessList.ofSeq [1..100]
                let l2 = RandomAccessList.ofSeq [1..100]

                Expect.isTrue "structural equality" (l1 = l2)

                let l3 = l2 |> RandomAccessList.update 98 7

                Expect.isFalse "structural equality" (l1 = l3) }

            test "RandomAccessList.ofSeq random access list" {
                let x = RandomAccessList.ofSeq ["a";"b";"c";"d";"e";"f";"g";"h";"i";"j"]

                (((x |> RandomAccessList.nth 0) = "a") && ((x |> RandomAccessList.nth 1) = "b") && ((x |> RandomAccessList.nth 2) = "c") && ((x |> RandomAccessList.nth 3) = "d") 
                && ((x |> RandomAccessList.nth 4) = "e") && ((x |> RandomAccessList.nth 5) = "f") && ((x |> RandomAccessList.nth 6) = "g") && ((x |> RandomAccessList.nth 7) = "h")
                && ((x |> RandomAccessList.nth 8) = "i") && ((x |> RandomAccessList.nth 9) = "j")) |> Expect.isTrue "RandomAccessList.ofSeq" }

            test "allow init" {
                let randomAccessList = RandomAccessList.init 5 (fun x -> x * 2) 
                let s = Seq.init 5 (fun x -> x * 2)

                s |> Seq.toList |> Expect.equal "" [0;2;4;6;8]
                Expect.equal "init" [0;2;4;6;8] (randomAccessList |> Seq.toList) }

            test "RandomAccessList.toSeq to list" {
                let l = ["f";"e";"d";"c";"b";"a"] 
                let rl = RandomAccessList.ofSeq l

                Expect.equal "RandomAccessList.toSeq" l (rl |> RandomAccessList.toSeq |> List.ofSeq) }

            test "enumerate RandomAccessList.empty" {
                for i in RandomAccessList.empty do
                    ignore()

                Expect.isTrue "enumerate RandomAccessList.empty" true }
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
        IRandomAccessList generators from random RandomAccessList.ofSeq and/or conj elements from random list 
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

        let intGens start =
            let v = Array.create 3 RandomAccessListIntGen
            v.[1] <- RandomAccessListIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)
            v.[2] <- RandomAccessListIntConsGen |> Gen.filter (fun (q, l) -> l.Length >= start)
            v

        let intGensStart1 =
            intGens 1  //this will accept all

        let intGensStart2 =
            intGens 2 // this will accept 11 out of 12

        testList "RandomAccessList property tests" [
            testPropertyWithConfig config10k  "RandomAccessList.fold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> q |> RandomAccessList.fold (fun l' elem -> elem::l') [] = (List.rev l) )
              
            testPropertyWithConfig config10k  "RandomAccessList OfSeq RandomAccessList.fold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> q |> RandomAccessList.fold (fun l' elem -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k  "andomAccessList Consfold matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> q |> RandomAccessList.fold (fun l' elem  -> elem::l') [] = (List.rev l) )

            testPropertyWithConfig config10k  "RandomAccessList.foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> RandomAccessList.foldBack (fun elem l' -> elem::l') q [] = l )
              
            testPropertyWithConfig config10k  "OfSeq RandomAccessList.foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> RandomAccessList.foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k  "Conj RandomAccessList.foldBack matches build list" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> RandomAccessList.foldBack (fun elem l' -> elem::l') q [] = l )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> (RandomAccessList.head q) = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> (RandomAccessList.head q) = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> (RandomAccessList.head q) = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList safely 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> (RandomAccessList.tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList safely 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> (RandomAccessList.tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.head from RandomAccessList safely 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> (RandomAccessList.tryHead q).Value = (List.item 0 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun ((q), l) -> q.Tail.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList safely 0" (Prop.forAll (Arb.fromGen intGensStart2.[0]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList safely 1" (Prop.forAll (Arb.fromGen intGensStart2.[1]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "get RandomAccessList.tail from RandomAccessList safely 2" (Prop.forAll (Arb.fromGen intGensStart2.[2]) <|
                fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l) )

            testPropertyWithConfig config10k  "int RandomAccessList builds and serializes 0" (Prop.forAll (Arb.fromGen intGensStart1.[0]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "int RandomAccessList builds and serializes 1" (Prop.forAll (Arb.fromGen intGensStart1.[1]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "int RandomAccessList builds and serializes 2" (Prop.forAll (Arb.fromGen intGensStart1.[2]) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "obj RandomAccessList builds and serializes" (Prop.forAll (Arb.fromGen RandomAccessListObjGen) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "string RandomAccessList builds and serializes" (Prop.forAll (Arb.fromGen RandomAccessListStringGen) <|
                fun (q, l) -> q |> Seq.toList = l )

            testPropertyWithConfig config10k  "rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntGen) <|
                fun (q, l) -> q |> RandomAccessList.rev |> List.ofSeq = (List.rev l) )
              
            testPropertyWithConfig config10k  "OfSeq rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntOfSeqGen) <|
                fun (q, l) -> q |> RandomAccessList.rev |> List.ofSeq = (List.rev l) )

            testPropertyWithConfig config10k  "Cons rev matches build list rev" (Prop.forAll (Arb.fromGen RandomAccessListIntConsGen) <|
                fun (q, l) -> q |> RandomAccessList.rev |> List.ofSeq = (List.rev l) )
        ]
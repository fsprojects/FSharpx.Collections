namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Expecto
open Expecto.Flip

module PersistentVectorTests =
    [<Tests>]
    let testPersistentVector =

        testList "PersistentVector" [
            test "PersistentVector.empty vector should be PersistentVector.empty" {
                let x = PersistentVector.empty<int>
                Expect.equal "PersistentVector.empty" 0 (x |> PersistentVector.length) }

            test "multiple cons to an PersistentVector.empty vector should increase the count" {
                Expect.equal "cons" 3 (PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.conj 4 |> PersistentVector.conj 25 |> PersistentVector.length) }

            test "cons to an PersistentVector.empty vector should create a singleton vector" {
                Expect.equal "cons" 1 (PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.nth 0) }

            test "multiple cons to an PersistentVector.empty vector should create a vector" {
                Expect.equal "cons" 4 (PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.conj 4 |> PersistentVector.conj 25 |> PersistentVector.nth 1) }

            test "multiple assoc to the end should work like cons and create a vector" {
                let v = PersistentVector.empty |> PersistentVector.update 0 1 |> PersistentVector.update 1 4 |> PersistentVector.update 2 25 
                Expect.equal "assoc" 1 (v |> PersistentVector.nth 0)
                Expect.equal "assoc" 4 (v |> PersistentVector.nth 1)
                Expect.equal "assoc" 25 (v |> PersistentVector.nth 2) }

            test "300 cons to an PersistentVector.empty vector should create a vector" {
                let vector = ref PersistentVector.empty
                for i in 1..300 do
                    vector := PersistentVector.conj i (!vector)

                Expect.equal "" 101 (!vector |> PersistentVector.nth 100)
                Expect.equal "" 201 (!vector |> PersistentVector.nth 200) }

            test "assoc an element to a nonempty vector should not change the original vector" {
                let v = PersistentVector.empty |> PersistentVector.conj "1" |> PersistentVector.conj "4" |> PersistentVector.conj "25" 

                Expect.equal "assoc" "5" (v |> PersistentVector.update 2 "5" |> PersistentVector.nth 2)
                Expect.equal "assoc" "25" (v |> PersistentVector.nth 2) }

            test "vector should should be convertable to a seq" {
                Expect.equal "" [1;4;25] (PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.conj 4 |> PersistentVector.conj 25  |> Seq.toList) }

            test "vector with 300 elements should be convertable to a seq" {
                let vector = ref PersistentVector.empty
                for i in 1..300 do
                    vector := PersistentVector.conj i (!vector)

                let a = !vector |> Seq.toArray 
                for i in 1..300 do i |> Expect.equal "toSeq toArray" a.[i-1] }

            test "vector can be created from a seq" {
                let xs = [7;88;1;4;25;30] 
                Expect.equal "" xs (PersistentVector.ofSeq xs |> Seq.toList) } 

            test "vector with 300 elements should allow assocN" {
                let vector = ref PersistentVector.empty
                for i in 1..300 do
                    vector := PersistentVector.conj i (!vector)

                for i in 1..300 do
                    vector := PersistentVector.update (i-1) (i*2) (!vector)

                let a = !vector |> Seq.toArray 
                for i in 1..300 do i * 2 |> Expect.equal "assocN" a.[i-1] }

            test "vector of vectors can be accessed with PersistentVector.nthNth" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.equal "" 3 (outer |> PersistentVector.nthNth 0 2)
                Expect.equal "" 5 (outer |> PersistentVector.nthNth 1 4) }

            test "PersistentVector.nthNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.nthNth" (fun () -> PersistentVector.nthNth 2 2 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.nthNth" (fun () -> PersistentVector.nthNth 1 5 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.nthNth" (fun () -> PersistentVector.nthNth -1 2 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.nthNth" (fun () -> PersistentVector.nthNth 1 -2 outer |> ignore) }

            test "vector of vectors can be accessed with PersistentVector.tryNthNth" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.equal "" (Some 3) (outer |> PersistentVector.tryNthNth 0 2)
                Expect.equal "" (Some 5) (outer |> PersistentVector.tryNthNth 1 4) } 

            test "PersistentVector.tryNthNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.isNone "PersistentVector.tryNthNth" (outer |> PersistentVector.tryNthNth 2 2)  
                Expect.isNone "PersistentVector.tryNthNth" (outer |> PersistentVector.tryNthNth 1 5)
                Expect.isNone "PersistentVector.tryNthNth" (outer |> PersistentVector.tryNthNth -1 2)
                Expect.isNone "PersistentVector.tryNthNth" (outer |> PersistentVector.tryNthNth 1 -2) }

            test "vector of vectors can be updated with PersistentVector.updateNth" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.equal "PersistentVector.updateNth" 7 (outer |> PersistentVector.updateNth 0 2 7 |> PersistentVector.nthNth 0 2) 
                Expect.equal "PersistentVector.updateNth" 9 (outer |> PersistentVector.updateNth 1 4 9 |> PersistentVector.nthNth 1 4) } 

            test "PersistentVector.updateNth should not change the original vector" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = ref PersistentVector.empty
                outer := PersistentVector.conj inner (!outer)
                outer := PersistentVector.conj inner (!outer)

                Expect.equal "PersistentVector.updateNth" 7 (!outer |> PersistentVector.updateNth 0 2 7 |> PersistentVector.nthNth 0 2) 
                Expect.equal "PersistentVector.updateNth" 3 (!outer |> PersistentVector.nthNth 0 2) 
                Expect.equal "PersistentVector.updateNth" 9 (!outer |> PersistentVector.updateNth 1 4 9 |> PersistentVector.nthNth 1 4) 
                Expect.equal "PersistentVector.updateNth" 5 (!outer |> PersistentVector.nthNth 1 4) } 

            test "PersistentVector.updateNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.updateNth" (fun () -> PersistentVector.updateNth 0 6 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.updateNth" (fun () -> PersistentVector.updateNth 9 2 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.updateNth" (fun () -> PersistentVector.updateNth 1 -4 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "PersistentVector.updateNth" (fun () -> PersistentVector.updateNth -1 4 7 outer |> ignore) }

            test "PersistentVector.tryUpdateNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                Expect.isNone "" (PersistentVector.tryUpdateNth 0 6 7 outer)
                Expect.isNone "" (PersistentVector.tryUpdateNth 9 2 7 outer)
                Expect.isNone "" (PersistentVector.tryUpdateNth 1 -4 7 outer)
                Expect.isNone "" (PersistentVector.tryUpdateNth -1 4 7 outer) } 

            test "PersistentVector.tryUpdateNth is like PersistentVector.updateNth but returns option" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = PersistentVector.empty |> PersistentVector.conj inner |> PersistentVector.conj inner

                let result = outer |> PersistentVector.tryUpdateNth 0 2 7
                Expect.isSome "PersistentVector.tryUpdateNth" result
                Expect.equal "PersistentVector.tryUpdateNth" 7 (result |> Option.get |> PersistentVector.nthNth 0 2)

                let result2 = outer |> PersistentVector.tryUpdateNth 1 4 9
                Expect.isSome "PersistentVector.tryUpdateNth" result2
                Expect.equal "PersistentVector.tryUpdateNth" 9 (result2 |> Option.get |> PersistentVector.nthNth 1 4) }

            test "PersistentVector.tryUpdateNth should not change the original vector" {
                let inner = [1; 2; 3; 4; 5] |> PersistentVector.ofSeq
                let outer = ref PersistentVector.empty
                outer := PersistentVector.conj inner (!outer)
                outer := PersistentVector.conj inner (!outer)

                Expect.equal "PersistentVector.tryUpdateNth" 7 (!outer |> PersistentVector.tryUpdateNth 0 2 7 |> Option.get |> PersistentVector.nthNth 0 2)
                Expect.equal "PersistentVector.tryUpdateNth" 3 (!outer |> PersistentVector.nthNth 0 2) 
                Expect.equal "PersistentVector.tryUpdateNth" 9 (!outer |> PersistentVector.tryUpdateNth 1 4 9 |> Option.get |> PersistentVector.nthNth 1 4) 
                Expect.equal "PersistentVector.tryUpdateNth" 5 (!outer |> PersistentVector.nthNth 1 4) } 

            test "can peek elements from a vector" {
                let vector = PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.conj 4 |> PersistentVector.conj 25 
                Expect.equal "last" 25 (vector |> PersistentVector.last) }

            test "can pop elements from a vector" {
                let vector = PersistentVector.empty |> PersistentVector.conj 1 |> PersistentVector.conj 4 |> PersistentVector.conj 25 
                Expect.equal "last" 25 (vector |> PersistentVector.last) 
                Expect.equal "last" 4 (vector |> PersistentVector.initial |> PersistentVector.last) 
                Expect.equal "last" 1 (vector |> PersistentVector.initial |> PersistentVector.initial |> PersistentVector.last) 

                Expect.equal "last" 3 (vector |> PersistentVector.length) 
                Expect.equal "last" 2 (vector |> PersistentVector.initial |> PersistentVector.length) 
                Expect.equal "last" 1 (vector |> PersistentVector.initial |> PersistentVector.initial |> PersistentVector.length) } 

            test "vector with 300 elements should allow pop" {
                let vector = ref PersistentVector.empty
                for i in 1..300 do
                    vector := PersistentVector.conj i (!vector)

                for i in 1..300 do
                    vector := PersistentVector.initial (!vector)

                Expect.equal "PersistentVector.initial" [] (!vector |> Seq.toList) }

            test "vector with 3 elements can compute hashcodes" {
                let vector1 = ref PersistentVector.empty
                for i in 1..3 do
                    vector1 := PersistentVector.conj i (!vector1)

                let vector2 = ref PersistentVector.empty
                for i in 1..3 do
                    vector2 := PersistentVector.conj i (!vector2)

                let vector3 = ref PersistentVector.empty
                for i in 1..3 do
                    vector3 := PersistentVector.conj (2*i) (!vector3)

                Expect.equal "GetHashCode" (vector2.GetHashCode()) (vector1.GetHashCode())
                Expect.equal "GetHashCode" (vector2.GetHashCode()) (vector1.GetHashCode()) }

            test "vector with 3 elements can be compared" {
                let vector1 = ref PersistentVector.empty
                for i in 1..3 do
                    vector1 := PersistentVector.conj i (!vector1)

                let vector2 = ref PersistentVector.empty
                for i in 1..3 do
                    vector2 := PersistentVector.conj i (!vector2)

                let vector3 = ref PersistentVector.empty
                for i in 1..3 do
                    vector3 := PersistentVector.conj (2*i) (!vector3)

                Expect.equal "compare" vector1 vector1 
                Expect.equal "compare" vector1 vector2
                Expect.notEqual "compare" vector1 vector3 }

            test "appending two vectors keeps order of items" {
                let vector1 = ref PersistentVector.empty
                for i in 1..3 do
                    vector1 := PersistentVector.conj i (!vector1)

                let vector2 = ref PersistentVector.empty
                for i in 7..9 do
                    vector2 := PersistentVector.conj i (!vector2)

                Expect.equal "append" [1;2;3;7;8;9] (PersistentVector.append (!vector1) (!vector2) |> PersistentVector.toSeq |> Seq.toList) }

            test "vector should allow map" {
                let vector = ref PersistentVector.empty
                for i in 1..300 do
                    vector := PersistentVector.conj i (!vector)

                let vector2 = PersistentVector.map (fun x -> x * 2) (!vector)

                let a = vector2 |> Seq.toArray 
                for i in 1..300 do i * 2 |> Expect.equal "map" a.[i-1] }

            test "vector should allow init" {
                let vector = PersistentVector.init 5 (fun x -> x * 2) 
                let s = Seq.init 5 (fun x -> x * 2)

                Expect.equal "init" [0;2;4;6;8] (s |> Seq.toList)
                Expect.equal "init" [0;2;4;6;8] (vector |> Seq.toList) }

            test "windowSeq should keep every value from its original list" {
                let seq30 = seq { for i in 1..30 do yield i }
                let fullVec = PersistentVector.ofSeq seq30
                for i in 1..35 do
                    let vecs = PersistentVector.windowSeq i seq30
                    Expect.equal "windowSeq" fullVec (vecs |> PersistentVector.fold PersistentVector.append PersistentVector.empty) } 

            test "windowSeq should return vectors of equal length if possible" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len3vecs = PersistentVector.windowSeq 3 seq30
                let len5vecs = PersistentVector.windowSeq 5 seq30
                let len6vecs = PersistentVector.windowSeq 6 seq30

                Expect.equal "windowSeq" 10 (len3vecs |> PersistentVector.length) 
                Expect.equal "windowSeq" 6 (len5vecs |> PersistentVector.length)
                Expect.equal "windowSeq" 5 (len6vecs |> PersistentVector.length)
                Expect.equal "windowSeq" [3;3;3;3;3;3;3;3;3;3] (len3vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [5;5;5;5;5;5] (len5vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList)
                Expect.equal "windowSeq" [6;6;6;6;6] (len6vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList) } 

            test "windowSeq should return vectors all of equal length except the last" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len4vecs = PersistentVector.windowSeq 4 seq30
                let len7vecs = PersistentVector.windowSeq 7 seq30
                let len8vecs = PersistentVector.windowSeq 8 seq30
                let len17vecs = PersistentVector.windowSeq 17 seq30

                Expect.equal "windowSeq" 8 (len4vecs |> PersistentVector.length) 
                Expect.equal "windowSeq" 5 (len7vecs |> PersistentVector.length) 
                Expect.equal "windowSeq" 4 (len8vecs |> PersistentVector.length) 
                Expect.equal "windowSeq" 2 (len17vecs |> PersistentVector.length) 
                Expect.equal "windowSeq" [4;4;4;4;4;4;4;2] (len4vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList)
                Expect.equal "windowSeq" [7;7;7;7;2] (len7vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [8;8;8;6] (len8vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [17;13] (len17vecs |> PersistentVector.map PersistentVector.length |> PersistentVector.toSeq |> Seq.toList) } 

            testList "rangedIterator" [
                test "0..count is same as toSeq" {
                    let vector = PersistentVector.init 3 id
                    let expected = PersistentVector.toSeq vector |> List.ofSeq
                    let actual = PersistentVector.rangedIterator 0 3 vector |> List.ofSeq
                    Expect.equal "0..count is same as toSeq" expected actual
                }

                test "0..0 is empty" {
                    let vector = PersistentVector.init 3 id
                    let expected = List.empty
                    let actual = PersistentVector.rangedIterator 0 0 vector |> List.ofSeq
                    Expect.equal "should be empty" expected actual
                }

                test "1..length-1 skips first and last" {
                    let l = [0;1;2]
                    let vector = PersistentVector.ofSeq l
                    let expected = [1]
                    let actual = PersistentVector.rangedIterator 1 2 vector |> List.ofSeq
                    Expect.equal "should be [1]" expected actual
                }

                test "-1..-2 (negative empty range) throws before iteration starts" {
                    let vector = PersistentVector.init 3 id
                    Expect.throwsT<IndexOutOfRangeException> "should throw" (fun () -> PersistentVector.rangedIterator -1 -2 vector |> ignore)
                }

                test "-2..-1 (1 element range, but before start) throws before iteration starts" {
                    let vector = PersistentVector.init 3 id
                    Expect.throwsT<IndexOutOfRangeException> "should throw" (fun () -> PersistentVector.rangedIterator -2 -1 vector |> ignore)
                }

                test "n..length+m throws when iterating outside bounds" {
                    let vector = PersistentVector.init 3 id
                    let actual = PersistentVector.rangedIterator 0 10 vector // Doesn't throw on creation
                    Expect.throwsT<IndexOutOfRangeException> "should throw" (fun () -> actual |> List.ofSeq |> ignore)
                }
            ]
        ]
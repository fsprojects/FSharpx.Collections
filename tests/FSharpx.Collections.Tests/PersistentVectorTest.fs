namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.PersistentVector
open Expecto
open Expecto.Flip

module PersistentVectorTests =
    let testPersistentVector =

        testList "PersistentVector" [
            test "empty vector should be empty" {
                let x = empty<int>
                Expect.equal "empty" 0 (x |> length) }

            test "multiple cons to an empty vector should increase the count" {
                Expect.equal "cons" 3 (empty |> conj 1 |> conj 4 |> conj 25 |> length) }

            test "cons to an empty vector should create a singleton vector" {
                Expect.equal "cons" 1 (empty |> conj 1 |> nth 0) }

            test "multiple cons to an empty vector should create a vector" {
                Expect.equal "cons" 4 (empty |> conj 1 |> conj 4 |> conj 25 |> nth 1) }

            test "multiple assoc to the end should work like cons and create a vector" {
                let v = empty |> update 0 1 |> update 1 4 |> update 2 25 
                Expect.equal "assoc" 1 (v |> nth 0)
                Expect.equal "assoc" 4 (v |> nth 1)
                Expect.equal "assoc" 25 (v |> nth 2) }

            test "300 cons to an empty vector should create a vector" {
                let vector = ref empty
                for i in 1..300 do
                    vector := conj i (!vector)

                Expect.equal "" 101 (!vector |> nth 100)
                Expect.equal "" 201 (!vector |> nth 200) }

            test "assoc an element to a nonempty vector should not change the original vector" {
                let v = empty |> conj "1" |> conj "4" |> conj "25" 

                Expect.equal "assoc" "5" (v |> update 2 "5" |> nth 2)
                Expect.equal "assoc" "25" (v |> nth 2) }

            test "vector should should be convertable to a seq" {
                Expect.equal "" [1;4;25] (empty |> conj 1 |> conj 4 |> conj 25  |> Seq.toList) }

            test "vector with 300 elements should be convertable to a seq" {
                let vector = ref empty
                for i in 1..300 do
                    vector := conj i (!vector)

                let a = !vector |> Seq.toArray 
                for i in 1..300 do i |> Expect.equal "toSeq toArray" a.[i-1] }

            test "vector can be created from a seq" {
                let xs = [7;88;1;4;25;30] 
                Expect.equal "" xs (ofSeq xs |> Seq.toList) } 

            test "vector with 300 elements should allow assocN" {
                let vector = ref empty
                for i in 1..300 do
                    vector := conj i (!vector)

                for i in 1..300 do
                    vector := update (i-1) (i*2) (!vector)

                let a = !vector |> Seq.toArray 
                for i in 1..300 do i * 2 |> Expect.equal "assocN" a.[i-1] }

            test "vector of vectors can be accessed with nthNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.equal "" 3 (outer |> nthNth 0 2)
                Expect.equal "" 5 (outer |> nthNth 1 4) }

            test "nthNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 2 2 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 1 5 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth -1 2 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "nthNth" (fun () -> nthNth 1 -2 outer |> ignore) }

            test "vector of vectors can be accessed with tryNthNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.equal "" (Some 3) (outer |> tryNthNth 0 2)
                Expect.equal "" (Some 5) (outer |> tryNthNth 1 4) } 

            test "tryNthNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.isNone "tryNthNth" (outer |> tryNthNth 2 2)  
                Expect.isNone "tryNthNth" (outer |> tryNthNth 1 5)
                Expect.isNone "tryNthNth" (outer |> tryNthNth -1 2)
                Expect.isNone "tryNthNth" (outer |> tryNthNth 1 -2) }

            test "vector of vectors can be updated with updateNth" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.equal "updateNth" 7 (outer |> updateNth 0 2 7 |> nthNth 0 2) 
                Expect.equal "updateNth" 9 (outer |> updateNth 1 4 9 |> nthNth 1 4) } 

            test "updateNth should not change the original vector" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = ref empty
                outer := conj inner (!outer)
                outer := conj inner (!outer)

                Expect.equal "updateNth" 7 (!outer |> updateNth 0 2 7 |> nthNth 0 2) 
                Expect.equal "updateNth" 3 (!outer |> nthNth 0 2) 
                Expect.equal "updateNth" 9 (!outer |> updateNth 1 4 9 |> nthNth 1 4) 
                Expect.equal "updateNth" 5 (!outer |> nthNth 1 4) } 

            test "updateNth throws exception for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 0 6 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 9 2 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth 1 -4 7 outer |> ignore)
                Expect.throwsT<System.IndexOutOfRangeException> "updateNth" (fun () -> updateNth -1 4 7 outer |> ignore) }

            test "tryUpdateNth returns None for out-of-bounds indices" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                Expect.isNone "" (tryUpdateNth 0 6 7 outer)
                Expect.isNone "" (tryUpdateNth 9 2 7 outer)
                Expect.isNone "" (tryUpdateNth 1 -4 7 outer)
                Expect.isNone "" (tryUpdateNth -1 4 7 outer) } 

            test "tryUpdateNth is like updateNth but returns option" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = empty |> conj inner |> conj inner

                let result = outer |> tryUpdateNth 0 2 7
                Expect.isSome "tryUpdateNth" result
                Expect.equal "tryUpdateNth" 7 (result |> Option.get |> nthNth 0 2)

                let result2 = outer |> tryUpdateNth 1 4 9
                Expect.isSome "tryUpdateNth" result2
                Expect.equal "tryUpdateNth" 9 (result2 |> Option.get |> nthNth 1 4) }

            test "tryUpdateNth should not change the original vector" {
                let inner = [1; 2; 3; 4; 5] |> ofSeq
                let outer = ref empty
                outer := conj inner (!outer)
                outer := conj inner (!outer)

                Expect.equal "tryUpdateNth" 7 (!outer |> tryUpdateNth 0 2 7 |> Option.get |> nthNth 0 2)
                Expect.equal "tryUpdateNth" 3 (!outer |> nthNth 0 2) 
                Expect.equal "tryUpdateNth" 9 (!outer |> tryUpdateNth 1 4 9 |> Option.get |> nthNth 1 4) 
                Expect.equal "tryUpdateNth" 5 (!outer |> nthNth 1 4) } 

            test "can peek elements from a vector" {
                let vector = empty |> conj 1 |> conj 4 |> conj 25 
                Expect.equal "last" 25 (vector |> last) }

            test "can pop elements from a vector" {
                let vector = empty |> conj 1 |> conj 4 |> conj 25 
                Expect.equal "last" 25 (vector |> last) 
                Expect.equal "last" 4 (vector |> initial |> last) 
                Expect.equal "last" 1 (vector |> initial |> initial |> last) 

                Expect.equal "last" 3 (vector |> length) 
                Expect.equal "last" 2 (vector |> initial |> length) 
                Expect.equal "last" 1 (vector |> initial |> initial |> length) } 

            test "vector with 300 elements should allow pop" {
                let vector = ref empty
                for i in 1..300 do
                    vector := conj i (!vector)

                for i in 1..300 do
                    vector := initial (!vector)

                Expect.equal "initial" [] (!vector |> Seq.toList) }

            test "vector with 3 elements can compute hashcodes" {
                let vector1 = ref empty
                for i in 1..3 do
                    vector1 := conj i (!vector1)

                let vector2 = ref empty
                for i in 1..3 do
                    vector2 := conj i (!vector2)

                let vector3 = ref empty
                for i in 1..3 do
                    vector3 := conj (2*i) (!vector3)

                Expect.equal "GetHashCode" (vector2.GetHashCode()) (vector1.GetHashCode())
                Expect.equal "GetHashCode" (vector2.GetHashCode()) (vector1.GetHashCode()) }

            test "vector with 3 elements can be compared" {
                let vector1 = ref empty
                for i in 1..3 do
                    vector1 := conj i (!vector1)

                let vector2 = ref empty
                for i in 1..3 do
                    vector2 := conj i (!vector2)

                let vector3 = ref empty
                for i in 1..3 do
                    vector3 := conj (2*i) (!vector3)

                Expect.equal "compare" vector1 vector1 
                Expect.equal "compare" vector1 vector2
                Expect.notEqual "compare" vector1 vector3 }

            test "appending two vectors keeps order of items" {
                let vector1 = ref empty
                for i in 1..3 do
                    vector1 := conj i (!vector1)

                let vector2 = ref empty
                for i in 7..9 do
                    vector2 := conj i (!vector2)

                Expect.equal "append" [1;2;3;7;8;9] (append (!vector1) (!vector2) |> toSeq |> Seq.toList) }

            test "vector should allow map" {
                let vector = ref empty
                for i in 1..300 do
                    vector := conj i (!vector)

                let vector2 = map (fun x -> x * 2) (!vector)

                let a = vector2 |> Seq.toArray 
                for i in 1..300 do i * 2 |> Expect.equal "map" a.[i-1] }

            test "vector should allow init" {
                let vector = init 5 (fun x -> x * 2) 
                let s = Seq.init 5 (fun x -> x * 2)

                Expect.equal "init" [0;2;4;6;8] (s |> Seq.toList)
                Expect.equal "init" [0;2;4;6;8] (vector |> Seq.toList) }

            test "windowSeq should keep every value from its original list" {
                let seq30 = seq { for i in 1..30 do yield i }
                let fullVec = ofSeq seq30
                for i in 1..35 do
                    let vecs = windowSeq i seq30
                    Expect.equal "windowSeq" fullVec (vecs |> fold append empty) } 

            test "windowSeq should return vectors of equal length if possible" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len3vecs = windowSeq 3 seq30
                let len5vecs = windowSeq 5 seq30
                let len6vecs = windowSeq 6 seq30

                Expect.equal "windowSeq" 10 (len3vecs |> length) 
                Expect.equal "windowSeq" 6 (len5vecs |> length)
                Expect.equal "windowSeq" 5 (len6vecs |> length)
                Expect.equal "windowSeq" [3;3;3;3;3;3;3;3;3;3] (len3vecs |> map length |> toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [5;5;5;5;5;5] (len5vecs |> map length |> toSeq |> Seq.toList)
                Expect.equal "windowSeq" [6;6;6;6;6] (len6vecs |> map length |> toSeq |> Seq.toList) } 

            test "windowSeq should return vectors all of equal length except the last" {
                let seq30 = seq { for i in 1..30 do yield i }

                let len4vecs = windowSeq 4 seq30
                let len7vecs = windowSeq 7 seq30
                let len8vecs = windowSeq 8 seq30
                let len17vecs = windowSeq 17 seq30

                Expect.equal "windowSeq" 8 (len4vecs |> length) 
                Expect.equal "windowSeq" 5 (len7vecs |> length) 
                Expect.equal "windowSeq" 4 (len8vecs |> length) 
                Expect.equal "windowSeq" 2 (len17vecs |> length) 
                Expect.equal "windowSeq" [4;4;4;4;4;4;4;2] (len4vecs |> map length |> toSeq |> Seq.toList)
                Expect.equal "windowSeq" [7;7;7;7;2] (len7vecs |> map length |> toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [8;8;8;6] (len8vecs |> map length |> toSeq |> Seq.toList) 
                Expect.equal "windowSeq" [17;13] (len17vecs |> map length |> toSeq |> Seq.toList) } 
        ]
module FSharpx.Collections.Experimental.Tests.PersistentVectorTest

open System
open FSharpx.Collections
open FSharpx.Collections.PersistentVector
open NUnit.Framework
open FsUnit

[<Test>]
let ``empty vector should be empty``() =
    let x = empty<int>
    x |> length |> should equal 0

[<Test>]
let ``multiple cons to an empty vector should increase the count``() =
    empty |> conj 1 |> conj 4 |> conj 25 |> length |> should equal 3

[<Test>]
let ``cons to an empty vector should create a singleton vector``() =
    empty |> conj 1 |> nth 0 |> should equal 1

[<Test>]
let ``multiple cons to an empty vector should create a vector``() =
    empty |> conj 1 |> conj 4 |> conj 25 |> nth 1 |> should equal 4

[<Test>]
let ``multiple assoc to the end should work like cons and create a vector``() =
    let v = empty |> update 0 1 |> update 1 4 |> update 2 25 
    v |> nth 0 |> should equal 1
    v |> nth 1 |> should equal 4
    v |> nth 2 |> should equal 25

[<Test>]
let ``300 cons to an empty vector should create a vector``() =
    let vector = ref empty
    for i in 1..300 do
        vector := conj i (!vector)

    !vector |> nth 100 |> should equal 101
    !vector |> nth 200 |> should equal 201

[<Test>]
let ``assoc an element to a nonempty vector should not change the original vector``() =
    let v = empty |> conj "1" |> conj "4" |> conj "25" 

    v |> update 2 "5" |> nth 2 |> should equal "5"
    v |> nth 2 |> should equal "25"

[<Test>]
let ``vector should should be convertable to a seq``() =
    empty |> conj 1 |> conj 4 |> conj 25  |> Seq.toList |> should equal [1;4;25]

[<Test>]
let ``vector with 300 elements should be convertable to a seq``() =
    let vector = ref empty
    for i in 1..300 do
        vector := conj i (!vector)

    let a = !vector |> Seq.toArray 
    for i in 1..300 do i |> should equal a.[i-1]

[<Test>]
let ``vector can be created from a seq``() =
    let xs = [7;88;1;4;25;30] 
    ofSeq xs |> Seq.toList |> should equal xs

[<Test>]
let ``vector with 300 elements should allow assocN``() =
    let vector = ref empty
    for i in 1..300 do
        vector := conj i (!vector)

    for i in 1..300 do
        vector := update (i-1) (i*2) (!vector)

    let a = !vector |> Seq.toArray 
    for i in 1..300 do i * 2 |> should equal a.[i-1]

[<Test>]
let ``vector of vectors can be accessed with nthNth``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    outer |> nthNth 0 2 |> should equal 3
    outer |> nthNth 1 4 |> should equal 5

[<Test>]
let ``nthNth throws exception for out-of-bounds indices``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    (fun () -> nthNth 2 2 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> nthNth 1 5 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> nthNth -1 2 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> nthNth 1 -2 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>

[<Test>]
let ``vector of vectors can be accessed with tryNthNth``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    outer |> tryNthNth 0 2 |> should equal (Some 3)
    outer |> tryNthNth 1 4 |> should equal (Some 5)

[<Test>]
let ``tryNthNth returns None for out-of-bounds indices``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    outer |> tryNthNth 2 2 |> should equal None
    outer |> tryNthNth 1 5 |> should equal None
    outer |> tryNthNth -1 2 |> should equal None
    outer |> tryNthNth 1 -2 |> should equal None

[<Test>]
let ``vector of vectors can be updated with updateNth``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    outer |> updateNth 0 2 7 |> nthNth 0 2 |> should equal 7
    outer |> updateNth 1 4 9 |> nthNth 1 4 |> should equal 9

[<Test>]
let ``updateNth should not change the original vector``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = ref empty
    outer := conj inner (!outer)
    outer := conj inner (!outer)

    !outer |> updateNth 0 2 7 |> nthNth 0 2 |> should equal 7
    !outer |> nthNth 0 2 |> should equal 3
    !outer |> updateNth 1 4 9 |> nthNth 1 4 |> should equal 9
    !outer |> nthNth 1 4 |> should equal 5

[<Test>]
let ``updateNth throws exception for out-of-bounds indices``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    (fun () -> updateNth 0 6 7 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> updateNth 9 2 7 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> updateNth 1 -4 7 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>
    (fun () -> updateNth -1 4 7 outer |> ignore) |> should throw typeof<System.IndexOutOfRangeException>

[<Test>]
let ``tryUpdateNth returns None for out-of-bounds indices``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    tryUpdateNth 0 6 7 outer |> should equal None
    tryUpdateNth 9 2 7 outer |> should equal None
    tryUpdateNth 1 -4 7 outer |> should equal None
    tryUpdateNth -1 4 7 outer |> should equal None

[<Test>]
let ``tryUpdateNth is like updateNth but returns option``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = empty |> conj inner |> conj inner

    let result = outer |> tryUpdateNth 0 2 7
    result |> Option.isSome |> should be True
    result |> Option.get |> nthNth 0 2 |> should equal 7

    let result2 = outer |> tryUpdateNth 1 4 9
    result2 |> Option.isSome |> should be True
    result2 |> Option.get |> nthNth 1 4 |> should equal 9

[<Test>]
let ``tryUpdateNth should not change the original vector``() =
    let inner = [1; 2; 3; 4; 5] |> ofSeq
    let outer = ref empty
    outer := conj inner (!outer)
    outer := conj inner (!outer)

    !outer |> tryUpdateNth 0 2 7 |> Option.get |> nthNth 0 2 |> should equal 7
    !outer |> nthNth 0 2 |> should equal 3
    !outer |> tryUpdateNth 1 4 9 |> Option.get |> nthNth 1 4 |> should equal 9
    !outer |> nthNth 1 4 |> should equal 5

[<Test>]
let ``can peek elements from a vector``() =
    let vector = empty |> conj 1 |> conj 4 |> conj 25 
    vector |> last |> should equal 25
    
[<Test>]
let ``can pop elements from a vector``() =
    let vector = empty |> conj 1 |> conj 4 |> conj 25 
    vector |> last |> should equal 25
    vector |> initial |> last |> should equal 4
    vector |> initial |> initial |> last |> should equal 1

    vector |> length |> should equal 3
    vector |> initial |> length |> should equal 2
    vector |> initial |> initial |> length |> should equal 1

[<Test>]
let ``vector with 300 elements should allow pop``() =
    let vector = ref empty
    for i in 1..300 do
        vector := conj i (!vector)

    for i in 1..300 do
        vector := initial (!vector)

    !vector |> Seq.toList |> should equal []

[<Test>]
let ``vector with 3 elements can compute hashcodes``() =
    let vector1 = ref empty
    for i in 1..3 do
        vector1 := conj i (!vector1)

    let vector2 = ref empty
    for i in 1..3 do
        vector2 := conj i (!vector2)

    let vector3 = ref empty
    for i in 1..3 do
        vector3 := conj (2*i) (!vector3)

    vector1.GetHashCode() |> should equal (vector2.GetHashCode())
    vector1.GetHashCode() |> should equal (vector2.GetHashCode())

[<Test>]
let ``vector with 3 elements can be compared``() =
    let vector1 = ref empty
    for i in 1..3 do
        vector1 := conj i (!vector1)

    let vector2 = ref empty
    for i in 1..3 do
        vector2 := conj i (!vector2)

    let vector3 = ref empty
    for i in 1..3 do
        vector3 := conj (2*i) (!vector3)


    vector1 = vector1 |> should equal true
    vector1 = vector2 |> should equal true
    vector1 = vector3 |> should equal false

[<Test>]
let ``appending two vectors keeps order of items``() =
    let vector1 = ref empty
    for i in 1..3 do
        vector1 := conj i (!vector1)

    let vector2 = ref empty
    for i in 7..9 do
        vector2 := conj i (!vector2)

    append (!vector1) (!vector2) |> toSeq |> Seq.toList |> should equal [1;2;3;7;8;9]

[<Test>]
let ``vector should allow map``() =
    let vector = ref empty
    for i in 1..300 do
        vector := conj i (!vector)

    let vector2 = map (fun x -> x * 2) (!vector)

    let a = vector2 |> Seq.toArray 
    for i in 1..300 do i * 2 |> should equal a.[i-1]

[<Test>]
let ``vector should allow init``() =
    let vector = init 5 (fun x -> x * 2) 
    let s = Seq.init 5 (fun x -> x * 2)

    s |> Seq.toList |> should equal [0;2;4;6;8]
    vector |> Seq.toList |> should equal [0;2;4;6;8]

[<Test>]
let ``windowSeq should keep every value from its original list``() =
    let seq30 = seq { for i in 1..30 do yield i }
    let fullVec = ofSeq seq30
    for i in 1..35 do
        let vecs = windowSeq i seq30
        vecs |> fold append empty |> should equal fullVec

[<Test>]
let ``windowSeq should return vectors of equal length if possible``() =
    let seq30 = seq { for i in 1..30 do yield i }

    let len3vecs = windowSeq 3 seq30
    let len5vecs = windowSeq 5 seq30
    let len6vecs = windowSeq 6 seq30

    len3vecs |> length |> should equal 10
    len5vecs |> length |> should equal 6
    len6vecs |> length |> should equal 5
    len3vecs |> map length |> toSeq |> Seq.toList |> should equal [3;3;3;3;3;3;3;3;3;3]
    len5vecs |> map length |> toSeq |> Seq.toList |> should equal [5;5;5;5;5;5]
    len6vecs |> map length |> toSeq |> Seq.toList |> should equal [6;6;6;6;6]

[<Test>]
let ``windowSeq should return vectors all of equal length except the last``() =
    let seq30 = seq { for i in 1..30 do yield i }

    let len4vecs = windowSeq 4 seq30
    let len7vecs = windowSeq 7 seq30
    let len8vecs = windowSeq 8 seq30
    let len17vecs = windowSeq 17 seq30

    len4vecs |> length |> should equal 8
    len7vecs |> length |> should equal 5
    len8vecs |> length |> should equal 4
    len17vecs |> length |> should equal 2
    len4vecs |> map length |> toSeq |> Seq.toList |> should equal [4;4;4;4;4;4;4;2]
    len7vecs |> map length |> toSeq |> Seq.toList |> should equal [7;7;7;7;2]
    len8vecs |> map length |> toSeq |> Seq.toList |> should equal [8;8;8;6]
    len17vecs |> map length |> toSeq |> Seq.toList |> should equal [17;13]

module FSharpx.Collections.Experimental.Tests.PersistentHashMapTest

open System
open FSharpx.Collections
open FSharpx.Collections.PersistentHashMap
open NUnit.Framework
open FsUnit

[<Test>]
let ``empty map should be empty``() =
    let x = empty<int>
    x |> length |> should equal 0


[<Test>]
let ``empty map should not contain key 0``() =
    let x = empty
    x |> containsKey 1 |> should equal false

[<Test>]
let ``can add null entry to empty map``() =
    let x = empty
    x |> containsKey "value" |> should equal false
    x |> containsKey null |> should equal false
    x |> add null "Hello" |> containsKey null |> should equal true


[<Test>]
let ``can add empty string as key to empty map``() =
    let x = empty
    x |> containsKey "" |> should equal false
    x |> add "" "Hello" |> containsKey null |> should equal false
    x |> add "" "Hello" |> containsKey "" |> should equal true

[<Test>]
let ``can add some integers to empty map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
            
    x |> containsKey 1 |> should equal true
    x |> containsKey 5 |> should equal true
    x |> containsKey 6 |> should equal false


[<Test>]
let ``can add tons of integers to empty map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add i i !x

    for i in 0 .. counter do 
        !x |> containsKey i |> should equal true
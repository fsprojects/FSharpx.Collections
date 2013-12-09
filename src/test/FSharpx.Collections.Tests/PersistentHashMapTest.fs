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
    let x = empty<int>
    x |> containsKey 1 |> should equal false

[<Test>]
let ``can add null entry to empty map``() =
    let x = empty<string>
    x |> containsKey "value" |> should equal false
    x |> containsKey null |> should equal false
    x |> add null "Hello" |> containsKey null |> should equal true
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

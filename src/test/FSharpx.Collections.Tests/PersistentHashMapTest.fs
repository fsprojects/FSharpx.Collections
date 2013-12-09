module FSharpx.Collections.Experimental.Tests.PersistentHashMapTest

open System
open FSharpx.Collections
open FSharpx.Collections.PersistentHashMap
open NUnit.Framework
open FsUnit

[<Test>]
let ``empty map should be empty``() =
    let x = empty<int,int>
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
            
    x |> containsKey 1 |> shouldEqual true
    x |> containsKey 5 |> shouldEqual true
    x |> containsKey 6 |> shouldEqual false

[<Test>]
let ``can lookup integers from a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "l"
    x |> find 5 |> shouldEqual "o"

[<Test>]
let ``can add tons of integers to empty map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add i i !x

    for i in 0 .. counter do 
        !x |> containsKey i |> should equal true

[<Test>]
let ``can lookup tons of integers form a map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add i i !x

    for i in 0 .. counter do 
        !x |> find i |> shouldEqual i

[<CustomComparison; CustomEquality>]
type AlwaysSameHash = 
    { Name  : string; }

    interface System.IComparable<AlwaysSameHash> with
        member this.CompareTo { Name = name } =
            compare this.Name name

    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? AlwaysSameHash as other -> (this :> IComparable<_>).CompareTo other
            | _                    -> invalidArg "obj" "not a AlwaysSameHash"

    interface IEquatable<AlwaysSameHash> with
        member this.Equals { Name = name  } = this.Name = name

    override this.Equals obj =
        match obj with
        | :? AlwaysSameHash as other -> (this :> IEquatable<_>).Equals other
        | _                    -> invalidArg "obj" "not a AlwaysSameHash"

    override this.GetHashCode () = 42

[<Test>]
let ``two AlwaysSameHash have same hash``() =
    hash { Name = "Test"} |> should equal (hash { Name = "Test"})
    { Name = "Test"} |> should equal { Name = "Test"}
    hash { Name = "Test"} |> should equal (hash { Name = "Test1"})
    { Name = "Test"} |> shouldNotEqual { Name = "Test1"}


[<Test>]
let ``can add keys with colliding hashes to empty map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        empty 
        |> add x x.Name 
        |> add y y.Name
    
    map |> containsKey x |> should equal true
    map |> containsKey y |> should equal true

    empty |> containsKey y |> should equal false


[<Test>]
let ``can lookup keys with colliding hashes from map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        empty 
        |> add x x
        |> add y y
    
    map |> find x |> shouldEqual { Name = "Test"}
    map |> find y |> shouldEqual { Name = "Test1"}


[<Test>]
let ``can add lots of keys with colliding hashes to empty map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add { Name = i.ToString() } i !x

    for i in 0 .. counter do 
        !x |> containsKey { Name = i.ToString() } |> should equal true
module FSharpx.Collections.Experimental.Tests.TransientHashMapTest

open System
open FSharpx.Collections
open FSharpx.Collections.PersistentHashMap
open NUnit.Framework
open FsUnit

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
let ``empty map should be empty``() =
    let x = TransientHashMap<int,int>.Empty()
    x.persistent() |> length |> should equal 0


[<Test>]
let ``empty map should not contain key 0``() =
    let x = TransientHashMap<int,int>.Empty()
    x.persistent() |> containsKey 1 |> should equal false

[<Test>]
let ``can add null entry to empty map``() =
    let x = TransientHashMap<string,string>.Empty()
    x.persistent() |> containsKey "value" |> should equal false
    x.persistent() |> containsKey null |> should equal false
    x.Add(null,"Hello").persistent() |> containsKey null |> should equal true


[<Test>]
let ``can add empty string as key to empty map``() =
    let x = TransientHashMap<string,string>.Empty()
    x.persistent() |> containsKey "" |> should equal false
    x.Add("","Hello").persistent() |> containsKey null |> should equal false
    x.Add("","Hello").persistent() |> containsKey "" |> should equal true

[<Test>]
let ``can add some integers to empty map``() =
    let x =
        TransientHashMap<int,string>.Empty()
            .Add(1,"h")
            .Add(2,"a")
            .Add(3,"l")
            .Add(4,"l")
            .Add(5,"o")
            .persistent()

    x |> containsKey 1 |> shouldEqual true
    x |> containsKey 5 |> shouldEqual true
    x |> containsKey 6 |> shouldEqual false


[<Test>]
let ``can remove some integers from a map``() =
    let x =
        TransientHashMap<int,string>.Empty()
            .Add(1,"h")
            .Add(2,"a")
            .Add(3,"l")
            .Add(4,"l")
            .Add(5,"o")
            .Remove(1)
            .Remove(4)
            .persistent()
        
            
    x |> containsKey 1 |> shouldEqual false
    x |> containsKey 4 |> shouldEqual false
    x |> containsKey 5 |> shouldEqual true
    x |> containsKey 6 |> shouldEqual false

[<Test>]
let ``can find integers in a map``() =
    let x =
        TransientHashMap<int,string>.Empty()
            .Add(1,"h")
            .Add(2,"a")
            .Add(3,"l")
            .Add(4,"l")
            .Add(5,"o")
            .persistent()
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "l"
    x |> find 5 |> shouldEqual "o"

[<Test>]
let ``can lookup integers from a map``() =
    let x =
        TransientHashMap<int,string>.Empty()
            .Add(1,"h")
            .Add(2,"a")
            .Add(3,"l")
            .Add(4,"l")
            .Add(5,"o")
            
    x.[1] |> shouldEqual "h"
    x.[4] |> shouldEqual "l"
    x.[5] |> shouldEqual "o"


[<Test>]
let ``can add the same key multiple to a map``() =
    let x =
        TransientHashMap<int,string>.Empty()
            .Add(1,"h")
            .Add(2,"a")
            .Add(3,"l")
            .Add(4,"l")
            .Add(5,"o")
            .Add(4,"a")
            .Add(5,"o")
            .persistent()
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "a"
    x |> find 5 |> shouldEqual "o"

    
[<Test>]
let ``can add tons of integers to empty map``() =
    let x = ref (TransientHashMap<int,int>.Empty())
    let counter = 1000

    for i in 0 .. counter do 
        x := (!x).Add(i,i)

    let x = (!x).persistent()
    for i in 0 .. counter do 
       x |> containsKey i |> should equal true

[<Test>]
let ``can lookup tons of integers from a map``() =
    let x = ref (TransientHashMap<int,int>.Empty())
    let counter = 1000

    for i in 0 .. counter do 
        x := (!x).Add(i,i)

    for i in 0 .. counter do 
        (!x).[i] |> shouldEqual i

[<Test>]
let ``can find tons of integers in a map``() =
    let x = ref (TransientHashMap<int,int>.Empty())
    let counter = 1000

    for i in 0 .. counter do 
        x := (!x).Add(i,i)

    for i in 0 .. counter do 
        (!x).[i] |> shouldEqual i


[<Test>]
let ``can add keys with colliding hashes to empty map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        TransientHashMap<AlwaysSameHash,string>.Empty() 
            .Add(x,x.Name)
            .Add(y,y.Name)
            .persistent()
    
    map |> containsKey x |> should equal true
    map |> containsKey y |> should equal true

    empty |> containsKey y |> should equal false


[<Test>]
let ``can lookup keys with colliding hashes from map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        TransientHashMap<AlwaysSameHash,AlwaysSameHash>.Empty() 
            .Add(x,x)
            .Add(y,y)
            .persistent()
    
    map |> find x |> shouldEqual { Name = "Test"}
    map |> find y |> shouldEqual { Name = "Test1"}

[<Test>]
let ``can add lots of keys with colliding hashes to empty map``() =
    let x = ref (TransientHashMap<AlwaysSameHash,int>.Empty() )
    let counter = 1000

    for i in 0 .. counter do 
        x := (!x).Add({ Name = i.ToString() },i)

    let x = (!x).persistent()
    for i in 0 .. counter do 
        x |> containsKey { Name = i.ToString() } |> should equal true
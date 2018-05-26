namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Expecto
open Expecto.Flip

module TransientHashMapTests =
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

        override __.GetHashCode () = 42

    let testTransientHashMap =

        testList "TransientHashMap" [

            test "two AlwaysSameHash have same hash" {
                Expect.equal "AlwaysSameHash" (hash { Name = "Test"}) (hash { Name = "Test"})
                Expect.equal "AlwaysSameHash" { Name = "Test"} { Name = "Test"} 
                Expect.equal "AlwaysSameHash" (hash { Name = "Test1"}) (hash { Name = "Test"})
                Expect.notEqual "AlwaysSameHash" { Name = "Test1"} { Name = "Test"} }

            test "empty map should be empty" {
                let x = TransientHashMap<int,int>.Empty()
                Expect.equal "empty" 0 (x.persistent() |> PersistentHashMap.length) } 

            test "empty map should not contain key 0" {
                let x = TransientHashMap<int,int>.Empty()
                Expect.isFalse "empty" (x.persistent() |> PersistentHashMap.containsKey 1) }

            test "can add null entry to empty map" {
                let x = TransientHashMap<string,string>.Empty()
                Expect.isFalse "PersistentHashMap.containsKey" (x.persistent() |> PersistentHashMap.containsKey "value")  
                Expect.isFalse "PersistentHashMap.containsKey" (x.persistent() |> PersistentHashMap.containsKey null) 
                Expect.isTrue "PersistentHashMap.containsKey" (x.Add(null,"Hello").persistent() |> PersistentHashMap.containsKey null) } 

            test "can add empty string as key to empty map" {
                let x = TransientHashMap<string,string>.Empty()
                Expect.isFalse "" (x.persistent() |> PersistentHashMap.containsKey "") 
                Expect.isFalse ""  (x.Add("","Hello").persistent() |> PersistentHashMap.containsKey null)
                Expect.isTrue "" (x.Add("","Hello").persistent() |> PersistentHashMap.containsKey "") }  

            test "can add some integers to empty map" {
                let x =
                    TransientHashMap<int,string>.Empty()
                        .Add(1,"h")
                        .Add(2,"a")
                        .Add(3,"l")
                        .Add(4,"l")
                        .Add(5,"o")
                        .persistent()

                Expect.isTrue "PersistentHashMap.containsKey" (x |> PersistentHashMap.containsKey 1)
                Expect.isTrue "PersistentHashMap.containsKey" (x |> PersistentHashMap.containsKey 5)
                Expect.isFalse "PersistentHashMap.containsKey" (x |> PersistentHashMap.containsKey 6) } 

            test "can remove some integers from a map" {
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

                Expect.isFalse "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey 1 x 
                Expect.isFalse "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey 4 x 
                Expect.isTrue "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey 5 x 
                Expect.isFalse "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey 6 x }

            test "can find integers in a map" {
                let x =
                    TransientHashMap<int,string>.Empty()
                        .Add(1,"h")
                        .Add(2,"a")
                        .Add(3,"l")
                        .Add(4,"l")
                        .Add(5,"o")
                        .persistent()
            
                Expect.equal "find" "h" (x |> PersistentHashMap.find 1)
                Expect.equal "find" "l" (x |> PersistentHashMap.find 4)
                Expect.equal "find" "o" (x |> PersistentHashMap.find 5) }

            test "can lookup integers from a map" {
                let x =
                    TransientHashMap<int,string>.Empty()
                        .Add(1,"h")
                        .Add(2,"a")
                        .Add(3,"l")
                        .Add(4,"l")
                        .Add(5,"o")
            
                Expect.equal "lookup" "h" x.[1]
                Expect.equal "lookup" "l" x.[4]
                Expect.equal "lookup" "o" x.[5] }

            test "can add the same key multiple to a map" {
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
            
                Expect.equal "" "h" <| PersistentHashMap.find 1 x 
                Expect.equal "" "a" <| PersistentHashMap.find 4 x 
                Expect.equal "" "o" <| PersistentHashMap.find 5 x }

            test "can add tons of integers to empty map" {
                let x = ref (TransientHashMap<int,int>.Empty())
                let counter = 1000

                for i in 0 .. counter do 
                    x := (!x).Add(i,i)

                let x = (!x).persistent()
                for i in 0 .. counter do 
                   x |> PersistentHashMap.containsKey i |> Expect.isTrue "lookup" }

            test "can lookup tons of integers from a map" {
                let x = ref (TransientHashMap<int,int>.Empty())
                let counter = 1000

                for i in 0 .. counter do 
                    x := (!x).Add(i,i)

                for i in 0 .. counter do 
                    (!x).[i] |> Expect.equal "lookup" i }

            test "can find tons of integers in a map" {
                let x = ref (TransientHashMap<int,int>.Empty())
                let counter = 1000

                for i in 0 .. counter do 
                    x := (!x).Add(i,i)

                for i in 0 .. counter do 
                    (!x).[i] |> Expect.equal "lookup" i }

            test "can add keys with colliding hashes to empty map" {
                let x = { Name = "Test"}
                let y = { Name = "Test1"}
                let map = 
                    TransientHashMap<AlwaysSameHash,string>.Empty() 
                        .Add(x,x.Name)
                        .Add(y,y.Name)
                        .persistent()
    
                Expect.isTrue "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey x map 
                Expect.isTrue "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey y map  

                Expect.isFalse "PersistentHashMap.containsKey" <| PersistentHashMap.containsKey y PersistentHashMap.empty }

            test "can lookup keys with colliding hashes from map" {
                let x = { Name = "Test"}
                let y = { Name = "Test1"}
                let map = 
                    TransientHashMap<AlwaysSameHash,AlwaysSameHash>.Empty() 
                        .Add(x,x)
                        .Add(y,y)
                        .persistent()
    
                Expect.equal "find" { Name = "Test"} (map |> PersistentHashMap.find x)
                Expect.equal "find" { Name = "Test1"} (map |> PersistentHashMap.find y) }

            test "can add lots of keys with colliding hashes to empty map" {
                let x = ref (TransientHashMap<AlwaysSameHash,int>.Empty() )
                let counter = 1000

                for i in 0 .. counter do 
                    x := (!x).Add({ Name = i.ToString() },i)

                let x = (!x).persistent()
                for i in 0 .. counter do 
                    x |> PersistentHashMap.containsKey { Name = i.ToString() } |> Expect.isTrue "PersistentHashMap.containsKey" }
        ]
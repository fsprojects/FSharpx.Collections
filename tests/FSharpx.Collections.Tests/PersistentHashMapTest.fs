namespace FSharpx.Collections.Tests



open System
open FSharpx.Collections
open FSharpx.Collections.Tests.TransientHashMapTests
open FSharpx.Collections.PersistentHashMap
open Expecto
open Expecto.Flip

module PersistentHashMapTests =
    let testPersistentHashMap =

        testList "PersistentHashMap" [

            test "empty map should be empty" {
                let x = empty<int,int>
                Expect.equal "length" 0 <| length x }

            test "empty map should not contain key 0" {
                let x = empty
                Expect.isFalse "containsKey" <| containsKey 1 x } 

            test "can add null entry to empty map" {
                Expect.isFalse "empty" <| containsKey "value" empty
                Expect.isFalse "empty" <| containsKey null empty
                Expect.isTrue "empty" (add null "Hello" empty |> containsKey null) }

            test "can add empty string as key to empty map" {
                Expect.isFalse "empty" <| containsKey "" empty
                Expect.isFalse "empty" (add "" "Hello" empty |> containsKey null)
                Expect.isTrue "empty" (add "" "Hello" empty |> containsKey "")
                Expect.equal "empty" 1 (add "" "Hello" empty |> length) }

            test "can add some integers to empty map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"

                Expect.isTrue "add" <| containsKey 1 x
                Expect.isTrue "add" <| containsKey 5 x
                Expect.isFalse "add" <| containsKey 6 x
                Expect.equal "add" 5 <| length x }

            test "add operates immutable" {
                let y =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                let x =
                    y
                    |> add 4 "l"
                    |> add 5 "o"

                Expect.equal "" 3 <| length y
                Expect.equal "" 5 <| length x }

            test "can remove some integers from a map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
                    |> remove 1
                    |> remove 4
            
                Expect.isFalse "add remove" <| containsKey 1 x
                Expect.isFalse "add remove" <| containsKey 4 x
                Expect.isTrue "add remove" <| containsKey 5 x
                Expect.isFalse "add remove" <| containsKey 6 x
                Expect.equal "add remove" 3 <| length x }

            test "remove operates" {
                let y =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
                let x =
                    y
                    |> remove 1
                    |> remove 4
            
                Expect.equal "remove" 3 <| length x
                Expect.equal "add" 5 <| length y }

            test "can find integers in a map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
            
                Expect.equal "add" "h" <| find 1 x
                Expect.equal "add" "l" <| find 4 x
                Expect.equal "add" "o" <| find 5 x }

            test "can lookup integers from a map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
            
                Expect.equal "lookup" "h" x.[1]
                Expect.equal "lookup" "l" x.[4]
                Expect.equal "lookup" "o" x.[5] }

            test "can add the same key multiple to a map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
                    |> add 3 "a"
                    |> add 4 "a"
            
                Expect.equal "find" "h" <| find 1 x
                Expect.equal "find" "a" <| find 4 x
                Expect.equal "find" "o" <| find 5 x
                Expect.equal "length" 5 <| length x }

            test "can iterate through a map" {
                let x =
                    empty
                    |> add 1 "h"
                    |> add 2 "a"
                    |> add 3 "l"
                    |> add 4 "l"
                    |> add 5 "o"
            
                Expect.equal "find" "h" <| find 1 x
                Expect.equal "find" "l" <| find 4 x
                Expect.equal "find" "o" <| find 5 x }

            test "can convert a seq to a map" {
                let list = [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"]
                Expect.equal "ofSeq" [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"] (ofSeq list |> toSeq |> Seq.toList) }

            test "a map is always sorter" {
                let list = [ 4,"l"; 5,"o"; 2,"a"; 1,"h"; 3,"l"]
                Expect.equal "toSeq" [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"] (ofSeq list |> toSeq |> Seq.toList) }

            test "can map a HashMap" {
                let x =
                    empty
                    |> add 1 1
                    |> add 2 2
                    |> add 3 3
                    |> add 4 4
                    |> add 5 5
            
                Expect.equal "map" [1,2; 2,3; 3,4; 4,5; 5,6] (x |> map (fun x -> x + 1) |> Seq.toList) }

            test "can add tons of integers to empty map" {
                let x = ref empty
                let counter = 1000

                for i in 0 .. counter do 
                    x := add i i !x

                for i in 0 .. counter do 
                    !x |> containsKey i |> Expect.isTrue "containsKey" }

            test "can find tons of integers in a map" {
                let x = ref empty
                let counter = 1000

                for i in 0 .. counter do 
                    x := add i i !x

                for i in 0 .. counter do 
                    !x |> find i |> Expect.equal "find" i }

            test "can add keys with colliding hashes to empty map" {
                let x = { Name = "Test"}
                let y = { Name = "Test1"}
                let map = 
                    empty 
                    |> add x x.Name 
                    |> add y y.Name
    
                Expect.isTrue "" <| containsKey x map
                Expect.isTrue "" <| containsKey y map
                Expect.isFalse "" <| containsKey y empty }

            test "can lookup keys with colliding hashes from map" {
                let x = { Name = "Test"}
                let y = { Name = "Test1"}
                let map = 
                    empty 
                    |> add x x
                    |> add y y
    
                Expect.equal "colliding hashes" { Name = "Test"} <| find x map
                Expect.equal "colliding hashes" { Name = "Test1"} <| find y map }

            test "can add lots of keys with colliding hashes to empty map" {
                let x = ref empty
                let counter = 1000

                for i in 0 .. counter do 
                    x := add { Name = i.ToString() } i !x

                for i in 0 .. counter do 
                    !x |> containsKey { Name = i.ToString() } |> Expect.isTrue "colliding hashes" }

            test "can find tons of strings in a map" {
                let x = ref empty
                let n = 10000
                let r = new Random()

                for i in 0 .. n do 
                    x := add (i.ToString()) i !x

                for i in 0 .. 1000000 do 
                    !x |> containsKey ((r.Next n).ToString()) |> Expect.isTrue "Next" }
        ]
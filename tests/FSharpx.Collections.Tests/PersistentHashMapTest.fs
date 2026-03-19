namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.Tests.TransientHashMapTests
open Expecto
open Expecto.Flip

module PersistentHashMapTests =
    [<Tests>]
    let testPersistentHashMap =

        testList
            "PersistentHashMap"
            [

              test "PersistentHashMap.empty map should be PersistentHashMap.empty" {
                  let x = PersistentHashMap.empty<int, int>
                  Expect.equal "length" 0 <| PersistentHashMap.length x
              }

              test "PersistentHashMap.empty map should not contain key 0" {
                  let x = PersistentHashMap.empty

                  Expect.isFalse "PersistentHashMap.containsKey"
                  <| PersistentHashMap.containsKey 1 x
              }

              test "can PersistentHashMap.add null entry to PersistentHashMap.empty map" {
                  Expect.isFalse "PersistentHashMap.empty"
                  <| PersistentHashMap.containsKey "value" PersistentHashMap.empty

                  Expect.isFalse "PersistentHashMap.empty"
                  <| PersistentHashMap.containsKey null PersistentHashMap.empty

                  Expect.isTrue
                      "PersistentHashMap.empty"
                      (PersistentHashMap.add null "Hello" PersistentHashMap.empty
                       |> PersistentHashMap.containsKey null)
              }

              //https://github.com/fsprojects/FSharpx.Collections/issues/85
              ptest "can add None value to empty map" {
                  let x = PersistentHashMap<string, string option>.Empty()

                  Expect.isTrue "PersistentHashMap.containsKey" (x.Add("Hello", None) |> PersistentHashMap.containsKey "Hello")
              }

              test "can PersistentHashMap.add PersistentHashMap.empty string as key to PersistentHashMap.empty map" {
                  Expect.isFalse "PersistentHashMap.empty"
                  <| PersistentHashMap.containsKey "" PersistentHashMap.empty

                  Expect.isFalse
                      "PersistentHashMap.empty"
                      (PersistentHashMap.add "" "Hello" PersistentHashMap.empty
                       |> PersistentHashMap.containsKey null)

                  Expect.isTrue
                      "PersistentHashMap.empty"
                      (PersistentHashMap.add "" "Hello" PersistentHashMap.empty
                       |> PersistentHashMap.containsKey "")

                  Expect.equal
                      "PersistentHashMap.empty"
                      1
                      (PersistentHashMap.add "" "Hello" PersistentHashMap.empty
                       |> PersistentHashMap.length)
              }

              test "can PersistentHashMap.add some integers to PersistentHashMap.empty map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"

                  Expect.isTrue "PersistentHashMap.add"
                  <| PersistentHashMap.containsKey 1 x

                  Expect.isTrue "PersistentHashMap.add"
                  <| PersistentHashMap.containsKey 5 x

                  Expect.isFalse "PersistentHashMap.add"
                  <| PersistentHashMap.containsKey 6 x

                  Expect.equal "PersistentHashMap.add" 5 <| PersistentHashMap.length x
              }

              test "PersistentHashMap.add operates immutable" {
                  let y =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"

                  let x = y |> PersistentHashMap.add 4 "l" |> PersistentHashMap.add 5 "o"

                  Expect.equal "" 3 <| PersistentHashMap.length y
                  Expect.equal "" 5 <| PersistentHashMap.length x
              }

              test "can remove some integers from a map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"
                      |> PersistentHashMap.remove 1
                      |> PersistentHashMap.remove 4

                  Expect.isFalse "PersistentHashMap.add remove"
                  <| PersistentHashMap.containsKey 1 x

                  Expect.isFalse "PersistentHashMap.add remove"
                  <| PersistentHashMap.containsKey 4 x

                  Expect.isTrue "PersistentHashMap.add remove"
                  <| PersistentHashMap.containsKey 5 x

                  Expect.isFalse "PersistentHashMap.add remove"
                  <| PersistentHashMap.containsKey 6 x

                  Expect.equal "PersistentHashMap.add remove" 3
                  <| PersistentHashMap.length x
              }

              test "remove operates" {
                  let y =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"

                  let x = y |> PersistentHashMap.remove 1 |> PersistentHashMap.remove 4

                  Expect.equal "remove" 3 <| PersistentHashMap.length x
                  Expect.equal "PersistentHashMap.add" 5 <| PersistentHashMap.length y
              }

              test "can find integers in a map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"

                  Expect.equal "PersistentHashMap.add" "h" <| PersistentHashMap.find 1 x
                  Expect.equal "PersistentHashMap.add" "l" <| PersistentHashMap.find 4 x
                  Expect.equal "PersistentHashMap.add" "o" <| PersistentHashMap.find 5 x
              }

              test "can lookup integers from a map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"

                  Expect.equal "lookup" "h" x.[1]
                  Expect.equal "lookup" "l" x.[4]
                  Expect.equal "lookup" "o" x.[5]
              }

              test "can PersistentHashMap.add the same key multiple to a map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"
                      |> PersistentHashMap.add 3 "a"
                      |> PersistentHashMap.add 4 "a"

                  Expect.equal "find" "h" <| PersistentHashMap.find 1 x
                  Expect.equal "find" "a" <| PersistentHashMap.find 4 x
                  Expect.equal "find" "o" <| PersistentHashMap.find 5 x
                  Expect.equal "length" 5 <| PersistentHashMap.length x
              }

              test "can iterate through a map" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 "h"
                      |> PersistentHashMap.add 2 "a"
                      |> PersistentHashMap.add 3 "l"
                      |> PersistentHashMap.add 4 "l"
                      |> PersistentHashMap.add 5 "o"

                  Expect.equal "find" "h" <| PersistentHashMap.find 1 x
                  Expect.equal "find" "l" <| PersistentHashMap.find 4 x
                  Expect.equal "find" "o" <| PersistentHashMap.find 5 x
              }

              test "can convert a seq to a map" {
                  let list = [ 1, "h"; 2, "a"; 3, "l"; 4, "l"; 5, "o" ]

                  Expect.equal
                      "ofSeq"
                      [ 1, "h"; 2, "a"; 3, "l"; 4, "l"; 5, "o" ]
                      (PersistentHashMap.ofSeq list |> PersistentHashMap.toSeq |> Seq.toList)
              }

              test "a map is always sorter" {
                  let list = [ 4, "l"; 5, "o"; 2, "a"; 1, "h"; 3, "l" ]

                  Expect.equal
                      "toSeq"
                      [ 1, "h"; 2, "a"; 3, "l"; 4, "l"; 5, "o" ]
                      (PersistentHashMap.ofSeq list |> PersistentHashMap.toSeq |> Seq.toList)
              }

              test "can map a HashMap" {
                  let x =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add 1 1
                      |> PersistentHashMap.add 2 2
                      |> PersistentHashMap.add 3 3
                      |> PersistentHashMap.add 4 4
                      |> PersistentHashMap.add 5 5

                  Expect.equal "map" [ 1, 2; 2, 3; 3, 4; 4, 5; 5, 6 ] (x |> PersistentHashMap.map(fun x -> x + 1) |> Seq.toList)
              }

              test "can PersistentHashMap.add tons of integers to PersistentHashMap.empty map" {
                  let x = ref PersistentHashMap.empty
                  let counter = 1000

                  for i in 0..counter do
                      x := PersistentHashMap.add i i !x

                  for i in 0..counter do
                      !x
                      |> PersistentHashMap.containsKey i
                      |> Expect.isTrue "PersistentHashMap.containsKey"
              }

              test "can find tons of integers in a map" {
                  let x = ref PersistentHashMap.empty
                  let counter = 1000

                  for i in 0..counter do
                      x := PersistentHashMap.add i i !x

                  for i in 0..counter do
                      !x |> PersistentHashMap.find i |> Expect.equal "find" i
              }

              test "can PersistentHashMap.add keys with colliding hashes to PersistentHashMap.empty map" {
                  let x = { Name = "Test" }
                  let y = { Name = "Test1" }

                  let map =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add x x.Name
                      |> PersistentHashMap.add y y.Name

                  Expect.isTrue "" <| PersistentHashMap.containsKey x map
                  Expect.isTrue "" <| PersistentHashMap.containsKey y map

                  Expect.isFalse ""
                  <| PersistentHashMap.containsKey y PersistentHashMap.empty
              }

              test "can lookup keys with colliding hashes from map" {
                  let x = { Name = "Test" }
                  let y = { Name = "Test1" }

                  let map =
                      PersistentHashMap.empty
                      |> PersistentHashMap.add x x
                      |> PersistentHashMap.add y y

                  Expect.equal "colliding hashes" { Name = "Test" }
                  <| PersistentHashMap.find x map

                  Expect.equal "colliding hashes" { Name = "Test1" }
                  <| PersistentHashMap.find y map
              }

              test "can PersistentHashMap.add lots of keys with colliding hashes to PersistentHashMap.empty map" {
                  let x = ref PersistentHashMap.empty
                  let counter = 1000

                  for i in 0..counter do
                      x := PersistentHashMap.add { Name = i.ToString() } i !x

                  for i in 0..counter do
                      !x
                      |> PersistentHashMap.containsKey { Name = i.ToString() }
                      |> Expect.isTrue "colliding hashes"
              }

              test "can find tons of strings in a map" {
                  let x = ref PersistentHashMap.empty
                  let n = 10000
                  let r = new Random()

                  for i in 0..n do
                      x := PersistentHashMap.add (i.ToString()) i !x

                  for i in 0..1000000 do
                      !x
                      |> PersistentHashMap.containsKey((r.Next n).ToString())
                      |> Expect.isTrue "Next"
              }

              test "tryFind returns Some for existing key" {
                  let m = PersistentHashMap.ofSeq [ ("a", 1); ("b", 2) ]
                  Expect.equal "tryFind existing" (Some 1) (PersistentHashMap.tryFind "a" m)
              }

              test "tryFind returns None for missing key" {
                  let m = PersistentHashMap.ofSeq [ ("a", 1) ]
                  Expect.equal "tryFind missing" None (PersistentHashMap.tryFind "z" m)
              }

              test "filter keeps only matching entries" {
                  let m = PersistentHashMap.ofSeq [ (1, "a"); (2, "b"); (3, "c") ]
                  let result = PersistentHashMap.filter (fun k _ -> k > 1) m
                  Expect.equal "filter count" 2 (PersistentHashMap.count result)
                  Expect.isFalse "filter excludes 1" (PersistentHashMap.containsKey 1 result)
                  Expect.isTrue "filter keeps 2" (PersistentHashMap.containsKey 2 result)
              }

              test "iter visits all entries" {
                  let m = PersistentHashMap.ofSeq [ (1, 10); (2, 20); (3, 30) ]
                  let seen = System.Collections.Generic.HashSet<int>()
                  PersistentHashMap.iter (fun k _ -> seen.Add(k) |> ignore) m
                  Expect.equal "iter visits all" 3 seen.Count
              }

              test "fold sums all values" {
                  let m = PersistentHashMap.ofSeq [ ("a", 1); ("b", 2); ("c", 3) ]
                  let total = PersistentHashMap.fold (fun acc _ v -> acc + v) 0 m
                  Expect.equal "fold sum" 6 total
              }

              test "exists returns true when predicate matches" {
                  let m = PersistentHashMap.ofSeq [ (1, "x"); (2, "y") ]
                  Expect.isTrue "exists" (PersistentHashMap.exists (fun k _ -> k = 2) m)
              }

              test "exists returns false when no match" {
                  let m = PersistentHashMap.ofSeq [ (1, "x"); (2, "y") ]
                  Expect.isFalse "exists false" (PersistentHashMap.exists (fun k _ -> k = 99) m)
              }

              test "forall returns true when all entries match" {
                  let m = PersistentHashMap.ofSeq [ (1, 10); (2, 20) ]
                  Expect.isTrue "forall" (PersistentHashMap.forall (fun _ v -> v > 0) m)
              }

              test "forall returns false when some entry does not match" {
                  let m = PersistentHashMap.ofSeq [ (1, 10); (2, -1) ]
                  Expect.isFalse "forall false" (PersistentHashMap.forall (fun _ v -> v > 0) m)
              }

              test "choose keeps and transforms matching entries" {
                  let m = PersistentHashMap.ofSeq [ (1, 10); (2, 20); (3, 30) ]

                  let result =
                      PersistentHashMap.choose (fun _ v -> if v > 15 then Some(v * 2) else None) m

                  Expect.equal "choose count" 2 (PersistentHashMap.count result)
                  Expect.equal "choose value" 40 (PersistentHashMap.find 2 result)
              }

              test "toList round-trips via ofList" {
                  let pairs = [ (1, "one"); (2, "two"); (3, "three") ]
                  let m = PersistentHashMap.ofList pairs
                  let result = PersistentHashMap.toList m |> List.sortBy fst
                  Expect.equal "toList/ofList round-trip" (pairs |> List.sortBy fst) result
              }

              test "toArray round-trips via ofArray" {
                  let pairs = [| (1, "one"); (2, "two") |]
                  let m = PersistentHashMap.ofArray pairs
                  let result = PersistentHashMap.toArray m |> Array.sortBy fst
                  Expect.equal "toArray/ofArray round-trip" (pairs |> Array.sortBy fst) result
              }

              test "keys returns all keys" {
                  let m = PersistentHashMap.ofSeq [ (1, "a"); (2, "b"); (3, "c") ]
                  let ks = PersistentHashMap.keys m |> Seq.sort |> Seq.toList
                  Expect.equal "keys" [ 1; 2; 3 ] ks
              }

              test "values returns all values" {
                  let m = PersistentHashMap.ofSeq [ ("a", 1); ("b", 2); ("c", 3) ]
                  let vs = PersistentHashMap.values m |> Seq.sort |> Seq.toList
                  Expect.equal "values" [ 1; 2; 3 ] vs
              } ]

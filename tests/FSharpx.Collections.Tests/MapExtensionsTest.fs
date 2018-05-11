namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module MapExtensionsTests =
    [<Tests>]
    let testMapExtensions =
        testList "MapExtensions" [
            test "map insertWith" {
              let a = Map.ofList [1,"one"; 2,"two"]
              Expect.equal "insertWith" [1,"new one"; 2,"two"] (a |> Map.insertWith (+) 1 "new " |> Map.toList) } 

            test "map updateWith should update value if (f x) is Some" {
              let f x = if x = "one" then Some "new one" else None
              let a = Map.ofList [1,"one"; 2,"two"]
              Expect.equal "updateWith" [1,"new one"; 2,"two"] (a |> Map.updateWith f 1 |> Map.toList) }

            test "map updateWith should delete element if (f x) is None" {
              let f x = if x = "one" then Some "new one" else None
              let a = Map.ofList [1,"one"; 2,"two"]
              Expect.equal "updateWith" [1,"one"] (a |> Map.updateWith f 2 |> Map.toList) } 

            test "test Map_splitWithKey correctly breaks the dictionary on the specified predicate" {
              let a = Map.ofList [0,"zero"; 1,"one"; 2,"two"; 3,"three"; 4,"four"]
              let v1,v2 = a |> Map.splitWithKey ((>=) 2)
              Expect.equal "splitWithKey" [0,"zero"; 1,"one"; 2,"two"] <| Map.toList v1 
              Expect.equal "splitWithKey" [3,"three"; 4,"four"] <| Map.toList v2 }

            test "test Map_spanWithKey correctly breaks the dictionary on the specified predicate" {
              let a = Map.ofList [0,"zero"; 1,"one"; 2,"two"; 3,"three"; 4,"four"]
              let v1,v2 = a |> Map.spanWithKey ((<) 2)
              Expect.equal "spanWithKey" [0,"zero"; 1,"one"; 2,"two"] <|  Map.toList v1
              Expect.equal "spanWithKey" [3,"three"; 4,"four"] <| Map.toList v2 }
        ]

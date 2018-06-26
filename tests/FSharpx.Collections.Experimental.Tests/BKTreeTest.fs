namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto

module BKTreeTest =

    let sem tree = tree |> BKTree.toList |> List.sort

    let trans f xs = xs |> BKTree.Int.ofList |> f |> sem

    let rec inv m = function
        | Empty -> true
        | BKTree.Node(a, _, map) ->
            List.forall (fun (d,b) -> BKTree.Int.distance a b = d) m &&
                List.forall (fun (d,t) -> inv ((d,a)::m) t) (IntMap.toList map)

    let invariant t = inv [] t

    let rec removeFirst n = function
        | [] -> []
        | x::xs when x = n -> xs
        | x::xs -> x :: removeFirst n xs

    [<Tests>]
    let testBKTree =

        testList "Experimental BKTree" [
            testPropertyWithConfig config10k "native empty" <|
                fun (xs :int list) ->
                    BKTree.List.distance [] xs = List.length xs && BKTree.List.distance xs [] = List.length xs

            testPropertyWithConfig config10k "native cons" <|
                fun (x:int) xs ys ->
                    BKTree.List.distance (x::xs) (x::ys) = BKTree.List.distance xs ys

            testPropertyWithConfig config10k "native diff" <|
                fun (x:int) y xs ys ->
                    x <> y ==>
                        (BKTree.List.distance (x::xs) (y::ys) =
                          1 + List.min [BKTree.List.distance (x::xs) ys; BKTree.List.distance (x::xs) (x::ys); BKTree.List.distance xs (y::ys)])

            testPropertyWithConfig config10k "empty" <|
                fun n -> not <| BKTree.Int.exists n BKTree.empty

            testPropertyWithConfig config10k  "isEmpty" <|
                fun xs -> BKTree.isEmpty (BKTree.Int.ofList xs) = List.isEmpty xs

            testPropertyWithConfig config10k  "singleton" <|
                fun n -> BKTree.toList (BKTree.Int.ofList [n]) = [n]

            testPropertyWithConfig config10k  "ofList" <|
                fun xs -> sem (BKTree.Int.ofList xs) = List.sort xs

            testPropertyWithConfig config10k "ofList inv" <|
                fun xs -> invariant (BKTree.Int.ofList xs)

            testPropertyWithConfig config10k  "add" <|
                fun n xs -> trans (BKTree.Int.add n) xs = List.sort (n::xs)

            testPropertyWithConfig config10k "add inv" <|
                fun n xs -> invariant (BKTree.Int.add n (BKTree.Int.ofList xs))

            testPropertyWithConfig config10k  "exists" <|
                fun n xs -> BKTree.Int.exists n (BKTree.Int.ofList xs) = List.exists ((=) n) xs

            testPropertyWithConfig config10k  "existsDistance" <|
                fun dist n xs ->
                    let d = dist % 5
                    let reference = List.exists (fun e -> BKTree.Int.distance n e <= d) xs
                    Prop.collect reference
                        (BKTree.Int.existsDistance d n (BKTree.Int.ofList xs) = List.exists (fun e -> BKTree.Int.distance n e <= d) xs)

            testPropertyWithConfig config10k  "delete" <|
                fun n xs -> trans (BKTree.Int.delete n) xs = List.sort (removeFirst n xs)
     
            testPropertyWithConfig config10k "delete inv" <|
                fun n xs -> invariant (BKTree.Int.delete n (BKTree.Int.ofList xs))

            testPropertyWithConfig config10k  "toList" <|
                fun xs -> List.sort (BKTree.toList (BKTree.Int.ofList xs)) = List.sort xs

            testPropertyWithConfig config10k  "toListDistance" <|
                fun dist n xs ->
                    let d = dist % 5
                    List.sort (BKTree.Int.toListDistance d n (BKTree.Int.ofList xs)) =
                        List.sort (List.filter (fun e -> BKTree.Int.distance n e <= d) xs)

            testPropertyWithConfig config10k  "concat" <|
                fun xss -> sem (BKTree.Int.concat (List.map BKTree.Int.ofList xss)) = List.sort (List.concat xss)

            testPropertyWithConfig config10k "concat inv" <|
                fun xss -> invariant (BKTree.Int.concat (List.map BKTree.Int.ofList xss))

            testPropertyWithConfig config10k  "append" <|
                fun xs ys ->
                    sem (BKTree.Int.append (BKTree.Int.ofList xs) (BKTree.Int.ofList ys)) = List.sort (List.append xs ys)

            testPropertyWithConfig config10k "append inv" <|
                fun xs ys -> invariant (BKTree.Int.append (BKTree.Int.ofList xs) (BKTree.Int.ofList ys))

            testPropertyWithConfig config10k "delete . add = id" <|
                fun n xs -> trans ((BKTree.Int.delete n) << (BKTree.Int.add n)) xs = List.sort xs

            testPropertyWithConfig config10k "The size of an empty BKTree is 0" <|
                fun _ -> BKTree.size BKTree.empty = 0

            testPropertyWithConfig config10k "ofList and size" <|
                fun xs -> BKTree.size (BKTree.Int.ofList xs) = List.length xs

            testPropertyWithConfig config10k "add . size = size + 1" <|
                fun n xs ->
                    let tree = BKTree.Int.ofList xs
                    BKTree.size (BKTree.Int.add n tree) = BKTree.size tree + 1

            testPropertyWithConfig config10k "delete . size = size - 1" <|
                fun n xs ->
                    let tree = BKTree.Int.ofList xs
                    BKTree.size (BKTree.Int.delete n tree) = BKTree.size tree - if BKTree.Int.exists n tree then 1 else 0

            testPropertyWithConfig config10k "append and size" <|
                fun xs ys ->
                    let treeXs = BKTree.Int.ofList xs
                    let treeYs = BKTree.Int.ofList ys
                    BKTree.size (BKTree.Int.append treeXs treeYs) = BKTree.size treeXs + BKTree.size treeYs

            testPropertyWithConfig config10k "concat and size" <|
                fun xss ->
                    let trees = List.map BKTree.Int.ofList xss
                    BKTree.size (BKTree.Int.concat trees) = List.sum (List.map BKTree.size trees)

            testPropertyWithConfig config10k "concat and exists" <|
                fun xss ->
                    let tree = BKTree.Int.concat (List.map BKTree.Int.ofList xss)
                    List.forall (fun x -> BKTree.Int.exists x tree) (List.concat xss)

            testPropertyWithConfig config10k "ofList and exists" <|
                fun xs ->
                    let tree = BKTree.Int.ofList xs
                    List.forall (fun x -> BKTree.Int.exists x tree) xs

            testPropertyWithConfig config10k "implements IEnumerable" <|
                fun values ->
                    let tree = BKTree.Int.ofList values
                    let a = tree :> _ seq |> Seq.toList
                    set values = set a && a.Length = values.Length
        ]

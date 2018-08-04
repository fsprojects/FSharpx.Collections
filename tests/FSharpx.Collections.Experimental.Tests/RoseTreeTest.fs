namespace FSharpx.Collections.Experimental.Tests

open FsCheck
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip
open Properties

module RoseTreeTest =

    let tree a b = RoseTree.create a (LazyList.ofList b)

    let atree = tree 1 [tree 2 [tree 3 []]; tree 4 [tree 5 [tree 6 []]]]
    let ctree = tree "f" [tree "b" [tree "a" []; tree "d" [tree "c" []; tree "e" []]]; tree "g" [tree "i" [tree "h" []]]]

    type HtmlElement = { TagName: string; Attributes: (string * string) list }

    type HtmlNode =
    | Element of HtmlElement
    | Text of string

    let elemA tag attr = HtmlNode.Element { TagName = tag; Attributes = attr }
    let elem tag = elemA tag []
    let text t = tree (HtmlNode.Text t) []
    type Html = HtmlNode RoseTree

    let htmldoc = 
        tree (elem "body") [tree (elem "div") [text "hello world"]]

    // dfs examples borrowed from http://en.wikipedia.org/wiki/Tree_traversal#Exampl
    [<Tests>]
    let testRoseTree =

        testList "Experimental RoseTree" [
            test "dfs pre" {
                let actual = RoseTree.dfsPre ctree |> Seq.toList
                Expect.equal "" ["f";"b";"a";"d";"c";"e";"g";"i";"h"] actual }

            test "dfs post" {
                let actual = RoseTree.dfsPost ctree |> Seq.toList
                Expect.equal "" ["a";"c";"e";"d";"b";"h";"i";"g";"f"] actual }
    
            test "map" {
                let actual = RoseTree.map ((+) 1) atree
                let expected = tree 2 [tree 3 [tree 4 []]; tree 5 [tree 6 [tree 7 []]]]
                Expect.equal "" expected actual }

            test "fold via dfs" {
                let actual = RoseTree.dfsPre atree |> Seq.fold (*) 1
                Expect.equal "" 720 actual }

            test "unfold" {
                let a = RoseTree.unfold (fun i -> i, LazyList.ofSeq {i+1..3}) 0
                let expected = tree 0 [tree 1 [tree 2 [tree 3 []]; tree 3 []]; tree 2 [tree 3 []]; tree 3 []]
                Expect.equal "" expected a }  

            test "mapAccum" {
                let e, taggedHtmlDoc = 
                    RoseTree.mapAccum 
                        (fun i -> function
                                  | Element x -> i+1, Element { x with Attributes = ("data-i",i.ToString())::x.Attributes }
                                  | x -> i,x) 0 htmldoc
                let expected = 
                    tree (elemA "body" ["data-i","0"]) 
                        [tree (elemA "div" ["data-i","1"]) [text "hello world"]]
                Expect.equal "" expected taggedHtmlDoc
                Expect.equal "" 2 e }

            test "bind" {
                let wrapText =
                    function
                    | Text t -> tree (elem "span") [text t]
                    | x -> RoseTree.singleton x
                let newDoc = htmldoc |> RoseTree.bind wrapText
                let expected = tree (elem "body") [tree (elem "div") [tree (elem "span") [text "hello world"]]]
                Expect.equal "" expected newDoc }
        ]

    [<Tests>]
    let testRoseTreeProperties =

        let roseTree() = 
            let rec impl s =
                gen {
                        let! root = Arb.generate
                    // need to set these frequencies to avoid blowing the stack
                        let! children =
                            match s with
                            | s when s > 0 -> Gen.frequency [70, Gen.constant LazyList.empty; 1, impl (s/2) |> Gen.listOf |> Gen.map LazyList.ofList]
                            | _ -> Gen.constant LazyList.empty
                    return RoseTree.create root children 
                }
            impl |> Gen.sized |> Arb.fromGen

        let roseTreeOfObj n (o : obj) =
            let tail =
                let rec loop i l =
                    match i with
                    | n when n > 0 ->
                        loop (n - 1) ((RoseTree.singleton o)::l)
                    | _ ->
                        l
                loop (n - 1) List.empty
            RoseTree.create o (LazyList.ofList tail)

        let associativity() =
            gen {
                let! n1 = Arb.generate<int> |> Gen.filter (fun n -> n > 0 && n < 10)
                let! n2 = Arb.generate<int> |> Gen.filter (fun n -> n > 0 && n < 10)
                

                let! xs = Gen.listObj n2 |> Gen.filter (fun x -> x.Length > 0 && x.Length < 100)
                return (roseTreeOfObj n1), (roseTreeOfObj n2), (RoseTree.create xs.Head (LazyList.ofList (xs.Tail |> List.map RoseTree.singleton)))
            } |> Arb.fromGen

        let composition() =
            gen {
                let f = id
                let g = id
                let! n = Arb.generate<int> |> Gen.filter (fun n -> n > 0 && n < 10)

                let! xs = Gen.listObj n |> Gen.filter (fun x -> x.Length > 0 && x.Length < 100)
                return f, g, (RoseTree.create xs.Head (LazyList.ofList (xs.Tail |> List.map RoseTree.singleton)))
            } |> Arb.fromGen

        let roseTreeOfLength stdGen1 stdGen2 n =
            match n with
            | n when n > 1 ->
                let tail =      
                    Gen.eval (n - 1) (Random.StdGen(stdGen1, stdGen2)) <| (Gen.ArbitrarySeqGen() |> Gen.filter (fun xs -> Seq.length xs > 0))
                    |> Seq.toList
                RoseTree.create tail.Head (tail |> List.map  RoseTree.singleton |> LazyList.ofList)
            | _ -> 
                let head = 
                    Gen.eval (n - 1) (Random.StdGen(stdGen1, stdGen2)) Arb.generate
                RoseTree.create head LazyList.empty

        let leftIdentity() =
            gen {
                let! stdGen1 = Arb.generate<int>
                let! stdGen2 = Arb.generate<int>
                let! n = Arb.generate<int> |> Gen.filter (fun n -> n > 0 && n < 100)
                return  (roseTreeOfLength stdGen1 stdGen2), n
            } |> Arb.fromGen

        let map = RoseTree.map
        let inline (>>=) m f = RoseTree.bind f m
        let ret = RoseTree.singleton

        testList "Experimental RoseTree properties" [

            testPropertyWithConfig config10k "RoseTree functor laws: preserves identity"
                (Prop.forAll (roseTree()) <|
                    fun value -> map id value = value )

            testPropertyWithConfig config10k "RoseTree functor laws: preserves composition" 
                (Prop.forAll (composition()) <|
                    fun (f, g, value) -> map (f << g) value = (map f << map g) value  )

            testPropertyWithConfig config10k "RoseTree monad laws : left identity" 
                (Prop.forAll (leftIdentity()) <|
                    fun (f, a) -> ret a >>= f = f a )

            testPropertyWithConfig config10k "RoseTree monad laws : right identity"
                (Prop.forAll (roseTree()) <|
                    fun x -> x >>= ret = x )

            testPropertyWithConfig config10k "RoseTree monad laws : associativity" 
                (Prop.forAll (associativity()) <|
                    fun (f, g, v) ->
                        let a = (v >>= f) >>= g
                        let b = v >>= (fun x -> f x >>= g)
                        a = b )

        //// TODO port example from http://blog.moertel.com/articles/2007/03/07/directory-tree-printing-in-haskell-part-two-refactoring
        ]

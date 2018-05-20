namespace FSharpx.Collections.Experimental.Tests

open FSharpx
open FSharpx.Collections
open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

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

            //open FsCheck
            //open FSharpx.Collections.Experimental.Tests.Properties

            //type RoseTreeGen =
            //    static member RoseTree() =
            //        let rec roseTreeGen() = 
            //            gen {
            //                let! root = Arb.generate
            //                // need to set these frequencies to avoid blowing the stack
            //                let! children = Gen.frequency [70, gen.Return LazyList.empty; 1, Gen.finiteLazyList()]
            //                return RoseTree.create root children
            //            }
            //        Arb.fromGen (roseTreeGen())

            //let registerGen = lazy (Arb.register<RoseTreeGen>() |> ignore)

            //let equality() =
            //    registerGen.Force()
            //    checkEquality<int RoseTree> "RoseTree"

            //test "functor laws" {
            //    registerGen.Force()
            //    let map = RoseTree.map
            //    let n = sprintf "RoseTree : functor %s"
            //    fsCheck (n "preserves identity") <| 
            //        fun value -> map id value = value
            //    fsCheck (n "preserves composition") <|
            //        fun f g value -> map (f << g) value = (map f << map g) value

            //test "monad laws" {
            //    registerGen.Force()
            //    let n = sprintf "RoseTree : monad %s"
            //    let inline (>>=) m f = RoseTree.bind f m
            //    let ret = RoseTree.singleton
            //    fsCheck "left identity" <| 
            //        fun f a -> ret a >>= f = f a
            //    fsCheck "right identity" <| 
            //        fun x -> x >>= ret = x
            //    fsCheck "associativity" <| 
            //        fun f g v ->
            //            let a = (v >>= f) >>= g
            //            let b = v >>= (fun x -> f x >>= g)
            //            a = b

        //// TODO port example from http://blog.moertel.com/articles/2007/03/07/directory-tree-printing-in-haskell-part-two-refactoring
        ]

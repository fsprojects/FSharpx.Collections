namespace FSharpx.Collections.Experimental.Tests

open FSharpx
open FSharpx.Collections
open FSharpx.Collections.Experimental
open FsCheck
open FSharpx.Collections.Experimental.Tests.Properties
open Expecto
open Expecto.Flip

module EagerRoseTreeTest =

    let tree a b = EagerRoseTree.create a b

    let atree = tree 1 [tree 2 [tree 3 []]; tree 4 [tree 5 [tree 6 []]]]
    let ctree = tree "f" [tree "b" [tree "a" []; tree "d" [tree "c" []; tree "e" []]]; tree "g" [tree "i" [tree "h" []]]]

    // dfs examples borrowed from http://en.wikipedia.org/wiki/Tree_traversal#Example

    [<Tests>]
    let testEagerRoseTree =

        testList "Experimental EagerRoseTree" [
            //[<Test>]
            //test "dfs pre" {
            //    let actual = EagerRoseTree.dfsPre ctree |> Seq.toList
            //    Expect.equal "" ["f";"b";"a";"d";"c";"e";"g";"i";"h"], actual)

            //[<Test>]
            //test "dfs post" {
            //    let actual = EagerRoseTree.dfsPost ctree |> Seq.toList
            //    Expect.equal "" ["a";"c";"e";"d";"b";"h";"i";"g";"f"], actual)
    
            //[<Test>]
            //let map() =
            //    let actual = EagerRoseTree.map ((+) 1) atree
            //    let expected = tree 2 [tree 3 [tree 4 []]; tree 5 [tree 6 [tree 7 []]]]
            //    Expect.equal "" expected, actual)

            //[<Test>]
            //test "fold via dfs" {
            //    let actual = EagerRoseTree.dfsPre atree |> Seq.fold (*) 1
            //    Expect.equal "" 720, actual)


            //[<Test>]
            //let unfold() =
            //    let a = EagerRoseTree.unfold (fun i -> i, List.ofSeq {i+1..3}) 0
            //    let expected = tree 0 [tree 1 [tree 2 [tree 3 []]; tree 3 []]; tree 2 [tree 3 []]; tree 3 []]
            //    Expect.equal "" expected, a)

            //// not the best example, as text nodes cannot have children

            //type HtmlElement = { TagName: string; Attributes: (string * string) list }

            //type HtmlNode =
            //| Element of HtmlElement
            //| Text of string

            //let elemA tag attr = HtmlNode.Element { TagName = tag; Attributes = attr }
            //let elem tag = elemA tag []
            //let text t = tree (HtmlNode.Text t) []
            //type Html = HtmlNode EagerRoseTree

            //let htmldoc = 
            //    tree (elem "body") [tree (elem "div") [text "hello world"]]

            //[<Test>]
            //let mapAccum() =
            //    let e, taggedHtmlDoc = 
            //        EagerRoseTree.mapAccum 
            //            (fun i -> function
            //                      | Element x -> i+1, Element { x with Attributes = ("data-i",i.ToString())::x.Attributes }
            //                      | x -> i,x) 0 htmldoc
            //    let expected = 
            //        tree (elemA "body" ["data-i","0"]) 
            //            [tree (elemA "div" ["data-i","1"]) [text "hello world"]]
            //    Expect.equal "" expected, taggedHtmlDoc)
            //    Expect.equal "" 2, e)

            //[<Test>]
            //let bind() =
            //    let wrapText =
            //        function
            //        | Text t -> tree (elem "span") [text t]
            //        | x -> EagerRoseTree.singleton x
            //    let newDoc = htmldoc |> EagerRoseTree.bind wrapText
            //    let expected = tree (elem "body") [tree (elem "div") [tree (elem "span") [text "hello world"]]]
            //    Expect.equal "" expected, newDoc)

            //let finiteEagerRoseTreeForest() =
            //    gen {
            //        let! n = Gen.length1thru 5
            //        let! l = Gen.listOfLength n Arb.generate<int>
            //        return List.fold (fun (s : list<EagerRoseTree<int>>) (t : int) -> (EagerRoseTree.singleton t)::s) [] l
            //    }

            //type EagerRoseTreeGen =
            //    static member EagerRoseTree() =
            //        let rec EagerRoseTreeGen() = 
            //            gen {
            //                let! root = Arb.generate<int>
            //                // need to set these frequencies to avoid blowing the stack
            //                let! children = Gen.frequency [70, gen.Return List.empty; 1, finiteEagerRoseTreeForest()]
            //                return EagerRoseTree.create root children
            //            }
            //        Arb.fromGen (EagerRoseTreeGen())

            //let registerGen = lazy (Arb.register<EagerRoseTreeGen>() |> ignore)

            //[<Test>]
            //let equality() =
            //    registerGen.Force()
            //    checkEquality<int EagerRoseTree> "EagerRoseTree"

            //let eRTF l = List.fold (fun (s : list<EagerRoseTree<int>>) (t : int) -> (EagerRoseTree.singleton t)::s) [] l
            //let eRTF2 = [ for i = 1 to 5 do
            //                yield (EagerRoseTree.create 1 (eRTF [1..5]))
            //            ]
            //let eRT = EagerRoseTree.create 1 eRTF2
            //let singleRT = EagerRoseTree.singleton 1

            ////[<Test>]
            ////test "functor laws" {
            ////    registerGen.Force()
            ////    let map = EagerRoseTree.map
            ////    let n = sprintf "EagerRoseTree : functor %s"
            ////    fsCheck (n "preserves identity") <| 
            ////        fun value -> map id value = value
            ////    fsCheck (n "preserves composition") <|
            ////        fun f g value -> map (f << g) value = (map f << map g) value
            ////
            //[<Test>]
            //test "functor laws" {
            //    //fsCheck version of functor and monad laws stackoverflows 
            //    let map = EagerRoseTree.map
    
            //    //preserves identity
            //    ((map id eRT) = eRT) |> Expect.isTrue "" }
            //    ((map id singleRT) = singleRT) |> Expect.isTrue "" }
    
            //    let f = (fun x -> x + 5)
            //    let g = (fun x -> x - 2)

            //    //preserves composition
            //    map (f << g) eRT = (map f << map g) eRT |> Expect.isTrue "" }
            //    map (f << g) singleRT = (map f << map g) singleRT |> Expect.isTrue "" }

            ////[<Test>]
            ////test "monad laws" {
            ////    registerGen.Force()
            ////    let n = sprintf "EagerRoseTree : monad %s"
            ////    let inline (>>=) m f = EagerRoseTree.bind f m
            ////    let ret = EagerRoseTree.singleton
            ////    fsCheck "left identity" <| 
            ////        fun f a -> ret a >>= f = f a
            ////    fsCheck "right identity" <| 
            ////        fun x -> x >>= ret = x
            ////    fsCheck "associativity" <| 
            ////        fun f g v ->
            ////            let a = (v >>= f) >>= g
            ////            let b = v >>= (fun x -> f x >>= g)
            ////            a = b
            //[<Test>]
            //test "monad laws" {
            //    //fsCheck version of functor and monad laws stackoverflows
            //    let inline (>>=) m f = EagerRoseTree.bind f m
            //    let ret = EagerRoseTree.singleton

            //    let myF x = EagerRoseTree.create x [(EagerRoseTree.singleton x); (EagerRoseTree.singleton x)]
            //    let a = 1

            //    //left identity 
            //    ret a >>= myF = myF a |> Expect.isTrue "" }

            //    //right identity 
            //    eRT >>= ret = eRT |> Expect.isTrue "" }
            //    singleRT >>= ret = singleRT |> Expect.isTrue "" }

            //    //associativity 
            //    let myG x = EagerRoseTree.create (x=x) [(EagerRoseTree.singleton (x=x)); (EagerRoseTree.singleton (x=x))]

            //    let a' = (eRT >>= myF) >>= myG
            //    let b' = eRT >>= (fun x -> myF x >>= myG)
            //    a' = b' |> Expect.isTrue "" }

            //    let a'' = (singleRT >>= myF) >>= myG
            //    let b'' = singleRT >>= (fun x -> myF x >>= myG)
            //    a'' = b'' |> Expect.isTrue "" }

        ]
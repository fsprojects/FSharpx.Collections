namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open FSharpx.Collections
open Expecto
open Expecto.Flip

module IndexedRoseTreeTest =

    let tree a b =
        IndexedRoseTree.create a (PersistentVector.ofSeq b)

    let atree = tree 1 [ tree 2 [ tree 3 [] ]; tree 4 [ tree 5 [ tree 6 [] ] ] ]

    let ctree =
        tree "f" [
            tree "b" [ tree "a" []; tree "d" [ tree "c" []; tree "e" [] ] ]
            tree "g" [ tree "i" [ tree "h" [] ] ]
        ]

    type HtmlElement = {
        TagName: string
        Attributes: (string * string) list
    }

    type HtmlNode =
        | Element of HtmlElement
        | Text of string

    type Html = HtmlNode IndexedRoseTree

    let iRTF l =
        List.fold (fun (s: list<IndexedRoseTree<int>>) (t: int) -> (IndexedRoseTree.singleton t) :: s) [] l
        |> PersistentVector.ofSeq

    let iRTF2 =
        let rec loop (v: PersistentVector<IndexedRoseTree<int>>) dec =
            match dec with
            | 0 -> v
            | _ -> loop (v.Conj(IndexedRoseTree.create 1 (iRTF [ 1..5 ]))) (dec - 1)

        loop PersistentVector.empty 5

    let iRT = IndexedRoseTree.create 1 iRTF2
    let singleRT = IndexedRoseTree.singleton 1

    [<Tests>]
    let testIndexedRoseTree =
        // not the best example, as text nodes cannot have children

        let elemA tag attr =
            HtmlNode.Element { TagName = tag; Attributes = attr }

        let elem tag =
            elemA tag []

        let text t =
            tree (HtmlNode.Text t) []


        let htmldoc = tree (elem "body") [ tree (elem "div") [ text "hello world" ] ]

        testList "Experimental IndexedRoseTree" [
            test "preOrder works" {
                let actual = IndexedRoseTree.preOrder ctree |> Seq.toList
                Expect.equal "" [ "f"; "b"; "a"; "d"; "c"; "e"; "g"; "i"; "h" ] actual
            }

            test "postOrder works" {
                let actual = IndexedRoseTree.postOrder ctree |> Seq.toList
                Expect.equal "" [ "a"; "c"; "e"; "d"; "b"; "h"; "i"; "g"; "f" ] actual
            }

            test "map" {
                let actual = IndexedRoseTree.map ((+) 1) atree
                let expected = tree 2 [ tree 3 [ tree 4 [] ]; tree 5 [ tree 6 [ tree 7 [] ] ] ]
                Expect.equal "" expected actual
            }

            test "fold via preOrder" {
                let actual = IndexedRoseTree.preOrder atree |> Seq.fold (*) 1
                Expect.equal "" 720 actual
            }

            test "bind" {
                let wrapText =
                    function
                    | Text t -> tree (elem "span") [ text t ]
                    | x -> IndexedRoseTree.singleton x

                let newDoc = htmldoc |> IndexedRoseTree.bind wrapText

                let expected =
                    tree (elem "body") [ tree (elem "div") [ tree (elem "span") [ text "hello world" ] ] ]

                Expect.equal "" expected newDoc
            }

            test "unfold" {
                let a = IndexedRoseTree.unfold (fun i -> i, PersistentVector.ofSeq { i + 1 .. 3 }) 0

                let expected =
                    tree 0 [ tree 1 [ tree 2 [ tree 3 [] ]; tree 3 [] ]; tree 2 [ tree 3 [] ]; tree 3 [] ]

                Expect.equal "" expected a
            }

            test "functor laws" {
                //fsCheck version of functor and monad laws stackoverflows
                let map = IndexedRoseTree.map

                //preserves identity
                ((map id iRT) = iRT) |> Expect.isTrue ""
                ((map id singleRT) = singleRT) |> Expect.isTrue ""

                let f = (fun x -> x + 5)
                let g = (fun x -> x - 2)

                //preserves composition
                map (f << g) iRT = (map f << map g) iRT |> Expect.isTrue ""

                map (f << g) singleRT = (map f << map g) singleRT |> Expect.isTrue ""
            }

            test "monad laws" {
                //fsCheck version of functor and monad laws stackoverflows
                let inline (>>=) m f =
                    IndexedRoseTree.bind f m

                let ret = IndexedRoseTree.singleton

                let myF x =
                    IndexedRoseTree.create
                        x
                        (PersistentVector.empty
                         |> PersistentVector.conj(IndexedRoseTree.singleton x)
                         |> PersistentVector.conj(IndexedRoseTree.singleton x))

                let a = 1

                //left identity
                ret a >>= myF = myF a |> Expect.isTrue ""

                //right identity
                iRT >>= ret = iRT |> Expect.isTrue ""
                singleRT >>= ret = singleRT |> Expect.isTrue ""

                //associativity
                let myG x =
                    IndexedRoseTree.create
                        (x = x)
                        (PersistentVector.empty
                         |> PersistentVector.conj(IndexedRoseTree.singleton(x = x))
                         |> PersistentVector.conj(IndexedRoseTree.singleton(x = x)))

                let a' = (iRT >>= myF) >>= myG
                let b' = iRT >>= (fun x -> myF x >>= myG)
                a' = b' |> Expect.isTrue ""

                let a'' = (singleRT >>= myF) >>= myG
                let b'' = singleRT >>= (fun x -> myF x >>= myG)
                a'' = b'' |> Expect.isTrue ""
            }
        ]

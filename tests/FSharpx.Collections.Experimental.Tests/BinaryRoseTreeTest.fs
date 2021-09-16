namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open Expecto.Flip

module BinaryRoseTreeTest =

    let atree = 
        BinaryRoseTree.createTree 1
            (BinaryRoseTree.createTree 2
                (BinaryRoseTree.createForest 3 
                    BinaryRoseTree.empty
                    (BinaryRoseTree.createTree 4
                        (BinaryRoseTree.createTree 5
                            (BinaryRoseTree.singleton 6)
                        )
                    )
                )
            )

    let expected = 
        BinaryRoseTree.createTree 2
            (BinaryRoseTree.createTree 3
                (BinaryRoseTree.createForest 4 
                    BinaryRoseTree.empty
                    (BinaryRoseTree.createTree 5
                        (BinaryRoseTree.createTree 6
                            (BinaryRoseTree.singleton 7)
                        )
                    )
                )
            )

    let ctree = 
        BinaryRoseTree.createTree "f"
            (
                BinaryRoseTree.createForest "b" 
                    (BinaryRoseTree.createForest "a" 
                        BinaryRoseTree.empty
                        (BinaryRoseTree.createTree "d"
                            (BinaryRoseTree.createForest "c" 
                                BinaryRoseTree.empty
                                (BinaryRoseTree.singleton "e")
                            )
                        )
                    )

                    (BinaryRoseTree.createTree "g"
                        (BinaryRoseTree.createTree "i"
                            (BinaryRoseTree.singleton "h")
                        )
                    )
            )

    [<Tests>]
    let testBinaryRoseTree=

        testList "Experimental BinaryRoseTree" [
            test "preOrder works" {
                let actual = BinaryRoseTree.preOrder ctree |> Seq.toList
                Expect.equal "" ["f";"b";"a";"d";"c";"e";"g";"i";"h"] actual }

            test "postOrder works" {
                let actual = BinaryRoseTree.postOrder ctree |> Seq.toList
                Expect.equal "" ["a";"c";"e";"d";"b";"h";"i";"g";"f"] actual }
    
            test "map" {
                let actual = BinaryRoseTree.map ((+) 1) atree 
                Expect.equal "" expected actual }

            test "fold via preOrder" {
                let actual = BinaryRoseTree.preOrder atree |> Seq.fold (*) 1
                Expect.equal "" 720 actual }

            test "functor laws" {
                let iRT = BinaryRoseTree.createTree 1 (BinaryRoseTree.createForest 2 atree expected)
                let singleRT = BinaryRoseTree.singleton 1

                //fsCheck version of functor and monad laws stackoverflows 
                let map = BinaryRoseTree.map
    
                //preserves identity
                ((map id iRT) = iRT) |> Expect.isTrue "" 
                ((map id singleRT) = singleRT) |> Expect.isTrue "" 
    
                let f = (fun x -> x + 5)
                let g = (fun x -> x - 2)

                //preserves composition
                map (f << g) iRT = (map f << map g) iRT |> Expect.isTrue "" 
                map (f << g) singleRT = (map f << map g) singleRT |> Expect.isTrue "" }
        ]
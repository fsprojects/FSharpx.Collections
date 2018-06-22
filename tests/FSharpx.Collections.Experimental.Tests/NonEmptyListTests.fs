namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FsCheck
open Expecto
open Expecto.Flip
open Properties

module NonEmptyListTests =

    type NonEmptyListGen =
        static member NonEmptyList() =
            (Gen.apply (Gen.apply (gen.Return NonEmptyList.create) Arb.generate) (Arb.generate |> Gen.filter (fun l -> l.Length < 10)))
            |> Arb.fromGen

    let registerGen = lazy (Arb.register<NonEmptyListGen>() |> ignore)

    registerGen.Force()

    let EqualLengthNELGen() =
        gen {
            let! a = NonEmptyListGen.NonEmptyList().Generator
            let! b = Gen.listOfLength a.Length Arb.generate
                        |> Gen.map (fun l -> NonEmptyList.create (List.head l) (List.tail l))
            return a, b
        } |> Arb.fromGen

    let ArbitrarySeq() = 
        gen {
            let! len = Gen.choose (0, 10)
            let! l = Gen.listOfLength len Arb.generate
            return seq { for i = 0 to len - 1 do yield l.[i] }
        } |> Arb.fromGen

    let map = NonEmptyList.map
    let ret (x: int) = NonEmptyList.singleton x
    let inline (>>=) m f = NonEmptyList.collect f m

    [<Tests>]
    let testNonEmptyList =

        testList "Experimental NonEmptyList" [
            testPropertyWithConfig config10k "NonEmptyList functor laws: preserves identity" <|
                fun value -> map id value = value

            testPropertyWithConfig config10k "NonEmptyList functor laws: preserves composition" <|
                fun f g value -> map (f << g) value = (map f << map g) value

            testPropertyWithConfig config10k "NonEmptyList monad laws: left identity" <|
                fun f a -> ret a >>= f = f a

            testPropertyWithConfig config10k "NonEmptyList monad laws: right identity" <|
                fun x -> x >>= ret = x

            testPropertyWithConfig config10k "NonEmptyList monad laws: associativity" <|
                fun f g v ->
                    let a = (v >>= f) >>= g
                    let b = v >>= (fun x -> f x >>= g)
                    a = b

            testPropertyWithConfig config10k "toList gives non-empty list" <|
                fun nel -> NonEmptyList.toList nel |> List.length > 0

            testPropertyWithConfig config10k "toArray gives non-empty array" <|
                fun nel -> NonEmptyList.toArray nel |> Array.length > 0

            testPropertyWithConfig config10k "toList is same length as non-empty list" <|
                fun nel -> NonEmptyList.toList nel |> List.length = nel.Length

            testPropertyWithConfig config10k "toArray is same length as non-empty list" <|
                fun nel -> NonEmptyList.toArray nel |> Array.length = nel.Length

            testPropertyWithConfig config10k "ofArray" <|
                fun arr ->
                    try Seq.forall2 (=) (NonEmptyList.ofArray arr) arr
                    with :? System.ArgumentException -> arr.Length = 0

            testPropertyWithConfig config10k "ofList" <|
                fun l ->
                    try Seq.forall2 (=) (NonEmptyList.ofList l) l
                    with :? System.ArgumentException -> l = []

            testPropertyWithConfig config10k "ofSeq" (Prop.forAll (ArbitrarySeq()) <|
                fun s -> 
                    try Seq.forall2 (=) (NonEmptyList.ofSeq s) s
                    with :? System.ArgumentException -> Seq.isEmpty s)

            testPropertyWithConfig config10k "reverse . reverse = id" <|
                fun nel -> (NonEmptyList.rev << NonEmptyList.rev) nel = nel

            testPropertyWithConfig config10k "last . reverse = head" <|
                fun nel -> (NonEmptyList.last << NonEmptyList.rev) nel = NonEmptyList.head nel

            testPropertyWithConfig config10k "head . reverse = last" <|
                fun nel -> (NonEmptyList.head << NonEmptyList.rev) nel = NonEmptyList.last nel

            testPropertyWithConfig config10k "last is last and never fails" <|
                fun nel ->
                    let actualLast = NonEmptyList.last nel
                    let expectedLast = 
                        let l = NonEmptyList.toList nel
                        l.[l.Length-1]
                    expectedLast = actualLast

            testPropertyWithConfig config10k "append has combined length" <|
                fun (a: _ list) (b: _ list) ->
                    if a.IsEmpty || b.IsEmpty then
                        // we don't test non-empty lists here.
                        true
                    else
                        let neA = NonEmptyList.create a.Head a.Tail
                        let neB = NonEmptyList.create b.Head b.Tail
                        (NonEmptyList.append neA neB).Length = neA.Length + neB.Length

            testPropertyWithConfig config10k "reduce" <|
                fun nel ->
                    let actual = NonEmptyList.reduce (+) nel
                    let expected = nel |> NonEmptyList.toList |> List.sum
                    expected = actual

            testPropertyWithConfig config10k "zip" (Prop.forAll (EqualLengthNELGen()) <|
                fun (nel1, nel2) ->
                    let actual = NonEmptyList.zip nel1 nel2 |> NonEmptyList.toList
                    let expected = List.zip <| NonEmptyList.toList nel1 
                                            <| NonEmptyList.toList nel2
                    expected = actual )

            testPropertyWithConfig config10k "zip on lists with different lengths raises an exception" <|
                fun nel1 nel2 ->
                    try
                        NonEmptyList.zip nel1 nel2 |> ignore
                        nel1.Length = nel2.Length
                    with :? System.ArgumentException -> 
                        nel1.Length <> nel2.Length
        ]

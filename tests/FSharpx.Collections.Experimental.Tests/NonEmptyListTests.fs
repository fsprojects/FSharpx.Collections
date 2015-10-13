module FSharpx.NonEmptyListTests

open FSharpx.Collections
open NUnit.Framework
open FsCheck
open FsCheck.NUnit

type NonEmptyListGen =
    static member NonEmptyList() =
        gen.Return NonEmptyList.create 
        |> Gen.ap Arb.generate
        |> Gen.ap (Arb.generate |> Gen.suchThat (fun l -> l.Length < 10))
        |> Arb.fromGen

let registerGen = lazy (Arb.register<NonEmptyListGen>() |> ignore)

[<Test>]
let ``functor laws``() =
    registerGen.Force()
    let n = sprintf "NonEmptyList : functor %s"
    let map = NonEmptyList.map
    fsCheck (n "preserves identity") <| 
        fun value -> map id value = value
    fsCheck (n "preserves composition") <|
        fun f g value -> map (f << g) value = (map f << map g) value

[<Test>]
let ``monad laws``() =
    registerGen.Force()
    let ret (x: int) = NonEmptyList.singleton x
    let n = sprintf "NonEmptyList : monad %s"
    let inline (>>=) m f = NonEmptyList.collect f m
    fsCheck "left identity" <| 
        fun f a -> ret a >>= f = f a
    fsCheck "right identity" <| 
        fun x -> x >>= ret = x
    fsCheck "associativity" <| 
        fun f g v ->
            let a = (v >>= f) >>= g
            let b = v >>= (fun x -> f x >>= g)
            a = b

let fsCheck t = fsCheck "" t

[<Test>]
let ``toList gives non-empty list``() =
    fsCheck (fun nel -> NonEmptyList.toList nel |> List.length > 0)

[<Test>]
let ``toArray gives non-empty array``() =
    fsCheck (fun nel -> NonEmptyList.toArray nel |> Array.length > 0)

[<Test>]
let ``toList is same length as non-empty list`` () =
    fsCheck (fun nel -> NonEmptyList.toList nel |> List.length = nel.Length)

[<Test>]
let ``toArray is same length as non-empty list`` () =
    fsCheck (fun nel -> NonEmptyList.toArray nel |> Array.length = nel.Length)

[<Test>]
let ofArray () =
    fsCheck <| fun arr ->
        try Seq.forall2 (=) (NonEmptyList.ofArray arr) arr
        with :? System.ArgumentException -> arr.Length = 0

[<Test>]
let ofList () =
    fsCheck <| fun l ->
        try Seq.forall2 (=) (NonEmptyList.ofList l) l
        with :? System.ArgumentException -> l = []

[<Test>]
let ofSeq () =
    let ArbitrarySeq = 
        gen {
            let! len = Gen.choose (0, 10)
            let! l = Gen.listOfLength len Arb.generate
            return seq { for i = 0 to len - 1 do yield l.[i] }
        } |> Arb.fromGen
    fsCheck <| Prop.forAll ArbitrarySeq (fun s -> 
        try Seq.forall2 (=) (NonEmptyList.ofSeq s) s
        with :? System.ArgumentException -> Seq.isEmpty s)

[<Test>]
let ``reverse . reverse = id`` () =
    fsCheck (fun nel -> (NonEmptyList.rev << NonEmptyList.rev) nel = nel)

[<Test>]
let ``last . reverse = head``() =
    fsCheck (fun nel -> (NonEmptyList.last << NonEmptyList.rev) nel = NonEmptyList.head nel)

[<Test>]
let ``head . reverse = last``() =
    fsCheck (fun nel -> (NonEmptyList.head << NonEmptyList.rev) nel = NonEmptyList.last nel)

[<Test>]
let ``last is last and never fails``() =
    fsCheck <| fun nel ->
        let actualLast = NonEmptyList.last nel
        let expectedLast = 
            let l = NonEmptyList.toList nel
            l.[l.Length-1]
        expectedLast = actualLast

[<Test>]
let ``append has combined length``() =
    fsCheck <| fun a b ->
        let c = NonEmptyList.append a b
        c.Length = a.Length + b.Length

[<Test>]
let reduce() =
    fsCheck <| fun nel ->
        let actual = NonEmptyList.reduce (+) nel
        let expected = nel |> NonEmptyList.toList |> List.sum
        expected = actual

[<Test>]
let zip() =
    let EqualLengthNELGen =
        gen {
            let! a = NonEmptyListGen.NonEmptyList().Generator
            let! b = Gen.listOfLength a.Length Arb.generate
                     |> Gen.map (fun l -> NonEmptyList.create (List.head l) (List.tail l))
            return a, b
        } |> Arb.fromGen

    fsCheck <| Prop.forAll EqualLengthNELGen (fun (nel1, nel2) ->
        let actual = NonEmptyList.zip nel1 nel2 |> NonEmptyList.toList
        let expected = List.zip <| NonEmptyList.toList nel1 
                                <| NonEmptyList.toList nel2
        expected = actual)

[<Test>]
let ``zip on lists with different lengths raises an exception``()=
    fsCheck <| fun nel1 nel2 ->
        try
            NonEmptyList.zip nel1 nel2 |> ignore
            nel1.Length = nel2.Length
        with :? System.ArgumentException -> 
            nel1.Length <> nel2.Length


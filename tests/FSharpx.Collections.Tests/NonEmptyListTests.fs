namespace FSharpx.Collections.Tests

open FsCheck
open Expecto
open Expecto.Flip

open FSharpx.Collections
open Properties

module NonEmptyListTests =

    type NonEmptyListGen =
        static member NonEmptyList() =
            (Gen.apply (Gen.apply (gen.Return NonEmptyList.create) Arb.generate) (Arb.generate |> Gen.filter(fun l -> l.Length < 10)))
            |> Arb.fromGen

    let registerGen = lazy (Arb.register<NonEmptyListGen>() |> ignore)

    let EqualLengthNELGen() =
        gen {
            let! a = NonEmptyListGen.NonEmptyList().Generator

            let! b =
                Gen.listOfLength a.Length Arb.generate
                |> Gen.map(fun l -> NonEmptyList.create (List.head l) (List.tail l))

            return a, b
        }
        |> Arb.fromGen

    let arbitraryMapMapNE() =
        gen {
            let f = fun x -> x
            let g = fun x -> x
            let! ne = (Gen.apply (Gen.apply (gen.Return NonEmptyList.create) Arb.generate) (Arb.generate |> Gen.filter(fun l -> l.Length < 10)))
            return (f, g, ne)
        }
        |> Arb.fromGen

    let nonEmptyListGen() = gen {
        let! ne = (Gen.apply (Gen.apply (gen.Return NonEmptyList.create) Arb.generate) (Arb.generate |> Gen.filter(fun l -> l.Length < 10)))
        return ne
    }

    let twoDifferentLengths() =
        gen {
            let! ne1 = nonEmptyListGen()
            let! ne2 = nonEmptyListGen() |> Gen.filter(fun xs -> xs.Length <> ne1.Length)
            return ne1, ne2
        }
        |> Arb.fromGen

    let neOfLength stdGen1 stdGen2 n =
        match n with
        | n when n > 1 ->
            let tail =
                Gen.eval (n - 1) (Random.StdGen(stdGen1, stdGen2))
                <| (Gen.ArbitrarySeqGen() |> Gen.filter(fun xs -> Seq.length xs > 0))
                |> Seq.toList

            NonEmptyList.create tail.Head tail
        | _ ->
            let head = Gen.eval (n - 1) (Random.StdGen(stdGen1, stdGen2)) Arb.generate
            NonEmptyList.create head List.empty

    let neListOfInt() =
        gen {
            let! n = Arb.generate<int> |> Gen.filter(fun n -> n > 0 && n < 1000)
            let! l = Gen.listInt n
            return NonEmptyList.create l.Head l.Tail
        }
        |> Arb.fromGen

    let leftIdentity() =
        gen {
            let! stdGen1 = Arb.generate<int>
            let! stdGen2 = Arb.generate<int>
            let! n = Arb.generate<int> |> Gen.filter(fun n -> n > 0 && n < 100)
            return (neOfLength stdGen1 stdGen2), n
        }
        |> Arb.fromGen

    let neListOfObj n (o: obj) =
        let tail =
            let rec loop i l =
                match i with
                | n when n > 0 -> loop (n - 1) (o :: l)
                | _ -> l

            loop (n - 1) List.empty

        NonEmptyList.create o tail

    let associativity() =
        gen {
            let! n1 = Arb.generate<int> |> Gen.filter(fun n -> n > 0 && n < 10)
            let! n2 = Arb.generate<int> |> Gen.filter(fun n -> n > 0 && n < 10)
            let! o = Arb.generate<obj>

            let! xs = Gen.listObj n2 |> Gen.filter(fun x -> x.Length > 0 && x.Length < 100)
            return (neListOfObj n1), (neListOfObj n2), (NonEmptyList.create xs.Head xs.Tail)
        }
        |> Arb.fromGen

    [<Tests>]
    let testNonEmptyList =

        let map = NonEmptyList.map

        let ret(x: int) =
            NonEmptyList.singleton x

        let inline (>>=) m f =
            NonEmptyList.collect f m

        testList "NonEmptyList" [ testPropertyWithConfig
                                      config10k
                                      "NonEmptyList functor laws: preserves identity"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun value -> map id value = value)

                                  testPropertyWithConfig
                                      config10k
                                      "NonEmptyList functor laws: preserves composition"
                                      (Prop.forAll(arbitraryMapMapNE())
                                       <| fun (f, g, value) -> map (f << g) value = (map f << map g) value)

                                  testPropertyWithConfig
                                      config10k
                                      "NonEmptyList monad laws: left identity"
                                      (Prop.forAll(leftIdentity()) <| fun (f, a) -> ret a >>= f = f a)

                                  testPropertyWithConfig
                                      config10k
                                      "NonEmptyList monad laws: right identity"
                                      (Prop.forAll(neListOfInt()) <| fun x -> x >>= ret = x)

                                  testPropertyWithConfig
                                      config10k
                                      "NonEmptyList monad laws: associativity"
                                      (Prop.forAll(associativity())
                                       <| fun (f, g, v) ->
                                           let a = (v >>= f) >>= g
                                           let b = v >>= (fun x -> f x >>= g)
                                           a = b)

                                  testPropertyWithConfig
                                      config10k
                                      "toList gives non-empty list"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> NonEmptyList.toList nel |> List.length > 0)

                                  testPropertyWithConfig
                                      config10k
                                      "toArray gives non-empty array"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> NonEmptyList.toArray nel |> Array.length > 0)

                                  testPropertyWithConfig
                                      config10k
                                      "toList is same length as non-empty list"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> NonEmptyList.toList nel |> List.length = nel.Length)

                                  testPropertyWithConfig
                                      config10k
                                      "toArray is same length as non-empty list"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> NonEmptyList.toArray nel |> Array.length = nel.Length)

                                  testPropertyWithConfig config10k "ofArray"
                                  <| fun arr ->
                                      try
                                          Seq.forall2 (=) (NonEmptyList.ofArray arr) arr
                                      with :? System.ArgumentException ->
                                          arr.Length = 0

                                  testPropertyWithConfig config10k "ofList"
                                  <| fun l ->
                                      try
                                          Seq.forall2 (=) (NonEmptyList.ofList l) l
                                      with :? System.ArgumentException ->
                                          l = []

                                  testPropertyWithConfig
                                      config10k
                                      "ofSeq"
                                      (Prop.forAll(Gen.ArbitrarySeq())
                                       <| fun s ->
                                           try
                                               Seq.forall2 (=) (NonEmptyList.ofSeq s) s
                                           with :? System.ArgumentException ->
                                               Seq.isEmpty s)

                                  testPropertyWithConfig
                                      config10k
                                      "reverse . reverse = id"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> (NonEmptyList.rev << NonEmptyList.rev) nel = nel)

                                  testPropertyWithConfig
                                      config10k
                                      "last . reverse = head"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> (NonEmptyList.last << NonEmptyList.rev) nel = NonEmptyList.head nel)

                                  testPropertyWithConfig
                                      config10k
                                      "head . reverse = last"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel -> (NonEmptyList.head << NonEmptyList.rev) nel = NonEmptyList.last nel)

                                  testPropertyWithConfig
                                      config10k
                                      "last is last and never fails"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel ->
                                           let actualLast = NonEmptyList.last nel

                                           let expectedLast =
                                               let l = NonEmptyList.toList nel
                                               l.[l.Length - 1]

                                           expectedLast = actualLast)

                                  testPropertyWithConfig config10k "append has combined length"
                                  <| fun (a: _ list) (b: _ list) ->
                                      if a.IsEmpty || b.IsEmpty then
                                          // we don't test non-empty lists here.
                                          true
                                      else
                                          let neA = NonEmptyList.create a.Head a.Tail
                                          let neB = NonEmptyList.create b.Head b.Tail
                                          (NonEmptyList.append neA neB).Length = neA.Length + neB.Length

                                  testPropertyWithConfig
                                      config10k
                                      "reduce"
                                      (Prop.forAll(NonEmptyListGen.NonEmptyList())
                                       <| fun nel ->
                                           let actual = NonEmptyList.reduce (+) nel
                                           let expected = nel |> NonEmptyList.toList |> List.sum
                                           expected = actual)

                                  testPropertyWithConfig
                                      config10k
                                      "zip"
                                      (Prop.forAll(EqualLengthNELGen())
                                       <| fun (nel1, nel2) ->
                                           let actual = NonEmptyList.zip nel1 nel2 |> NonEmptyList.toList
                                           let expected = List.zip <| NonEmptyList.toList nel1 <| NonEmptyList.toList nel2
                                           expected = actual)

                                  testPropertyWithConfig
                                      config10k
                                      "zip on lists with different lengths raises an exception"
                                      (Prop.forAll(twoDifferentLengths())
                                       <| fun (nel1, nel2) ->
                                           Expect.throwsT<System.ArgumentException>
                                               (sprintf "length %i; length %i" nel1.Length nel2.Length)
                                               (fun () -> NonEmptyList.zip nel1 nel2 |> ignore)) ]

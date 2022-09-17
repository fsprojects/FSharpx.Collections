namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open Properties
open Expecto
open Expecto.Flip
open FsCheck

module DListTests =

    [<Tests>]
    let testDList =

        let emptyDList = DList.empty

        testList "DList" [ test "allow to DList.tail to work" {
                               Expect.isTrue "DList.conj DList.tail" (emptyDList |> DList.conj 1 |> DList.tail |> DList.isEmpty)
                           }

                           test "DList.conj to work" {
                               Expect.isFalse "DList.conj length" (emptyDList |> DList.conj 1 |> DList.conj 2 |> DList.isEmpty)
                           }

                           test "DList.cons to work" {
                               Expect.equal "DList.cons length" 2 (emptyDList |> DList.cons 1 |> DList.cons 2 |> DList.length)
                           }

                           test "allow to DList.cons and DList.conj to work" {
                               Expect.equal
                                   "DList.cons DList.conj length"
                                   3
                                   (emptyDList
                                    |> DList.cons 1
                                    |> DList.cons 2
                                    |> DList.conj 3
                                    |> DList.length)
                           }

                           test "DList.cons pattern discriminator - DList" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                               let h1, t1 =
                                   match q with
                                   | DList.Cons(h, t) -> h, t
                                   | _ -> "x", q

                               Expect.isTrue "DList.cons pattern discriminator" ((h1 = "f") && (t1.Length = 5))
                           }

                           test "empty DList should be empty" { Expect.isTrue "empty" (emptyDList |> DList.isEmpty) }

                           test "fail if there is no DList.head in the DList" {
                               Expect.throwsT<System.Exception> "empty DList.head" (fun () -> emptyDList |> DList.head |> ignore)
                           }

                           test "fail if there is no DList.tail in the DList" {
                               Expect.throwsT<System.Exception> "no DList.tail" (fun () -> emptyDList |> DList.tail |> ignore)
                           }

                           test "foldBack matches build list 2" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               let lq = DList.foldBack (fun (elem: string) (l': string list) -> elem :: l') q []
                               Expect.equal "foldBack" (DList.toList q) lq
                           }

                           test "fold matches build list rev 2" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               let lq = DList.fold (fun (l': string list) (elem: string) -> elem :: l') [] q
                               Expect.equal "fold rev" (List.rev <| DList.toList q) lq
                           }

                           test "give None if there is no DList.head in the DList" { Expect.isNone "DList.tryHead" (emptyDList |> DList.tryHead) }

                           test "give None if there is no DList.tail in the DList" { Expect.isNone "tryTail" (emptyDList |> DList.tryTail) }

                           test "TryUncons wind-down to None" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                               let rec loop(q': DList<string>) =
                                   match (q'.TryUncons) with
                                   | Some(hd, tl) -> loop tl
                                   | None -> None

                               Expect.isNone "TryUncons" <| loop q
                           }

                           test "Uncons wind-down to None" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                               let rec loop(q': DList<string>) =
                                   match (q'.Uncons) with
                                   | hd, tl when tl.IsEmpty -> true
                                   | hd, tl -> loop tl

                               Expect.isTrue "Uncons" <| loop q
                           }

                           test "test length should return 6" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               Expect.equal "length" 6 <| DList.length q
                           }

                           test "singleton length 1" { Expect.equal "singleton length" 1 (DList.singleton 1 |> DList.length) }

                           test "empty length 0" { Expect.equal "empty length" 0 (DList.empty |> DList.length) }

                           test "test ofSeq should create a DList from a list" {
                               let test = [ for i in 0..4 -> i ]
                               Expect.sequenceEqual "ofSeq" (List.toSeq test) (DList.ofSeq test |> DList.toSeq)
                           }

                           test "test ofSeq should create a DList from an array" {
                               let test = [| for i in 0..4 -> i |]
                               Expect.sequenceEqual "ofSeq from Array" (Array.toSeq test) (DList.ofSeq test |> DList.toSeq)
                           }

                           test "test singleton should return a Unit containing the solo value" {
                               Expect.equal "singleton" 1 (DList.singleton 1 |> DList.head)
                           }

                           test "test append should join two DLists together" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               let q2 = DList.ofSeq [ "1"; "2"; "3"; "4"; "5"; "6" ]
                               let q3 = DList.append q q2
                               Expect.equal "append" 12 (q3 |> DList.length)
                               Expect.equal "append" "f" (q3 |> DList.head)
                           }

                           test "test toSeq" {
                               let q = DList.ofSeq [ "f"; "e"; "d"; "c"; "b"; "a" ]

                               Expect.equal "toSeq" [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               <| List.ofSeq(DList.toSeq q)
                           }

                           test "test toList" {
                               let l = [ "f"; "e"; "d"; "c"; "b"; "a" ]
                               let q = DList.ofSeq l
                               Expect.equal "toList" l <| DList.toList q
                           }

                           test "structural equality" {
                               let l1 = DList.ofSeq [ 1..100 ]
                               let l2 = DList.ofSeq [ 1..100 ]

                               Expect.sequenceEqual "structural equality" l1 l2

                               let l3 = DList.ofSeq [ 1..99 ] |> DList.conj 7

                               Expect.isFalse "structural equality" (l1 = l3)
                           }

                           test "test DList pairwise on 0 1 2 3 lengths" {
                               for lengthTest in 0..3 do
                                   let testList = [ for i in 0..lengthTest -> i ]
                                   let expectedPairs = testList |> List.pairwise |> DList.ofSeq
                                   let testDList = DList.ofSeq testList
                                   let paired = DList.pairwise testDList

                                   let message =
                                       sprintf "pairwise does not match List.pairwise for length %d" lengthTest

                                   Expect.sequenceEqual message expectedPairs paired
                           }

                           test "test DList pairwise 10k" {
                               let testList = [ for i in 0..10000 -> i ]
                               let expectedPairs = testList |> List.pairwise |> DList.ofSeq
                               let testDList = DList.ofSeq testList
                               let paired = DList.pairwise testDList
                               Expect.sequenceEqual "pairwise does not match List.pairwise" expectedPairs paired
                           } ]

    [<Tests>]
    let propertyTestDList =

        let enDListThruList l q =
            let rec loop (q': 'a DList) (l': 'a list) =
                match l' with
                | hd :: [] -> q'.Conj hd
                | hd :: tl -> loop (q'.Conj hd) tl
                | [] -> q'

            loop q l

        //DList
        (*
        non-IDList generators from random ofList
        *)
        let DListOfListGen = gen {
            let! n = Gen.length2thru12
            let! x = Gen.listInt n
            return ((DList.ofSeq x), x)
        }

        (*
        IDList generators from random ofSeq and/or DList.conj elements from random list 
        *)
        let DListIntGen = gen {
            let! n = Gen.length1thru12
            let! n2 = Gen.length2thru12
            let! x = Gen.listInt n
            let! y = Gen.listInt n2
            return ((DList.ofSeq x |> enDListThruList y), (x @ y))
        }

        let DListIntOfSeqGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((DList.ofSeq x), x)
        }

        let DListIntConjGen = gen {
            let! n = Gen.length1thru12
            let! x = Gen.listInt n
            return ((DList.empty |> enDListThruList x), x)
        }

        let DListObjGen = gen {
            let! n = Gen.length2thru12
            let! n2 = Gen.length1thru12
            let! x = Gen.listObj n
            let! y = Gen.listObj n2
            return ((DList.ofSeq x |> enDListThruList y), (x @ y))
        }

        let DListStringGen = gen {
            let! n = Gen.length1thru12
            let! n2 = Gen.length2thru12
            let! x = Gen.listString n
            let! y = Gen.listString n2
            return ((DList.ofSeq x |> enDListThruList y), (x @ y))
        }

        let intGens start =
            let v = Array.create 3 DListIntGen
            v.[1] <- DListIntOfSeqGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v.[2] <- DListIntConjGen |> Gen.filter(fun (q, l) -> l.Length >= start)
            v

        let intGensStart1 = intGens 1 //this will accept all

        let intGensStart2 = intGens 2 // this will accept 11 out of 12

        testList "DList property tests" [

                                          testPropertyWithConfig
                                              config10k
                                              "DList fold matches build list rev"
                                              (Prop.forAll(Arb.fromGen DListIntGen)
                                               <| fun (q, l) -> q |> DList.fold (fun l' elem -> elem :: l') [] = List.rev l)

                                          testPropertyWithConfig
                                              config10k
                                              "DList OfSeq fold matches build list rev"
                                              (Prop.forAll(Arb.fromGen DListIntOfSeqGen)
                                               <| fun (q, l) -> q |> DList.fold (fun l' elem -> elem :: l') [] = List.rev l)

                                          testPropertyWithConfig
                                              config10k
                                              "DList Conj fold matches build list rev"
                                              (Prop.forAll(Arb.fromGen DListIntConjGen)
                                               <| fun (q, l) -> q |> DList.fold (fun l' elem -> elem :: l') [] = List.rev l)

                                          testPropertyWithConfig
                                              config10k
                                              "DList foldBack matches build list"
                                              (Prop.forAll(Arb.fromGen DListIntGen)
                                               <| fun (q, l) -> DList.foldBack (fun elem l' -> elem :: l') q [] = l)

                                          testPropertyWithConfig
                                              config10k
                                              "DList OfSeq foldBack matches build list"
                                              (Prop.forAll(Arb.fromGen DListIntOfSeqGen)
                                               <| fun (q, l) -> DList.foldBack (fun elem l' -> elem :: l') q [] = l)

                                          testPropertyWithConfig
                                              config10k
                                              "DList Conj foldBack matches build list"
                                              (Prop.forAll(Arb.fromGen DListIntConjGen)
                                               <| fun (q, l) -> DList.foldBack (fun elem l' -> elem :: l') q [] = l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList 0"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[0])
                                               <| fun (q, l) -> DList.head q = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList 1"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[1])
                                               <| fun (q, l) -> DList.head q = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList 2"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[2])
                                               <| fun (q, l) -> DList.head q = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList safely 0"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[0])
                                               <| fun (q, l) -> (DList.tryHead q).Value = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList safely 1"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[1])
                                               <| fun (q, l) -> (DList.tryHead q).Value = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.head from DList safely 2"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[2])
                                               <| fun (q, l) -> (DList.tryHead q).Value = List.item 0 l)

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList 0"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[0])
                                               <| fun ((q: DList<int>), l) -> q.Tail.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList 1"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[1])
                                               <| fun ((q: DList<int>), l) -> q.Tail.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList 2"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[2])
                                               <| fun ((q: DList<int>), l) -> q.Tail.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList safely 0"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[0])
                                               <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList safely 1"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[1])
                                               <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "get DList.tail from DList safely 2"
                                              (Prop.forAll(Arb.fromGen intGensStart2.[2])
                                               <| fun (q, l) -> q.TryTail.Value.Head = (List.item 1 l))

                                          testPropertyWithConfig
                                              config10k
                                              "int DList builds and serializes 0"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[0])
                                               <| fun (q, l) -> q |> Seq.toList = l)

                                          testPropertyWithConfig
                                              config10k
                                              "int DList builds and serializes 1"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[1])
                                               <| fun (q, l) -> q |> Seq.toList = l)

                                          testPropertyWithConfig
                                              config10k
                                              "int DList builds and serializes 2"
                                              (Prop.forAll(Arb.fromGen intGensStart1.[2])
                                               <| fun (q, l) -> q |> Seq.toList = l)

                                          testPropertyWithConfig
                                              config10k
                                              "obj DList builds and serializes"
                                              (Prop.forAll(Arb.fromGen DListObjGen)
                                               <| fun (q, l) -> q |> Seq.toList = l)

                                          testPropertyWithConfig
                                              config10k
                                              "string DList builds and serializes"
                                              (Prop.forAll(Arb.fromGen DListStringGen)
                                               <| fun (q, l) -> q |> Seq.toList = l) ]

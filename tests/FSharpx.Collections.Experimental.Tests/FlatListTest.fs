namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip

(*
FlatList generators from random FlatList.ofSeq and/or conj elements from random list 
*)

module FlatListTest =

    let flatlistIntGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listInt n
        return ((FlatList.ofSeq x), x)
    }

    let flatlistObjGen = gen {
        let! n = Gen.length2thru100
        let! x = Gen.listObj n
        return ((FlatList.ofSeq x), x)
    }

    let flatlistStringGen = gen {
        let! n = Gen.length1thru100
        let! x = Gen.listString n
        return ((FlatList.ofSeq x), x)
    }

    // NUnit TestCaseSource does not understand array of tuples at runtime
    let intGens start =
        let v = Array.create 2 flatlistIntGen
        v.[1] <- flatlistIntGen |> Gen.filter(fun (v, l) -> l.Length >= start)
        v

    let intGensStart1 = intGens 1 //this will accept all

    let intGensStart2 = intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testFlatList =

        testList "Experimental FlatList" [

            test "FlatList.append: multiple appends to an FlatList.empty flatlist should increase the FlatList.length" {
                FlatList.empty
                |> FlatList.append(FlatList.singleton 1)
                |> FlatList.append(FlatList.singleton 4)
                |> FlatList.append(FlatList.singleton 25)
                |> FlatList.length
                |> Expect.equal "" 3
            }

            test "FlatList.append: multiple FlatList.append to an FlatList.empty flatlist should create a flatlist" {
                let x =
                    (FlatList.empty
                     |> FlatList.append(FlatList.singleton 1)
                     |> FlatList.append(FlatList.singleton 4)
                     |> FlatList.append(FlatList.singleton 25))

                x.[0] |> Expect.equal "" 25
                x.[1] |> Expect.equal "" 4
                x.[2] |> Expect.equal "" 1
            }

            test "FlatList.append: to an FlatList.empty flatlist should create a FlatList.singleton flatlist" {
                (FlatList.empty |> FlatList.append(FlatList.singleton 1)).[0]
                |> Expect.equal "" 1
            }

            test "FlatList.concat: expected result" {

                let aTable max = seq { for i in 1..max -> [| for j in 1..max -> (i, j, i * j) |] }
                let a = Array.concat(aTable 3)

                let fTable max =
                    seq { for i in 1..max -> [| for j in 1..max -> (i, j, i * j) |] }
                    |> FlatList.ofSeq

                let f = FlatList.concat(fTable 3)

                Array.toList a |> Expect.equal "" (FlatList.toList f)
            }

            test "Equality: flatlist with 3 elements can be compared" {
                let flatlist1 = ref FlatList.empty

                for i in 1..3 do
                    flatlist1 := FlatList.append (!flatlist1) (FlatList.singleton i)

                let flatlist2 = ref FlatList.empty

                for i in 1..3 do
                    flatlist2 := FlatList.append (!flatlist2) (FlatList.singleton i)

                let flatlist3 = ref FlatList.empty

                for i in 1..3 do
                    flatlist3 := FlatList.append (!flatlist3) (FlatList.singleton(2 * i))


                flatlist1 = flatlist1 |> Expect.isTrue ""
                flatlist1 = flatlist2 |> Expect.isTrue ""
                flatlist1 = flatlist3 |> Expect.isFalse ""
            }

            test "Equality: structural equality" {

                let l1 = FlatList.ofSeq [ 1..100 ]
                let l2 = FlatList.ofSeq [ 1..100 ]

                l1 = l2 |> Expect.isTrue ""

                let l3 = FlatList.append (FlatList.ofSeq [ 1..99 ]) (FlatList.singleton 99)

                l1 = l3 |> Expect.isFalse ""
            }

            test "FlatList.empty: flatlist should be FlatList.empty" {
                let x = FlatList.empty<int>
                x |> FlatList.length |> Expect.equal "" 0
            }

            test "GetHashCode: flatlist with 3 elements can compute hashcodes" {
                let flatlist1 = ref FlatList.empty

                for i in 1..3 do
                    flatlist1 := FlatList.append (!flatlist1) (FlatList.singleton i)

                let flatlist2 = ref FlatList.empty

                for i in 1..3 do
                    flatlist2 := FlatList.append (!flatlist2) (FlatList.singleton i)

                let flatlist3 = ref FlatList.empty

                for i in 1..3 do
                    flatlist3 := FlatList.append (!flatlist3) (FlatList.singleton(2 * i))

                flatlist1.GetHashCode() |> Expect.equal "" (flatlist2.GetHashCode())

                ((flatlist1.GetHashCode()) = (flatlist3.GetHashCode()))
                |> Expect.isFalse ""
            }

            test "FlatList.init: flatlist should allow FlatList.init" {
                let flatlist = FlatList.init 5 (fun x -> x * 2)
                let s = Seq.init 5 (fun x -> x * 2)

                s |> Seq.toList |> Expect.equal "" [ 0; 2; 4; 6; 8 ]
                flatlist |> Seq.toList |> Expect.equal "" [ 0; 2; 4; 6; 8 ]
            }

            test "IEnumarable: flatlist with 300 elements should be convertable to a seq" {
                let flatlist = ref FlatList.empty

                for i in 1..300 do
                    flatlist := FlatList.append (!flatlist) (FlatList.singleton i)

                !flatlist |> Seq.toList |> Expect.equal "" [ 1..300 ]
            }

            test "FlatList.iter: flatlist should allow FlatList.iter" {
                let l' = ref []

                let l2 = [ 1; 2; 3; 4 ]
                let v = FlatList.ofSeq l2

                FlatList.iter (fun (elem: int) -> l' := elem :: !l') v

                !l' |> Expect.equal "" (List.rev l2)
            }

            test "FlatList.iter2: flatlist should allow FlatList.iter2" {
                let l' = ref []

                let l2 = [ 1; 2; 3; 4 ]
                let v = FlatList.ofSeq l2

                FlatList.iter2 (fun (elem1: int) (elem2: int) -> l' := elem1 :: elem2 :: !l') v v

                !l' |> Expect.equal "" (List.rev [ 1; 1; 2; 2; 3; 3; 4; 4 ])
            }

            test "FlatList.iteri: flatlist should allow FlatList.iteri" {
                let l' = ref []

                let l2 = [ 1; 2; 3; 4 ]
                let v = FlatList.ofSeq l2

                FlatList.iteri (fun i (elem: int) -> l' := (i * elem) :: !l') v

                !l' |> Expect.equal "" (List.rev [ 0; 2; 6; 12 ])
            }

            test "FlatList.ofList: flatlist can be created" {
                let xs = [ 7; 88; 1; 4; 25; 30 ]
                FlatList.ofList xs |> Seq.toList |> Expect.equal "" xs
            }

            test "FlatList.ofSeq: flatlist can be created" {
                let xs = [ 7; 88; 1; 4; 25; 30 ]
                FlatList.ofSeq xs |> Seq.toList |> Expect.equal "" xs
            }

            test "physicalEquality: works" {
                let l1 = FlatList.ofSeq [ 1..100 ]
                let l2 = l1
                let l3 = FlatList.ofSeq [ 1..100 ]

                FlatList.physicalEquality l1 l2 |> Expect.isTrue ""

                FlatList.physicalEquality l1 l3 |> Expect.isFalse ""
            }

            test "FlatList.rev: FlatList.empty" { FlatList.isEmpty(FlatList.empty |> FlatList.rev) |> Expect.isTrue "" }

            test "FlatList.toMap: works" {
                let l2 = [ (1, "a"); (2, "b"); (3, "c"); (4, "d") ]
                let v = FlatList.ofList l2

                let m = FlatList.toMap v

                m.[1] |> Expect.equal "" "a"
                m.[2] |> Expect.equal "" "b"
                m.[3] |> Expect.equal "" "c"
                m.[4] |> Expect.equal "" "d"
                m.ContainsKey 5 |> Expect.isFalse ""
            }

            test "FlatList.unzip: works" {
                let l2 = [ (1, "a"); (2, "b"); (3, "c"); (4, "d") ]
                let v = FlatList.ofList l2
                let x, y = FlatList.unzip v

                FlatList.toList x |> Expect.equal "" [ 1; 2; 3; 4 ]
                FlatList.toList y |> Expect.equal "" [ "a"; "b"; "c"; "d" ]
            }

            test "FlatList.zip: works" {
                let l1 = [ 1; 2; 3; 4 ]
                let l2 = [ "a"; "b"; "c"; "d" ]
                let v1 = FlatList.ofList l1
                let v2 = FlatList.ofList l2
                let v = FlatList.zip v1 v2

                FlatList.toList v
                |> Expect.equal "" [ (1, "a"); (2, "b"); (3, "c"); (4, "d") ]
            }
        ]

    [<Tests>]
    let testFlatListProperties =

        let listFun =
            fun (l': (int * int) list) (elem1: int) (elem2: int) -> (elem1, elem2) :: l'

        let objFun =
            fun (l': (obj * obj) list) (elem1: obj) (elem2: obj) -> (elem1, elem2) :: l'

        let stringFun =
            fun (l': (string * string) list) (elem1: string) (elem2: string) -> (elem1, elem2) :: l'

        let rec nth l i =
            match i with
            | 0 -> List.head l
            | _ -> nth (List.tail l) (i - 1)

        testList "Experimental FlatList Properties" [
            testPropertyWithConfig
                config10k
                "FlatList.collect: expected result"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     v
                     |> FlatList.collect(fun elem -> FlatList.ofList [ 0..elem ])
                     |> FlatList.toList = (l
                                           |> Array.ofList
                                           |> Array.collect(fun elem -> [| 0..elem |])
                                           |> Array.toList))

            testPropertyWithConfig
                config10k
                "FlatList.exists: expected result"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> v |> FlatList.exists(fun elem -> elem = 6) = (l |> Array.ofList |> Array.exists(fun elem -> elem = 6)))

            testPropertyWithConfig
                config10k
                "filter: expected result"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     v |> FlatList.filter(fun elem -> elem % 2 = 0) |> FlatList.toList = (l
                                                                                          |> Array.ofList
                                                                                          |> Array.filter(fun elem -> elem % 2 = 0)
                                                                                          |> Array.toList))

            testPropertyWithConfig
                config10k
                "fold: matches build list FlatList.rev int"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> v |> FlatList.fold (fun (l': int list) (elem: int) -> elem :: l') [] = (List.rev l))

            testPropertyWithConfig
                config10k
                "fold: matches build list FlatList.rev obj"
                (Prop.forAll(Arb.fromGen flatlistObjGen)
                 <| fun (v, l) -> v |> FlatList.fold (fun (l': obj list) (elem: obj) -> elem :: l') [] = (List.rev l))

            testPropertyWithConfig
                config10k
                "fold: matches build list FlatList.rev string"
                (Prop.forAll(Arb.fromGen flatlistStringGen)
                 <| fun (v, l) ->
                     v
                     |> FlatList.fold (fun (l': string list) (elem: string) -> elem :: l') [] = (List.rev l))

            testPropertyWithConfig
                config10k
                "fold2: matches build list fold int"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> (v, v) ||> FlatList.fold2 listFun [] = List.fold2 listFun [] l l)

            testPropertyWithConfig
                config10k
                "fold2: matches build list fold obj"
                (Prop.forAll(Arb.fromGen flatlistObjGen)
                 <| fun (v, l) -> (v, v) ||> FlatList.fold2 objFun [] = List.fold2 objFun [] l l)

            testPropertyWithConfig
                config10k
                "fold2: matches build list fold string"
                (Prop.forAll(Arb.fromGen flatlistStringGen)
                 <| fun (v, l) -> (v, v) ||> FlatList.fold2 stringFun [] = List.fold2 stringFun [] l l)

            testPropertyWithConfig
                config10k
                "foldback: matches build list int"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> FlatList.foldBack (fun (elem: int) (l': int list) -> elem :: l') v [] = l)

            testPropertyWithConfig
                config10k
                "foldback: matches build list obj"
                (Prop.forAll(Arb.fromGen flatlistObjGen)
                 <| fun (v, l) -> FlatList.foldBack (fun (elem: obj) (l': obj list) -> elem :: l') v [] = l)

            testPropertyWithConfig
                config10k
                "foldback: matches build list string"
                (Prop.forAll(Arb.fromGen flatlistStringGen)
                 <| fun (v, l) -> FlatList.foldBack (fun (elem: string) (l': string list) -> elem :: l') v [] = l)

            testPropertyWithConfig
                config10k
                "foldback2: matches build FlatList"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let listFun =
                         fun (elem1: int) (elem2: int) (l': (int * int) list) -> (elem1, elem2) :: l'

                     FlatList.foldBack2 listFun v v [] = List.foldBack2 listFun l l [])

            testPropertyWithConfig
                config10k
                "foldback2: matches build FlatList obj"
                (Prop.forAll(Arb.fromGen flatlistObjGen)
                 <| fun (v, l) ->
                     let objFun =
                         fun (elem1: obj) (elem2: obj) (l': (obj * obj) list) -> (elem1, elem2) :: l'

                     FlatList.foldBack2 objFun v v [] = List.foldBack2 objFun l l [])

            testPropertyWithConfig
                config10k
                "foldback2: matches build FlatList string"
                (Prop.forAll(Arb.fromGen flatlistStringGen)
                 <| fun (v, l) ->
                     let stringFun =
                         fun (elem1: string) (elem2: string) (l': (string * string) list) -> (elem1, elem2) :: l'

                     FlatList.foldBack2 stringFun v v [] = List.foldBack2 stringFun l l [])

            testPropertyWithConfig
                config10k
                "forall: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> FlatList.forall (fun (elem: int) -> elem < 1000) v = true)

            testPropertyWithConfig
                config10k
                "forall2: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> FlatList.forall2 (fun (elem1: int) (elem2: int) -> (elem1 < 1000 && elem2 < 1000)) v v = true)

            testPropertyWithConfig
                config10k
                "Item: get last from flatlist"
                (Prop.forAll(Arb.fromGen intGensStart1.[0])
                 <| fun (v: FlatList<int>, l: list<int>) -> v.[l.Length - 1] = (nth l (l.Length - 1)))

            testPropertyWithConfig
                config10k
                "map: flatlist should allow map"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funMap = (fun x -> x * 2)
                     FlatList.map funMap v |> FlatList.toList = List.map funMap l)

            testPropertyWithConfig
                config10k
                "map2: flatlist should allow map2"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funMap2 = (fun x y -> ((x * 2), (y * 2)))
                     FlatList.map2 funMap2 v v |> FlatList.toList = List.map2 funMap2 l l)

            testPropertyWithConfig
                config10k
                "mapi: flatlist should allow mapi"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funMapi = (fun i x -> i * x)
                     FlatList.mapi funMapi v |> FlatList.toList = List.mapi funMapi l)

            testPropertyWithConfig
                config10k
                "partition: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funMapi = (fun x -> x % 2 = 0)
                     let x, y = FlatList.partition funMapi v
                     ((FlatList.toList x), (FlatList.toList y)) = List.partition funMapi l)

            testPropertyWithConfig
                config10k
                "FlatList.rev: matches build FlatList FlatList.rev"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (q, l) -> q |> FlatList.rev |> List.ofSeq = (List.rev l))

            testPropertyWithConfig
                config10k
                "FlatList.rev: matches build FlatList obj FlatList.rev"
                (Prop.forAll(Arb.fromGen flatlistObjGen)
                 <| fun (q, l) -> q |> FlatList.rev |> List.ofSeq = (List.rev l))

            testPropertyWithConfig
                config10k
                "FlatList.rev: matches build FlatList string FlatList.rev"
                (Prop.forAll(Arb.fromGen flatlistStringGen)
                 <| fun (q, l) -> q |> FlatList.rev |> List.ofSeq = (List.rev l))

            testPropertyWithConfig
                config10k
                "sum: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) -> FlatList.sum v = List.sum l)

            testPropertyWithConfig
                config10k
                "sumBy: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funSumBy = (fun x -> x * 2)
                     FlatList.sumBy funSumBy v = List.sumBy funSumBy l)

            testPropertyWithConfig
                config10k
                "tryFind: works"
                (Prop.forAll(Arb.fromGen flatlistIntGen)
                 <| fun (v, l) ->
                     let funTryFind = (fun x -> x % 2 = 0)

                     match FlatList.tryFind funTryFind v with
                     | None -> None = List.tryFind funTryFind l
                     | Some x -> x = (List.tryFind funTryFind l).Value)
        ]

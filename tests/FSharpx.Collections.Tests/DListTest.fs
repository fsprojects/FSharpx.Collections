namespace FSharpx.Collections.Tests

open System
open FSharpx.Collections
open FSharpx.Collections.DList
open FSharpx.Collections.Tests.Properties
open Expecto
open Expecto.Flip
open FsCheck

module DListTests =
    
    [<Tests>]
    let testDList =

        let emptyDList = DList.empty

        testList "DList" [
            test "allow to tail to work" {
                Expect.isTrue "conj tail" (emptyDList |> conj 1 |> tail |> isEmpty) }

            test "conj to work" {
                Expect.isFalse "conj length" (emptyDList |> conj 1 |> conj 2 |> isEmpty) }

            test "cons to work" {
                Expect.equal "cons length" 2 (emptyDList |> cons 1 |> cons 2 |> length) }

            test "allow to cons and conj to work" {
                Expect.equal "cons conj length" 3 (emptyDList |> cons 1 |> cons 2 |> conj 3 |> length) }

            test "cons pattern discriminator - DList" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
    
                let h1, t1 = 
                    match q with
                    | Cons(h, t) -> h, t
                    | _ ->  "x", q

                Expect.isTrue "cons pattern discriminator" ((h1 = "f") && (t1.Length = 5)) }

            test "empty DList should be empty" {
                Expect.isTrue "empty" (emptyDList |> isEmpty) }

            test "fail if there is no head in the DList" {
                Expect.throwsT<System.Exception> "empty head" (fun () -> emptyDList |> head |> ignore) }

            test "fail if there is no tail in the DList" {
                Expect.throwsT<System.Exception> "no tail" (fun () -> emptyDList |> tail |> ignore) }

            test "foldBack matches build list 2" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
                let lq = foldBack (fun (elem : string) (l' : string list) -> elem::l') q []
                Expect.equal "foldBack" (DList.toList q) lq }

            test "fold matches build list rev 2" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
                let lq = fold (fun (l' : string list) (elem : string) -> elem::l') [] q
                Expect.equal "fold rev" (List.rev <| DList.toList q) lq }

            test "give None if there is no head in the DList" {
                Expect.isNone "tryHead" (emptyDList |> tryHead) }

            test "give None if there is no tail in the DList" {
                Expect.isNone "tryTail" (emptyDList |> tryTail) } 

            test "TryUncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : DList<string>) = 
                    match (q'.TryUncons) with
                    | Some(hd, tl) ->  loop tl
                    | None -> None

                Expect.isNone "TryUncons" <| loop q }

            test "Uncons wind-down to None" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 

                let rec loop (q' : DList<string>) = 
                    match (q'.Uncons) with
                    | hd, tl when tl.IsEmpty ->  true
                    | hd, tl ->  loop tl

                Expect.isTrue "Uncons" <| loop q }

            test "test length should return 6" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 
                Expect.equal "length" 6 <| length q }

            test "singleton length 1" {
                Expect.equal "singleton length" 1 (singleton 1 |> length) }

            test "empty length 0" {
                Expect.equal "empty length" 0 (empty |> length) }

            test "test ofSeq should create a DList from a list" {
                let test = [ for i in 0..4 -> i ]
                Expect.equal "ofSeq" (List.toSeq test) (DList.ofSeq test |> DList.toSeq) }

            test "test ofSeq should create a DList from an array" {
                let test = [| for i in 0..4 -> i |]
                Expect.equal "ofSeq from Array" (Array.toSeq test)  (DList.ofSeq test |> DList.toSeq) }

            test "test singleton should return a Unit containing the solo value" {
                Expect.equal "singleton" 1 (singleton 1 |> head) }

            test "test append should join two DLists together" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"]
                let q2 = ofSeq ["1";"2";"3";"4";"5";"6"]
                let q3 =  append q q2
                Expect.equal "append" 12 (q3 |> length)
                Expect.equal "append" "f" (q3  |> head) }

            test "test toSeq" {
                let q = ofSeq ["f";"e";"d";"c";"b";"a"] 
                Expect.equal "toSeq" ["f";"e";"d";"c";"b";"a"] <| List.ofSeq (DList.toSeq q) }

            test "test toList" {
                let l = ["f";"e";"d";"c";"b";"a"]
                let q = ofSeq  l
                Expect.equal "toList" l  <| DList.toList q }

            //type DListGen =
            //    static member DList {
            //        let rec dListGen { 
            //            gen {
            //                let! xs = Arb.generate
            //                return DList.ofSeq (Seq.ofList xs)
            //            }
            //        Arb.fromGen (dListGen())

            //let registerGen = lazy (Arb.register<DListGen>() |> ignore)

            //[<Test>]
            test "structural equality" {
                let l1 = ofSeq [1..100]
                let l2 = ofSeq [1..100]

                Expect.equal "structural equality" l1 l2

                let l3 = ofSeq [1..99] |> conj 7

                Expect.equal "structural equality" l1 l3 }
        ]

    //[<Tests>]
    //let propertyTestDList =

    //    let enDListThruList l q  =
    //        let rec loop (q' : 'a DList) (l' : 'a list) = 
    //            match l' with
    //            | hd :: [] -> q'.Conj hd
    //            | hd :: tl -> loop (q'.Conj hd) tl
    //            | [] -> q'
        
    //        loop q l

    //    //DList
    //    (*
    //    non-IDList generators from random ofList
    //    *)
    //    let DListOfListGen =
    //        gen {   let! n = Gen.length2thru12
    //                let! x = Gen.listInt n
    //                return ( (DList.ofSeq x), x) }

    //    (*
    //    IDList generators from random ofSeq and/or conj elements from random list 
    //    *)
    //    let DListIntGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! n2 = Gen.length2thru12
    //                let! x =  Gen.listInt n
    //                let! y =  Gen.listInt n2
    //                return ( (DList.ofSeq x |> enDListThruList y), (x @ y) ) }

    //    let DListIntOfSeqGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x = Gen.listInt n
    //                return ( (DList.ofSeq x), x) }

    //    let DListIntConjGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! x = Gen.listInt n
    //                return ( (DList.empty |> enDListThruList x), x) }

    //    let DListObjGen =
    //        gen {   let! n = Gen.length2thru12
    //                let! n2 = Gen.length1thru12
    //                let! x =  Gen.listObj n
    //                let! y =  Gen.listObj n2
    //                return ( (DList.ofSeq x |> enDListThruList y), (x @ y) ) }

    //    let DListStringGen =
    //        gen {   let! n = Gen.length1thru12
    //                let! n2 = Gen.length2thru12
    //                let! x =  Gen.listString n
    //                let! y =  Gen.listString n2  
    //                return ( (DList.ofSeq x |> enDListThruList y), (x @ y) ) }

    //    // HACK: from when using NUnit TestCaseSource does not understand array of tuples at runtime
    //    let intGens start =
    //        let v = Array.create 3 (box (DListIntGen, "DList"))
    //        v.[1] <- box ((DListIntOfSeqGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "DList OfSeq")
    //        v.[2] <- box ((DListIntConjGen |> Gen.filter (fun (q, l) -> l.Length >= start)), "DList conjDList") 
    //        v

    //    let intGensStart1 =
    //        intGens 1  //this will accept all

    //    let intGensStart2 =
    //        intGens 2 // this will accept 11 out of 12

    //    testList "DList property tests" [

    //        testPropertyWithConfig config10k "DList fold matches build list rev" (Prop.forAll (Arb.fromGen DListIntGen) <|
    //            fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = List.rev l )
              
    //        testPropertyWithConfig config10k "DList OfSeq fold matches build list rev" (Prop.forAll (Arb.fromGen DListIntOfSeqGen) <|
    //            fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = List.rev l )

    //        testPropertyWithConfig config10k "DList Conj fold matches build list rev" (Prop.forAll (Arb.fromGen DListIntConjGen) <|
    //            fun (q, l) -> q |> fold (fun l' elem -> elem::l') [] = List.rev l )

    //        testPropertyWithConfig config10k "DList foldBack matches build list" (Prop.forAll (Arb.fromGen DListIntGen) <|
    //            fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )
              
    //        testPropertyWithConfig config10k "DList OfSeq foldBack matches build list" (Prop.forAll (Arb.fromGen DListIntOfSeqGen) <|
    //            fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

    //        testPropertyWithConfig config10k "DList Conj foldBack matches build list" (Prop.forAll (Arb.fromGen DListIntConjGen)  <|
    //            fun (q, l) -> foldBack (fun elem l' -> elem::l') q [] = l )

    //        testPropertyWithConfig config10k "get head from DList" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
    //            fun (q : DList<int>, l) -> (head q) = (List.item 0 l) )

    //        testPropertyWithConfig config10k "get head from DList safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
    //            fun (q : DList<int>, l) -> (tryHead q).Value = (List.item 0 l) )

    //        testPropertyWithConfig config10k "get tail from DList" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun ((q : DList<int>), l) -> q.Tail.Head = (List.item 1 l) )

    //        testPropertyWithConfig config10k "get tail from DList safely" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart2)) <|
    //            fun (q : DList<int>, l) -> q.TryTail.Value.Head = (List.item 1 l) )

    //        testPropertyWithConfig config10k "int DList builds and serializes" (Prop.forAll (Arb.fromGen (fst <| unbox intGensStart1)) <|
    //            fun (q : DList<int>, l) -> q |> Seq.toList = l )

    //        testPropertyWithConfig config10k "obj DList builds and serializes" (Prop.forAll (Arb.fromGen DListObjGen) <|
    //            fun (q, l) -> q |> Seq.toList = l )

    //        testPropertyWithConfig config10k "string DList builds and serializes" (Prop.forAll (Arb.fromGen DListStringGen) <|
    //            fun (q, l) -> q |> Seq.toList = l )
    //    ]
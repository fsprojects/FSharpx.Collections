namespace FSharpx.Collections.Experimental.Tests

open System
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.FlatList
open FSharpx.Collections.Experimental.Tests.Properties
open FsCheck
open Expecto
open Expecto.Flip

(*
FlatList generators from random ofSeq and/or conj elements from random list 
*)

module FlatListTest =

    let flatlistIntGen =
        gen {   let! n = Gen.length1thru100
                let! x = Gen.listInt n
                return ( (FlatList.ofSeq x), x) }

    let flatlistObjGen =
        gen {   let! n = Gen.length2thru100
                let! x =  Gen.listObj n
                return ( (FlatList.ofSeq x), x) }

    let flatlistStringGen =
        gen {   let! n = Gen.length1thru100
                let! x =  Gen.listString n
                return ( (FlatList.ofSeq x), x) }

    // NUnit TestCaseSource does not understand array of tuples at runtime
    let intGens start =
        let v = Array.create 2 (box (flatlistIntGen, "FlatList"))
        v.[1] <- box ((flatlistIntGen |> Gen.filter (fun (v, l) -> l.Length >= start)), "FlatList OfSeq")
        v

    let intGensStart1 =
        intGens 1  //this will accept all

    let intGensStart2 =
        intGens 2 // this will accept 11 out of 12

    [<Tests>]
    let testFlatList =

        testList "Experimental FlatList" [

            //[<Test>]
            //test "append: multiple appends to an empty flatlist should increase the length" {
            //    empty |> append (singleton 1) |> append (singleton  4) |> append (singleton  25) |> length |> Expect.equal "" } 3

            //[<Test>]
            //test "append: multiple append to an empty flatlist should create a flatlist" {
            //    let x = (empty |> append (singleton  1) |> append (singleton  4) |> append (singleton  25)) 
            //    x.[0] |> Expect.equal "" } 25
            //    x.[1] |> Expect.equal "" } 4
            //    x.[2] |> Expect.equal "" } 1

            //[<Test>]
            //test "append: to an empty flatlist should create a singleton flatlist" {
            //    (empty |> append (singleton 1)).[0] |> Expect.equal "" } 1

            //[<Test>]
            //test "collect: expected result" {

            //    fsCheck "FlatList int" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v :FlatList<int>), (l : int list)) -> v |> collect (fun elem -> ofList [ 0 .. elem ]) 
            //                                                       |> toList
            //                                                       = (l |> Array.ofList 
            //                                                            |> Array.collect (fun elem -> [| 0 .. elem |])
            //                                                            |> Array.toList) ))


            //[<Test>]
            //test "concat: expected result" {

            //    let aTable max = seq { for i in 1 .. max -> [| for j in 1 .. max -> (i, j, i*j) |] }
            //    let a = Array.concat (aTable 3)

            //    let fTable max = 
            //        seq { for i in 1 .. max -> [| for j in 1 .. max -> (i, j, i*j) |]}
            //        |> ofSeq
            //    let f = concat (fTable 3)

            //    Array.toList a |> Expect.equal "" } (toList f)

            //[<Test>]
            //test "Equality: flatlist with 3 elements can be compared" {
            //    let flatlist1 = ref empty
            //    for i in 1..3 do
            //        flatlist1 := append (!flatlist1) (singleton i)

            //    let flatlist2 = ref empty
            //    for i in 1..3 do
            //        flatlist2 := append (!flatlist2) (singleton i)

            //    let flatlist3 = ref empty
            //    for i in 1..3 do
            //        flatlist3 := append (!flatlist3) (singleton (2*i))


            //    flatlist1 = flatlist1 |> Expect.isTrue "" }
            //    flatlist1 = flatlist2 |> Expect.isTrue "" }
            //    flatlist1 = flatlist3 |> Expect.isFalse "" }

            //[<Test>]
            //test "Equality: structural equality" {

            //    let l1 = ofSeq [1..100]
            //    let l2 = ofSeq [1..100]

            //    l1 = l2 |> Expect.isTrue "" }

            //    let l3 = append (ofSeq [1..99]) (singleton 99)

            //    l1 = l3 |> Expect.isFalse "" }

            //[<Test>]
            //test "empty: flatlist should be empty" {
            //    let x = empty<int>
            //    x |> length |> Expect.equal "" } 0

            //[<Test>]
            //test "exists: expected result" {

            //    fsCheck "FlatList int" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v :FlatList<int>), (l : int list)) -> v |> exists (fun elem -> elem = 6) 
            //                                                       = (l |> Array.ofList 
            //                                                            |> Array.exists (fun elem -> elem = 6))
            //                                                            ))

            //[<Test>]
            //test "filter: expected result" {

            //    fsCheck "FlatList int" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v :FlatList<int>), (l : int list)) -> v |> filter (fun elem -> elem % 2 = 0) 
            //                                                       |> toList
            //                                                       = (l |> Array.ofList 
            //                                                            |> Array.filter (fun elem -> elem % 2 = 0)
            //                                                            |> Array.toList) ))

            //[<Test>]
            //test "fold: matches build list rev" {

            //    fsCheck "FlatList int" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v :FlatList<int>), (l : int list)) -> v |> fold (fun (l' : int list) (elem : int) -> elem::l') [] = (List.rev l) ))
              
            //    fsCheck "FlatList obj" (Prop.forAll (Arb.fromGen flatlistObjGen) 
            //       (fun ((v :FlatList<obj>), (l : obj list)) -> v |> fold (fun (l' : obj list) (elem : obj) -> elem::l') [] = (List.rev l) ))

            //    fsCheck "FlatList string" (Prop.forAll (Arb.fromGen flatlistStringGen) 
            //         (fun ((v :FlatList<string>), (l : string list)) -> v |> fold (fun (l' : string list) (elem : string) -> elem::l') [] = (List.rev l) ))

            //[<Test>]
            //test "fold2: matches build list fold" {

            //    let listFun = fun (l' : (int * int) list) (elem1 : int) (elem2 : int) -> (elem1, elem2)::l'
            //    fsCheck "FlatList int" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v :FlatList<int>), (l : int list)) -> (v,v) ||> fold2 listFun [] = List.fold2 listFun [] l l ))
              
            //    let objFun = fun (l' : (obj * obj) list) (elem1 : obj) (elem2 : obj) -> (elem1, elem2)::l'
            //    fsCheck "FlatList obj" (Prop.forAll (Arb.fromGen flatlistObjGen) 
            //       (fun ((v :FlatList<obj>), (l : obj list)) -> (v,v) ||> fold2 objFun [] = List.fold2 objFun [] l l ))

            //    let stringFun = fun (l' : (string * string) list) (elem1 : string) (elem2 : string) -> (elem1, elem2)::l'
            //    fsCheck "FlatList string" (Prop.forAll (Arb.fromGen flatlistStringGen) 
            //         (fun ((v :FlatList<string>), (l : string list)) -> (v,v) ||> fold2 stringFun [] = List.fold2 stringFun []  l l ))

            //[<Test>]
            //test "foldback: matches build list" {

            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> foldBack (fun (elem : int) (l' : int list)  -> elem::l') v [] = l ))
              
            //    fsCheck "FlatList obj" (Prop.forAll (Arb.fromGen flatlistObjGen) 
            //        (fun ((v : FlatList<obj>), (l : obj list)) -> foldBack (fun (elem : obj) (l' : obj list) -> elem::l') v [] = l ))

            //    fsCheck "FlatList string" (Prop.forAll (Arb.fromGen flatlistStringGen) 
            //        (fun ((v : FlatList<string>), (l : string list)) -> foldBack (fun (elem : string) (l' : string list) -> elem::l') v [] = l ))

            //[<Test>]
            //test "foldback2: matches build list" {

            //    let listFun = fun (elem1 : int) (elem2 : int) (l' : (int * int) list) -> (elem1, elem2)::l'
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> foldBack2 listFun v v [] = List.foldBack2 listFun l l [] ))
              
            //    let objFun = fun (elem1 : obj) (elem2 : obj) (l' : (obj * obj) list) -> (elem1, elem2)::l'
            //    fsCheck "FlatList obj" (Prop.forAll (Arb.fromGen flatlistObjGen) 
            //        (fun ((v : FlatList<obj>), (l : obj list)) -> foldBack2 objFun v v [] = List.foldBack2 objFun l l [] ))

            //    let stringFun = fun (elem1 : string) (elem2 : string) (l' : (string * string) list) -> (elem1, elem2)::l'
            //    fsCheck "FlatList string" (Prop.forAll (Arb.fromGen flatlistStringGen) 
            //        (fun ((v : FlatList<string>), (l : string list)) -> foldBack2 stringFun v v [] = List.foldBack2 stringFun l l [] ))

            //[<Test>]
            //test "forall: works" {

            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> forall (fun (elem : int) -> elem < 1000) v  =  true ))

            //[<Test>]
            //test "forall2: works" {

            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> forall2 (fun (elem1 : int) (elem2 : int) -> (elem1 < 1000 && elem2 < 1000)) v v  =  true ))

            //[<Test>]
            //test "GetHashCode: flatlist with 3 elements can compute hashcodes" {
            //    let flatlist1 = ref empty
            //    for i in 1..3 do
            //        flatlist1 := append (!flatlist1) (singleton i) 

            //    let flatlist2 = ref empty
            //    for i in 1..3 do
            //        flatlist2 := append (!flatlist2) (singleton i)

            //    let flatlist3 = ref empty
            //    for i in 1..3 do
            //        flatlist3 := append (!flatlist3) (singleton (2*i))

            //    flatlist1.GetHashCode() |> Expect.equal "" } (flatlist2.GetHashCode())
            //    ((flatlist1.GetHashCode()) = (flatlist3.GetHashCode())) |> Expect.isFalse "" }

            //[<Test>]
            //test "init: flatlist should allow init" {
            //    let flatlist = init 5 (fun x -> x * 2) 
            //    let s = Seq.init 5 (fun x -> x * 2)

            //    s |> Seq.toList |> Expect.equal "" } [0;2;4;6;8]
            //    flatlist |> Seq.toList |> Expect.equal "" } [0;2;4;6;8]

            //[<Test>]
            //test "IEnumarable: flatlist with 300 elements should be convertable to a seq" {
            //    let flatlist = ref empty
            //    for i in 1..300 do
            //        flatlist := append (!flatlist) (singleton i) 

            //    !flatlist |> Seq.toList |> Expect.equal "" } [1..300]

            //let rec nth l i =
            //    match i with
            //    | 0 -> List.head l
            //    | _ -> nth (List.tail l) (i-1)

            //[<Test>]
            //[<TestCaseSource("intGensStart1")>]
            //test "Item: get last from flatlist``(x : obj) =
            //    let genAndName = unbox x 
            //    fsCheck (snd genAndName) (Prop.forAll (Arb.fromGen (fst genAndName)) (fun (v : FlatList<int>, l : list<int>) -> v.[l.Length - 1] = (nth l (l.Length - 1)) ))

            //[<Test>]
            //test "iter: flatlist should allow iter" {

            //    let l' = ref []
   
            //    let l2 = [1;2;3;4]
            //    let v = ofSeq l2

            //    iter (fun (elem : int) -> l' := elem::!l') v
    
            //    !l' |> Expect.equal "" } (List.rev l2)                          

            //[<Test>]
            //test "iter2: flatlist should allow iter2" {

            //    let l' = ref []
   
            //    let l2 = [1;2;3;4]
            //    let v = ofSeq l2

            //    iter2 (fun (elem1 : int) (elem2 : int) -> l' := elem1::elem2::!l') v v
    
            //    !l' |> Expect.equal "" } (List.rev [1;1;2;2;3;3;4;4])        

            //[<Test>]
            //test "iteri: flatlist should allow iteri" {

            //    let l' = ref []
   
            //    let l2 = [1;2;3;4]
            //    let v = ofSeq l2

            //    iteri (fun i (elem : int) -> l' := (i * elem)::!l') v
    
            //    !l' |> Expect.equal "" } (List.rev [0;2;6;12])   


            //[<Test>]
            //test "map: flatlist should allow map" {

            //    let funMap = (fun x ->  x * 2)
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> map funMap v |> toList =  List.map funMap l ))

            //[<Test>]
            //test "map2: flatlist should allow map2" {

            //    let funMap2 = (fun x y ->  ((x * 2), (y * 2)))
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> map2 funMap2 v v |> toList =  List.map2 funMap2 l l ))

            //[<Test>]
            //test "mapi: flatlist should allow mapi" {

            //    let funMapi = (fun i x ->  i * x)
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> mapi funMapi v |> toList =  List.mapi funMapi l ))

            //[<Test>]
            //test "ofList: flatlist can be created" {
            //    let xs = [7;88;1;4;25;30] 
            //    ofList xs |> Seq.toList |> Expect.equal "" } xs

            //[<Test>]
            //test "ofSeq: flatlist can be created" {
            //    let xs = [7;88;1;4;25;30] 
            //    ofSeq xs |> Seq.toList |> Expect.equal "" } xs

            //[<Test>]
            //test "partition: works" {

            //    let funMapi = (fun x ->  x % 2 = 0)
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> let x, y = partition funMapi v 
            //                                                      ((toList x),(toList y)) =  List.partition funMapi l ))

            //[<Test>]
            //test "physicalEquality: works" {

            //    let l1 = ofSeq [1..100]
            //    let l2 = l1
            //    let l3 = ofSeq [1..100]

            //    physicalEquality l1 l2 |> Expect.isTrue "" }

            //    physicalEquality l1 l3 |> Expect.isFalse "" }

            //[<Test>]
            //test "rev: empty" {
            //    isEmpty (empty |> rev) |> Expect.isTrue "" }
    
            //[<Test>]
            //test "rev: matches build list rev" {

            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((q :FlatList<int>), (l : int list)) -> q |> rev |> List.ofSeq = (List.rev l) ))
              
            //    fsCheck "FlatList obj" (Prop.forAll (Arb.fromGen flatlistObjGen) 
            //        (fun ((q :FlatList<obj>), (l : obj list)) -> q |> rev |> List.ofSeq = (List.rev l) ))

            //    fsCheck "FlatList string" (Prop.forAll (Arb.fromGen flatlistStringGen) 
            //         (fun ((q :FlatList<string>), (l : string list)) -> q |> rev |> List.ofSeq = (List.rev l) ))

            //[<Test>]
            //test "sum: works" {

            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> sum v = List.sum l ))

            //[<Test>]
            //test "sumBy: works" {

            //    let funSumBy = (fun x ->  x * 2)
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> sumBy funSumBy v = List.sumBy funSumBy l ))

            //[<Test>]
            //test "toMap: works" {
  
            //    let l2 = [(1,"a");(2,"b");(3,"c");(4,"d")]
            //    let v = ofList l2

            //    let m = toMap v
    
            //    m.[1] |> Expect.equal "" "a" }   
            //    m.[2] |> Expect.equal "" } "b"
            //    m.[3] |> Expect.equal "" } "c"
            //    m.[4] |> Expect.equal "" } "d" 
            //    m.ContainsKey 5 |> Expect.isFalse "" }

            //[<Test>]
            //test "tryFind: works" {

            //    let funTryFind = (fun x ->  x % 2 = 0)
            //    fsCheck "FlatList" (Prop.forAll (Arb.fromGen flatlistIntGen) 
            //        (fun ((v : FlatList<int>), (l : int list)) -> 
            //                                                    match tryFind funTryFind v with
            //                                                    | None -> None = List.tryFind funTryFind l 
            //                                                    | Some x -> x = (List.tryFind funTryFind l).Value  ))

            //[<Test>]
            //test "unzip: works" {
  
            //    let l2 = [(1,"a");(2,"b");(3,"c");(4,"d")]
            //    let v = ofList l2
            //    let x, y = unzip v
    
            //    toList x |> Expect.equal "" } [1;2;3;4]  
            //    toList y |> Expect.equal "" } ["a";"b";"c";"d"]

            //[<Test>]
            //test "zip: works" {
  
            //    let l1 = [1;2;3;4]
            //    let l2 = ["a";"b";"c";"d"]
            //    let v1 = ofList l1
            //    let v2 = ofList l2
            //    let v = zip v1 v2
    
            //    toList v |> Expect.equal "" } [(1,"a");(2,"b");(3,"c");(4,"d")]  
        ]
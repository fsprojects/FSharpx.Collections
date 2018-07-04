namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Properties
open FsCheck
open Expecto
open Expecto.Flip
open System.Linq

module IntMapTest =
    type IntMapGen =
        static member IntMap() =
            let rec intMapGen() = 
                gen {
                    let! ks = Arb.generate
                    let! xs = Arb.generate
                    return IntMap.ofSeq (Seq.zip (Seq.ofList xs) (Seq.ofList ks))
                }
            Arb.fromGen (intMapGen())

    let registerGen = lazy (Arb.register<IntMapGen>() |> ignore)

    [<Tests>]
    let testIntMap =

        let employeeDept = IntMap.ofList([(1,2); (3,1)])

        testList "Experimental IntMap" [
            test "test isEmpty" {
                IntMap.isEmpty IntMap.empty |> Expect.isTrue ""
                (1, 'a') ||> IntMap.singleton |> IntMap.isEmpty |> Expect.isFalse "" }

            test "test size" {
                IntMap.size IntMap.empty |> Expect.equal "" 0
                IntMap.singleton 1 'a' |> IntMap.size |> Expect.equal "" 1
                IntMap.ofList [(1,'a'); (2,'b'); (3,'c')] |> IntMap.size |> Expect.equal "" 3} 

            test "test member" {
                IntMap.exists 5 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.isTrue ""
                IntMap.exists 1 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.isFalse "" }

            test "test notMember" {
                IntMap.notExists 5 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.isFalse ""
                IntMap.notExists 1 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.isTrue "" }

            test "test tryFind" {
                let deptCountry = IntMap.ofList([(1,1); (2,2)])
                let countryCurrency = IntMap.ofList([(1, 2); (2, 1)])
                let employeeCurrency name = 
                    match IntMap.tryFind name employeeDept with
                    | Some dept ->
                        match IntMap.tryFind dept deptCountry with
                        | Some country -> IntMap.tryFind country countryCurrency
                        | None -> None
                    | None -> None

                employeeCurrency 1 |> Expect.equal "" (Some 1)
                employeeCurrency 2 |> Expect.isNone "" }

            test "test find" {
                let employeeDept = IntMap.ofList([(1,1); (2,2)])
                Expect.equal "" 2 <| IntMap.find 2 employeeDept}


            test "test findWithDefault" {
                IntMap.findWithDefault 'x' 1 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.equal "" 'x'
                IntMap.findWithDefault 'x' 5 (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.equal "" 'a' }

            test "test tryFindLT" {
                IntMap.tryFindLT 3 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.isNone ""
                IntMap.tryFindLT 4 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(3, 'a')) } 

            test "test tryFindGT" {
                IntMap.tryFindGT 4 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(5,'b'))
                IntMap.tryFindGT 5 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.isNone "" }

            test "test tryFindLE" {
                IntMap.tryFindLE 2 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.isNone ""
                IntMap.tryFindLE 4 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(3,'a'))
                IntMap.tryFindLE 5 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(5,'b')) }

            test "test tryFindGE" {
                IntMap.tryFindGE 3 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(3,'a'))
                IntMap.tryFindGE 4 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.equal "" (Some(5,'b'))
                IntMap.tryFindGE 6 (IntMap.ofList [(3,'a'); (5,'b')]) |> Expect.isNone "" }

            // Construction

            test "test empty" {
                IntMap.empty |> Expect.equal "" (IntMap.ofList [])
                IntMap.size IntMap.empty |> Expect.equal "" 0 }

            test "test singleton" {
                IntMap.singleton 1 'a' |> Expect.equal "" (IntMap.ofList [(1, 'a')])
                IntMap.size (IntMap.singleton 1 'a') |> Expect.equal "" 1 }

            test "test insert" {
                IntMap.insert 5 'x' (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.equal "" (IntMap.ofList [(3, 'b'); (5, 'x')])
                IntMap.insert 7 'x' (IntMap.ofList [(5,'a'); (3,'b')]) |> Expect.equal "" (IntMap.ofList [(3, 'b'); (5, 'a'); (7, 'x')])
                IntMap.insert 5 'x' IntMap.empty |> Expect.equal "" (IntMap.singleton 5 'x') } 

            test "test insertWith" {
                IntMap.insertWith (+) 5 "xxx" (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "xxxa")])
                IntMap.insertWith (+) 7 "xxx" (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a"); (7, "xxx")])
                IntMap.insertWith (+) 5 "xxx" IntMap.empty |> Expect.equal "" (IntMap.singleton 5 "xxx") } 

            test "test insertWithKey" {
                let f key new_value old_value = string key + ":" + new_value + "|" + old_value
                IntMap.insertWithKey f 5 "xxx" (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "5:xxx|a")])
                IntMap.insertWithKey f 7 "xxx" (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a"); (7, "xxx")])
                IntMap.insertWithKey f 5 "xxx" IntMap.empty |> Expect.equal "" (IntMap.singleton 5 "xxx") } 

            test "test insertTryFindWithKey" {
                let f key new_value old_value = string key + ":" + new_value + "|" + old_value
                IntMap.insertTryFindWithKey f 5 "xxx" (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some "a", IntMap.ofList [(3, "b"); (5, "5:xxx|a")])
                let fnd, map = IntMap.insertTryFindWithKey f 2 "xxx" (IntMap.ofList [(5,"a"); (3,"b")])
                fnd |> Expect.isNone ""
                map |> Expect.equal "" (IntMap.ofList [(2,"xxx");(3,"b");(5,"a")])
                let fnd, map = IntMap.insertTryFindWithKey f 7 "xxx" (IntMap.ofList [(5,"a"); (3,"b")])
                fnd |> Expect.isNone ""
                map |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a"); (7, "xxx")])
                let fnd, map = IntMap.insertTryFindWithKey f 5 "xxx" IntMap.empty
                fnd |> Expect.isNone ""
                map |> Expect.equal "" (IntMap.singleton 5 "xxx") } 

            // Delete/Update

            test "test delete" {
                IntMap.delete 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 3 "b")
                IntMap.delete 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.delete 5 IntMap.empty |> Expect.equal "" IntMap.empty } 

            test "test adjust" {
                IntMap.adjust ((+) "new ") 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "new a")])
                IntMap.adjust ((+) "new ") 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.adjust ((+) "new ") 7 IntMap.empty |> Expect.equal "" IntMap.empty } 

            test "test adjustWithKey" {
                let f key x = (string key) + ":new " + x
                IntMap.adjustWithKey f 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "5:new a")])
                IntMap.adjustWithKey f 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal ""  (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.adjustWithKey f 7 IntMap.empty |> Expect.equal "" IntMap.empty } 

            test "test update" {
                let f x = if x = "a" then Some "new a" else None
                IntMap.update f 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "new a")])
                IntMap.update f 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.update f 3 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test updateWithKey" {
                let f k x = if x = "a" then Some ((string k) + ":new a") else None
                IntMap.updateWithKey f 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "5:new a")])
                IntMap.updateWithKey f 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.updateWithKey f 3 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test updateTryFindWithKey" {
                let f k x = if x = "a" then Some ((string k) + ":new a") else None
                IntMap.updateTryFindWithKey f 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some "a", IntMap.ofList [(3, "b"); (5, "5:new a")])
                let fnd, map = IntMap.updateTryFindWithKey f 7 (IntMap.ofList [(5,"a"); (3,"b")])
                fnd |> Expect.isNone "" 
                map |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.updateTryFindWithKey f 3 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some "b", IntMap.singleton 5 "a") } 

            test "test alter" {
                let f _ = None
                let g _ = Some "c"
                IntMap.alter f 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                IntMap.alter f 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 3 "b")
                IntMap.alter g 7 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a"); (7, "c")])
                IntMap.alter g 5 (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "c")]) }

            // Combine

            test "test append" {
                IntMap.append (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a"); (7, "C")]) } 

            test "test appendWith" {
                IntMap.appendWith (+) (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "aA"); (7, "C")]) } 

            test "test appendWithKey" { 
                let f key left_value right_value = (string key) + ":" + left_value + "|" + right_value
                IntMap.appendWithKey f (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "5:a|A"); (7, "C")]) } 

            test "test concat" {
                IntMap.concat [IntMap.ofList [(5, "a"); (3, "b")]; IntMap.ofList [(5, "A"); (7, "C")]; IntMap.ofList [(5, "A3"); (3, "B3")]]
                |> Expect.equal ""  (IntMap.ofList [(3, "b"); (5, "a"); (7, "C")])
                IntMap.concat [IntMap.ofList [(5, "A3"); (3, "B3")]; IntMap.ofList [(5, "A"); (7, "C")]; IntMap.ofList [(5, "a"); (3, "b")]]
                |> Expect.equal "" (IntMap.ofList [(3, "B3"); (5, "A3"); (7, "C")]) } 

            test "test concatWith" {
                IntMap.concatWith (+) [IntMap.ofList [(5, "a"); (3, "b")]; IntMap.ofList [(5, "A"); (7, "C")]; IntMap.ofList [(5, "A3"); (3, "B3")]]
                |> Expect.equal "" (IntMap.ofList [(3, "bB3"); (5, "aAA3"); (7, "C")]) } 

            test "test difference" {
                IntMap.difference (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.singleton 3 "b") } 

            test "test differenceWith" {
                let f al ar = if al = "b" then Some (al + ":" + ar) else None
                IntMap.differenceWith f (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (3, "B"); (7, "C")])
                |> Expect.equal "" (IntMap.singleton 3 "b:B") } 

            test "test differenceWithKey" {
                let f k al ar = if al = "b" then Some ((string k) + ":" + al + "|" + ar) else None
                IntMap.differenceWithKey f (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (3, "B"); (10, "C")])
                |> Expect.equal "" (IntMap.singleton 3 "3:b|B") } 

            test "test intersection" {
                IntMap.intersection (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test intersectionWith" {
                IntMap.intersectionWith (+) (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.singleton 5 "aA") } 

            test "test intersectionWithKey" {
                let f k al ar = (string k) + ":" + al + "|" + ar
                IntMap.intersectionWithKey f (IntMap.ofList [(5, "a"); (3, "b")]) (IntMap.ofList [(5, "A"); (7, "C")])
                |> Expect.equal "" (IntMap.singleton 5 "5:a|A") } 

            // Traversal

            test "test map" {
                IntMap.map ((fun f a b -> f b a) (+) "x") (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "bx"); (5, "ax")]) } 

            test "test mapWithKey" {
                let f key x = (string key) + ":" + x
                IntMap.mapWithKey f (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "3:b"); (5, "5:a")]) } 

            test "test mapAccum" {
                let f a b = (a + b, b + "X")
                IntMap.mapAccum f "Everything: " (IntMap.ofList [(5,"a"); (3,"b")])
                |> Expect.equal "" ("Everything: ba", IntMap.ofList [(3, "bX"); (5, "aX")]) } 

            test "test mapAccumWithKey" {
                let f a k b = (a + " " + (string k) + "-" + b, b + "X")
                IntMap.mapAccumWithKey f "Everything:" (IntMap.ofList [(5,"a"); (3,"b")])
                |> Expect.equal "" ("Everything: 3-b 5-a", IntMap.ofList [(3, "bX"); (5, "aX")]) } 

            test "test mapKeys" {
                IntMap.mapKeys ((fun f a b -> f b a) (+) 1) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal ""  (IntMap.ofList [(4, "b"); (6, "a")])
                IntMap.mapKeys (fun _ -> 1) (IntMap.ofList [(1,"b"); (2,"a"); (3,"d"); (4,"c")]) |> Expect.equal ""  (IntMap.singleton 1 "c")
                IntMap.mapKeys (fun _ -> 3) (IntMap.ofList [(1,"b"); (2,"a"); (3,"d"); (4,"c")]) |> Expect.equal "" (IntMap.singleton 3 "c") } 

            test "test mapKeysWith" {
                IntMap.mapKeysWith (+) (fun _ -> 1) (IntMap.ofList [(1,"b"); (2,"a"); (3,"d"); (4,"c")]) |> Expect.equal ""  (IntMap.singleton 1 "cdab")
                IntMap.mapKeysWith (+) (fun _ -> 3) (IntMap.ofList [(1,"b"); (2,"a"); (3,"d"); (4,"c")]) |> Expect.equal "" (IntMap.singleton 3 "cdab") } 

            // Conversion

            test "test values" {
                IntMap.values (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" ["b";"a"]
                IntMap.values IntMap.empty |> Expect.equal "" [] } 

            test "test keys" {
                IntMap.keys (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" [3;5] 
                IntMap.keys IntMap.empty |> Expect.equal "" [] }

            // Lists

            test "test toList" {
                IntMap.toList (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" [(3,"b"); (5,"a")]
                IntMap.toList IntMap.empty |> Expect.equal "" [] } 

            test "test ofList" {
                IntMap.ofList [] |> Expect.equal "" IntMap.empty
                IntMap.ofList [(5,"a"); (3,"b"); (5, "c")] |> Expect.equal "" (IntMap.ofList [(5,"c"); (3,"b")])
                IntMap.ofList [(5,"c"); (3,"b"); (5, "a")] |> Expect.equal "" (IntMap.ofList [(5,"a"); (3,"b")]) } 

            test "test ofListWith" {
                IntMap.ofListWith (+) [(5,"a"); (5,"b"); (3,"b"); (3,"a"); (5,"a")] |> Expect.equal "" (IntMap.ofList [(3, "ab"); (5, "aba")])
                IntMap.ofListWith (+) [] |> Expect.equal "" IntMap.empty } 

            test "test ofListWithKey" {
                let f k a1 a2 = (string k) + a1 + a2
                IntMap.ofListWithKey f [(5,"a"); (5,"b"); (3,"b"); (3,"a"); (5,"a")] |> Expect.equal "" (IntMap.ofList [(3, "3ab"); (5, "5a5ba")])
                IntMap.ofListWithKey f [] |> Expect.equal "" IntMap.empty } 

            // Filter

            test "test filter" {
                IntMap.filter ((fun f a b -> f b a) (>) "a") (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 3 "b")
                IntMap.filter ((fun f a b -> f b a) (>) "x") (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" IntMap.empty
                IntMap.filter ((fun f a b -> f b a) (<) "a") (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" IntMap.empty } 

            test "test filteWithKey" {
                IntMap.filterWithKey (fun k _ -> k > 4) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test partition" {
                let left, right = IntMap.partition ((fun f a b -> f b a) (>) "a") (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 3 "b")
                right |> Expect.equal "" (IntMap.singleton 5 "a")
                let left, right = IntMap.partition ((fun f a b -> f b a) (<) "x") (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                right |> Expect.equal "" IntMap.empty
                let left, right = IntMap.partition ((fun f a b -> f b a) (>) "x") (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                right |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")]) } 

            test "test partitionWithKey" {
                let left, right = IntMap.partitionWithKey (fun k _ -> k > 3) (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 5 "a")
                right |> Expect.equal "" (IntMap.singleton 3 "b")
                let left, right = IntMap.partitionWithKey (fun k _ -> k < 7) (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")])
                right |> Expect.equal "" IntMap.empty
                let left, right = IntMap.partitionWithKey (fun k _ -> k > 7) (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                right |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "a")]) } 

            test "test mapOption" {
                let f x = if x = "a" then Some "new a" else None
                IntMap.mapOption f (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "new a") } 

            test "test mapOptionWithKey" {
                let f k _ = if k < 5 then Some ("key : " + (string k)) else None
                IntMap.mapOptionWithKey f (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 3 "key : 3") } 

            test "test mapChoice" {
                let f a = if a < "c" then Choice1Of2 a else Choice2Of2 a
                IntMap.mapChoice f (IntMap.ofList [(5,"a"); (3,"b"); (1,"x"); (7,"z")])
                |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")], IntMap.ofList [(1,"x"); (7,"z")])
                IntMap.mapChoice (fun a -> Choice2Of2 a) (IntMap.ofList [(5,"a"); (3,"b"); (1,"x"); (7,"z")])
                |> Expect.equal "" (IntMap.empty, IntMap.ofList [(5,"a"); (3,"b"); (1,"x"); (7,"z")]) } 

            test "test mapChoiceWithKey" {
                let f k a = if k < 5 then Choice1Of2 (k * 2) else Choice2Of2 (a + a)
                IntMap.mapChoiceWithKey f (IntMap.ofList [(5,"a"); (3,"b"); (1,"x"); (7,"z")])
                |> Expect.equal "" (IntMap.ofList [(1,2); (3,6)], IntMap.ofList [(5,"aa"); (7,"zz")])
                IntMap.mapChoiceWithKey (fun _ a -> Choice2Of2 a) (IntMap.ofList [(5,"a"); (3,"b"); (1,"x"); (7,"z")])
                |> Expect.equal "" (IntMap.empty, IntMap.ofList [(1,"x"); (3,"b"); (5,"a"); (7,"z")]) } 

            test "test split" {
                let left, right = IntMap.split 2 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                right |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")])
                let left, right = IntMap.split 3 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                right |> Expect.equal "" (IntMap.singleton 5 "a")
                let left, right = IntMap.split 4 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 3 "b")
                right |> Expect.equal "" (IntMap.singleton 5 "a")
                let left, right = IntMap.split 5 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 3 "b")
                right |> Expect.equal "" IntMap.empty
                let left, right = IntMap.split 6 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")])
                right |> Expect.equal "" IntMap.empty } 

            test "test splitTryFind" {
                let left, center, right = IntMap.splitTryFind 2 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                center |> Expect.isNone "" 
                right |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")])
                let left, center, right = IntMap.splitTryFind 3 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" IntMap.empty
                center |> Expect.equal "" (Some "b")
                right |> Expect.equal "" (IntMap.singleton 5 "a")
                let left, center, right = IntMap.splitTryFind 4 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 3 "b")
                center |> Expect.isNone "" 
                right |> Expect.equal "" (IntMap.singleton 5 "a")
                let left, center, right = IntMap.splitTryFind 5 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.singleton 3 "b")
                center |> Expect.equal "" (Some "a")
                right |> Expect.equal "" IntMap.empty
                let left, center, right = IntMap.splitTryFind 6 (IntMap.ofList [(5,"a"); (3,"b")])
                left |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")])
                center |> Expect.isNone "" 
                right |> Expect.equal "" IntMap.empty } 

            // Submap

            test "test isSubmapOfBy" {
                IntMap.isSubmapOfBy (=) (IntMap.ofList [(int 'a',1)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isTrue ""
                IntMap.isSubmapOfBy (<=) (IntMap.ofList [(int 'a',1)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isTrue ""
                IntMap.isSubmapOfBy (=) (IntMap.ofList [(int 'a',1);(int 'b',2)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isTrue ""
                IntMap.isSubmapOfBy (=) (IntMap.ofList [(int 'a',2)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isFalse ""
                IntMap.isSubmapOfBy (<)  (IntMap.ofList [(int 'a',1)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isFalse ""
                IntMap.isSubmapOfBy (=) (IntMap.ofList [(int 'a',1);(int 'b',2)]) (IntMap.ofList [(int 'a',1)]) |> Expect.isFalse "" }

            test "test isSubmapOf" {
                IntMap.isSubmapOf (IntMap.ofList [(int 'a',1)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isTrue ""
                IntMap.isSubmapOf (IntMap.ofList [(int 'a',1);(int 'b',2)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isTrue ""
                IntMap.isSubmapOf (IntMap.ofList [(int 'a',2)]) (IntMap.ofList [(int 'a',1);(int 'b',2)]) |> Expect.isFalse ""
                IntMap.isSubmapOf (IntMap.ofList [(int 'a',1);(int 'b',2)]) (IntMap.ofList [(int 'a',1)]) |> Expect.isFalse "" }

            test "test isProperSubmapOfBy" {
                IntMap.isProperSubmapOfBy (=) (IntMap.ofList [(1,1)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isTrue ""
                IntMap.isProperSubmapOfBy (<=) (IntMap.ofList [(1,1)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isTrue ""
                IntMap.isProperSubmapOfBy (=) (IntMap.ofList [(1,1);(2,2)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isFalse ""
                IntMap.isProperSubmapOfBy (=) (IntMap.ofList [(1,1);(2,2)]) (IntMap.ofList [(1,1)]) |> Expect.isFalse ""
                IntMap.isProperSubmapOfBy (<)  (IntMap.ofList [(1,1)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isFalse "" }

            test "test isProperSubmapOf" {
                IntMap.isProperSubmapOf (IntMap.ofList [(1,1)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isTrue ""
                IntMap.isProperSubmapOf (IntMap.ofList [(1,1);(2,2)]) (IntMap.ofList [(1,1);(2,2)]) |> Expect.isFalse ""
                IntMap.isProperSubmapOf (IntMap.ofList [(1,1);(2,2)]) (IntMap.ofList [(1,1)]) |> Expect.isFalse "" }

            // Min/Max

            test "test findMin" {
                IntMap.findMin (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (3,"b") } 

            test "test findMax" {
                IntMap.findMax (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (5,"a") } 

            test "test deleteMin" {
                IntMap.deleteMin (IntMap.ofList [(5,"a"); (3,"b"); (7,"c")]) |> Expect.equal "" (IntMap.ofList [(5,"a"); (7,"c")])
                IntMap.deleteMin IntMap.empty |> Expect.equal "" IntMap.empty } 

            test "test deleteMax" {
                IntMap.deleteMax (IntMap.ofList [(5,"a"); (3,"b"); (7,"c")]) |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"a")])
                IntMap.deleteMax IntMap.empty |> Expect.equal "" IntMap.empty } 

            test "test deleteFindMin" {
                IntMap.deleteFindMin (IntMap.ofList [(5,"a"); (3,"b"); (10,"c")]) |> Expect.equal "" ((3,"b"), IntMap.ofList [(5,"a"); (10,"c")]) } 

            test "test deleteFindMax" {
                IntMap.deleteFindMax (IntMap.ofList [(5,"a"); (3,"b"); (10,"c")]) |> Expect.equal "" ((10,"c"), IntMap.ofList [(3,"b"); (5,"a")]) } 

            test "test updateMin" {
                IntMap.updateMin (fun a -> Some ("X" + a)) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "Xb"); (5, "a")])
                IntMap.updateMin (fun _ -> None) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test updateMax" {
                IntMap.updateMax (fun a -> Some ("X" + a)) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.ofList [(3, "b"); (5, "Xa")])
                IntMap.updateMax (fun _ -> None) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 3 "b") } 

            test "test updateMinWithKey" {
                IntMap.updateMinWithKey (fun k a -> Some ((string k) + ":" + a)) (IntMap.ofList [(5,"a"); (3,"b")])
                |> Expect.equal "" (IntMap.ofList [(3,"3:b"); (5,"a")])
                IntMap.updateMinWithKey (fun _ _ -> None) (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (IntMap.singleton 5 "a") } 

            test "test updateMaxWithKey" {
                IntMap.updateMaxWithKey (fun k a -> Some ((string k) + ":" + a)) (IntMap.ofList [(5,"a"); (3,"b")])
                |> Expect.equal "" (IntMap.ofList [(3,"b"); (5,"5:a")])
                IntMap.updateMaxWithKey (fun _ _ -> None) (IntMap.ofList [(5,"a"); (3,"b")])
                |> Expect.equal "" (IntMap.singleton 3 "b") } 

            test "test minView" {
                IntMap.minView (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some ("b", IntMap.singleton 5 "a"))
                IntMap.minView IntMap.empty |> Expect.isNone "" }

            test "test maxView" {
                IntMap.maxView (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some ("a", IntMap.singleton 3 "b"))
                IntMap.maxView IntMap.empty |> Expect.isNone "" }

            test "test minViewWithKey" {
                IntMap.minViewWithKey (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some ((3,"b"), IntMap.singleton 5 "a"))
                IntMap.minViewWithKey IntMap.empty |> Expect.isNone "" }

            test "test maxViewWithKey" {
                IntMap.maxViewWithKey (IntMap.ofList [(5,"a"); (3,"b")]) |> Expect.equal "" (Some ((5,"a"), IntMap.singleton 3 "b"))
                IntMap.maxViewWithKey IntMap.empty |> Expect.isNone "" }
        ]

    [<Tests>]
    let testIntMapProperties =

        registerGen.Force()

        let except (xs: _ seq) ys = xs.Except(ys)
        let intersect (xs: _ seq) ys = xs.Intersect(ys)
        let mapOption (f: 'a -> 'b option) l = List.foldBack (fun x xs -> match f x with Some v -> v::xs | None -> xs) l []

        testList "Experimental IntMap Properties" [

            testPropertyWithConfig config10k "prop singleton" <|
                fun k x -> IntMap.insert k x IntMap.empty = IntMap.singleton k x

            testPropertyWithConfig config10k "prop insert and tryFind" <|
                fun k t -> IntMap.tryFind k (IntMap.insert k () t) <> None

            ptestPropertyWithConfig config10k "prop insert and delete" <|
                fun k t -> IntMap.tryFind k t = None ==> (IntMap.delete k (IntMap.insert k () t) = t)

            ptestPropertyWithConfig config10k "prop delete non member" <|
                fun k t -> IntMap.tryFind k t = None ==> (IntMap.delete k t = t)

            testPropertyWithConfig config10k "prop append" <|
                fun xs ys ->
                    List.sort (IntMap.keys (IntMap.append (IntMap.ofList xs) (IntMap.ofList ys)))
                        = List.sort (List.ofSeq (Seq.distinct (List.append (List.map fst xs) (List.map fst ys))))

            testPropertyWithConfig config10k "prop append and singleton" <|
                fun t k x -> IntMap.append (IntMap.singleton k x) t = IntMap.insert k x t

            testPropertyWithConfig config10k "prop append and sum" <|
                fun xs ys ->
                    List.sum (IntMap.values (IntMap.appendWith (+) (IntMap.ofListWith (+) xs) (IntMap.ofListWith (+) ys)))
                        = List.sum (List.map snd xs) + List.sum (List.map snd ys)

            testPropertyWithConfig config10k "prop difference" <|
                fun xs ys ->
                    List.sort (IntMap.keys (IntMap.difference (IntMap.ofListWith (+) xs) (IntMap.ofListWith (+) ys)))
                        = List.sort (List.ofSeq (except (Seq.distinct (List.map fst xs)) (Seq.distinct (List.map fst ys))))

            testPropertyWithConfig config10k "prop intersection" <|
                fun xs ys ->
                    List.sort (IntMap.keys (IntMap.intersection (IntMap.ofListWith (+) xs) (IntMap.ofListWith (+) ys)))
                        = List.sort (List.ofSeq (Seq.distinct (intersect (List.map fst xs) (List.map fst ys))))

            testPropertyWithConfig config10k "prop intersectionWith" <|
                fun (xs: (int * int) list) (ys: (int * int) list) ->
                    let xs' = Seq.distinctBy fst xs |> Seq.toList
                    let ys' = Seq.distinctBy fst ys |> Seq.toList
                    let f l r = l + 2 * r
                    IntMap.toList (IntMap.intersectionWith f (IntMap.ofList xs') (IntMap.ofList ys'))
                        = [ for (kx, vx) in List.sort xs' do for (ky, vy) in ys' do if kx = ky then yield (kx, f vx vy) ]

            testPropertyWithConfig config10k "prop intersectionWithKey" <|
                fun (xs: (int * int) list) (ys: (int * int) list) ->
                    let xs' = Seq.distinctBy fst xs |> Seq.toList
                    let ys' = Seq.distinctBy fst ys |> Seq.toList
                    let f k l r = k + 2 * l + 3 * r
                    IntMap.toList (IntMap.intersectionWithKey f (IntMap.ofList xs') (IntMap.ofList ys'))
                        = [ for (kx, vx) in List.sort xs' do for (ky, vy) in ys' do if kx = ky then yield (kx, f kx vx vy) ]

            testPropertyWithConfig config10k "prop mergeWithKey" <|
                fun (xs: (int * int) list) (ys: (int * int) list) ->
                    let xs' = Seq.distinctBy fst xs |> Seq.toList
                    let ys' = Seq.distinctBy fst ys |> Seq.toList
                    let xm = IntMap.ofList xs'
                    let ym = IntMap.ofList ys'

                    let emulateMergeWithKey f keep_x keep_y =
                        let combine k =
                            match (List.tryFind (fst >> (=) k) xs', List.tryFind (fst >> (=) k) ys') with
                            | (None, Some(_, y)) -> if keep_y then Some (k, y) else None
                            | (Some(_, x), None) -> if keep_x then Some (k, x) else None
                            | (Some(_, x), Some(_, y)) ->  f k x y |> Option.map (fun v -> (k, v))
                            | _ -> failwith "emulateMergeWithKey: combine"
                        mapOption combine (List.sort (List.ofSeq (Seq.distinct (List.append (List.map fst xs') (List.map fst ys')))))

                    let testMergeWithKey f keep_x keep_y =
                        let keep b m = match b with | false -> IntMap.empty | true -> m
                        IntMap.toList (IntMap.mergeWithKey f (keep keep_x) (keep keep_y) xm ym) = emulateMergeWithKey f keep_x keep_y
                    
                    List.forall id [ for f in
                        [ (fun _ x1 _ -> Some x1);
                            (fun _ _ x2 -> Some x2);
                            (fun _ _ _ -> None);
                            (fun k x1 x2 -> if k % 2 = 0 then None else Some (2 * x1 + 3 * x2))
                        ] do
                            for keep_x in [ true; false ] do
                                for keep_y in [ true; false ] do yield testMergeWithKey f keep_x keep_y
                    ]

            testPropertyWithConfig config10k "prop list" <|
                fun (xs: int list) ->
                    List.sort (List.ofSeq (Seq.distinct xs)) = [ for (x,()) in IntMap.toList (IntMap.ofList [ for x in xs do yield (x,()) ]) do yield x ]

            ptestPropertyWithConfig config10k "prop alter" <|
                fun t k ->
                    let f = function | Some () -> None | None -> Some ()
                    let t' = IntMap.alter f k t
                    match IntMap.tryFind k t with
                    | Some _ -> IntMap.size t - 1 = IntMap.size t' && IntMap.tryFind k t' = None
                    | None -> IntMap.size t + 1 = IntMap.size t' && IntMap.tryFind k t' <> None

            ptestPropertyWithConfig config10k "prop isEmpty" <|
                fun m -> IntMap.isEmpty m = (IntMap.size m = 0)

            testPropertyWithConfig config10k "prop exists" <|
                fun xs n ->
                    let m = IntMap.ofList (List.zip xs xs)
                    List.forall (fun k -> IntMap.exists k m = List.exists ((=) k) xs) (n::xs)

            testPropertyWithConfig config10k "prop notExists" <|
                fun xs n ->
                    let m = IntMap.ofList (List.zip xs xs)
                    List.forall (fun k -> IntMap.notExists k m = not (List.exists ((=) k) xs)) (n::xs)

            testPropertyWithConfig config10k "implements IEnumerable 1" <|
                fun xs ->
                    let xs = List.zip xs xs
                    let map = IntMap.ofList xs
                    let a = map :> _ seq |> Seq.toList
                    set xs = set a 

            testPropertyWithConfig config10k "implements IEnumerable 2" <|
                fun xs ->
                    let xs = List.zip xs xs
                    let map = IntMap.ofList xs
                    let a = map :> _ seq |> Seq.toList
                    a.Length = (List.length (List.ofSeq (Seq.distinct xs)))

            testPropertyWithConfig config10k "functor laws: preserves identity" <| 
                fun value -> IntMap.map id value = value

            testPropertyWithConfig config10k "functor laws: preserves composition" <|
                fun f g value -> IntMap.map (f << g) value = (IntMap.map f << IntMap.map g) value
        ]

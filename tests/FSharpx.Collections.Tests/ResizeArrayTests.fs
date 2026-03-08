namespace FSharpx.Collections.Tests

open FSharpx.Collections
open Expecto
open Expecto.Flip

module ResizeArrayTests =
    open System.Collections.Generic

    let rng = System.Random()
    let ra = ResizeArray.ofList

    let (=?) a b =
        ResizeArray.toList a = b

    [<Tests>]
    let testResizeArray =
        testList
            "ResizeArray"
            [ test "ra_exists2_a" {
                  Expect.isTrue "exists2"
                  <| ResizeArray.exists2 (=) (ra [ 1; 2; 3; 4; 5; 6 ]) (ra [ 2; 3; 4; 5; 6; 6 ])
              }

              test "exists2_b" {
                  Expect.isFalse "exists2"
                  <| ResizeArray.exists2 (=) (ra [ 1; 2; 3; 4; 5; 6 ]) (ra [ 2; 3; 4; 5; 6; 7 ])
              }

              test "ra_findIndex_a" {
                  Expect.equal "findIndex" 4
                  <| ResizeArray.findIndex (fun i -> i >= 4) (ra [ 0..10 ])
              }

              test "ra_findIndex_b" {
                  Expect.throwsT<KeyNotFoundException> "findIndex" (fun () -> ResizeArray.findIndex (fun i -> i >= 20) (ra [ 0..10 ]) |> ignore)
              }

              test "ra_find_indexi_a" {
                  Expect.equal "findIndexi" 3
                  <| ResizeArray.findIndexi (=) (ra [ 1; 2; 3; 3; 2; 1 ])
              }

              test "ra_find_indexi_b" {
                  Expect.throwsT<KeyNotFoundException> "findIndexi" (fun () -> ResizeArray.findIndexi (=) (ra [ 1..10 ]) |> ignore)
              }

              test "ra_forall2_a" {
                  Expect.isTrue "forall2"
                  <| ResizeArray.forall2 (=) (ra [ 1..10 ]) (ra [ 1..10 ])
              }

              test "ra_forall2_b" {
                  Expect.isFalse "forall2"
                  <| ResizeArray.forall2 (=) (ra [ 1; 2; 3; 4; 5 ]) (ra [ 1; 2; 3; 0; 5 ])
              }

              test "ra_isEmpty_a" { Expect.isTrue "isEmpty" <| ResizeArray.isEmpty(ra []) }

              test "ra_isEmpty_b" { Expect.isFalse "isEmpty" <| ResizeArray.isEmpty(ra [ 1; 2 ]) }

              test "ra_mapi2" {
                  Expect.isTrue
                      "mapi2"
                      (ResizeArray.mapi2 (fun i j k -> i + j + k) (ra [ 1..10 ]) (ra [ 1..10 ])
                       =? [ 2..+3..29 ])
              }

              test "ra_mapi2_b" {
                  Expect.throwsT<System.ArgumentException> "mapi2" (fun () ->
                      ResizeArray.mapi2 (fun i j k -> i + j + k) (ra []) (ra [ 1..10 ])
                      |> ignore)
              }


              test "ra_iteri2" {
                  Expect.equal "iteri2" (6 + 60 + 3)
                  <| (let c = ref 0
                      ResizeArray.iteri2 (fun i j k -> c := !c + i + j + k) (ra [ 1; 2; 3 ]) (ra [ 10; 20; 30 ])
                      !c)
              }

              test "ra_singleton" { Expect.isTrue "singleton" (ResizeArray.singleton 42 =? [ 42 ]) }

              test "ra_zip" {
                  Expect.isTrue
                      "zip"
                      (ResizeArray.zip (ra [ 1..10 ]) (ra [ 1..10 ])
                       =? [ for i in 1..10 -> i, i ])
              }

              test "ra_unzip" {
                  Expect.isTrue "unzip"
                  <| (let unzip1, unzip2 = ResizeArray.unzip <| ra [ for i in 1..10 -> i, i + 1 ]
                      unzip1 =? [ 1..10 ] && unzip2 =? [ 2..11 ])
              }

              test "ra_reduce_left" { Expect.equal "reduce" 8 <| ResizeArray.reduce (+) (ra [ 2; 2; 2; 2 ]) }

              test "ra_reduce_right" {
                  Expect.equal "reduceBack" 8
                  <| ResizeArray.reduceBack (+) (ra [ 2; 2; 2; 2 ])
              }

              test "ra_fold2" {
                  Expect.equal "fold2" 112
                  <| ResizeArray.fold2 (fun i j k -> i + j + k) 100 (ra [ 1; 2; 3 ]) (ra [ 1; 2; 3 ])
              }

              test "ra_fold2_b" {
                  Expect.equal "fold2" (100 - 12)
                  <| ResizeArray.fold2 (fun i j k -> i - j - k) 100 (ra [ 1; 2; 3 ]) (ra [ 1; 2; 3 ])
              }

              test "ra_foldBack2" {
                  Expect.equal "foldBack2" 112
                  <| ResizeArray.foldBack2 (fun i j k -> i + j + k) (ra [ 1; 2; 3 ]) (ra [ 1; 2; 3 ]) 100
              }

              test "ra_foldBack2_b" {
                  Expect.equal "foldBack2" (100 - 12)
                  <| ResizeArray.foldBack2 (fun i j k -> k - i - j) (ra [ 1; 2; 3 ]) (ra [ 1; 2; 3 ]) 100
              }

              test "ra_scan" { Expect.isTrue "scan" (ResizeArray.scan (+) 0 (ra [ 1..5 ]) =? [ 0; 1; 3; 6; 10; 15 ]) }

              test "ra_scanBack" { Expect.isTrue "scanBack" (ResizeArray.scanBack (+) (ra [ 1..5 ]) 0 =? [ 15; 14; 12; 9; 5; 0 ]) }

              test "ra_tryfind_index" {
                  Expect.equal "tryFindIndex" (Some 4)
                  <| ResizeArray.tryFindIndex (fun x -> x = 4) (ra [ 0..10 ])
              }

              test "ra_tryfind_index_b" {
                  Expect.isNone "tryFindIndex"
                  <| ResizeArray.tryFindIndex (fun x -> x = 42) (ra [ 0..10 ])
              }

              test "ra_tryfind_indexi" {
                  Expect.equal "tryFindIndexi" (Some 4)
                  <| ResizeArray.tryFindIndexi (=) (ra [ 1; 2; 3; 4; 4; 3; 2; 1 ])
              }

              test "ra_tryfind_indexi_b" {
                  Expect.isNone "tryFindIndexi"
                  <| ResizeArray.tryFindIndexi (=) (ra [ 1..10 ])
              }

              test "ra_iter" {
                  Expect.equal "iter" 100
                  <| (let c = ref -1

                      ResizeArray.iter
                          (fun x ->
                              incr c
                              Expect.equal "ra_iter" x !c)
                          (ra [ 0..100 ])

                      !c)
              }

              test "ra_map" { Expect.isTrue "map" (ra [ 1..100 ] |> ResizeArray.map((+) 1) =? [ 2..101 ]) }

              test "ra_mapi" { Expect.isTrue "mapi" (ra [ 0..100 ] |> ResizeArray.mapi (+) =? [ 0..+2..200 ]) }

              test "ra_iteri" {
                  Expect.equal "iteri" 100
                  <| (let c = ref -1

                      ResizeArray.iteri
                          (fun i x ->
                              incr c
                              Expect.isTrue "ra_iteri" (x = !c && i = !c))
                          (ra [ 0..100 ])

                      !c)
              }

              test "ra_exists" { Expect.isTrue "exists" (ra [ 1..100 ] |> ResizeArray.exists((=) 50)) }

              test "ra_exists b" { Expect.isFalse "exists" (ra [ 1..100 ] |> ResizeArray.exists((=) 150)) }

              test "ra_forall" { Expect.isTrue "forall" (ra [ 1..100 ] |> ResizeArray.forall(fun x -> x < 150)) }

              test "ra_forall b" { Expect.isFalse "forall" (ra [ 1..100 ] |> ResizeArray.forall(fun x -> x < 80)) }

              test "ra_find" {
                  Expect.equal "find" 51
                  <| (ra [ 1..100 ] |> ResizeArray.find(fun x -> x > 50))
              }

              test "ra_find b" {
                  Expect.throwsT<KeyNotFoundException> "find" (fun () -> ra [ 1..100 ] |> ResizeArray.find(fun x -> x > 180) |> ignore)
              }

              test "ra_first" {
                  Expect.equal
                      "tryPick"
                      (Some(51 * 51))
                      (ra [ 1..100 ]
                       |> ResizeArray.tryPick(fun x -> if x > 50 then Some(x * x) else None))
              }

              test "ra_first b" { Expect.isNone "tryPick" (ra [ 1..100 ] |> ResizeArray.tryPick(fun x -> None)) }

              test "ra_first c" { Expect.isNone "tryPick" (ra [] |> ResizeArray.tryPick(fun _ -> Some 42)) }

              test "ra_tryfind" { Expect.equal "tryFind" (Some 51) (ra [ 1..100 ] |> ResizeArray.tryFind(fun x -> x > 50)) }

              test "ra_tryfind b" { Expect.isNone "tryFind" (ra [ 1..100 ] |> ResizeArray.tryFind(fun x -> x > 180)) }

              test "ra_iter2" {
                  Expect.equal "iter2" 100
                  <| (let c = ref -1

                      ResizeArray.iter2
                          (fun x y ->
                              incr c
                              Expect.isTrue "ra_iter2" (!c = x && !c = y))
                          (ra [ 0..100 ])
                          (ra [ 0..100 ])

                      !c)
              }

              test "ra_map2" { Expect.isTrue "map2" (ResizeArray.map2 (+) (ra [ 0..100 ]) (ra [ 0..100 ]) =? [ 0..+2..200 ]) }

              test "ra_choose" {
                  Expect.isTrue
                      "choose"
                      (ResizeArray.choose (fun x -> if x % 2 = 0 then Some(x / 2) else None) (ra [ 0..100 ])
                       =? [ 0..50 ])
              }

              test "ra_filter" {
                  Expect.isTrue
                      "filter"
                      (ResizeArray.filter (fun x -> x % 2 = 0) (ra [ 0..100 ])
                       =? [ 0..+2..100 ])
              }

              test "ra_filter b" { Expect.isTrue "filter" (ResizeArray.filter (fun x -> false) (ra [ 0..100 ]) =? []) }

              test "ra_filter c" { Expect.isTrue "filter" (ResizeArray.filter (fun x -> true) (ra [ 0..100 ]) =? [ 0..100 ]) }

              test "ra_partition" {
                  Expect.isTrue
                      "partition"
                      (let p1, p2 = ResizeArray.partition (fun x -> x % 2 = 0) (ra [ 0..100 ])
                       p1 =? [ 0..+2..100 ] && p2 =? [ 1..+2..100 ])
              }

              test "ra_rev" { Expect.isTrue "rev" (ResizeArray.rev(ra [ 0..100 ]) =? [ 100..-1..0 ]) }

              test "ra_rev b" { Expect.isTrue "rev" (ResizeArray.rev(ra [ 1 ]) =? [ 1 ]) }

              test "ra_rev c" { Expect.isTrue "rev" (ResizeArray.rev(ra []) =? []) }

              test "ra_rev d" { Expect.isTrue "rev" (ResizeArray.rev(ra [ 1; 2 ]) =? [ 2; 1 ]) }

              test "ra_concat ra ra" {
                  Expect.isTrue
                      "concat"
                      (ResizeArray.concat(ra [ ra [ 1; 2 ]; ra [ 3 ]; ra [ 4; 5; 6 ]; ra []; ra [ 7 ] ])
                       =? [ 1; 2; 3; 4; 5; 6; 7 ])
              }

              test "ra_concat list ra" {
                  Expect.isTrue
                      "concat"
                      (ResizeArray.concat [ ra [ 1; 2 ]; ra [ 3 ]; ra [ 4; 5; 6 ]; ra []; ra [ 7 ] ]
                       =? [ 1; 2; 3; 4; 5; 6; 7 ])
              }

              test "ra_concat concat" {
                  Expect.isTrue
                      "concat"
                      (ResizeArray.concat(ra [ ra [ 1; 2 ]; ra [ 3 ]; ra [ 4; 5; 6 ]; ra []; ra [ 7 ] ])
                       =? (ResizeArray.concat(
                               Seq.ofList[ra [ 1; 2 ]
                                          ra [ 3 ]
                                          ra [ 4; 5; 6 ]
                                          ra []
                                          ra [ 7 ]]
                           )
                           |> List.ofSeq))
              }

              test "ra_distinct a" {
                  Expect.isTrue
                      "distinct"
                      (let ar =
                          [ for i = 1 to 100 do
                                yield rng.Next(0, 10) ] in

                       (ar |> Seq.distinct |> Seq.toArray) = (ar |> ra |> ResizeArray.distinct |> ResizeArray.toArray))
              }

              test "ra_distinctBy a" {
                  Expect.isTrue
                      "distinctBy"
                      (let ar =
                          [ for i = 1 to 100 do
                                yield rng.Next(0, 10), rng.Next(0, 10) ] in

                       (ar |> Seq.distinctBy(fun (x, _) -> x) |> Seq.toArray) = (ar
                                                                                 |> ra
                                                                                 |> ResizeArray.distinctBy(fun (x, _) -> x)
                                                                                 |> ResizeArray.toArray))
              } ]

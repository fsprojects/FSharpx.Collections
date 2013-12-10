(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentVector
=============

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector. 
*)

open FSharpx.Collections.PersistentVector


(**

Performance tests
-----------------

Bulk operations on HashMaps use an internal TransientHashMap in order to get much better performance. The following scripts shows this.
*)

let c = 5
let r = new System.Random()

open FSharpx.Collections.TimeMeasurement

let initArrayAndVectorFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]

    let initvector list = 
        let v = ref empty
        for x in list do
            v := conj x !v
        !v

    compareThreeRuntimes c
        "  Array.ofSeq" (fun () -> Array.ofSeq list)
        "  Multiple PersistentVector.conj " (fun () -> initvector list)
        "  PersistentVector.ofSeq" (fun () -> ofSeq list)

let lookupInArrayAndVector n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    compareTwoRuntimes c
        "  Array" (fun () -> for i in 1..count do array.[r.Next n] |> ignore)
        "  PersistentVector" (fun () -> for i in 1..count do nth (r.Next n) vector |> ignore)


let replaceInArrayAndVector n count =
    sprintf "%d writes in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    compareTwoRuntimes c
        "  Array" (fun () -> for i in 1..count do array.[r.Next n] <- r.Next())
        "  PersistentVector" (fun () -> for i in 1..count do update (r.Next n) (r.Next()) vector |> ignore)

initArrayAndVectorFromList 10000
initArrayAndVectorFromList 100000
initArrayAndVectorFromList 1000000

lookupInArrayAndVector 10000 10000
lookupInArrayAndVector 100000 10000
lookupInArrayAndVector 1000000 10000
lookupInArrayAndVector 10000000 10000

replaceInArrayAndVector 10000 10000
replaceInArrayAndVector 100000 10000
replaceInArrayAndVector 1000000 10000
replaceInArrayAndVector 10000000 10000

// [fsi:Init with n = 10000]
// [fsi:  Array.ofSeq 0.0ms]
// [fsi:  Multiple PersistentVector.conj  3.0ms]
// [fsi:  PersistentVector.ofSeq 3.0ms]
// [fsi:Init with n = 100000]
// [fsi:  Array.ofSeq 0.4ms]
// [fsi:  Multiple PersistentVector.conj  38.8ms]
// [fsi:  PersistentVector.ofSeq 17.6ms]
// [fsi:Init with n = 1000000]
// [fsi:  Array.ofSeq 5.4ms]
// [fsi:  Multiple PersistentVector.conj  420.6ms]
// [fsi:  PersistentVector.ofSeq 250.6ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 0.4ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 0.6ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 2.0ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 3.4ms]
// [fsi:10000 writes in size n = 10000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 11.4ms]
// [fsi:10000 writes in size n = 100000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 11.2ms]
// [fsi:10000 writes in size n = 1000000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 15.0ms]
// [fsi:10000 writes in size n = 10000000]
// [fsi:  Array 1.0ms]
// [fsi:  PersistentVector 33.8ms]
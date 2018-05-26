(*** hide ***)
#r "../../bin/FSharpx.Collections/net461/FSharpx.Collections.dll"
open System


(**
PersistentVector - Performance tests
====================================

Bulk operations on PersistentVector use an internal TransientVector in order to get much better performance. The following scripts shows this:
*)

open FSharpx.Collections.PersistentVector

let trials = 5
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

    averageTime trials "  Array.ofSeq" 
        (fun () -> Array.ofSeq list)

    averageTime trials "  Multiple PersistentVector.conj" 
        (fun () -> initvector list)

    averageTime trials "  PersistentVector.ofSeq" 
        (fun () -> ofSeq list)

let lookupInArrayAndVector n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    averageTime trials "  Array" 
        (fun () -> for i in 1..count do array.[r.Next n])

    averageTime trials "  PersistentVector" 
        (fun () -> for i in 1..count do nth (r.Next n) vector)


let replaceInArrayAndVector n count =
    sprintf "%d writes in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    averageTime trials "  Array" 
        (fun () -> for i in 1..count do array.[r.Next n] <- r.Next())

    averageTime trials "  PersistentVector" 
        (fun () -> for i in 1..count do update (r.Next n) (r.Next()) vector)

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
// [fsi:  Multiple PersistentVector.conj 4.2ms]
// [fsi:  PersistentVector.ofSeq 2.0ms]
// [fsi:Init with n = 100000]
// [fsi:  Array.ofSeq 0.4ms]
// [fsi:  Multiple PersistentVector.conj 43.0ms]
// [fsi:  PersistentVector.ofSeq 18.4ms]
// [fsi:Init with n = 1000000]
// [fsi:  Array.ofSeq 5.2ms]
// [fsi:  Multiple PersistentVector.conj 429.6ms]
// [fsi:  PersistentVector.ofSeq 251.2ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 0.6ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 0.6ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 1.8ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 3.2ms]
// [fsi:10000 writes in size n = 10000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 6.4ms]
// [fsi:10000 writes in size n = 100000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 8.2ms]
// [fsi:10000 writes in size n = 1000000]
// [fsi:  Array 0.4ms]
// [fsi:  PersistentVector 11.4ms]
// [fsi:10000 writes in size n = 10000000]
// [fsi:  Array 1.0ms]
// [fsi:  PersistentVector 24.2ms]
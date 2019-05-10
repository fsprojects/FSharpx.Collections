(*** hide ***)
#r "../../bin/FSharpx.Collections/netstandard2.0/FSharpx.Collections.dll"
open System

/// Stops the runtime for a given function
let stopTime f = 
    let sw = new System.Diagnostics.Stopwatch()
    sw.Start()
    let result = f()
    sw.Stop()
    result,float sw.ElapsedMilliseconds

/// Stops the average runtime for a given function and applies it the given count
let stopAverageTime count f = 
    System.GC.Collect() // force garbage collector before testing
    let sw = new System.Diagnostics.Stopwatch()
    sw.Start()
    for _ in 1..count do
        f() |> ignore

    sw.Stop()
    float sw.ElapsedMilliseconds / float count

let printInFsiTags s = printfn " [fsi:%s]" s

/// Stops the average runtime for a given function and applies it the given count
/// Afterwards it reports it with the given description
let averageTime count desc f =
    let time = stopAverageTime count f
    sprintf "%s %Ams" desc time |> printInFsiTags
(**
PersistentVector - Performance tests
====================================

Bulk operations on PersistentVector use an internal TransientVector in order to get much better performance. The following scripts shows this:
*)

open FSharpx.Collections

let trials = 5
let r = new System.Random()

let initArrayAndVectorFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]

    let initvector list = 
        let v = ref PersistentVector.empty
        for x in list do
            v := PersistentVector.conj x !v
        !v

    averageTime trials "  Array.ofSeq" 
        (fun () -> Array.ofSeq list)

    averageTime trials "  Multiple PersistentVector.conj" 
        (fun () -> initvector list)

    averageTime trials "  PersistentVector.ofSeq" 
        (fun () -> PersistentVector.ofSeq list)

let lookupInArrayAndVector n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = PersistentVector.ofSeq list

    averageTime trials "  Array" 
        (fun () -> for i in 1..count do array.[r.Next n])

    averageTime trials "  PersistentVector" 
        (fun () -> for i in 1..count do PersistentVector.nth (r.Next n) vector)


let replaceInArrayAndVector n count =
    sprintf "%d writes in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = PersistentVector.ofSeq list

    averageTime trials "  Array" 
        (fun () -> for i in 1..count do array.[r.Next n] <- r.Next())

    averageTime trials "  PersistentVector" 
        (fun () -> for i in 1..count do PersistentVector.update (r.Next n) (r.Next()) vector)

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
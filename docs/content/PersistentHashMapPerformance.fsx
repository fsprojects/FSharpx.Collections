(*** hide ***)
#r "../../bin/FSharpx.Collections/netstandard2.0/FSharpx.Collections.dll"
//#r "../lib/System.Runtime.dll"
#r "../../packages/System.Collections.Immutable.1.5.0/lib/netstandard2.0/System.Collections.Immutable.dll"
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
PersistentHashMap - Performance tests
=====================================

Bulk operations on HashMaps use an internal TransientHashMap in order to get much better performance. The following scripts shows this:
*)

open FSharpx.Collections

let trials = 5
let r = new System.Random()

let initFSharpMapAndPersistentMapFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    
    let list = List.sortBy snd [for i in 1..n -> i,r.Next().ToString()]

    averageTime trials "  FSharp.Map.ofSeq" 
        (fun () -> Map.ofSeq list)

    let initPersistentHashMap list = 
        let m = ref PersistentHashMap.empty
        for (key,value) in list do
            m := PersistentHashMap.add key value !m
        !m

    averageTime trials "  Multiple PersistentHashMap.add" 
        (fun () -> initPersistentHashMap list)

    averageTime trials "  PersistentHashMap.ofSeq" 
        (fun () -> PersistentHashMap.ofSeq list)

    let initImmutableDictionary list = 
        let d = ref (System.Collections.Immutable.ImmutableDictionary<int,string>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    averageTime trials "  Multiple ImmutableDictionay.add" 
        (fun () -> initImmutableDictionary list)

    let initImmutableDictionary list = 
        let d = ref (System.Collections.Immutable.ImmutableSortedDictionary<int,string>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    averageTime trials "  Multiple ImmutableSortedDictionay.add" 
        (fun () -> initImmutableDictionary list)

let lookupInFSharpMapAndPersistentMap n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 0..n-1 -> i.ToString(),r.Next()]
    let fsharpMap = Map.ofSeq list
    let map = PersistentHashMap.ofSeq list

    averageTime trials "  FSharp.Map" 
        (fun () -> for i in 1..count do fsharpMap.[r.Next(n).ToString()])

    averageTime trials "  PersistentHashMap" 
        (fun () -> for i in 1..count do map.[r.Next(n).ToString()])

    let dict =
        let d = ref (System.Collections.Immutable.ImmutableDictionary<string,int>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    averageTime trials "  ImmutableDictionay" 
        (fun () -> for i in 1..count do dict.[r.Next(n).ToString()])

    let dict' =
        let d = ref (System.Collections.Immutable.ImmutableSortedDictionary<string,int>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    averageTime trials "  ImmutableSortedDictionay" 
        (fun () -> for i in 1..count do dict'.[r.Next(n).ToString()])

initFSharpMapAndPersistentMapFromList 10000
initFSharpMapAndPersistentMapFromList 100000
initFSharpMapAndPersistentMapFromList 1000000

lookupInFSharpMapAndPersistentMap 10000 10000
lookupInFSharpMapAndPersistentMap 100000 10000
lookupInFSharpMapAndPersistentMap 1000000 10000
lookupInFSharpMapAndPersistentMap 10000000 10000

// [fsi:Init with n = 10000]
// [fsi:  FSharp.Map.ofSeq 16.0ms]
// [fsi:  Multiple PersistentHashMap.add 43.6ms]
// [fsi:  PersistentHashMap.ofSeq 25.4ms]
// [fsi:  Multiple ImmutableDictionay.add 29.6ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 15.2ms]
// [fsi:Init with n = 100000]
// [fsi:  FSharp.Map.ofSeq 233.4ms]
// [fsi:  Multiple PersistentHashMap.add 293.2ms]
// [fsi:  PersistentHashMap.ofSeq 217.2ms]
// [fsi:  Multiple ImmutableDictionay.add 375.0ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 222.0ms]
// [fsi:Init with n = 1000000]
// [fsi:  FSharp.Map.ofSeq 5323.8ms]
// [fsi:  Multiple PersistentHashMap.add 5562.4ms]
// [fsi:  PersistentHashMap.ofSeq 4489.8ms]
// [fsi:  Multiple ImmutableDictionay.add 7061.4ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 4685.0ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  FSharp.Map 5.2ms]
// [fsi:  PersistentHashMap 9.6ms]
// [fsi:  ImmutableDictionay 4.2ms]
// [fsi:  ImmutableSortedDictionay 15.0ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  FSharp.Map 7.0ms]
// [fsi:  PersistentHashMap 10.8ms]
// [fsi:  ImmutableDictionay 5.4ms]
// [fsi:  ImmutableSortedDictionay 20.2ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  FSharp.Map 12.2ms]
// [fsi:  PersistentHashMap 12.4ms]
// [fsi:  ImmutableDictionay 8.2ms]
// [fsi:  ImmutableSortedDictionay 29.6ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  FSharp.Map 19.2ms]
// [fsi:  PersistentHashMap 15.4ms]
// [fsi:  ImmutableDictionay 13.8ms]
// [fsi:  ImmutableSortedDictionay 42.0ms]
(**
---
category: Collections
categoryindex: 1
index: 2
---
*)

(*** hide ***)
#r "../src/FSharpx.Collections/bin/Debug/netstandard2.0/FSharpx.Collections.dll"

open System

(**
# PersistentHashMap

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). count is O(1).
More details can be found in the [API docs](reference/fsharpx-collections-persistenthashmap-2.html).
*)

open FSharpx.Collections

// Create a new HashMap and add some items to it
let m =
    PersistentHashMap.empty
    |> PersistentHashMap.add 42 "hello"
    |> PersistentHashMap.add 99 "world"
(*** include-value: m ***)

// lookup some items
PersistentHashMap.find 42 m
(*** include-it ***)

PersistentHashMap.find 99 m
(*** include-it ***)

m.[99]
(*** include-it ***)

// Check no. of elements in the HashMap
PersistentHashMap.length m
(*** include-it ***)


(**
PersistentHashMaps are immutable and therefor allow to create new version without destruction of the old ones.
*)

let m' = m |> PersistentHashMap.add 104 "!" |> PersistentHashMap.add 42 "hi" // replace existing value

PersistentHashMap.find 42 m'
(*** include-it ***)

PersistentHashMap.find 104 m'
(*** include-it ***)

PersistentHashMap.find 42 m
(*** include-it ***)

PersistentHashMap.length m
(*** include-it ***)

PersistentHashMap.length m'
(*** include-it ***)

// remove an element
let m'' = m' |> PersistentHashMap.remove 104

PersistentHashMap.length m''
(*** include-it ***)

(** 
There a couple of interesting operations on HashMaps:
*)

// Convert a sequence of key value pairs to a HashMap
let stringBasedMap =
    PersistentHashMap.ofSeq [ ("a", 1); ("b", 2); ("c", 3); ("d", 4); ("e", 5) ]
(*** include-value: stringBasedMap ***)


// Square all values in a HashMap
let stringBasedMap' = PersistentHashMap.map (fun x -> x * x) stringBasedMap
stringBasedMap'.["d"]
(*** include-it ***)


(**
## Using from C#

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). Count is O(1).

    [lang=csharp,file=csharp/PersistentHashMap.cs,key=create-hashmap]

PersistentHashMaps are immutable and therefor allow to create new version without destruction of the old ones.

    [lang=csharp,file=csharp/PersistentHashMap.cs,key=modify-hashmap]

*)


(*** hide ***)
#r "../../packages/System.Collections.Immutable.1.5.0/lib/netstandard2.0/System.Collections.Immutable.dll"
open System

/// Stops the runtime for a given function
let stopTime f =
    let sw = Diagnostics.Stopwatch()
    sw.Start()
    let result = f()
    sw.Stop()
    result, float sw.ElapsedMilliseconds

/// Stops the average runtime for a given function and applies it the given count
let stopAverageTime count f =
    System.GC.Collect() // force garbage collector before testing
    let sw = Diagnostics.Stopwatch()
    sw.Start()

    for _ in 1..count do
        f() |> ignore

    sw.Stop()
    float sw.ElapsedMilliseconds / float count

let printInFsiTags s =
    printfn " [fsi:%s]" s

/// Stops the average runtime for a given function and applies it the given count
/// Afterwards it reports it with the given description
let averageTime count desc f =
    let time = stopAverageTime count f
    sprintf "%s %Ams" desc time |> printInFsiTags
(**
## Performance Tests

Bulk operations on HashMaps use an internal TransientHashMap in order to get much better performance. The following scripts shows this:
*)

open FSharpx.Collections

let trials = 5
let r = new System.Random()

let initFSharpMapAndPersistentMapFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags

    let list = List.sortBy snd [ for i in 1..n -> i, r.Next().ToString() ]

    averageTime trials "  FSharp.Map.ofSeq" (fun () -> Map.ofSeq list)

    let initPersistentHashMap list =
        let m = ref PersistentHashMap.empty

        for (key, value) in list do
            m := PersistentHashMap.add key value !m

        !m

    averageTime trials "  Multiple PersistentHashMap.add" (fun () -> initPersistentHashMap list)

    averageTime trials "  PersistentHashMap.ofSeq" (fun () -> PersistentHashMap.ofSeq list)

    let initImmutableDictionary list =
        let d = ref(System.Collections.Immutable.ImmutableDictionary<int, string>.Empty)

        for (key, value) in list do
            d := (!d).Add(key, value)

        !d

    averageTime trials "  Multiple ImmutableDictionay.add" (fun () -> initImmutableDictionary list)

    let initImmutableDictionary list =
        let d =
            ref(System.Collections.Immutable.ImmutableSortedDictionary<int, string>.Empty)

        for (key, value) in list do
            d := (!d).Add(key, value)

        !d

    averageTime trials "  Multiple ImmutableSortedDictionay.add" (fun () -> initImmutableDictionary list)

let lookupInFSharpMapAndPersistentMap n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [ for i in 0 .. n - 1 -> i.ToString(), r.Next() ]
    let fsharpMap = Map.ofSeq list
    let map = PersistentHashMap.ofSeq list

    averageTime trials "  FSharp.Map" (fun () ->
        for i in 1..count do
            fsharpMap.[r.Next(n).ToString()])

    averageTime trials "  PersistentHashMap" (fun () ->
        for i in 1..count do
            map.[r.Next(n).ToString()])

    let dict =
        let d = ref(System.Collections.Immutable.ImmutableDictionary<string, int>.Empty)

        for (key, value) in list do
            d := (!d).Add(key, value)

        !d

    averageTime trials "  ImmutableDictionay" (fun () ->
        for i in 1..count do
            dict.[r.Next(n).ToString()])

    let dict' =
        let d =
            ref(System.Collections.Immutable.ImmutableSortedDictionary<string, int>.Empty)

        for (key, value) in list do
            d := (!d).Add(key, value)

        !d

    averageTime trials "  ImmutableSortedDictionay" (fun () ->
        for i in 1..count do
            dict'.[r.Next(n).ToString()])

initFSharpMapAndPersistentMapFromList 10000
initFSharpMapAndPersistentMapFromList 100000
initFSharpMapAndPersistentMapFromList 1000000
(*** include-output ***)


lookupInFSharpMapAndPersistentMap 10000 10000
lookupInFSharpMapAndPersistentMap 100000 10000
lookupInFSharpMapAndPersistentMap 1000000 10000
(*** include-output ***)

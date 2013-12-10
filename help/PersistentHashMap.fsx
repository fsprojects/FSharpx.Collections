(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentHashMap
=================

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). count is O(1).
*)

open FSharpx.Collections.PersistentHashMap

// Create a new map and add some items to it
let m = 
    empty 
    |> add 42 "hello"
    |> add 99 "world"
// [fsi:val m : FSharpx.Collections.PersistentHashMap<int,string>]

// lookup some items and print them to the console
find 42 m
// [fsi:val it : string = "hello"]
find 99 m
// [fsi:val it : string = "world"]
m.[99]
// [fsi:val it : string = "world"]

(**
PersistentHashMaps are immutable and therefor allow to create new version without destruction of the old ones.
*)

let m' = 
    m 
    |> add 104 "!" 
    |> add 42 "hi" // replace existing value

find 42 m'
// [fsi:val it : string = "hi"]
find 104 m'
// [fsi:val it : string = "!"]

find 42 m
// [fsi:val it : string = "hello"]

(** There a couple of interesting operations on HashMaps:
*)

// Convert a sequence of key value pairs to a HashMap
let stringBasedMap = ofSeq [("a",1); ("b",2); ("c",3); ("d",4); ("e",5)]
// [fsi:val stringBaseMap : FSharpx.Collections.PersistentHashMap<string,int>]

// Square all values in a HashMap
let stringBasedMap' = map (fun x -> x * x) stringBasedMap
stringBasedMap'.["d"]
// [fsi:val it : int = 16]

(**

Performance tests
-----------------

Bulk operations on HashMaps use an internal TransientHashMap in order to get much better performance. The following scripts shows this:
*)

let trials = 5
let r = new System.Random()

open FSharpx.Collections.TimeMeasurement

let initFSharpMapAndPersistentMapFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    let list = [for i in 1..n -> r.Next(),r.Next().ToString()]

    let initpersistentmap list = 
        let m = ref empty
        for (key,value) in list do
            m := add key value !m
        !m

    compareThreeRuntimes trials
        "  FSharp.Map.ofSeq" (fun () -> Map.ofSeq list)
        "  Multiple PersistentHashMap.add" (fun () -> initpersistentmap list)
        "  PersistentHashMap.ofSeq" (fun () -> ofSeq list)

initFSharpMapAndPersistentMapFromList 10000
initFSharpMapAndPersistentMapFromList 100000
initFSharpMapAndPersistentMapFromList 1000000

// [fsi:Init with n = 10000]
// [fsi:  FSharp.Map.ofSeq 25.2ms]
// [fsi:  Multiple PersistentHashMap.add 22.4ms]
// [fsi:  PersistentHashMap.ofSeq 12.2ms]
// [fsi:Init with n = 100000]
// [fsi:  FSharp.Map.ofSeq 260.6ms]
// [fsi:  Multiple PersistentHashMap.add 309.8ms]
// [fsi:  PersistentHashMap.ofSeq 214.0ms]
// [fsi:Init with n = 1000000]
// [fsi:  FSharp.Map.ofSeq 4955.6ms]
// [fsi:  Multiple PersistentHashMap.add 5770.4ms]
// [fsi:  PersistentHashMap.ofSeq 3867.6ms]
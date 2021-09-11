(**
---
category: Using from F#
categoryindex: 1
index: 2
---
*)

(*** hide ***)
#r "../../bin/FSharpx.Collections/netstandard2.0/FSharpx.Collections.dll"
open System


(**
PersistentHashMap
=================

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). count is O(1).
More details can be found in the [API docs](apidocs/fsharpx-collections-persistenthashmap-2.html).
*)

open FSharpx.Collections

// Create a new HashMap and add some items to it
let m = 
    PersistentHashMap.empty 
    |> PersistentHashMap.add 42 "hello"
    |> PersistentHashMap.add 99 "world"
// [fsi:val m : FSharpx.Collections.PersistentHashMap<int,string>]

// lookup some items
PersistentHashMap.find 42 m
// [fsi:val it : string = "hello"]
PersistentHashMap.find 99 m
// [fsi:val it : string = "world"]
m.[99]
// [fsi:val it : string = "world"]

// Check no. of elements in the HashMap
PersistentHashMap.length m
// [fsi:val it : int = 2]


(**
PersistentHashMaps are immutable and therefor allow to create new version without destruction of the old ones.
*)

let m' = 
    m 
    |> PersistentHashMap.add 104 "!" 
    |> PersistentHashMap.add 42 "hi" // replace existing value

PersistentHashMap.find 42 m'
// [fsi:val it : string = "hi"]
PersistentHashMap.find 104 m'
// [fsi:val it : string = "!"]

PersistentHashMap.find 42 m
// [fsi:val it : string = "hello"]
PersistentHashMap.length m
// [fsi:val it : int = 2]
PersistentHashMap.length m'
// [fsi:val it : int = 3]

// remove an element
let m'' = 
    m'
    |> PersistentHashMap.remove 104

PersistentHashMap.length m''
// [fsi:val it : int = 2]

(** There a couple of interesting operations on HashMaps:
*)

// Convert a sequence of key value pairs to a HashMap
let stringBasedMap = PersistentHashMap.ofSeq [("a",1); ("b",2); ("c",3); ("d",4); ("e",5)]
// [fsi:val stringBaseMap : FSharpx.Collections.PersistentHashMap<string,int>]

// Square all values in a HashMap
let stringBasedMap' = PersistentHashMap.map (fun x -> x * x) stringBasedMap
stringBasedMap'.["d"]
// [fsi:val it : int = 16]
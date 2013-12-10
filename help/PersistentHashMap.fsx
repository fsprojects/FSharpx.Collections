(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentHashMap
=============

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
    |> add 42 "hi" // replace existing

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

// Sequare all values in a map
let stringBasedMap' = map (fun x -> x * x) stringBasedMap
stringBasedMap'.["d"]
// [fsi:val it : int = 16]

(**
Bulk operations on HashMaps use an internal TransientHashMap in order to get much better performance.
*)


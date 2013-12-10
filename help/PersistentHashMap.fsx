(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentHashMap
=================

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). count is O(1).
More details can be found in the [API docs](apidocs/fsharpx-collections-persistenthashmap-2.html).
*)

open FSharpx.Collections.PersistentHashMap

// Create a new HashMap and add some items to it
let m = 
    empty 
    |> add 42 "hello"
    |> add 99 "world"
// [fsi:val m : FSharpx.Collections.PersistentHashMap<int,string>]

// lookup some items
find 42 m
// [fsi:val it : string = "hello"]
find 99 m
// [fsi:val it : string = "world"]
m.[99]
// [fsi:val it : string = "world"]

// Check no. of elements in the HashMap
length m
// [fsi:val it : int = 2]


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
length m
// [fsi:val it : int = 2]
length m'
// [fsi:val it : int = 3]

// remove an element
let m'' = 
    m'
    |> remove 104

length m''
// [fsi:val it : int = 2]

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

let lookupInFSharpMapAndPersistentMap n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 0..n-1 -> i.ToString(),r.Next()]
    let fsharpMap = Map.ofSeq list
    let map = ofSeq list

    compareTwoRuntimes trials
        "  FSharp.Map" (fun () -> for i in 1..count do fsharpMap.[r.Next(n).ToString()] |> ignore)
        "  PersistentHashMap" (fun () -> for i in 1..count do map.[r.Next(n).ToString()] |> ignore)

initFSharpMapAndPersistentMapFromList 10000
initFSharpMapAndPersistentMapFromList 100000
initFSharpMapAndPersistentMapFromList 1000000

lookupInFSharpMapAndPersistentMap 10000 10000
lookupInFSharpMapAndPersistentMap 100000 10000
lookupInFSharpMapAndPersistentMap 1000000 10000
lookupInFSharpMapAndPersistentMap 10000000 10000

// [fsi:Init with n = 10000]
// [fsi:  FSharp.Map.ofSeq 14.4ms]
// [fsi:  Multiple PersistentHashMap.add 20.2ms]
// [fsi:  PersistentHashMap.ofSeq 11.6ms]
// [fsi:Init with n = 100000]
// [fsi:  FSharp.Map.ofSeq 238.8ms]
// [fsi:  Multiple PersistentHashMap.add 351.2ms]
// [fsi:  PersistentHashMap.ofSeq 195.8ms]
// [fsi:Init with n = 1000000]
// [fsi:  FSharp.Map.ofSeq 3760.6ms]
// [fsi:  Multiple PersistentHashMap.add 5912.0ms]
// [fsi:  PersistentHashMap.ofSeq 3680.2ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  FSharp.Map 8.8ms]
// [fsi:  PersistentHashMap 8.4ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  FSharp.Map 11.0ms]
// [fsi:  PersistentHashMap 11.4ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  FSharp.Map 13.0ms]
// [fsi:  PersistentHashMap 12.4ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  FSharp.Map 22.8ms]
// [fsi:  PersistentHashMap 15.6ms]
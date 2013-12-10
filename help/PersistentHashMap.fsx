(*** hide ***)
#r "../build/FSharpx.Collections.dll"
#r "../lib/System.Runtime.dll"
#r "../packages/Microsoft.Bcl.Immutable.1.0.30/lib/portable-net45+win8+wp8/System.Collections.Immutable.dll"
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
    
    let list = List.sortBy snd [for i in 1..n -> i,r.Next().ToString()]

    let initPersistentHashMap list = 
        let m = ref empty
        for (key,value) in list do
            m := add key value !m
        !m

    let initImmutableDictionary list = 
        let d = ref (System.Collections.Immutable.ImmutableSortedDictionary<int,string>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    stopAndReportAvarageTime trials "  FSharp.Map.ofSeq" (fun () -> Map.ofSeq list) |> ignore
    stopAndReportAvarageTime trials "  Multiple PersistentHashMap.add" (fun () -> initPersistentHashMap list) |> ignore
    stopAndReportAvarageTime trials "  PersistentHashMap.ofSeq" (fun () -> ofSeq list) |> ignore
    stopAndReportAvarageTime trials "  Multiple ImmutableSortedDictionay.add" (fun () -> initImmutableDictionary list) |> ignore

let lookupInFSharpMapAndPersistentMap n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 0..n-1 -> i.ToString(),r.Next()]
    let fsharpMap = Map.ofSeq list
    let map = ofSeq list
    let dict =
        let d = ref (System.Collections.Immutable.ImmutableSortedDictionary<string,int>.Empty)
        for (key,value) in list do
            d := (!d).Add(key,value)
        !d

    stopAndReportAvarageTime trials "  FSharp.Map" (fun () -> for i in 1..count do fsharpMap.[r.Next(n).ToString()] |> ignore) |> ignore
    stopAndReportAvarageTime trials "  PersistentHashMap" (fun () -> for i in 1..count do map.[r.Next(n).ToString()] |> ignore) |> ignore
    stopAndReportAvarageTime trials "  ImmutableSortedDictionay" (fun () -> for i in 1..count do dict.[r.Next(n).ToString()] |> ignore) |> ignore

initFSharpMapAndPersistentMapFromList 10000
initFSharpMapAndPersistentMapFromList 100000
initFSharpMapAndPersistentMapFromList 1000000

lookupInFSharpMapAndPersistentMap 10000 10000
lookupInFSharpMapAndPersistentMap 100000 10000
lookupInFSharpMapAndPersistentMap 1000000 10000
lookupInFSharpMapAndPersistentMap 10000000 10000

// [fsi:Init with n = 10000]
// [fsi:  FSharp.Map.ofSeq 22.4ms]
// [fsi:  Multiple PersistentHashMap.add 19.4ms]
// [fsi:  PersistentHashMap.ofSeq 12.6ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 15.4ms]
// [fsi:Init with n = 100000]
// [fsi:  FSharp.Map.ofSeq 259.0ms]
// [fsi:  Multiple PersistentHashMap.add 276.8ms]
// [fsi:  PersistentHashMap.ofSeq 202.0ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 229.4ms]
// [fsi:Init with n = 1000000]
// [fsi:  FSharp.Map.ofSeq 5126.6ms]
// [fsi:  Multiple PersistentHashMap.add 5881.2ms]
// [fsi:  PersistentHashMap.ofSeq 4237.4ms]
// [fsi:  Multiple ImmutableSortedDictionay.add 4488.8ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  FSharp.Map 8.6ms]
// [fsi:  PersistentHashMap 11.0ms]
// [fsi:  ImmutableSortedDictionay 16.8ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  FSharp.Map 7.0ms]
// [fsi:  PersistentHashMap 11.2ms]
// [fsi:  ImmutableSortedDictionay 22.4ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  FSharp.Map 12.8ms]
// [fsi:  PersistentHashMap 13.4ms]
// [fsi:  ImmutableSortedDictionay 30.8ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  FSharp.Map 22.8ms]
// [fsi:  PersistentHashMap 16.4ms]
// [fsi:  ImmutableSortedDictionay: System.OutOfMemoryException]
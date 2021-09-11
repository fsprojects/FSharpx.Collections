(**
---
category: Using from F#
categoryindex: 1
index: 1
---
*)

(*** hide ***)
#r "../../bin/FSharpx.Collections/netstandard2.0/FSharpx.Collections.dll"
open System


(**
PersistentVector
================

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector.
More details can be found in the [API docs](apidocs/fsharpx-collections-persistentvector-1.html).
*)

open FSharpx.Collections

// Create a new PersistentVector and add some items to it
let v = 
    PersistentVector.empty 
    |> PersistentVector.conj "hello"
    |> PersistentVector.conj "world"
    |> PersistentVector.conj "!"

// [fsi:val v : FSharpx.Collections.PersistentVector<string>]

// lookup some items
PersistentVector.nth 0 v
// [fsi:val it : string = "hello"]
PersistentVector.nth 1 v
// [fsi:val it : string = "world"]
v.[2]
// [fsi:val it : string = "!"]

// Check no. of elements in the PersistentVector
PersistentVector.length v
// [fsi:val it : int = 3]

(**
PersistentVectors are immutable and therefor allow to create new version without destruction of the old ones.
*)

let v' = 
    v
    |> PersistentVector.conj "!!!" 
    |> PersistentVector.update 0 "hi" // replace existing value

PersistentVector.nth 0 v'
// [fsi:val it : string = "hi"]
PersistentVector.nth 3 v'
// [fsi:val it : string = "!!!"]

PersistentVector.nth 0 v
// [fsi:val it : string = "hello"]
PersistentVector.length v
// [fsi:val it : int = 3]
PersistentVector.length v'
// [fsi:val it : int = 4]

// remove the last element from a PersistentVector
let v'' = PersistentVector.initial v'

PersistentVector.length v''
// [fsi:val it : int = 3]


(** There a couple of interesting operations on PersistentVectors:
*)

// Convert a sequence of values to a PersistentVectors
let intVector = PersistentVector.ofSeq [1..10]
// [fsi:val intVector : FSharpx.Collections.PersistentVector<int>]

// Square all values in a PersistentVector
let intVector' = PersistentVector.map (fun x -> x * x) intVector
intVector'.[3]
// [fsi:val it : int = 256]
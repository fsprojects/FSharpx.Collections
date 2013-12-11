(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentVector
================

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector.
More details can be found in the [API docs](apidocs/fsharpx-collections-persistentvector-1.html).
*)

open FSharpx.Collections.PersistentVector

// Create a new PersistentVector and add some items to it
let v = 
    empty 
    |> conj "hello"
    |> conj "world"
    |> conj "!"

// [fsi:val v : FSharpx.Collections.PersistentVector<string>]

// lookup some items
nth 0 v
// [fsi:val it : string = "hello"]
nth 1 v
// [fsi:val it : string = "world"]
v.[2]
// [fsi:val it : string = "!"]

// Check no. of elements in the PersistentVector
length v
// [fsi:val it : int = 3]

(**
PersistentVectors are immutable and therefor allow to create new version without destruction of the old ones.
*)

let v' = 
    v
    |> conj "!!!" 
    |> update 0 "hi" // replace existing value

nth 0 v'
// [fsi:val it : string = "hi"]
nth 3 v'
// [fsi:val it : string = "!!!"]

nth 0 v
// [fsi:val it : string = "hello"]
length v
// [fsi:val it : int = 3]
length v'
// [fsi:val it : int = 4]

// remove the last element from a PersistentVector
let v'' = initial v'

length v''
// [fsi:val it : int = 3]


(** There a couple of interesting operations on PersistentVectors:
*)

// Convert a sequence of values to a PersistentVectors
let intVector = ofSeq [1..10]
// [fsi:val intVector : FSharpx.Collections.PersistentVector<int>]

// Square all values in a PersistentVector
let intVector' = map (fun x -> x * x) intVector
intVector'.[3]
// [fsi:val it : int = 256]
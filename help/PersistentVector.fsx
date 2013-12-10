(*** hide ***)
#r "../build/FSharpx.Collections.dll"
open System


(**
PersistentVector
=============

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector. 
*)

open FSharpx.Collections.PersistentVector

// Create a new PersistentVector and add some items to it
let v = 
    empty 
    |> conj "hello"
    |> conj "world"
    |> conj "!"

// [fsi:val v : FSharpx.Collections.PersistentVector<string>]

// lookup some items and print them to the console
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
PersistentVectorss are immutable and therefor allow to create new version without destruction of the old ones.
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

(**

Performance tests
-----------------

Bulk operations on PersistentVector use an internal TransientVector in order to get much better performance. The following scripts shows this:
*)

let trials = 5
let r = new System.Random()

open FSharpx.Collections.TimeMeasurement

let initArrayAndVectorFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]

    let initvector list = 
        let v = ref empty
        for x in list do
            v := conj x !v
        !v

    compareThreeRuntimes trials
        "  Array.ofSeq" (fun () -> Array.ofSeq list)
        "  Multiple PersistentVector.conj" (fun () -> initvector list)
        "  PersistentVector.ofSeq" (fun () -> ofSeq list)

let lookupInArrayAndVector n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    compareTwoRuntimes trials
        "  Array" (fun () -> for i in 1..count do array.[r.Next n] |> ignore)
        "  PersistentVector" (fun () -> for i in 1..count do nth (r.Next n) vector |> ignore)


let replaceInArrayAndVector n count =
    sprintf "%d writes in size n = %d" count n |> printInFsiTags
    let list = [for i in 1..n -> r.Next()]
    let array = Array.ofSeq list
    let vector = ofSeq list

    compareTwoRuntimes trials
        "  Array" (fun () -> for i in 1..count do array.[r.Next n] <- r.Next())
        "  PersistentVector" (fun () -> for i in 1..count do update (r.Next n) (r.Next()) vector |> ignore)

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
// [fsi:  Multiple PersistentVector.conj 3.0ms]
// [fsi:  PersistentVector.ofSeq 3.0ms]
// [fsi:Init with n = 100000]
// [fsi:  Array.ofSeq 0.4ms]
// [fsi:  Multiple PersistentVector.conj 38.8ms]
// [fsi:  PersistentVector.ofSeq 17.6ms]
// [fsi:Init with n = 1000000]
// [fsi:  Array.ofSeq 5.4ms]
// [fsi:  Multiple PersistentVector.conj 420.6ms]
// [fsi:  PersistentVector.ofSeq 250.6ms]
// [fsi:10000 Lookups in size n = 10000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 0.4ms]
// [fsi:10000 Lookups in size n = 100000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 0.6ms]
// [fsi:10000 Lookups in size n = 1000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 2.0ms]
// [fsi:10000 Lookups in size n = 10000000]
// [fsi:  Array 0.0ms]
// [fsi:  PersistentVector 3.4ms]
// [fsi:10000 writes in size n = 10000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 11.4ms]
// [fsi:10000 writes in size n = 100000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 11.2ms]
// [fsi:10000 writes in size n = 1000000]
// [fsi:  Array 0.2ms]
// [fsi:  PersistentVector 15.0ms]
// [fsi:10000 writes in size n = 10000000]
// [fsi:  Array 1.0ms]
// [fsi:  PersistentVector 33.8ms]
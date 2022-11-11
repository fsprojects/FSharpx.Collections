(**
---
category: Collections
categoryindex: 1
index: 1
---
*)

(*** hide ***)
#r "../src/FSharpx.Collections/bin/Debug/netstandard2.0/FSharpx.Collections.dll"

open System

(**
# PersistentVector

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector.
More details can be found in the [API docs](reference/fsharpx-collections-persistentvector-1.html).
*)

open FSharpx.Collections

// Create a new PersistentVector and add some items to it
let v =
    PersistentVector.empty
    |> PersistentVector.conj "hello"
    |> PersistentVector.conj "world"
    |> PersistentVector.conj "!"
(*** include-value: v ***)

// lookup some items
PersistentVector.nth 0 v
(*** include-it ***)

PersistentVector.nth 1 v
(*** include-it ***)

v.[2]
(*** include-it ***)

// Check no. of elements in the PersistentVector
PersistentVector.length v
(*** include-it ***)

(**
PersistentVectors are immutable and therefor allow to create new version without destruction of the old ones.
*)

let v' = v |> PersistentVector.conj "!!!" |> PersistentVector.update 0 "hi" // replace existing value

PersistentVector.nth 0 v'
(*** include-it ***)

PersistentVector.nth 3 v'
(*** include-it ***)

PersistentVector.nth 0 v
(*** include-it ***)

PersistentVector.length v
(*** include-it ***)

PersistentVector.length v'
(*** include-it ***)

// remove the last element from a PersistentVector
let v'' = PersistentVector.initial v'

PersistentVector.length v''
(*** include-it ***)


(** 
There a couple of interesting operations on PersistentVectors:
*)

// Convert a sequence of values to a PersistentVectors
let intVector = PersistentVector.ofSeq [ 1..10 ]
(*** include-value: intVector ***)

// Square all values in a PersistentVector
let intVector' = PersistentVector.map (fun x -> x * x) intVector
intVector'.[3]
(*** include-it ***)

(**
## Using from C#

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. Count is O(1). Conj puts the item at the end of the vector.

    [lang=csharp,file=csharp/PersistentVector.cs,key=create-vector]

PersistentVectors are immutable and therefor allow to create new version without destruction of the old ones.

    [lang=csharp,file=csharp/PersistentVector.cs,key=modify-vector]

*)

(*** hide ***)

/// Stops the runtime for a given function
let stopTime f =
    let sw = new System.Diagnostics.Stopwatch()
    sw.Start()
    let result = f()
    sw.Stop()
    result, float sw.ElapsedMilliseconds

/// Stops the average runtime for a given function and applies it the given count
let stopAverageTime count f =
    System.GC.Collect() // force garbage collector before testing
    let sw = new System.Diagnostics.Stopwatch()
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

Bulk operations on PersistentVector use an internal TransientVector in order to get much better performance. The following scripts shows this:
*)

open FSharpx.Collections

let trials = 5
let r = new System.Random()

let initArrayAndVectorFromList n =
    sprintf "Init with n = %d" n |> printInFsiTags
    let list = [ for i in 1..n -> r.Next() ]

    let initvector list =
        let v = ref PersistentVector.empty

        for x in list do
            v := PersistentVector.conj x !v

        !v

    averageTime trials "  Array.ofSeq" (fun () -> Array.ofSeq list)

    averageTime trials "  Multiple PersistentVector.conj" (fun () -> initvector list)

    averageTime trials "  PersistentVector.ofSeq" (fun () -> PersistentVector.ofSeq list)

let lookupInArrayAndVector n count =
    sprintf "%d Lookups in size n = %d" count n |> printInFsiTags
    let list = [ for i in 1..n -> r.Next() ]
    let array = Array.ofSeq list
    let vector = PersistentVector.ofSeq list

    averageTime trials "  Array" (fun () ->
        for i in 1..count do
            array.[r.Next n])

    averageTime trials "  PersistentVector" (fun () ->
        for i in 1..count do
            PersistentVector.nth (r.Next n) vector)


let replaceInArrayAndVector n count =
    sprintf "%d writes in size n = %d" count n |> printInFsiTags
    let list = [ for i in 1..n -> r.Next() ]
    let array = Array.ofSeq list
    let vector = PersistentVector.ofSeq list

    averageTime trials "  Array" (fun () ->
        for i in 1..count do
            array.[r.Next n] <- r.Next())

    averageTime trials "  PersistentVector" (fun () ->
        for i in 1..count do
            PersistentVector.update (r.Next n) (r.Next()) vector)

initArrayAndVectorFromList 10000
initArrayAndVectorFromList 100000
initArrayAndVectorFromList 1000000
(*** include-output ***)

lookupInArrayAndVector 10000 10000
lookupInArrayAndVector 100000 10000
lookupInArrayAndVector 1000000 10000
(*** include-output ***)

replaceInArrayAndVector 10000 10000
replaceInArrayAndVector 100000 10000
replaceInArrayAndVector 1000000 10000
(*** include-output ***)

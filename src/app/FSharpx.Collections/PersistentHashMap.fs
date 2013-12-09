/// vector implementation ported from https://github.com/clojure/clojure/blob/master/src/jvm/clojure/lang/APersistentMap.java

namespace FSharpx.Collections

open System.Threading
open System.Collections.Generic

type Box(value:obj) =
    member val Value = value with get, set

type BitmapIndexedNode(thread,bitmap,array:obj[]) =
    let thread = thread
    let mask(hash, shift) = (hash >>> shift) &&& 0x01f
    let bitpos(hash, shift) = 1 <<< mask(hash, shift)

    let NumberOfSetBits(i) =
        let i = i - ((i >>> 1) &&& 0x55555555)
        let i = (i &&& 0x33333333) + ((i >>> 2) &&& 0x33333333)
        (((i + (i >>> 4)) &&& 0x0F0F0F0F) * 0x01010101) >>> 24

    let index(bit) = NumberOfSetBits(bitmap &&& (bit - 1))

    let cloneAndSet(array:obj[], i, a) =
        let clone = Array.copy array
        clone.[i] <- a
        clone

    let cloneAndSet2(array:obj[], i, a, j, b) =
        let clone = Array.copy array
        clone.[i] <- a
        clone.[j] <- b
        clone

    let createNode( shift, key1, val1, key2hash, key2, val2) =
        let key1hash = hash(key1)

        if key1hash = key2hash then
            failwith "collision not implemented"
            //return new HashCollisionNode(null, key1hash, 2, new Object[] {key1, val1, key2, val2});
        let addedLeaf = Box(null)
        let edit = ref null
        BitmapIndexedNode()
            .assoc( shift, key1hash, key1, val1, addedLeaf)  // edit
            .assoc( shift, key2hash, key2, val2, addedLeaf) // edit


    new() = BitmapIndexedNode(ref null,0,Array.create 0 null)
    with                
        member this.Find(shift, hash, key) =
            let bit = bitpos(hash, shift)
            if bitmap &&& bit = 0 then null else
            let idx = index(bit)
            let keyOrNull = array.[2*idx]
            let valOrNode = array.[2*idx+1]
            if keyOrNull = null then (valOrNode :?> BitmapIndexedNode).Find(shift + 5, hash, key) else
            if key = keyOrNull then
                valOrNode
            else 
                null
       

        member this.assoc(shift, hash, key, value, addedLeaf:Box) : BitmapIndexedNode = 
            let bit = bitpos(hash, shift)
            let idx = index(bit)
            if (bitmap &&& bit) <> 0 then
                let keyOrNull = array.[2*idx]
                let valOrNode = array.[2*idx+1]
                if keyOrNull = null then
                    let n = (valOrNode :?> BitmapIndexedNode).assoc(shift + 5, hash, key, value, addedLeaf)
                    if n = (valOrNode :?> BitmapIndexedNode) then this else BitmapIndexedNode(ref null, bitmap, cloneAndSet(array, 2*idx+1, n))
                else
                    if key = keyOrNull then
                        if value = valOrNode then this else BitmapIndexedNode(ref null, bitmap, cloneAndSet(array, 2*idx+1, value))
                    else
                        addedLeaf.Value <- addedLeaf :> obj
                        BitmapIndexedNode(ref null, bitmap, cloneAndSet2(array, 2*idx, null, 2*idx+1, createNode(shift + 5, keyOrNull, valOrNode, hash, key, value)))
            else
                    let n = NumberOfSetBits(bitmap)
                    if n >= 16 then
                        failwith "not implemented"
//                        let nodes = Array.create 32 Unchecked.defaultof<BitmapIndexedNode>
//                        let jdx = mask(hash, shift)
//                        nodes.[jdx] <- BitmapIndexedNode().assoc(shift + 5, hash, key, value, addedLeaf)
//                        let mutable j = 0
//                        for i in 0..31 do
//                            if ((bitmap >>> i) &&& 1) <> 0 then
//                                    if array.[j] = null then
//                                        nodes.[i] <- array[j+1]
//                                    else
//                                        nodes.[i] = BitmapIndexedNode().assoc(shift + 5, hash(array.[j]), array.[j], array.[j+1], addedLeaf)
//                                    j <- j + 2
//
//                        ArrayNode(null, n + 1, nodes);
                    else
                        let newArray = Array.create (2*(n+1)) null
                        System.Array.Copy(array, 0, newArray, 0, 2*idx)
                        newArray.[2*idx] <- key
                        addedLeaf.Value <- addedLeaf
                        newArray.[2*idx+1] <- value
                        System.Array.Copy(array, 2*idx, newArray, 2*(idx+1), 2*(n-idx))
                        BitmapIndexedNode(ref null, bitmap ||| bit, newArray)
        
type PersistentHashMap<[<EqualityConditionalOn>]'T when 'T : equality> (count,root:BitmapIndexedNode,hasNull, nullValue) =
    
    static member Empty() : PersistentHashMap<'T> = PersistentHashMap(0, Unchecked.defaultof<BitmapIndexedNode>, false, null)
    member this.Length : int = count

    member this.ContainsKey (key:'T) =
        if key = Unchecked.defaultof<'T> then hasNull else
        if root = Unchecked.defaultof<BitmapIndexedNode> then false else
        root.Find(0, hash(key), key) <> null

    member this.Add(key, value) =
        if key = Unchecked.defaultof<'T> then
            if hasNull && value = nullValue then this else
            let count = if hasNull then count else count + 1
            PersistentHashMap<'T>(count, root, true, value)
        else 
            let addedLeaf = Box(null)
            let newroot =
                (if root = Unchecked.defaultof<BitmapIndexedNode> then BitmapIndexedNode() else root)
                    .assoc(0, hash(key), key, value, addedLeaf)

            if newroot = root then this else
            let count = if addedLeaf.Value = null then count else count + 1
            PersistentHashMap(count, newroot, hasNull, nullValue)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PersistentHashMap = 
    ///O(1), returns an empty PersistentHashMap
    let empty<'T when 'T : equality> = PersistentHashMap.Empty() :> PersistentHashMap<'T>

    ///O(1), returns the count of the elements in the PersistentHashMap
    let length (map:PersistentHashMap<'T>) = map.Length

    ///O(log32n), returns if the key exists in the map
    let containsKey key (map:PersistentHashMap<'T>) = map.ContainsKey key

    ///O(log32n), adds an element to the map
    let add key value (map:PersistentHashMap<'T>) = map.Add(key,value)

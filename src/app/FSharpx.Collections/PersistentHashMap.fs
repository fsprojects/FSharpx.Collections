/// vector implementation ported from https://github.com/clojure/clojure/blob/master/src/jvm/clojure/lang/APersistentMap.java

namespace FSharpx.Collections

open System.Threading
open System.Collections.Generic

type Box(value:obj) =
    member val Value = value with get, set

type INode =
    abstract member assoc : int * int * obj  * obj * Box -> INode
    abstract member find : int * int * obj -> obj


type NodeHelpers =
    static member mask(hash, shift) = (hash >>> shift) &&& 0x01f
    static member bitpos(hash, shift) = 1 <<< NodeHelpers.mask(hash, shift)

    static member NumberOfSetBits(i) =
        let i = i - ((i >>> 1) &&& 0x55555555)
        let i = (i &&& 0x33333333) + ((i >>> 2) &&& 0x33333333)
        (((i + (i >>> 4)) &&& 0x0F0F0F0F) * 0x01010101) >>> 24

    static member index(bitmap,bit) = NodeHelpers.NumberOfSetBits(bitmap &&& (bit - 1))

    static member cloneAndSet(array:INode[], i, a) =
        let clone = Array.copy array
        clone.[i] <- a
        clone

    static member cloneAndSet(array:obj[], i, a) =
        let clone = Array.copy array
        clone.[i] <- a
        clone

    static member cloneAndSet2(array:obj[], i, a, j, b) =
        let clone = Array.copy array
        clone.[i] <- a
        clone.[j] <- b
        clone

    static member createNode( shift, key1, val1, key2hash, key2, val2) =
        let key1hash = hash(key1)

        if key1hash = key2hash then
            failwith "collision not implemented"
            //return new HashCollisionNode(null, key1hash, 2, new Object[] {key1, val1, key2, val2});
        let addedLeaf = Box(null)
        let edit = ref null
        (BitmapIndexedNode() :> INode)
            .assoc(shift, key1hash, key1, val1, addedLeaf) // edit
            .assoc(shift, key2hash, key2, val2, addedLeaf) // edit


and ArrayNode(thread,count,array:INode[]) =
    let x = 1
    with
        interface INode with
            member this.assoc(shift, hashKey, key, value, addedLeaf) : INode = 
                let idx = NodeHelpers.mask(hashKey, shift)
                let node = array.[idx]
                if node = Unchecked.defaultof<INode> then
                    ArrayNode(ref null, count + 1, NodeHelpers.cloneAndSet(array, idx, (BitmapIndexedNode() :> INode).assoc(shift + 5, hashKey, key, value, addedLeaf))) :> INode
                else
                    let n = node.assoc(shift + 5, hashKey, key, value, addedLeaf)
                    if n = node then this :> INode else
                    ArrayNode(ref null, count, NodeHelpers.cloneAndSet(array, idx, n))  :> INode


            member this.find(shift, hash, key) =
                let idx = NodeHelpers.mask(hash, shift)
                let node = array.[idx]
                if node = Unchecked.defaultof<INode> then null else
                node.find(shift + 5, hash, key)

and BitmapIndexedNode(thread,bitmap,array:obj[]) =
    let thread = thread


    new() = BitmapIndexedNode(ref null,0,Array.create 0 null)
    with                     
        interface INode with

            member this.find(shift, hash, key) =
                let bit = NodeHelpers.bitpos(hash, shift)
                if bitmap &&& bit = 0 then null else
                let idx = NodeHelpers.index(bitmap,bit)
                let keyOrNull = array.[2*idx]
                let valOrNode = array.[2*idx+1]
                if keyOrNull = null then (valOrNode :?> INode).find(shift + 5, hash, key) else
                if key = keyOrNull then
                    valOrNode
                else 
                    null

            member this.assoc(shift, hashKey, key, value, addedLeaf) : INode = 
                let bit = NodeHelpers.bitpos(hashKey, shift)
                let idx = NodeHelpers.index(bitmap,bit)
                if (bitmap &&& bit) <> 0 then
                    let keyOrNull = array.[2*idx]
                    let valOrNode = array.[2*idx+1]
                    if keyOrNull = null then
                        let n = (valOrNode :?> INode).assoc(shift + 5, hashKey, key, value, addedLeaf)
                        if n = (valOrNode :?> INode) then this :> INode else BitmapIndexedNode(ref null, bitmap, NodeHelpers.cloneAndSet(array, 2*idx+1, n)) :> INode
                    else
                        if key = keyOrNull then
                            if value = valOrNode then this  :> INode else BitmapIndexedNode(ref null, bitmap, NodeHelpers.cloneAndSet(array, 2*idx+1, value)) :> INode
                        else
                            addedLeaf.Value <- addedLeaf :> obj
                            BitmapIndexedNode(ref null, bitmap, NodeHelpers.cloneAndSet2(array, 2*idx, null, 2*idx+1, NodeHelpers.createNode(shift + 5, keyOrNull, valOrNode, hashKey, key, value))) :> INode
                else
                    let n = NodeHelpers.NumberOfSetBits(bitmap)
                    if n >= 16 then
                        let nodes = Array.create 32 Unchecked.defaultof<INode>
                        let jdx = NodeHelpers.mask(hashKey, shift)
                        nodes.[jdx] <- (BitmapIndexedNode() :> INode).assoc(shift + 5, hashKey, key, value, addedLeaf)
                        let mutable j = 0
                        for i in 0..31 do
                            if ((bitmap >>> i) &&& 1) <> 0 then
                                if array.[j] = null then
                                    nodes.[i] <- array.[j+1] :?> INode
                                else
                                    nodes.[i] <- (BitmapIndexedNode() :> INode).assoc(shift + 5, hash(array.[j]), array.[j], array.[j+1], addedLeaf)
                                j <- j + 2

                        ArrayNode(ref null, n + 1, nodes) :> INode
                    else
                        let newArray = Array.create (2*(n+1)) null
                        System.Array.Copy(array, 0, newArray, 0, 2*idx)
                        newArray.[2*idx] <- key
                        addedLeaf.Value <- addedLeaf
                        newArray.[2*idx+1] <- value
                        System.Array.Copy(array, 2*idx, newArray, 2*(idx+1), 2*(n-idx))
                        BitmapIndexedNode(ref null, bitmap ||| bit, newArray) :> INode


type PersistentHashMap<[<EqualityConditionalOn>]'T when 'T : equality> (count,root:INode,hasNull, nullValue) =
    
    static member Empty() : PersistentHashMap<'T> = PersistentHashMap(0, Unchecked.defaultof<INode>, false, null)
    member this.Length : int = count

    member this.ContainsKey (key:'T) =
        if key = Unchecked.defaultof<'T> then hasNull else
        if root = Unchecked.defaultof<INode> then false else
        root.find(0, hash(key), key) <> null

    member this.Add(key, value) =
        if key = Unchecked.defaultof<'T> then
            if hasNull && value = nullValue then this else
            let count = if hasNull then count else count + 1
            PersistentHashMap<'T>(count, root, true, value)
        else 
            let addedLeaf = Box(null)
            let newroot =
                (if root = Unchecked.defaultof<INode> then BitmapIndexedNode() :> INode else root)
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

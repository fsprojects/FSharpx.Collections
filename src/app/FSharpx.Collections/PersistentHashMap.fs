/// vector implementation ported from https://github.com/clojure/clojure/blob/master/src/jvm/clojure/lang/APersistentMap.java

namespace FSharpx.Collections

open System.Threading
open System.Collections.Generic

type BitmapIndexNode(thread,bitmap,array:obj[]) =
    let thread = thread
    let mask(hash, shift) = (hash >>> shift) &&& 0x01f
    let bitpos(hash, shift) = 1 <<< mask(hash, shift)

    let NumberOfSetBits(i) =
        let i = i - ((i >>> 1) &&& 0x55555555)
        let i = (i &&& 0x33333333) + ((i >>> 2) &&& 0x33333333)
        (((i + (i >>> 4)) &&& 0x0F0F0F0F) * 0x01010101) >>> 24

    let index(bit) = NumberOfSetBits(bitmap &&& (bit - 1))


    new() = BitmapIndexNode(ref null,0,Array.create 0 null)
    with
        member this.Find(shift, hash, key) =
            let bit = bitpos(hash, shift)
            if bitmap &&& bit = 0 then null else
            let idx = index(bit)
            let keyOrNull = array.[2*idx]
            let valOrNode = array.[2*idx+1]
            if keyOrNull = null then (valOrNode :?> BitmapIndexNode).Find(shift + 5, hash, key) else
            if key = keyOrNull then
                valOrNode
            else 
                null
        
type PersistentHashMap<[<EqualityConditionalOn>]'T when 'T : equality> (count,root:BitmapIndexNode,hasNull, nullValue) =
    
    static member Empty() : PersistentHashMap<'T> = PersistentHashMap(0, Unchecked.defaultof<BitmapIndexNode>, false, null)
    member this.Length : int = count

    member this.ContainsKey (key:'T) =
        if key = Unchecked.defaultof<'T> then hasNull else
        if root = Unchecked.defaultof<BitmapIndexNode> then false else
        root.Find(0, hash(key), key) <> null


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PersistentHashMap = 
    ///O(1), returns an empty PersistentHashMap
    let empty<'T when 'T : equality> = PersistentHashMap.Empty() :> PersistentHashMap<'T>

    ///O(1), returns the count of the elements in the PersistentHashMap
    let length (map:PersistentHashMap<'T>) = map.Length

    ///O(log32n), returns if the key exists in the map
    let containsKey key (map:PersistentHashMap<'T>) = map.ContainsKey key

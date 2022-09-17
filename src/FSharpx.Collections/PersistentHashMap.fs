// hash map implementation ported from https://github.com/clojure/clojure/blob/master/src/jvm/clojure/lang/APersistentMap.java
namespace FSharpx.Collections

#if !FABLE_COMPILER

open System.Threading

type internal Box(value: obj) =
    member val Value = value with get, set

type internal INode =
    abstract member assoc: int * int * obj * obj * Box -> INode
    abstract member assoc: Thread ref * int * int * obj * obj * Box -> INode
    abstract member find: int * int * obj -> obj
    abstract member tryFind: int * int * obj -> obj option
    abstract member without: int * int * obj -> INode
    abstract member without: Thread ref * int * int * obj * Box -> INode
    abstract member nodeSeq: unit -> (obj * obj) seq

module private BitCount =
    let bitCounts =
        let bitCounts = Array.create 65536 0
        let mutable position1 = -1
        let mutable position2 = -1

        for i in 1..65535 do
            if position1 = position2 then
                position1 <- 0
                position2 <- i

            bitCounts.[i] <- bitCounts.[position1] + 1
            position1 <- position1 + 1

        bitCounts

    let inline NumberOfSetBits value =
        bitCounts.[value &&& 65535] + bitCounts.[(value >>> 16) &&& 65535]

    let inline mask(hash, shift) =
        (hash >>> shift) &&& 0x01f

    let inline bitpos(hash, shift) =
        1 <<< mask(hash, shift)

    let inline index(bitmap, bit) =
        NumberOfSetBits(bitmap &&& (bit - 1))

module private NodeHelpers =
    let inline cloneAndSet(array: obj[], i, a) =
        let clone = Array.copy array
        clone.[i] <- a
        clone

    let inline cloneAndSetNodes(array: INode[], i, a) =
        let clone = Array.copy array
        clone.[i] <- a
        clone

    let inline cloneAndSet2(array, i, a, j, b) =
        let clone = Array.copy array
        clone.[i] <- a
        clone.[j] <- b
        clone

    let inline removePair(array: obj[], i) =
        let newArray = Array.create (array.Length - 2) null
        System.Array.Copy(array, 0, newArray, 0, 2 * i)
        System.Array.Copy(array, 2 * (i + 1), newArray, 2 * i, newArray.Length - 2 * i)
        newArray

    let inline createNodeSeq(array: obj[]) = seq {
        let mutable j = 0

        while j < array.Length do
            let isNode = array.[j + 1] :? INode

            if isNode then
                let node = array.[j + 1] :?> INode

                if node <> Unchecked.defaultof<INode> then
                    yield! node.nodeSeq()
            else if array.[j] <> null then
                yield array.[j], array.[j + 1]

            j <- j + 2
    }

open BitCount
open NodeHelpers

type private NodeCreation =
    static member createNode(thread, shift, key1, val1, key2hash, key2, val2) =
        let key1hash = hash(key1)

        if key1hash = key2hash then
            HashCollisionNode(ref null, key1hash, 2, [| key1; val1; key2; val2 |]) :> INode
        else

            let addedLeaf = Box(null)

            (BitmapIndexedNode() :> INode)
                .assoc(thread, shift, key1hash, key1, val1, addedLeaf)
                .assoc(thread, shift, key2hash, key2, val2, addedLeaf)

    static member createNode(shift, key1, val1, key2hash, key2, val2) =
        let key1hash = hash(key1)

        if key1hash = key2hash then
            HashCollisionNode(ref null, key1hash, 2, [| key1; val1; key2; val2 |]) :> INode
        else

            let addedLeaf = Box(null)

            (BitmapIndexedNode() :> INode)
                .assoc(shift, key1hash, key1, val1, addedLeaf)
                .assoc(shift, key2hash, key2, val2, addedLeaf)

and private HashCollisionNode(thread, hashCollisionKey, count', array': obj[]) =
    let thread = thread
    member val array = array' with get, set
    member val count = count' with get, set

    with
        member this.findIndex key =
            let mutable i = 0

            while (i < 2 * this.count) && (key <> this.array.[i]) do
                i <- i + 2

            if i < 2 * this.count then i else -1

        member this.ensureEditable(thread1, count1, array1) =
            if !thread1 = !thread then
                this.array <- array1
                this.count <- count1
                this
            else
                HashCollisionNode(thread1, hashCollisionKey, count1, array1)

        member this.ensureEditable(thread1) =
            if !thread1 = !thread then
                this
            else
                let newArray = Array.create (2 * (this.count + 1)) null // make room for next assoc
                System.Array.Copy(this.array, 0, newArray, 0, 2 * this.count)
                HashCollisionNode(thread1, hashCollisionKey, this.count, newArray)

        member this.editAndSet(thread1, i, a) =
            let editable = this.ensureEditable(thread1)
            editable.array.[i] <- a
            editable


        member this.editAndSet(thread1, i, a, j, b) =
            let editable = this.ensureEditable(thread1)
            editable.array.[i] <- a
            editable.array.[j] <- b
            editable

        interface INode with

            member this.assoc(shift, hashKey, key, value, addedLeaf) : INode =
                if hashKey = hashCollisionKey then
                    let idx = this.findIndex(key)

                    if idx <> -1 then
                        if this.array.[idx + 1] = value then
                            this :> INode
                        else
                            HashCollisionNode(ref null, hashKey, this.count, cloneAndSet(this.array, idx + 1, value)) :> INode
                    else
                        let newArray = Array.create (2 * (this.count + 1)) null
                        System.Array.Copy(this.array, 0, newArray, 0, 2 * this.count)
                        newArray.[2 * this.count] <- key
                        newArray.[2 * this.count + 1] <- value
                        addedLeaf.Value <- addedLeaf :> obj
                        HashCollisionNode(thread, hashKey, this.count + 1, newArray) :> INode
                else
                    (BitmapIndexedNode(ref null, bitpos(hashCollisionKey, shift), [| null; this |]) :> INode)
                        .assoc(shift, hashKey, key, value, addedLeaf)

            member this.assoc(thread1, shift, hashKey, key, value, addedLeaf) : INode =
                if hashCollisionKey = hashKey then
                    let idx = this.findIndex(key)

                    if idx <> -1 then
                        if this.array.[idx + 1] = value then
                            this :> INode
                        else
                            this.editAndSet(thread1, idx + 1, value) :> INode
                    else if this.array.Length > 2 * this.count then
                        addedLeaf.Value <- addedLeaf :> obj

                        let editable =
                            this.editAndSet(thread1, 2 * this.count, key, 2 * this.count + 1, value)

                        editable.count <- editable.count + 1
                        editable :> INode
                    else
                        let newArray = Array.create (this.array.Length + 2) null
                        System.Array.Copy(this.array, 0, newArray, 0, this.array.Length)
                        newArray.[this.array.Length] <- key
                        newArray.[this.array.Length + 1] <- value
                        addedLeaf.Value <- addedLeaf :> obj
                        this.ensureEditable(thread1, this.count + 1, newArray) :> INode
                else
                    // nest it in a bitmap node
                    (BitmapIndexedNode(thread1, bitpos(hashCollisionKey, shift), [| null; this; null; null |]) :> INode)
                        .assoc(thread1, shift, hashKey, key, value, addedLeaf)

            member this.find(shift, hash, key) =
                let idx = this.findIndex(key)

                if idx < 0 then null
                else if key = this.array.[idx] then this.array.[idx + 1]
                else null

            member this.tryFind(shift, hash, key) =
                let idx = this.findIndex(key)

                if idx < 0 then
                    None
                else if key = this.array.[idx] then
                    Some this.array.[idx + 1]
                else
                    None

            member this.without(shift, hashKey, key) =
                let idx = this.findIndex(key)

                if idx = -1 then
                    this :> INode
                else if this.count = 1 then
                    Unchecked.defaultof<INode>
                else
                    HashCollisionNode(ref null, hashKey, this.count - 1, removePair(this.array, idx / 2)) :> INode

            member this.nodeSeq() =
                createNodeSeq this.array

            member this.without(thread, shift, hashKey, key, removedLeaf) =
                let idx = this.findIndex(key)

                if idx = -1 then
                    this :> INode
                else
                    removedLeaf.Value <- removedLeaf :> obj

                    if this.count = 1 then
                        Unchecked.defaultof<INode>
                    else
                        let editable = this.ensureEditable(thread)
                        editable.array.[idx] <- editable.array.[2 * this.count - 2]
                        editable.array.[idx + 1] <- editable.array.[2 * this.count - 1]
                        editable.array.[2 * this.count - 2] <- null
                        editable.array.[2 * this.count - 1] <- null
                        editable.count <- editable.count - 1
                        editable :> INode

and private ArrayNode(thread, count', array': INode[]) =
    member val array = array' with get, set
    member val count = count' with get, set

    with

        member this.pack(thred, idx) =
            let newArray = Array.create (2 * (this.count - 1)) null
            let mutable j = 1
            let mutable bitmap = 0

            for i in 0 .. idx - 1 do
                if this.array.[i] <> Unchecked.defaultof<INode> then
                    newArray.[j] <- this.array.[i] :> obj
                    bitmap <- bitmap ||| (1 <<< i)
                    j <- j + 2

            for i in idx + 1 .. this.array.Length - 1 do
                if this.array.[i] <> Unchecked.defaultof<INode> then
                    newArray.[j] <- this.array.[i] :> obj
                    bitmap <- bitmap ||| (1 <<< i)
                    j <- j + 2

            BitmapIndexedNode(thread, bitmap, newArray)

        member this.ensureEditable(thread1) =
            if !thread1 = !thread then
                this
            else
                ArrayNode(thread1, this.count, Array.copy this.array)


        member this.editAndSet(thread1, i, n) =
            let editable = this.ensureEditable(thread1)
            editable.array.[i] <- n
            editable

        interface INode with

            member this.assoc(shift, hashKey, key, value, addedLeaf) : INode =
                let idx = mask(hashKey, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    ArrayNode(
                        ref null,
                        this.count + 1,
                        cloneAndSetNodes(this.array, idx, (BitmapIndexedNode() :> INode).assoc(shift + 5, hashKey, key, value, addedLeaf))
                    )
                    :> INode
                else
                    let n = node.assoc(shift + 5, hashKey, key, value, addedLeaf)

                    if n = node then
                        this :> INode
                    else
                        ArrayNode(ref null, this.count, cloneAndSetNodes(this.array, idx, n)) :> INode

            member this.assoc(thread1, shift, hashKey, key, value, addedLeaf) : INode =
                let idx = mask(hashKey, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    let editable =
                        this.editAndSet(
                            thread1,
                            idx,
                            (BitmapIndexedNode() :> INode)
                                .assoc(thread1, shift + 5, hashKey, key, value, addedLeaf)
                        )

                    editable.count <- editable.count + 1
                    editable :> INode
                else
                    let n = node.assoc(thread1, shift + 5, hashKey, key, value, addedLeaf)

                    if n = node then
                        this :> INode
                    else
                        this.editAndSet(thread1, idx, n) :> INode

            member this.find(shift, hash, key) =
                let idx = mask(hash, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    null
                else
                    node.find(shift + 5, hash, key)

            member this.tryFind(shift, hash, key) =
                let idx = mask(hash, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    None
                else
                    node.tryFind(shift + 5, hash, key)

            member this.without(shift, hashKey, key) =
                let idx = mask(hashKey, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    this :> INode
                else
                    let n = node.without(shift + 5, hashKey, key)

                    if n = node then
                        this :> INode
                    else if n = Unchecked.defaultof<INode> then
                        if this.count <= 8 then // shrink
                            this.pack(ref null, idx) :> INode
                        else
                            ArrayNode(ref null, this.count - 1, cloneAndSetNodes(this.array, idx, n)) :> INode
                    else
                        ArrayNode(ref null, this.count, cloneAndSetNodes(this.array, idx, n)) :> INode

            member this.without(thread1, shift, hashKey, key, removedLeaf) =
                let idx = mask(hashKey, shift)
                let node = this.array.[idx]

                if node = Unchecked.defaultof<INode> then
                    this :> INode
                else
                    let n = node.without(thread1, shift + 5, hashKey, key, removedLeaf)

                    if n = node then
                        this :> INode
                    else if n = Unchecked.defaultof<INode> then
                        if this.count <= 8 then // shrink
                            this.pack(thread1, idx) :> INode
                        else
                            let editable = this.editAndSet(thread1, idx, n)
                            editable.count <- editable.count - 1
                            editable :> INode
                    else
                        this.editAndSet(thread1, idx, n) :> INode

            member this.nodeSeq() = seq {
                let mutable j = 0

                while j < this.array.Length do
                    if this.array.[j] <> Unchecked.defaultof<INode> then
                        yield! this.array.[j].nodeSeq()

                    j <- j + 1
            }


and private BitmapIndexedNode(thread, bitmap', array': obj[]) =
    member val array = array' with get, set
    member val bitmap = bitmap' with get, set

    new() = BitmapIndexedNode(ref null, 0, Array.create 0 null)
    with
        member this.ensureEditable(thread1) =
            if !thread = !thread1 then
                this
            else
                let n = NumberOfSetBits(this.bitmap)
                let newArray = Array.create (if n >= 0 then 2 * (n + 1) else 4) null // make room for next assoc
                System.Array.Copy(this.array, 0, newArray, 0, 2 * n)
                BitmapIndexedNode(thread1, this.bitmap, newArray)

        member this.editAndSet(thread1, i, a) =
            let editable = this.ensureEditable(thread1)
            editable.array.[i] <- a
            editable

        member this.editAndSet(thread1, i, a, j, b) =
            let editable = this.ensureEditable(thread1)
            editable.array.[i] <- a
            editable.array.[j] <- b
            editable

        member this.editAndRemovePair(thread1, bit, i) =
            if this.bitmap = bit then
                Unchecked.defaultof<BitmapIndexedNode>
            else
                let editable = this.ensureEditable(thread1)
                editable.bitmap <- editable.bitmap ^^^ bit
                System.Array.Copy(editable.array, 2 * (i + 1), editable.array, 2 * i, editable.array.Length - 2 * (i + 1))
                editable.array.[editable.array.Length - 2] <- null
                editable.array.[editable.array.Length - 1] <- null
                editable

        interface INode with

            member this.find(shift, hash, key) =
                let bit = bitpos(hash, shift)

                if this.bitmap &&& bit = 0 then
                    null
                else
                    let idx' = index(this.bitmap, bit) * 2
                    let keyOrNull = this.array.[idx']
                    let valOrNode = this.array.[idx' + 1]

                    if keyOrNull = null then
                        (valOrNode :?> INode).find(shift + 5, hash, key)
                    else if key = keyOrNull then
                        valOrNode
                    else
                        null

            member this.tryFind(shift, hash, key) =
                let bit = bitpos(hash, shift)

                if this.bitmap &&& bit = 0 then
                    None
                else
                    let idx = index(this.bitmap, bit)
                    let keyOrNull = this.array.[2 * idx]
                    let valOrNode = this.array.[2 * idx + 1]

                    if keyOrNull = null then
                        (valOrNode :?> INode).tryFind(shift + 5, hash, key)
                    else if key = keyOrNull then
                        Some valOrNode
                    else
                        None

            member this.assoc(shift, hashKey, key, value, addedLeaf) =
                let bit = bitpos(hashKey, shift)
                let idx' = index(this.bitmap, bit) * 2

                if (this.bitmap &&& bit) <> 0 then
                    let keyOrNull = this.array.[idx']
                    let valOrNode = this.array.[idx' + 1]

                    if keyOrNull = null then
                        let n = (valOrNode :?> INode).assoc(shift + 5, hashKey, key, value, addedLeaf)

                        if n = (valOrNode :?> INode) then
                            this :> INode
                        else
                            BitmapIndexedNode(ref null, this.bitmap, cloneAndSet(this.array, idx' + 1, n)) :> INode
                    else if key = keyOrNull then
                        if value = valOrNode then
                            this :> INode
                        else
                            BitmapIndexedNode(ref null, this.bitmap, cloneAndSet(this.array, idx' + 1, value)) :> INode
                    else
                        addedLeaf.Value <- addedLeaf

                        BitmapIndexedNode(
                            ref null,
                            this.bitmap,
                            cloneAndSet2(
                                this.array,
                                idx',
                                null,
                                idx' + 1,
                                NodeCreation.createNode(shift + 5, keyOrNull, valOrNode, hashKey, key, value) :> obj
                            )
                        )
                        :> INode
                else
                    let n = NumberOfSetBits(this.bitmap)

                    if n >= 16 then
                        let nodes = Array.create 32 Unchecked.defaultof<INode>
                        let jdx = mask(hashKey, shift)
                        nodes.[jdx] <- (BitmapIndexedNode() :> INode).assoc(shift + 5, hashKey, key, value, addedLeaf)
                        let mutable j = 0

                        for i in 0..31 do
                            if ((this.bitmap >>> i) &&& 1) <> 0 then
                                if this.array.[j] = null then
                                    nodes.[i] <- this.array.[j + 1] :?> INode
                                else
                                    nodes.[i] <-
                                        (BitmapIndexedNode() :> INode)
                                            .assoc(shift + 5, hash(this.array.[j]), this.array.[j], this.array.[j + 1], addedLeaf)

                                j <- j + 2

                        ArrayNode(ref null, n + 1, nodes) :> INode
                    else
                        let newArray = Array.create (2 * (n + 1)) null
                        System.Array.Copy(this.array, 0, newArray, 0, idx')
                        newArray.[idx'] <- key
                        addedLeaf.Value <- addedLeaf
                        newArray.[idx' + 1] <- value
                        System.Array.Copy(this.array, idx', newArray, idx' + 2, 2 * n - idx')
                        BitmapIndexedNode(ref null, this.bitmap ||| bit, newArray) :> INode

            member this.nodeSeq() =
                createNodeSeq this.array


            member this.assoc(thread1, shift, hashKey, key, value, addedLeaf) =
                let bit = bitpos(hashKey, shift)
                let idx' = index(this.bitmap, bit) * 2

                if (this.bitmap &&& bit) <> 0 then
                    let keyOrNull = this.array.[idx']
                    let valOrNode = this.array.[idx' + 1]

                    if keyOrNull = null then
                        let n =
                            (valOrNode :?> INode).assoc(thread1, shift + 5, hashKey, key, value, addedLeaf)

                        if n = (valOrNode :?> INode) then
                            this :> INode
                        else
                            this.editAndSet(thread1, idx' + 1, n) :> INode
                    else if key = keyOrNull then
                        if value = valOrNode then
                            this :> INode
                        else
                            this.editAndSet(thread1, idx' + 1, value) :> INode
                    else
                        addedLeaf.Value <- addedLeaf :> obj

                        this.editAndSet(
                            thread1,
                            idx',
                            null,
                            idx' + 1,
                            NodeCreation.createNode(thread1, shift + 5, keyOrNull, valOrNode, hashKey, key, value)
                        )
                        :> INode
                else
                    let n = NumberOfSetBits(this.bitmap) * 2
                    let n' = n * 2

                    if n' < this.array.Length then
                        addedLeaf.Value <- addedLeaf :> obj
                        let editable = this.ensureEditable(thread1)
                        System.Array.Copy(editable.array, idx', editable.array, idx' + 2, n' - idx')
                        editable.array.[idx'] <- key
                        editable.array.[idx' + 1] <- value
                        editable.bitmap <- editable.bitmap ||| bit
                        editable :> INode
                    else if n >= 16 then
                        let nodes = Array.create 32 Unchecked.defaultof<INode>
                        let jdx = mask(hashKey, shift)

                        nodes.[jdx] <-
                            (BitmapIndexedNode() :> INode)
                                .assoc(thread1, shift + 5, hashKey, key, value, addedLeaf)

                        let mutable j = 0

                        for i in 0..31 do
                            if ((this.bitmap >>> i) &&& 1) <> 0 then
                                if this.array.[j] = null then
                                    nodes.[i] <- this.array.[j + 1] :?> INode
                                else
                                    nodes.[i] <-
                                        (BitmapIndexedNode() :> INode)
                                            .assoc(thread1, shift + 5, hash(this.array.[j]), this.array.[j], this.array.[j + 1], addedLeaf)

                                j <- j + 2

                        ArrayNode(thread1, n + 1, nodes) :> INode
                    else
                        let newArray = Array.create (n' + 8) null
                        System.Array.Copy(this.array, 0, newArray, 0, idx')
                        newArray.[idx'] <- key
                        addedLeaf.Value <- addedLeaf :> obj
                        newArray.[idx' + 1] <- value
                        System.Array.Copy(this.array, idx', newArray, idx' + 2, n' - idx')
                        let editable = this.ensureEditable(thread1)
                        editable.array <- newArray
                        editable.bitmap <- this.bitmap ||| bit
                        editable :> INode


            member this.without(shift, hashKey, key) =
                let bit = bitpos(hashKey, shift)

                if (this.bitmap &&& bit) = 0 then
                    this :> INode
                else
                    let idx = index(this.bitmap, bit)
                    let keyOrNull = this.array.[2 * idx]
                    let valOrNode = this.array.[2 * idx + 1]

                    if keyOrNull = null then
                        let n = (valOrNode :?> INode).without(shift + 5, hashKey, key)

                        if n = (valOrNode :?> INode) then
                            this :> INode
                        else if n <> Unchecked.defaultof<INode> then
                            BitmapIndexedNode(ref null, this.bitmap, cloneAndSet(this.array, 2 * idx + 1, n)) :> INode
                        else if this.bitmap = bit then
                            Unchecked.defaultof<INode>
                        else
                            BitmapIndexedNode(ref null, this.bitmap ^^^ bit, removePair(this.array, idx)) :> INode
                    else if key = keyOrNull then
                        // TODO: collapse
                        BitmapIndexedNode(ref null, this.bitmap ^^^ bit, removePair(this.array, idx)) :> INode
                    else
                        this :> INode

            member this.without(thread1, shift, hashKey, key, removedLeaf) =
                let bit = bitpos(hashKey, shift)

                if (this.bitmap &&& bit) = 0 then
                    this :> INode
                else
                    let idx = index(this.bitmap, bit)

                    let keyOrNull = this.array.[2 * idx]
                    let valOrNode = this.array.[2 * idx + 1]

                    if keyOrNull = null then
                        let n = (valOrNode :?> INode).without(thread1, shift + 5, hashKey, key, removedLeaf)

                        if n = (valOrNode :?> INode) then
                            this :> INode
                        else if n <> Unchecked.defaultof<INode> then
                            this.editAndSet(thread1, 2 * idx + 1, n) :> INode
                        else if this.bitmap = bit then
                            Unchecked.defaultof<INode>
                        else
                            this.editAndRemovePair(thread1, bit, idx) :> INode
                    else if key = keyOrNull then
                        removedLeaf.Value <- removedLeaf :> obj
                        // TODO: collapse
                        this.editAndRemovePair(thread1, bit, idx) :> INode
                    else
                        this :> INode

type internal TransientHashMap<[<EqualityConditionalOn>] 'T, 'S when 'T: equality and 'S: equality>
    (
        thread,
        count',
        root': INode,
        hasNull',
        nullValue': 'S
    ) =
    let leafFlag = Box(null)

    member val hasNull = hasNull' with get, set
    member val nullValue = nullValue' with get, set
    member val count = count' with get, set
    member val root = root' with get, set

    static member Empty() : TransientHashMap<'T, 'S> =
        TransientHashMap(ref Thread.CurrentThread, 0, Unchecked.defaultof<INode>, false, Unchecked.defaultof<'S>)

    member this.Length: int = this.count

    member internal this.EnsureEditable() =
        if !thread = Thread.CurrentThread then
            ()
        else
            if !thread <> null then
                failwith "Transient used by non-owner thread"

            failwith "Transient used after persistent! call"

    member this.ContainsKey(key: 'T) =
        if key = Unchecked.defaultof<'T> then this.hasNull
        else if this.root = Unchecked.defaultof<INode> then false
        else this.root.find(0, hash(key), key) <> null

    member this.Add(key: 'T, value: 'S) =
        if key = Unchecked.defaultof<'T> then
            if this.nullValue <> value then
                this.nullValue <- value

            if not this.hasNull then
                this.count <- this.count + 1
                this.hasNull <- true

            this
        else
            leafFlag.Value <- null

            let n =
                (if this.root = Unchecked.defaultof<INode> then
                     BitmapIndexedNode() :> INode
                 else
                     this.root)
                    .assoc(thread, 0, hash(key), key, value, leafFlag)

            if n <> this.root then
                this.root <- n

            if leafFlag.Value <> null then
                this.count <- this.count + 1

            this


    member this.Remove(key: 'T) =
        if key = Unchecked.defaultof<'T> then
            if not this.hasNull then
                this
            else
                this.hasNull <- false
                this.nullValue <- Unchecked.defaultof<'S>
                this.count <- this.count - 1
                this
        else if this.root = Unchecked.defaultof<INode> then
            this
        else
            leafFlag.Value <- null
            let n = this.root.without(thread, 0, hash(key), key, leafFlag)

            if n <> this.root then
                this.root <- n

            if leafFlag.Value <> null then
                this.count <- this.count - 1

            this

    member this.Item
        with get key =
            if key = Unchecked.defaultof<'T> then
                if this.hasNull then
                    this.nullValue
                else
                    failwith "Key null is not found in the map."
            else if this.root = Unchecked.defaultof<INode> then
                failwithf "Key %A is not found in the map." key
            else
                match this.root.tryFind(0, hash(key), key) with
                | Some value -> value :?> 'S
                | _ -> failwithf "Key %A is not found in the map." key

    member this.persistent() : PersistentHashMap<'T, 'S> =
        thread := null
        PersistentHashMap(this.count, this.root, this.hasNull, this.nullValue)

/// A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals.
/// Hash maps provide fast access (log32N hops). count is O(1).
and PersistentHashMap<[<EqualityConditionalOn>] 'T, 'S when 'T: equality and 'S: equality> =
    val private count: int
    val private root: INode
    val private hasNull: bool
    val private nullValue: 'S

    static member Empty() : PersistentHashMap<'T, 'S> =
        PersistentHashMap(0, Unchecked.defaultof<INode>, false, Unchecked.defaultof<'S>)

    member this.Length: int = this.count
    member this.Count: int = this.count

    internal new(count', root': INode, hasNull', nullValue': 'S) =
        {
            count = count'
            root = root'
            hasNull = hasNull'
            nullValue = nullValue'
        }

    member this.ContainsKey(key: 'T) =
        if key = Unchecked.defaultof<'T> then this.hasNull
        else if this.root = Unchecked.defaultof<INode> then false
        else this.root.find(0, hash(key), key) <> null

    static member ofSeq(items: ('T * 'S) seq) =
        let mutable ret = TransientHashMap<'T, 'S>.Empty ()

        for (key, value) in items do
            ret <- ret.Add(key, value)

        ret.persistent()

    member this.Add(key: 'T, value: 'S) =
        if key = Unchecked.defaultof<'T> then
            if this.hasNull && value = this.nullValue then
                this
            else
                let count = if this.hasNull then this.count else this.count + 1
                PersistentHashMap<'T, 'S>(count, this.root, true, value)
        else
            let addedLeaf = Box(null)

            let newroot =
                (if this.root = Unchecked.defaultof<INode> then
                     BitmapIndexedNode() :> INode
                 else
                     this.root)
                    .assoc(0, hash(key), key, value, addedLeaf)

            if newroot = this.root then
                this
            else
                let count =
                    if addedLeaf.Value = null then
                        this.count
                    else
                        this.count + 1

                PersistentHashMap(count, newroot, this.hasNull, this.nullValue)

    member this.Remove(key: 'T) =
        if key = Unchecked.defaultof<'T> then
            if this.hasNull then
                PersistentHashMap(this.count - 1, this.root, false, Unchecked.defaultof<'S>)
            else
                this
        else if this.root = Unchecked.defaultof<INode> then
            this
        else
            let newroot = this.root.without(0, hash(key), key)

            if newroot = this.root then
                this
            else
                PersistentHashMap(this.count - 1, newroot, this.hasNull, this.nullValue)

    member this.Item
        with get key =
            if key = Unchecked.defaultof<'T> then
                if this.hasNull then
                    this.nullValue
                else
                    failwith "Key null is not found in the map."
            else if this.root = Unchecked.defaultof<INode> then
                failwithf "Key %A is not found in the map." key
            else
                match this.root.tryFind(0, hash(key), key) with
                | Some value -> value :?> 'S
                | _ -> failwithf "Key %A is not found in the map." key

    member this.Iterator<'T, 'S>() : ('T * 'S) seq = seq {
        if this.hasNull then
            yield Unchecked.defaultof<'T>, this.nullValue

        if this.root <> Unchecked.defaultof<INode> then
            yield!
                this.root.nodeSeq()
                |> Seq.map(fun (key, value) -> key :?> 'T, value :?> 'S)
    }

    interface System.Collections.Generic.IEnumerable<'T * 'S> with
        member this.GetEnumerator() =
            this.Iterator().GetEnumerator()

    interface System.Collections.IEnumerable with
        member this.GetEnumerator() =
            (this :> System.Collections.Generic.IEnumerable<'T * 'S>).GetEnumerator() :> System.Collections.IEnumerator

    interface System.Collections.Generic.IReadOnlyCollection<'T * 'S> with
        member this.Count = this.Count

/// Defines functions which allow to access and manipulate PersistentHashMaps.
[<RequireQualifiedAccess>]
module PersistentHashMap =
    ///O(1), returns an empty PersistentHashMap
    let empty<'T, 'S when 'T: equality and 'S: equality> =
        PersistentHashMap.Empty() :> PersistentHashMap<'T, 'S>

    ///O(1), returns the count of the elements in the PersistentHashMap (same as count)
    let inline length(map: PersistentHashMap<'T, 'S>) =
        map.Length

    ///O(1), returns the count of the elements in the PersistentHashMap
    let inline count(map: PersistentHashMap<'T, 'S>) = map.Count

    ///O(log32n), returns if the key exists in the map
    let inline containsKey key (map: PersistentHashMap<'T, 'S>) =
        map.ContainsKey key

    ///O(log32n), returns the value if the exists in the map
    let inline find key (map: PersistentHashMap<'T, 'S>) = map.[key]

    ///O(log32n), adds an element to the map
    let inline add key value (map: PersistentHashMap<'T, 'S>) =
        map.Add(key, value)

    ///O(log32n), removes the element with the given key from the map
    let inline remove key (map: PersistentHashMap<'T, 'S>) =
        map.Remove(key)

    ///O(n). Views the given HashMap as a sequence.
    let inline toSeq(map: PersistentHashMap<'T, 'S>) =
        map :> seq<'T * 'S>

    ///O(n). Returns a HashMap of the seq.
    let inline ofSeq(items: ('T * 'S) seq) =
        PersistentHashMap<'T, 'S>.ofSeq items

    ///O(n). Returns a HashMap whose elements are the results of applying the supplied function to each of the elements of a supplied HashMap.
    let map (f: 'S -> 'S1) (map: PersistentHashMap<'T, 'S>) : PersistentHashMap<'T, 'S1> =
        let mutable ret = TransientHashMap<'T, 'S1>.Empty ()

        for (key, value) in map do
            ret <- ret.Add(key, f value)

        ret.persistent()

#endif

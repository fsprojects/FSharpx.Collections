﻿namespace FSharpx.Collections

open System

module internal Literals2 =

    [<Literal>]
    let internal blockSizeShift = 5 // TODO: what can we do in 64Bit case?

    [<Literal>]
    let internal blockSize = 32

    [<Literal>]
    let internal blockIndexMask = 0x01f

open System.Threading

// Work-arounds for the lack of functionality in the Fable support library
#if FABLE_COMPILER
module Thread =
    module CurrentThread =
        let ManagedThreadId = 1
#endif

module internal Utils =
#if FABLE_COMPILER
    module Array =
        let clone(x: _ array) : obj =
            Array.init x.Length (fun i -> x.[i]) :> obj
#else
    module Array =
        let clone(x: _ array) : obj = x.Clone()
#endif

[<Serializable>]
type NodeR(threadId, array: obj[]) =
    let mutable threadId = threadId
    new() = NodeR(None, Array.create Literals2.blockSize null)

    static member InCurrentThread() =
        NodeR(Some Thread.CurrentThread.ManagedThreadId, Array.create Literals2.blockSize null)

    member this.Array = array
    member this.ThreadId = threadId

    member this.SetThread t =
        threadId <- t

[<Serializable>]
type internal TransientVect<'T>(count, shift: int, root: NodeR, tail: obj[]) =
    let mutable count = count
    let mutable root = root
    let mutable tail = tail
    let mutable shift = shift

    new() = TransientVect<'T>(0, Literals2.blockSizeShift, NodeR.InCurrentThread(), Array.create Literals2.blockSize null)

    member internal this.EnsureEditable(node: NodeR) =
        if node.ThreadId = root.ThreadId then
            node
        else
            NodeR(root.ThreadId, Array.copy node.Array)

    member internal this.NewPath(level, node: NodeR) =
        if level = 0 then
            node
        else
            let ret = Array.create Literals2.blockSize null
            ret.[0] <- this.NewPath(level - Literals2.blockSizeShift, node) :> obj
            NodeR(node.ThreadId, ret)

    member internal this.PushTail(level, parent: NodeR, tailnode) =
        //if parent is leaf, insert node,
        // else does it map to an existing child? -> nodeToInsert = pushNode one more level
        // else alloc new path
        //return  nodeToInsert placed in copy of parent
        let parent = this.EnsureEditable parent
        let subidx = ((count - 1) >>> level) &&& Literals2.blockIndexMask
        let ret = parent

        let nodeToInsert =
            if level = Literals2.blockSizeShift then
                tailnode
            else

                let child = parent.Array.[subidx]

                if child <> null then
                    this.PushTail(level - Literals2.blockSizeShift, child :?> NodeR, tailnode)
                else
                    this.NewPath(level - Literals2.blockSizeShift, tailnode)

        ret.Array.[subidx] <- nodeToInsert :> obj
        ret

    member internal this.ArrayFor i =
        if i >= 0 && i < count then
            if i >= this.TailOff() then
                tail
            else
                let mutable node = root
                let mutable level = shift

                while level > 0 do
                    let pos = (i >>> level) &&& Literals2.blockIndexMask
                    node <- node.Array.[pos] :?> NodeR
                    level <- level - Literals2.blockSizeShift

                node.Array
        else
            raise(new System.IndexOutOfRangeException())

    member this.conj<'T>(x: 'T) =
        this.EnsureEditable()

        //room in tail?
        if count - this.TailOff() < Literals2.blockSize then
            tail.[count &&& Literals2.blockIndexMask] <- x :> obj
        else
            //full tail, push into tree
            let tailNode = NodeR(root.ThreadId, tail)
            let newShift = shift
            let newTail = Array.create Literals2.blockSize null
            newTail.[0] <- x :> obj

            //overflow root?
            let newRoot =
                if (count >>> Literals2.blockSizeShift) > (1 <<< shift) then
                    let newRoot = NodeR(root.ThreadId, Array.create Literals2.blockSize null)
                    newRoot.Array.[0] <- root :> obj
                    newRoot.Array.[1] <- this.NewPath(shift, tailNode) :> obj
                    shift <- shift + Literals2.blockSizeShift
                    newRoot
                else
                    this.PushTail(shift, root, tailNode)

            tail <- newTail
            root <- newRoot

        count <- count + 1
        this

    member this.persistent() : RandomAccessList<'T> =
        this.EnsureEditable()
        root.SetThread None
        let l = count - this.TailOff()
        let trimmedTail = Array.init l (fun i -> tail.[i])
        RandomAccessList(count, shift, root, trimmedTail)

    member internal this.EnsureEditable() =
        if root.ThreadId = Some Thread.CurrentThread.ManagedThreadId then
            ()
        else
            if root.ThreadId <> None then
                failwith "Transient used by non-owner thread"

            failwith "Transient used after persistent! call"

    member internal this.TailOff() =
        if count < Literals2.blockSize then
            0
        else
            ((count - 1) >>> Literals2.blockSizeShift)
            <<< Literals2.blockSizeShift

and [<Serializable>] RandomAccessList<'T>(count, shift: int, root: NodeR, tail: obj[]) =
    let mutable hashCode = None

    let tailOff =
        if count < Literals2.blockSize then
            0
        else
            ((count - 1) >>> Literals2.blockSizeShift)
            <<< Literals2.blockSizeShift

    static member Empty() : RandomAccessList<'T> =
        RandomAccessList<'T>(0, Literals2.blockSizeShift, NodeR(), [||])

    static member ofSeq(items: 'T seq) =

        let mutable ret = TransientVect()

        for item in (items |> List.ofSeq |> List.rev |> Seq.ofList) do
            ret <- ret.conj item

        ret.persistent()

    override this.GetHashCode() =
        match hashCode with
        | None ->
            let mutable hash = 1

            for x in this.rangedIterator(0, count) do
                hash <- 31 * hash + Unchecked.hash x

            hashCode <- Some hash
            hash
        | Some hash -> hash

    override this.Equals(other) =
        match other with
        | :? RandomAccessList<'T> as y -> (this :> IEquatable<RandomAccessList<'T>>).Equals(y)
        | _ -> false

    member internal this.SetHash hash =
        hashCode <- hash
        this

    member internal this.NewPath(level, node: NodeR) =
        if level = 0 then
            node
        else
            let ret = NodeR(root.ThreadId, Array.create Literals2.blockSize null)
            ret.Array.[0] <- this.NewPath(level - Literals2.blockSizeShift, node) :> obj
            ret

    member internal this.PushTail(level, parent: NodeR, tailnode) =
        //if parent is leaf, insert node,
        // else does it map to an existing child? -> nodeToInsert = pushNode one more level
        // else alloc new path
        //return  nodeToInsert placed in copy of parent
        let subidx = ((count - 1) >>> level) &&& Literals2.blockIndexMask
        let ret = NodeR(parent.ThreadId, Array.copy parent.Array)

        let nodeToInsert =
            if level = Literals2.blockSizeShift then
                tailnode
            else

                let child = parent.Array.[subidx]

                if child <> null then
                    this.PushTail(level - Literals2.blockSizeShift, child :?> NodeR, tailnode)
                else
                    this.NewPath(level - Literals2.blockSizeShift, tailnode)

        ret.Array.[subidx] <- nodeToInsert :> obj
        ret

    member internal this.ArrayFor i =
        if i >= 0 && i < count then
            if i >= tailOff then
                tail
            else
                let mutable node = root
                let mutable level = shift

                while level > 0 do
                    let pos = (i >>> level) &&& Literals2.blockIndexMask
                    node <- node.Array.[pos] :?> NodeR
                    level <- level - Literals2.blockSizeShift

                node.Array
        else
            raise(System.IndexOutOfRangeException())

    member internal this.doAssoc(level, node: NodeR, i, x) =
        let ret = NodeR(root.ThreadId, Array.copy node.Array)

        if level = 0 then
            ret.Array.[i &&& Literals2.blockIndexMask] <- x :> obj
        else
            let subidx = (i >>> level) &&& Literals2.blockIndexMask
            ret.Array.[subidx] <- this.doAssoc(level - Literals2.blockSizeShift, node.Array.[subidx] :?> NodeR, i, x) :> obj

        ret

    member internal this.PopTail(level, node: NodeR) : NodeR =
        let subidx = ((count - 2) >>> level) &&& Literals2.blockIndexMask

        if level > Literals2.blockSizeShift then
            let newchild =
                this.PopTail(level - Literals2.blockSizeShift, node.Array.[subidx] :?> NodeR)

            if newchild = Unchecked.defaultof<NodeR> && subidx = 0 then
                Unchecked.defaultof<NodeR>
            else
                let ret = NodeR(root.ThreadId, Array.copy node.Array)
                ret.Array.[subidx] <- newchild :> obj
                ret

        elif subidx = 0 then
            Unchecked.defaultof<NodeR>
        else

            let ret = new NodeR(root.ThreadId, Array.copy node.Array)
            ret.Array.[subidx] <- null
            ret

    member this.rangedIterator<'T>(startIndex, endIndex) : 'T seq =
        if count = 0 then
            Seq.empty
        else
            let mutable i = endIndex - 1
            let mutable array = if (endIndex - 1) < count then this.ArrayFor i else null

            seq {
                while i > (startIndex - 1) do
                    if (i + 1) % Literals2.blockSize = 0 then
                        array <- this.ArrayFor i

                    yield (array).[i &&& Literals2.blockIndexMask] :?> 'T
                    i <- i - 1
            }

    member this.Cons(x: 'T) =
        if count - tailOff < Literals2.blockSize then
            let newTail = Array.append tail [| x :> obj |]
            RandomAccessList<'T>(count + 1, shift, root, newTail)
        else
            //full tail, push into tree
            let tailNode = NodeR(root.ThreadId, tail)
            let newShift = shift

            //overflow root?
            if (count >>> Literals2.blockSizeShift) > (1 <<< shift) then
                let newRoot = NodeR()
                newRoot.Array.[0] <- root :> obj
                newRoot.Array.[1] <- this.NewPath(shift, tailNode) :> obj
                RandomAccessList<'T>(count + 1, shift + Literals2.blockSizeShift, newRoot, [| x |])
            else
                let newRoot = this.PushTail(shift, root, tailNode)
                RandomAccessList<'T>(count + 1, shift, newRoot, [| x |])

    member this.IsEmpty = (count = 0)

    member this.Item
        with get i =
            let k = (count - 1) - i
            let node = this.ArrayFor k
            node.[k &&& Literals2.blockIndexMask] :?> 'T

    member this.Head =
        if count > 0 then
            this.[0]
        else
            failwith "Can't peek empty randomAccessList"

    member this.Rev() =
        if count = 0 then
            RandomAccessList.Empty() :> RandomAccessList<'T>
        else
            let mutable ret = TransientVect()

            for item in this.rangedIterator(0, count) do
                ret <- ret.conj item

            ret.persistent()

    member this.TryHead = if count > 0 then Some(this.[0]) else None

    member this.Length: int = count

    member this.Tail =
        if count = 0 then
            failwith "Can't tail empty randomAccessList"
        else if count = 1 then
            RandomAccessList<'T>.Empty ()
        else

        if
            count - tailOff > 1
        then
            let mutable newroot =
                NodeR(Some Thread.CurrentThread.ManagedThreadId, (Utils.Array.clone root.Array) :?> obj[])

            let mutable ret =
                TransientVect(count - 1, shift, newroot, tail.[0 .. (tail.Length - 1)])

            ret.persistent()
        else
            let newtail = this.ArrayFor(count - 2)

            let mutable newroot = this.PopTail(shift, root)
            let mutable newshift = shift

            if newroot = Unchecked.defaultof<NodeR> then
                newroot <- NodeR()

            if shift > Literals2.blockSizeShift && newroot.Array.[1] = null then
                newroot <- newroot.Array.[0] :?> NodeR
                newshift <- newshift - Literals2.blockSizeShift

            RandomAccessList(count - 1, newshift, newroot, newtail)

    member this.TryTail = if count = 0 then None else Some(this.Tail)

    member this.Uncons =
        if count > 0 then
            this.[0], this.Tail
        else
            failwith "Can't peek empty randomAccessList"

    member this.TryUncons = if count > 0 then Some(this.[0], this.Tail) else None

    member this.Update(i, x: 'T) =
        let k = (count - 1) - i

        if k >= 0 && k < count then
            if k >= tailOff then
                let newTail = Array.copy tail
                newTail.[k &&& Literals2.blockIndexMask] <- x :> obj
                RandomAccessList(count, shift, root, newTail)
            else
                RandomAccessList(count, shift, this.doAssoc(shift, root, k, x), tail)
        elif k = count then
            this.Cons x
        else
            raise(new System.IndexOutOfRangeException())

    member this.TryUpdate(i, x: 'T) =
        if i >= 0 && i < count then
            Some(this.Update(i, x))
        else
            None

    interface IEquatable<RandomAccessList<'T>> with
        member this.Equals(y) =
            if this.Length <> y.Length then false
            else if this.GetHashCode() <> y.GetHashCode() then false
            else Seq.forall2 (Unchecked.equals) this y

    interface System.Collections.Generic.IEnumerable<'T> with
        member this.GetEnumerator() =
            this.rangedIterator(0, count).GetEnumerator()

    interface System.Collections.IEnumerable with
        member this.GetEnumerator() =
            (this.rangedIterator(0, count).GetEnumerator()) :> System.Collections.IEnumerator

    interface System.Collections.Generic.IReadOnlyCollection<'T> with
        member this.Count = this.Length

    interface System.Collections.Generic.IReadOnlyList<'T> with
        member this.Item
            with get i = this.[i]

[<RequireQualifiedAccess>]
module RandomAccessList =
    //pattern discriminators  (active pattern)
    let (|Cons|Nil|)(v: RandomAccessList<'T>) =
        match v.TryUncons with
        | Some(a, b) -> Cons(a, b)
        | None -> Nil

    let append (listA: RandomAccessList<'T>) (listB: RandomAccessList<'T>) =
        let mutable ret = TransientVect()

        for i in (listB.Length - 1) .. -1 .. 0 do
            ret <- ret.conj listB.[i]

        for i in (listA.Length - 1) .. -1 .. 0 do
            ret <- ret.conj listA.[i]

        ret.persistent()

    let inline cons (x: 'T) (randomAccessList: 'T RandomAccessList) =
        randomAccessList.Cons x

    let empty<'T> = RandomAccessList.Empty() :> RandomAccessList<'T>

    let inline fold (f: ('State -> 'T -> 'State)) (state: 'State) (v: RandomAccessList<'T>) =
        let rec loop state' (v': RandomAccessList<'T>) count =
            match count with
            | _ when count = v'.Length -> state'
            | _ -> loop (f state' v'.[count]) v' (count + 1)

        loop state v 0

    let inline foldBack (f: ('T -> 'State -> 'State)) (v: RandomAccessList<'T>) (state: 'State) =
        let rec loop state' (v': RandomAccessList<'T>) count =
            match count with
            | -1 -> state'
            | _ -> loop (f v'.[count] state') v' (count - 1)

        loop state v (v.Length - 1)

    let init count (f: int -> 'T) : 'T RandomAccessList =
        let mutable ret = TransientVect()

        for i in 0 .. (count - 1) do
            ret <- ret.conj(f i)

        ret.persistent().Rev()

    let inline isEmpty(randomAccessList: 'T RandomAccessList) =
        randomAccessList.IsEmpty

    let inline head(randomAccessList: 'T RandomAccessList) =
        randomAccessList.Head

    let inline tryHead(randomAccessList: 'T RandomAccessList) =
        randomAccessList.TryHead

    let inline length(randomAccessList: 'T RandomAccessList) : int =
        randomAccessList.Length

    let map (f: 'T -> 'T1) (randomAccessList: 'T RandomAccessList) : 'T1 RandomAccessList =
        let mutable ret = TransientVect()

        for item in randomAccessList do
            ret <- ret.conj(f item)

        ret.persistent().Rev()

    let inline nth i (randomAccessList: 'T RandomAccessList) =
        randomAccessList.[i]

    let inline tryNth i (randomAccessList: 'T RandomAccessList) =
        if i >= 0 && i < randomAccessList.Length then
            Some(randomAccessList.[i])
        else
            None

    let inline nthNth i j (randomAccessList: 'T RandomAccessList RandomAccessList) : 'T =
        randomAccessList.[i] |> nth j

    let inline tryNthNth i j (randomAccessList: 'T RandomAccessList RandomAccessList) =
        match tryNth i randomAccessList with
        | Some v' -> tryNth j v'
        | None -> None

    let ofSeq(items: 'T seq) =
        RandomAccessList.ofSeq items

    let inline rev(randomAccessList: 'T RandomAccessList) =
        randomAccessList.Rev()

    let inline singleton(x: 'T) =
        empty |> cons x

    let inline tail(randomAccessList: 'T RandomAccessList) =
        randomAccessList.Tail

    let inline tryTail(randomAccessList: 'T RandomAccessList) =
        randomAccessList.TryTail

    let inline toSeq(randomAccessList: 'T RandomAccessList) =
        randomAccessList :> seq<'T>

    let inline uncons(randomAccessList: 'T RandomAccessList) =
        randomAccessList.Uncons

    let inline tryUncons(randomAccessList: 'T RandomAccessList) =
        randomAccessList.TryUncons

    let inline update i (x: 'T) (randomAccessList: 'T RandomAccessList) =
        randomAccessList.Update(i, x)

    let inline updateNth i j (x: 'T) (randomAccessList: 'T RandomAccessList RandomAccessList) : 'T RandomAccessList RandomAccessList =
        randomAccessList.Update(i, (randomAccessList.[i].Update(j, x)))

    let inline tryUpdate i (x: 'T) (randomAccessList: 'T RandomAccessList) =
        randomAccessList.TryUpdate(i, x)

    let inline tryUpdateNth i j (x: 'T) (randomAccessList: 'T RandomAccessList RandomAccessList) =
        if
            i >= 0
            && i < randomAccessList.Length
            && j >= 0
            && j < randomAccessList.[i].Length
        then
            Some(updateNth i j x randomAccessList)
        else
            None

    let inline windowFun windowLength =
        fun t (v: RandomAccessList<RandomAccessList<'T>>) ->
            if v.Head.Length = windowLength then
                v |> cons(empty.Cons(t))
            else
                tail v |> cons(head v |> cons t)

    let inline windowSeq windowLength (items: 'T seq) =
        if windowLength < 1 then
            invalidArg "windowLength" "length is less than 1"
        else
            (Seq.foldBack (windowFun windowLength) items (empty.Cons empty<'T>)) (*Seq.fold (windowFun windowLength) (empty.Cons empty<'T>) items*) // TODO: Check if this should be foldBack due to inversion effects of prepending

    let zip (randomAccessList1: RandomAccessList<'T>) (randomAccessList2: RandomAccessList<'T2>) =
        if
            randomAccessList1.Length = randomAccessList2.Length
            || randomAccessList1.IsEmpty
        then
            let arr =
                Array.create randomAccessList1.Length (randomAccessList1.[0], randomAccessList2.[0])

            for i in [ 1 .. arr.Length - 1 ] do
                arr.[i] <- randomAccessList1.[i], randomAccessList2.[i]

            RandomAccessList.ofSeq arr
        else
            invalidArg "zip" "length of RandomAccessList not the same or both empty"

    let reduce f (randomAccessList: RandomAccessList<'T>) =
        if randomAccessList.Length > 0 then
            if randomAccessList.Length = 1 then
                randomAccessList.[0]
            else
                let rec loop i accum =
                    if i > randomAccessList.Length - 1 then
                        accum
                    else
                        loop (i + 1) (f accum randomAccessList.[i])

                loop 2 (f randomAccessList.[0] randomAccessList.[1])
        else
            invalidArg "zip" "length of RandomAccessList not the same or both empty"

    let map2 f (randomAccessList1: RandomAccessList<'T1>) (randomAccessList2: RandomAccessList<'T2>) =
        randomAccessList2 |> zip randomAccessList1 |> map(fun (x, y) -> f x y)

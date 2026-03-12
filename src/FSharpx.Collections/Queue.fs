// XML docs in fsi

namespace FSharpx.Collections

type Queue<'T>(front: list<'T>, rBack: list<'T>) =
    let mutable hashCode = None
    member internal this.front = front
    member internal this.rBack = rBack

    override this.GetHashCode() =
        match hashCode with
        | None ->
            let mutable hash = 1

            for x in this do
                hash <- 31 * hash + Unchecked.hash x

            hashCode <- Some hash
            hash
        | Some hash -> hash

    override this.Equals(other) =
        match other with
        | :? Queue<'T> as y -> (this :> System.IEquatable<Queue<'T>>).Equals y
        | _ -> false

    member this.Conj x =
        match front, x :: rBack with
        | [], r -> Queue((List.rev r), [])
        | f, r -> Queue(f, r)

    member this.Head =
        match front with
        | hd :: _ -> hd
        | _ -> raise(new System.Exception("Queue is empty"))

    member this.TryHead =
        match front with
        | hd :: _ -> Some(hd)
        | _ -> None

    member this.IsEmpty = front.IsEmpty

    member this.Length = front.Length + rBack.Length

    member this.Rev() =
        match rBack, front with
        | [], r -> Queue((List.rev r), [])
        | f, r -> Queue(f, r)

    member this.Tail =
        match front with
        | hd :: tl ->
            match tl, rBack with
            | [], r -> Queue((List.rev r), [])
            | f, r -> Queue(f, r)
        | _ -> raise(new System.Exception("Queue is empty"))

    member this.TryTail =
        match front with
        | hd :: tl ->
            match tl, rBack with
            | [], r -> Some(Queue((List.rev r), []))
            | f, r -> Some(Queue(f, r))
        | _ -> None

    member this.Uncons =
        match front with
        | hd :: tl ->
            hd,
            (match tl, rBack with
             | [], r -> Queue((List.rev r), [])
             | f, r -> Queue(f, r))
        | _ -> raise(new System.Exception("Queue is empty"))

    member this.TryUncons =
        match front with
        | hd :: tl ->
            match tl, rBack with
            | [], r -> Some(hd, Queue((List.rev r), []))
            | f, r -> Some(hd, Queue(f, r))
        | _ -> None

    interface System.IEquatable<Queue<'T>> with
        member this.Equals(y: Queue<'T>) =
            if this.Length <> y.Length then false
            else if this.GetHashCode() <> y.GetHashCode() then false
            else Seq.forall2 (Unchecked.equals) this y

    interface System.Collections.Generic.IEnumerable<'T> with
        override this.GetEnumerator() : System.Collections.Generic.IEnumerator<'T> =
            let e =
                seq {
                    yield! front
                    yield! (List.rev rBack)
                }

            e.GetEnumerator()

    interface System.Collections.IEnumerable with
        override this.GetEnumerator() =
            (this :> System.Collections.Generic.IEnumerable<'T>).GetEnumerator() :> System.Collections.IEnumerator

    interface System.Collections.Generic.IReadOnlyCollection<'T> with
        member this.Count = this.Length

[<RequireQualifiedAccess>]
module Queue =
    //pattern discriminators  (active pattern)
    let (|Cons|Nil|)(q: Queue<'T>) =
        match q.TryUncons with
        | Some(a, b) -> Cons(a, b)
        | None -> Nil

    let inline conj (x: 'T) (q: Queue<'T>) =
        (q.Conj x)

    let empty<'T> : Queue<'T> = Queue<_>([], [])

    let fold (f: ('State -> 'T -> 'State)) (state: 'State) (q: Queue<'T>) =
        let s = List.fold f state q.front
        List.fold f s (List.rev q.rBack)

    let foldBack (f: ('T -> 'State -> 'State)) (q: Queue<'T>) (state: 'State) =
        let s = List.foldBack f (List.rev q.rBack) state
        (List.foldBack f q.front s)

    let inline head(q: Queue<'T>) = q.Head

    let inline tryHead(q: Queue<'T>) = q.TryHead

    let inline isEmpty(q: Queue<'T>) = q.IsEmpty

    let inline length(q: Queue<'T>) = q.Length

    let ofList xs =
        Queue<'T>(xs, [])

    let ofSeq xs =
        Queue<'T>((List.ofSeq xs), [])

    let inline rev(q: Queue<'T>) = q.Rev()

    let inline tail(q: Queue<'T>) = q.Tail

    let inline tryTail(q: Queue<'T>) = q.TryTail

    let inline toSeq(q: Queue<'T>) =
        q :> seq<'T>

    ///O(n). Returns a list of the queue elements in FIFO order.
    let toList(q: Queue<'T>) : 'T list =
        q.front @ List.rev q.rBack

    ///O(n). Returns an array of the queue elements in FIFO order.
    let toArray(q: Queue<'T>) : 'T[] =
        let arr = Array.zeroCreate q.Length
        let mutable i = 0

        List.iter
            (fun x ->
                arr.[i] <- x
                i <- i + 1)
            q.front

        List.iter
            (fun x ->
                arr.[i] <- x
                i <- i + 1)
            (List.rev q.rBack)

        arr

    ///O(n). Returns a new queue whose elements are the results of applying the given function to each element.
    let map (f: 'T -> 'U) (q: Queue<'T>) : Queue<'U> =
        Queue<'U>(List.map f q.front, List.map f q.rBack)

    ///O(n). Returns a new queue containing only the elements of the original for which the given predicate returns true.
    let filter (predicate: 'T -> bool) (q: Queue<'T>) : Queue<'T> =
        let f = List.filter predicate q.front
        let r = List.filter predicate q.rBack

        match f with
        | [] -> Queue<'T>(List.rev r, [])
        | _ -> Queue<'T>(f, r)

    ///O(n). Applies the given function to each element of the queue.
    let iter (f: 'T -> unit) (q: Queue<'T>) =
        List.iter f q.front
        List.iter f (List.rev q.rBack)

    ///O(n). Returns true if any element of the queue satisfies the given predicate.
    let exists (predicate: 'T -> bool) (q: Queue<'T>) : bool =
        List.exists predicate q.front || List.exists predicate q.rBack

    ///O(n). Returns true if all elements of the queue satisfy the given predicate.
    let forall (predicate: 'T -> bool) (q: Queue<'T>) : bool =
        List.forall predicate q.front && List.forall predicate q.rBack

    let inline uncons(q: Queue<'T>) = q.Uncons

    let inline tryUncons(q: Queue<'T>) =
        q.TryUncons

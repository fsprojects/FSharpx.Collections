﻿//originally published by Julien
// original implementation taken from http://lepensemoi.free.fr/index.php/2009/12/17/double-ended-queue

//jf -- added rev, ofSeq, ofSeqC, lookup, append, appendC, update, remove, tryUpdate, tryRemove

//pattern discriminators Cons, Snoc, and Nil

namespace FSharpx.Collections.Experimental

open System.Collections
open System.Collections.Generic
open FSharpx.Collections
open ListHelpr

type Deque<'T>(front, rBack) =

    static member private splitAux n (r: 'T list) (acc: 'T list) =
        match r with
        | hd :: tl when List.length acc < n -> Deque.splitAux n tl (hd :: acc)
        | _ -> List.rev r, List.rev acc

    static member private split(r: 'T list) =
        Deque.splitAux (List.length r / 2) r []

    static member private checkf: 'T list * 'T list -> 'T list * 'T list =
        function
        | [], r -> Deque.split r
        | deq -> deq

    static member private checkr: 'T list * 'T list -> 'T list * 'T list =
        function
        | f, [] ->
            let a, b = Deque.split f
            b, a
        | deq -> deq

    static member internal Empty() =
        Deque(List.Empty, List.Empty)

    static member internal OfCatLists (xs: 'T list) (ys: 'T list) =
        match xs, ys with
        | [], _ -> new Deque<'T>(Deque.checkf(xs, (List.rev ys)))
        | _, [] -> new Deque<'T>(Deque.checkr(xs, (List.rev ys)))
        | _, _ -> new Deque<'T>(xs, (List.rev ys))

    static member internal OfSeq(xs: seq<'T>) =
        new Deque<'T>(Deque.checkr((List.ofSeq xs), []))

    static member internal Singleton x =
        Deque([ x ], List.Empty)

    ///O(1) amortized, O(n), worst case. Returns a new deque with the element added to the beginning.
    member this.Cons x =
        let f, r = Deque.checkr(x :: front, rBack)
        Deque(f, r)

    ///O(1) amortized, O(n), worst case. Returns the first element.
    member this.Head =
        match front, rBack with
        | [], [] -> raise Exceptions.Empty
        | hd :: tl, _ -> hd
        | [], xs -> List.rev xs |> List.head

    ///O(1) amortized, O(n), worst case. Returns option first element.
    member this.TryGetHead =
        match front, rBack with
        | [], [] -> None
        | hd :: tl, _ -> Some(hd)
        | [], xs -> Some(List.rev xs |> List.head)

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements before the last element.
    member this.Init =
        let rec loop: 'T list * 'T list -> Deque<'T> =
            function
            | [], [] -> raise Exceptions.Empty
            | f, hd :: tl ->
                let f, r = Deque.checkr(f, tl)
                Deque(f, r)
            | hd :: [], [] -> Deque([], [])
            | f, [] -> Deque.split f |> loop

        loop(front, rBack)

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements before the last element.
    member this.TryGetInit =
        let rec loop: 'T list * 'T list -> Deque<'T> option =
            function
            | [], [] -> None
            | f, hd :: tl ->
                let f, r = Deque.checkr(f, tl)
                Some(Deque(f, r))
            | hd :: [], [] -> Some(Deque([], []))
            | f, [] -> Deque.split f |> loop

        loop(front, rBack)

    ///O(1). Returns true if the deque has no elements.
    member this.IsEmpty =
        match front, rBack with
        | [], [] -> true
        | _ -> false

    ///O(1) amortized, O(n), worst case. Returns the last element.
    member this.Last =
        match front, rBack with
        | [], [] -> raise Exceptions.Empty
        | xs, [] -> List.rev xs |> List.head
        | _, hd :: tl -> hd

    ///O(1) amortized, O(n), worst case. Returns option last element.
    member this.TryGetLast =
        match front, rBack with
        | [], [] -> None
        | xs, [] -> Some(List.rev xs |> List.head)
        | _, hd :: tl -> Some(hd)

    ///O(1). Returns the count of elements.
    member this.Length = front.Length + rBack.Length

    ///O(n), worst case. Returns element by index.
    member this.Lookup(i: int) =
        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> raise Exceptions.OutOfBounds
        | lenF, front, lenR, rear when i < lenF ->
            let rec loopF =
                function
                | xs, i' when i' = 0 -> List.head xs
                | xs, i' -> loopF((List.tail xs), (i' - 1))

            loopF(front, i)
        | lenF, front, lenR, rear ->
            let rec loopF =
                function
                | xs, i' when i' = 0 -> List.head xs
                | xs, i' -> loopF((List.tail xs), (i' - 1))

            loopF(rear, ((lenR - (i - lenF)) - 1))

    ///O(n), worst case. Returns option element by index.
    member this.TryLookup(i: int) =
        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> None
        | lenF, front, lenR, rear when i < lenF ->
            let rec loopF =
                function
                | xs, i' when i' = 0 -> Some(List.head xs)
                | xs, i' -> loopF((List.tail xs), (i' - 1))

            loopF(front, i)
        | lenF, front, lenR, rear ->
            let rec loopF =
                function
                | xs, i' when i' = 0 -> Some(List.head xs)
                | xs, i' -> loopF((List.tail xs), (i' - 1))

            loopF(rear, ((lenR - (i - lenF)) - 1))

    ///O(n), worst case. Returns deque with element removed by index.
    member this.Remove(i: int) =

        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> raise Exceptions.OutOfBounds
        | lenF, front, lenR, rear when i < lenF ->
            let newFront =
                if (i = 0) then
                    List.tail front
                else
                    let left, right = loop2Array (Array.create i (List.head front)) front (i - 1)
                    loopFromArray ((Seq.length left) - 1) left right 0

            (new Deque<'T>(newFront, rear))

        | lenF, front, lenR, rear ->
            let n = lenR - (i - lenF) - 1

            let newRear =
                if (n = 0) then
                    List.tail rear
                else
                    let left, right = loop2Array (Array.create n (List.head rear)) rear (n - 1)
                    loopFromArray ((Seq.length left) - 1) left right 0

            (new Deque<'T>(Deque.checkf(front, newRear)))

    ///O(n), worst case. Returns option deque with element removed by index.
    member this.TryRemove(i: int) =

        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> None
        | lenF, front, lenR, rear when i < lenF ->
            let newFront =
                if (i = 0) then
                    List.tail front
                else
                    let left, right = loop2Array (Array.create i (List.head front)) front (i - 1)
                    loopFromArray ((Seq.length left) - 1) left right 0

            Some((new Deque<'T>(newFront, rear)))

        | lenF, front, lenR, rear ->
            let n = lenR - (i - lenF) - 1

            let newRear =
                if (n = 0) then
                    List.tail rear
                else
                    let left, right = loop2Array (Array.create n (List.head rear)) rear (n - 1)
                    loopFromArray ((Seq.length left) - 1) left right 0

            Some((new Deque<'T>(Deque.checkf(front, newRear))))

    ///O(1). Returns deque reversed.
    member this.Rev = new Deque<'T>(rBack, front)

    ///O(1) amortized, O(n), worst case. Returns a new deque with the element added to the end.
    member this.Snoc x =
        let f, r = Deque.checkf(front, x :: rBack)
        Deque(f, r)

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements trailing the first element.
    member this.Tail =
        let rec loop: 'T list * 'T list -> Deque<'T> =
            function
            | [], [] -> raise Exceptions.Empty
            | hd :: tl, r ->
                let f, r = Deque.checkf(tl, r)
                Deque(f, r)
            | [], r -> Deque.split r |> loop

        loop(front, rBack)

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements trailing the first element.
    member this.TryGetTail =
        let rec loop: 'T list * 'T list -> Deque<'T> option =
            function
            | [], [] -> None
            | hd :: tl, r ->
                let f, r = Deque.checkf(tl, r)
                Some(Deque(f, r))
            | [], r -> Deque.split r |> loop

        loop(front, rBack)

    ///O(1) amortized, O(n), worst case. Returns the first element and tail.
    member this.Uncons =
        match front, rBack with
        | [], [] -> raise Exceptions.Empty
        | _, _ -> this.Head, this.Tail

    ///O(1) amortized, O(n), worst case. Returns option first element and tail.
    member this.TryUncons =
        match front, rBack with
        | [], [] -> None
        | _, _ -> Some(this.Head, this.Tail)

    ///O(1) amortized, O(n), worst case. Returns init and the last element.
    member this.Unsnoc =
        match front, rBack with
        | [], [] -> raise Exceptions.Empty
        | _, _ -> this.Init, this.Last

    ///O(1) amortized, O(n), worst case. Returns option init and the last element.
    member this.TryUnsnoc =
        match front, rBack with
        | [], [] -> None
        | _, _ -> Some(this.Init, this.Last)

    ///O(n), worst case. Returns deque with element updated by index.
    member this.Update (i: int) (y: 'T) =
        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> raise Exceptions.OutOfBounds
        | lenF, front, lenR, rear when i < lenF ->
            let newFront =
                if (i = 0) then
                    y :: (List.tail front)
                else
                    let left, right = loop2Array (Array.create i (List.head front)) front (i - 1)
                    loopFromArray ((Seq.length left) - 1) left (y :: right) 0

            new Deque<'T>(Deque.checkf(newFront, rear))

        | lenF, front, lenR, rear ->
            let n = lenR - (i - lenF) - 1

            let newRear =
                if (n = 0) then
                    y :: (List.tail rear)
                else
                    let left, right = loop2Array (Array.create n (List.head rear)) rear (n - 1)
                    loopFromArray ((Seq.length left) - 1) left (y :: right) 0

            new Deque<'T>(Deque.checkf(front, newRear))

    ///O(n), worst case. Returns option deque with element updated by index.
    member this.TryUpdate (i: int) (y: 'T) =
        match (List.length front), front, (List.length rBack), rBack with
        | lenF, front, lenR, rear when i > (lenF + lenR - 1) -> None
        | lenF, front, lenR, rear when i < lenF ->
            let newFront =
                if (i = 0) then
                    y :: (List.tail front)
                else
                    let left, right = loop2Array (Array.create i (List.head front)) front (i - 1)
                    loopFromArray ((Seq.length left) - 1) left (y :: right) 0

            Some((new Deque<'T>(Deque.checkf(newFront, rear))))

        | lenF, front, lenR, rear ->
            let n = lenR - (i - lenF) - 1

            let newRear =
                if (n = 0) then
                    y :: (List.tail rear)
                else
                    let left, right = loop2Array (Array.create n (List.head rear)) rear (n - 1)
                    loopFromArray ((Seq.length left) - 1) left (y :: right) 0

            Some((new Deque<'T>(Deque.checkf(front, newRear))))

    interface IDeque<'T> with

        member this.Cons x =
            this.Cons x :> _

        member this.Count = this.Length

        member this.Head = this.Head

        member this.TryGetHead = this.TryGetHead

        member this.Init = this.Init :> _

        member this.TryGetInit = Some(this.TryGetInit.Value :> _)

        member this.IsEmpty = this.IsEmpty

        member this.Last = this.Last

        member this.TryGetLast = this.TryGetLast

        member this.Length = this.Length

        member this.Lookup i =
            this.Lookup i

        member this.TryLookup i =
            this.TryLookup i

        member this.Remove i =
            this.Remove i :> _

        member this.TryRemove i =
            match this.TryRemove i with
            | None -> None
            | Some(q) -> Some(q :> _)

        member this.Rev = this.Rev :> _

        member this.Snoc x =
            this.Snoc x :> _

        member this.Tail = this.Tail :> _

        member this.TryGetTail =
            match this.TryGetTail with
            | None -> None
            | Some(q) -> Some(q :> _)

        member this.Uncons =
            let x, xs = this.Uncons
            x, xs :> _

        member this.TryUncons =
            match this.TryUncons with
            | None -> None
            | Some(x, q) -> Some(x, q :> _)

        member this.Unsnoc =
            let xs, x = this.Unsnoc
            xs :> _, x

        member this.TryUnsnoc =
            match this.TryUnsnoc with
            | None -> None
            | Some(q, x) -> Some(q :> _, x)

        member this.Update i y =
            this.Update i y :> _

        member this.TryUpdate i y =
            match this.TryUpdate i y with
            | None -> None
            | Some(q) -> Some(q :> _)

    interface IReadOnlyList<'T> with
        member this.Item
            with get i = this.Lookup i

        member this.Count = this.Length

        member this.GetEnumerator() =
            let e = seq {
                yield! front
                yield! (List.rev rBack)
            }

            e.GetEnumerator()

        member this.GetEnumerator() =
            (this :> _ seq).GetEnumerator() :> IEnumerator

[<RequireQualifiedAccess>]
module Deque =

    //pattern discriminators

    let (|Cons|Nil|)(q: Deque<'T>) =
        match q.TryUncons with
        | Some(a, b) -> Cons(a, b)
        | None -> Nil

    let (|Snoc|Nil|)(q: Deque<'T>) =
        match q.TryUnsnoc with
        | Some(a, b) -> Snoc(a, b)
        | None -> Nil

    ///O(n), worst case. Returns a new deque with the element added to the beginning.
    let inline cons (x: 'T) (q: Deque<'T>) = q.Cons x

    ///O(1). Returns deque of no elements.
    let empty() =
        Deque.Empty()

    ///O(1) amortized, O(n), worst case. Returns the first element.
    let inline head(q: Deque<'T>) = q.Head

    ///O(1) amortized, O(n), worst case. Returns option first element.
    let inline tryGetHead(q: Deque<'T>) =
        q.TryGetHead

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements before the last element.
    let inline init(q: Deque<'T>) = q.Init

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements before the last element.
    let inline tryGetInit(q: Deque<'T>) =
        q.TryGetInit

    ///O(1). Returns true if the deque has no elements.
    let inline isEmpty(q: Deque<'T>) = q.IsEmpty

    ///O(1) amortized, O(n), worst case. Returns the last element.
    let inline last(q: Deque<'T>) = q.Last

    ///O(1) amortized, O(n), worst case. Returns option last element.
    let inline tryGetLast(q: Deque<'T>) =
        q.TryGetLast

    ///O(1). Returns the count of elements.
    let inline length(q: Deque<'T>) = q.Length

    ///O(n), worst case. Returns element by index.
    let inline lookup i (q: Deque<'T>) =
        q.Lookup i

    ///O(n), worst case. Returns option element by index.
    let inline tryLookup i (q: Deque<'T>) =
        q.TryLookup i

    ///O(ys). Returns a deque of the two lists concatenated.
    let ofCatLists xs ys =
        Deque.OfCatLists xs ys

    ///O(n/2). Returns a deque of the seq.
    let ofSeq xs =
        Deque.OfSeq xs

    ///O(n), worst case. Returns deque with element removed by index.
    let inline remove i (q: Deque<'T>) =
        q.Remove i

    ///O(n), worst case. Returns option deque with element removed by index.
    let inline tryRemove i (q: Deque<'T>) =
        q.TryRemove i

    ///O(1). Returns deque reversed.
    let inline rev(q: Deque<'T>) = q.Rev

    ///O(1). Returns a deque of one element.
    let singleton x =
        Deque.Singleton x

    ///O(1) amortized, O(n), worst case. Returns a new deque with the element added to the end.
    let inline snoc (x: 'T) (q: Deque<'T>) =
        (q.Snoc x)

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements trailing the first element.
    let inline tail(q: Deque<'T>) = q.Tail

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements trailing the first element.
    let inline tryGetTail(q: Deque<'T>) =
        q.TryGetTail

    ///O(1) amortized, O(n), worst case. Returns the first element and tail.
    let inline uncons(q: Deque<'T>) = q.Uncons

    ///O(1) amortized, /O(n), worst case. Returns option first element and tail.
    let inline tryUncons(q: Deque<'T>) =
        q.TryUncons

    ///O(1) amortized, O(n), worst case. Returns init and the last element.
    let inline unsnoc(q: Deque<'T>) = q.Unsnoc

    ///O(1) amortized, O(n), worst case. Returns option init and the last element.
    let inline tryUnsnoc(q: Deque<'T>) =
        q.TryUnsnoc

    ///O(n), worst case. Returns deque with element updated by index.
    let inline update i y (q: Deque<'T>) =
        q.Update i y

    ///O(n), worst case. Returns option deque with element updated by index.
    let inline tryUpdate i y (q: Deque<'T>) =
        q.TryUpdate i y

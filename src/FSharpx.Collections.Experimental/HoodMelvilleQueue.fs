﻿//originally published by Julien
// original implementation taken from http://lepensemoi.free.fr/index.php/2010/01/21/hood-melville-queue

//jf -- added ofSeq, ofList, Uncons, and try...
//pattern discriminators Snoc and Nil

namespace FSharpx.Collections.Experimental

open System.Collections
open System.Collections.Generic
open FSharpx.Collections

type RotationState<'T> =
    | Idle
    | Reversing of int * list<'T> * list<'T> * list<'T> * list<'T>
    | Appending of int *list<'T> * list<'T>
    | Done of list<'T>

type HoodMelvilleQueue<'T> (frontLength : int, front : list<'T>, state : RotationState<'T>, rBackLength : int, rBack : list<'T>) = 

    member private this.frontLength = frontLength

    member internal this.front = front

    member private this.state = state

    member private this.rBackLength = rBackLength

    member internal this.rBack = rBack

    static member private exec : RotationState<'T> -> RotationState<'T> = function
        | Reversing(ok, x::f, f', y::r, r') -> Reversing(ok+1, f, x::f', r, y::r')
        | Reversing(ok, [], f', [y], r') -> Appending(ok, f', y::r')
        | Appending(0, f', r') -> Done r'
        | Appending(ok, x::f', r') -> Appending(ok-1, f', x::r')
        | state -> state

    static member private invalidate : RotationState<'T> -> RotationState<'T> = function
        | Reversing(ok, f, f', r, r') -> Reversing(ok-1, f, f', r, r')
        | Appending(0, f', x::r') -> Done r'
        | Appending(ok, f', r') -> Appending(ok-1, f', r')
        | state -> state

    static member private exec2 (q : HoodMelvilleQueue<'T>) =
        match (HoodMelvilleQueue.exec (HoodMelvilleQueue.exec q.state)) with
        | Done newf -> HoodMelvilleQueue(q.frontLength , newf, Idle, q.rBackLength, q.rBack) 
        | newstate -> HoodMelvilleQueue(q.frontLength , q.front, newstate, q.rBackLength, q.rBack) 

    static member private check (q : HoodMelvilleQueue<'T>) =
        if q.rBackLength <= q.frontLength then
          HoodMelvilleQueue.exec2 q
        else
          let newstate = Reversing(0, q.front, [], q.rBack, [])
          HoodMelvilleQueue((q.frontLength + q.rBackLength), q.front, newstate, 0, []) |> HoodMelvilleQueue.exec2

    static member internal readyQ (q : HoodMelvilleQueue<'T>) = 
        let rec loop (q' : HoodMelvilleQueue<'T>) =
            match q'.state with
            | Done _ | Idle -> q'
            | _ -> loop (HoodMelvilleQueue.check q')
        loop q

    static member internal Empty() : HoodMelvilleQueue<'T> = HoodMelvilleQueue(0, [], Idle, 0, [])

    static member internal fold (f : ('State -> 'T -> 'State)) (state : 'State) (q : HoodMelvilleQueue<'T>)  :  'State =     
        let q' = HoodMelvilleQueue.readyQ q
        let s = List.fold f state q'.front
        List.fold f s (List.rev q'.rBack)

    static member internal foldBack (f : ('T -> 'State -> 'State)) (q : HoodMelvilleQueue<'T>) (state : 'State) :  'State = 
        let q' = HoodMelvilleQueue.readyQ q
        let s = List.foldBack f (List.rev q'.rBack) state 
        List.foldBack f q'.front s

    static member internal OfList (xs:list<'T>) = HoodMelvilleQueue<'T>(xs.Length, xs, Idle, 0, [])

    static member internal OfSeq (xs:seq<'T>) = 
        HoodMelvilleQueue<'T>((Seq.length xs), (List.ofSeq xs), Idle, 0, [])
   
    ///returns the first element
    member this.Head  =
        match front with
        | hd::_ -> hd
        | _ -> raise Exceptions.Empty

    ///returns option first element
    member this.TryGetHead =
        match front with
        | hd::_ -> Some(hd)
        | _ -> None
         
    ///returns true if the queue has no elements
    member this.IsEmpty = (frontLength = 0)

    ///returns the count of elements
    member this.Length = frontLength + rBackLength

    ///returns a new queue with the element added to the end
    member this.Snoc x = 
        HoodMelvilleQueue<'T>(frontLength, front, state, (rBackLength + 1), (x::rBack))
        |> HoodMelvilleQueue.check

    ///returns a new queue of the elements trailing the first element
    member this.Tail =
        match front with
        | hd::tl ->
            HoodMelvilleQueue<'T>((frontLength-1), tl, (HoodMelvilleQueue.invalidate state), rBackLength, rBack)
            |> HoodMelvilleQueue.check
        | _ -> raise Exceptions.Empty

    ///returns option queue of the elements trailing the first element
    member this.TryGetTail =
        match front with
        | hd::tl ->
            Some(HoodMelvilleQueue<'T>((frontLength-1), tl, (HoodMelvilleQueue.invalidate state), rBackLength, rBack)
            |> HoodMelvilleQueue.check)
        | _ -> None

    ///returns the first element and tail
    member this.Uncons =  
        match front with
        | hd::tl ->
            hd, (HoodMelvilleQueue<'T>((frontLength-1), tl, (HoodMelvilleQueue.invalidate state), rBackLength, rBack) |> HoodMelvilleQueue.check)
        | _ -> raise Exceptions.Empty

    ///returns option first element and tail
    member this.TryUncons =  
        match front with
        | hd::tl ->
            Some(hd, (HoodMelvilleQueue<'T>((frontLength-1), tl, (HoodMelvilleQueue.invalidate state), rBackLength, rBack)
            |> HoodMelvilleQueue.check))
        | _ -> None

    with
    interface IQueue<'T> with

        member this.Count() = this.Length

        member this.Head = this.Head

        member this.TryGetHead = this.TryGetHead

        member this.IsEmpty = this.IsEmpty

        member this.Length() = this.Length

        member this.Snoc x = this.Snoc x :> _

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
          
    interface IReadOnlyCollection<'T> with
        member this.Count = this.Length

        member this.GetEnumerator() = 
            let e = 
                let q = HoodMelvilleQueue.readyQ this
                
                seq {
                    yield! q.front
                    yield! (List.rev q.rBack)  }
                   
            e.GetEnumerator()

        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> IEnumerator

[<RequireQualifiedAccess>]
module HoodMelvilleQueue =
    //pattern discriminators
    let (|Cons|Nil|) (q : HoodMelvilleQueue<'T>) = match q.TryUncons with Some(a,b) -> Cons(a,b) | None -> Nil

    ///returns queue of no elements
    let empty() = HoodMelvilleQueue.Empty()

    ///applies a function to each element of the queue, threading an accumulator argument through the computation, left to right
    let fold (f : ('State -> 'T -> 'State)) (state : 'State) (q : HoodMelvilleQueue<'T>) = HoodMelvilleQueue<_>.fold f state q

    ///applies a function to each element of the queue, threading an accumulator argument through the computation, right to left
    let foldBack (f : ('T -> 'State -> 'State)) (q : HoodMelvilleQueue<'T>) (state : 'State) =  HoodMelvilleQueue<_>.foldBack f q state

    ///returns the first element
    let inline head (q : HoodMelvilleQueue<'T>) = q.Head

    ///returns option first element
    let inline tryGetHead (q : HoodMelvilleQueue<'T>) = q.TryGetHead

    ///returns true if the queue has no elements

    let inline isEmpty (q : HoodMelvilleQueue<'T>) = q.IsEmpty

    ///returns the count of elements
    let inline length (q : HoodMelvilleQueue<'T>) = q.Length

    ///returns a queue of the list
    let ofList xs = HoodMelvilleQueue.OfSeq xs

    ///returns a queue of the seq
    let ofSeq xs = HoodMelvilleQueue.OfSeq xs

    ///returns a new queue with the element added to the end
    let inline snoc (x : 'T) (q : HoodMelvilleQueue<'T>) = (q.Snoc x) 

    ///returns a new queue of the elements trailing the first element
    let inline tail (q : HoodMelvilleQueue<'T>) = q.Tail 

    ///returns option queue of the elements trailing the first element
    let inline tryGetTail (q : HoodMelvilleQueue<'T>) = q.TryGetTail 

    ///returns the first element and tail
    let inline uncons (q : HoodMelvilleQueue<'T>) = q.Uncons

    ///returns option first element and tail
    let inline tryUncons (q : HoodMelvilleQueue<'T>) = q.TryUncons
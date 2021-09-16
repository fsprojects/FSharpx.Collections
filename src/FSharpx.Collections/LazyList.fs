// First version copied from the F# Power Pack
// https://raw.github.com/fsharp/powerpack/master/src/FSharp.PowerPack/LazyList.fs

// (c) Microsoft Corporation 2005-2009.

namespace FSharpx.Collections

open System
open System.Collections.Generic

#nowarn "40" // recursive initialization

exception UndefinedException

[<NoEquality; NoComparison>]
type LazyList<'T> =
    { status : Lazy<LazyListCell<'T>> }

    member x.Value = x.status.Value

    static member inline getCell (x : LazyList<'T>) = x.Value

    member this.IsEmpty =
      match this.Value with
      | CellCons _ -> false
      | CellEmpty -> true

    member this.Head =
      match this.Value with
      | CellCons(a,_) -> a
      | CellEmpty -> invalidArg "s" "the list is empty"

    member this.TryHead =
      match this.Value with
      | CellCons(a,_) -> Some a
      | CellEmpty -> None

    member this.Length() =
        let rec lengthAux n s =
          match LazyList.getCell s with
          | CellEmpty -> n
          | CellCons(_,b) -> lengthAux (n+1) b

        lengthAux 0 this

    member this.Tail =
      match this.Value with
      | CellCons(_,b) -> b
      | CellEmpty -> invalidArg "s" "the list is empty"

    member this.TryTail =
      match this.Value with
      | CellCons(_,b) -> Some b
      | CellEmpty -> None

    member this.Uncons =
        match this.Value with
        | CellCons (a,b) -> a,b
        | CellEmpty -> invalidArg "x" "the list does not contain head and tail"

    member this.TryUncons =
        match this.Value with
        | CellCons (a,b) -> Some(a,b)
        | CellEmpty -> None

    member s.GetEnumeratorImpl() =
        let getCell (x : LazyList<'T>) = x.Value
        let toSeq s = Seq.unfold (fun ll -> match getCell ll with CellEmpty -> None | CellCons(a,b) -> Some(a,b)) s
        (toSeq s).GetEnumerator()

    interface IEnumerable<'T> with
        member this.GetEnumerator() = this.GetEnumeratorImpl()

    interface System.Collections.IEnumerable with
        member this.GetEnumerator() = this.GetEnumeratorImpl() :> System.Collections.IEnumerator

    interface IReadOnlyCollection<'T> with
        member this.Count = this.Length()

and
    [<NoEquality; NoComparison>]
    LazyListCell<'T> =
    | CellCons of 'T * LazyList<'T>
    | CellEmpty

[<RequireQualifiedAccess>]
module LazyList =

    let lzy f = { status = Lazy<_>.Create f }
    let force (x: LazyList<'T>) = x.Value

    let notlazy v = { status = Lazy<_>.CreateFromValue v }

    let getCell (x : LazyList<'T>) = force x
    let empty<'T> : LazyList<'T> = notlazy CellEmpty
    let consc x l = CellCons(x,l)
    let cons x l = lzy(fun () -> (consc x l))
    let consDelayed x l = lzy(fun () -> (consc x (lzy(fun () ->  (force (l()))))))

    let uncons (s : LazyList<'T>) = s.Uncons

    let tryUncons (s : LazyList<'T>) = s.TryUncons

    let rec unfold f z =
      lzy(fun () ->
          match f z with
          | None       -> CellEmpty
          | Some (x,z) -> CellCons (x,unfold f z))

    let rec append l1  l2 = lzy(fun () ->  (appendc l1 l2))
    and appendc l1 l2 =
      match getCell l1 with
      | CellEmpty -> force l2
      | CellCons(a,b) -> consc a (append b l2)

    let delayed f = lzy(fun () ->  (getCell (f())))
    let repeat x =
      let rec s = cons x (delayed (fun () -> s)) in s

    let rec map f s =
      lzy(fun () ->
        match getCell s with
        | CellEmpty -> CellEmpty
        | CellCons(a,b) -> consc (f a) (map f b))

    let rec map2 f s1 s2 =
      lzy(fun () ->
        match getCell s1, getCell s2  with
        | CellCons(a1,b1),CellCons(a2,b2) -> consc (f a1 a2) (map2 f b1 b2)
        | _ -> CellEmpty)

    let rec zip s1 s2 =
      lzy(fun () ->
        match getCell s1, getCell s2  with
        | CellCons(a1,b1),CellCons(a2,b2) -> consc (a1,a2) (zip b1 b2)
        | _ -> CellEmpty)

    let rec concat s1 =
      lzy(fun () ->
        match getCell s1 with
        | CellCons(a,b) -> appendc a (concat b)
        | CellEmpty -> CellEmpty)

    let rec filter p s1= lzy(fun () ->  filterc p s1)
    and filterc p s1 =
        match getCell s1 with
        | CellCons(a,b) -> if p a then consc a (filter p b) else filterc p b
        | CellEmpty -> CellEmpty

    let rec tryFind p s1 =
        match getCell s1 with
        | CellCons(a,b) -> if p a then Some a else tryFind p b
        | CellEmpty -> None

    let indexNotFound() = raise (new System.Collections.Generic.KeyNotFoundException("An index satisfying the predicate was not found in the collection"))

    let find p s1 =
        match tryFind p s1 with
        | Some a -> a
        | None   -> indexNotFound()

    let rec scan f acc s1 =
      lzy(fun () ->
        match getCell s1 with
        | CellCons(a,b) -> let acc' = f acc a in consc acc (scan f acc' b)
        | CellEmpty -> consc acc empty)

    let head (s : LazyList<'T>) = s.Head

    let tryHead (s : LazyList<'T>) = s.TryHead

    let tail (s : LazyList<'T>) = s.Tail

    let tryTail (s : LazyList<'T>) = s.TryTail

    let isEmpty (s : LazyList<'T>) = s.IsEmpty

    let rec take n s =
      lzy(fun () ->
        if n < 0 then invalidArg "n" "the number must not be negative"
        elif n = 0 then CellEmpty
        else
          match getCell s with
          | CellCons(a,s) -> consc a (take (n-1) s)
          | CellEmpty -> invalidArg "n" "not enough items in the list" )

    let rec tryTake n s =
        if n < 0 then None
        elif n = 0 then Some empty
        else
            match getCell s with
            | CellCons(a,s) -> Some (consDelayed a ( fun () -> match (tryTake (n-1) s) with Some x -> x | None -> empty ) )
            | CellEmpty -> None

    let rec skipc n s =
      if n = 0 then force s
      else
        match getCell s with
        | CellCons(_,s) -> skipc (n-1) s
        | CellEmpty -> invalidArg "n" "not enough items in the list"

    let rec skip n s =
      lzy(fun () ->
        if n < 0 then invalidArg "n" "the value must not be negative"
        else skipc n s)

    let rec skipcOpt n s =
      if n = 0 then Some s
      else
        match getCell s with
        | CellCons(_,s) -> match (skipcOpt (n-1) s) with Some x -> Some x | None -> None
        | CellEmpty -> None

    let rec trySkip n s =
        if n < 0 then None
        else skipcOpt n s

    let fold f s l =
        let rec loop s l =
            match  getCell l with
            | CellEmpty -> s
            | CellCons(x,xs) ->
                let s = f s x
                loop s xs
        loop s l

    let mapAccum f s l =
        let rec loop s l cont =
            match  getCell l with
            | CellEmpty -> cont (s, empty)
            | CellCons(x,xs) ->
                let s, y = f s x
                loop s xs (fun (s,ys) -> cont (s, cons y ys))
        loop s l id

    let rec ofList l =
      lzy(fun () ->
        match l with [] -> CellEmpty | h :: t -> consc h (ofList t))

    let toList s =
      let rec loop s acc =
          match getCell s with
          | CellEmpty -> List.rev acc
          | CellCons(h,t) -> loop t (h::acc)
      loop s []

    let rec iter f s =
      match getCell s with
      | CellEmpty -> ()
      | CellCons(h,t) -> f h; iter f t

    let rec copyFrom i a =
      lzy(fun () ->
        if i >= Array.length a then CellEmpty
        else consc a.[i] (copyFrom (i+1) a))

    let rec copyTo (arr: _[]) s i =
      match getCell s with
      | CellEmpty -> ()
      | CellCons(a,b) -> arr.[i] <- a; copyTo arr b (i+1)

    let ofArray a = copyFrom 0 a
    let toArray s = Array.ofList (toList s)

    let rec lengthAux n s =
      match getCell s with
      | CellEmpty -> n
      | CellCons(_,b) -> lengthAux (n+1) b

    let length (s : LazyList<'T>) = s.Length()

    let toSeq (s: LazyList<'T>) = (s :> IEnumerable<_>)

    // Note: this doesn't dispose of the IEnumerator if the iteration is not run to the end
    let rec ofFreshIEnumerator (e : IEnumerator<_>) =
      lzy(fun () ->
        if e.MoveNext() then
          consc e.Current (ofFreshIEnumerator e)
        else
           e.Dispose()
           CellEmpty)

    let ofSeq (c : IEnumerable<_>) =
      ofFreshIEnumerator (c.GetEnumerator())

    let (|Cons|Nil|) l = match getCell l with CellCons(a,b) -> Cons(a,b) | CellEmpty -> Nil

    let rev r =
        let rec revAux r acc =
            match r with
            | Nil -> acc
            | Cons(hd, tl) -> revAux tl (cons hd acc)
        revAux r empty

    let rec drop n xs =
        if n < 0 then invalidArg "n" "n was negative"
        elif n > 0 then
            match xs with
            | Cons(_, xs') -> drop (n-1) xs'
            | Nil -> empty
        else
            xs

    let split (ll:LazyList<'T>) n  =
        let rec loop z (leftL:'T list) (ll':LazyList<'T>) =
            match z with
            | 0 -> leftL, (ll'.Tail)
            | _ -> loop (z - 1)  ((ll'.Head)::leftL) (ll'.Tail)
        loop n [] ll

    let rec compareWith (comparer: 'a -> 'a -> _) source1 source2 =
        match source1, source2 with
        | Nil, Nil -> 0
        | Cons (x1, xs1), Cons (x2, xs2) ->
            match comparer x1 x2 with
            | 0 -> compareWith comparer xs1 xs2
            | x -> x
        | Cons _, Nil -> 1
        | Nil, Cons _ -> -1

    let rec equalsWith (fEquality: 'a -> 'a -> _) source1 source2 =
        match source1, source2 with
        | Nil, Nil -> true
        | Cons (x1, xs1), Cons (x2, xs2) ->
            match fEquality x1 x2 with
            | true -> equalsWith fEquality xs1 xs2
            | false -> false
        | Cons _, Nil | Nil, Cons _ -> false
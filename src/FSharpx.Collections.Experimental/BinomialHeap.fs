﻿// originally published by Julien
// original implementation taken http://lepensemoi.free.fr/index.php/2009/12/10/binomial-heap

//J.F. modified

namespace FSharpx.Collections.Experimental

open System.Collections
open System.Collections.Generic
open FSharpx.Collections

type BinomialTree<'T> = Node of (int * 'T * list<BinomialTree<'T>>)

type BinomialHeap<'T when 'T : comparison> (isDescending : bool, heap : list<BinomialTree<'T>>) =

    member private this.heap = heap

    ///O(1). Returns true if the heap has max element at head.
    member this.IsDescending = isDescending

    ///O(log n). Returns the count of elements.
    member this.Length() = List.fold (fun acc (Node(r, _, _)) -> acc + int (2.0**(float r))) 0 this.heap

    static member internal  empty isDescending = BinomialHeap<'T>(isDescending, [])

    static member private  isEmpty : BinomialHeap<'T> -> bool = function 
        | h when h.heap.IsEmpty -> true 
        | _ -> false

    static member private  rank ((Node(r, _, _)) : BinomialTree<'T>) = r

    static member private  root ((Node(_, x, _)) : BinomialTree<'T>) = x

    static member private  link isDescending (Node(r, x1, xs1) as n1) (Node(_, x2, xs2) as n2) : BinomialTree<'T> =
        if x1 <= x2 then
            if isDescending then Node(r+1, x2, n1 :: xs2)
            else Node(r+1, x1, n2 :: xs1)
        else
            if isDescending then Node(r+1, x1, n2 :: xs1)
            else Node(r+1, x2, n1 :: xs2)

    static member private insertTree (isDescending : bool) (t : BinomialTree<'T>) (h : list<BinomialTree<'T>>) : list<BinomialTree<'T>> = 
        match h with
        | [] -> [t]
        | hd::tl as t' ->
            if BinomialHeap.rank t < BinomialHeap.rank hd then t::t'
            else BinomialHeap.insertTree isDescending (BinomialHeap.link isDescending t hd) tl

    static member private  insert isDescending x (h : list<BinomialTree<'T>>) =
        BinomialHeap.insertTree isDescending (Node(0, x, [])) h

    static member private merge isDescending (h1  : list<BinomialTree<'T>>) (h2  : list<BinomialTree<'T>>)  : list<BinomialTree<'T>> =
        match h1, h2 with
        | [], x -> x
        | x, [] -> x
        | (hd1::tl1), (hd2::tl2) ->
          if BinomialHeap.rank hd1 < BinomialHeap.rank hd2 
          then hd1 :: BinomialHeap.merge isDescending tl1 h2
          elif BinomialHeap.rank hd1 > BinomialHeap.rank hd2 then hd2 :: BinomialHeap.merge isDescending h1 tl2
          else BinomialHeap.insertTree isDescending (BinomialHeap.link isDescending hd1 hd2) (BinomialHeap.merge isDescending ( tl1) ( tl2))

    static member private removeHeadTree : bool * list<BinomialTree<'T>> -> (BinomialTree<'T> * list<BinomialTree<'T>>) = function
        | _, [] -> raise Exceptions.Empty
        | _, [t] -> t, []
        | isDescending, hd::tl ->
            let hd', tl'= BinomialHeap.removeHeadTree (isDescending, tl)
            if isDescending then
                if BinomialHeap.root hd > BinomialHeap.root hd' 
                then hd, tl 
                else hd', hd::tl'
            else
                if BinomialHeap.root hd <= BinomialHeap.root hd' 
                then hd, tl 
                else hd', hd::tl'

    static member private head isDescending (h : list<BinomialTree<'T>>) : 'T =
        let (Node(_, x, xs1), _) = BinomialHeap.removeHeadTree (isDescending, h)
        x

    static member internal ofSeq (descending: bool) (s:seq<'T>) : BinomialHeap<'T> = 
        if Seq.isEmpty s then BinomialHeap.empty descending
        else
            let x = 
                Seq.fold (fun acc elem -> [Node(0, elem, [])]::acc) [] s
    
            let pairWiseMerge (l: list<list<BinomialTree<'T>>>) =
                let rec loop (acc: list<list<BinomialTree<'T>>>) : list<list<BinomialTree<'T>>> -> list<list<BinomialTree<'T>>> = function
                    | h1::h2::tl -> loop ((BinomialHeap.merge descending h1 h2)::acc) tl
                    | [ h1 ] -> h1::acc
                    | [] -> acc

                loop [] l

            let rec loop : list<list<BinomialTree<'T>>> -> list<BinomialTree<'T>> = function
                | [ h ] -> h
                | x -> loop (pairWiseMerge x)
                
            BinomialHeap<'T>(descending, (loop x)) 

    static member private tail isDescending (h : list<BinomialTree<'T>>) =
        let Node(_, x, xs1), xs2 = BinomialHeap.removeHeadTree (isDescending, h)
        BinomialHeap.merge isDescending (List.rev xs1) xs2

    static member private uncons isDescending (h : list<BinomialTree<'T>>) : 'T * BinomialHeap<'T> =
        let Node(_, x, xs1), xs2 = BinomialHeap.removeHeadTree (isDescending, h)
        x, BinomialHeap<'T>(isDescending, (BinomialHeap.merge isDescending (List.rev xs1) xs2))
    
    static member private foldHeap nodeF leafV (h : list<BinomialTree<'T>>) = 

        let rec loop (h : list<BinomialTree<'T>>) cont =
            match h with
            | Node(_, a, h')::tl -> loop h'  (fun lacc ->  
                                    loop tl (fun racc -> 
                                    cont (nodeF a lacc racc))) 
            | _ -> cont leafV
           
        loop h (fun x -> x)

    static member private inOrder (h : list<BinomialTree<'T>>) = (BinomialHeap.foldHeap (fun x l r acc -> l (x :: (r acc))) (fun acc -> acc) h) [] 

    ///O(log n). Returns the min or max element.
    member this.Head() = BinomialHeap.head this.IsDescending this.heap

    ///O(log n). Returns option first min or max element.
    member this.TryGetHead() = 
        if this.heap.IsEmpty then None
        else Some(BinomialHeap.head this.IsDescending this.heap)

    ///O(log n). Returns a new heap with the element inserted.
    member this.Insert x  = BinomialHeap<'T>(this.IsDescending, (BinomialHeap.insert this.IsDescending x this.heap))

    ///O(1). Returns true if the heap has no elements.
    member this.IsEmpty = BinomialHeap.isEmpty this

    ///O(log n). Returns heap from merging two heaps, both must have same isDescending.
    member this.Merge (xs : BinomialHeap<'T>) : BinomialHeap<'T> = 
        if (this.IsDescending = xs.IsDescending) then
            BinomialHeap<'T>(this.IsDescending, (BinomialHeap.merge this.IsDescending this.heap xs.heap))
        else failwith "not same max or min"

    ///O(log n). Returns heap option from merging two heaps.
    member this.TryMerge (xs : BinomialHeap<'T>) = 
        if (this.IsDescending <> xs.IsDescending) then None
        else Some(BinomialHeap<'T>(this.IsDescending, (BinomialHeap.merge this.IsDescending this.heap xs.heap)))

    ///O(log n). Returns a new heap of the elements trailing the head.
    member this.Tail() = 
        BinomialHeap<'T>(this.IsDescending, (BinomialHeap.tail this.IsDescending this.heap))
       
    ///O(log n). Returns option heap of the elements trailing the head.
    member this.TryGetTail() = 
        if this.heap.IsEmpty then None
        else Some(BinomialHeap<'T>(this.IsDescending, (BinomialHeap.tail this.IsDescending this.heap)))

    ///O(log n). Returns the head element and tail.
    member this.Uncons() = BinomialHeap.uncons this.IsDescending this.heap

    ///O(log n). Returns option head element and tail.
    member this.TryUncons() = 
        if this.heap.IsEmpty then None
        else Some(BinomialHeap.uncons this.IsDescending this.heap)

    interface IReadOnlyCollection<'T> with
        member this.Count = this.Length()
        member this.GetEnumerator() = 
            let e = 
                if this.IsDescending
//WARNING! List.sort |> List.rev significantly faster (caveat: on 32-bit Win 7) than List.sortwith...go figure!
//            BinomialHeap.inOrder this |> List.sortWith (fun x y -> if (x > y) then -1
//                                                                             else 
//                                                                               if (x = y) then 0
//                                                                               else 1) |> List.fold f state
                then BinomialHeap.inOrder this.heap |> List.sort |> List.rev |> List.toSeq
                else BinomialHeap.inOrder this.heap |> List.sort |> List.toSeq

            e.GetEnumerator()

        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> IEnumerator  

    interface IHeap<BinomialHeap<'T>, 'T> with
        
        member this.Count() = this.Length()

        member this.Head() = BinomialHeap.head this.IsDescending this.heap

        member this.TryGetHead() = 
            if this.heap.IsEmpty then None
            else Some(BinomialHeap.head this.IsDescending this.heap)

        member this.Insert (x : 'T) = BinomialHeap<'T>(this.IsDescending, (BinomialHeap.insert this.IsDescending x this.heap))

        member this.IsEmpty = BinomialHeap.isEmpty this

        member this.IsDescending = this.IsDescending 

        member this.Length() = this.Length() 

        member this.Merge (xs : BinomialHeap<'T>) = 
            if (this.IsDescending = xs.IsDescending) then
                BinomialHeap<'T>(this.IsDescending, (BinomialHeap.merge this.IsDescending this.heap xs.heap))
            else failwith "not same max or min"

        member this.TryMerge (xs : BinomialHeap<'T>)  = 
            if (this.IsDescending <> xs.IsDescending) then None
            else Some(BinomialHeap<'T>(this.IsDescending, (BinomialHeap.merge this.IsDescending this.heap xs.heap)))

        member this.Tail() = BinomialHeap<'T>(this.IsDescending, (BinomialHeap.tail this.IsDescending this.heap))

        member this.TryGetTail() =
            if this.heap.IsEmpty then None
            else Some(BinomialHeap<'T>(this.IsDescending, (BinomialHeap.tail this.IsDescending this.heap)))

        member this.Uncons() = BinomialHeap.uncons this.IsDescending this.heap

        member this.TryUncons() =
            if this.heap.IsEmpty then None
            else Some(BinomialHeap.uncons this.IsDescending this.heap)

    interface IPriorityQueue<'T> with
        member this.IsEmpty = this.IsEmpty
        member this.Insert element = this.Insert element :> IPriorityQueue<'T>
        member this.TryPeek() = this.TryGetHead()
        member this.Peek() = this.Head()
        member this.Length = this.Length()

        member this.TryPop() = 
            match this.TryUncons() with
            | Some(element,newHeap) -> Some(element,newHeap  :> IPriorityQueue<'T>)
            | None -> None

        member this.Pop() = 
            let element,newHeap = this.Uncons()
            element,(newHeap  :> IPriorityQueue<'T>)

[<RequireQualifiedAccess>]
module BinomialHeap = 
    //pattern discriminator
    let (|Cons|Nil|) (h: BinomialHeap<'T>) = match h.TryUncons() with Some(a,b) -> Cons(a,b) | None -> Nil
  
    ///O(1). Returns a empty heap.
    let empty (descending: bool) = BinomialHeap.empty descending

    ///O(log n). Returns the min or max element.
    let inline head (xs: BinomialHeap<'T>)  = xs.Head()

    ///O(log n). Returns option first min or max element.
    let inline tryGetHead (xs: BinomialHeap<'T>)  = xs.TryGetHead()

    ///O(log n). Returns a new heap with the element inserted.
    let inline insert x (xs: BinomialHeap<'T>) = xs.Insert x   

    ///O(1). Returns true if the heap has no elements.
    let inline isEmpty (xs: BinomialHeap<'T>) = xs.IsEmpty

    ///O(1). Returns true if the heap has max element at head.
    let inline isDescending (xs: BinomialHeap<'T>) = xs.IsDescending

    ///O(log n). Returns the count of elements.
    let inline length (xs: BinomialHeap<'T>) = xs.Length() 

    ///O(log n) Returns heap from merging two heaps, both must have same isDescending.
    let inline merge (xs: BinomialHeap<'T>) (ys: BinomialHeap<'T>) = xs.Merge ys

    ///O(log n). Returns heap option from merging two heaps.
    let inline tryMerge (xs: BinomialHeap<'T>) (ys: BinomialHeap<'T>) = xs.TryMerge ys

    ///O(n * log n). Returns heap from the sequence.
    let ofSeq descending s = BinomialHeap.ofSeq descending s

    ///O(log n). Returns a new heap of the elements trailing the head.
    let inline tail (xs: BinomialHeap<'T>) = xs.Tail()

    ///O(log n). Returns option heap of the elements trailing the head
    let inline tryGetTail (xs: BinomialHeap<'T>) = xs.TryGetTail()

    /// O(log n). Returns the head element and tail.
    let inline uncons (xs: BinomialHeap<'T>) = xs.Uncons()

    /// O(log n). Returns option head element and tail.
    let inline tryUncons (xs: BinomialHeap<'T>) = xs.TryUncons()
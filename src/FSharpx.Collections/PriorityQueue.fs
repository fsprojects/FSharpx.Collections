// based on pairing heap published by Okasaki
// original implementation taken from http://lepensemoi.free.fr/index.php/2009/12/17/pairing-heap
//J.F. modified

namespace FSharpx.Collections


open System.Collections
open System.Collections.Generic

type IPriorityQueue<'T when 'T : comparison> =
    inherit System.Collections.IEnumerable
    inherit System.Collections.Generic.IEnumerable<'T>

    ///returns true if the queue has no elements
    abstract member IsEmpty : bool with get

    ///returns a new queue with the element added to the end
    abstract member Insert : 'T -> IPriorityQueue<'T>

    ///returns option first element
    abstract member TryPeek : unit -> 'T option

    ///returns the first element
    abstract member Peek : unit -> 'T

    ///returns the count of the elements
    abstract member Length : int

    //returns the option first element and tail
    abstract member TryPop : unit -> ('T * IPriorityQueue<'T>) option

    ///returns the first element and tail
    abstract member Pop : unit -> 'T * IPriorityQueue<'T> 

/// Heap is an ordered linear structure where the ordering is either ascending or descending. 
/// "head" inspects the first element in the ordering, "tail" takes the remaining structure 
/// after head, and "insert" places elements within the ordering. PriorityQueue is available 
/// as an alternate interface.
/// According to Okasaki the time complexity of the heap functions in this Heap implementation 
/// (based on the "pairing" heap) have "resisted" time complexity analysis. 
type Heap<'T when 'T : comparison>(isDescending : bool, length : int, data : HeapData<'T> ) =
    let mutable hashCode = None
    member internal this.heapData = data
    member internal this.heapLength = length

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
        | :? Heap<'T> as y -> 
            if this.Length <> y.Length then false 
            else
                if this.GetHashCode() <> y.GetHashCode() then false
                else Seq.forall2 (Unchecked.equals) this y
        | _ -> false

    static member private merge isDescending newLength (h1 : HeapData<'T>) (h2 : HeapData<'T>) : Heap<'T> = 
        match h1, h2 with
        | E, h -> Heap(isDescending, newLength, h)
        | h, E -> Heap(isDescending, newLength, h)
        | T(x, xs), T(y, ys) ->
            if isDescending then
                if x <= y then Heap(isDescending, newLength, T(y, h1::ys)) else Heap(isDescending, newLength, T(x, h2::xs))
            else 
                if x <= y then Heap(isDescending, newLength, T(x, h2::xs)) else Heap(isDescending, newLength, T(y, h1::ys))

    //http://lorgonblog.wordpress.com/2008/04/06/catamorphisms-part-two
    static member private foldHeap nodeF leafV (h : list<HeapData<'T>>) = 

        let rec loop (h : list<HeapData<'T>>) cont =
            match h with
            | T(a, h')::tl -> loop h'  (fun lacc ->  
                                    loop tl (fun racc -> 
                                    cont (nodeF a lacc racc))) 
            | _ -> cont leafV
        
        loop h (fun x -> x)

    static member private inOrder (h : list<HeapData<'T>>) = (Heap.foldHeap (fun x l r acc -> l (x :: (r acc))) (fun acc -> acc) h) [] 
 
    static member internal ofSeq (isDescending: bool) (s:seq<'T>) : Heap<'T> = 
        if Seq.isEmpty s then Heap(isDescending, 0, E)
        else
            let len, h' =
                 Seq.fold (fun (lnth, (h : 'T HeapData)) x -> 
                    match h with 
                    | E -> 1, T(x, [])
                    | T(y, ys) ->
                    if isDescending then
                        if x <= y then (lnth + 1), T(y, T(x, [])::ys) else (lnth + 1), T(x, T(y, ys)::[])
                    else 
                        if x <= y then (lnth + 1), T(x, T(y, ys)::[]) else (lnth + 1), T(y, T(x, [])::ys) ) (0,E) s
            Heap(isDescending, len, h')
    
    ///O(1) worst case. Returns the min or max element.   
    member this.Head = 
        match data with
        | E -> raise (new System.Exception("Heap is empty"))
        | T(x, _) -> x

    ///O(1) worst case. Returns option first min or max element.
    member this.TryHead = 
        match data with
        | E -> None
        | T(x, _) -> Some(x)

    ///O(log n) amortized time. Returns a new heap with the element inserted.
    member this.Insert x  = 
        Heap.merge isDescending (length + 1) (T(x, [])) data

    ///O(1). Returns true if the heap has no elements.
    member this.IsEmpty = 
        match data with
        | E -> true 
        | _ -> false

    ///O(1). Returns true if the heap has max element at head.
    member this.IsDescending = isDescending

    ///O(n). Returns the count of elememts.
    member this.Length = length

    ///O(log n) amortized time. Returns heap from merging two heaps, both must have same descending.
    member this.Merge (xs : Heap<'T>) = 
        if isDescending = xs.IsDescending then Heap.merge isDescending (length + xs.heapLength) data xs.heapData
        else failwith "heaps to merge have different sort orders"

    ///O(log n) amortized time. Returns heap option from merging two heaps.
    member this.TryMerge (xs : Heap<'T>) = 
        if isDescending = xs.IsDescending then Some(Heap.merge isDescending (length + xs.heapLength) data xs.heapData)
        else None

    ///O(n log n). Returns heap reversed.
    member this.Rev() = 
        if isDescending then Heap<'T>.ofSeq false (this :> seq<'T>)
        else  Heap<'T>.ofSeq true (this :> seq<'T>)

    ///O(log n) amortized time. Returns a new heap of the elements trailing the head.
    member this.Tail() =

        let mergeData (h1 : HeapData<'T>) (h2 : HeapData<'T>) : HeapData<'T> = 
            match h1, h2 with
            | E, h -> h
            | h, E -> h
            | T(x, xs), T(y, ys) ->
                if isDescending then
                    if x <= y then T(y, h1::ys) else T(x, h2::xs)
                else 
                    if x <= y then T(x, h2::xs) else T(y, h1::ys)

        match data with
        | E -> raise (new System.Exception("Heap is empty"))
        | T(x, xs) -> 
            let combinePairs state item =
                match state with
                | Some p, l ->
                    (None, (mergeData item p)::l)
                | None, l ->
                    (Some item, l)
            
            let tail = 
                xs
                |> List.fold combinePairs (None, [])
                |> function
                   | Some i, l -> i::l
                   | None, l -> l
                |> List.fold mergeData E
            
            Heap(isDescending, (length - 1), tail)
      
    ///O(log n) amortized time. Returns option heap of the elements trailing the head.
    member this.TryTail() =
        match data with
        | E -> None
        | _ -> Some (this.Tail())

    ///O(log n) amortized time. Returns the head element and tail.
    member this.Uncons() = 
        match data with
        | E -> raise (new System.Exception("Heap is empty"))
        | T(x, xs) -> x, this.Tail() 

    ///O(log n) amortized time. Returns option head element and tail.
    member this.TryUncons() = 
        match data with
        | E -> None
        | T(x, xs) -> Some(x, this.Tail())

    interface IEnumerable<'T> with

        member this.GetEnumerator() = 
            let e = 
                let listH = data::[]
                if isDescending
                then Heap.inOrder listH |> List.sort |> List.rev |> List.toSeq
                else Heap.inOrder listH |> List.sort |> List.toSeq

            e.GetEnumerator()

        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> IEnumerator

    interface IPriorityQueue<'T>

        with
        member this.IsEmpty = this.IsEmpty
        member this.Insert element = this.Insert element :> IPriorityQueue<'T>
        member this.TryPeek() = this.TryHead
        member this.Peek() = this.Head
        member this.Length = this.Length

        member this.TryPop() = 
            match this.TryUncons() with
            | Some(element,newHeap) -> Some(element,newHeap  :> IPriorityQueue<'T>)
            | None -> None

        member this.Pop() = 
            let element,newHeap = this.Uncons()
            element,(newHeap  :> IPriorityQueue<'T>)

and HeapData<'T when 'T : comparison> =
    | E 
    | T of 'T * list<HeapData<'T>>

module Heap =   
    //pattern discriminator

    let (|Cons|Nil|) (h: Heap<'T>) = match h.TryUncons() with Some(a,b) -> Cons(a,b) | None -> Nil
  
    ///O(1). Returns a empty heap.
    let empty<'T when 'T : comparison> (isDescending: bool) = Heap<'T>(isDescending, 0, E)

    ///O(1) worst case. Returns the min or max element.
    let inline head (xs: Heap<'T>)  = xs.Head

    ///O(1) worst case. Returns option first min or max element.
    let inline tryHead (xs: Heap<'T>)  = xs.TryHead

    ///O(log n) amortized time. Returns a new heap with the element inserted.
    let inline insert x (xs: Heap<'T>) = xs.Insert x   

    ///O(1). Returns true if the heap has no elements.
    let inline isEmpty (xs: Heap<'T>) = xs.IsEmpty

    ///O(1). Returns true if the heap has max element at head.
    let inline isDescending (xs: Heap<'T>) = xs.IsDescending

    ///O(n). Returns the count of elememts.
    let inline length (xs: Heap<'T>) = xs.Length 

    ///O(log n) amortized time. Returns heap from merging two heaps, both must have same descending.
    let inline merge (xs: Heap<'T>) (ys: Heap<'T>) = xs.Merge ys

    ///O(log n) amortized time. Returns heap option from merging two heaps.
    let inline tryMerge (xs: Heap<'T>) (ys: Heap<'T>) = xs.TryMerge ys

    ///O(n log n). Returns heap, bool isDescending, from the sequence.
    let ofSeq isDescending s  = Heap<'T>.ofSeq isDescending s 
    
    ///O(n). Returns heap reversed.
    let inline rev (xs: Heap<'T>) = xs.Rev()

    ///O(log n) amortized time. Returns a new heap of the elements trailing the head.
    let inline tail (xs: Heap<'T>) = xs.Tail()

    ///O(log n) amortized time. Returns option heap of the elements trailing the head.
    let inline tryTail (xs: Heap<'T>) = xs.TryTail()

    ///O(n). Views the given heap as a sequence.
    let inline toSeq (xs: Heap<'T>) = xs :> seq<'T>

    ///O(log n) amortized time. Returns the head element and tail.
    let inline uncons (xs: Heap<'T>) = xs.Uncons()

    ///O(log n) amortized time. Returns option head element and tail.
    let inline tryUncons (xs: Heap<'T>) = xs.TryUncons()

module PriorityQueue =
    ///O(1). Returns a empty queue, with indicated ordering.
    let empty<'T when 'T : comparison> isDescending = Heap.empty isDescending :> IPriorityQueue<'T>

    ///O(1). Returns true if the queue has no elements.
    let inline isEmpty (pq:IPriorityQueue<'T>) = pq.IsEmpty

    ///O(log n) amortized time. Returns a new queue with the element added to the end.
    let inline insert element (pq:IPriorityQueue<'T>) = pq.Insert element

    ///O(1). Returns option first element.
    let inline tryPeek (pq:IPriorityQueue<'T>) = pq.TryPeek()

    ///O(1). Returns the first element.
    let inline peek (pq:IPriorityQueue<'T>) = pq.Peek()

    ///O(log n) amortized time. Returns the option first element and tail.
    let inline tryPop (pq:IPriorityQueue<'T>) = pq.TryPop()

    ///O(log n) amortized time. Returns the first element and tail.
    let inline pop (pq:IPriorityQueue<'T>) = pq.Pop()

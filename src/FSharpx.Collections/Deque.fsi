namespace FSharpx.Collections

/// Double-ended queue is an ordered linear linear structure implementing the signature of List
/// (head, tail, cons) as well as the mirror-image Vector signature (last, initial, conj). "head" inspects
/// the first or left-most element in the structure, while "last" inspects the last or
/// right-most element. "rev" (reverse) has time complexity O(1). Ordering is by insertion history.
[<Class>]
type Deque<'T> =
    interface System.IEquatable<Deque<'T>>
    interface System.Collections.IEnumerable
    interface System.Collections.Generic.IEnumerable<'T>
    interface System.Collections.Generic.IReadOnlyCollection<'T>

    ///O(1). Returns a new deque with the element added to the end.
    member Conj: 'T -> Deque<'T>

    ///O(1). Returns a new deque with the element added to the beginning.
    member Cons: 'T -> Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns the first element.
    member Head: 'T

    ///O(1) amortized, O(n), worst case. Returns option first element.
    member TryHead: 'T option

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements before the last element.
    member Initial: Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements before the last element.
    member TryInitial: Deque<'T> option

    ///O(1). Returns true if the deque has no elements.
    member IsEmpty: bool

    ///O(1) amortized, O(n), worst case. Returns the last element.
    member Last: 'T

    ///O(1) amortized, O(n), worst case. Returns option last element.
    member TryLast: 'T option

    ///O(1). Returns the count of elememts.
    member Length: int

    ///O(1). Returns deque reversed.
    member Rev: Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements trailing the first element.
    member Tail: Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements trailing the first element.
    member TryTail: Deque<'T> option

    ///O(1) amortized, O(n), worst case. Returns init and the last element.
    member Unconj: Deque<'T> * 'T

    ///O(1) amortized, O(n), worst case. Returns option init and the last element.
    member TryUnconj: (Deque<'T> * 'T) option

    ///O(1) amortized, O(n), worst case. Returns the first element and tail.
    member Uncons: 'T * Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns option first element and tail.
    member TryUncons: ('T * Deque<'T>) option

[<RequireQualifiedAccess>]
module Deque =

    //pattern discriminators
    val (|Cons|Nil|): Deque<'T> -> Choice<('T * Deque<'T>), unit>

    val (|Conj|Nil|): Deque<'T> -> Choice<(Deque<'T> * 'T), unit>

    ///O(1). Returns a new deque with the element added to the end.
    val inline conj: 'T -> Deque<'T> -> Deque<'T>

    ///O(1). Returns a new deque with the element added to the beginning.
    val inline cons: 'T -> Deque<'T> -> Deque<'T>

    ///O(1). Returns deque of no elements.
    [<GeneralizableValue>]
    val empty<'T> : Deque<'T>

    ///O(n). Applies a function to each element of the deque, threading an accumulator argument through the computation, left to right
    val fold: ('State -> 'T -> 'State) -> 'State -> Deque<'T> -> 'State

    ///O(n). Applies a function to each element of the deque, threading an accumulator argument through the computation, right to left
    val foldBack: ('T -> 'State -> 'State) -> Deque<'T> -> 'State -> 'State

    ///O(1) amortized, O(n), worst case. Returns the first element.
    val inline head: Deque<'T> -> 'T

    ///O(1) amortized, O(n), worst case. Returns option first element.
    val inline tryHead: Deque<'T> -> 'T option

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements before the last element.
    val inline initial: Deque<'T> -> Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements before the last element.
    val inline tryInitial: Deque<'T> -> Deque<'T> option

    ///O(1). Returns true if the deque has no elements.
    val inline isEmpty: Deque<'T> -> bool

    ///O(1) amortized, O(n), worst case. Returns the last element.
    val inline last: Deque<'T> -> 'T

    ///O(1) amortized, O(n), worst case. Returns option last element.
    val inline tryLast: Deque<'T> -> 'T option

    ///O(1). Returns the count of elememts.
    val inline length: Deque<'T> -> int

    ///O(n), worst case. Returns a deque of the two lists concatenated.
    val ofCatLists: 'T list -> 'T list -> Deque<'T>

    ///O(n), worst case. Returns a deque of the list.
    val ofList: 'T list -> Deque<'T>

    ///O(n), worst case. Returns a deque of the seq.
    val ofSeq: seq<'T> -> Deque<'T>

    ///O(1). Returns deque reversed.
    val inline rev: Deque<'T> -> Deque<'T>

    ///O(1). Returns a deque of one element.
    val singleton: 'T -> Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns a new deque of the elements trailing the first element.
    val inline tail: Deque<'T> -> Deque<'T>

    ///O(1) amortized, O(n), worst case. Returns option deque of the elements trailing the first element.
    val inline tryTail: Deque<'T> -> Deque<'T> option

    ///O(1) amortized, O(n), worst case. Returns init and the last element.
    val inline unconj: Deque<'T> -> Deque<'T> * 'T

    ///O(1) amortized, O(n), worst case. Returns option init and the last element.
    val inline tryUnconj: Deque<'T> -> (Deque<'T> * 'T) option

    ///O(1) amortized, O(n), worst case. Returns the first element and tail.
    val inline uncons: Deque<'T> -> 'T * Deque<'T>

    ///O(n). Views the given deque as a sequence.
    val inline toSeq: Deque<'T> -> seq<'T>

    ///O(n). Returns a list of the deque elements in FIFO order.
    val toList: Deque<'T> -> 'T list

    ///O(n). Returns an array of the deque elements in FIFO order.
    val toArray: Deque<'T> -> 'T[]

    ///O(n). Returns a new deque whose elements are the results of applying the given function to each element.
    val map: ('T -> 'U) -> Deque<'T> -> Deque<'U>

    ///O(n). Returns a new deque containing only the elements for which the given predicate returns true.
    val filter: ('T -> bool) -> Deque<'T> -> Deque<'T>

    ///O(n). Applies the given function to each element of the deque.
    val iter: ('T -> unit) -> Deque<'T> -> unit

    ///O(n). Returns true if any element of the deque satisfies the given predicate.
    val exists: ('T -> bool) -> Deque<'T> -> bool

    ///O(n). Returns true if all elements of the deque satisfy the given predicate.
    val forall: ('T -> bool) -> Deque<'T> -> bool

    ///O(1) amortized, O(n), worst case. Returns option first element and tail.
    val inline tryUncons: Deque<'T> -> ('T * Deque<'T>) option

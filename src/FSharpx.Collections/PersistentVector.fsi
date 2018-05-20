namespace FSharpx.Collections
#if FX_NO_THREAD
#else
/// PersistentVector is an ordered linear structure implementing the inverse of the List signature, 
/// (last, initial, conj) in place of (head, tail, cons). Length is O(1). Indexed lookup or update
/// (returning a new immutable instance of Vector) of any element is O(log32n), which is close enough
/// to O(1) as to make no practical difference: a PersistentVector containing 4 billion items can
/// lookup or update any item in at most 7 steps.
/// Ordering is by insertion history. The original idea can be found in [Clojure](http://clojure.org/data_structures).
[<Class>]
type PersistentVector<'T> =

    interface System.Collections.Generic.IEnumerable<'T>
    interface System.Collections.IEnumerable

    /// O(1). Returns a new vector with the element added at the end.
    member Conj : 'T -> PersistentVector<'T>
         
    /// O(1) for all practical purposes; really O(log32n). Returns a new vector without the last item. If the collection is empty it throws an exception.
    member Initial : PersistentVector<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option vector without the last item.
    member TryInitial : PersistentVector<'T> option

    /// O(1). Returns true if the vector has no elements.
    member IsEmpty : bool

    /// O(1). Returns a new PersistentVector with no elements.
    static member Empty : unit -> PersistentVector<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns vector element at the index.
    member Item : int -> 'T with get

    /// O(1). Returns the last element in the vector. If the vector is empty it throws an exception.
    member Last : 'T

    /// O(1). Returns option last element in the vector.
    member TryLast : 'T option

    /// O(1). Returns the number of items in the vector.
    member Length : int

    /// O(n). Returns random access list reversed.
    member Rev : unit -> PersistentVector<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns tuple last element and vector without last item
    member Unconj : PersistentVector<'T> * 'T

    /// O(1) for all practical purposes; really O(log32n). Returns option tuple last element and vector without last item
    member TryUnconj : (PersistentVector<'T> * 'T) option

    /// O(1) for all practical purposes; really O(log32n). Returns a new vector that contains the given value at the index.
    member Update : int * 'T -> PersistentVector<'T> 
            
    /// O(1) for all practical purposes; really O(log32n). Returns option vector that contains the given value at the index.
    member TryUpdate : int * 'T -> PersistentVector<'T> option

/// Defines functions which allow to access and manipulate PersistentVectors.
module PersistentVector = 
    //pattern discriminators (active pattern)
    val (|Conj|Nil|) : PersistentVector<'T> ->  Choice<(PersistentVector<'T> * 'T),unit>
    
    /// O(n). Returns a new vector with the elements of the second vector added at the end.
    val append : PersistentVector<'T> -> PersistentVector<'T> -> PersistentVector<'T>

    /// O(1). Returns a new vector with the element added at the end.   
    val inline conj : 'T -> PersistentVector<'T> -> PersistentVector<'T>

    /// O(1). Returns vector of no elements.
    [<GeneralizableValue>]
    val empty<'T> : PersistentVector<'T>

    /// O(m,n). Returns a seq from a vector of vectors.
    val inline flatten : PersistentVector<PersistentVector<'T>> -> seq<'T>

    /// O(n). Returns a state from the supplied state and a function operating from left to right.
    val inline fold : ('State -> 'T -> 'State) -> 'State -> PersistentVector<'T> -> 'State

    /// O(n). Returns a state from the supplied state and a function operating from right to left.
    val inline foldBack : ('T -> 'State -> 'State) -> PersistentVector<'T> -> 'State -> 'State

    /// O(n). Returns a vector of the supplied length using the supplied function operating on the index. 
    val init : int -> (int -> 'T) -> PersistentVector<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns a new vector without the last item. If the collection is empty it throws an exception.
    val inline initial : PersistentVector<'T> -> PersistentVector<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option vector without the last item.
    val inline tryInitial : PersistentVector<'T> -> PersistentVector<'T> option

    /// O(1). Returns true if the vector has no elements.
    val inline isEmpty : PersistentVector<'T> -> bool

    /// O(1). Returns the last element in the vector. If the vector is empty it throws an exception.
    val inline last : PersistentVector<'T> -> 'T

    /// O(1). Returns option last element in the vector.
    val inline tryLast : PersistentVector<'T> -> 'T option

    /// O(1). Returns the number of items in the vector.
    val inline length : PersistentVector<'T> -> int

    /// O(n). Returns a vector whose elements are the results of applying the supplied function to each of the elements of a supplied vector.
    val map : ('T -> 'T1) -> PersistentVector<'T> -> PersistentVector<'T1>

    /// O(1) for all practical purposes; really O(log32n). Returns the value at the index. If the index is out of bounds it throws an exception.
    val inline nth : int -> PersistentVector<'T> -> 'T

    /// O(log32(m,n)). Returns the value at the outer index, inner index. If either index is out of bounds it throws an exception.
    val inline nthNth : int -> int -> PersistentVector<PersistentVector<'T>> -> 'T
 
    /// O(1) for all practical purposes; really O(log32n). Returns option value at the index.
    val inline tryNth : int -> PersistentVector<'T> -> 'T option

    /// O(log32(m,n)). Returns option value at the indices. 
    val inline tryNthNth : int -> int -> PersistentVector<PersistentVector<'T>> -> 'T option

    /// O(n). Returns a vector of the seq.
    val ofSeq : seq<'T> -> PersistentVector<'T>

    /// O(n). Returns vector reversed.
    val inline rev : PersistentVector<'T> -> PersistentVector<'T>

    /// O(1). Returns a new vector of one element.   
    val inline singleton : 'T -> PersistentVector<'T>

    /// O(n). Views the given vector as a sequence.
    val inline toSeq  : PersistentVector<'T> ->  seq<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns tuple last element and vector without last item
    val inline unconj : PersistentVector<'T> -> PersistentVector<'T> * 'T

    /// O(1) for all practical purposes; really O(log32n). Returns option tuple last element and vector without last item
    val inline tryUnconj : PersistentVector<'T> -> (PersistentVector<'T> * 'T) option

    /// O(1) for all practical purposes; really O(log32n). Returns a new vector that contains the given value at the index.
    val inline update : int -> 'T -> PersistentVector<'T> -> PersistentVector<'T>

    /// O(log32(m,n)). Returns a new vector of vectors that contains the given value at the indices. 
    val inline updateNth : int -> int -> 'T -> PersistentVector<PersistentVector<'T>> -> PersistentVector<PersistentVector<'T>>

    /// O(1) for all practical purposes; really O(log32n). Returns option vector that contains the given value at the index.
    val inline tryUpdate : int -> 'T -> PersistentVector<'T> -> PersistentVector<'T> option

    /// O(log32(m,n)). Returns option vector that contains the given value at the indices. 
    val inline tryUpdateNth : int -> int -> 'T -> PersistentVector<PersistentVector<'T>> -> PersistentVector<PersistentVector<'T>> option

    /// O(n). Returns a vector of vectors of given length from the seq. Result may be a jagged vector.
    val inline windowSeq : int  -> seq<'T> -> PersistentVector<PersistentVector<'T>>
#endif

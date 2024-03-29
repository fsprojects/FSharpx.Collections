﻿namespace FSharpx.Collections
/// RandomAccessList is an ordered linear structure implementing the List signature
/// (head, tail, cons), as well as inspection (lookup) and update (returning a new
/// immutable instance) of any element in the structure by index. Length is O(1). Indexed
/// lookup or update (returning a new immutable instance of RandomAccessList) of any element
/// is O(log32n), which is close enough to O(1) as to make no practical difference: a
/// RandomAccessList containing 4 billion items can lookup or update any item in at most 7
/// steps. Ordering is by insertion history. While PersistentVector&lt;'T&gt; is appending to the
/// end this version prepends elements to the list.
[<Class>]
type RandomAccessList<'T> =
    interface System.IEquatable<RandomAccessList<'T>>
    interface System.Collections.Generic.IEnumerable<'T>
    interface System.Collections.IEnumerable
    interface System.Collections.Generic.IReadOnlyCollection<'T>
    interface System.Collections.Generic.IReadOnlyList<'T>

    /// O(1). Returns a new random access list with the element added at the start.
    member Cons : 'T -> RandomAccessList<'T>

    /// O(1). Returns true if the random access list has no elements.
    member IsEmpty : bool

    /// O(1) for all practical purposes; really O(log32n). Returns random access list element at the index.
    member Item : int -> 'T with get

    /// O(1). Returns the first element in the random access list. If the random access list is empty it throws an exception.
    member Head : 'T

    /// O(1). Returns option first element in the random access list.
    member TryHead : 'T option

    /// O(1). Returns the number of items in the random access list.
    member Length : int

    /// O(n). Returns random access list reversed.
    member Rev : unit -> RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns a new random access list without the first item. If the collection is empty it throws an exception.
    member Tail : RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option random access list without the first item.
    member TryTail : RandomAccessList<'T> option

    /// O(1) for all practical purposes; really O(log32n). Returns tuple first element and random access list without first item
    member Uncons : 'T * RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option tuple first element and random access list without first item
    member TryUncons : ('T * RandomAccessList<'T>) option

    /// O(1) for all practical purposes; really O(log32n). Returns a new random access list that contains the given value at the index.
    member Update : int * 'T -> RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option random access list that contains the given value at the index.
    member TryUpdate : int * 'T -> RandomAccessList<'T> option

/// Defines functions which allow to access and manipulate RandomAccessLists.
[<RequireQualifiedAccess>]
module RandomAccessList =
    //pattern discriminators (active pattern)
    val (|Cons|Nil|) : RandomAccessList<'T> ->  Choice<('T * RandomAccessList<'T> ),unit>

    /// O(n). Returns a new random access list with the elements of the second random access list added at the end.
    val append : RandomAccessList<'T> -> RandomAccessList<'T> -> RandomAccessList<'T>

    /// O(1). Returns a new random access list with the element added at the start.
    val inline cons : 'T -> RandomAccessList<'T> -> RandomAccessList<'T>

    /// O(1). Returns random access list of no elements.
    [<GeneralizableValue>]
    val empty<'T> : RandomAccessList<'T>

    /// O(n). Returns a state from the supplied state and a function operating from left to right.
    val inline fold : ('State -> 'T -> 'State) -> 'State -> RandomAccessList<'T> -> 'State

    /// O(n). Returns a state from the supplied state and a function operating from right to left.
    val inline foldBack : ('T -> 'State -> 'State) -> RandomAccessList<'T> -> 'State -> 'State

    /// O(n). Returns a random access list of the supplied length using the supplied function operating on the index.
    val init : int -> (int -> 'T) -> RandomAccessList<'T>

    /// O(1). Returns true if the random access list has no elements.
    val inline isEmpty : RandomAccessList<'T> -> bool

    /// O(1). Returns the first element in the random access list. If the random access list is empty it throws an exception.
    val inline head : RandomAccessList<'T> -> 'T

    /// O(1). Returns option first element in the random access list.
    val inline tryHead : RandomAccessList<'T> -> 'T option

    /// O(1). Returns the number of items in the random access list.
    val inline length : RandomAccessList<'T> -> int

    /// O(n). Returns a random access list whose elements are the results of applying the supplied function to each of the elements of a supplied random access list.
    val map : ('T -> 'T1) -> RandomAccessList<'T> -> RandomAccessList<'T1>

    /// O(1) for all practical purposes; really O(log32n). Returns the value at the index.
    val inline nth : int -> RandomAccessList<'T> -> 'T

    /// O(1) for all practical purposes; really O(log32n). Returns option value at the index.
    val inline tryNth : int -> RandomAccessList<'T> -> 'T option

    /// O(log32(m,n)). Returns the value at the outer index, inner index. If either index is out of bounds it throws an exception.
    val inline nthNth : int -> int -> RandomAccessList<RandomAccessList<'T>> -> 'T

    /// O(log32(m,n)). Returns option value at the indices.
    val inline tryNthNth : int -> int -> RandomAccessList<RandomAccessList<'T>> -> 'T option

    /// O(n). Returns a random access list of the seq.
    val ofSeq : seq<'T> -> RandomAccessList<'T>

    /// O(n). Returns new random access list reversed.
    val inline rev : RandomAccessList<'T> -> RandomAccessList<'T>

    /// O(1). Returns a new random access list of one element.
    val inline singleton : 'T -> RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns a new random access list without the first item. If the collection is empty it throws an exception.
    val inline tail : RandomAccessList<'T> -> RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option random access list without the first item.
    val inline tryTail : RandomAccessList<'T> -> RandomAccessList<'T> option

    /// O(n). Views the given random access list as a sequence.
    val inline toSeq  : RandomAccessList<'T> ->  seq<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns tuple first element and random access list without first item
    val inline uncons : RandomAccessList<'T> -> 'T * RandomAccessList<'T>

    /// O(1) for all practical purposes; really O(log32n). Returns option tuple first element and random access list without first item
    val inline tryUncons : RandomAccessList<'T> -> ('T * RandomAccessList<'T>) option

    /// O(1) for all practical purposes; really O(log32n). Returns a new random access list that contains the given value at the index.
    val inline update : int -> 'T -> RandomAccessList<'T> -> RandomAccessList<'T>

    /// O(log32(m,n)). Returns a new random access list of random access lists that contains the given value at the indices.
    val inline updateNth : int -> int -> 'T -> RandomAccessList<RandomAccessList<'T>> -> RandomAccessList<RandomAccessList<'T>>

    /// O(1) for all practical purposes; really O(log32n). Returns option random access list that contains the given value at the index.
    val inline tryUpdate : int -> 'T -> RandomAccessList<'T> -> RandomAccessList<'T> option

    /// O(log32(m,n)). Returns option random access list that contains the given value at the indices.
    val inline tryUpdateNth : int -> int -> 'T -> RandomAccessList<RandomAccessList<'T>> -> RandomAccessList<RandomAccessList<'T>> option

    /// O(n). Returns a random access list of random access lists of given length from the seq. Result may be a jagged random access list.
    val inline windowSeq : int  -> seq<'T> -> RandomAccessList<RandomAccessList<'T>>

    /// O(n). Combines the two RandomAccessLists into a RandomAccessList of pairs. The two RandomAccessLists must have equal lengths, otherwise an ArgumentException is raised.
    val zip : randomAccessList1 : RandomAccessList<'T> -> randomAccessList2 : RandomAccessList<'T2> -> RandomAccessList<'T * 'T2>

    /// O(n). Applies a function to each element of the collection, threading an accumulator argument through the computation. This function first applies the function to the first two elements of the list. Then, it passes this result into the function along with the third element and so on. Finally, it returns the final result. If the input function is f and the elements are i0...iN, then it computes f (... (f i0 i1) i2 ...) iN.
    val reduce : f: ('T -> 'T -> 'T) -> randomAccessList : RandomAccessList<'T> -> 'T

    /// O(n). Builds a new collection whose elements are the results of applying the given function to the corresponding elements of the two collections pairwise. The two input arrays must have the same lengths, otherwise ArgumentException is raised.
    val map2 : ('T1 -> 'T2 -> 'U) -> randomAccessList1 : RandomAccessList<'T1> ->  randomAccessList2 : RandomAccessList<'T2> -> RandomAccessList<'U>
// First version copied from the F# Power Pack 
// https://raw.github.com/fsharp/powerpack/master/src/FSharp.PowerPack/ResizeArray.fsi

//==========================================================================
// ResizeArray
// 
// (c) Microsoft Corporation 2005-2008.  
//===========================================================================

namespace FSharpx.Collections

open System
open System.Collections.Generic

/// Generic operations on the type System.Collections.Generic.List, which is called ResizeArray in the F# libraries.
[<RequireQualifiedAccess>]
module ResizeArray =

    /// Return the length of the collection.  You can also use property <c>arr.Length</c>.
    val length: ResizeArray<'T> -> int

    /// Fetch an element from the collection.  You can also use the syntax <c>arr.[idx]</c>.
    val get: ResizeArray<'T> -> int -> 'T


    /// Set the value of an element in the collection. You can also use the syntax <c>arr.[idx] &lt;- e</c>.
    val set: ResizeArray<'T> -> int -> 'T -> unit

    /// Create an array whose elements are all initially the given value.
    val create: int -> 'T -> ResizeArray<'T>
     
    /// Create an array by calling the given generator on each index.
    val init: int -> (int -> 'T) -> ResizeArray<'T>

    /// Build a new array that contains the elements of the first array followed by the elements of the second array
    val append: ResizeArray<'T> -> ResizeArray<'T> -> ResizeArray<'T>

    /// Build a new array that contains the elements of each of the given list of arrays
    val concat: seq<ResizeArray<'T>> -> ResizeArray<'T>

    /// Build a new array that contains the given subrange specified by
    /// starting index and length.
    val sub: ResizeArray<'T> -> int -> int -> ResizeArray<'T>

    /// Build a new array that contains the elements of the given array
    val copy: ResizeArray<'T> -> ResizeArray<'T>

    /// Fill a range of the collection with the given element
    val fill: ResizeArray<'T> -> int -> int -> 'T -> unit

    /// Read a range of elements from the first array and write them into the second.
    val blit: ResizeArray<'T> -> int -> ResizeArray<'T> -> int -> int -> unit

    /// Build a list from the given array
    val toList: ResizeArray<'T> -> 'T list

    /// Build an array from the given list
    val ofList: 'T list -> ResizeArray<'T>

    /// Build and array from the given seq
    val ofSeq : 'T seq -> ResizeArray<'T>

    /// Apply a function to each element of the collection, threading an accumulator argument
    /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c> 
    /// then computes <c>f (... (f s i0)...) iN</c>
    val fold: ('T -> 'State -> 'T) -> 'T -> ResizeArray<'State> -> 'T

    /// Apply a function to each element of the array, threading an accumulator argument
    /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c> then 
    /// computes <c>f i0 (...(f iN s))</c>.
    val foldBack: ('T -> 'State -> 'State) -> ResizeArray<'T> -> 'State -> 'State

    /// Apply the given function to each element of the array. 
    val iter: ('T -> unit) -> ResizeArray<'T> -> unit

    /// Build a new array whose elements are the results of applying the given function
    /// to each of the elements of the array.
    val map: ('T -> 'U) -> ResizeArray<'T> -> ResizeArray<'U>

    /// Apply the given function to two arrays simultaneously. The
    /// two arrays must have the same lengths, otherwise an Invalid_argument exception is
    /// raised.
    val iter2: ('T1 -> 'T2 -> unit) -> ResizeArray<'T1> -> ResizeArray<'T2> -> unit

    /// Build a new collection whose elements are the results of applying the given function
    /// to the corresponding elements of the two collections pairwise.  The two input
    /// arrays must have the same lengths.
    val map2: ('T1 -> 'T2 -> 'U) -> ResizeArray<'T1> -> ResizeArray<'T2> -> ResizeArray<'U>

    /// Apply the given function to each element of the array.  The integer passed to the
    /// function indicates the index of element.
    val iteri: (int -> 'T -> unit) -> ResizeArray<'T> -> unit

    /// Build a new array whose elements are the results of applying the given function
    /// to each of the elements of the array. The integer index passed to the
    /// function indicates the index of element being transformed.
    val mapi: (int -> 'T -> 'U) -> ResizeArray<'T> -> ResizeArray<'U>

    /// Test if any element of the array satisfies the given predicate.
    /// If the input function is <c>f</c> and the elements are <c>i0...iN</c> 
    /// then computes <c>p i0 or ... or p iN</c>.
    val exists: ('T -> bool) -> ResizeArray<'T> -> bool

    /// Test if all elements of the array satisfy the given predicate.
    /// If the input function is <c>f</c> and the elements are <c>i0...iN</c> and "j0...jN"
    /// then computes <c>p i0 && ... && p iN</c>.
    val forall: ('T -> bool) -> ResizeArray<'T> -> bool

    /// Return a new collection containing only the elements of the collection
    /// for which the given predicate returns <c>true</c>
    val filter: ('T -> bool) -> ResizeArray<'T> -> ResizeArray<'T>

    /// Split the collection into two collections, containing the 
    /// elements for which the given predicate returns <c>true</c> and <c>false</c>
    /// respectively 
    val partition: ('T -> bool) -> ResizeArray<'T> -> ResizeArray<'T> * ResizeArray<'T>

    /// Apply the given function to each element of the array. Return
    /// the array comprised of the results "x" for each element where
    /// the function returns Some(x)
    val choose: ('T -> 'U option) -> ResizeArray<'T> -> ResizeArray<'U>

    /// Return the first element for which the given function returns <c>true</c>.
    /// Raise <c>KeyNotFoundException</c> if no such element exists.
    val find: ('T -> bool) -> ResizeArray<'T> -> 'T

    /// Return the first element for which the given function returns <c>true</c>.
    /// Return None if no such element exists.
    val tryFind: ('T -> bool) -> ResizeArray<'T> -> 'T option

    /// Apply the given function to successive elements, returning the first
    /// result where function returns "Some(x)" for some x.
    val tryPick: ('T -> 'U option) -> ResizeArray<'T> -> 'U option

    /// Return a new array with the elements in reverse order
    val rev: ResizeArray<'T> -> ResizeArray<'T>

    /// Sort the elements using the given comparison function
    val sort: ('T -> 'T -> int) -> ResizeArray<'T> -> unit

    /// Sort the elements using the key extractor and generic comparison on the keys
    val sortBy: ('T -> 'Key) -> ResizeArray<'T> -> unit when 'Key : comparison

    /// Return a fixed-length array containing the elements of the input ResizeArray
    val toArray : ResizeArray<'T> -> 'T[]
    /// Build a ResizeArray from the given elements
    val ofArray : 'T[] -> ResizeArray<'T>
    /// Return a view of the array as an enumerable object
    val toSeq : ResizeArray<'T> -> seq<'T>

    /// Test elements of the two arrays pairwise to see if any pair of element satisfies the given predicate.
    /// Raise ArgumentException if the arrays have different lengths.
    val exists2 : ('T1 -> 'T2 -> bool) -> ResizeArray<'T1> -> ResizeArray<'T2> -> bool

    /// Return the index of the first element in the array
    /// that satisfies the given predicate. Raise <c>KeyNotFoundException</c> if 
    /// none of the elements satisfy the predicate.
    val findIndex : ('T -> bool) -> ResizeArray<'T> -> int

    /// Return the index of the first element in the array
    /// that satisfies the given predicate. Raise <c>KeyNotFoundException</c> if 
    /// none of the elements satisfy the predicate.
    val findIndexi : (int -> 'T -> bool) -> ResizeArray<'T> -> int

    /// Apply a function to each element of the array, threading an accumulator argument
    /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c> 
    /// then computes <c>f (... (f i0 i1)...) iN</c>. Raises ArgumentException if the array has size zero.
    val reduce : ('T -> 'T -> 'T) -> ResizeArray<'T> -> 'T

    /// Apply a function to each element of the array, threading an accumulator argument
    /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c> then 
    /// computes <c>f i0 (...(f iN-1 iN))</c>. Raises ArgumentException if the array has size zero.
    val reduceBack : ('T -> 'T -> 'T) -> ResizeArray<'T> -> 'T

    /// Apply a function to pairs of elements drawn from the two collections, 
    /// left-to-right, threading an accumulator argument
    /// through the computation.  The two input
    /// arrays must have the same lengths, otherwise an <c>ArgumentException</c> is
    /// raised.
    val fold2: ('State -> 'T1 -> 'T2 -> 'State) -> 'State -> ResizeArray<'T1> -> ResizeArray<'T2> -> 'State

    /// Apply a function to pairs of elements drawn from the two collections, right-to-left, 
    /// threading an accumulator argument through the computation.  The two input
    /// arrays must have the same lengths, otherwise an <c>ArgumentException</c> is
    /// raised.
    val foldBack2 : ('T1 -> 'T2 -> 'State -> 'State) -> ResizeArray<'T1> -> ResizeArray<'T2> -> 'State -> 'State

    /// Test elements of the two arrays pairwise to see if all pairs of elements satisfy the given predicate.
    /// Raise ArgumentException if the arrays have different lengths.
    val forall2 : ('T1 -> 'T2 -> bool) -> ResizeArray<'T1> -> ResizeArray<'T2> -> bool

    /// Return true if the given array is empty, otherwise false
    val isEmpty : ResizeArray<'T> -> bool

    /// Apply the given function to pair of elements drawn from matching indices in two arrays,
    /// also passing the index of the elements. The two arrays must have the same lengths, 
    /// otherwise an <c>ArgumentException</c> is raised.
    val iteri2 : (int -> 'T1 -> 'T2 -> unit) -> ResizeArray<'T1> -> ResizeArray<'T2> -> unit

    /// Build a new collection whose elements are the results of applying the given function
    /// to the corresponding elements of the two collections pairwise.  The two input
    /// arrays must have the same lengths, otherwise an <c>ArgumentException</c> is
    /// raised.
    val mapi2 : (int -> 'T1 -> 'T2 -> 'U) -> ResizeArray<'T1> -> ResizeArray<'T2> -> ResizeArray<'U>

    /// Like <c>fold</c>, but return the intermediary and final results
    val scan : ('State -> 'T -> 'State) -> 'State -> ResizeArray<'T> -> ResizeArray<'State>

    /// Like <c>foldBack</c>, but return both the intermediary and final results
    val scanBack : ('T -> 'State -> 'State) -> ResizeArray<'T> -> 'State -> ResizeArray<'State>

    /// Return an array containing the given element
    val singleton : 'T -> ResizeArray<'T>
    
    /// Return the index of the first element in the array
    /// that satisfies the given predicate.
    val tryFindIndex : ('T -> bool) -> ResizeArray<'T> -> int option

    /// Return the index of the first element in the array
    /// that satisfies the given predicate.
    val tryFindIndexi : (int -> 'T -> bool) -> ResizeArray<'T> -> int option

    /// Combine the two arrays into an array of pairs. The two arrays must have equal lengths, otherwise an <c>ArgumentException</c> is
    /// raised..
    val zip : ResizeArray<'T1> -> ResizeArray<'T2> -> ResizeArray<'T1 * 'T2>

    /// Split an array of pairs into two arrays
    val unzip : ResizeArray<'T1 * 'T2> -> ResizeArray<'T1> * ResizeArray<'T2>

    /// Returns an array that contains no duplicate entries according to the 
    /// generic hash and equality comparisons on the keys returned by the given key-generating function.
    /// If an element occurs multiple times in the array then the later occurrences are discarded.
    val distinctBy: ('T -> 'Key) -> ResizeArray<'T> -> ResizeArray<'T> when 'Key : equality

    /// Returns an array that contains no duplicate entries according to generic hash and
    /// equality comparisons on the entries.
    /// If an element occurs multiple times in the array then the later occurrences are discarded.
    val distinct: ResizeArray<'T> -> ResizeArray<'T> when 'T : equality
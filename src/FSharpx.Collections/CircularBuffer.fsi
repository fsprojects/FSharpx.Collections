namespace FSharpx.Collections.Mutable

open System

/// needs doc
type CircularBuffer<'T> =
    interface System.Collections.IEnumerable
    interface System.Collections.Generic.IEnumerable<'T>
    interface System.Collections.Generic.IReadOnlyCollection<'T>

    /// needs doc
    new : bufferSize:int -> CircularBuffer<'T>
    /// needs doc
    member Dequeue : count:int -> 'T []
    /// needs doc
    member Enqueue : value:'T [] -> unit
#if !FABLE_COMPILER
    /// needs doc
    member Enqueue : value:ArraySegment<'T> -> unit
#endif
    /// needs doc
    member Enqueue : value:'T -> unit
    /// needs doc
    member Enqueue : value:'T [] * offset:int -> unit
    /// needs doc
    member Enqueue : value:'T [] * offset:int * count:int -> unit
    /// needs doc
    member GetEnumerator : unit -> System.Collections.Generic.IEnumerator<'T>
    /// needs doc
    member Count : int

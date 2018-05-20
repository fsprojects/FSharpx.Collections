namespace FSharpx.Collections.Mutable

open System
open System.Collections.Generic

/// needs doc
type CircularBuffer<'T> =
  class
    interface IEnumerable<'T>
    /// needs doc
    new : bufferSize:int -> CircularBuffer<'T>
    /// needs doc
    member Dequeue : count:int -> 'T []
    /// needs doc
    member Enqueue : value:'T [] -> unit
    /// needs doc
    member Enqueue : value:ArraySegment<'T> -> unit
    /// needs doc
    member Enqueue : value:'T -> unit
    /// needs doc
    member Enqueue : value:'T [] * offset:int -> unit
    /// needs doc
    member Enqueue : value:'T [] * offset:int * count:int -> unit
    /// needs doc
    member GetEnumerator : unit -> IEnumerator<'T>
    /// needs doc
    member Count : int
  end
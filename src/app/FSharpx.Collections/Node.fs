namespace FSharpx.Collections

open System.Threading

type Node(thread,array:obj[]) =
    let thread = thread
    new() = Node(ref null,Array.create Literals.blockSize null)
    with
        static member InCurrentThread() = Node(ref Thread.CurrentThread,Array.create Literals.blockSize null)
        member this.Array = array
        member this.Thread = thread
        member this.SetThread t = thread := t

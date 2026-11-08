/// Operators for working with LazyList.
/// This module is opened automatically when FSharpx.Collections is opened.
[<AutoOpen>]
module FSharpx.Collections.LazyListOperators

/// O(1). Returns a new LazyList with the given head and a lazy tail.
/// Infix operator alias for LazyList.consLazy.
/// Example: <c>let rec ones = 1 @@ lazy ones</c>
let inline (@@) (head: 'T) (tail: Lazy<LazyList<'T>>) : LazyList<'T> =
    LazyList.consLazy head tail

namespace FSharpx.Collections

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices

type NonEmptyList<'T> =
    private
        { List: 'T list }

    member this.Head = this.List.Head
    member this.Tail = this.List.Tail
    member this.Length = this.List.Length

    interface IEnumerable<'T> with
        member this.GetEnumerator() =
            (this.List :> _ seq).GetEnumerator()

    interface IEnumerable with
        member this.GetEnumerator() =
            (this.List :> _ seq).GetEnumerator() :> IEnumerator

    interface IReadOnlyCollection<'T> with
        member this.Count = this.Length

[<Extension>]
[<RequireQualifiedAccess>]
module NonEmptyList =
    [<CompiledName("Create")>]
    let create head tail =
        { List = head :: tail }

#if !FABLE_COMPILER
    [<CompiledName("Create")>]
    let createParamsArray(head, [<ParamArray>] tail) =
        { List = head :: List.ofArray tail }
#endif

    [<CompiledName("Singleton")>]
    let inline singleton value =
        create value []

    [<CompiledName("Head")>]
    let inline head(x: NonEmptyList<_>) = x.Head

    [<CompiledName("Tail")>]
    let inline tail(x: NonEmptyList<_>) = x.Tail

    [<CompiledName("ToFSharpList")>]
    [<Extension>]
    let toList(x: NonEmptyList<_>) = x.List

    [<CompiledName("Length")>]
    let inline length(x: NonEmptyList<_>) = x.Length

    [<CompiledName("ToArray")>]
    [<Extension>]
    let toArray list =
        Array.ofList list.List

    [<CompiledName("AsEnumerable")>]
    [<Extension>]
    let inline toSeq(list: NonEmptyList<_>) =
        list :> _ seq

    [<CompiledName("OfArray")>]
    let ofArray(arr: _ array) =
        match arr.Length with
        | 0 -> invalidArg "arr" "Array is empty"
        | _ -> { List = List.ofArray arr }

    [<CompiledName("OfList")>]
    let ofList(l: _ list) =
        match l with
        | head :: tail -> create head tail
        | _ -> invalidArg "l" "List is empty"

    [<CompiledName("OfSeq")>]
    let ofSeq(e: _ seq) =
        if Seq.isEmpty e then
            invalidArg "e" "Sequence is empty"
        else
            { List = List.ofSeq e }

    [<CompiledName("Select")>]
    let map f list =
        { List = List.map f list.List }

    [<CompiledName("Cons")>]
    let cons head tail =
        { List = head :: tail.List }

#if !FABLE_COMPILER
    [<CompiledName("Concat")>]
#endif
    let appendList list1 list2 =
        { List = list1.List @ list2 }

    [<CompiledName("Concat")>]
    let append list1 list2 =
        { List = list1.List @ list2.List }

    [<CompiledName("Aggregate")>]
    let reduce reduction list =
        List.reduce reduction list.List

    [<CompiledName("Last")>]
    let last list =
        List.last list.List

    [<CompiledName("Reverse")>]
    [<Extension>]
    let rev list =
        { List = List.rev list.List }

    [<CompiledName("SelectMany")>]
    let collect (mapping: 'a -> NonEmptyList<'b>) (list: NonEmptyList<'a>) =
        list.List |> List.collect(fun x -> (mapping x).List) |> ofList

    [<CompiledName("Zip")>]
    let zip list1 list2 =
        { List = List.zip list1.List list2.List }

    [<CompiledName("Iterate")>]
    let iter action list =
        List.iter action list.List

    [<CompiledName("IterateIndexed")>]
    let iteri action list =
        List.iteri action list.List

    [<CompiledName("SelectIndexed")>]
    let mapi mapping list =
        { List = List.mapi mapping list.List }

    [<CompiledName("Exists")>]
    let exists predicate list =
        List.exists predicate list.List

    [<CompiledName("ForAll")>]
    let forall predicate list =
        List.forall predicate list.List

    [<CompiledName("Contains")>]
    let contains value list =
        List.contains value list.List

    [<CompiledName("SortWith")>]
    let sortWith comparer list =
        { List = List.sortWith comparer list.List }

    [<CompiledName("SortBy")>]
    let sortBy projection list =
        { List = List.sortBy projection list.List }

    [<CompiledName("Sort")>]
    let sort list =
        { List = List.sort list.List }

    [<CompiledName("MaxBy")>]
    let maxBy projection list =
        List.maxBy projection list.List

    [<CompiledName("MinBy")>]
    let minBy projection list =
        List.minBy projection list.List

    /// O(n). Returns the largest element of the non-empty list.
    [<CompiledName("Max")>]
    let max list =
        List.max list.List

    /// O(n). Returns the smallest element of the non-empty list.
    [<CompiledName("Min")>]
    let min list =
        List.min list.List

    /// O(n). Applies a function to each element of the collection, threading an accumulator argument.
    [<CompiledName("Fold")>]
    let fold (folder: 'State -> 'T -> 'State) (state: 'State) (list: NonEmptyList<'T>) =
        List.fold folder state list.List

    /// O(n). Applies a function to each element of the collection from right to left, threading an accumulator argument.
    [<CompiledName("FoldBack")>]
    let foldBack (folder: 'T -> 'State -> 'State) (list: NonEmptyList<'T>) (state: 'State) =
        List.foldBack folder list.List state

    /// O(n), worst case. Returns the first element for which the given function returns <c>Some</c>.
    [<CompiledName("TryFind")>]
    let tryFind (predicate: 'T -> bool) (list: NonEmptyList<'T>) =
        List.tryFind predicate list.List

    /// O(n), worst case. Returns the first element for which the given function returns <c>true</c>.
    /// Raises <c>KeyNotFoundException</c> if no such element exists.
    [<CompiledName("Find")>]
    let find (predicate: 'T -> bool) (list: NonEmptyList<'T>) =
        List.find predicate list.List

    /// O(n). Returns a new list containing only the elements for which the given predicate returns <c>true</c>.
    /// The result may be empty, so a plain <c>'T list</c> is returned.
    [<CompiledName("Filter")>]
    let filter (predicate: 'T -> bool) (list: NonEmptyList<'T>) : 'T list =
        List.filter predicate list.List

    /// O(n). Applies the given function to each element and returns a list of the values returned by
    /// the function where the function returned <c>Some</c>. The result may be empty.
    [<CompiledName("Choose")>]
    let choose (mapping: 'T -> 'U option) (list: NonEmptyList<'T>) : 'U list =
        List.choose mapping list.List

    /// O(n). Splits the collection into two lists; the first containing elements for which the given
    /// predicate returns <c>true</c>, the second for which it returns <c>false</c>. Both parts may be empty.
    [<CompiledName("Partition")>]
    let partition (predicate: 'T -> bool) (list: NonEmptyList<'T>) : 'T list * 'T list =
        List.partition predicate list.List

    /// O(n). Returns a NonEmptyList of each element paired with its index.
    [<CompiledName("Indexed")>]
    let indexed(list: NonEmptyList<'T>) : NonEmptyList<int * 'T> =
        { List = List.indexed list.List }

    /// O(n). Splits a NonEmptyList of pairs into a pair of NonEmptyLists.
    [<CompiledName("Unzip")>]
    let unzip(list: NonEmptyList<'T1 * 'T2>) : NonEmptyList<'T1> * NonEmptyList<'T2> =
        let a, b = List.unzip list.List
        { List = a }, { List = b }

    /// O(n). Returns a list of each element and its successor. The result may be empty for a singleton list.
    [<CompiledName("Pairwise")>]
    let pairwise(list: NonEmptyList<'T>) : ('T * 'T) list =
        List.pairwise list.List

    /// O(n). Returns a NonEmptyList of states by threading an accumulator through the list.
    /// The result always contains at least the initial state followed by the intermediate states.
    [<CompiledName("Scan")>]
    let scan (folder: 'State -> 'T -> 'State) (state: 'State) (list: NonEmptyList<'T>) : NonEmptyList<'State> =
        { List = List.scan folder state list.List }

    /// O(n). Applies a key-generating function to each element and yields a NonEmptyList of
    /// unique keys together with a NonEmptyList of all elements that match each key.
    [<CompiledName("GroupBy")>]
    let groupBy (projection: 'T -> 'Key) (list: NonEmptyList<'T>) : NonEmptyList<'Key * NonEmptyList<'T>> =
        { List =
            list.List
            |> List.groupBy projection
            |> List.map(fun (k, vs) -> k, { List = vs }) }

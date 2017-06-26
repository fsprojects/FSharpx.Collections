namespace FSharpx.Collections

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices

type NonEmptyList<'T> = 
    private { List: 'T list }

    member x.Head = x.List.Head
    member x.Tail = x.List.Tail
    member x.Length = x.List.Length

    interface IEnumerable<'T> with
        member x.GetEnumerator() = 
            (x.List :> seq<_>).GetEnumerator()
    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = (x :> _ seq).GetEnumerator() :> IEnumerator

[<Extension>]
module NonEmptyList =
    [<CompiledName("Create")>]
    let create head tail = { List = head :: tail }

    [<CompiledName("Create")>]
    let createParamsArray(head, [<ParamArray>] tail) = { List = head :: List.ofArray tail }

    [<CompiledName("Singleton")>]
    let inline singleton value = create value []

    [<CompiledName("Head")>]
    let inline head (x: NonEmptyList<_>) = x.Head

    [<CompiledName("Tail")>]
    let inline tail (x: NonEmptyList<_>) = x.Tail

    [<CompiledName("ToFSharpList")>]
    [<Extension>]
    let toList (x: NonEmptyList<_>) = x.List

    [<CompiledName("Length")>]
    let inline length (x: NonEmptyList<_>) = x.Length

    [<CompiledName("ToArray")>]
    [<Extension>]
    let toArray list =
         Array.ofList list.List

    [<CompiledName("AsEnumerable")>]
    [<Extension>]
    let inline toSeq (list: NonEmptyList<_>) = list :> _ seq

    [<CompiledName("OfArray")>]
    let ofArray (arr: _ array) =
        match arr.Length with
        | 0 -> invalidArg "arr" "Array is empty"
        | _ -> { List = List.ofArray arr }

    [<CompiledName("OfList")>]
    let ofList (l: _ list) =
        match l with
        | head :: tail -> create head tail
        | _ -> invalidArg "l" "List is empty"

    [<CompiledName("OfSeq")>]
    let ofSeq (e: _ seq) =
        if Seq.isEmpty e then
            invalidArg "e" "Sequence is empty"
        else
            {List = List.ofSeq e}

    [<CompiledName("Select")>]
    let map f list =
        { List = List.map f list.List }

    [<CompiledName("Cons")>]
    let cons head tail =
        { List = head :: tail.List }

    [<CompiledName("Concat")>]
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
        { List = List.rev list.List}

    [<CompiledName("SelectMany")>]
    let collect (mapping:'a -> NonEmptyList<'b>) (list:NonEmptyList<'a>) =
        list.List |> List.collect (fun x -> (mapping x).List) |> ofList

    [<CompiledName("Zip")>]
    let zip list1 list2 =
        { List = List.zip list1.List list2.List }
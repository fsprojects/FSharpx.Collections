namespace rec FSharpx.Collections.Experimental

open System.Collections
open FSharpx.Collections
open System.Collections.Generic

(* Implementation guided by following paper: https://arxiv.org/pdf/1412.4882.pdf *)

/// A balanced binary tree similar to a red-black tree which may have less predictable performance.
type AaTree<'T when 'T: comparison> = 
    | E
    | T of int * AaTree<'T> * 'T * AaTree<'T>
    
    member x.ToList() = 
        AaTree.toList x

    interface IEnumerable<'T> with
        member x.GetEnumerator() =
            (x.ToList() :> _ seq).GetEnumerator()

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (x :> _ seq).GetEnumerator()

[<RequireQualifiedAccess>]
module AaTree =
    /// O(1): Returns a boolean if tree is empty.
    let isEmpty = function
        | E -> true
        | _ -> false

    let private sngl = function
        | E -> false
        | T(_, _, _, E) -> true
        | T(lvx, _, _, T(lvy, _, _, _)) -> lvx > lvy

    /// O(1): Returns an empty AaTree.
    let empty = E

    let private lvl = function
        | E -> 0
        | T(lvt, _, _, _) -> lvt

    let private nlvl = function
        | T(lvt, _, _, _) as t -> 
            if sngl t
            then (lvt - 1)
            else lvt
        | _ -> failwith "unexpected nlvl case"

    let private skew = function
        | T(lvx, T(lvy, a, ky, b), kx, c) when lvx = lvy
            -> T(lvx, a, ky, T(lvx, b, kx, c))
        | t -> t

    let private split = function
        | T(lvx, a, kx, T(lvy, b, ky, T(lvz, c, kz, d))) 
            when lvx = lvy && lvy = lvz
              -> T(lvx + 1, T(lvx, a, kx, b), ky, T(lvx, c, kz, d))
        | t -> t

    /// O(log n): Returns a new AaTree with the parameter inserted.
    let rec insert item = function
        | E -> T(1, E, item, E)
        | T(h, l, v, r) as node ->
            if item < v
            then split <| (skew <| T(h, insert item l, v, r))
            elif item > v
            then split <| (skew <| T(h, l, v, insert item r))
            else node

    let private adjust = function
        | T(lvt, lt, kt, rt) as t when lvl lt >= lvt - 1 && lvl rt >= (lvt - 1) 
            -> t
        | T(lvt, lt, kt, rt) when lvl rt < lvt - 1 && sngl lt-> 
            skew <| T(lvt - 1, lt, kt, rt)
        | T(lvt, T(lv1, a, kl, T(lvb, lb, kb, rb)), kt, rt) when lvl rt < lvt - 1 
            -> T(lvb + 1, T(lv1, a, kl, lb), kb, T(lvt - 1, rb, kt, rt))
        | T(lvt, lt, kt, rt) when lvl rt < lvt 
            -> split <| T(lvt - 1, lt, kt, rt)
        | T(lvt, lt, kt, T(lvr, T(lva, c, ka, d), kr, b)) -> 
            let a = T(lva, c, ka, d)
            T(lva + 1, T(lvt - 1, lt, kt, c), ka, (split (T(nlvl a, d, kr, b))))
        | _ -> failwith "unexpected adjust case"

    let rec private dellrg = function
        | T(_, l, v, E) -> (l, v)
        | T(h, l, v, r) ->
            let (newLeft, newVal) = dellrg l
            T(h, newLeft, v, r), newVal
        | _ -> failwith "unexpected dellrg case"

    /// O(log n): Returns an AaTree with the parameter removed.
    let rec delete item = function
        | E -> E
        | T(_, E, v, rt) when item = v -> rt
        | T(_, lt, v, E) when item = v -> lt
        | T(h, l, v, r) as node ->
            if item < v
            then adjust <| T(h, delete item l, v, r)
            elif item > v
            then T(h, l, v, delete item r)
            else 
                let (newLeft, newVal) = dellrg l
                T(h, newLeft, newVal, r)

    /// O(log n): Returns true if the given item exists in the tree.
    let rec exists item = function
        | E -> false
        | T(_, l, v, r) ->
            if v = item then true
            elif item < v then exists item l
            else exists item r

    /// O(log n): Returns true if the given item does not exist in the tree.
    let rec notExists item tree =
        not <| exists item tree

    /// O(log n): Returns Some item if it was found in the tree; else, returns None.
    let rec tryFind item = function
        | E -> None
        | T(_, l, v, r) ->
            if v = item then Some v
            elif item < v then tryFind item l
            else tryFind item r

    /// O(log n): Returns an item if it was found in the tree; else, throws error.
    let rec find item tree =
        match tryFind item tree with
        | None -> failwith <| sprintf "Item %A was not found in the tree." item
        | Some x -> x

    let rec private foldOpt (f: OptimizedClosures.FSharpFunc<_,_,_>) x t =
        match t with
        | E -> x
        | T(_, l, v, r) ->
            let x = foldOpt f x l
            let x = f.Invoke(x,v)
            foldOpt f x r

    /// Executes a function on each element in order (for example: 1, 2, 3 or a, b, c).
    let fold f x t = foldOpt (OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)) x t

    let rec private foldBackOpt (f: OptimizedClosures.FSharpFunc<_,_,_>) x t =
        match t with
        | E -> x
        | T(_, l, v, r) ->
            let x = foldBackOpt f x r
            let x = f.Invoke(x,v)
            foldBackOpt f x l

    /// Executes a function on each element in reverse order (for example: 3, 2, 1 or c, b, a).
    let foldBack f x t = foldBackOpt (OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)) x t

    /// O(n): Returns a list containing the elements in the tree.
    let toList (tree: AaTree<'T>) =
        foldBack (fun a e -> e::a) [] tree

    let toSeq (tree: AaTree<'T>) =
        tree |> toList |> List.toSeq

    let toArray (tree: AaTree<'T>) =
        tree |> toList |> List.toArray

    /// O(n log n): Builds an AaTree from the elements in the given list.
    let ofList collection =
        List.fold (fun acc item -> insert item acc) empty collection

    let ofSeq collection =
        Seq.fold (fun acc item -> insert item acc) empty collection

    let ofArray collection =
        Array.fold (fun acc item -> insert item acc) empty collection

    type AaTree<'T when 'T: comparison> with
        member x.Insert(y) = insert y x
        member x.Delete(y) = delete y x
        member x.ToSeq() = toSeq x
        member x.ToArray() = toArray x
        member x.Fold(folder, initialState) = fold folder initialState x
        member x.FoldBack(folder, initialState) = foldBack folder initialState x
        member x.Find(y) = find y x
        member x.TryFind(y) = tryFind y x
        member x.IsEmpty() = isEmpty x

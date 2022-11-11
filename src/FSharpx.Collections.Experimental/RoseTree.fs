namespace FSharpx.Collections.Experimental

open FSharpx.Collections
open System
open System.Runtime.CompilerServices

/// Multi-way tree, also known as rose tree.
// Ported from https://hackage.haskell.org/package/containers-0.6.6/docs/Data-Tree.html
[<CustomEquality; NoComparison>]
type RoseTree<[<EqualityConditionalOn>] 'T> =
    {
        Root: 'T
        Children: LazyList<RoseTree<'T>>
    }

    override x.Equals y =
        match y with
        | :? RoseTree<'T> as y -> (x :> _ IEquatable).Equals y
        | _ -> false

    override x.GetHashCode() =
        391
        + (box x.Root).GetHashCode() * 23
        + (x.Children :> _ seq).GetHashCode()

    interface IEquatable<RoseTree<'T>> with
        member x.Equals y =
            Unchecked.equals x.Root y.Root
            && LazyList.equalsWith Unchecked.equals x.Children y.Children

module L = LazyList

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<Extension>]
[<RequireQualifiedAccess>]
module RoseTree =
    open FSharpx

    let inline create root children = { Root = root; Children = children }

    let inline singleton x =
        create x L.empty

    let rec map f (x: _ RoseTree) = {
        RoseTree.Root = f x.Root
        Children = L.map (map f) x.Children
    }

    let rec ap x f = {
        RoseTree.Root = f.Root x.Root
        Children =
            let a = L.map (map f.Root) x.Children
            let b = L.map (fun c -> ap x c) f.Children
            L.append a b
    }

    let inline lift2 f a b =
        singleton f |> ap a |> ap b

    let rec bind f x =
        let a = f x.Root

        {
            RoseTree.Root = a.Root
            Children = L.append a.Children (L.map (bind f) x.Children)
        }

    [<CompiledName("DfsPre")>]
    [<Extension>]
    let rec dfsPre(x: _ RoseTree) = seq {
        yield x.Root
        yield! Seq.collect dfsPre x.Children
    }

    [<CompiledName("DfsPost")>]
    [<Extension>]
    let rec dfsPost(x: _ RoseTree) = seq {
        yield! Seq.collect dfsPost x.Children
        yield x.Root
    }

    let rec unfold f seed =
        let root, bs = f seed
        create root (unfoldForest f bs)

    and unfoldForest f =
        L.map(unfold f)

    /// Behaves like a combination of map and fold;
    /// it applies a function to each element of a tree,
    /// passing an accumulating parameter,
    /// and returning a final value of this accumulator together with the new tree.
    let rec mapAccum f state tree =
        let nstate, root = f state tree.Root
        let nstate, children = LazyList.mapAccum (mapAccum f) nstate tree.Children
        nstate, create root children

// TODO:
// bfs: http://pdf.aminer.org/000/309/950/the_under_appreciated_unfold.pdf
// sequence / mapM / filterM
// zipper: http://hackage.haskell.org/package/rosezipper-0.2

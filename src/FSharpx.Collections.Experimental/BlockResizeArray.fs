﻿namespace FSharpx.Collections.Experimental

/// Resize array fith fixed size block memory allocation.
/// Provide more optimal space usage for huge arrays than standard ResizeArray.
/// Basic version created by Avdyukhin Dmitry <dimonbv@gmail.com>
/// As evidenced by the tests that cannot run in mono, and pending destabilizing tests,
/// beware that this data structure can be destabilizing to your assembly.
open System.Collections
open System.Collections.Generic
            
type BlockResizeArray<'T> () =

    let initArraysCount = 1
    let mutable count = 0
    let shift = 17
    let blockSize = 1 <<< shift
    let smallPart = blockSize - 1
    let mutable arrays = Array.init initArraysCount (fun _ -> Array.zeroCreate<'T> blockSize)
    let mutable cap = blockSize * arrays.Length
    let mutable nextAllocate = cap
    let mutable active = 1

    let countTop c = 
        let tail = c % blockSize 
        if c <> 0 
        then if tail = 0 then blockSize - 1 else tail - 1
        else -1

    interface IEnumerable<'T> with
        member this.GetEnumerator () =
            let e = 
                seq {for a in arrays do yield! a}
                |> Seq.take this.Length
            e.GetEnumerator()

        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> IEnumerator

    ///Adds element to the block resize array.
    member this.Add x =
        if count = nextAllocate then
            if count = cap then
                let oldArrays = arrays
                arrays <- Array.zeroCreate (arrays.Length * 2)
                for i = 0 to oldArrays.Length-1 do
                    arrays.[i] <- oldArrays.[i]
                cap <- blockSize * arrays.Length
            arrays.[count >>> shift] <- Array.zeroCreate blockSize
            nextAllocate <- nextAllocate + blockSize
            active <- active + 1
        arrays.[count >>> shift].[count &&& smallPart] <- x
        count <- count + 1

    ///Allows to get-set element to block resize array.
    member this.Item
        with get (i:int) = arrays.[i >>> shift].[i &&& smallPart]
        and set i v = arrays.[i >>> shift].[i &&& smallPart] <- v

    ///Deletes block given index.
    member this.DeleteBlock i = arrays.[i] <- null
    
    ///Returns the length of a block resize array.
    member this.Length = count
    
    member private this.Arrays = arrays

    ///Returns the shift size for block resize array.
    member this.Shift = shift

    member private this.setCount newCount = 
        count <- newCount
    
    member private this.SetArrays arr =
        arrays <- arr
    
    member private this.SetActive i = active <- i
    
    ///Creates an array from the given block resize array.
    member this.ToArray() =
        let res = Array.zeroCreate count
        for i = 0 to (count >>> shift) - 1 do
            Array.blit arrays.[i] 0 res (i <<< shift) blockSize
        if (count &&& smallPart) <> 0 then
            let i = count >>> shift
            Array.blit arrays.[i] 0 res (i <<< shift) (count &&& smallPart)
        res
    
    member private this.SetArr i arr = arrays.[i] <- arr

    ///Creates a block resize array given the dimension and a generator function to compute the elements.
    static member Init initCount (f : int -> 'T)  =
        let bra = new BlockResizeArray<_>()
        let blockSize = 1 <<< bra.Shift
        let blocksCount = initCount / blockSize
        let smallPartCount = initCount % blockSize
        if blocksCount = 0
        then
            for i in 0..initCount - 1 do
                bra.Arrays.[0].[i] <- f i
        else
            let l = blocksCount + 1
            let newArr = Array.init l (fun i -> if i < blocksCount then Array.init blockSize (fun j -> f (blockSize * i + j)) else Array.zeroCreate<'T> blockSize)
            if smallPartCount <> 0
            then
                for i in 0..smallPartCount - 1 do
                    newArr.[blocksCount].[i] <- f (blockSize * blocksCount + i)
            bra.SetArrays newArr
        bra.SetActive (blocksCount + 1)
        bra.setCount initCount
        bra

    ///Creates a block resize array where the entries are initially the default value Unchecked.defaultof<'T>.
    static member ZeroCreate initCount =
        let bra = new BlockResizeArray<_>()
        let blockSize = 1 <<< bra.Shift
        let blocksCount =  initCount / blockSize
        if blocksCount <> 0
        then          
            let l = blocksCount + 1
            let newArr = Array.init l (fun _ -> Array.zeroCreate<_> blockSize)
            bra.SetArrays newArr
        bra.setCount initCount
        bra.SetActive (blocksCount + 1)
        bra

    ///Returns the first element for which the given function returns true. Return None if no such element exists.
    member this.TryFind f =
        let mutable c = None
        let mutable i = 0
        if active > 1
        then
            while c.IsNone && i < active - 1 do
                c <- Array.tryFind f arrays.[i]
                i <- i + 1
        let top = countTop count
        i <- 0
        if top > 0
        then
            while c.IsNone && i < top do
                if f arrays.[active - 1].[i]
                then 
                    c <- Some arrays.[active - 1].[i]
                i <- i + 1
        c

    ///Returns the first element for which the given function returns true. Raise KeyNotFoundException if no such element exists.
    member this.Find f = 
        let mutable c = this.TryFind f  
        if c.IsSome then c.Value else raise(System.Collections.Generic.KeyNotFoundException())
    
    
    ///Applies a function to each element of the collection, threading an accumulator argument through the computation.
    member this.Fold (folder : 'State -> 'T -> 'State) (state : 'State) : 'State =
        let mutable state = state
        if active > 1
        then            
            for i in 0..active - 2 do
                state <- Array.fold folder state arrays.[i]
        let a = arrays.[active - 1]
        let top = countTop count
        for i in 0..top do
            state <- folder state a.[i]
        state
            
    ///Returns a new collection containing only the elements of the collection for which the given predicate returns true.
    member this.Filter f =
        let bra = new BlockResizeArray<_>()
        let arr = new ResizeArray<_>()
        let mutable a = Array.zeroCreate blockSize
        let mutable index = 0
        let mutable resCount = 0
        for i = 0 to active - 1 do
            for j = 0 to blockSize - 1 do
                let x = arrays.[i].[j] 
                if f x && i * blockSize + j < this.Length
                then 
                    a.[index] <- x
                    index <- index + 1
                    resCount <- resCount + 1
                    if index = blockSize
                    then
                        arr.Add a
                        a <- Array.zeroCreate blockSize
                        index <- 0
        if index <> 0
        then arr.Add a
        let l = arr.Count
        let resArr = Array.zeroCreate l
        for i = 0 to l - 1 do
            resArr.[i] <- arr.[i]
        bra.SetArrays resArr
        bra.setCount resCount
        bra.SetActive l
        bra   

    ///Builds a new block resize array whose elements are the results of applying the given function to each of the elements of the array.
    member this.Map (f : 'T -> 'U) =
        let result = new BlockResizeArray<'U>()
        let arr = Array.zeroCreate<_> active
        if active > 1
        then
            for i = 0 to active - 2 do
                arr.[i] <- Array.map f arrays.[i]
        let a = arrays.[active - 1]
        arr.[active - 1] <- Array.zeroCreate<_> blockSize
        let top = countTop count
        for i in 0..top do
            arr.[active - 1].[i] <- f a.[i]
        result.SetArrays arr
        result.setCount count
        result.SetActive active
        result

    ///Applies the given function to each element of the block resize array.
    member this.Iter f =
        if active > 1
        then
            for i = 0 to active - 2 do
                Array.iter f arrays.[i]
        let a = arrays.[active - 1]
        let top = countTop count
        for i in 0..top do
            f a.[i] 

[<RequireQualifiedAccess>]
module BlockResizeArray = 

    ///Applies the given function to each element of the block resize array.
    let iter (f : 'T -> unit) (bra : BlockResizeArray<_>) = bra.Iter f

    ///Applies a function to each element of the collection, threading an accumulator argument through the computation.
    let fold f s (bra : BlockResizeArray<_>) = bra.Fold f s

    ///Returns the length of a block resize array.
    let inline count (bra : BlockResizeArray<_>) = bra.Length  

    ///Builds a new block resize array whose elements are the results of applying the given function to each of the elements of the array.
    let map (f : 'T -> 'U) (bra : BlockResizeArray<_>) = bra.Map f

    ///Returns a new collection containing only the elements of the collection for which the given predicate returns true.
    let filter (f : 'T -> bool) (bra : BlockResizeArray<_>) = bra.Filter f

    ///Returns the first element for which the given function returns true. Return None if no such element exists.
    let tryFind (f : 'T -> bool) (bra : BlockResizeArray<_>) = bra.TryFind f

    ///Returns the first element for which the given function returns true. 
    let find (f : 'T -> bool) (bra : BlockResizeArray<_>) = bra.Find f

    ///Creates an array from the given block resize array.
    let toArray (bra : BlockResizeArray<_>) = bra.ToArray () 
    
    ///Adds element to the block resize array.
    let add x (bra : BlockResizeArray<_>) = bra.Add x           

            

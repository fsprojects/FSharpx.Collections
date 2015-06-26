namespace FSharpx.Collections.Experimental

// Resize array fith fixed size block memory allocation.
// Provide more optimal space usage for huge arrays than standard ResizeArray.
// Basic version created by Avdyukhin Dmitry <dimonbv@gmail.com>
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
    
    interface IEnumerable<'T> with
        member this.GetEnumerator () =
            let e = 
                seq {for a in arrays do yield! a}
                |> Seq.take this.Count
            e.GetEnumerator()

        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> IEnumerator

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
    
    member this.Item
        with get (i:int) = arrays.[i >>> shift].[i &&& smallPart]
        and set i v = arrays.[i >>> shift].[i &&& smallPart] <- v

    member this.DeleteBlock i = arrays.[i] <- null
    member this.Count = count
    member private this.Arrays = arrays
    member this.Shift = shift

    member private this.setCount newCount = 
        count <- newCount
    
    member private this.SetArrays arr =
        arrays <- arr
    
    member private this.SetActive i = active <- i

    member this.ToArray() =
        let res = Array.zeroCreate count
        for i = 0 to (count >>> shift) - 1 do
            Array.blit arrays.[i] 0 res (i <<< shift) blockSize
        if (count &&& smallPart) <> 0 then
            let i = count >>> shift
            Array.blit arrays.[i] 0 res (i <<< shift) (count &&& smallPart)
        res
    
    member private this.SetArr i arr = arrays.[i] <- arr

    static member Init initCount (f : int -> 'T)  =
        let bra = new BlockResizeArray<_>()
        let blockSize = 1 <<< bra.Shift
        let blocksCount =  int <| initCount / blockSize
        let smallPartCount = initCount % blockSize
        if blocksCount = 0
        then
             let arr = Array.init initCount f
             Array.blit arr 0 bra.Arrays.[0] 0 initCount
        else
            let l = blocksCount * 2
            let newArr = Array.init l (fun i -> if i < blocksCount then Array.init blockSize (fun j -> f (blockSize * i + j)) else Array.zeroCreate<'T> blockSize)
            if smallPartCount <> 0
            then
                let s = Array.init smallPartCount (fun j -> f (blockSize * blocksCount + j))
                Array.blit s 0 newArr.[blocksCount] 0 smallPartCount 
            bra.SetArrays newArr
        bra.SetActive (blocksCount + 1)
        bra.setCount initCount
        bra

    static member ZeroCreate initCount =
        let bra = new BlockResizeArray<_>()
        let blockSize = 1 <<< bra.Shift
        let blocksCount =  int <| initCount / blockSize
        let smallPartCount = initCount % blockSize
        if blocksCount <> 0
        then          
            let l = blocksCount * 2
            let newArr = Array.init l (fun _ -> Array.zeroCreate<_> blockSize)
            bra.SetArrays newArr
        bra.setCount initCount
        bra.SetActive (blocksCount + 1)
        bra

    member this.Find f = 
        let mutable c = None
        let mutable i = 0
        while(c.IsNone && i < active) do
            c <- Array.tryFind f arrays.[i]
            i <- i + 1    
        if c.IsSome then c.Value else raise(System.Collections.Generic.KeyNotFoundException())

    member this.TryFind f =
        let mutable c = None
        let mutable i = 0
        while(c.IsNone && i < active) do
            c <- Array.tryFind f arrays.[i]
            i <- i + 1
        c
    
    member this.Filter f =
        let bra = new BlockResizeArray<_>()
        let arr = new ResizeArray<_>()
        let mutable a = Array.zeroCreate blockSize
        arr.Add a
        let mutable index = 0
        let mutable resCount = 0
        for i = 0 to active - 1 do
            for j = 0 to blockSize - 1 do
                let x = arrays.[i].[j] 
                if f x && i * blockSize + j < this.Count
                then 
                    a.[index] <- x
                    index <- index + 1
                    resCount <- resCount + 1
                    if index = blockSize
                    then
                        arr.Add a
                        a <- Array.zeroCreate blockSize
                        index <- 0
        let l = arr.Count
        let resArr = Array.zeroCreate (l * 2)
        for i = 0 to l - 1 do
            resArr.[i] <- arr.[i]
        bra.SetArrays resArr
        bra.setCount resCount
        bra.SetActive l
        bra   

    member this.Map (f : 'T -> 'U) =
        let result = new BlockResizeArray<'U>()
        let arr = Array.zeroCreate<_> arrays.Length
        for i = 0 to active - 1 do
            arr.[i] <- Array.map f arrays.[i]
        result.SetArrays arr
        result

    member this.Iter f =
        let smallPartCount = count % blockSize
        for i = 0 to active - 1 do
            let x = i
            Array.iter f arrays.[i] 
            
                
            

            

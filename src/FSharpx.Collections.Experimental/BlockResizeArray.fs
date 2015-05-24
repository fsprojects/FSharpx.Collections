namespace FSharpx.Collections

// Resize array fith fixed size block memory allocation.
// Provide more optimal space usage for huge arrays than standard ResizeArray.
// Basic version created by Avdyukhin Dmitry <dimonbv@gmail.com>

type BlockResizeArray<'T> () =
    let initArraysCount = 1
    let mutable count = 0
    let shift = 17
    let blockSize = 1 <<< shift
    let smallPart = blockSize - 1
    let mutable arrays =    Array.init initArraysCount (fun _ -> Array.zeroCreate blockSize)
    let mutable cap = blockSize * arrays.Length
    let mutable nextAllocate = cap
    member this.Add (x : 'T) =
        if count = nextAllocate then
            if count = cap then
                let oldArrays = arrays
                arrays <- Array.zeroCreate (arrays.Length * 2)
                for i = 0 to oldArrays.Length-1 do
                    arrays.[i] <- oldArrays.[i]
                cap <- blockSize * arrays.Length
            arrays.[count >>> shift] <- Array.zeroCreate blockSize
            nextAllocate <- nextAllocate + blockSize
        arrays.[count >>> shift].[count &&& smallPart] <- x
        count <- count + 1

    member this.Item i =
        arrays.[i >>> shift].[i &&& smallPart]

    member this.Set i value =
        arrays.[i >>> shift].[i &&& smallPart] <- value

    member this.DeleteBlock i = arrays.[i] <- null
    member this.Count = count
    member this.Shift = shift

    member this.ToArray() =
        let res = Array.zeroCreate count
        for i = 0 to (count >>> shift) - 1 do
            Array.blit arrays.[i] 0 res (i <<< shift) blockSize
        if (count &&& smallPart) <> 0 then
            let i = count >>> shift
            Array.blit arrays.[i] 0 res (i <<< shift) (count &&& smallPart)
        res

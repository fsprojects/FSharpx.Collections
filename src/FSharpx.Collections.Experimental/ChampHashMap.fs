namespace FSharpx.Collections.Experimental
open System.Collections.Specialized


module BitUtilities =
    let inline bitcount bitmap: int32 = 
        let count2 = bitmap - ((bitmap >>> 1) &&& 0x55555555)
        let count4 = (count2 &&& 0x33333333) + ((count2 >>> 2) &&& 0x33333333)
        let count8 = (count4 + (count4 >>> 4)) &&& 0x0f0f0f0f
        (count8 * 0x01010101) >>> 24

    let inline index (bitmap: BitVector32) pos = 
        bitcount (bitmap.Data &&& (pos - 1)) 

    let [<Literal>] PartitionSize = 5

    let [<Literal>] PartitionMaxValue = 31s

[<Struct>]
type internal KeyValuePair<'TKey, 'TValue> = {Key: 'TKey; Value: 'TValue}

type internal Node<'TKey, 'TValue when 'TKey : equality> = 
    | BitmapNode of entryMap: BitVector32 * nodeMap: BitVector32 * items: array<KeyValuePair<'TKey, 'TValue>> * nodes: array<Node<'TKey, 'TValue>>
    | CollisionNode of items: array<KeyValuePair<'TKey, 'TValue>> * hash: BitVector32
    | EmptyNode

open BitUtilities
open FSharpx.Collections
open System.Collections.Specialized
module internal Node = 

    let inline keyNotFound key =
        raise (Exceptions.KeyNotFound key)

    let inline insert (array: array<'T>) index value =
        let newItems = Array.zeroCreate (array.Length + 1)
        Array.blit array 0 newItems 0 index
        Array.blit array index newItems (index + 1) (array.Length - index)
        Array.set newItems index value
        newItems

    let rec getValue node key (hash: BitVector32) (section: BitVector32.Section) = 
        match node with
        | BitmapNode(entryMap, nodeMap, items, nodes) -> 
            let hashIndex = hash.Item section
            let mask = BitVector32.CreateMask hashIndex
            if (entryMap.Item mask) then
                let entryIndex = index entryMap mask
                if ((Array.get items entryIndex).Key = key) then
                    (Array.get items entryIndex).Value
                else keyNotFound key
            elif (nodeMap.Item mask) then
                let subNode = Array.get nodes <| index nodeMap mask
                getValue subNode key hash (BitVector32.CreateSection (PartitionMaxValue, section))
            else keyNotFound key
        | EmptyNode -> keyNotFound key
        | _ -> failwith "Not supported"

    let rec tryGetValue node key (hash: BitVector32) (section: BitVector32.Section) = 
        match node with
        | BitmapNode(entryMap, nodeMap, items, nodes) -> 
            let hashIndex = hash.Item section
            let mask = BitVector32.CreateMask hashIndex
            if (entryMap.Item mask) then
                let entryIndex = index entryMap mask
                if ((Array.get items entryIndex).Key = key) then
                    Some (Array.get items entryIndex).Value
                else None
            elif (nodeMap.Item mask) then
                let subNode = Array.get nodes <| index nodeMap mask
                tryGetValue subNode key hash (BitVector32.CreateSection (PartitionMaxValue, section))
            else None
        | EmptyNode -> None
        | _ -> failwith "Not supported"

    let rec merge pair1 pair2 (pair1Hash: BitVector32) (pair2Hash: BitVector32) (section: BitVector32.Section) = 
        if (section.Offset >= 25s) then
            CollisionNode([|pair1;pair2|], pair1Hash)
        else
            let nextLevel = BitVector32.CreateSection (PartitionMaxValue, section)
            let pair1Index = pair1Hash.Item nextLevel
            let pair2Index = pair2Hash.Item nextLevel
            if (pair1Index <> pair2Index) then
                let mutable dataMap = BitVector32 (BitVector32.CreateMask pair2Index)
                dataMap.Item (BitVector32.CreateMask pair2Index) <- true
                if (pair1Index < pair2Index) then
                    BitmapNode(dataMap, BitVector32 0, [|pair1;pair2|], Array.empty)
                else 
                    BitmapNode(dataMap, BitVector32 0, [|pair2;pair1|], Array.empty)
            else 
                let node = merge pair1 pair2 pair1Hash pair2Hash (BitVector32.CreateSection (PartitionMaxValue, section))
                let nodeMap = BitVector32 (BitVector32.CreateMask pair1Index)
                BitmapNode(BitVector32 0, nodeMap, Array.empty, [|node|])
    
    let rec update node inplace change (hash: BitVector32) (section: BitVector32.Section) =
        match node with
        | EmptyNode -> 
            let dataMap = hash.Item section |> BitVector32.CreateMask |> BitVector32
            let items = [|change|]
            let nodes = Array.empty
            let nodeMap = BitVector32(0)
            BitmapNode(nodeMap, dataMap, items, nodes)
        | BitmapNode(entryMap, nodeMap, items, nodes) ->
            let hashIndex = hash.Item section
            let mask = BitVector32.CreateMask hashIndex
            let inline set items index value inplace = 
                if (inplace) then
                    Array.set items index value |> ignore
                    items
                else
                    let copy = Array.copy items
                    Array.set copy index value |> ignore
                    copy
            if (entryMap.Item mask) then
                let entryIndex = index entryMap mask
                if ((Array.get items entryIndex).Key = change.Key) then
                    let newItems = set items entryIndex change inplace
                    BitmapNode(nodeMap, entryMap, newItems, nodes)
                else 
                    let currentEntry = Array.get items entryIndex
                    let currentHash = BitVector32(int32(currentEntry.Key.GetHashCode()))
                    let node = merge change currentEntry hash currentHash section
                    let newItems = Array.filter (fun elem -> elem.Key <> currentEntry.Key) items
                    let mutable newEntryMap = entryMap
                    newEntryMap.Item mask <- false
                    let mutable newNodeMap = nodeMap
                    newNodeMap.Item mask <- true
                    let newNodes = insert nodes entryIndex node
                    BitmapNode(entryMap, newNodeMap, newItems, newNodes)
            elif (nodeMap.Item mask) then
                let nodeIndex = index nodeMap mask
                let nodeToUpdate = Array.get nodes nodeIndex 
                let newNode = update nodeToUpdate inplace change hash (BitVector32.CreateSection (PartitionMaxValue, section))
                let newNodes = set nodes nodeIndex newNode inplace
                BitmapNode(nodeMap, entryMap, items, newNodes)
            else 
                let entryIndex = index entryMap mask
                let mutable entries = entryMap
                entries.Item mask <- true
                let newItems = insert items entryIndex change
                BitmapNode(nodeMap, entries, newItems, nodes)
        | CollisionNode(_) -> failwith "Not implemented"

    

type ChampHashMap<'TKey,'TValue when 'TKey : equality>() = 
    member this.Length : int = 0
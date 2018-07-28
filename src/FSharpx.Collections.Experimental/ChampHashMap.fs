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

    let inline mask index =
        1 <<< index

    let [<Literal>] PartitionSize = 5

    let [<Literal>] PartitionMaxValue = 31s

[<Struct>]
type internal KeyValuePair<'TKey,'TValue> = {Key: 'TKey; Value: 'TValue}

type internal Node<[<EqualityConditionalOn>]'TKey, [<EqualityConditionalOn>]'TValue when 'TKey : equality> = 
    | BitmapNode of entryMap: BitVector32 * nodeMap: BitVector32 * items: array<KeyValuePair<'TKey, 'TValue>> * nodes: array<Node<'TKey, 'TValue>>
    | CollisionNode of items: array<KeyValuePair<'TKey, 'TValue>> * hash: BitVector32
    | EmptyNode

open BitUtilities
open FSharpx.Collections
module internal Node = 
       
    let set items index value inplace = 
        if (inplace) then
            Array.set items index value |> ignore
            items
        else
            let copy = Array.copy items
            Array.set copy index value |> ignore
            copy

    let keyNotFound key =
        raise (Exceptions.KeyNotFound key)

    let insert (array: array<'T>) index value =
        let newItems = Array.zeroCreate (array.Length + 1)
        Array.blit array 0 newItems 0 index
        Array.blit array index newItems (index + 1) (array.Length - index)
        Array.set newItems index value
        newItems

    let removeAt (array: array<'T>) index = 
        let newItems = Array.zeroCreate (array.Length - 1)
        if (index > 0) then
            Array.blit array 0 newItems 0 index
        if (index + 1 < array.Length) then
            Array.blit array (index + 1) newItems index (array.Length - index - 1)
        newItems

    let rec getValue node key (hash: BitVector32) (section: BitVector32.Section) = 
        match node with
        | BitmapNode(entryMap, nodeMap, items, nodes) -> 
            let hashIndex = hash.[section]
            let mask = mask hashIndex
            if (entryMap.[mask]) then
                let entryIndex = index entryMap mask
                if ((Array.get items entryIndex).Key = key) then
                    (Array.get items entryIndex).Value
                else keyNotFound key
            elif (nodeMap.[mask]) then
                let subNode = Array.get nodes <| index nodeMap mask
                getValue subNode key hash (BitVector32.CreateSection (PartitionMaxValue, section))
            else keyNotFound key
        | EmptyNode -> keyNotFound key
        | CollisionNode(items, _) -> (Array.find (fun k -> k.Key = key) items).Value

    let rec tryGetValue node key (hash: BitVector32) (section: BitVector32.Section) = 
        match node with
        | BitmapNode(entryMap, nodeMap, items, nodes) -> 
            let hashIndex = hash.[section]
            let mask = mask hashIndex
            if (entryMap.[mask]) then
                let entryIndex = index entryMap mask
                if ((Array.get items entryIndex).Key = key) then
                    Some (Array.get items entryIndex).Value
                else None
            elif (nodeMap.[mask]) then
                let subNode = Array.get nodes <| index nodeMap mask
                tryGetValue subNode key hash (BitVector32.CreateSection (PartitionMaxValue, section))
            else None
        | EmptyNode -> None
        | CollisionNode(items, _) -> 
            match (Array.tryFind (fun k -> k.Key = key) items) with
            | Some(pair) -> Some(pair.Value)
            | None -> None

    let rec merge pair1 pair2 (pair1Hash: BitVector32) (pair2Hash: BitVector32) (section: BitVector32.Section) = 
        if (section.Offset >= 25s) then
            CollisionNode([|pair1;pair2|], pair1Hash)
        else
            let nextLevel = BitVector32.CreateSection (PartitionMaxValue, section)
            let pair1Index = pair1Hash.Item nextLevel
            let pair2Index = pair2Hash.Item nextLevel
            if (pair1Index <> pair2Index) then
                let mutable dataMap = BitVector32 (mask pair1Index)
                dataMap.[(mask pair2Index)] <- true
                if (pair1Index < pair2Index) then
                    BitmapNode(dataMap, BitVector32 0, [|pair1;pair2|], Array.empty)
                else 
                    BitmapNode(dataMap, BitVector32 0, [|pair2;pair1|], Array.empty)
            else 
                let node = merge pair1 pair2 pair1Hash pair2Hash (nextLevel)
                let nodeMap = BitVector32 (mask pair1Index)
                BitmapNode(BitVector32 0, nodeMap, Array.empty, [|node|])
    
    let rec update node inplace change (hash: BitVector32) (section: BitVector32.Section) =
        match node with
        | EmptyNode -> 
            let dataMap = hash.[section] |> mask |> BitVector32
            let items = [|change|]
            let nodes = Array.empty
            let nodeMap = BitVector32(0)
            BitmapNode(dataMap, nodeMap, items, nodes)
        | BitmapNode(entryMap, nodeMap, items, nodes) ->
            let hashIndex = hash.[section]
            let mask = mask hashIndex
            if (entryMap.[mask]) then
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
                    let nodeIndex = index nodeMap mask
                    let newNodes = insert nodes nodeIndex node
                    BitmapNode(newEntryMap, newNodeMap, newItems, newNodes)
            elif (nodeMap.[mask]) then
                let nodeIndex = index nodeMap mask
                let nodeToUpdate = Array.get nodes nodeIndex 
                let newNode = update nodeToUpdate inplace change hash (BitVector32.CreateSection (PartitionMaxValue, section))
                let newNodes = set nodes nodeIndex newNode inplace
                BitmapNode(entryMap, nodeMap, items, newNodes)
            else 
                let entryIndex = index entryMap mask
                let mutable entries = entryMap
                entries.Item mask <- true
                let newItems = insert items entryIndex change
                BitmapNode(entries, nodeMap, newItems, nodes)
        | CollisionNode(items, hash) -> 
            match Array.tryFindIndex (fun i -> i.Key = change.Key) items with
            | Some(index) -> 
                let newArr = set items index change false
                CollisionNode(newArr, hash)
            | None ->
                let newArr = Array.append items [|change|]
                CollisionNode(newArr, hash)

    let rec remove node key (hash: BitVector32) (section: BitVector32.Section) = 
        match node with
        | EmptyNode -> keyNotFound key
        | CollisionNode(items, _) -> 
            match Array.length items with
            | 0 -> failwith "remove was called on CollisionNode but CollisionNode contained 0 elements"
            | 1 -> EmptyNode
            | 2 -> 
                let item = Array.find (fun i -> i.Key = key) items 
                update EmptyNode false item hash (BitVector32.CreateSection PartitionMaxValue)
            | _ -> CollisionNode(Array.filter (fun i -> i.Key <> key) items, hash)
        | BitmapNode(entryMap, nodeMap, entries, nodes) -> 
            let hashIndex = hash.Item section
            let mask = mask hashIndex
            if (entryMap.[mask]) then
                let ind = index entryMap mask
                if (entries.[ind].Key = key) then
                    let mutable newMap = entryMap
                    newMap.Item mask <- false
                    let newItems = removeAt entries ind
                    BitmapNode(newMap, nodeMap, newItems, nodes)
                else keyNotFound key
            elif (nodeMap.[mask]) then
                let ind = index nodeMap mask
                let subNode = remove nodes.[ind] key hash (BitVector32.CreateSection(PartitionMaxValue, section))
                match subNode with
                | EmptyNode -> failwith "Subnode must have at least one element"
                | BitmapNode(subItemMap, subNodeMap, subItems, subNodes) ->
                    if (Array.length subItems = 1 && Array.length subNodes = 0) then
                        // If the node only has one subnode, make that subnode the new node
                        if (Array.length entries = 0 && Array.length nodes = 1) then
                            BitmapNode(subItemMap, subNodeMap, subItems, subNodes)
                        else
                            let indexToInsert = index entryMap mask 
                            let mutable newNodeMap = nodeMap
                            let mutable newEntryMap = entryMap
                            newNodeMap.[mask] <- false
                            newEntryMap.[mask] <- true
                            let newEntries = insert entries indexToInsert subItems.[0]
                            let newNodes = removeAt nodes ind
                            BitmapNode(newEntryMap, newNodeMap, newEntries, newNodes)
                    else
                        let nodeCopy = Array.copy nodes
                        nodeCopy.[ind] <- subNode
                        BitmapNode(entryMap, nodeMap, entries, nodeCopy)
                | CollisionNode(_, _) -> 
                    let nodeCopy = Array.copy nodes
                    nodeCopy.[ind] <- subNode
                    BitmapNode(entryMap, nodeMap, entries, nodeCopy)
            else 
                node
                
    
open Node
open System
type ChampHashMap<[<EqualityConditionalOn>]'TKey, [<EqualityConditionalOn>]'TValue when 'TKey : equality> private (root: Node<'TKey,'TValue>) = 
    member private this.Root = root

    override this.Equals(other) =
        match other with
        | :? ChampHashMap<'TKey, 'TValue> as map -> Unchecked.equals this.Root map.Root
        | _ -> false
    
    override this.GetHashCode() = Unchecked.hash this.Root

    new() = ChampHashMap(EmptyNode)

    member public this.Item(key, value) = 
        let hashVector = BitVector32(int32(key.GetHashCode()))
        let section = BitVector32.CreateSection(PartitionMaxValue)
        let newRoot = update this.Root false {Key=key; Value=value} hashVector section 
        ChampHashMap(newRoot)

    member private this.retrieveValue key valuefunc = 
        let hashVector = BitVector32(int32(key.GetHashCode()))
        let section = BitVector32.CreateSection(PartitionMaxValue)
        valuefunc this.Root key hashVector section

    member public this.Item key =
        this.retrieveValue key getValue
    
    member public this.TryGetValue key = 
        this.retrieveValue key tryGetValue

    member public this.Add key value =
        let hashVector = BitVector32(int32(key.GetHashCode()))
        let section = BitVector32.CreateSection(PartitionMaxValue)
        let newRoot = update this.Root false {Key=key; Value=value} hashVector section 
        ChampHashMap(newRoot)

    member public this.Remove key = 
        let hashVector = BitVector32(int32(key.GetHashCode()))
        let section = BitVector32.CreateSection PartitionMaxValue
        let newRoot = remove this.Root key hashVector section
        ChampHashMap(newRoot)

    interface IEquatable<ChampHashMap<'TKey, 'TValue>> with
        member this.Equals(other) = Unchecked.equals root other.Root
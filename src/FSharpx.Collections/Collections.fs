namespace FSharpx.Collections

open System
open System.Linq
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices

/// Extensions for F#'s Seq module.
module Seq =

    /// Prepends `x` to the seq `xs`
    let cons x xs = seq {
        yield x
        yield! xs
    }

    /// Returns the head and tail of the seq. If the seq is empty, returns `None`.
    let unCons s = if Seq.isEmpty s
                   then None
                   else Some (Seq.head s, Seq.tail s)

    /// <summary>
    /// Adds an index to a sequence
    /// </summary>
    /// <param name="a"></param>
    let inline index a = Seq.mapi (fun a b -> a,b) a

    /// <summary>
    /// Returns the first element (with its index) for which the given function returns true.
    /// Return None if no such element exists.
    /// </summary>
    /// <param name="pred">Predicate</param>
    /// <param name="l">Sequence</param>
    let tryFindWithIndex pred l =
        l |> index |> Seq.tryFind (fun (_,v) -> pred v)

    /// Returns the only element of the sequence for which the given function returns true.
    let inline findExactlyOne (predicate : 'a -> bool) (source:seq<'a>) : 'a =
        source |> Seq.filter predicate |> Seq.exactlyOne

    let inline lift2 f l1 l2 = 
        seq {
            for i in l1 do
                for j in l2 do
                    yield f i j }

    /// Will iterate the current sequence until the given predicate is statisfied
    let iterBreak (f:'T -> bool) (seq:seq<_>) =
        use en = seq.GetEnumerator()
        let mutable run = true
        while en.MoveNext() && run do
            run <- f en.Current

    /// The same as Seq.average except will return None if the seq is empty
    let inline tryAverage (seq : seq<(^a)>) : ^a option =
        use e = seq.GetEnumerator()
        let mutable acc = LanguagePrimitives.GenericZero< (^a) >
        let mutable count = 0
        while e.MoveNext() do
            acc <- acc + e.Current
            count <- count + 1
        if count = 0
        then None
        else Some(LanguagePrimitives.DivideByInt< (^a) > acc count)


    /// Splits a sequences at the given index
    let splitAt n seq = (Seq.take n seq, Seq.skip n seq)

    /// Splits a sequences up to the point where the predicate holds
    let span predicate source = (Seq.takeWhile predicate source, Seq.skipWhile predicate source)

    /// Applies a key-generating function to each element of a sequence and yields a sequence of unique keys and a sequence of all elements that have each key.
    /// This function groups together only neighbouring elements in the seq.
    let groupNeighboursBy (projection:'T->'Key) source =
        let rec go s =
            match Seq.tryHead s with
            | None   -> Seq.empty
            | Some e -> let pe = projection e
                        let predicate x = projection x = pe
                        let neighbours, rest = span predicate s
                        cons (pe, neighbours) (go rest)
        go source

    /// Converts a streamReader into a seq yielding on each line
    let ofStreamReader (streamReader : System.IO.StreamReader) =
         seq {
                use sr = streamReader
                while not(sr.EndOfStream) do
                    yield sr.ReadLine()
             }

    /// Converts a Stream into a sequence of bytes
    let ofStreamByByte (stream: System.IO.Stream) =
        seq { while stream.Length <> stream.Position do
                let x = stream.ReadByte()
                if (int x) < 0 then ()
                else yield x }

    /// Converts a stream into a seq of byte[] where the array is of the length given
    /// Note: the last chunk maybe less than the given chunk size
    let ofStreamByChunk chunkSize (stream: System.IO.Stream) =
        let buffer = Array.zeroCreate<byte> chunkSize
        seq { while stream.Length <> stream.Position do
                let bytesRead = stream.Read(buffer, 0, chunkSize)
                if bytesRead = 0 then ()
                else yield buffer }

    /// Creates a infinite sequences of the given values
    let asCircular values =
        let rec next () =
            seq {
                for element in values do
                    yield element
                yield! next()
            }
        next()

    /// Creates a infinite sequences of the given values, executing the given function everytime the given seq is exhausted
    let asCircularOnLoop f values =
        let rec next () =
            seq {
                for element in values do
                    yield element
                f()
                yield! next()
            }
        next()

    /// Creates a infinite sequences of the given values returning None everytime the given seq is exhausted
    let asCircularWithBreak values =
        let rec next () =
            seq {
                for element in values do
                    yield Some(element)
                yield None
                yield! next()
            }
        next()

    /// A safe version of seq head
    let tryHead (source : seq<_>) =
        use e = source.GetEnumerator()
        if e.MoveNext()
        then Some(e.Current)
        else None //empty list

    let tail (source : seq<_>) =
        seq {
            use e = source.GetEnumerator()
            if e.MoveNext()
            then
                while e.MoveNext() do
                    yield e.Current
            else invalidArg "source" "source sequence cannot be empty"
        }

    let tailNoFail (source : seq<_>) =
        seq {
            use e = source.GetEnumerator()
            if e.MoveNext()
            then
                while e.MoveNext() do
                    yield e.Current
            else ()
        }

    /// The same as Seq.nth except returns None if the sequence is empty or does not have enough elements
    let tryNth index (source : seq<_>) =
         let rec tryNth' index (e : System.Collections.Generic.IEnumerator<'T>) =
             if not (e.MoveNext()) then None
             else if index < 0 then None
             else if index = 0 then Some(e.Current)
             else tryNth' (index-1) e

         use e = source.GetEnumerator()
         tryNth' index e

    /// The same as Seq.skip except it returns empty if the sequence is empty or does not have enough elements.
    /// Alias for Enumerable.Skip
    let inline skipNoFail count (source: seq<_>) =
        Enumerable.Skip(source, count)

    /// Creates an infinite sequence of the given value
    let repeat a = seq { while true do yield a }

    /// Contracts a seq selecting every n values
    let rec contract n (source : seq<_>) =
        seq {
              let values = source |> skipNoFail (n - 1)
              match values |> tryNth 0 with
              | Some(v) ->
                    yield v
                    yield! contract n (tailNoFail values)
              | None -> ()
        }

    /// Creates a new collection whose elements are the results of applying the given function to the corresponding pairs of elements from the two sequences.
    /// Unlike Seq.map2, if one input sequence is shorter than the other then the remaining elements of the longer sequence are not ignored, they are yielded at the end of the resulting sequence.
    let rec combine f (a : seq<_>) (b : seq<_>) =
        seq {
            use e = a.GetEnumerator()
            use e' = b.GetEnumerator()
            let eNext = ref (e.MoveNext())
            let eNext' = ref (e'.MoveNext())
            while !eNext || !eNext' do
               yield f e.Current e'.Current
               eNext := e.MoveNext()
               eNext' := e'.MoveNext()
        }

    /// Replicates each element in the seq n-times
    let grow n = Seq.collect (fun x -> (repeat x) |> Seq.take n)

    /// Pages the underlying sequence
    let page page pageSize (source : seq<_>) =
          source |> skipNoFail (page * pageSize) |> Seq.truncate pageSize

    let prependToAll sep list =
        seq{
            for element in list do
                yield sep
                yield element
        }

    let intersperse (sep: 'a) (list: 'a seq) : 'a seq =
        seq {
            let notFirst = ref false
            for element in list do
              if !notFirst then yield sep;
              yield element;
              notFirst := true
      }

    /// The catOptions function takes a list of Options and returns a seq of all the Some values.
    let inline catOptions (xs:seq<Option<'a>>) = Seq.choose id xs

    /// Extracts from a seq of Choice all the Choice1Of2 elements. All the Choice1Of2 elements are extracted in order.
    let inline choice1s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        Seq.choose chooser xs

    /// Extracts from a seq of Choice all the Choice2Of2 elements. All the Choice2Of2 elements are extracted in order.
    let inline choice2s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        Seq.choose chooser xs

    /// Partitions a seq of Choice into two seqs. All the Choice1Of2 elements are extracted, in order, to the first component of the output. Similarly the Choice2Of2 elements are extracted to the second component of the output.
    let inline partitionChoices xs =
        (choice1s xs, choice2s xs)

    /// Compares two sequences for equality using the given comparison function, element by element.
    let inline equalsWith eq xs ys = Seq.compareWith (fun x y -> if eq x y then 0 else 1) xs ys = 0


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
/// Extensions for F#'s Array module.
module Array =
    let inline nth i arr = Array.get arr i
    let inline setAt i v arr = Array.set arr i v; arr

    /// Returns the only element of the array for which the given function returns true.
    let inline findExactlyOne (predicate : 'a -> bool) (source:'a[]) : 'a =
        source |> Array.filter predicate |> Array.exactlyOne

    let copyTo sourceStartIndx startIndx source target =
        let targetLength = (Array.length target)
        if startIndx < 0 || startIndx > targetLength - 1 then
            failwith "Start Index outside the bounds of the array"

        let sourceLength = (Array.length source)
        let elementsToCopy =
                    if (targetLength - startIndx - sourceStartIndx) > sourceLength then
                        sourceLength
                    else
                       (targetLength - startIndx - sourceStartIndx)

        Array.blit source sourceStartIndx target startIndx elementsToCopy

    let ofTuple (source : obj) : obj array =
        Microsoft.FSharp.Reflection.FSharpValue.GetTupleFields source

    let toTuple (source : 'T array) : 't =
        let elements = source |> Array.map (fun x -> x :> obj)
        Microsoft.FSharp.Reflection.FSharpValue.MakeTuple(elements, typeof<'t>) :?> 't

    /// Returns an array of sliding windows of data drawn from the source array.
    /// Each window contains the n elements surrounding the current element.
    let centeredWindow n (source: _ []) =
        if n < 0 then invalidArg "n" "n must be a positive integer"
        let lastIndex = source.Length - 1

        let window i _ =
            let windowStartIndex = Math.Max(i - n, 0)
            let windowEndIndex = Math.Min(i + n, lastIndex)
            let arrSize = windowEndIndex - windowStartIndex + 1
            let target = Array.zeroCreate arrSize
            Array.blit source windowStartIndex target 0 arrSize
            target

        Array.mapi window source

    /// Calculates the central moving average for the array using n elements either side
    /// of the point where the mean is being calculated.
    let inline centralMovingAverage n (a:^t []) =
        a |> centeredWindow n |> Array.map (Array.average)

    /// Calculates the central moving average for the array of optional elements using n
    /// elements either side of the point where the mean is being calculated. If any of
    /// the optional elements in the averaging window are None then the average itself
    /// is None.
    let inline centralMovingAverageOfOption n (a:^t option array) =
        a
        |> centeredWindow n
        |> Array.map (fun window ->
                        if Array.exists Option.isNone window
                        then None
                        else Some(Array.averageBy Option.get window))

    /// The catOptions function takes a list of Options and returns an array of all the Some values.
    let inline catOptions (xs:Option<'a>[]) = Array.choose id xs

    /// Extracts from an array of Choice all the Choice1Of2 elements. All the Choice1Of2 elements are extracted in order.
    let inline choice1s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        Array.choose chooser xs

    /// Extracts from an array of Choice all the Choice2Of2 elements. All the Choice2Of2 elements are extracted in order.
    let inline choice2s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        Array.choose chooser xs

    /// Partitions an aray of Choice into two arrays. All the Choice1Of2 elements are extracted, in order, to the first component of the output. Similarly the Choice2Of2 elements are extracted to the second component of the output.
    let inline partitionChoices xs =
        (choice1s xs, choice2s xs)

    /// Compares two arrays for equality using the given comparison function, element by element.
    let inline equalsWith eq xs ys = Array.compareWith (fun x y -> if eq x y then 0 else 1) xs ys = 0


/// Extensions for F#'s List module.
module List =
    /// Curried cons
    let inline cons hd tl = hd::tl

    let inline singleton x = [x]

    /// Returns the only element of the list for which the given function returns true.
    let inline findExactlyOne (predicate : 'a -> bool) (source:'a list) : 'a =
        source |> List.filter predicate |> List.exactlyOne

    let inline lift2 f (l1: _ list) (l2: _ list) = 
        [ for i in l1 do
            for j in l2 do
                yield f i j ]


    let span pred l =
        let rec loop l cont =
            match l with
            | [] -> ([],[])
            | x::[] when pred x -> (cont l, [])
            | x::xs when not (pred x) -> (cont [], l)
            | x::xs when pred x -> loop xs (fun rest -> cont (x::rest))
            | _ -> failwith "Unrecognized pattern"
        loop l id

    let split pred l = span (not << pred) l

    let skipWhile pred l = span pred l |> snd
    let skipUntil pred l = split pred l |> snd
    let takeWhile pred l = span pred l |> fst
    let takeUntil pred l = split pred l |> fst

    let splitAt n l =
        let pred i = i >= n
        let rec loop i l cont =
            match l with
            | [] -> ([],[])
            | x::[] when not (pred i) -> (cont l, [])
            | x::xs when pred i -> (cont [], l)
            | x::xs when not (pred i) -> loop (i+1) xs (fun rest -> cont (x::rest))
            | _ -> failwith "Unrecognized pattern"
        loop 0 l id

    let skip n l = splitAt n l |> snd
    let take n l = splitAt n l |> fst

    /// Applies a key-generating function to each element of a list and yields a list of unique keys and a list of all elements that have each key.
    /// This function groups together only neighbouring elements in the list.
    let groupNeighboursBy (projection:'T->'Key) source =
        let rec go s =
            match s with
            | []     -> []
            | x::xs  -> let pe = projection x
                        let predicate y = projection y = pe
                        let neighbours, rest = span predicate (x::xs)
                        cons (pe, neighbours) (go rest)
        go source

    let inline mapIf pred f =
        List.map (fun x -> if pred x then f x else x)

    /// Behaves like a combination of map and fold;
    /// it applies a function to each element of a list,
    /// passing an accumulating parameter from left to right,
    /// and returning a final value of this accumulator together with the new list.
    let mapAccum f s l =
        let rec loop s l cont =
            match l with
            | [] -> cont (s, [])
            | x::xs ->
                let (s, y) = f s x
                loop s xs (fun (s,ys) -> cont (s, y::ys))
        loop s l id

    let rec transpose lst =
        match lst with
        | (_::_)::_ -> List.map List.head lst :: transpose (List.map List.tail lst)
        | _         -> []

    /// Merges to sequences using the given function to transform the elements for comparision
    let rec mergeBy f (a : _ list) (b : _ list) =
        [
            match a, b with
            | h :: t, h' :: t' when f(h) < f(h') ->
                yield h; yield! mergeBy f t b
            | h :: t, h' :: t' ->
                yield h'; yield! mergeBy f a t'
            | h :: t, [] -> yield! a
            | [], h :: t -> yield! b
            | [], [] -> ()
        ]

    /// Merges two sequences by the default comparer for 'T
    let merge a b = mergeBy id a b


    let pad (amt: int) (elem: 'a) (list: 'a list) : 'a list =
        if amt >=0 then list @ (List.replicate amt elem)
        else list

    let fill (total:int) (elem: 'a) (list: 'a list) =
        if List.length list >= total then
            list
        else
            pad (total - List.length list) elem list

    /// The catOptions function takes a list of Options and returns a list of all the Some values.
    let inline catOptions (xs:Option<'a> list) = List.choose id xs

    /// Extracts from a list of Choice all the Choice1Of2 elements. All the Choice1Of2 elements are extracted in order.
    let inline choice1s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        List.choose chooser xs

    /// Extracts from a list of Choice all the Choice2Of2 elements. All the Choice2Of2 elements are extracted in order.
    let inline choice2s xs =
        let inline chooser a = match a with
                               | Choice1Of2 x -> Some x
                               | _            -> None
        List.choose chooser xs

    /// Partitions a list of Choice into two lists. All the Choice1Of2 elements are extracted, in order, to the first component of the output. Similarly the Choice2Of2 elements are extracted to the second component of the output.
    let inline partitionChoices (xs:Choice<'a, 'b> list) =
        let inline choice f1 f2 a = match a with
                                    | Choice1Of2 x -> f1 x
                                    | Choice2Of2 y -> f2 y
        let inline choice1 x (c1, c2) = (cons x c1, c2       )
        let inline choice2 y (c1, c2) = (c1       , cons y c2)
        List.foldBack (choice choice1 choice2) xs ([], [])

    /// Compares two lists for equality using the given comparison function, element by element.
    let inline equalsWith eq xs ys = List.compareWith (fun x y -> if eq x y then 0 else 1) xs ys = 0


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
/// Extensions for System.Collections.Generic.Dictionary.
module Dictionary =
    let tryFind key (d: IDictionary<_,_>) =
        match d.TryGetValue key with
        | true,v -> Some v
        | _ -> None

    let inline toSeq (d:seq<KeyValuePair<'a, 'b>>) : seq<'a * 'b> =
        d |> Seq.map (fun (KeyValue(x,y)) -> x, y)


[<Extension>]
type ExtraIDictionary() =
    [<Extension>]
    static member inline TryFind(d:IDictionary<'k, 'v>, k:'k) =
        match d.TryGetValue k with
        | true, v -> Some v
        | _ -> None

[<Extension>]
type ExtraIReadOnlyDictionary() =
    [<Extension>]
    static member inline TryFind(d:IReadOnlyDictionary<'k, 'v>, k:'k) =
        match d.TryGetValue k with
        | true, v -> Some v
        | _ -> None


/// Extensions for F#'s Map module.
module Map =
    /// <summary>
    /// Returns a new map made from the given Dictionary (or anything else that implements <code>seq&lt;KeyValuePair&lt;&apos;a, &apos;b&gt;&gt;</code>).
    /// </summary>
    let ofDict (d:seq<KeyValuePair<'a, 'b>>) =
        d |> Dictionary.toSeq |> Map.ofSeq

    let spanWithKey pred map =
        map
        |> Map.fold (fun (l,r) k v ->
            match k,v with
            | (key, value) when pred key -> (l, r |> Map.add key value)
            | (key, value) when not (pred key) -> (l |> Map.add key value, r)
            | _ -> failwith "Unrecognized pattern"
        ) (Map.empty, Map.empty)

    let splitWithKey pred d = spanWithKey (not << pred) d

    /// <summary>
    /// <code>insertWith f key value mp</code> will insert the pair <code>(key, value)</code> into <code>mp</code> if <code>key</code> does not exist in the map.
    /// If the key does exist, the function will insert <code>f new_value old_value</code>.
    /// </summary>
    let insertWith f key value map =
        match Map.tryFind key map with
        | Some oldValue -> map |> Map.add key (f value oldValue)
        | None -> map |> Map.add key value

    /// <summary>
    /// <code>update f k map</code> updates the value <code>x</code> at key <code>k</code> (if it is in the map).
    /// If <code>f x</code> is <code>None</code>, the element is deleted.
    /// If it is <code>Some y</code>, the key is bound to the new value <code>y</code>.
    /// </summary>
    let updateWith f key map =
        let inner v map =
            match f v with
            | Some value -> map |> Map.add key value
            | None -> map |> Map.remove key
        match Map.tryFind key map with
        | Some v -> inner v map
        | None -> map

    let valueList map = map |> Map.toList |> List.unzip |> snd

    
    /// Combines the two Maps into a single Map, with custom decisioning on what happens in case of conflict
    let inline unionWith (decide : 'k -> 'v -> 'v -> 'v) (m1:Map<'k, 'v>) (m2:Map<'k, 'v>) =
        let inline add m k v1 =
            let v = match Map.tryFind k m with
                    | None    -> v1
                    | Some v2 -> decide k v1 v2
            Map.add k v m
        Map.fold add m1 m2

    /// Combines the two Maps into a single Map
    let union (loses: Map<_,_>) (wins: Map<_,_>) =
        Seq.fold (fun m (KeyValue(k,v)) -> Map.add k v m) loses wins

    let choose (f : 'T -> 'b -> 'c option) (map : Map<'T,'b>) =
        (Map.empty, map) ||> Map.fold (fun s k v ->
                    let result = f k v
                    if Option.isSome result
                    then Map.add k result.Value s
                    else s)

    /// Allows to remove many keys from a Map
    let removeMany (keys : seq<'T>) (map : Map<'T,'b>) =
        Seq.fold (fun s key -> Map.remove key s) map keys

    /// Retrieves the values from a Map
    let inline values (map : Map<'T,'b>) = 
        map |> Map.toSeq |> Seq.map snd

    /// Retrieves the keys from a Map    
    let inline keys (map : Map<'T,'b>) = 
        map |> Map.toSeq |> Seq.map fst

    /// Retrieves the key set from a Map
    let inline keySet map = keys map |> Set.ofSeq

    let findOrDefault key defaultValue (map : Map<'T,'b>) =
        defaultArg (map.TryFind key) defaultValue

    /// The catOptions function takes a map of Options and values and returns a map of all the Some keys and values.
    let inline catOptionKeys (table:Map<Option<'a>, 'b>) =
        seq {
            for k, v in Map.toSeq table do
                if k.IsSome then yield k.Value, v
        }
        |> Map.ofSeq

    /// The catOptions function takes a map of keys and Options and returns a map of all the keys and Some values.
    let inline catOptionValues (table:Map<'a, Option<'b>>) =
        let chooser _ vo = vo
        choose chooser table

    /// Compares two maps for equality using the given comparison function, element by element.
    let inline equalsWith (eq : 'k -> 'v -> 'k -> 'v -> bool) (xs:Map<'k, 'v>) (ys:Map<'k, 'v>) =
        let eq' (k1, v1) (k2, v2) = eq k1 v1 k2 v2
        let xs' = Map.toSeq xs
        let ys' = Map.toSeq ys
        Seq.equalsWith eq' xs' ys'


#if FX_PORTABLE
#else
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<Extension>]
/// Extensions for NameValueCollections.
module NameValueCollection =
    open System.Collections.Specialized
    open System.Linq

    /// <summary>
    /// Returns a new <see cref="NameValueCollection"/> with the concatenation of two <see cref="NameValueCollection"/>s
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [<Extension>]
    [<CompiledName("Concat")>]
    let concat a b =
        let x = NameValueCollection()
        x.Add a
        x.Add b
        x

    /// <summary>
    /// In-place add of a key-value pair to a <see cref="NameValueCollection"/>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    let inline addInPlace (x: NameValueCollection) (a,b) = x.Add(a,b)

    /// Adds an element to a copy of an existing NameValueCollection
    let add name value (x: NameValueCollection) =
        let r = NameValueCollection x
        r.Add(name,value)
        r

    /// <summary>
    /// Returns a <see cref="NameValueCollection"/> as an array of key-value pairs.
    /// Note that keys may be duplicated.
    /// </summary>
    /// <param name="a"></param>
    [<Extension>]
    [<CompiledName("ToArray")>]
    let toArray (a: NameValueCollection) =
        a.AllKeys
        |> Array.collect (fun k -> a.GetValues k |> Array.map (fun v -> k,v))

    /// <summary>
    /// Returns a <see cref="NameValueCollection"/> as a sequence of key-value pairs.
    /// Note that keys may be duplicated.
    /// </summary>
    /// <param name="a"></param>
    [<Extension>]
    [<CompiledName("ToEnumerable")>]
    let toSeq (a: NameValueCollection) =
        a.AllKeys
        |> Seq.collect (fun k -> a.GetValues k |> Seq.map (fun v -> k,v))

    /// <summary>
    /// Returns a <see cref="NameValueCollection"/> as a list of key-value pairs.
    /// Note that keys may be duplicated.
    /// </summary>
    /// <param name="a"></param>
    let inline toList a = toSeq a |> Seq.toList

    /// <summary>
    /// Creates a <see cref="NameValueCollection"/> from a list of key-value pairs
    /// </summary>
    /// <param name="l"></param>
    let ofSeq l =
        let x = NameValueCollection()
        Seq.iter (addInPlace x) l
        x

    [<Extension>]
    [<CompiledName("ToLookup")>]
    let toLookup a =
        let s = toSeq a
        s.ToLookup(fst, snd)

    [<Extension>]
    [<CompiledName("AsDictionary")>]
    let asDictionary (x: NameValueCollection) =
        let notimpl() = raise <| NotImplementedException()
        let getEnumerator() =
            let enum = x.GetEnumerator()
            let wrapElem (o: obj) =
                let key = o :?> string
                let values = x.GetValues key
                KeyValuePair(key,values)
            { new IEnumerator<KeyValuePair<string,string[]>> with
                member e.Current = wrapElem enum.Current
                member e.MoveNext() = enum.MoveNext()
                member e.Reset() = enum.Reset()
                member e.Dispose() = ()
                member e.Current = box (wrapElem enum.Current) }
        { new IDictionary<string,string[]> with
            member d.Count = x.Count
            member d.IsReadOnly = false
            member d.Item
                with get k =
                    let v = x.GetValues k
                    if v = null
                        then raise <| KeyNotFoundException(sprintf "Key '%s' not found" k)
                        else v
                and set k v =
                    x.Remove k
                    for i in v do
                        x.Add(k,i)
            member d.Keys = upcast ResizeArray<string>(x.Keys |> Seq.cast)
            member d.Values =
                let values = ResizeArray<string[]>()
                for i in 0..x.Count-1 do
                    values.Add(x.GetValues i)
                upcast values
            member d.Add v = d.Add(v.Key, v.Value)
            member d.Add(key,value) =
                if key = null
                    then raise <| ArgumentNullException("key")
                if d.ContainsKey key
                    then raise <| ArgumentException(sprintf "Duplicate key '%s'" key, "key")
                d.[key] <- value
            member d.Clear() = x.Clear()
            member d.Contains item = x.GetValues(item.Key) = item.Value
            member d.ContainsKey key = x.[key] <> null
            member d.CopyTo(array,arrayIndex) = notimpl()
            member d.GetEnumerator() = getEnumerator()
            member d.GetEnumerator() = getEnumerator() :> IEnumerator
            member d.Remove (item: KeyValuePair<string,string[]>) =
                if d.Contains item then
                    x.Remove item.Key
                    true
                else
                    false
            member d.Remove (key: string) =
                let exists = d.ContainsKey key
                x.Remove key
                exists
            member d.TryGetValue(key: string, value: byref<string[]>) =
                if d.ContainsKey key then
                    value <- d.[key]
                    true
                else false
            }

    [<Extension>]
    [<CompiledName("AsReadonlyDictionary")>]
    let asReadonlyDictionary x =
        let a = asDictionary x
        let notSupported() = raise <| NotSupportedException("Readonly dictionary")
        { new IDictionary<string,string[]> with
            member d.Count = a.Count
            member d.IsReadOnly = true
            member d.Item
                with get k = a.[k]
                and set k v = notSupported()
            member d.Keys = a.Keys
            member d.Values = a.Values
            member d.Add v = notSupported()
            member d.Add(key,value) = notSupported()
            member d.Clear() = notSupported()
            member d.Contains item = a.Contains item
            member d.ContainsKey key = a.ContainsKey key
            member d.CopyTo(array,arrayIndex) = a.CopyTo(array,arrayIndex)
            member d.GetEnumerator() = a.GetEnumerator()
            member d.GetEnumerator() = a.GetEnumerator() :> IEnumerator
            member d.Remove (item: KeyValuePair<string,string[]>) = notSupported(); false
            member d.Remove (key: string) = notSupported(); false
            member d.TryGetValue(key: string, value: byref<string[]>) = a.TryGetValue(key, ref value)
        }

    [<Extension>]
    [<CompiledName("AsLookup")>]
    let asLookup (this: NameValueCollection) =
        let getEnumerator() =
            let e = this.GetEnumerator()
            let wrapElem (o: obj) =
                let key = o :?> string
                let values = this.GetValues key :> seq<string>
                { new IGrouping<string,string> with
                    member x.Key = key
                    member x.GetEnumerator() = values.GetEnumerator()
                    member x.GetEnumerator() = values.GetEnumerator() :> IEnumerator }

            { new IEnumerator<IGrouping<string,string>> with
                member x.Current = wrapElem e.Current
                member x.MoveNext() = e.MoveNext()
                member x.Reset() = e.Reset()
                member x.Dispose() = ()
                member x.Current = box (wrapElem e.Current) }

        { new ILookup<string,string> with
            member x.Count = this.Count
            member x.Item
                with get key =
                    match this.GetValues key with
                    | null -> Seq.empty
                    | a -> upcast a
            member x.Contains key = this.Get key <> null
            member x.GetEnumerator() = getEnumerator()
            member x.GetEnumerator() = getEnumerator() :> IEnumerator }
#endif

/// vector implementation ported from https://github.com/clojure/clojure/blob/master/src/jvm/clojure/lang/APersistentMap.java

namespace FSharpx.Collections

open System.Threading

type PersistentHashMap<[<EqualityConditionalOn>]'T when 'T : equality> (count,root:Node,hasNull, nullValue) =
   
        
    static member Empty() : PersistentHashMap<'T> = PersistentHashMap(0, Unchecked.defaultof<Node>, false, null)
    member this.Length : int = count

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PersistentHashMap = 
    ///O(1), returns an empty PersistentHashMap
    let empty<'T when 'T : equality> = PersistentHashMap.Empty() :> PersistentHashMap<'T>

    ///O(1), returns the count of the elements in the PersistentHashMap
    let length (map:PersistentHashMap<'T>) = map.Length

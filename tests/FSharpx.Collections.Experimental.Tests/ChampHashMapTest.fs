namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto
open System

[<Struct>]
[<CustomEquality>]
[<NoComparison>]
type CollidingKey<'T when 'T : equality> (value:'T) =
    member this.item = value
    override this.GetHashCode() = 1
    override this.Equals(obj: obj) = 
        match obj with
        | :? CollidingKey<'T> as key -> key.item.Equals(this.item)
        | _ -> false

    with interface IEquatable<CollidingKey<'T>> with
        member this.Equals(other) = this.item.Equals(other.item)


module champHashMapTests =
    [<Tests>]
    let testQueue = 
        testList "Experimental ChampHashMap tests" [
            test "Insert 1000 elements and make sure they are retrieved." {
                let startingMap = ChampHashMap<string, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Add (i.ToString()) i) (startingMap) (seq {1..1000})  
                let valExists i = 
                    let returnedVal = fullMap.TryGetValue(i.ToString())
                    match returnedVal with 
                    | Some(value) -> value = i
                    | None -> false
                let truthVal = Seq.forall valExists (seq {1..1000})
                Expect.isTrue truthVal "Inserted objects were not retrieved from hashmap"
            } 
            test "Insert 1000 colliding elements and make sure they are retrieved" {
                let startingMap = ChampHashMap<CollidingKey<string>, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<CollidingKey<string>,int>) (i: int) -> data.Add (CollidingKey(i.ToString())) i) (startingMap) (seq {1..1000})  
                let valExists i = 
                    let returnedVal = fullMap.TryGetValue(CollidingKey(i.ToString()))
                    match returnedVal with 
                    | Some(value) -> value = i
                    | None -> false
                let truthVal = Seq.forall valExists (seq {1..1000})
                Expect.isTrue truthVal "Inserted objects were not retrieved from hashmap"
            }
            test "Insert 1000 elements and then remove the first 500 elements" {
                let startingMap = ChampHashMap<string, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Add ((i.ToString())) i) (startingMap) (seq {1..1000})  
                let filteredMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Remove (i.ToString())) (fullMap) (seq {1..500})
                let valExists i =
                    let returnVal = filteredMap.TryGetValue (i.ToString())
                    match returnVal with
                    | Some(value) -> value = i
                    | None -> false
                let valNotExists i = not (valExists i)
                Expect.all (seq {1..500}) valNotExists "Elements were supposed to be removed but were not removed from the collection"
                Expect.all (seq {501..1000}) valExists "Elements were supposed to be still present in the collection but they were removed"
            }
            test "Insert 1000 colliding elements and then remove the first 500 elements" {
                let startingMap = ChampHashMap<CollidingKey<string>, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<CollidingKey<string>,int>) (i: int) -> data.Add ((CollidingKey(i.ToString()))) i) (startingMap) (seq {1..1000})  
                let filteredMap = Seq.fold (fun (data: ChampHashMap<CollidingKey<string>,int>) (i: int) -> data.Remove (CollidingKey(i.ToString()))) (fullMap) (seq {1..500})
                let valExists i =
                    let returnVal = filteredMap.TryGetValue (CollidingKey(i.ToString()))
                    match returnVal with
                    | Some(value) -> value = i
                    | None -> false
                let valNotExists i = not (valExists i)
                Expect.all (seq {1..500}) valNotExists "Elements were supposed to be removed but were not removed from the collection"
                Expect.all (seq {501..1000}) valExists "Elements were supposed to be still present in the collection but they were removed"
            }
            test "Two references to the same hashmap should be equal" {
                let startingMap = ChampHashMap<string, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Add ((i.ToString())) i) (startingMap) (seq {1..100})
                let secondMap = fullMap
                Expect.isTrue (fullMap = secondMap) "Two references pointing to the same object should be equal"
            }
            test "Two maps holding the same values should be equal" {
                let startingMap = ChampHashMap<string, int>()
                let secondStartingMap = ChampHashMap<string, int>()
                let fullMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Add ((i.ToString())) i) (startingMap) (seq {1..100})
                let secondMap = Seq.fold (fun (data: ChampHashMap<string,int>) (i: int) -> data.Add ((i.ToString())) i) (secondStartingMap) (seq {1..100})
                Expect.isTrue (fullMap = secondMap) "Two maps holding the same values should be equal"
            }
        ]
    
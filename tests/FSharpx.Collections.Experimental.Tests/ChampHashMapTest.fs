namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto

module champHashMapTests =
    [<Tests>]
    let testQueue = 
        testList "CHAMP tests" [
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
        ]
    
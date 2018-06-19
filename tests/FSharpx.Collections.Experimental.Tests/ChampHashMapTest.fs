namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto

module champHashMapTests =
    [<Tests>]
    let testQueue = 
        testList "CHAMP tests" [
            test "Insert 1000 elements and make sure they are retrieved." {
                let map = ChampHashMap<string, int>()
                let fullMap = Seq.fold (fun i -> map.Add i.toString i) map seq {1..1000}  
                let valExists i: int = 
                    match map.TryGetValue i.toString with 
                    | Some(value) -> value = i
                    | None -> false
                let truthVal = Seq.forall valExists seq {1..1000}
                Expect.isTrue truthVal
            } 
        ]
    
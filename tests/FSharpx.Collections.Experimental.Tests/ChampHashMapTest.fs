namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections.Experimental
open Expecto

module champHashMapTests =

    test "Insert 1000 elements and make sure they are retrieved." {
        let map = ChampHashMap<string, int>()
        
    } 
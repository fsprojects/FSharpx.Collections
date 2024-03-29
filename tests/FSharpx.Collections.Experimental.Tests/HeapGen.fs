﻿namespace FSharpx.Collections.Experimental.Tests

open FSharpx.Collections
open FSharpx.Collections.Experimental
open FSharpx.Collections.Experimental.Tests.Properties
open FsCheck

module HeapGen =

    let insertThruList l h =
        List.fold (fun (h': #IHeap<_, 'a>) x -> h'.Insert x) h l

    (*
IHeap generators from random ofSeq and/or snoc elements from random list 
*)
    let maxLeftistHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((LeftistHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let maxLeftistHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((LeftistHeap.ofSeq true x), (x |> List.sort |> List.rev))
    }

    let maxLeftistHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((LeftistHeap.empty true |> insertThruList x), (x |> List.sort |> List.rev))
    }

    let maxLeftistHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((LeftistHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let minLeftistHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((LeftistHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

    let minLeftistHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((LeftistHeap.ofSeq false x), (x |> List.sort))
    }

    let minLeftistHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((LeftistHeap.empty false |> insertThruList x), (x |> List.sort))
    }

    let minLeftistHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((LeftistHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

    (*
IHeap generators from random ofSeq and/or snoc elements from random list 
*)
    let maxBinomialHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((BinomialHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let maxBinomialHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((BinomialHeap.ofSeq true x), (x |> List.sort |> List.rev))
    }

    let maxBinomialHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((BinomialHeap.empty true |> insertThruList x), (x |> List.sort |> List.rev))
    }

    let maxBinomialHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((BinomialHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let minBinomialHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((BinomialHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

    let minBinomialHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((BinomialHeap.ofSeq false x), (x |> List.sort))
    }

    let minBinomialHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((BinomialHeap.empty false |> insertThruList x), (x |> List.sort))
    }

    let minBinomialHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((BinomialHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

    (*
IHeap generators from random ofSeq and/or snoc elements from random list 
*)
    let maxPairingHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((PairingHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let maxPairingHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((PairingHeap.ofSeq true x), (x |> List.sort |> List.rev))
    }

    let maxPairingHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((PairingHeap.empty true |> insertThruList x), (x |> List.sort |> List.rev))
    }

    let maxPairingHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((PairingHeap.ofSeq true x |> insertThruList y), ((x @ y) |> List.sort |> List.rev))
    }

    let minPairingHeapIntGen = gen {
        let! n = Gen.length2thru12
        let! n2 = Gen.length1thru12
        let! x = Gen.listInt n
        let! y = Gen.listInt n2
        return ((PairingHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

    let minPairingHeapIntOfSeqGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((PairingHeap.ofSeq false x), (x |> List.sort))
    }

    let minPairingHeapIntInsertGen = gen {
        let! n = Gen.length1thru12
        let! x = Gen.listInt n
        return ((PairingHeap.empty false |> insertThruList x), (x |> List.sort))
    }

    let minPairingHeapStringGen = gen {
        let! n = Gen.length1thru12
        let! n2 = Gen.length2thru12
        let! x = Gen.listString n
        let! y = Gen.listString n2
        return ((PairingHeap.ofSeq false x |> insertThruList y), ((x @ y) |> List.sort))
    }

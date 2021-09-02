namespace FSharpx.Collections.Tests

open System
open System.Linq
open FSharpx.Collections
open FSharpx.Collections.Tests.Properties
open Expecto
open Expecto.Flip

module SeqTests =
    [<Tests>]
    let testSeq =

        let data = [1.;2.;3.;4.;5.;6.;7.;8.;9.;10.]

        let intersperse elem (list:'a list) = 
            let interpersed = Seq.intersperse elem list

            if Seq.length list = 0 then 
                Seq.length interpersed = 0
            else
                Seq.length interpersed = (Seq.length list) * 2 - 1

        testList "Seq" [
            
            test "index "{
              let a = {'a'..'z'}
              Expect.equal "index" [0,'a'; 1,'b'; 2,'c'; 3,'d'; 4,'e'] (Seq.index a |> Seq.take 5 |> Seq.toList) }

            test "tryFindWithIndex_None" {
              let a = {1..10}
              Expect.isNone "tryFindWithIndex" (Seq.tryFindWithIndex ((<)10) a) }

            test "tryFindWithIndex_Some" {
              let a = {'a'..'z'}
              Expect.equal "" (Some (4,'e')) (Seq.tryFindWithIndex ((=)'e') a) }

            test "lift2" {
                Expect.equal "" [0;2;1;3] (Seq.lift2 (+) [0;1] [0;2] |> Seq.toList) }
    
            test "I should be able to break the iteration of a seq based on a predicate" {
                let result = ref []
                Seq.iterBreak (fun a -> result := a :: !result;  a <= 5.) data
                Expect.equal "iterBreak" [1.;2.;3.;4.;5.;6.] (!result |> List.rev) }

            test "If I tryAverage an empty seq I should get none" {
                Seq.empty<float> 
                |> Seq.tryAverage
                |> Expect.isNone "tryAverage" }

            test "If I tryAverage a none empty seq I should get the average" {
                data |> Seq.tryAverage
                |> Expect.equal "tryAverage" (Some (5.5)) }

            test "If I tryHeadTail and I don't have a head, I should return None" {
                Seq.empty<float> |> Seq.tryHeadTail
                |> Expect.isNone "tryHeadTail" }

            test "If I tryHeadTail a non-empty seq, I should return both head and tail" {
                let data = [1; 2; 3]
                let actual = data |> Seq.tryHeadTail
                Expect.isSome "tryHeadTail" actual
                match actual with
                | Some (head, tail) ->
                    Expect.equal "tryHeadTail" 1 head
                    Expect.sequenceEqual "tryHeadTail" [2; 3] tail
                | _ -> failwith "Unreachable" }                

            test "I should be a to split a seq at an index" {
                let (a,b) = Seq.splitAt 5 data
                Expect.sequenceEqual "splitAt" (List.toSeq [1.;2.;3.;4.;5.]) a
                Expect.sequenceEqual "splitAt" (List.toSeq [6.;7.;8.;9.;10.]) b }

            test "I should be able to turn a stream reader into a sequence" {
                use data = new IO.MemoryStream(Text.Encoding.UTF8.GetBytes("1\r\n2\r\n3\r\n"))
                use reader = new IO.StreamReader(data)
                let expected = List.toSeq ["1";"2";"3"]
                let actual = Seq.ofStreamReader reader
                Expect.isTrue "ofStreamReader" (expected.SequenceEqual(actual));
            }

            test "I should be able to turn a stream into a sequence of bytes" {
                let bytes = Text.Encoding.UTF8.GetBytes("1\r\n2\r\n3\r\n")
                use stream = new IO.MemoryStream(bytes)
                let expected = Array.toSeq bytes
                let actual = Seq.ofStreamByByte stream |> Seq.map (fun x -> byte x)
                Expect.isTrue "ofStreamByByte" (expected.SequenceEqual(actual))
            }

            test "I should be able to turn a stream into a chunked sequence of bytes" {
                let bytes = Text.Encoding.UTF8.GetBytes("1\r\n2\r\n3\r\n")
                use stream = new IO.MemoryStream(bytes)

                Seq.ofStreamByChunk 3 stream
                |> Expect.sequenceEqual 
                    "ofStreamByChunk" ([
                                        Text.Encoding.UTF8.GetBytes("1\r\n")
                                        Text.Encoding.UTF8.GetBytes("2\r\n")
                                        Text.Encoding.UTF8.GetBytes("3\r\n")
                                        ] |> List.toSeq)
            }

            test "I should be able to create a infinite seq of values" {
                let data = [1;2]
                Seq.asCircular [1;2] |> Seq.take 4
                |> Expect.sequenceEqual "asCircular" (List.toSeq [1;2;1;2]) }

            test "I should be able to create a infinite seq of values and have none in between each iteration" {
                let data = [1;2]
                Seq.asCircularWithBreak [1;2] |> Seq.take 5
                |> Expect.sequenceEqual "asCircularWithBreak" (List.toSeq [Some 1;Some 2; None; Some 1; Some 2]) }

            test "I should be able to create a infinite seq of values and call function when seq exhusted" {
                let called = ref false
                let data = [1;2]
                Seq.asCircularOnLoop (fun () -> called := true) [1;2] 
                |> Seq.take 4
                |> Expect.sequenceEqual "asCircularOnLoop" (List.toSeq [1;2;1;2]) }

            test "I should get none if try to get a index outside the seq" {
                Seq.tryNth 20 data
                |> Expect.isNone "tryNth"  }

            test "I should get some if try to get a index inside the seq" {
                Seq.tryNth 2 data
                |> Expect.equal "tryNth" (Some(3.)) }

            test "I should get none when trySkip past the end of the seq" {
                Seq.skipNoFail 20 data
                |> Expect.sequenceEqual "skipNoFail" Seq.empty }

            test "I should get Some when trySkip" {
                Seq.skipNoFail 5 data
                |> Expect.sequenceEqual "skipNoFail" (List.toSeq [6.;7.;8.;9.;10.]) }

            test "I should be able to repeat a single value infinitely" {
                Seq.repeat 1 |> Seq.take 5
                |> Expect.sequenceEqual "repeat" (List.toSeq [1;1;1;1;1]) }

            test "I should be able to get the tail of a sequence" {
                Seq.tail [1;2;3;4]
                |> Expect.sequenceEqual "tail" (List.toSeq [2;3;4]) }

            ptest "I should not be able to get the tail of a empty sequence" {
                Expect.throwsT<_> "empty tail" (fun () -> Seq.tail [] |> ignore) }

            test "I should be able to get the tail of a empty sequence without a fail" {
                Seq.tailNoFail []
                |> Expect.sequenceEqual "tailNoFail" Seq.empty }

            test "I should be able to contract a seq taking every nth value" {
                Seq.contract 5 data
                |> Expect.sequenceEqual "contract" (List.toSeq [5.;10.]) }
    
            test "I should be able to contract a seq sequence by a given ratio" { 
                let actual = Seq.contract 2 (Seq.init 72 (fun i -> 0)) |> Seq.toList
                let expected = Seq.init 36 (fun i -> 0) |> Seq.toList
                Expect.sequenceEqual "contract" expected actual}

            test "I should be able to contract an empty sequence" {
                Expect.sequenceEqual "contract" Seq.empty <| Seq.contract 5 (Seq.empty)}

            test "I should be able to contract a infinite sequence" {
                let actual = Seq.contract 5 (Seq.initInfinite (fun i -> i + 1))
                Expect.sequenceEqual "contract" (List.toSeq [5;10;15;20;25]) (actual |> Seq.take 5) }

            test "Should be able to combine two sequences" {
                let a,b = [1;2;3;4;5], [6;7;8;9;10]
                Seq.combine (+) a b
                |> Expect.sequenceEqual "combine" (List.toSeq [7;9;11;13;15]) }

            test "Should be able to combine two empty sequences" {
                let a,b = [], []
                Expect.sequenceEqual "combine" Seq.empty <| Seq.combine (+) a b }

            test "Should be able to combine two sequences when one is infinite" {
                let a,b = [1;2;3;4;5], (Seq.initInfinite (fun i -> i + 6))
                Seq.combine (+) a b |> Seq.take 5
                |> Expect.sequenceEqual "combine" (List.toSeq [7;9;11;13;15]) }

            test "Should be able to combine two sequences when both are infinite" {
                let a,b = (Seq.initInfinite (fun i -> i + 1)), (Seq.initInfinite (fun i -> i + 6))
                Seq.combine (+) a b |> Seq.take 5
                |> Expect.sequenceEqual "combine" (List.toSeq [7;9;11;13;15]) }

            test "I should be able to expand a seq" {
                let a = [1;2;3;4;5]
                Seq.grow 2 a
                |> Expect.sequenceEqual "grow" (List.toSeq [1;1;2;2;3;3;4;4;5;5]) }
                  
            test "I should be able to page a seq" {
                Seq.page 0 2 data |> Expect.sequenceEqual "page" (List.toSeq [1.;2.])
                Seq.page 1 2 data |> Expect.sequenceEqual "page" (List.toSeq [3.;4.])
                Seq.page 2 2 data |> Expect.sequenceEqual "page" (List.toSeq [5.;6.]) }

            test "I should intersperse a seq" { 
                let a = "foobar".ToCharArray()
                let expected = ['f';',';'o';',';'o';',';'b';',';'a';',';'r'] |> List.toSeq
                
                Expect.sequenceEqual "" expected (a |> Seq.intersperse ',') }

            test "I shouldn't interperse an empty list" { 
                let a = Seq.empty
                Expect.sequenceEqual "" a (a |> Seq.intersperse ',') }

            testPropertyWithConfig config10k "I should interperse always 2n-1 elements" intersperse 
        ]
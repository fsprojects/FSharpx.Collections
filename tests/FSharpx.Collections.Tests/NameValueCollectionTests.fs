namespace FSharpx.Collections.Tests

open System.Collections.Generic
open System.Collections.Specialized
open System.Linq
open FSharpx.Collections
open Expecto
open Expecto.Flip

module NameValueCollectionTests =

    [<Tests>]
    let testNameValueCollection =

        let assertKeyIs (l: ILookup<_,_>) a key = 
          Expect.equal "assertKeyIs" a (l.[key] |> Seq.toList)

        testList "NameValueCollection" [

             test "ofSeq" {
               let n1 = NameValueCollection.ofSeq ["1","uno"; "1","one"; "2","two"]
               let n2 = NameValueCollection()
               n2.Add("1", "uno")
               n2.Add("1", "one")
               n2.Add("2", "two")
               
               Expect.equal "ofSeq" n1.AllKeys n2.AllKeys 
               Expect.equal "ofSeq" (n1.GetValues("1")) (n2.GetValues("1")) }

             test "toSeq" {
               let r = ["1","uno"; "1","one"; "2","two"]
               let a = NameValueCollection.ofSeq r
               let s = NameValueCollection.toSeq a
               Expect.sequenceEqual "toSeq" (List.toSeq r) s }

             test "toArray" {
               let r = [|"1","uno"; "1","one"; "2","two"|]
               let a = NameValueCollection.ofSeq r
               let s = NameValueCollection.toArray a
               Expect.equal "toArray" r s }

             test "toList" {
               let r = ["1","uno"; "1","one"; "2","two"]
               let a = NameValueCollection.ofSeq r
               let s = NameValueCollection.toList a
               Expect.equal "toList" r s }

             test "concat" {
               let a = NameValueCollection()
               a.Add("1", "uno")
               a.Add("2", "dos")
               let b = NameValueCollection()
               b.Add("1", "one")
               b.Add("2", "two")
               let c = NameValueCollection.concat a b
               Expect.equal "concat" [("1", "uno"); ("1", "one"); ("2", "dos"); ("2", "two")] <| NameValueCollection.toList c 
               Expect.equal "ofSeq" [|"1"; "2"|] c.AllKeys }

             test "toLookup" {
               let n1 = NameValueCollection.ofSeq ["1","uno"; "1","one"; "2","two"]
               let l = NameValueCollection.toLookup n1
               "1" |> assertKeyIs l ["uno"; "one"]
               "2" |> assertKeyIs l ["two"]
               "3" |> assertKeyIs l []
               n1.Add("3", "three")
               "3" |> assertKeyIs l [] }

             test "asLookup" {
               let n1 = NameValueCollection.ofSeq ["1","uno"; "1","one"; "2","two"]
               let l = NameValueCollection.asLookup n1
               "1" |> assertKeyIs l ["uno"; "one"]
               "2" |> assertKeyIs l ["two"]
               "3" |> assertKeyIs l []
               n1.Add("3", "three")
               "3" |> assertKeyIs l ["three"] }

             test "asDictionary" {
               let n1 = NameValueCollection.ofSeq ["1","uno"; "1","one"; "2","two"]
               let d = NameValueCollection.asDictionary n1
               Expect.equal "asDictionary" [|"uno";"one"|] d.["1"]
               Expect.equal "asDictionary" [|"two"|] d.["2"]
               Expect.throwsT<KeyNotFoundException> "asDictionary" (fun () -> ignore d.["3"] |> ignore)
               n1.Add("3", "three")
               Expect.equal "asDictionary" [|"three"|] d.["3"] }
        ]

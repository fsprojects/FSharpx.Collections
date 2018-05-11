namespace FSharpx.Collections.Tests
// First version copied from the F# Power Pack 
// https://raw.github.com/fsharp/powerpack/master/src/FSharp.PowerPack.Unittests/Utilities.fs

open System
open System.Collections.Generic
open Expecto
open Expecto.Flip

//[<AutoOpen>]
module Utilities = ()
//    let test msg b = Expect.isTrue  "MiniTest '" + msg + "'" b
//    let logMessage msg = 
//        System.Console.WriteLine("LOG:" + msg)
////        System.Diagnostics.Trace.WriteLine("LOG:" + msg)
//    let check msg v1 v2 = test msg (v1 = v2)
//    let reportFailure msg = Assert.Fail msg

//    let throws f = try f() |> ignore; false with e -> true





//    // Verifies two sequences are equal (same length, equiv elements)
//    let verifySeqsEqual seq1 seq2 =
//        if Seq.length seq1 <> Seq.length seq2 then Assert.Fail()
        
//        let zippedElements = Seq.zip seq1 seq2
//        if zippedElements |> Seq.forall (fun (a, b) -> a = b) 
//        then ()
//        else Assert.Fail()
        
//    /// Check that the lamda throws an exception of the given type. Otherwise
//    /// calls Assert.Fail()
//    let private checkThrowsExn<'a when 'a :> exn> (f : unit -> unit) =
//        let funcThrowsAsExpected =
//            try
//                let _ = f ()
//                false // Did not throw!
//            with
//            | :? 'a
//                -> true   // Thew null ref, OK
//            | _ -> false  // Did now throw a null ref exception!
//        if funcThrowsAsExpected
//        then ()
//        else Assert.Fail()

//    // Illegitimate exceptions. Once we've scrubbed the library, we should add an
//    // attribute to flag these exception's usage as a bug.
//    let checkThrowsNullRefException      f = checkThrowsExn<NullReferenceException>   f
//    let checkThrowsIndexOutRangException f = checkThrowsExn<IndexOutOfRangeException> f

//    // Legit exceptions
//    let checkThrowsNotSupportedException f = checkThrowsExn<NotSupportedException>    f
//    let checkThrowsArgumentException     f = checkThrowsExn<ArgumentException>        f
//    let checkThrowsArgumentNullException f = checkThrowsExn<ArgumentNullException>    f
//    let checkThrowsKeyNotFoundException  f = checkThrowsExn<KeyNotFoundException>     f
//    let checkThrowsDivideByZeroException f = checkThrowsExn<DivideByZeroException>    f
//    let checkThrowsInvalidOperationExn   f = checkThrowsExn<InvalidOperationException> f

       
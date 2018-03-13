﻿module FsCheck.NUnit

open FSharpx
open FsCheck
open NUnit.Framework

let private nUnitRunner =
    { new IRunner with
        member x.OnStartFixture t = ()
        member x.OnArguments(ntest, args, every) = ()
        member x.OnShrink(args, everyShrink) = ()
        member x.OnFinished(name, result) = 
            match result with 
#if NETCOREAPP2_0
            | TestResult.True (data, _) -> 
#else
            | TestResult.True data -> 
#endif
                printfn "%s" (Runner.onFinishedToString name result)
            | _ -> Assert.Fail(Runner.onFinishedToString name result) }
   
let private nUnitConfig = { Config.Default with Runner = nUnitRunner }

let fsCheck name testable =
    FsCheck.Check.One (name, nUnitConfig, testable)

module Gen = 
    let ap x y = Gen.apply y x
namespace FSharpx.Collections.Experimental.Tests

open Expecto
open Expecto.Impl

module RunTests =

    [<EntryPoint>]
    let main args =
        Tests.runTestsInAssembly Tests.defaultConfig args

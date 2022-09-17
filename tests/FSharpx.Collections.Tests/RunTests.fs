namespace FSharpx.Collections.Tests

open Expecto
open Expecto.Impl

module RunTests =

    [<EntryPoint>]
    let main args =
        Tests.runTestsInAssembly Tests.defaultConfig args

namespace FSharpx.Collections.Tests

open Expecto
open Expecto.Impl

module RunTests =

    [<EntryPoint>]
    let main args = runTestsInAssembly ExpectoConfig.defaultConfig args

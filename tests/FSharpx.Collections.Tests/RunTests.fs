namespace FSharpx.Collections.Tests

open Expecto

module RunTests =

    [<EntryPoint>]
    let main args =

        Tests.runTestsWithArgs defaultConfig args ArrayTests.testArray |> ignore
        Tests.runTestsWithArgs defaultConfig args ResizeArrayTests.testResizeArray |> ignore
        //Tests.runTestsWithArgs defaultConfig args <test> |> ignore
        //Tests.runTestsWithArgs defaultConfig args <test> |> ignore

        0


module Program

open Fable.Mocha

let allTests =
    testList
        "All"
        [ QueueTests.tests
          PersistentVectorTests.tests
          NonEmptyListTests.tests
          LazyListTests.tests
          DequeTests.tests ]

[<EntryPoint>]
let main args =
    Mocha.runTests allTests

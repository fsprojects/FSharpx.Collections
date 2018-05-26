namespace FSharpx.Collections.Tests

open Expecto

module RunTests =

    [<EntryPoint>]
    let main args =

        Tests.runTestsWithArgs defaultConfig args ArrayTests.testArray |> ignore

        Tests.runTestsWithArgs defaultConfig args ByteStringTests.testByteString |> ignore

        Tests.runTestsWithArgs defaultConfig args ResizeArrayTests.testResizeArray |> ignore

        Tests.runTestsWithArgs defaultConfig args DequeTests.testDeque |> ignore
        Tests.runTestsWithArgs defaultConfig args DequeTests.propertyTestDeque |> ignore

        Tests.runTestsWithArgs defaultConfig args DictionaryExtensionsTests.testDictionaryExtensions |> ignore

        Tests.runTestsWithArgs defaultConfig args DListTests.testDList |> ignore
        Tests.runTestsWithArgs defaultConfig args DListTests.propertyTestDList |> ignore

        Tests.runTestsWithArgs defaultConfig args HeapTests.testHeap |> ignore
        Tests.runTestsWithArgs defaultConfig args HeapTests.propertyTestHeap |> ignore

        Tests.runTestsWithArgs defaultConfig args LazyList.testLazyList |> ignore

        Tests.runTestsWithArgs defaultConfig args ListExtensionsTests.testListExtensions |> ignore
        Tests.runTestsWithArgs defaultConfig args ListExtensionsTests.propertyTestListExtensions |> ignore

        Tests.runTestsWithArgs defaultConfig args MapExtensionsTests.testMapExtensions |> ignore

        Tests.runTestsWithArgs defaultConfig args MapTests.testMap |> ignore

        Tests.runTestsWithArgs defaultConfig args NameValueCollectionTests.testNameValueCollection |> ignore

        Tests.runTestsWithArgs defaultConfig args PriorityQueueTests.testPriorityQueue |> ignore

        Tests.runTestsWithArgs defaultConfig args QueueTests.testQueue |> ignore
        Tests.runTestsWithArgs defaultConfig args QueueTests.propertyTestQueue |> ignore

        Tests.runTestsWithArgs defaultConfig args RandomAccessListTest.testRandomAccessList |> ignore
        Tests.runTestsWithArgs defaultConfig args RandomAccessListTest.propertyTestRandomAccessList |> ignore

        Tests.runTestsWithArgs defaultConfig args SeqTests.testSeq |> ignore

        Tests.runTestsWithArgs defaultConfig args PersistentVectorTests.testPersistentVector |> ignore

        Tests.runTestsWithArgs defaultConfig args TransientHashMapTests.testTransientHashMap |> ignore

        Tests.runTestsWithArgs defaultConfig args PersistentHashMapTests.testPersistentHashMap |> ignore

        0


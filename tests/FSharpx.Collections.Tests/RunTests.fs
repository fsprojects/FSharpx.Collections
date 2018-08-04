namespace FSharpx.Collections.Tests

open Expecto
open Expecto.Impl

module RunTests =

    [<EntryPoint>]
    let main args =

        [
            Tests.runTestsWithArgs defaultConfig args ArrayTests.testArray

            Tests.runTestsWithArgs defaultConfig args ByteStringTests.testByteString

            Tests.runTestsWithArgs defaultConfig args CircularBufferTests.testCircularBuffer

            Tests.runTestsWithArgs defaultConfig args ResizeArrayTests.testResizeArray

            Tests.runTestsWithArgs defaultConfig args DequeTests.testDeque
            Tests.runTestsWithArgs defaultConfig args DequeTests.propertyTestDeque

            Tests.runTestsWithArgs defaultConfig args DictionaryExtensionsTests.testDictionaryExtensions

            Tests.runTestsWithArgs defaultConfig args DListTests.testDList
            Tests.runTestsWithArgs defaultConfig args DListTests.propertyTestDList

            Tests.runTestsWithArgs defaultConfig args HeapTests.testHeap
            Tests.runTestsWithArgs defaultConfig args HeapTests.propertyTestHeap

            Tests.runTestsWithArgs defaultConfig args LazyList.testLazyList

            Tests.runTestsWithArgs defaultConfig args ListExtensionsTests.testListExtensions
            Tests.runTestsWithArgs defaultConfig args ListExtensionsTests.propertyTestListExtensions

            Tests.runTestsWithArgs defaultConfig args MapExtensionsTests.testMapExtensions

            Tests.runTestsWithArgs defaultConfig args MapTests.testMap

            Tests.runTestsWithArgs defaultConfig args NameValueCollectionTests.testNameValueCollection

            Tests.runTestsWithArgs defaultConfig args NonEmptyListTests.testNonEmptyList

            Tests.runTestsWithArgs defaultConfig args PriorityQueueTests.testPriorityQueue

            Tests.runTestsWithArgs defaultConfig args QueueTests.testQueue
            Tests.runTestsWithArgs defaultConfig args QueueTests.propertyTestQueue

            Tests.runTestsWithArgs defaultConfig args RandomAccessListTest.testRandomAccessList
            Tests.runTestsWithArgs defaultConfig args RandomAccessListTest.propertyTestRandomAccessList

            Tests.runTestsWithArgs defaultConfig args SeqTests.testSeq

            Tests.runTestsWithArgs defaultConfig args PersistentVectorTests.testPersistentVector

            Tests.runTestsWithArgs defaultConfig args TransientHashMapTests.testTransientHashMap

            Tests.runTestsWithArgs defaultConfig args PersistentHashMapTests.testPersistentHashMap
        ]
        |> List.sum


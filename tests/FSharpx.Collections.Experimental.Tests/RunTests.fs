namespace FSharpx.Collections.Experimental.Tests

open Expecto
open Expecto.Impl

module RunTests =

    [<EntryPoint>]
    let main args =

        [
            Tests.runTestsWithArgs defaultConfig args AltBinaryRandomAccessListTest.testAltBinaryRandomAccessList

            Tests.runTestsWithArgs defaultConfig args BankersDequeTest.testBankersDeque

            Tests.runTestsWithArgs defaultConfig args BatchDequeTest.testBatchDeque

            Tests.runTestsWithArgs defaultConfig args BinaryRandomAccessListTest.testBinaryRandomAccessList

            Tests.runTestsWithArgs defaultConfig args BinaryRoseTreeTest.testBinaryRoseTree

            Tests.runTestsWithArgs defaultConfig args BinaryTreeZipperTest.testBinaryTreeZipper

            Tests.runTestsWithArgs defaultConfig args BinomialHeapTest.testBinomialHeap
            Tests.runTestsWithArgs defaultConfig args BinomialHeapTest.propertyBinomialHeap

            Tests.runTestsWithArgs defaultConfig args BKTreeTest.testBKTree

            Tests.runTestsWithArgs defaultConfig args BlockResizeArrayTest.testBlockResizeArray
            Tests.runTestsWithArgs defaultConfig args BlockResizeArrayTest.testBlockResizeArrayPropeerties

            Tests.runTestsWithArgs defaultConfig args BootstrappedQueueTest.testBootstrappedQueue

            Tests.runTestsWithArgs defaultConfig args BottomUpMergeSortTest.testBottomUpMergeSort

            Tests.runTestsWithArgs defaultConfig args DequeTest.testDeque

            Tests.runTestsWithArgs defaultConfig args DListTest.testDList

            Tests.runTestsWithArgs defaultConfig args EagerRoseTreeTest.testEagerRoseTree
            Tests.runTestsWithArgs defaultConfig args EagerRoseTreeTest.testEagerRoseTreePropeerties

            Tests.runTestsWithArgs defaultConfig args EditDistanceTest.testEditDistance

            Tests.runTestsWithArgs defaultConfig args FileSystemZipperTest.testFileSystemZipper

            Tests.runTestsWithArgs defaultConfig args FlatListTest.testFlatList
            Tests.runTestsWithArgs defaultConfig args FlatListTest.testFlatListProperties// |> ignore

            Tests.runTestsWithArgs defaultConfig args HeapPriorityQueueTest.testHeapPriorityQueue

            Tests.runTestsWithArgs defaultConfig args ImplicitQueueTest.testImplicitQueue

            Tests.runTestsWithArgs defaultConfig args IndexedRoseTreeTest.testIndexedRoseTree

            Tests.runTestsWithArgs defaultConfig args IntMapTest.testIntMap
            Tests.runTestsWithArgs defaultConfig args IntMapTest.testIntMapProperties

            Tests.runTestsWithArgs defaultConfig args IQueueTest.testIQueue
            Tests.runTestsWithArgs defaultConfig args IQueueTest.testIQueueProperties

            Tests.runTestsWithArgs defaultConfig args LeftistHeapTest.testLeftistHeap
            Tests.runTestsWithArgs defaultConfig args LeftistHeapTest.testLeftistHeapProperties

            Tests.runTestsWithArgs defaultConfig args ListZipperTest.testListZipper

            Tests.runTestsWithArgs defaultConfig args PairingHeapTest.testPairingHeap
            Tests.runTestsWithArgs defaultConfig args PairingHeapTest.testPairingHeapProperties

            Tests.runTestsWithArgs defaultConfig args RealTimeDequeTest.testRealTimeDeque

            Tests.runTestsWithArgs defaultConfig args RealTimeQueueTest.testRealTimeQueue

            Tests.runTestsWithArgs defaultConfig args RingBufferTest.testRingBuffer

            Tests.runTestsWithArgs defaultConfig args RoseTreeTest.testRoseTree
            Tests.runTestsWithArgs defaultConfig args RoseTreeTest.testRoseTreeProperties

            Tests.runTestsWithArgs defaultConfig args SkewBinaryRandomAccessListTest.testSkewBinaryRandomAccessList

            Tests.runTestsWithArgs defaultConfig args SkewBinomialHeapTest.testSkewBinomialHeap

            Tests.runTestsWithArgs defaultConfig args TimeSeriesTest.testTimeSeries
        ]
        |> List.sum


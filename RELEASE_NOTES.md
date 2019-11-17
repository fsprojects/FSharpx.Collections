#### 2.1.1 - 2019-11-17
* Fix FSharp.Core dependency version

#### 2.1.0 - 2019-11-12
* target net45 and netstandard2.0, thanks Grzegorz Dziadkiewicz

#### 2.0.0 - 2019-05-15
* BREAKING CHANGE: only netstandard2.0 supported
* PersistentHashMap implements Count (issues/12)
* add compareWith and areEqual to LazyList (issues/114) thanks teo-tsirpanis
* fix experimental RoseTree equality (issues/114) thanks teo-tsirpanis
* fix Nop thread comparison in PersistentHashMap (issues/66)
* make RandomAccessList serializable (issue 127) thanks teo-tsirpanis
* Implemented CHAMP algorithm for a persistent hash map, thanks bsomes 
* Implement IReadOnlyList and IReadOnlyCollection for the RandomAccessList (issue 130) thanks teo-tsirpanis
* LazyListbased on the BCL's Lazy type, thanks teo-tsirpanis

#### 2.0.0-beta3 - 2018-06-19
* pairwise DList, thanks Brendan Fahy

#### 2.0.0-beta2 - 2018-05-26
* target net45 and netstandard2.0 (beta1 incorrectly targetted net461)

#### 2.0.0-beta1 - 2018-05-26
* Microsoft.NET.Sdk projects
* target net45 and netstandard2.0
* BREAKING CHANGE: RequireQualifiedAccess
* BREAKING CHANGE: type RealTimeQueue under Experimental namespace
* BREAKING CHANGE: type BootstrappedQueue under Experimental namespace
* BREAKING CHANGE: type ListZipper under Experimental namespace
* BREAKING CHANGE: types BinaryTree, TreeDirection, BinaryTreeZipper under Experimental namespace
* BREAKING CHANGE: type ImplicitQueue under Experimental namespace
* BREAKING CHANGE: type BinaryRandomAccessList under Experimental namespace
* BREAKING CHANGE: type Digit for BinaryRandomAccessList renamed TreeBRALDigit

#### 1.17.0 - 26.06.2017
* PERFORMANCE: NonEmptyList Collect had poor performance - https://github.com/fsprojects/FSharpx.Collections/pull/75

#### 1.16.0 - 25.05.2017
* New tagged structures - https://github.com/fsprojects/FSharpx.Collections/pull/69
* Use FSharp.Core 4.0 
* Couple of new helper functions - https://github.com/fsprojects/FSharpx.Collections/pull/64 https://github.com/fsprojects/FSharpx.Collections/pull/63
* Improved RandomAccessList API with some functions from PersistentVector - https://github.com/fsprojects/FSharpx.Collections/pull/54
* Faster NonEmptyList implementation - https://github.com/fsprojects/FSharpx.Collections/pull/62

#### 1.14.0 - 13.02.2016 
* Allow 4.0 FSharp.Core 
* Distinct and DistinctBy functions for ResizeArray - https://github.com/fsprojects/FSharpx.Collections/pull/56
* Add zip to NonEmptyList - https://github.com/fsprojects/FSharpx.Collections/pull/46
 
#### 1.12.4 - 13.09.2015 
* Fixed enumerating empty RandomAccessList - https://github.com/fsprojects/FSharpx.Collections/pull/45

#### 1.12.3 - 12.09.2015 
* ResizeArray.collect made to take seq as an input - https://github.com/fsprojects/FSharpx.Collections/pull/41
* Made conflict resolution explicit in variable names - https://github.com/fsprojects/FSharpx.Collections/pull/39

#### 1.12.1 - 17.07.2015 
* LazyList.fold - https://github.com/fsprojects/FSharpx.Collections/pull/34

#### 1.12.0 - 17.07.2015 
* New SkewBinomialHeap - https://github.com/fsprojects/FSharpx.Collections/pull/36

#### 1.11.1 - 17.07.2015 
* New Block resize array functions - https://github.com/fsprojects/FSharpx.Collections/pull/37

#### 1.11.0 - 25.05.2015 
* New Block resize array - https://github.com/fsprojects/FSharpx.Collections/pull/33
* BUGFIX: Prevent ArrayNode's tryFind from returning Some null - https://github.com/fsprojects/FSharpx.Collections/pull/22

#### 1.10.0 - 26.02.2015 
* Added profile 259 and fixed profile 47 folder name - https://github.com/fsprojects/FSharpx.Collections/pull/26

#### 1.9.6 - 13.01.2015 
* Bump version due to broken logo link - https://github.com/fsprojects/FSharpx.Collections/issues/21

#### 1.9.5 - 13.01.2015 
* Bump version due to broken package meta data - https://github.com/fsprojects/FSharpx.Collections/issues/21

#### 1.9.4 - 07.08.2014 
* Add Profile47

#### 1.9.3 - 07.08.2014 
* Tail recursive implementation of Heap.Tail - https://github.com/fsprojects/FSharpx.Collections/pull/17

#### 1.9.2 - 14.01.2013 
* Fixing nuget package

#### 1.9.1 - 14.01.2013 
* Initial release from new location; previous contributor history lost

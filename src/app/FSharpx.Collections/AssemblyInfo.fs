module FSharpx.Collections.AssemblyInfo
#nowarn "49" // uppercase argument names
#nowarn "67" // this type test or downcast will always hold
#nowarn "66" // tis upast is unnecessary - the types are identical
#nowarn "58" // possible incorrect indentation..
#nowarn "57" // do not use create_DelegateEvent
#nowarn "51" // address-of operator can occur in the code
open System
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
exception ReturnException183c26a427ae489c8fd92ec21a0c9a59 of obj
exception ReturnNoneException183c26a427ae489c8fd92ec21a0c9a59

[<assembly: ComVisible (false)>]

[<assembly: CLSCompliant (false)>]

[<assembly: Guid ("32DA9CE0-5245-4100-B7B8-6346B753B179")>]

[<assembly: AssemblyTitle ("FSharpx")>]

[<assembly: AssemblyDescription ("FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library. Its main target is F# but it aims to be compatible with all .NET languages wherever possible.

It currently implements:

* Purely functional data structures: Queues, double-ended Queues, BottomUpMergeSort, RandomAccessList, Vector, RoseTree, BKTree
")>]

[<assembly: AssemblyProduct ("FSharpx")>]

[<assembly: AssemblyVersion ("0.0.1")>]

[<assembly: AssemblyFileVersion ("0.0.1")>]

[<assembly: AssemblyDelaySign (false)>]

()

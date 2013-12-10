namespace System
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<assembly: AssemblyTitleAttribute("FSharpx.Collections")>]
[<assembly: AssemblyDescriptionAttribute("FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library. Its main target is F# but it aims to be compatible with all .NET languages wherever possible.

It currently implements:

* Purely functional data structures: Queues, double-ended Queues, BottomUpMergeSort, RandomAccessList, Vector, RoseTree, BKTree
")>]
[<assembly: InternalsVisibleToAttribute("FSharpx.Collections.Tests")>]
[<assembly: GuidAttribute("32DA9CE0-5245-4100-B7B8-6346B753B179")>]
[<assembly: AssemblyProductAttribute("FSharpx.Collections")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyInformationalVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"

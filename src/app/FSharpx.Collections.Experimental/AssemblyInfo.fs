namespace System
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<assembly: AssemblyTitleAttribute("FSharpx.Collections.Experimental")>]
[<assembly: AssemblyDescriptionAttribute("FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library. Its main target is F# but it aims to be compatible with all .NET languages wherever possible.

This library provides experimental data structures.")>]
[<assembly: InternalsVisibleToAttribute("FSharpx.Collections.Experimental.Tests")>]
[<assembly: GuidAttribute("4C646C09-6925-47D0-B187-8A5C3D061329")>]
[<assembly: AssemblyProductAttribute("FSharpx.Collections")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyInformationalVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"

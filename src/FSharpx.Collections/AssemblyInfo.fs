namespace System
open System.Reflection
open System.Runtime.CompilerServices

[<assembly: AssemblyTitleAttribute("FSharpx.Collections")>]
[<assembly: AssemblyProductAttribute("FSharpx.Collections")>]
[<assembly: AssemblyDescriptionAttribute("FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library.")>]
[<assembly: AssemblyVersionAttribute("1.9.1")>]
[<assembly: InternalsVisibleToAttribute("FSharpx.Collections.Tests")>]
[<assembly: InternalsVisibleToAttribute("FSharpx.Collections.Experimental.Tests")>]
[<assembly: AssemblyFileVersionAttribute("1.9.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.9.1"

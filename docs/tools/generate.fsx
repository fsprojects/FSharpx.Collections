// --------------------------------------------------------------------------------------
// Builds the documentation from `.fsx` and `.md` files in the 'docs/content' directory
// (the generated documentation is stored in the 'docs/output' directory)
// --------------------------------------------------------------------------------------
let referenceBinaries = []
// Web site location for the generated documentation
#if TESTING
let website = __SOURCE_DIRECTORY__ + "../output"
#else
let website = "/FSharpx.Collections"
#endif

let githubLink = "http://github.com/fsprojects/FSharpx.Collections"

// Specify more information about your project
let info =
  [ "project-name", "fsprojects/FSharpx.Collections"
    "project-author", "Steffen Forkmann"
    "project-summary", "FSharpx.Collections is a collection of datastructures for use with F# and C#."
    "project-github", githubLink
    "project-nuget", "https://www.nuget.org/packages/FSharpx.Collections" ]

// --------------------------------------------------------------------------------------
// For typical project, no changes are needed below
// --------------------------------------------------------------------------------------

#I "../../packages/FAKE/tools/"
#load "../../packages/FSharp.Formatting/FSharp.Formatting.fsx"
#r "NuGet.Core.dll"
#r "FakeLib.dll"
open Fake
open System.IO
open Fake.FileHelper
open FSharp.Literate
open FSharp.MetadataFormat
open FSharp.Formatting.Razor

// When called from 'build.fsx', use the public project URL as <root>
// otherwise, use the current 'output' directory.
#if RELEASE
let root = website
#else
let root = "file://" + (__SOURCE_DIRECTORY__ @@ "../output")
#endif

// Paths with template/source/output locations
let bin        = __SOURCE_DIRECTORY__ @@ "../../bin"
let content    = __SOURCE_DIRECTORY__ @@ "../content"
let output     = __SOURCE_DIRECTORY__ @@ "../output"
let files      = __SOURCE_DIRECTORY__ @@ "../files"
let templates  = __SOURCE_DIRECTORY__ @@ "templates"
let formatting = __SOURCE_DIRECTORY__ @@ "../../packages/FSharp.Formatting/"
let docTemplate = formatting @@ "templates/docpage.cshtml"

// Where to look for *.csproj templates (in this order)
let layoutRootsAll = new System.Collections.Generic.Dictionary<string, string list>()
layoutRootsAll.Add("en",[ templates; formatting @@ "templates"
                          formatting @@ "templates/reference" ])
subDirectories (directoryInfo templates)
|> Seq.iter (fun d ->
                let name = d.Name
                if name.Length = 2 || name.Length = 3 then
                    layoutRootsAll.Add(
                            name, [templates @@ name
                                   formatting @@ "templates"
                                   formatting @@ "templates/reference" ]))

let fsiEvaluator = lazy (Some (FsiEvaluator() :> IFsiEvaluator))

// Copy static files and CSS + JS from F# Formatting
let copyFiles () =
  CopyRecursive files output true |> Log "Copying file: "
  ensureDirectory (output @@ "content")
  CopyRecursive (formatting @@ "styles") (output @@ "content") true 
    |> Log "Copying styles and scripts: "

let references =
  if isMono then
    // Workaround compiler errors in Razor-ViewEngine
    let d = RazorEngine.Compilation.ReferenceResolver.UseCurrentAssembliesReferenceResolver()
    let loadedList = d.GetReferences () |> Seq.map (fun r -> r.GetFile()) |> Seq.cache
    // We replace the list and add required items manually as mcs doesn't like duplicates...
    let getItem name = loadedList |> Seq.find (fun l -> l.Contains name)
    [ (getItem "FSharp.Core").Replace("4.3.0.0", "4.3.1.0")
      Path.GetFullPath "./../../packages/FSharp.Compiler.Service/lib/net40/FSharp.Compiler.Service.dll"
      Path.GetFullPath "./../../packages/FSharp.Formatting/lib/net40/System.Web.Razor.dll"
      Path.GetFullPath "./../../packages/FSharp.Formatting/lib/net40/RazorEngine.dll"
      Path.GetFullPath "./../../packages/FSharp.Formatting/lib/net40/FSharp.Literate.dll"
      Path.GetFullPath "./../../packages/FSharp.Formatting/lib/net40/FSharp.CodeFormat.dll"
      Path.GetFullPath "./../../packages/FSharp.Formatting/lib/net40/FSharp.MetadataFormat.dll" ]
    |> Some
  else None

let binaries =
    let manuallyAdded = 
        referenceBinaries 
        |> List.map (fun b -> bin @@ b)
   
    let conventionBased = 
        directoryInfo bin 
        |> subDirectories
        |> Array.map (fun d -> d.Name, (subDirectories d |> Array.filter(fun x -> x.FullName.ToLower().Contains("net45")) ).[0] )
        |> Array.map (fun (name, d) -> 
            d.GetFiles()
            |> Array.filter (fun x -> 
                x.Name.ToLower() = (sprintf "%s.dll" name).ToLower())
            |> Array.map (fun x -> x.FullName) 
            )
        |> Array.concat
        |> List.ofArray

    conventionBased @ manuallyAdded

let libDirs =
    let conventionBasedbinDirs =
        directoryInfo bin 
        |> subDirectories
        |> Array.map (fun d -> d.FullName)
        |> List.ofArray

    conventionBasedbinDirs @ [bin]

// Build API reference from XML comments
let buildReference () =
  CleanDir (output @@ "reference")
  RazorMetadataFormat.Generate
    ( binaries, output @@ "reference", layoutRootsAll.["en"],
      parameters = ("root", root)::info,
      sourceRepo = githubLink @@ "tree/master",
      sourceFolder = __SOURCE_DIRECTORY__ @@ ".." @@ "..",
      publicOnly = true,libDirs = libDirs )

// Build documentation from `fsx` and `md` files in `docs/content`
let buildDocumentation () =
  let subdirs =
    [ content, docTemplate; ]
  for dir, template in subdirs do
    let sub = "." // Everything goes into the same output directory here
    let langSpecificPath(lang, path:string) =
        path.Split([|'/'; '\\'|], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.exists(fun i -> i = lang)
    let layoutRoots =
        let key = layoutRootsAll.Keys |> Seq.tryFind (fun i -> langSpecificPath(i, dir))
        match key with
        | Some lang -> layoutRootsAll.[lang]
        | None -> layoutRootsAll.["en"] // "en" is the default language
    RazorLiterate.ProcessDirectory
      ( dir, template, output @@ sub, replacements = ("root", root)::info,
        layoutRoots = layoutRoots,
        generateAnchors = true,
        processRecursive = false,
        includeSource = true, // Only needed for 'side-by-side' pages, but does not hurt others
        ?fsiEvaluator = fsiEvaluator.Value ) // Currently we don't need it but it's a good stress test to have it here.

// Generate
copyFiles()
#if HELP
buildDocumentation()
#endif
#if REFERENCE
buildReference()
#endif
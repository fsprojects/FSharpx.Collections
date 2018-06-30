// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r "paket: groupref FakeBuild //"

#load "./.fake/build.fsx/intellisense.fsx"

open System.IO
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.DotNet.Testing
open Fake.Tools

// --------------------------------------------------------------------------------------
// START TODO: Provide project-specific details below
// --------------------------------------------------------------------------------------

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package
//  - to run tests and to publish documentation on GitHub gh-pages
//  - for documentation, you also need to edit info in "docs/tools/generate.fsx"

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "FSharpx.Collections"

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "FSharpx.Collections is a collection of datastructures for use with F# and C#."

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = "FSharpx.Collections is a collection of datastructures for use with F# and C#."

// List of author names (for NuGet package)
let authors = "Steffen Forkmann, Daniel Mohl, Tomas Petricek, Ryan Riley, Mauricio Scheffer, Phil Trelford, JackFox"

// Tags for your project (for NuGet package)
let tags = "F# fsharp fsharpx collections datastructures"

// File system information 
let solutionFile  = "FSharpx.Collections.sln"

// Target configuration
let configuration = "Release"

// Pattern specifying assemblies to be tested using NUnit
let testAssemblies = "tests/**/bin/Release/net47/*Tests.exe"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "fsprojects" 
let gitHome = "https://github.com/" + gitOwner

// The name of the project on GitHub
let gitName = "FSharpx.Collections"

let website = "/FSharpx.Collections"

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps
// --------------------------------------------------------------------------------------

// Read additional information from the release notes document
let release = ReleaseNotes.load "RELEASE_NOTES.md"

// Helper active pattern for project types
let (|Fsproj|Csproj|Vbproj|) (projFileName:string) = 
    match projFileName with
    | f when f.EndsWith("fsproj") -> Fsproj
    | f when f.EndsWith("csproj") -> Csproj
    | f when f.EndsWith("vbproj") -> Vbproj
    | _                           -> failwith (sprintf "Project file %s not supported. Unknown project type." projFileName)

// Generate assembly info files with the right version & up-to-date information
Target.create "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName =
        [ AssemblyInfo.Title (projectName)
          AssemblyInfo.Product project
          AssemblyInfo.Description summary
          AssemblyInfo.InternalsVisibleTo "FSharpx.Collections.Tests"
          AssemblyInfo.InternalsVisibleTo "FSharpx.Collections.Experimental.Tests"
          AssemblyInfo.Version release.AssemblyVersion
          AssemblyInfo.FileVersion release.AssemblyVersion
          AssemblyInfo.Configuration configuration ]

    let getProjectDetails projectPath =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        ( projectPath, 
          projectName,
          System.IO.Path.GetDirectoryName(projectPath),
          (getAssemblyInfoAttributes projectName)
        )

    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (projFileName, _, folderName, attributes) ->
        match projFileName with
        | Fsproj -> AssemblyInfoFile.createFSharp (folderName </> "AssemblyInfo.fs") attributes
        | Csproj -> AssemblyInfoFile.createCSharp ((folderName </> "Properties") </> "AssemblyInfo.cs") attributes
        | Vbproj -> AssemblyInfoFile.createVisualBasic ((folderName </> "My Project") </> "AssemblyInfo.vb") attributes
        )
)

// Copies binaries from default VS location to exepcted bin folder
// But keeps a subdirectory structure for each project in the 
// src folder to support multiple project outputs
Target.create "CopyBinaries" (fun _ ->
    !! "src/**/*.??proj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "bin/Release", "bin" @@ (System.IO.Path.GetFileNameWithoutExtension f)))
    |>  Seq.iter (fun (fromDir, toDir) -> Shell.copyDir toDir fromDir (fun _ -> true))
)

// --------------------------------------------------------------------------------------
// Clean build results

let buildConfiguration = DotNet.Custom <| Environment.environVarOrDefault "configuration" configuration

Target.create "Clean" (fun _ ->
    Shell.cleanDirs ["bin"; "temp"]
)

Target.create "CleanDocs" (fun _ ->
    Shell.cleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Target.create "Build" (fun _ ->
    solutionFile 
    |> DotNet.build (fun p -> 
        { p with
            Configuration = buildConfiguration })
)

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner

Target.create "RunTests" (fun _ ->
    !! testAssemblies
    |> Expecto.run id
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target.create "Pack" (fun _ ->
    let releaseNotes = release.Notes |> String.toLines

    // environment vars for Travis
    Environment.setEnvironVar "PackageVersion" release.NugetVersion
    Environment.setEnvironVar "Version" release.NugetVersion
    Environment.setEnvironVar "PackageReleaseNotes" releaseNotes

    Paket.pack(fun p -> 
        { p with
            OutputPath = "bin"
            Version = release.NugetVersion
            ReleaseNotes = releaseNotes})
)

Target.create "PublishNuget" (fun _ ->
    Paket.push(fun p -> 
        { p with
            WorkingDir = "bin" })
)

// --------------------------------------------------------------------------------------
// Generate the documentation

// Paths with template/source/output locations
let bin        = __SOURCE_DIRECTORY__ @@ "bin"
let content    = __SOURCE_DIRECTORY__ @@ "docs/content"
let output     = __SOURCE_DIRECTORY__ @@ "docs"
let files      = __SOURCE_DIRECTORY__ @@ "docs/files"
let templates  = __SOURCE_DIRECTORY__ @@ "docs/tools/templates"
let formatting = __SOURCE_DIRECTORY__ @@ "packages/formatting/FSharp.Formatting/"
let docTemplate = "docpage.cshtml"

let copyFiles () =
    Shell.copyRecursive files output true 
    |> Trace.logItems "Copying file: "
    Directory.ensure (output @@ "content")
    Shell.copyRecursive (formatting @@ "styles") (output @@ "content") true 
    |> Trace.logItems "Copying styles and scripts: "

let copyBaseDocs () =
    File.delete "docs/content/release-notes.md"
    Shell.copyFile "docs/content/" "RELEASE_NOTES.md"
    Shell.rename "docs/content/release-notes.md" "docs/content/RELEASE_NOTES.md"

    File.delete "docs/content/license.md"
    Shell.copyFile "docs/content/" "LICENSE.txt"
    Shell.rename "docs/content/license.md" "docs/content/LICENSE.txt"

let root = website
let github_release_user = Environment.environVarOrDefault "github_release_user" gitOwner
let githubLink = sprintf "https://github.com/%s/%s" github_release_user gitName
let referenceBinaries = []

let layoutRootsAll = new System.Collections.Generic.Dictionary<string, string list>()
layoutRootsAll.Add("en",[   templates; 
                            formatting @@ "templates"
                            formatting @@ "templates/reference" ])

let info =
  [ "project-name", project
    "project-author", authors
    "project-summary", summary
    "project-github", githubLink
    "project-nuget", "http://nuget.org/packages/FSharpx.Collections" ]

Target.create "GenerateReferenceDocs" (fun _ ->
    Directory.ensure (output @@ "reference")

    let binaries () =
        let manuallyAdded = 
            referenceBinaries 
            |> List.map (fun b -> bin @@ b)
   
        let conventionBased = 
            DirectoryInfo.getSubDirectories <| DirectoryInfo bin
            |> Array.map (fun d -> 
                let net45Bin = 
                    DirectoryInfo.getSubDirectories d |> Array.filter(fun x -> x.FullName.ToLower().Contains("net45"))
                d.Name, net45Bin.[0] ) 
            |> Array.map (fun (name, d) -> 
                d.GetFiles()
                |> Array.filter (fun x -> 
                    x.Name.ToLower() = (sprintf "%s.dll" name).ToLower())
                |> Array.map (fun x -> x.FullName) 
                )
            |> Array.concat
            |> List.ofArray

        conventionBased @ manuallyAdded

    binaries()
    |> FSFormatting.createDocsForDlls (fun args ->
        { args with
            OutputDirectory = output @@ "reference"
            LayoutRoots =  layoutRootsAll.["en"]
            ProjectParameters =  ("root", root)::info
            SourceRepository = githubLink @@ "tree/master" }
           )
)

Target.create "GenerateHelp" (fun _ ->
    copyBaseDocs ()

    DirectoryInfo.getSubDirectories (DirectoryInfo.ofPath templates)
    |> Seq.iter (fun d ->
                    let name = d.Name
                    if name.Length = 2 || name.Length = 3 then
                        layoutRootsAll.Add(
                                name, [templates @@ name
                                       formatting @@ "templates"
                                       formatting @@ "templates/reference" ]))
    copyFiles ()
    
    for dir in  [ content; ] do
        let langSpecificPath(lang, path:string) =
            path.Split([|'/'; '\\'|], System.StringSplitOptions.RemoveEmptyEntries)
            |> Array.exists(fun i -> i = lang)
        let layoutRoots =
            let key = layoutRootsAll.Keys |> Seq.tryFind (fun i -> langSpecificPath(i, dir))
            match key with
            | Some lang -> layoutRootsAll.[lang]
            | None -> layoutRootsAll.["en"] // "en" is the default language

        FSFormatting.createDocs (fun args ->
            { args with
                Source = content
                OutputDirectory = output 
                LayoutRoots = layoutRoots
                ProjectParameters  = ("root", root)::info
                Template = docTemplate } )
)

Target.create "GenerateDocs" ignore

let createIndexFsx lang =
    let content = """(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../../bin"

(**
FSharpx.Collections ({0})
=========================
*)
"""
    let targetDir = "docs/content" @@ lang
    let targetFile = targetDir @@ "index.fsx"
    Directory.ensure targetDir
    System.IO.File.WriteAllText(targetFile, System.String.Format(content, lang))

Target.create "AddLangDocs" (fun _ ->
    let args = System.Environment.GetCommandLineArgs()
    if args.Length < 4 then
        failwith "Language not specified."

    args.[3..]
    |> Seq.iter (fun lang ->
        if lang.Length <> 2 && lang.Length <> 3 then
            failwithf "Language must be 2 or 3 characters (ex. 'de', 'fr', 'ja', 'gsw', etc.): %s" lang

        let templateFileName = "template.cshtml"
        let templateDir = "docs/tools/templates"
        let langTemplateDir = templateDir </> lang
        let langTemplateFileName = langTemplateDir </> templateFileName

        if System.IO.File.Exists(langTemplateFileName) then
            failwithf "Documents for specified language '%s' have already been added." lang

        Directory.ensure langTemplateDir
        Shell.copy langTemplateDir [ templateDir </> templateFileName ]

        createIndexFsx lang)
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target.create "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    Shell.cleanDir tempDocsDir
    Git.Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    Shell.copyRecursive "docs/output" tempDocsDir true |> Trace.tracefn "%A"
    Git.Staging.stageAll tempDocsDir
    Git.Commit.exec tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Git.Branches.push tempDocsDir
)

Target.create "Release" (fun _ ->
    Git.Staging.stageAll ""
    Git.Commit.exec "" (sprintf "Bump version to %s" release.NugetVersion)
    Git.Branches.push ""

    Git.Branches.tag "" release.NugetVersion
    Git.Branches.pushTag "" "origin" release.NugetVersion
)

Target.create "BuildPackage" ignore

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

let isLocalBuild = (BuildServer.buildServer = LocalBuild)

Target.create "All" ignore

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "CopyBinaries"
  ==> "RunTests"
  =?> ("GenerateReferenceDocs", isLocalBuild)
  =?> ("GenerateDocs", isLocalBuild)
  ==> "All"
  =?> ("ReleaseDocs", isLocalBuild)

"All" 
  ==> "Pack"
  ==> "BuildPackage"

"CleanDocs"
  ==> "GenerateHelp"
  ==> "GenerateReferenceDocs"
  ==> "GenerateDocs"
   
"ReleaseDocs"
  ==> "Release"

"BuildPackage"
  ==> "PublishNuget"
  ==> "Release"

Target.runOrDefault "All"
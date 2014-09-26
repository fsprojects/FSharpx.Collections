// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#I @"packages/FAKE/tools"
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open System

let projects = [|"FSharpx.Collections"; "FSharpx.Collections.Experimental"|]

let summary = "FSharpx.Collections is a collection of datastructures for use with F# and C#."
let description = "FSharpx.Collections is a collection of datastructures for use with F# and C#."
let authors = ["Steffen Forkmann"; "Daniel Mohl"; "Tomas Petricek"; "Ryan Riley"; "Mauricio Scheffer"; "Phil Trelford"]
let tags = "F# fsharp fsharpx collections datastructures"

let solutionFile  = "FSharpx.Collections"

let testAssemblies = "tests/**/bin/Release/*.Tests*.dll"
let cloneUrl = "git@github.com:fsprojects/FSharpx.Collections.git"
let nugetDir = "./nuget/"
let profile47dir = "./bin/portable-net4+sl4+wp71+win8/"

// Read additional information from the release notes document
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let release = parseReleaseNotes (IO.File.ReadAllLines "RELEASE_NOTES.md")

// Generate assembly info files with the right version & up-to-date information
Target "AssemblyInfo" (fun _ ->
  for project in projects do
    let fileName = "src/" + project + "/AssemblyInfo.fs"
    CreateFSharpAssemblyInfo fileName
        [ Attribute.Title project
          Attribute.Product project
          Attribute.Description summary
          Attribute.Version release.AssemblyVersion
          Attribute.InternalsVisibleTo "FSharpx.Collections.Tests"
          Attribute.InternalsVisibleTo "FSharpx.Collections.Experimental.Tests"
          Attribute.FileVersion release.AssemblyVersion ] 
)

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs ["bin"; "temp"; nugetDir]
)

Target "CleanDocs" (fun _ ->
    CleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Build" (fun _ ->
    !! (solutionFile + ".sln")
    |> MSBuild "" "Rebuild" (["Configuration","Release"])
    |> ignore
)

Target "BuildProfile47" (fun _ ->
    !! "src/**/*.fsproj"
    |> MSBuild profile47dir "Rebuild" (["Configuration","Release";
                                        "TargetFrameworkProfile", "Profile47"
                                        "TargetFrameworkVersion", "v4.0"
                                        "TargetFSharpCoreVersion", "2.3.5.0"
                                        "DefineConstants", "FX_PORTABLE" ])
    |> ignore
)

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner & kill test runner when complete

Target "RunTests" (fun _ ->
    ActivateFinalTarget "CloseTestRunner"

    !! testAssemblies
    |> NUnit (fun p ->
        { p with
            DisableShadowCopy = true
            TimeOut = TimeSpan.FromMinutes 20.
            OutputFile = "TestResults.xml" })
)

FinalTarget "CloseTestRunner" (fun _ ->  
    ProcessHelper.killProcess "nunit-agent.exe"
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    projects
    |> Seq.iter (fun project -> 
          let nugetDocsDir = nugetDir @@ "docs"
          let nugetlibDir = nugetDir @@ "lib"

          CleanDir nugetDocsDir
          CleanDir nugetlibDir
              
          CopyDir nugetlibDir "bin" (fun file -> file.EndsWith (project + ".dll") || file.EndsWith (project + ".xml", StringComparison.InvariantCultureIgnoreCase))  
          CopyDir nugetDocsDir "./docs/output" allFiles
          
          NuGet (fun p -> 
              { p with   
                  Authors = authors
                  Project = project
                  Summary = summary
                  Description = description
                  Version = release.NugetVersion
                  ReleaseNotes = release.Notes |> toLines
                  Tags = tags
                  OutputPath = nugetDir
                  AccessKey = getBuildParamOrDefault "nugetkey" ""
                  Publish = hasBuildParam "nugetkey"
                  Dependencies = [] })
              ("FSharpx.Collections.nuspec"))
)

// --------------------------------------------------------------------------------------
// Generate the documentation

Target "GenerateDocs" (fun _ ->
    executeFSIWithArgs "docs/tools" "generate.fsx" ["--define:RELEASE"] [] |> ignore
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    if not (System.IO.Directory.Exists tempDocsDir) then 
       Repository.cloneSingleBranch "" cloneUrl "gh-pages" tempDocsDir

    fullclean tempDocsDir
    CopyRecursive "docs/output" tempDocsDir true |> tracefn "%A"
    StageAll tempDocsDir
    Commit tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Branches.push tempDocsDir
)

Target "Release" DoNothing

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "BuildProfile47"
  ==> "Build"
  ==> "RunTests"
  ==> "All"

"All" 
  ==> "CleanDocs"
  ==> "GenerateDocs"
  ==> "NuGet"
  ==> "ReleaseDocs"
  ==> "Release"

RunTargetOrDefault "All"

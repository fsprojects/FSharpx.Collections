// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open System

let projects = [|"FSharpx.Collections"; "FSharpx.Collections.Experimental"|]

let summary = "FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library."
let description = "FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library."
let authors = ["Steffen Forkmann"; "Daniel Mohl"; "Tomas Petricek"; "Ryan Riley"; "Mauricio Scheffer"; "Phil Trelford"]
let tags = "F# fsharp fsharpx collections datastructures"

let solutionFile  = "FSharpx.Collections"

let testAssemblies = "tests/**/bin/Release/*.Tests*.dll"
let gitHome = "https://github.com/forki/FSharpx.Collections"
let gitName = "FSharpx.Collections"
let cloneUrl = "git@github.com:forki/FSharpx.Collections.git"
let nugetDir = "./nuget/"

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
// Clean build results & restore NuGet packages

Target "RestorePackages" RestorePackages

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
    |> MSBuildRelease "" "Rebuild"
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
          let nugetlibDir = nugetDir @@ "lib/net35"

          CleanDir nugetDocsDir
          CleanDir nugetlibDir
              
          CopyDir nugetlibDir "bin" (fun file -> file.EndsWith (project + ".dll") || file.EndsWith (project + ".xml"))  
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
    CleanDir tempDocsDir
    Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

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
  ==> "RestorePackages"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "RunTests"
  ==> "All"

"All" 
  ==> "CleanDocs"
  ==> "GenerateDocs"
  ==> "ReleaseDocs"
  ==> "NuGet"
  ==> "Release"

RunTargetOrDefault "All"

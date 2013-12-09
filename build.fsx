#r "./tools/FAKE/tools/FakeLib.dll"

open Fake 
open Fake.Git
open System.IO

RestorePackages()

// properties
let currentDate = System.DateTime.UtcNow
let projectName = "FSharpx"

let coreSummary = "FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library."
let projectSummary = "FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library."
let authors = ["Steffen Forkmann"; "Daniel Mohl"; "Tomas Petricek"; "Ryan Riley"; "Mauricio Scheffer"; "Phil Trelford" ]
let mail = "ryan.riley@panesofglass.org"
let homepage = "http://github.com/fsharp/fsharpx"

let buildVersion = if isLocalBuild then "0.0.1" else buildVersion

// directories
let buildDir = "./build/"
let packagesDir = "./packages/"
let testDir = "./test/"
let deployDir = "./deploy/"
let docsDir = "./docs/"
let nugetMainDir = "./nuget/"

let targetPlatformDir = getTargetPlatformDir "v4.0.30319"

let nugetDir package = sprintf "./nuget/%s/" package
let nugetLibDir package = nugetDir package @@ "lib"
let nugetDocsDir package = nugetDir package @@ "docs"

let packages = ["Collections"; "Collections.Experimental"]

let projectDesc = "FSharpx is a library for the .NET platform implementing general functional constructs on top of the F# core library. Its main target is F# but it aims to be compatible with all .NET languages wherever possible."

let rec getPackageDesc = function
| "Collections.Experimental" -> projectDesc + "\r\n\r\nThis library provides experimental data structures."
| _ -> projectDesc + "\r\n\r\nIt currently implements:\r\n\r\n" + 
                       "* Purely functional data structures: Queues, double-ended Queues, BottomUpMergeSort, RandomAccessList, Vector, RoseTree, BKTree\r\n" 

// params
let target = getBuildParamOrDefault "target" "All"

let normalizeFrameworkVersion frameworkVersion =
    let v = ("[^\\d]" >=> "") frameworkVersion
    v.Substring(0,2)

// files
let appReferences = !! "./src/app/**/*.*proj"

let testReferences  = !! "./src/test/**/*.*proj"

// targets
Target "Clean" (fun _ ->       
    CleanDirs [buildDir; testDir; deployDir; docsDir; nugetMainDir]

    packages
    |> Seq.iter (fun x -> CleanDirs [nugetDir x; nugetLibDir x; nugetDocsDir x])
)


Target "AssemblyInfo" (fun _ ->
    AssemblyInfo (fun p ->
        {p with 
            CodeLanguage = FSharp
            AssemblyVersion = buildVersion
            AssemblyTitle = projectName
            AssemblyDescription = getPackageDesc "Collections"
            Guid = "32DA9CE0-5245-4100-B7B8-6346B753B179"
            OutputFileName = "./src/app/FSharpx.Collections/AssemblyInfo.fs" })

    AssemblyInfo (fun p ->
        {p with 
            CodeLanguage = FSharp
            AssemblyVersion = buildVersion
            AssemblyTitle = projectName
            AssemblyDescription = getPackageDesc "Collections.Experimental"
            Guid = "4C646C09-6925-47D0-B187-8A5C3D061329"
            OutputFileName = "./src/app/FSharpx.Collections.Experimental/AssemblyInfo.fs" })
)

let buildAppTarget = TargetTemplate (fun frameworkVersion ->
    CleanDir buildDir

    appReferences
    |> MSBuild buildDir "Rebuild" (["Configuration","Release"])
    |> Log "AppBuild-Output: "
)

let buildTestTarget = TargetTemplate (fun frameworkVersion ->
    CleanDir testDir
    testReferences
    |> MSBuild testDir "Build" ["Configuration","Debug"] 
    |> Log "TestBuild-Output: "
)

let testTarget = TargetTemplate (fun frameworkVersion ->
    !! (testDir + "/*.Tests.dll")
    |> NUnit (fun p ->
        {p with
            DisableShadowCopy = true
            OutputFile = testDir + sprintf "TestResults.%s.xml" frameworkVersion })
)

let prepareNugetTarget = TargetTemplate (fun frameworkVersion ->
    packages
    |> Seq.iter (fun package ->
        let frameworkSubDir = nugetLibDir package @@ normalizeFrameworkVersion frameworkVersion
        CleanDir frameworkSubDir


        [for ending in ["dll";"pdb";"xml"] do
            yield sprintf "%sFSharpx.%s.%s" buildDir package ending
            yield sprintf "%sFSharpx.%s.DesignTime.%s" buildDir package ending]
        |> Seq.filter (fun f -> File.Exists f)
        |> CopyTo frameworkSubDir)
)

let buildFrameworkVersionTarget = TargetTemplate (fun frameworkVersion -> ())

let generateTargets() =
    let frameworkVersion = "net40"
    tracefn "Generating targets for .NET %s" frameworkVersion
    let v = normalizeFrameworkVersion frameworkVersion
    let buildApp = sprintf "BuildApp_%s" v
    let buildTest = sprintf "BuildTest_%s" v
    let test = sprintf "Test_%s" v
    let prepareNuget = sprintf "PrepareNuget_%s" v
    let buildFrameworkVersion = sprintf "Build_%s" v

    buildAppTarget buildApp frameworkVersion
    buildTestTarget buildTest frameworkVersion
    testTarget test frameworkVersion
    prepareNugetTarget prepareNuget frameworkVersion            
    buildFrameworkVersionTarget buildFrameworkVersion frameworkVersion

    "AssemblyInfo" ==> buildApp ==> buildTest ==> test ==> prepareNuget ==> buildFrameworkVersion

let nugetTarget = TargetTemplate (fun package ->
    [ "LICENSE.md" ] |> CopyTo (nugetDir package)
    NuGet (fun p -> 
        {p with               
            Authors = authors
            Project = projectName + "." + package
            Description = getPackageDesc package
            Version = buildVersion
            OutputPath = nugetDir package
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Dependencies =
                if package = "Collections" then p.Dependencies else
                  [projectName + ".Collections", RequireExactly (NormalizeVersion buildVersion)]
            Publish = hasBuildParam "nugetkey" })
        "FSharpx.Collections.nuspec"

    !! (nugetDir package + sprintf "FSharpx.%s.*.nupkg" package)
      |> CopyTo deployDir
)

Target "TestAll" DoNothing

let generateNugetTargets() =
    packages 
    |> Seq.fold
        (fun dependency package -> 
            tracefn "Generating nuget target for package %s" package
            let buildPackage = sprintf "Nuget_%s" package
            
            nugetTarget buildPackage package

            dependency ==> buildPackage)
            "TestAll"

Target "DeployZip" (fun _ ->
    !! (buildDir + "/**/*.*")
    |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName buildVersion)
)

Target "Deploy" DoNothing

// Build order
"Clean"
  ==> "AssemblyInfo"
  ==> (generateTargets())
  ==> "TestAll"
  ==> (generateNugetTargets())
  ==> "DeployZip"
  ==> "Deploy"

// Start build
RunTargetOrDefault "Deploy"

#r "./tools/FAKE/tools/FakeLib.dll"

#I "tools/FSharp.Formatting/lib/net40"
#I "tools/Microsoft.AspNet.Razor/lib/net40"
#I "tools/RazorEngine/lib/net40"
#r "System.Web.dll"
#r "FSharp.Markdown.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.Literate.dll"
#r "FSharp.MetadataFormat.dll"
#r "System.Web.Razor.dll"
#r "RazorEngine.dll"

open Fake
open FSharp.Literate
open Fake.Git
open FSharp.MetadataFormat

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
let apidocsDir = "./docs/apidocs/"

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

let frameworkVersion = "net40"

// files
let appReferences = !! "./src/app/**/*.*proj"
let testReferences  = !! "./src/test/**/*.*proj"

// targets
Target "Clean" (fun _ ->       
    CleanDirs [buildDir; testDir; deployDir; docsDir; apidocsDir]

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

Target "BuildApp" (fun _ ->
    CleanDir buildDir

    appReferences
    |> MSBuild buildDir "Rebuild" (["Configuration","Release"])
    |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    CleanDir testDir
    testReferences
    |> MSBuild testDir "Build" ["Configuration","Debug"] 
    |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
    !! (testDir + "/*.Tests.dll")
    |> NUnit (fun p ->
        {p with
            DisableShadowCopy = true
            OutputFile = testDir + sprintf "TestResults.xml" })
)

Target "GenerateDocs" (fun _ ->
    let source = "./help"
    let template = "./help/templates/template-project.html"
    let projInfo =
      [ "page-description", "FSharpx.Collections"
        "page-author", (separated ", " authors)
        "project-author", (separated ", " authors)
        "github-link", "http://github.com/forki/fsharpx.collections"
        "project-github", "http://github.com/forki/fsharpx.collections"
        "project-nuget", "https://www.nuget.org/packages/FSharpx.Collections"
        "root", "http://forki.github.io/FSharpx.Collections"
        "project-name", "FSharpx.Collections" ]

    Literate.ProcessDirectory (source, template, docsDir, replacements = projInfo)

    if isLocalBuild then  // TODO: this needs to be fixed in FSharp.Formatting
        MetadataFormat.Generate ( 
          ["./build/FSharpx.Collections.dll"], 
          apidocsDir, 
          ["./help/templates/"; "./help/templates/reference/"], 
          parameters = projInfo)

    WriteStringToFile false "./docs/.nojekyll" ""

    CopyDir (docsDir @@ "content") "help/content" allFiles
    CopyDir (docsDir @@ "pics") "help/pics" allFiles
)

Target "PrepareNuget" (fun _ ->
    packages
    |> Seq.iter (fun package ->
        let frameworkSubDir = nugetLibDir package @@ frameworkVersion
        CleanDir frameworkSubDir


        [for ending in ["dll";"pdb";"xml"] do
            yield sprintf "%sFSharpx.%s.%s" buildDir package ending]
        |> Seq.filter (fun f -> System.IO.File.Exists f)
        |> CopyTo frameworkSubDir)
)


Target "Nuget" (fun _ ->
    packages
    |> Seq.iter (fun package ->
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
          |> CopyTo deployDir)
)

Target "DeployZip" (fun _ ->
    !! (buildDir + "/**/*.*")
    |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName buildVersion)
)

Target "ReleaseDocs" (fun _ ->
    CleanDir "gh-pages"
    CommandHelper.runSimpleGitCommand "" "clone -b gh-pages --single-branch git@github.com:forki/FSharpx.Collections.git gh-pages" |> printfn "%s"
    
    fullclean "gh-pages"
    CopyRecursive "docs" "gh-pages" true |> printfn "%A"
    CommandHelper.runSimpleGitCommand "gh-pages" "add . --all" |> printfn "%s"
    CommandHelper.runSimpleGitCommand "gh-pages" (sprintf "commit -m \"Update generated documentation %s\"" buildVersion) |> printfn "%s"
    Branches.push "gh-pages"    
)

Target "Deploy" DoNothing

// Build order
"Clean"
  ==> "AssemblyInfo"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "Test"
  ==> "GenerateDocs"
  ==> "PrepareNuget"
  ==> "Nuget"
  ==> "DeployZip"
  ==> "Deploy"
  ==> "ReleaseDocs"

// Start build
RunTargetOrDefault "Deploy"

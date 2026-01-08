// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------
open System.IO
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.DotNet.Testing
open Fake.Tools
open Fake.JavaScript
open Fake.Runtime

let initializeContext() =
    let execContext = Context.FakeExecutionContext.Create false "build.fsx" []
    Context.setExecutionContext(Context.RuntimeContext.Fake execContext)

initializeContext()

// Target configuration
let configuration = "Release"

// Read additional information from the release notes document
let release = ReleaseNotes.load "RELEASE_NOTES.md"

// Helper active pattern for project types
let (|Fsproj|Csproj|Vbproj|)(projFileName: string) =
    match projFileName with
    | f when f.EndsWith("fsproj") -> Fsproj
    | f when f.EndsWith("csproj") -> Csproj
    | f when f.EndsWith("vbproj") -> Vbproj
    | _ -> failwith(sprintf "Project file %s not supported. Unknown project type." projFileName)

// Generate assembly info files with the right version & up-to-date information
Target.create "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName = [
        AssemblyInfo.Title(projectName)
        AssemblyInfo.Product "FSharpx.Collections"
        AssemblyInfo.Description "FSharpx.Collections is a collection of datastructures for use with F# and C#."
        AssemblyInfo.InternalsVisibleTo "FSharpx.Collections.Tests"
        AssemblyInfo.InternalsVisibleTo "FSharpx.Collections.Experimental.Tests"
        AssemblyInfo.Version release.AssemblyVersion
        AssemblyInfo.FileVersion release.AssemblyVersion
        AssemblyInfo.Configuration configuration
    ]

    let getProjectDetails(projectPath: string) =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        (projectPath, projectName, System.IO.Path.GetDirectoryName(projectPath), (getAssemblyInfoAttributes projectName))

    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter(fun (projFileName, _, folderName, attributes) ->
        match projFileName with
        | Fsproj -> AssemblyInfoFile.createFSharp (folderName </> "AssemblyInfo.fs") attributes
        | Csproj -> AssemblyInfoFile.createCSharp ((folderName </> "Properties") </> "AssemblyInfo.cs") attributes
        | Vbproj -> AssemblyInfoFile.createVisualBasic ((folderName </> "My Project") </> "AssemblyInfo.vb") attributes))

// --------------------------------------------------------------------------------------
// Clean build results

let buildConfiguration =
    DotNet.Custom
    <| Environment.environVarOrDefault "configuration" configuration

Target.create "Clean" (fun _ -> Shell.cleanDirs [ "bin"; "temp" ])

// --------------------------------------------------------------------------------------
// Build library & test project

Target.create "Build" (fun _ ->
    "FSharpx.Collections.sln"
    |> DotNet.build(fun p ->
        { p with
            Configuration = buildConfiguration
        }))

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner

Target.create "RunTests" (fun _ ->
    !! "tests/**/bin/Release/net6.0/*Tests.dll"
    |> Expecto.run(fun x ->
        { x with
            Parallel = true
            ParallelWorkers = System.Environment.ProcessorCount
        }))

Target.create "RunTestsFable" (fun _ ->
    let setParams =
        (fun (o: Yarn.YarnParams) ->
            { o with
                WorkingDirectory = "tests/fable"
            })

    Yarn.installPureLock setParams
    Yarn.exec "test" setParams)

// --------------------------------------------------------------------------------------
// Build a NuGet package
let nuGet out suffix =
    let releaseNotes = release.Notes |> String.toLines

    Paket.pack(fun p ->
        { p with
            ToolType = ToolType.CreateLocalTool()
            OutputPath = out
            Version = release.NugetVersion + (suffix |> Option.defaultValue "")
            ReleaseNotes = releaseNotes
        })

Target.create "NuGet" (fun _ -> nuGet "bin" None)

Target.create "CINuGet" (fun _ ->
    let suffix = "-alpha" + (System.DateTime.UtcNow.ToString("yyyyMMddHHmmss"))
    nuGet "temp" (Some suffix))

let ensureOk(pr: ProcessResult) =
    if not pr.OK then
        failwith(String.concat "," pr.Errors)

Target.create "PublishCINuGet" (fun _ ->
    let token = Environment.environVarOrFail "GITHUB_TOKEN"

    !! "temp/*.nupkg"
    |> Seq.iter(fun file ->
        DotNet.exec id "nuget" (sprintf "push %s -s https://nuget.pkg.github.com/fsprojects/index.json -k %s" file token)
        |> ensureOk //
    ))

Target.create "PublishNuget" (fun _ ->
    Paket.push(fun p ->
        { p with
            ToolType = ToolType.CreateLocalTool()
            WorkingDir = "bin"
        }))

// --------------------------------------------------------------------------------------
// Generate the documentation

Target.create "GenerateDocs" (fun _ ->
    Shell.cleanDir ".fsdocs"
    DotNet.exec id "build" "" |> ensureOk // we need assemblies compiled in debug mode for docs
    DotNet.exec id "fsdocs" "build --clean --eval" |> ensureOk)
// --------------------------------------------------------------------------------------
// Release Scripts

Target.create "ReleaseDocs" (fun _ ->
    let projectRepo = "https://github.com/fsprojects/FSharpx.Collections"

    Git.Repository.clone "" projectRepo "temp/gh-pages"
    Git.Branches.checkoutBranch "temp/gh-pages" "gh-pages"
    Shell.copyRecursive "output" "temp/gh-pages" true |> printfn "%A"

    Git.CommandHelper.runSimpleGitCommand "temp/gh-pages" "add ."
    |> printfn "%s"

    let cmd =
        sprintf """commit -a -m "Update generated documentation for version %s""" release.NugetVersion

    Git.CommandHelper.runSimpleGitCommand "temp/gh-pages" cmd
    |> printfn "%s"

    Git.Branches.push "temp/gh-pages")

Target.create "Release" (fun _ ->
    Git.Staging.stageAll ""
    Git.Commit.exec "" (sprintf "Bump version to %s" release.NugetVersion)
    Git.Branches.push ""

    Git.Branches.tag "" release.NugetVersion
    Git.Branches.pushTag "" "origin" release.NugetVersion)

// --------------------------------------------------------------------------------------
// Fantomas code formatting and style checking

let sourceFiles =
    !! "**/*.fs" ++ "**/*.fsx"
    -- "**/fable_modules/**/**.fs"
    -- "packages/**/*.*"
    -- "paket-files/**/*.*"
    -- ".fake/**/*.*"
    -- "**/obj/**/*.*"
    -- "**/AssemblyInfo.fs"

Target.create "Format" (fun _ ->
    let result =
        sourceFiles
        |> Seq.map(sprintf "\"%s\"")
        |> String.concat " "
        |> DotNet.exec id "fantomas"

    if not result.OK then
        printfn "Errors while formatting all files: %A" result.Messages)

Target.create "CheckFormat" (fun _ ->
    let result =
        sourceFiles
        |> Seq.map(sprintf "\"%s\"")
        |> String.concat " "
        |> sprintf "%s --check"
        |> DotNet.exec id "fantomas"

    if result.ExitCode = 0 then
        Trace.log "No files need formatting"
    elif result.ExitCode = 99 then
        failwith "Some files need formatting, run `dotnet fake build -t Format` to format them"
    else
        Trace.logf "Errors while formatting: %A" result.Errors
        failwith "Unknown errors while formatting")

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target.create "All" ignore

"Clean"
==> "AssemblyInfo"
==> "CheckFormat"
==> "Build"
==> "RunTests"
==> "RunTestsFable"
==> "CINuGet"
==> "GenerateDocs"
==> "All"
|> ignore

"All" ==> "NuGet" ==> "PublishNuget" ==> "ReleaseDocs" ==> "Release"
|> ignore

[<EntryPoint>]
let main args =
    try
        match args with
        | [| target |] -> Target.runOrDefault target
        | _ -> Target.runOrDefault "All"

        0
    with e ->
        printfn "%A" e
        1

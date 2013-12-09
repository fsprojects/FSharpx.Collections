@echo off

SET MinimalFAKEVersion=674
SET FAKEVersion=1
cls

if not exist tools\FAKE\tools\PatchVersion.txt ( 
    "tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
)

if not exist tools\FSharp.Formatting\lib\net40\FSharp.CodeFormat.dll ( 
	"tools\nuget\nuget.exe" "install" "FSharp.Formatting" "-OutputDirectory" "tools" "-ExcludeVersion" "-Prerelease"
)

SET TARGET="Deploy"

IF NOT [%1]==[] (set TARGET="%1")
  
"tools\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%"

exit /b %errorlevel%
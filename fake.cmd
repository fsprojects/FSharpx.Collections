@echo off
cls

dotnet tool restore
dotnet restore build.proj
dotnet fake build %*

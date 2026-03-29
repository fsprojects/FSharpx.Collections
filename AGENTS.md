# AGENTS.md — Contributor and AI Agent Guide

This file describes how to build, test, format, and contribute to FSharpx.Collections.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) (version specified in `global.json`)
- [Node.js](https://nodejs.org/) (v24+) — required for the Fable/JavaScript tests
- [Yarn](https://yarnpkg.com/) — required for the Fable tests

## Restore tools and dependencies

All local .NET tools (paket, fsdocs, fable, fantomas) and NuGet dependencies are managed via
Paket. Restore them before building or testing:

```sh
dotnet tool restore
dotnet paket restore
```

## Build

The project uses [FAKE](https://fake.build/) via a local build script.

**Linux / macOS:**

```sh
./build.sh
```

**Windows:**

```cmd
build.cmd
```

Both scripts run `dotnet tool restore`, `dotnet paket restore`, and then execute the FAKE build
pipeline (see `build/build.fs`).

To run a specific FAKE target:

```sh
./build.sh <Target>      # e.g. ./build.sh Build
build.cmd <Target>       # e.g. build.cmd Build
```

Common targets:

| Target          | Description                                                   |
|-----------------|---------------------------------------------------------------|
| `Build`         | Compile the solution (`FSharpx.Collections.sln`) in Release  |
| `RunTests`      | Run .NET unit tests (Expecto, parallel)                       |
| `RunTestsFable` | Run Fable/JavaScript tests (requires Node + Yarn)             |
| `CheckFormat`   | Check formatting with Fantomas (fails if files need changes)  |
| `Format`        | Format all source files with Fantomas                         |
| `All`           | Full pipeline: Clean → AssemblyInfo → CheckFormat → Build → RunTests → RunTestsFable → CINuGet → GenerateDocs |
| `NuGet`         | Pack NuGet packages into `bin/`                               |

To run only the .NET build and tests (skipping Fable and docs):

```sh
./build.sh RunTests   # also runs Build as a prerequisite
```

## Run tests directly (without FAKE)

After building, you can run the .NET tests directly:

```sh
dotnet test tests/FSharpx.Collections.Tests/FSharpx.Collections.Tests.fsproj -c Release
dotnet test tests/FSharpx.Collections.Experimental.Tests/FSharpx.Collections.Experimental.Tests.fsproj -c Release
```

For the Fable/JavaScript tests:

```sh
cd tests/fable
yarn install --frozen-lockfile
yarn test
```

## Code formatting

The project uses [Fantomas](https://fsprojects.github.io/fantomas/) (version pinned in
`.config/dotnet-tools.json`) to enforce consistent formatting.

**Check whether any files need formatting (CI enforces this):**

```sh
./build.sh CheckFormat
```

**Auto-format all source files:**

```sh
./build.sh Format
```

Or run Fantomas directly:

```sh
dotnet fantomas <file-or-directory>
dotnet fantomas --check <file-or-directory>
```

> **Important:** CI runs `CheckFormat` before `Build`. A PR with unformatted code will fail CI.

## CI pipeline

The GitHub Actions workflow (`.github/workflows/dotnet.yml`) runs on every push and pull request,
on both `ubuntu-latest` and `windows-latest`.  It:

1. Restores tools and packages.
2. Runs `dotnet run --project build/build.fsproj` (equivalent to `./build.sh All`), which covers
   check-format → build → .NET tests → Fable tests → NuGet packaging → docs generation.

All of the above must pass before a PR can be merged.

## Project structure

```
src/
  FSharpx.Collections/               Core collections library
  FSharpx.Collections.Experimental/  Experimental / less-stable collections
tests/
  FSharpx.Collections.Tests/         .NET tests for the core library (Expecto)
  FSharpx.Collections.Experimental.Tests/  .NET tests for experimental collections
  fable/                             Fable/JavaScript tests (mocha)
build/                               FAKE build script (build.fs)
docs/                                Documentation source (fsdocs)
```

## Contribution guidelines

- Keep PRs small and focused — one concern per PR.
- Add or update tests for any behaviour you change.
- Run `./build.sh CheckFormat` (or `build.cmd CheckFormat`) before opening a PR.
- Run the full test suite (`./build.sh RunTests`) before opening a PR.
- No new NuGet dependencies without prior discussion in an issue.
- No breaking API changes without maintainer approval.

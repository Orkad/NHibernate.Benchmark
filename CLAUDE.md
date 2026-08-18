# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repository is

A [BenchmarkDotNet](https://benchmarkdotnet.org/) harness that measures NHibernate performance
characteristics — mapping-strategy initialization cost, projection vs. full-entity query cost,
and session/change-tracking cost as result-set size grows. There is no production application
code here; every class in `NHibernate.Benchmark/` is a `[Benchmark]`-decorated class run by
BenchmarkDotNet, and `NHibernate.Benchmark.AuthorWork/` is a shared library of NHibernate entity
models + mappings (Fluent, ByCode, and XML/`.hbm.xml`) used as fixtures across the benchmarks.

`readme.md` at the repo root is not a project description — it is the last recorded
BenchmarkDotNet results table (checked in as a historical reference point), so don't expect
setup/usage docs there.

## Solution layout

- `NHibernate.Benchmark.sln` — two projects:
  - `NHibernate.Benchmark/` — the executable BenchmarkDotNet suite (`OutputType=Exe`).
    Targets `net48;net8.0;net9.0`.
  - `NHibernate.Benchmark.AuthorWork/` — class library with the shared domain model
    (`Person`, `Author`, `Work`, `Book`, `Song`) and three parallel mapping styles under
    `Mappings/{ByCode,Fluent,Xml}/`. Targets `net48;net8.0;net10.0`.
  - Note the target framework lists differ (net9.0 vs net10.0) between the two projects, and
    `InitializationBenchmark` references `RuntimeMoniker.Net10_0` in a `[SimpleJob]` attribute
    even though the executable project's TFMs stop at net9.0 — be aware of this mismatch if a
    build/job fails to resolve for that runtime moniker.
- All storage in benchmarks is an in-memory SQLite database (`Data Source=:memory:;Version=3;New=True;`)
  via `System.Data.SQLite.Core`, configured through `NHibernate.Cfg.Configuration.DataBaseIntegration`.
- Test data is generated with `Bogus` (seeded via `Bogus.Randomizer.Seed`) or simple loops, inserted
  through an `IStatelessSession` in `[GlobalSetup]`/`[IterationSetup]`.

## Build / run commands

This environment does not have the `dotnet` SDK installed, so builds/benchmarks cannot be
executed here — describe/edit code changes but note to the user that verification requires a
machine with the .NET SDK (and, for the `net48` TFM, .NET Framework 4.8/mono).

`.github/workflows/benchmarks.yml` runs the suite on a `windows-latest` GitHub Actions runner
(required for the `net48` job and for `System.Data.SQLite.Core`'s Windows-only native binaries),
triggered manually (`workflow_dispatch`) with inputs to pick one benchmark class or `All` and the
host TFM — never on every push, since these are slow perf runs, not correctness tests.

When a `dotnet` SDK is available:

```bash
# Restore & build the whole solution
dotnet build NHibernate.Benchmark.sln

# Build/run just the benchmark exe for one TFM
dotnet build NHibernate.Benchmark/NHibernate.Benchmark.csproj -f net8.0
```

BenchmarkDotNet benchmarks are run via `BenchmarkSwitcher`, not `dotnet test` — there is no unit
test project in this repo. Run the compiled benchmark exe with `-f <filter>` to pick a class
(the four launch profiles in `NHibernate.Benchmark/Properties/launchSettings.json` show the
expected filter pattern, e.g. `-f NHibernate.Benchmark.ProjectionBenchmark*`):

```bash
dotnet run -c Release --project NHibernate.Benchmark -f net8.0 -- -f NHibernate.Benchmark.ProjectionBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net8.0 -- -f NHibernate.Benchmark.InitializationBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net8.0 -- -f NHibernate.Benchmark.TrackingBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net8.0 -- -f NHibernate.Benchmark.LatencyBenchmark*
```

BenchmarkDotNet always builds a Release-mode isolated copy before executing, so always build/run
in `Release` — a Debug run will be rejected by BenchmarkDotNet's validators.

Some `[Benchmark]` methods in `ProjectionBenchmark` (`QueryOverNoProjection`/`Projection`,
`HqlNoProjection`/`Projection`, `SqlNoProjection`/`Projection`) are commented out rather than
deleted — this is an intentional way of narrowing which query APIs run without losing the code;
follow the same pattern (comment out, don't delete) if asked to prune benchmarks there.

## Architecture notes for the four benchmark classes

- **`InitializationBenchmark`** — compares session-factory build cost across the three mapping
  styles (Fluent / XML / ByCode) and, for each, "explicit type list" vs. "scan whole assembly".
  Uses `[IterationSetup]` to build a fresh `Configuration` per iteration and `ColdStart` jobs
  across net48/net8.0/net10.0 (see the TFM mismatch note above) with `launchCount: 30`,
  `iterationCount: 1`, `invocationCount: 1` — i.e. each launch is a full cold-start process, which
  is what makes this comparable to real app-startup cost.
- **`ProjectionBenchmark`** — compares fetching full `Person` entities (tracked and read-only)
  against LINQ projections into `PersonDto` with an increasing number of selected columns
  (1 through all 7 fields), across `ElementsCount` row counts (`[Params]`). Data is seeded once in
  `[GlobalSetup]`; each `[IterationSetup]` opens a fresh `ISession` on the same in-memory connection.
- **`TrackingBenchmark`** — measures `session.Flush()` cost after loading `ElementsCount` entities
  into a session's first-level cache (`[Params]` spans 2 to over 1,000,000), isolating pure
  session/change-tracking overhead from query cost. Uses `RunStrategy.Monitoring` (not `ColdStart`)
  since it measures steady-state flush cost, not process startup.
- **`LatencyBenchmark`** — not NHibernate-related; a baseline `Ping`-based latency measurement
  against `localhost`. Kept separate from the DB-related benchmarks as a sanity/noise baseline.

When adding a new benchmark class, follow the existing convention: put shared entity/mapping code
in `NHibernate.Benchmark.AuthorWork`, keep the SQLite in-memory setup pattern, and add a matching
profile to `Properties/launchSettings.json` with a `-f <Namespace.ClassName>*` filter.

## Style

`.editorconfig` enforces (via analyzers, not just IDE formatting) `file_scoped` namespaces,
4-space indentation, and CRLF line endings — match these in any new `.cs` file.

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repository is

A [BenchmarkDotNet](https://benchmarkdotnet.org/) harness that measures NHibernate performance
characteristics — mapping-strategy initialization cost, projection vs. full-entity query cost,
and session/change-tracking cost as result-set size grows — plus a small `EfCoreComparison/`
suite that puts NHibernate head-to-head against EF Core on those same three angles. There is no
production application code here; every class in `NHibernate.Benchmark/` is a `[Benchmark]`-decorated
class run by BenchmarkDotNet, and `NHibernate.Benchmark.AuthorWork/` is a shared library of
NHibernate entity models + mappings (Fluent, ByCode, and XML/`.hbm.xml`) used as fixtures across
the benchmarks.

`readme.md` at the repo root is a results index, not a setup/usage guide: it links to a
per-benchmark-class `.md` report (checked in next to each `[Benchmark]` class, e.g.
`NHibernate.Benchmark/ProjectionBenchmark.md`) and gives a cross-cutting synthesis of the latest
recorded run. Each report is a historical snapshot tied to a specific CI run — when benchmark
code or results change meaningfully, regenerate the affected report(s) from a fresh
`benchmarks.yml` run and update `readme.md`'s synthesis if the conclusions shifted, rather than
letting them drift silently out of sync with the code.

## Solution layout

- `NHibernate.Benchmark.sln` — three projects:
  - `NHibernate.Benchmark/` — the executable BenchmarkDotNet suite (`OutputType=Exe`), targeting
    `net48;net10.0`.
  - `NHibernate.Benchmark.AuthorWork/` — class library with the shared domain model
    (`Person`, `Author`, `Work`, `Book`, `Song`) and three parallel mapping styles under
    `Mappings/{ByCode,Fluent,Xml}/`, also targeting `net48;net10.0`.
  - `NHibernate.Benchmark.AuthorWork.EfCore/` — class library holding the EF Core side of the
    `EfCoreComparison/` suite (a mirror `Person` entity + a `PersonContext : DbContext`).
    **`net10.0`-only** — EF Core's current packages don't support `net48` — so
    `NHibernate.Benchmark.csproj` only takes a `ProjectReference` on it, and only compiles
    `EfCoreComparison/**/*.cs`, when building for `net10.0` (see the `Condition`-gated
    `ItemGroup`s in that csproj).
  - All `[SimpleJob]` runtime monikers across the four NHibernate-only benchmark classes are
    `Net48`/`Net10_0` only, matching those TFMs — net8.0/net9.0 support was intentionally dropped
    (2026-08-18) to keep the NHibernate projects' target frameworks in sync and avoid resolving
    jobs for a TFM the exe project doesn't build. The `EfCoreComparison/` classes are
    `Net10_0`-only for the reason above.
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
triggered manually (`workflow_dispatch`) with an input to pick one benchmark class or `All` —
never on every push, since these are slow perf runs, not correctness tests. It only needs the
`net10.0` SDK set up explicitly; `net48` is served by the .NET Framework dev pack preinstalled on
the runner image.

When a `dotnet` SDK is available:

```bash
# Restore & build the whole solution
dotnet build NHibernate.Benchmark.sln

# Build/run just the benchmark exe for one TFM
dotnet build NHibernate.Benchmark/NHibernate.Benchmark.csproj -f net10.0
```

BenchmarkDotNet benchmarks are run via `BenchmarkSwitcher`, not `dotnet test` — there is no unit
test project in this repo. Run the compiled benchmark exe with `-f <filter>` to pick a class
(the launch profiles in `NHibernate.Benchmark/Properties/launchSettings.json` show the
expected filter pattern, e.g. `-f NHibernate.Benchmark.ProjectionBenchmark*`):

```bash
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.ProjectionBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.InitializationBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.TrackingBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.LatencyBenchmark*

# EF Core vs NHibernate comparison suite (net10.0 only, see Architecture notes below)
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.EfCoreComparison.InitializationBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.EfCoreComparison.ProjectionBenchmark*
dotnet run -c Release --project NHibernate.Benchmark -f net10.0 -- -f NHibernate.Benchmark.EfCoreComparison.TrackingBenchmark*
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
  across net48/net10.0 with `launchCount: 30`, `iterationCount: 1`, `invocationCount: 1` — i.e.
  each launch is a full cold-start process, which is what makes this comparable to real
  app-startup cost.
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

## `EfCoreComparison/` suite

`NHibernate.Benchmark/EfCoreComparison/` mirrors the three data-driven benchmarks above, but each
class puts an NHibernate `[Benchmark]` method and an EF Core `[Benchmark]` method side by side
(`NHibernateFlush` vs. `EfCoreSaveChanges`, etc.) so a single BenchmarkDotNet results table shows
both ORMs directly compared, rather than two separate tables. It exists alongside — not instead
of — the four classes above, which remain the untouched NHibernate-only reference.

- **`InitializationBenchmark`** — NHibernate `SessionFactory` build cost vs. EF Core's first-use
  model build cost. The NHibernate side uses a Fluent mapping as the `[Benchmark(Baseline = true)]`
  and keeps a ByCode mapping alongside it for reference, so the table shows both NHibernate
  mapping styles next to EF Core. `ColdStart`, `launchCount: 30`, matching the root
  `InitializationBenchmark`'s methodology.
- **`ProjectionBenchmark`** — full entity (tracked/read-only) vs. full-field projection, across
  the same `ElementsCount` values as the root `ProjectionBenchmark`. Each ORM gets its own
  in-memory SQLite database/connection, seeded independently but with an identical `Bogus` seed
  so row content matches between the two.
- **`TrackingBenchmark`** — `session.Flush()` vs. `context.SaveChanges()` cost with
  `ElementsCount` entities tracked and no pending changes, isolating change-tracking overhead the
  same way the root `TrackingBenchmark` does.

These three classes — and `NHibernate.Benchmark.AuthorWork.EfCore/`, which they depend on — are
**`net10.0`-only**: current EF Core packages don't support `net48`. `NHibernate.Benchmark.csproj`
gates both the `ProjectReference` and the `EfCoreComparison/**/*.cs` sources behind
`Condition="'$(TargetFramework)'=='net10.0'"` so the `net48` build of the exe is unaffected.
There's no `LatencyBenchmark` equivalent here since it isn't NHibernate/EF-Core-related.

## Style

`.editorconfig` enforces (via analyzers, not just IDE formatting) `file_scoped` namespaces,
4-space indentation, and CRLF line endings — match these in any new `.cs` file.

# EfCoreComparison.InitializationBenchmark

Compares NHibernate `SessionFactory` build cost (Fluent mapping as the baseline, ByCode kept
alongside for reference) against EF Core's first-use model build cost, all against a fresh
SQLite in-memory schema. `ColdStart`, `launchCount: 30`, matching the root
[`InitializationBenchmark`](../InitializationBenchmark.md)'s methodology. `net10.0` only — EF
Core's current packages don't support `net48`.

Source: [`InitializationBenchmark.cs`](./InitializationBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `Job-BYFOFT`: .NET 10.0.11, X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
- `InvocationCount=1  IterationCount=1  LaunchCount=30  RunStrategy=ColdStart  UnrollFactor=1`
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method                          | Mean     | Error   | StdDev  | Ratio | RatioSD | Allocated  | Alloc Ratio |
|-------------------------------- |---------:|--------:|--------:|------:|--------:|-----------:|------------:|
| NHibernateFluentInitialization  | 529.4 ms | 4.03 ms | 6.03 ms |  1.00 |    0.02 | 1042.31 KB |        1.00 |
| NHibernateByCodeInitialization  | 359.7 ms | 3.24 ms | 4.85 ms |  0.68 |    0.01 |  787.59 KB |        0.76 |
| EfCoreInitialization            | 698.6 ms | 3.22 ms | 4.82 ms |  1.32 |    0.02 |  833.82 KB |        0.80 |

Ratio is relative to `NHibernateFluentInitialization` (the `[Benchmark(Baseline = true)]` method).

## Takeaways

- **NHibernate ByCode is the fastest cold-start path of the three** — 32% faster than the Fluent
  baseline and 48% faster than EF Core.
- **EF Core's first-use cost (model build + `EnsureCreated`) is ~1.3× the Fluent NHibernate
  baseline and ~1.9× ByCode** — consistent with EF Core compiling its model and running schema
  creation on first access, comparable in spirit to NHibernate's mapping compilation but
  measurably heavier here.
- **Allocations are closer than the timing gap suggests**: EF Core (834 KB) actually allocates
  less than Fluent NHibernate (1042 KB), and only slightly more than ByCode (788 KB) — the
  time cost isn't purely a memory-pressure story.
- Compare against the root suite's [`InitializationBenchmark`](../InitializationBenchmark.md) for
  the full same-ORM mapping-style breakdown this class doesn't repeat (XML, assembly-scan
  variants, `net48` numbers).

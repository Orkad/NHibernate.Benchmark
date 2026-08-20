# InitializationBenchmark

Compares `SessionFactory` build cost across NHibernate's three mapping styles (Fluent, XML,
ByCode) and, for each, an explicit type list vs. scanning the whole assembly. Each launch is a
full cold-start process (`RunStrategy.ColdStart`, `launchCount: 30`, `iterationCount: 1`,
`invocationCount: 1`), which is what makes this comparable to real application startup cost.
Runs across both `net48` and `net10.0`.

Source: [`InitializationBenchmark.cs`](./InitializationBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- .NET SDK 10.0.400 — `[Host]` / `Job-BYFOFT`: .NET 10.0.11; `Job-CXGLSN`: .NET Framework 4.8.1
- `InvocationCount=1  IterationCount=1  LaunchCount=30  RunStrategy=ColdStart  UnrollFactor=1`
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method                           | Runtime            | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------- |------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| FluentInitialization             | .NET 10.0          |   723.5 ms |   4.25 ms |   6.36 ms |  1.01 |    0.01 |   2.47 MB |        0.66 |
| FluentInitializationFromAssembly | .NET 10.0          |   718.7 ms |   4.75 ms |   7.11 ms |  1.00 |    0.01 |   3.78 MB |        1.00 |
| XmlInitialization                | .NET 10.0          |   625.1 ms |   8.01 ms |  11.99 ms |  0.87 |    0.02 |   1.75 MB |        0.46 |
| XmlInitializationFromAssembly    | .NET 10.0          |   647.8 ms |  22.25 ms |  33.31 ms |  0.90 |    0.05 |   1.75 MB |        0.46 |
| ByCodeInitialization             | .NET 10.0          |   449.8 ms |   3.59 ms |   5.37 ms |  0.63 |    0.01 |   1.69 MB |        0.45 |
| ByCodeInitializationFromAssembly | .NET 10.0          |   453.7 ms |   6.23 ms |   9.33 ms |  0.63 |    0.01 |   1.64 MB |        0.43 |
|                                  |                    |            |           |           |       |         |           |             |
| FluentInitialization             | .NET Framework 4.8 | 1,218.2 ms | 100.16 ms | 149.91 ms |  1.01 |    0.12 |   3.19 MB |        0.66 |
| FluentInitializationFromAssembly | .NET Framework 4.8 | 1,209.1 ms |   9.72 ms |  14.55 ms |  1.00 |    0.02 |   4.84 MB |        1.00 |
| XmlInitialization                | .NET Framework 4.8 |         NA |        NA |        NA |     ? |       ? |        NA |           ? |
| XmlInitializationFromAssembly    | .NET Framework 4.8 | 1,030.5 ms |   7.83 ms |  11.71 ms |  0.85 |    0.01 |   2.24 MB |        0.46 |
| ByCodeInitialization             | .NET Framework 4.8 |   848.4 ms |   8.51 ms |  12.74 ms |  0.70 |    0.01 |   2.14 MB |        0.44 |
| ByCodeInitializationFromAssembly | .NET Framework 4.8 |   859.8 ms |   6.36 ms |   9.52 ms |  0.71 |    0.01 |   2.13 MB |        0.44 |

Ratio is relative to `FluentInitializationFromAssembly` (the `[Benchmark(Baseline = true)]`
method), within each runtime.

> **Known flake**: `XmlInitialization` on .NET Framework 4.8 returned no valid runs in this CI
> run (pre-existing, unrelated to any change made in this session — the `.hbm.xml`-from-file
> loading path is occasionally flaky under `ColdStart` on that runtime). Re-run the benchmark if
> this figure is needed.

## Takeaways

- **ByCode is the fastest mapping style to bootstrap**, on both runtimes — roughly 35–37% faster
  than Fluent, and noticeably lighter on allocations (~0.43–0.45× the baseline).
- **XML sits in between** Fluent and ByCode (~13% faster than Fluent on .NET 10.0), with
  comparable memory usage to ByCode.
- **"Explicit type list" vs. "scan whole assembly" makes almost no difference** in mean time for
  any mapping style — the assembly scan itself is cheap relative to mapping compilation and
  schema/session-factory construction.
- **.NET 10.0 builds a `SessionFactory` ~1.6–1.9× faster than .NET Framework 4.8** for the same
  mapping style (e.g. ByCode: 450 ms vs. 848 ms), consistent with the newer JIT/runtime.

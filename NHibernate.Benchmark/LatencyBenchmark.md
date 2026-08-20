# LatencyBenchmark

Not NHibernate-related — a baseline `Ping`-based latency measurement against `localhost`. Kept
separate from the DB-related benchmarks as a sanity/noise baseline for the CI runner itself.

Source: [`LatencyBenchmark.cs`](./LatencyBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `Job-UWLSOM`: .NET 10.0.11, X64 RyuJIT AVX2
- `IterationCount=10  LaunchCount=1  WarmupCount=3`
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method  | Mean     | Error   | StdDev  | Min      | Max      |
|-------- |---------:|--------:|--------:|---------:|---------:|
| Latency | 465.3 us | 7.61 us | 5.04 us | 457.1 us | 474.4 us |

## Takeaways

- **~465 us round-trip to localhost** on this runner class establishes the measurement noise
  floor for the CI environment. Every other benchmark in this suite operates at hundreds of
  microseconds to milliseconds and above, so this floor isn't a confounding factor for those
  numbers — but it's a useful reference when a result looks suspiciously close to it.
- Low variance here (StdDev ≈ 1% of mean) compared to the DB-related benchmarks confirms most of
  the noise seen elsewhere in this suite comes from the workloads themselves (GC, JIT, SQLite),
  not from generic CI/VM jitter.

# TrackingBenchmark

Measures `session.Flush()` cost after loading `ElementsCount` entities into a session's
first-level cache, with no pending changes — isolating pure session/change-tracking overhead
from query cost. Uses `RunStrategy.Monitoring` (not `ColdStart`) since it measures steady-state
flush cost, not process startup. `net10.0` only.

Source: [`TrackingBenchmark.cs`](./TrackingBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `Job-UWLSOM`: .NET 10.0.11, X64 RyuJIT AVX2
- `IterationCount=30  RunStrategy=Monitoring  UnrollFactor=1`
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method          | ElementsCount | Mean         | Error       | StdDev      | Median        | Gen0       | Gen1       | Allocated    |
|---------------- |-------------- |-------------:|------------:|------------:|--------------:|-----------:|-----------:|-------------:|
| SessionTracking | 2             |     395.9 us |  1,337.4 us |  2,001.8 us |      26.60 us |          - |          - |      1.43 KB |
| SessionTracking | 4             |     400.9 us |  1,331.2 us |  1,992.5 us |      35.50 us |          - |          - |       2.2 KB |
| SessionTracking | 8             |     420.0 us |  1,396.7 us |  2,090.5 us |      34.40 us |          - |          - |      3.73 KB |
| SessionTracking | 16            |     417.5 us |  1,347.9 us |  2,017.4 us |      44.85 us |          - |          - |      6.79 KB |
| SessionTracking | 32            |     433.9 us |  1,313.4 us |  1,965.9 us |      66.95 us |          - |          - |     12.91 KB |
| SessionTracking | 64            |     505.9 us |  1,384.2 us |  2,071.7 us |     121.55 us |          - |          - |     25.16 KB |
| SessionTracking | 128           |     586.1 us |  1,388.6 us |  2,078.3 us |     198.70 us |          - |          - |     49.66 KB |
| SessionTracking | 258           |     867.5 us |  1,693.5 us |  2,534.8 us |     372.90 us |          - |          - |     99.43 KB |
| SessionTracking | 512           |   2,316.4 us |  2,630.9 us |  3,937.8 us |   1,030.00 us |          - |          - |    196.66 KB |
| SessionTracking | 1024          |   2,943.9 us |  3,160.1 us |  4,729.9 us |   1,368.45 us |          - |          - |    392.66 KB |
| SessionTracking | 2048          |   4,843.1 us |  3,137.1 us |  4,695.5 us |   2,871.15 us |          - |          - |    784.66 KB |
| SessionTracking | 4096          |   7,297.0 us |  3,903.4 us |  5,842.4 us |   5,223.10 us |          - |          - |   1568.66 KB |
| SessionTracking | 8192          |  11,288.4 us |  6,506.6 us |  9,738.7 us |  11,166.50 us |          - |          - |   3136.66 KB |
| SessionTracking | 16384         |  10,886.5 us |  7,602.3 us | 11,378.8 us |   5,733.40 us |          - |          - |   6272.66 KB |
| SessionTracking | 32768         |  17,527.6 us |  7,864.1 us | 11,770.7 us |  13,832.85 us |          - |          - |  12544.66 KB |
| SessionTracking | 65536         |  50,007.5 us |  9,279.3 us | 13,888.8 us |  45,505.85 us |  1000.0000 |          - |  25088.66 KB |
| SessionTracking | 131072        | 105,385.1 us | 12,251.4 us | 18,337.4 us | 100,239.70 us |  2000.0000 |          - |  50176.66 KB |
| SessionTracking | 262144        | 218,707.1 us | 18,231.8 us | 27,288.5 us | 212,799.40 us |  5000.0000 |  2000.0000 | 100352.66 KB |
| SessionTracking | 524288        | 429,201.6 us | 16,727.1 us | 25,036.4 us | 426,353.85 us | 11000.0000 |  6000.0000 | 200704.66 KB |
| SessionTracking | 1048576       | 834,751.0 us | 30,925.6 us | 46,288.0 us | 830,075.95 us | 23000.0000 | 10000.0000 | 401408.66 KB |

## Takeaways

- **Flush cost with zero dirty entities scales roughly linearly with the number of tracked
  entities** — the persistence context has to walk every tracked instance to detect changes,
  so cost is a function of first-level-cache size, not of anything actually being written.
- **Allocation is consistently ~384 bytes per tracked entity** (401,408.66 KB / 1,048,576
  entities ≈ 384 B), a stable per-entity change-tracking overhead across four orders of
  magnitude.
- **Below ~500 entities, the mean is dominated by noise, not signal**: `RunStrategy.Monitoring`
  has no warmup phase, so the first few of the 30 iterations include JIT/GC warmup spikes that
  push the mean well above the (more representative) median. Treat the `Median` column as the
  reliable figure for small `ElementsCount`.
- **Above 65,536 entities**, the flush enters Gen0/Gen1 GC territory and cost/allocation both
  scale cleanly with row count — this is the range where the benchmark's steady-state numbers
  are most trustworthy.

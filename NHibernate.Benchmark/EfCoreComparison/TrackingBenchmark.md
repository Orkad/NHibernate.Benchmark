# EfCoreComparison.TrackingBenchmark

Compares `session.Flush()` (NHibernate) against `context.SaveChanges()` (EF Core) cost with
`ElementsCount` entities tracked and no pending changes, isolating change-tracking overhead the
same way the root [`TrackingBenchmark`](../TrackingBenchmark.md) does. `net10.0` only.

Source: [`TrackingBenchmark.cs`](./TrackingBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `Job-XLQZYO`: .NET 10.0.11, X64 RyuJIT AVX2
- `IterationCount=30  RunStrategy=Monitoring  UnrollFactor=1`
- CI run: [#10](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32401249960) (commit `a0c3d73`)

## Results

| Method            | ElementsCount | Mean          | Error        | StdDev       | Median        | Ratio | RatioSD | Gen0       | Gen1       | Allocated   | Alloc Ratio |
|------------------ |-------------- |--------------:|-------------:|-------------:|--------------:|------:|--------:|-----------:|-----------:|------------:|------------:|
| NHibernateFlush   | 2             |     808.31 us |  1,960.88 us |  2,934.95 us |      32.45 us | 22.30 |   84.97 |          - |          - |      1464 B |        1.00 |
| EfCoreSaveChanges | 2             |      44.51 us |     30.19 us |     45.19 us |      36.40 us |  1.23 |    1.37 |          - |          - |       888 B |        0.61 |
| NHibernateFlush   | 4             |     403.58 us |  1,357.90 us |  2,032.44 us |      29.50 us | 12.73 |   64.89 |          - |          - |      2248 B |        1.00 |
| EfCoreSaveChanges | 4             |     413.21 us |  1,296.96 us |  1,941.22 us |      39.80 us | 13.04 |   61.99 |          - |          - |      1576 B |        0.70 |
| NHibernateFlush   | 8             |     421.42 us |  1,352.17 us |  2,023.86 us |      44.15 us |  8.68 |   43.11 |          - |          - |      3816 B |        1.00 |
| EfCoreSaveChanges | 8             |      74.04 us |     34.51 us |     51.65 us |      56.75 us |  1.52 |    1.20 |          - |          - |      2952 B |        0.77 |
| NHibernateFlush   | 16            |     831.96 us |  1,967.18 us |  2,944.38 us |      50.05 us | 14.27 |   52.97 |          - |          - |      6952 B |        1.00 |
| EfCoreSaveChanges | 16            |     104.66 us |     33.75 us |     50.52 us |      92.80 us |  1.80 |    1.11 |          - |          - |      5704 B |        0.82 |
| NHibernateFlush   | 32            |     442.29 us |  1,349.42 us |  2,019.75 us |      66.65 us |  5.97 |   27.58 |          - |          - |     13224 B |        1.00 |
| EfCoreSaveChanges | 32            |     198.08 us |     32.88 us |     49.21 us |     192.50 us |  2.67 |    0.91 |          - |          - |     11208 B |        0.85 |
| NHibernateFlush   | 64            |     489.81 us |  1,355.90 us |  2,029.44 us |     106.40 us |  4.13 |   17.40 |          - |          - |     25768 B |        1.00 |
| EfCoreSaveChanges | 64            |     333.85 us |     40.55 us |     60.69 us |     307.05 us |  2.82 |    0.88 |          - |          - |     22216 B |        0.86 |
| NHibernateFlush   | 128           |     567.51 us |  1,319.41 us |  1,974.83 us |     192.95 us |  2.68 |    9.40 |          - |          - |     50856 B |        1.00 |
| EfCoreSaveChanges | 128           |   1,364.29 us |  1,463.26 us |  2,190.15 us |     607.95 us |  6.45 |   10.50 |          - |          - |     44232 B |        0.87 |
| NHibernateFlush   | 258           |     775.67 us |  1,365.98 us |  2,044.54 us |     390.05 us |  1.90 |    5.05 |          - |          - |    101816 B |        1.00 |
| EfCoreSaveChanges | 258           |   2,777.15 us |  1,475.87 us |  2,209.02 us |   3,166.70 us |  6.80 |    5.63 |          - |          - |     88952 B |        0.87 |
| NHibernateFlush   | 512           |   2,315.37 us |  2,800.24 us |  4,191.27 us |     756.70 us |  2.41 |    4.80 |          - |          - |    201384 B |        1.00 |
| EfCoreSaveChanges | 512           |   5,389.34 us |  1,825.15 us |  2,731.81 us |   5,925.95 us |  5.62 |    3.90 |          - |          - |    176328 B |        0.88 |
| NHibernateFlush   | 1024          |   3,993.50 us |  3,043.92 us |  4,556.00 us |   2,533.80 us |  1.68 |    2.23 |          - |          - |    402088 B |        1.00 |
| EfCoreSaveChanges | 1024          |   6,049.62 us |    947.46 us |  1,418.11 us |   5,834.30 us |  2.55 |    1.35 |          - |          - |    352456 B |        0.88 |
| NHibernateFlush   | 2048          |   4,457.65 us |  3,076.22 us |  4,604.34 us |   3,671.15 us |  2.21 |    3.50 |          - |          - |    803496 B |        1.00 |
| EfCoreSaveChanges | 2048          |  15,011.42 us |  3,736.32 us |  5,592.34 us |  11,317.25 us |  7.43 |    7.29 |          - |          - |    704712 B |        0.88 |
| NHibernateFlush   | 4096          |   6,674.27 us |  2,938.36 us |  4,398.00 us |   4,596.55 us |  1.24 |    0.94 |          - |          - |   1606312 B |        1.00 |
| EfCoreSaveChanges | 4096          |  13,875.21 us |  6,830.79 us | 10,224.01 us |  10,235.05 us |  2.59 |    2.15 |          - |          - |   1409224 B |        0.88 |
| NHibernateFlush   | 8192          |   8,025.08 us |  5,313.68 us |  7,953.26 us |   3,409.45 us |  1.87 |    2.41 |          - |          - |   3211944 B |        1.00 |
| EfCoreSaveChanges | 8192          |  17,393.80 us | 17,483.43 us | 26,168.38 us |   2,784.05 us |  4.05 |    7.43 |          - |          - |   2818248 B |        0.88 |
| NHibernateFlush   | 16384         |  10,738.26 us |  6,101.66 us |  9,132.67 us |   6,061.55 us |  1.46 |    1.45 |          - |          - |   6423208 B |        1.00 |
| EfCoreSaveChanges | 16384         |  15,343.37 us |  6,018.29 us |  9,007.89 us |  10,387.45 us |  2.09 |    1.55 |          - |          - |   5636296 B |        0.88 |
| NHibernateFlush   | 32768         |  17,872.90 us |  5,559.92 us |  8,321.83 us |  14,697.00 us |  1.11 |    0.58 |          - |          - |  12845736 B |        1.00 |
| EfCoreSaveChanges | 32768         |  25,125.61 us | 11,556.88 us | 17,297.80 us |  19,033.95 us |  1.56 |    1.13 |          - |          - |  11272392 B |        0.88 |
| NHibernateFlush   | 65536         |  46,053.10 us |  7,182.91 us | 10,751.04 us |  43,718.80 us |  1.03 |    0.27 |  1000.0000 |          - |  25690792 B |        1.00 |
| EfCoreSaveChanges | 65536         |  30,204.48 us |    740.71 us |  1,108.67 us |  30,277.55 us |  0.67 |    0.09 |  1000.0000 |          - |  22544584 B |        0.88 |
| NHibernateFlush   | 131072        |  88,676.41 us | 11,319.16 us | 16,941.99 us |  83,908.30 us |  1.02 |    0.22 |  2000.0000 |          - |  51380904 B |        1.00 |
| EfCoreSaveChanges | 131072        |  54,786.09 us |    909.22 us |  1,360.87 us |  54,530.55 us |  0.63 |    0.07 |  2000.0000 |          - |  45088968 B |        0.88 |
| NHibernateFlush   | 262144        | 186,336.77 us | 19,064.66 us | 28,535.09 us | 181,219.55 us |  1.01 |    0.18 |  5000.0000 |  2000.0000 | 102761128 B |        1.00 |
| EfCoreSaveChanges | 262144        | 100,531.57 us |  1,853.35 us |  2,774.00 us | 100,205.40 us |  0.55 |    0.05 |  5000.0000 |          - |  90177736 B |        0.88 |
| NHibernateFlush   | 524288        | 398,841.02 us | 18,037.70 us | 26,997.99 us | 391,049.50 us |  1.00 |    0.09 | 11000.0000 |  6000.0000 | 205521576 B |        1.00 |
| EfCoreSaveChanges | 524288        | 192,691.01 us |  2,707.54 us |  4,052.52 us | 192,307.40 us |  0.48 |    0.03 | 10000.0000 |          - | 180355272 B |        0.88 |
| NHibernateFlush   | 1048576       | 810,731.91 us | 23,669.15 us | 35,426.87 us | 807,177.85 us |  1.00 |    0.06 | 23000.0000 | 10000.0000 | 411042584 B |        1.00 |
| EfCoreSaveChanges | 1048576       | 413,527.54 us |  4,262.46 us |  6,379.85 us | 411,413.75 us |  0.51 |    0.02 | 21000.0000 |          - | 360710344 B |        0.88 |

Ratio is relative to `NHibernateFlush` (the `[Benchmark(Baseline = true)]` method), within each
`ElementsCount` group.

## Takeaways

- **Below ~500 tracked entities, both numbers are noise, not signal**: `RunStrategy.Monitoring`
  has no warmup, so the reported means (and the wild Ratio values like 22.30 at
  `ElementsCount=2`) are dominated by JIT/first-call overhead rather than steady-state flush
  cost. The `Median` column is far more representative at this range, and even then, treat
  anything under a few hundred entities as directional only.
- **The trend flips clearly once volume is large enough to escape that noise floor.** From
  `ElementsCount=65536` on, `EfCoreSaveChanges` is consistently *faster* than `NHibernateFlush`:
  ~0.67× at 65,536 entities, tightening to ~0.51× at 1,048,576 — EF Core's no-op change
  detection pass scales roughly half as expensive as NHibernate's at the high end.
- **EF Core consistently allocates ~12% less than NHibernate** across the whole range
  (Alloc Ratio ≈ 0.88 once past the smallest sizes), regardless of which ORM is faster on time.
- **Combined with [`ProjectionBenchmark`](./ProjectionBenchmark.md)**: EF Core carries a fixed
  per-operation cost premium that dominates at small scale, but its change-tracking and
  projection code paths both scale *at least* as well as NHibernate's, and pull ahead at the
  high end for this workload.

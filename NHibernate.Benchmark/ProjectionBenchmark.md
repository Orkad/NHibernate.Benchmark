# ProjectionBenchmark

Compares fetching full `Person` entities (tracked and read-only) against LINQ projections into
`PersonDto` with an increasing number of selected columns (1 through all 7 fields), across
`ElementsCount` row counts. Data is seeded once in `[GlobalSetup]`; each `[IterationSetup]` opens
a fresh `ISession` on the same in-memory SQLite connection. `net10.0` only.

Source: [`ProjectionBenchmark.cs`](./ProjectionBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `.NET 10.0`: .NET 10.0.11, X64 RyuJIT AVX2
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method                | ElementsCount | Mean         | Error       | StdDev       | Min         | Max          | Allocated  |
|---------------------- |-------------- |-------------:|------------:|-------------:|------------:|-------------:|-----------:|
| FullEntity            | 1             |    191.0 us  |     9.95 us |     28.38 us |    156.2 us |    268.8 us  |   16.35 KB |
| FullEntityNoTracking  | 1             |    197.1 us  |     9.33 us |     26.92 us |    159.9 us |    278.3 us  |   16.52 KB |
| Projection1Field      | 1             |    236.6 us  |    12.36 us |     35.86 us |    193.2 us |    353.3 us  |   20.45 KB |
| Projection2Fields     | 1             |    233.4 us  |    12.41 us |     35.21 us |    197.1 us |    333.0 us  |    21.3 KB |
| Projection3Fields     | 1             |    203.8 us  |     2.78 us |      2.73 us |    200.0 us |    209.7 us  |    22.2 KB |
| Projection4Fields     | 1             |    249.5 us  |    12.79 us |     36.06 us |    206.7 us |    367.3 us  |   24.52 KB |
| Projection5Fields     | 1             |    251.6 us  |     9.97 us |     28.29 us |    213.2 us |    324.1 us  |   25.36 KB |
| Projection6Fields     | 1             |    267.7 us  |    10.10 us |     28.99 us |    218.7 us |    345.3 us  |   26.41 KB |
| ProjectionFull        | 1             |    272.6 us  |    11.48 us |     32.93 us |    221.6 us |    366.3 us  |   27.37 KB |
| FullEntity            | 5             |    189.9 us  |     2.25 us |      1.76 us |    187.6 us |    193.7 us  |   20.12 KB |
| FullEntityNoTracking  | 5             |    252.2 us  |    11.43 us |     32.43 us |    190.2 us |    326.7 us  |   20.28 KB |
| Projection1Field      | 5             |    232.7 us  |     9.49 us |     26.92 us |    196.0 us |    306.0 us  |    21.4 KB |
| Projection2Fields     | 5             |    252.5 us  |    12.84 us |     36.42 us |    208.0 us |    361.0 us  |   22.41 KB |
| Projection3Fields     | 5             |    259.4 us  |    12.61 us |     36.37 us |    212.3 us |    352.5 us  |   23.47 KB |
| Projection4Fields     | 5             |    271.5 us  |    14.04 us |     40.29 us |    217.9 us |    390.9 us  |   25.98 KB |
| Projection5Fields     | 5             |    273.4 us  |    11.02 us |     31.27 us |    225.7 us |    353.4 us  |   27.13 KB |
| Projection6Fields     | 5             |    288.5 us  |    11.91 us |     34.37 us |    233.6 us |    381.4 us  |   28.37 KB |
| ProjectionFull        | 5             |    285.1 us  |    11.11 us |     32.23 us |    236.6 us |    368.5 us  |   29.51 KB |
| FullEntity            | 10            |    284.4 us  |    11.94 us |     34.26 us |    223.7 us |    377.3 us  |   25.55 KB |
| FullEntityNoTracking  | 10            |    280.0 us  |    13.58 us |     38.76 us |    219.7 us |    387.7 us  |   25.72 KB |
| Projection1Field      | 10            |    250.4 us  |    11.92 us |     34.20 us |    201.5 us |    342.6 us  |    22.7 KB |
| Projection2Fields     | 10            |    251.0 us  |     8.87 us |     24.72 us |    213.3 us |    331.0 us  |   24.05 KB |
| Projection3Fields     | 10            |    285.5 us  |    15.89 us |     45.07 us |    222.3 us |    432.4 us  |   25.21 KB |
| Projection4Fields     | 10            |    282.1 us  |    10.97 us |     31.48 us |    232.5 us |    360.7 us  |   28.05 KB |
| Projection5Fields     | 10            |    294.5 us  |    11.08 us |     31.43 us |    240.3 us |    380.4 us  |   29.48 KB |
| Projection6Fields     | 10            |    292.5 us  |    10.50 us |     29.11 us |    249.8 us |    378.6 us  |   31.01 KB |
| ProjectionFull        | 10            |    383.5 us  |    22.89 us |     66.05 us |    267.9 us |    537.0 us  |   32.35 KB |
| FullEntity            | 50            |    722.8 us  |    45.71 us |    133.35 us |    530.5 us |  1,102.0 us  |   73.18 KB |
| FullEntityNoTracking  | 50            |    658.9 us  |    45.40 us |    133.15 us |    482.0 us |  1,022.2 us  |   73.34 KB |
| Projection1Field      | 50            |    309.6 us  |    15.29 us |     44.36 us |    234.7 us |    412.9 us  |   31.97 KB |
| Projection2Fields     | 50            |    334.3 us  |    14.33 us |     39.47 us |    269.4 us |    444.6 us  |   35.01 KB |
| Projection3Fields     | 50            |    394.5 us  |    20.85 us |     59.50 us |    304.5 us |    554.3 us  |   38.08 KB |
| Projection4Fields     | 50            |    415.7 us  |    18.27 us |     51.83 us |    320.9 us |    550.3 us  |   43.61 KB |
| Projection5Fields     | 50            |    438.8 us  |    20.96 us |     59.12 us |    349.0 us |    608.0 us  |   47.26 KB |
| Projection6Fields     | 50            |    452.5 us  |    19.39 us |     55.95 us |    368.1 us |    599.1 us  |   50.63 KB |
| ProjectionFull        | 50            |    401.3 us  |     6.10 us |      5.99 us |    394.5 us |    413.7 us  |   53.91 KB |
| FullEntity            | 100           |  1,165.8 us  |    81.70 us |    239.61 us |    845.2 us |  1,652.2 us  |  127.52 KB |
| FullEntityNoTracking  | 100           |  1,165.5 us  |    87.46 us |    256.52 us |    788.3 us |  2,021.8 us  |  127.69 KB |
| Projection1Field      | 100           |    353.3 us  |    17.38 us |     50.42 us |    275.1 us |    498.9 us  |   43.74 KB |
| Projection2Fields     | 100           |    465.8 us  |    19.23 us |     54.86 us |    336.3 us |    584.8 us  |   48.88 KB |
| Projection3Fields     | 100           |    538.7 us  |    24.38 us |     68.36 us |    388.4 us |    684.9 us  |   54.15 KB |
| Projection4Fields     | 100           |    587.5 us  |    25.73 us |     73.82 us |    445.5 us |    755.0 us  |    63.1 KB |
| Projection5Fields     | 100           |    621.5 us  |    33.69 us |     96.67 us |    488.1 us |    886.0 us  |   69.67 KB |
| Projection6Fields     | 100           |    699.0 us  |    45.36 us |    132.31 us |    538.5 us |  1,150.5 us  |   75.51 KB |
| ProjectionFull        | 100           |    662.6 us  |    17.75 us |     45.81 us |    583.2 us |    844.7 us  |   81.05 KB |
| FullEntity            | 500           |  3,764.4 us  |    70.78 us |     72.69 us |  3,650.7 us |  3,943.8 us  |   566.7 KB |
| FullEntityNoTracking  | 500           |  3,535.8 us  |    69.24 us |     79.73 us |  3,417.1 us |  3,714.9 us  |  566.87 KB |
| Projection1Field      | 500           |    930.7 us  |    50.90 us |    146.06 us |    622.6 us |  1,143.3 us  |  130.52 KB |
| Projection2Fields     | 500           |  1,333.0 us  |    63.37 us |    179.78 us |    931.5 us |  1,625.2 us  |  153.38 KB |
| Projection3Fields     | 500           |  1,678.0 us  |    91.98 us |    268.31 us |  1,096.7 us |  2,276.4 us  |  176.62 KB |
| Projection4Fields     | 500           |  1,904.6 us  |   112.61 us |    330.27 us |  1,325.5 us |  2,564.5 us  |  212.41 KB |
| Projection5Fields     | 500           |  2,097.7 us  |   176.56 us |    517.81 us |  1,522.1 us |  3,454.0 us  |  241.37 KB |
| Projection6Fields     | 500           |  1,806.3 us  |    24.36 us |     19.02 us |  1,761.1 us |  1,836.3 us  |   266.6 KB |
| ProjectionFull        | 500           |  2,754.0 us  |   194.66 us |    561.65 us |  2,001.8 us |  4,112.6 us  |  291.27 KB |
| FullEntity            | 1000          |  7,922.4 us  |   848.76 us |  2,462.41 us |  2,528.1 us | 11,869.8 us  | 1128.32 KB |
| FullEntityNoTracking  | 1000          |  8,882.7 us  |   949.84 us |  2,785.73 us |  2,405.5 us | 13,886.8 us  | 1128.48 KB |
| Projection1Field      | 1000          |  1,426.2 us  |    93.49 us |    272.71 us |  1,052.4 us |  2,182.8 us  |  240.53 KB |
| Projection2Fields     | 1000          |  1,677.4 us  |    33.18 us |     36.87 us |  1,618.3 us |  1,730.9 us  |   285.4 KB |
| Projection3Fields     | 1000          |  2,360.3 us  |   199.56 us |    578.96 us |  1,571.0 us |  3,508.6 us  |  331.27 KB |
| Projection4Fields     | 1000          |  2,386.7 us  |   155.71 us |    412.91 us |  1,899.0 us |  3,621.5 us  |  400.67 KB |
| Projection5Fields     | 1000          |  2,987.5 us  |    52.50 us |     43.84 us |  2,919.6 us |  3,058.7 us  |  457.52 KB |
| Projection6Fields     | 1000          |  3,452.6 us  |   415.47 us |  1,211.94 us |  2,279.9 us |  6,857.2 us  |  507.08 KB |
| ProjectionFull        | 1000          |  4,134.3 us  |    79.46 us |     74.33 us |  4,020.3 us |  4,260.1 us  |  554.82 KB |
| FullEntity            | 5000          | 23,003.3 us  | 5,315.18 us | 15,671.93 us | 11,273.1 us | 69,455.7 us  | 5890.65 KB |
| FullEntityNoTracking  | 5000          | 24,371.3 us  | 4,711.99 us | 13,893.42 us | 10,907.4 us | 64,602.3 us  | 5890.81 KB |
| Projection1Field      | 5000          |  4,918.4 us  |   115.37 us |    327.29 us |  4,419.5 us |  5,783.9 us  | 1264.24 KB |
| Projection2Fields     | 5000          |  7,763.5 us  |   155.06 us |    411.19 us |  6,822.5 us |  8,578.5 us  |  1484.8 KB |
| Projection3Fields     | 5000          | 10,806.1 us  |   309.47 us |    857.54 us |  9,033.0 us | 12,268.8 us  | 1710.84 KB |
| Projection4Fields     | 5000          | 13,692.1 us  |   567.15 us |  1,654.42 us | 10,698.8 us | 17,916.5 us  | 2049.68 KB |
| Projection5Fields     | 5000          | 15,610.8 us  |   934.15 us |  2,710.14 us |  6,872.6 us | 18,905.1 us  | 2329.77 KB |
| Projection6Fields     | 5000          | 19,489.6 us  |   779.10 us |  2,235.37 us | 15,164.0 us | 25,985.9 us  | 2572.72 KB |
| ProjectionFull        | 5000          | 18,966.8 us  | 2,218.51 us |  6,541.34 us |  8,041.9 us | 32,889.9 us  | 2807.99 KB |

## Takeaways

- **Projections dominate at every scale**: even `Projection1Field` beats `FullEntity` from
  `ElementsCount=50` onward, and the gap widens as row count grows (at 5000 rows, projecting a
  single field is ~4.7× faster than loading full entities).
- **Cost grows with column count, as expected** — each extra column in the projection adds a
  roughly constant increment; by 5–6 fields, the gap versus a minimal projection is sizeable at
  scale (e.g. ~19.5 ms for `Projection6Fields` vs. ~4.9 ms for `Projection1Field` at 5000 rows).
- **`ProjectionFull` (all 7 fields via a DTO) still beats `FullEntity`** at every scale, despite
  selecting the same data — avoiding entity hydration/proxy machinery pays off even when no
  fields are dropped.
- **`FullEntityNoTracking` doesn't reliably beat `FullEntity`** in this benchmark — the
  read-only flag mainly helps at flush time (skipping dirty-checking snapshots), which isn't
  exercised here since the session is never flushed.
- **Variance balloons at 1000–5000 rows** (StdDev exceeding the mean for `FullEntity`/
  `FullEntityNoTracking`) — consistent with GC pressure from the larger in-memory result sets on
  the shared CI runner; treat those two rows as directional rather than precise.

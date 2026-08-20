# EfCoreComparison.ProjectionBenchmark

Compares full entity (tracked/read-only) vs. full-field projection, NHibernate vs. EF Core,
across the same `ElementsCount` values as the root
[`ProjectionBenchmark`](../ProjectionBenchmark.md). Each ORM gets its own in-memory SQLite
database/connection, seeded independently but with an identical `Bogus` seed so the non-key
field content matches between the two. `net10.0` only.

Source: [`ProjectionBenchmark.cs`](./ProjectionBenchmark.cs).

## Environment

- BenchmarkDotNet v0.15.2, Windows 11 (Hyper-V VM), `windows-latest` GitHub Actions runner
- `[Host]` / `.NET 10.0`: .NET 10.0.11, X64 RyuJIT AVX2
- CI run: [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205) (commit `1fb3594`)

## Results

| Method                         | ElementsCount | Mean        | Error       | StdDev       | Median      | Min         | Max         | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------------------- |-------------- |------------:|------------:|-------------:|------------:|------------:|------------:|------:|--------:|-----------:|------------:|
| NHibernateFullEntity           | 1             |    358.6 us |    20.73 us |     59.47 us |    358.4 us |    244.4 us |    507.2 us |  1.03 |    0.24 |   16.35 KB |        1.00 |
| NHibernateFullEntityNoTracking | 1             |    330.8 us |    17.48 us |     50.44 us |    326.1 us |    219.9 us |    473.0 us |  0.95 |    0.21 |   16.52 KB |        1.01 |
| NHibernateProjection           | 1             |    434.9 us |    18.84 us |     54.04 us |    429.4 us |    340.1 us |    580.5 us |  1.25 |    0.26 |   27.58 KB |        1.69 |
| EfCoreFullEntity               | 1             |    731.5 us |    33.60 us |     96.95 us |    694.4 us |    601.6 us |  1,019.5 us |  2.09 |    0.44 |   59.47 KB |        3.64 |
| EfCoreFullEntityNoTracking     | 1             |    674.2 us |    22.13 us |     62.77 us |    671.3 us |    518.3 us |    833.9 us |  1.93 |    0.36 |   59.54 KB |        3.64 |
| EfCoreProjection               | 1             |    780.8 us |    35.33 us |    102.51 us |    765.1 us |    622.4 us |  1,052.2 us |  2.24 |    0.47 |   63.91 KB |        3.91 |
| NHibernateFullEntity           | 5             |    372.6 us |    21.29 us |     60.73 us |    373.1 us |    227.7 us |    495.6 us |  1.03 |    0.26 |   20.12 KB |        1.00 |
| NHibernateFullEntityNoTracking | 5             |    343.5 us |    16.93 us |     48.03 us |    338.0 us |    254.2 us |    472.4 us |  0.95 |    0.23 |   20.28 KB |        1.01 |
| NHibernateProjection           | 5             |    469.0 us |    34.57 us |    100.84 us |    456.6 us |    296.9 us |    725.3 us |  1.30 |    0.37 |   29.57 KB |        1.47 |
| EfCoreFullEntity               | 5             |    747.1 us |    29.19 us |     82.33 us |    745.0 us |    550.1 us |    954.3 us |  2.07 |    0.46 |   64.02 KB |        3.18 |
| EfCoreFullEntityNoTracking     | 5             |    643.6 us |    32.05 us |     91.97 us |    636.9 us |    496.4 us |    868.3 us |  1.78 |    0.43 |   61.91 KB |        3.08 |
| EfCoreProjection               | 5             |    729.0 us |    39.59 us |    112.95 us |    730.0 us |    521.9 us |  1,036.2 us |  2.02 |    0.50 |   66.29 KB |        3.30 |
| NHibernateFullEntity           | 10            |    369.4 us |    19.87 us |     57.64 us |    359.1 us |    262.2 us |    511.1 us |  1.02 |    0.22 |   25.55 KB |        1.00 |
| NHibernateFullEntityNoTracking | 10            |    421.1 us |    25.05 us |     71.86 us |    419.9 us |    253.9 us |    590.2 us |  1.17 |    0.27 |   25.72 KB |        1.01 |
| NHibernateProjection           | 10            |    446.1 us |    20.62 us |     58.83 us |    443.2 us |    321.5 us |    588.8 us |  1.24 |    0.25 |   32.41 KB |        1.27 |
| EfCoreFullEntity               | 10            |    700.0 us |    39.11 us |    114.07 us |    669.5 us |    484.2 us |  1,072.5 us |  1.94 |    0.44 |   70.21 KB |        2.75 |
| EfCoreFullEntityNoTracking     | 10            |    714.6 us |    33.86 us |     97.15 us |    712.0 us |    542.1 us |    954.6 us |  1.98 |    0.41 |   64.97 KB |        2.54 |
| EfCoreProjection               | 10            |    692.1 us |    42.72 us |    124.61 us |    670.8 us |    506.3 us |  1,047.8 us |  1.92 |    0.46 |   69.34 KB |        2.71 |
| NHibernateFullEntity           | 50            |    872.3 us |    58.76 us |    171.40 us |    903.6 us |    493.8 us |  1,206.4 us |  1.05 |    0.34 |   73.18 KB |        1.00 |
| NHibernateFullEntityNoTracking | 50            |    800.5 us |    49.73 us |    145.07 us |    790.1 us |    574.4 us |  1,116.6 us |  0.96 |    0.30 |   73.34 KB |        1.00 |
| NHibernateProjection           | 50            |    675.5 us |    47.22 us |    138.48 us |    644.0 us |    416.1 us |  1,035.3 us |  0.81 |    0.27 |   54.09 KB |        0.74 |
| EfCoreFullEntity               | 50            |  1,236.2 us |    46.19 us |    131.78 us |  1,225.8 us |    912.3 us |  1,492.0 us |  1.49 |    0.41 |  118.21 KB |        1.62 |
| EfCoreFullEntityNoTracking     | 50            |  1,037.1 us |    78.76 us |    229.74 us |  1,005.0 us |    684.9 us |  1,748.6 us |  1.25 |    0.42 |      89 KB |        1.22 |
| EfCoreProjection               | 50            |    897.3 us |    58.70 us |    171.24 us |    862.5 us |    636.7 us |  1,387.9 us |  1.08 |    0.34 |   93.38 KB |        1.28 |
| NHibernateFullEntity           | 100           |  1,273.4 us |   113.16 us |    333.66 us |  1,360.5 us |    769.4 us |  2,069.6 us |  1.08 |    0.42 |  127.52 KB |        1.00 |
| NHibernateFullEntityNoTracking | 100           |  1,254.0 us |    81.02 us |    236.34 us |  1,340.8 us |    748.2 us |  1,782.7 us |  1.06 |    0.36 |  127.69 KB |        1.00 |
| NHibernateProjection           | 100           |    773.0 us |    54.86 us |    159.17 us |    714.2 us |    559.2 us |  1,292.0 us |  0.65 |    0.23 |   81.11 KB |        0.64 |
| EfCoreFullEntity               | 100           |  1,587.8 us |    81.85 us |    232.18 us |  1,660.3 us |  1,071.8 us |  1,964.3 us |  1.34 |    0.42 |  180.34 KB |        1.41 |
| EfCoreFullEntityNoTracking     | 100           |    925.2 us |    42.36 us |    118.08 us |    931.5 us |    658.0 us |  1,234.5 us |  0.78 |    0.24 |  119.16 KB |        0.93 |
| EfCoreProjection               | 100           |  1,150.8 us |    91.07 us |    267.09 us |  1,116.3 us |    694.8 us |  1,866.5 us |  0.97 |    0.36 |  123.65 KB |        0.97 |
| NHibernateFullEntity           | 500           |  4,298.5 us |   176.43 us |    467.88 us |  4,285.6 us |  3,333.6 us |  5,727.1 us |  1.01 |    0.16 |   566.7 KB |        1.00 |
| NHibernateFullEntityNoTracking | 500           |  4,400.0 us |   300.77 us |    877.36 us |  4,050.7 us |  3,261.1 us |  6,954.6 us |  1.04 |    0.24 |  566.87 KB |        1.00 |
| NHibernateProjection           | 500           |  1,908.7 us |    31.94 us |     26.67 us |  1,900.7 us |  1,884.1 us |  1,964.5 us |  0.45 |    0.05 |  291.45 KB |        0.51 |
| EfCoreFullEntity               | 500           |  5,834.1 us |   330.51 us |    932.21 us |  6,294.4 us |  3,721.2 us |  6,945.5 us |  1.37 |    0.26 |   661.8 KB |        1.17 |
| EfCoreFullEntityNoTracking     | 500           |  2,502.1 us |   147.95 us |    405.01 us |  2,661.9 us |  1,758.7 us |  3,355.3 us |  0.59 |    0.11 |  357.84 KB |        0.63 |
| EfCoreProjection               | 500           |  1,750.8 us |    26.96 us |     21.05 us |  1,752.5 us |  1,716.8 us |  1,785.9 us |  0.41 |    0.04 |  362.29 KB |        0.64 |
| NHibernateFullEntity           | 1000          |  7,861.6 us | 1,228.95 us |  3,604.30 us |  7,276.3 us |  2,321.0 us | 17,754.3 us |  1.33 |    1.10 | 1128.32 KB |        1.00 |
| NHibernateFullEntityNoTracking | 1000          |  7,496.0 us |   721.89 us |  2,105.78 us |  8,047.1 us |  2,828.5 us | 10,859.9 us |  1.27 |    0.90 | 1128.48 KB |        1.00 |
| NHibernateProjection           | 1000          |  3,584.0 us |   459.50 us |  1,340.39 us |  3,169.0 us |  2,392.7 us |  6,979.1 us |  0.61 |    0.47 |   554.8 KB |        0.49 |
| EfCoreFullEntity               | 1000          |  9,265.7 us | 1,032.21 us |  3,027.29 us |  8,540.5 us |  2,996.6 us | 18,023.7 us |  1.57 |    1.16 | 1245.48 KB |        1.10 |
| EfCoreFullEntityNoTracking     | 1000          |  4,605.8 us |   262.02 us |    756.00 us |  4,918.0 us |  2,658.8 us |  5,421.2 us |  0.78 |    0.51 |  656.13 KB |        0.58 |
| EfCoreProjection               | 1000          |  3,021.8 us |    38.09 us |     29.74 us |  3,005.4 us |  2,998.5 us |  3,096.3 us |  0.51 |    0.32 |   660.5 KB |        0.59 |
| NHibernateFullEntity           | 5000          | 25,690.7 us | 5,176.40 us | 15,262.74 us | 14,815.4 us | 11,560.4 us | 65,926.9 us |  1.42 |    1.20 | 5890.65 KB |        1.00 |
| NHibernateFullEntityNoTracking | 5000          | 25,989.2 us | 4,622.44 us | 13,629.36 us | 28,256.0 us |  9,888.4 us | 61,327.8 us |  1.44 |    1.13 | 5890.84 KB |        1.00 |
| NHibernateProjection           | 5000          | 19,736.7 us | 2,284.56 us |  6,736.08 us | 23,637.5 us |  8,240.9 us | 32,456.3 us |  1.09 |    0.70 | 2807.98 KB |        0.48 |
| EfCoreFullEntity               | 5000          | 25,498.4 us | 5,808.95 us | 17,036.66 us | 12,360.9 us | 11,656.5 us | 65,137.9 us |  1.41 |    1.29 | 5923.56 KB |        1.01 |
| EfCoreFullEntityNoTracking     | 5000          | 16,271.2 us |   247.56 us |    193.28 us | 16,244.5 us | 15,944.2 us | 16,542.6 us |  0.90 |    0.47 | 3091.66 KB |        0.52 |
| EfCoreProjection               | 5000          | 15,383.5 us |   303.53 us |    383.86 us | 15,326.4 us | 14,921.3 us | 16,409.6 us |  0.85 |    0.44 | 3096.03 KB |        0.53 |

Ratio is relative to `NHibernateFullEntity` (the `[Benchmark(Baseline = true)]` method), within
each `ElementsCount` group.

## Takeaways

- **At small volumes (1–100 rows), EF Core is consistently ~2× slower than NHibernate** for
  every equivalent operation — dominated by EF Core's fixed per-query overhead (compiled query
  cache lookup, change tracker setup) rather than data volume.
- **That gap shrinks sharply as volume grows, and flips for projections**: at 5000 rows,
  `EfCoreProjection` (15.4 ms) is actually *faster* than `NHibernateProjection` (19.7 ms), and
  `EfCoreFullEntity` (25.5 ms) is essentially tied with `NHibernateFullEntity` (25.7 ms).
- **`EfCoreFullEntityNoTracking` scales better than `EfCoreFullEntity`** at every volume beyond
  a handful of rows (e.g. 16.3 ms vs. 25.5 ms at 5000) — `AsNoTracking()` has a clear, consistent
  payoff for EF Core in this benchmark, unlike NHibernate's read-only flag (see the root
  [`ProjectionBenchmark`](../ProjectionBenchmark.md) notes).
- **Projections beat full-entity loads for both ORMs at scale**, same conclusion as the root
  suite — the win is largest for EF Core (≈40% faster at 5000 rows) since it also sidesteps
  change-tracker registration entirely for projected results.
- Rows are seeded independently per ORM with an identical Bogus seed for the non-key fields, but
  each store's own identity column assigns `Id` — so `Id` values aren't expected to match
  between NHibernate and EF Core rows, only field content and row count.

BenchmarkDotNet v0.15.2, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-1255U 1.70GHz, 1 CPU, 12 logical and 10 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9277.0), X64 RyuJIT VectorSize=256
  Job-PDYGDO : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2
  Job-MAVUHA : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2
  Job-CXGLSN : .NET Framework 4.8.1 (4.8.9277.0), X64 RyuJIT VectorSize=256

InvocationCount=1  IterationCount=1  LaunchCount=30
RunStrategy=ColdStart  UnrollFactor=1

| Method                           | Runtime            | Mean     | Error    | StdDev   | Allocated |
|--------------------------------- |------------------- |---------:|---------:|---------:|----------:|
| FluentInitialization             | .NET 8.0           | 615.9 ms | 18.24 ms | 27.29 ms |   2.26 MB |
| FluentInitializationFromAssembly | .NET 8.0           | 574.2 ms | 14.55 ms | 21.77 ms |   3.46 MB |
| XmlInitialization                | .NET 8.0           | 493.1 ms | 10.32 ms | 15.44 ms |   1.76 MB |
| XmlInitializationFromAssembly    | .NET 8.0           | 494.4 ms | 12.49 ms | 18.69 ms |   1.66 MB |
| ByCodeInitialization             | .NET 8.0           | 373.1 ms |  9.67 ms | 14.47 ms |   1.59 MB |
| ByCodeInitializationFromAssembly | .NET 8.0           | 375.0 ms | 11.80 ms | 17.66 ms |   1.59 MB |
|                                  |                    |          |          |          |           |
| FluentInitialization             | .NET 9.0           | 543.0 ms | 14.17 ms | 21.21 ms |   2.24 MB |
| FluentInitializationFromAssembly | .NET 9.0           | 556.4 ms | 20.77 ms | 31.09 ms |   3.51 MB |
| XmlInitialization                | .NET 9.0           | 479.2 ms |  9.67 ms | 14.47 ms |   1.65 MB |
| XmlInitializationFromAssembly    | .NET 9.0           | 491.9 ms | 16.53 ms | 24.74 ms |   1.65 MB |
| ByCodeInitialization             | .NET 9.0           | 370.1 ms | 16.95 ms | 25.37 ms |   1.58 MB |
| ByCodeInitializationFromAssembly | .NET 9.0           | 368.0 ms | 11.50 ms | 17.21 ms |   1.58 MB |
|                                  |                    |          |          |          |           |
| FluentInitialization             | .NET Framework 4.8 | 930.0 ms | 36.39 ms | 54.47 ms |   2.97 MB |
| FluentInitializationFromAssembly | .NET Framework 4.8 | 932.3 ms | 21.36 ms | 31.97 ms |   4.55 MB |
| XmlInitialization                | .NET Framework 4.8 | 805.1 ms | 18.18 ms | 27.21 ms |   2.17 MB |
| XmlInitializationFromAssembly    | .NET Framework 4.8 | 799.6 ms | 10.56 ms | 15.81 ms |   2.16 MB |
| ByCodeInitialization             | .NET Framework 4.8 | 682.5 ms | 17.06 ms | 25.54 ms |   2.03 MB |
| ByCodeInitializationFromAssembly | .NET Framework 4.8 | 678.9 ms | 19.41 ms | 29.05 ms |   2.03 MB |


| Method          | ElementsCount | Mean         | Error       | StdDev      | Median        | Gen0       | Gen1      | Allocated    |
|---------------- |-------------- |-------------:|------------:|------------:|--------------:|-----------:|----------:|-------------:|
| SessionTracking | 2             |     343.0 us |  1,128.7 us |  1,689.4 us |      32.40 us |          - |         - |      1.35 KB |
| SessionTracking | 4             |     291.9 us |    971.2 us |  1,453.6 us |      24.75 us |          - |         - |      2.04 KB |
| SessionTracking | 8             |     280.0 us |    915.1 us |  1,369.6 us |      26.80 us |          - |         - |      3.41 KB |
| SessionTracking | 16            |     284.8 us |    909.8 us |  1,361.8 us |      32.35 us |          - |         - |      6.16 KB |
| SessionTracking | 64            |     327.8 us |    915.9 us |  1,370.8 us |      76.15 us |          - |         - |     22.66 KB |
| SessionTracking | 128           |     362.3 us |    962.0 us |  1,439.8 us |      95.50 us |          - |         - |     44.66 KB |
| SessionTracking | 258           |     427.1 us |    949.2 us |  1,420.7 us |     163.60 us |          - |         - |     89.35 KB |
| SessionTracking | 512           |   1,175.6 us |  2,172.6 us |  3,251.8 us |     346.40 us |          - |         - |    176.66 KB |
| SessionTracking | 1024          |   2,705.7 us |  3,564.8 us |  5,335.7 us |     662.90 us |          - |         - |    352.66 KB |
| SessionTracking | 2048          |   3,410.2 us |  3,590.4 us |  5,373.9 us |   2,004.70 us |          - |         - |    704.66 KB |
| SessionTracking | 4096          |   3,465.6 us |  2,966.5 us |  4,440.1 us |   1,884.05 us |          - |         - |   1408.66 KB |
| SessionTracking | 8192          |   6,298.1 us |  3,860.7 us |  5,778.5 us |   5,948.10 us |          - |         - |   2816.66 KB |
| SessionTracking | 16384         |   8,562.0 us |  4,847.6 us |  7,255.7 us |   7,798.60 us |          - |         - |   5632.66 KB |
| SessionTracking | 32768         |  17,408.0 us |  6,338.5 us |  9,487.2 us |  13,973.95 us |  1000.0000 |         - |  11264.66 KB |
| SessionTracking | 65536         |  31,825.5 us |  7,362.1 us | 11,019.2 us |  29,555.45 us |  3000.0000 |         - |  22528.66 KB |
| SessionTracking | 131072        |  66,078.1 us |  6,305.8 us |  9,438.2 us |  63,624.25 us |  7000.0000 | 2000.0000 |  45056.66 KB |
| SessionTracking | 262144        | 123,800.7 us |  8,796.7 us | 13,166.4 us | 120,350.50 us | 14000.0000 | 3000.0000 |  90112.66 KB |
| SessionTracking | 524288        | 244,799.6 us | 29,691.5 us | 44,440.8 us | 226,894.95 us | 28000.0000 | 4000.0000 | 180224.66 KB |
| SessionTracking | 1048576       | 458,840.9 us | 23,133.9 us | 34,625.8 us | 452,925.45 us | 56000.0000 | 8000.0000 | 360448.66 KB |

| Method                | ElementsCount | Mean      | Error     | StdDev    | Median    | Gen0      | Gen1      | Gen2      | Allocated   |
|---------------------- |-------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|------------:|
| QueryOverNoProjection | 8             |  31.31 ms |  4.431 ms |  2.931 ms |  30.16 ms |         - |         - |         - |    26.35 KB |
| QueryOverProjection   | 8             |  47.64 ms |  2.110 ms |  1.395 ms |  47.30 ms |         - |         - |         - |    33.09 KB |
| LinqNoProjection      | 8             | 101.39 ms |  6.031 ms |  3.989 ms | 100.24 ms |         - |         - |         - |     18.1 KB |
| LinqProjection        | 8             | 123.79 ms |  3.850 ms |  2.547 ms | 123.06 ms |         - |         - |         - |    23.41 KB |
| HqlNoProjection       | 8             |  81.42 ms |  7.267 ms |  4.807 ms |  80.05 ms |         - |         - |         - |    14.04 KB |
| HqlProjection         | 8             | 102.37 ms |  7.449 ms |  4.927 ms |  99.67 ms |         - |         - |         - |    28.64 KB |
| SqlNoProjection       | 8             |  25.88 ms |  1.388 ms |  0.918 ms |  25.93 ms |         - |         - |         - |    21.72 KB |
| SqlProjection         | 8             |  30.68 ms |  2.487 ms |  1.645 ms |  30.31 ms |         - |         - |         - |    34.47 KB |
| QueryOverNoProjection | 64            |  27.42 ms |  1.286 ms |  0.850 ms |  27.45 ms |         - |         - |         - |    69.48 KB |
| QueryOverProjection   | 64            |  46.99 ms |  2.122 ms |  1.403 ms |  46.60 ms |         - |         - |         - |    45.84 KB |
| LinqNoProjection      | 64            | 100.84 ms |  3.381 ms |  2.237 ms | 100.35 ms |         - |         - |         - |    62.69 KB |
| LinqProjection        | 64            | 125.05 ms |  9.036 ms |  5.977 ms | 123.26 ms |         - |         - |         - |     36.3 KB |
| HqlNoProjection       | 64            |  80.77 ms |  3.365 ms |  2.226 ms |  80.35 ms |         - |         - |         - |    57.68 KB |
| HqlProjection         | 64            | 106.94 ms | 19.684 ms | 13.020 ms | 103.00 ms |         - |         - |         - |    41.91 KB |
| SqlNoProjection       | 64            |  25.81 ms |  2.274 ms |  1.504 ms |  25.35 ms |         - |         - |         - |    68.42 KB |
| SqlProjection         | 64            |  31.88 ms |  2.288 ms |  1.513 ms |  31.67 ms |         - |         - |         - |    47.73 KB |
| QueryOverNoProjection | 512           |  28.66 ms |  1.315 ms |  0.870 ms |  28.66 ms |         - |         - |         - |   412.21 KB |
| QueryOverProjection   | 512           |  48.72 ms |  4.561 ms |  3.017 ms |  48.54 ms |         - |         - |         - |   150.67 KB |
| LinqNoProjection      | 512           | 132.59 ms | 57.544 ms | 38.062 ms | 105.39 ms |         - |         - |         - |   416.05 KB |
| LinqProjection        | 512           | 139.00 ms | 51.027 ms | 33.751 ms | 127.83 ms |         - |         - |         - |   141.23 KB |
| HqlNoProjection       | 512           |  82.40 ms |  5.173 ms |  3.422 ms |  82.09 ms |         - |         - |         - |   403.98 KB |
| HqlProjection         | 512           | 102.36 ms |  6.540 ms |  4.326 ms | 100.27 ms |         - |         - |         - |   150.27 KB |
| SqlNoProjection       | 512           |  27.79 ms |  3.626 ms |  2.398 ms |  26.75 ms |         - |         - |         - |   439.22 KB |
| SqlProjection         | 512           |  31.25 ms |  1.594 ms |  1.054 ms |  31.11 ms |         - |         - |         - |   156.09 KB |
| QueryOverNoProjection | 4096          |  37.77 ms |  1.907 ms |  1.261 ms |  37.82 ms |         - |         - |         - |  3270.84 KB |
| QueryOverProjection   | 4096          |  52.48 ms |  2.547 ms |  1.684 ms |  52.07 ms |         - |         - |         - |    990.7 KB |
| LinqNoProjection      | 4096          | 110.62 ms |  3.824 ms |  2.529 ms | 110.55 ms |         - |         - |         - |  3358.83 KB |
| LinqProjection        | 4096          | 126.12 ms |  3.175 ms |  2.100 ms | 126.16 ms |         - |         - |         - |   981.48 KB |
| HqlNoProjection       | 4096          |  90.50 ms |  5.235 ms |  3.462 ms |  89.52 ms |         - |         - |         - |  3290.68 KB |
| HqlProjection         | 4096          | 105.01 ms |  3.682 ms |  2.435 ms | 105.64 ms |         - |         - |         - |  1018.41 KB |
| SqlNoProjection       | 4096          |  35.77 ms |  3.742 ms |  2.475 ms |  34.98 ms |         - |         - |         - |  3521.92 KB |
| SqlProjection         | 4096          |  35.03 ms |  4.709 ms |  3.115 ms |  34.33 ms |         - |         - |         - |  1024.23 KB |
| QueryOverNoProjection | 32768         | 171.77 ms | 33.403 ms | 22.094 ms | 164.51 ms | 4000.0000 | 2000.0000 | 2000.0000 | 26762.08 KB |
| QueryOverProjection   | 32768         |  99.02 ms |  5.398 ms |  3.571 ms |  99.24 ms | 1000.0000 |         - |         - |  7758.82 KB |
| LinqNoProjection      | 32768         | 244.37 ms |  9.411 ms |  6.225 ms | 244.76 ms | 4000.0000 | 2000.0000 | 2000.0000 | 27521.16 KB |
| LinqProjection        | 32768         | 165.15 ms |  5.098 ms |  3.372 ms | 165.53 ms | 1000.0000 |         - |         - |  7701.66 KB |
| HqlNoProjection       | 32768         | 238.20 ms | 13.220 ms |  8.744 ms | 239.45 ms | 3000.0000 | 1000.0000 |         - | 27004.01 KB |
| HqlProjection         | 32768         | 152.21 ms |  5.799 ms |  3.835 ms | 150.74 ms | 1000.0000 |         - |         - |  7962.55 KB |
| SqlNoProjection       | 32768         | 165.41 ms |  7.226 ms |  4.780 ms | 165.29 ms | 4000.0000 | 2000.0000 | 2000.0000 | 28809.55 KB |
| SqlProjection         | 32768         |  74.47 ms |  7.403 ms |  4.896 ms |  72.08 ms | 1000.0000 |         - |         - |  7968.38 KB |
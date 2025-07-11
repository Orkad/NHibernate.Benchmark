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

| Method                | ElementsCount | Mean         | Error        | StdDev       | Min         | Max          | Median       | Gen0      | Gen1      | Gen2      | Allocated   |
|---------------------- |-------------- |-------------:|-------------:|-------------:|------------:|-------------:|-------------:|----------:|----------:|----------:|------------:|
| QueryOverNoProjection | 3             |     352.3 us |     77.49 us |     51.25 us |    259.8 us |     416.6 us |     353.5 us |         - |         - |         - |    31.04 KB |
| QueryOverProjection   | 3             |     328.9 us |     83.39 us |     55.16 us |    231.7 us |     418.4 us |     341.8 us |         - |         - |         - |    38.85 KB |
| LinqNoProjection      | 3             |     262.5 us |     70.91 us |     46.90 us |    195.0 us |     337.1 us |     255.1 us |         - |         - |         - |    18.13 KB |
| LinqProjection        | 3             |     347.2 us |     96.74 us |     63.99 us |    244.3 us |     424.6 us |     362.6 us |         - |         - |         - |    23.33 KB |
| HqlNoProjection       | 3             |     264.9 us |     40.03 us |     23.82 us |    229.2 us |     294.2 us |     263.6 us |         - |         - |         - |    14.15 KB |
| HqlProjection         | 3             |     623.7 us |     80.25 us |     53.08 us |    528.8 us |     715.8 us |     612.2 us |         - |         - |         - |    35.26 KB |
| SqlNoProjection       | 3             |     249.9 us |     89.10 us |     53.02 us |    191.0 us |     346.6 us |     242.0 us |         - |         - |         - |    20.25 KB |
| SqlProjection         | 3             |     535.3 us |     76.20 us |     50.40 us |    479.1 us |     626.9 us |     520.1 us |         - |         - |         - |    42.65 KB |
| QueryOverNoProjection | 30            |     343.6 us |     32.49 us |     21.49 us |    311.7 us |     383.9 us |     336.8 us |         - |         - |         - |    59.85 KB |
| QueryOverProjection   | 30            |     355.7 us |     77.39 us |     51.19 us |    265.9 us |     427.9 us |     352.7 us |         - |         - |         - |    46.05 KB |
| LinqNoProjection      | 30            |     321.4 us |     67.64 us |     44.74 us |    275.2 us |     423.5 us |     315.2 us |         - |         - |         - |    47.74 KB |
| LinqProjection        | 30            |     308.4 us |     44.44 us |     26.45 us |    263.4 us |     341.2 us |     318.6 us |         - |         - |         - |    31.02 KB |
| HqlNoProjection       | 30            |     312.9 us |     53.00 us |     35.06 us |    266.9 us |     382.9 us |     313.1 us |         - |         - |         - |    43.26 KB |
| HqlProjection         | 30            |     708.8 us |     88.65 us |     58.64 us |    636.3 us |     796.3 us |     696.3 us |         - |         - |         - |    43.07 KB |
| SqlNoProjection       | 30            |     286.8 us |     22.64 us |     14.97 us |    266.1 us |     312.9 us |     286.0 us |         - |         - |         - |    50.84 KB |
| SqlProjection         | 30            |     573.7 us |     58.19 us |     38.49 us |    501.2 us |     626.2 us |     574.9 us |         - |         - |         - |    50.46 KB |
| QueryOverNoProjection | 300           |   1,409.8 us |     50.70 us |     33.54 us |  1,372.7 us |   1,461.2 us |   1,403.2 us |         - |         - |         - |   357.66 KB |
| QueryOverProjection   | 300           |     782.1 us |     85.67 us |     56.66 us |    685.1 us |     883.7 us |     778.1 us |         - |         - |         - |   123.99 KB |
| LinqNoProjection      | 300           |   1,468.7 us |    149.52 us |     98.90 us |  1,338.6 us |   1,667.7 us |   1,459.0 us |         - |         - |         - |   358.63 KB |
| LinqProjection        | 300           |     555.0 us |     71.07 us |     42.29 us |    485.5 us |     633.1 us |     545.1 us |         - |         - |         - |    115.7 KB |
| HqlNoProjection       | 300           |   1,385.4 us |    115.89 us |     68.97 us |  1,310.7 us |   1,541.8 us |   1,364.2 us |         - |         - |         - |   346.55 KB |
| HqlProjection         | 300           |   1,173.2 us |     49.53 us |     29.48 us |  1,136.5 us |   1,231.8 us |   1,169.5 us |         - |         - |         - |   126.49 KB |
| SqlNoProjection       | 300           |   1,684.7 us |     73.51 us |     48.62 us |  1,620.1 us |   1,761.8 us |   1,681.5 us |         - |         - |         - |   368.89 KB |
| SqlProjection         | 300           |     967.6 us |     98.57 us |     65.20 us |    908.8 us |   1,112.8 us |     951.2 us |         - |         - |         - |   133.88 KB |
| QueryOverNoProjection | 3000          |  13,688.7 us |    855.72 us |    447.56 us | 13,107.0 us |  14,457.7 us |  13,541.2 us |         - |         - |         - |  3322.45 KB |
| QueryOverProjection   | 3000          |   4,578.0 us |    159.36 us |    105.41 us |  4,450.5 us |   4,755.0 us |   4,569.3 us |         - |         - |         - |   884.57 KB |
| LinqNoProjection      | 3000          |  15,953.1 us |  3,786.76 us |  2,253.44 us | 13,484.4 us |  20,057.3 us |  16,704.7 us |         - |         - |         - |  3414.47 KB |
| LinqProjection        | 3000          |   3,048.0 us |    178.38 us |    117.99 us |  2,884.1 us |   3,209.2 us |   3,045.4 us |         - |         - |         - |   903.97 KB |
| HqlNoProjection       | 3000          |  12,460.4 us |    437.85 us |    260.56 us | 12,211.0 us |  12,864.0 us |  12,363.6 us |         - |         - |         - |  3346.32 KB |
| HqlProjection         | 3000          |   4,298.8 us |    233.53 us |    154.47 us |  4,126.2 us |   4,563.6 us |   4,284.5 us |         - |         - |         - |   921.97 KB |
| SqlNoProjection       | 3000          |  12,599.6 us |  2,232.82 us |  1,328.71 us | 11,423.5 us |  15,261.0 us |  12,236.4 us |         - |         - |         - |  3516.32 KB |
| SqlProjection         | 3000          |   4,078.3 us |    215.01 us |    142.22 us |  3,908.8 us |   4,337.4 us |   4,077.0 us |         - |         - |         - |   929.36 KB |
| QueryOverNoProjection | 30000         | 101,721.4 us |  6,148.75 us |  3,215.91 us | 97,256.3 us | 107,020.3 us | 100,869.8 us | 5000.0000 | 3000.0000 | 1000.0000 | 32252.89 KB |
| QueryOverProjection   | 30000         |  37,953.9 us |  2,210.75 us |  1,462.27 us | 35,520.1 us |  40,744.7 us |  37,848.1 us | 1000.0000 |         - |         - |  8373.59 KB |
| LinqNoProjection      | 30000         | 121,292.6 us | 44,709.77 us | 29,572.74 us | 94,743.4 us | 176,979.4 us | 109,202.1 us | 5000.0000 | 3000.0000 | 1000.0000 | 33030.14 KB |
| LinqProjection        | 30000         |  24,685.4 us |    831.59 us |    494.86 us | 24,310.2 us |  25,809.9 us |  24,415.1 us | 1000.0000 |         - |         - |  8445.46 KB |
| HqlNoProjection       | 30000         | 120,288.3 us | 37,226.47 us | 24,623.00 us | 99,698.4 us | 171,160.1 us | 112,129.1 us | 5000.0000 | 3000.0000 | 1000.0000 | 32513.96 KB |
| HqlProjection         | 30000         |  40,731.3 us | 13,094.50 us |  8,661.20 us | 30,998.8 us |  50,774.9 us |  39,376.1 us | 1000.0000 |         - |         - |   8648.2 KB |
| SqlNoProjection       | 30000         |  99,713.4 us |  6,197.59 us |  3,241.46 us | 92,783.8 us | 102,550.6 us | 100,397.7 us | 5000.0000 | 3000.0000 | 1000.0000 | 34160.45 KB |
| SqlProjection         | 30000         |  39,265.0 us | 10,700.97 us |  7,078.03 us | 29,838.4 us |  46,340.5 us |  40,857.5 us | 1000.0000 |         - |         - |  8655.59 KB |
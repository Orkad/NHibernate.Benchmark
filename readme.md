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
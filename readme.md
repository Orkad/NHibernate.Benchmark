BenchmarkDotNet v0.15.2, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-1255U 1.70GHz, 1 CPU, 12 logical and 10 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9277.0), X64 RyuJIT VectorSize=256
  Job-PYFIWH : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2
  Job-CTMNKH : .NET Framework 4.8.1 (4.8.9277.0), X64 RyuJIT VectorSize=256

InvocationCount=1  IterationCount=1  LaunchCount=10
RunStrategy=ColdStart  UnrollFactor=1

| Method                           | Runtime            | Mean     | Error    | StdDev   | Allocated |
|--------------------------------- |------------------- |---------:|---------:|---------:|----------:|
| FluentInitialization             | .NET 8.0           | 532.4 ms | 34.04 ms | 22.52 ms |   2.25 MB |
| FluentInitializationFromAssembly | .NET 8.0           | 485.9 ms | 23.37 ms | 15.46 ms |   3.46 MB |
| XmlInitialization                | .NET 8.0           | 424.2 ms | 18.45 ms | 12.20 ms |   1.66 MB |
| XmlInitializationFromAssembly    | .NET 8.0           | 432.6 ms | 47.21 ms | 31.23 ms |   1.66 MB |
| ByCodeInitialization             | .NET 8.0           | 309.9 ms | 35.02 ms | 23.16 ms |   1.55 MB |
| ByCodeInitializationFromAssembly | .NET 8.0           | 322.8 ms | 51.42 ms | 34.01 ms |   1.55 MB |
|                                  |                    |          |          |          |           |
| FluentInitialization             | .NET Framework 4.8 | 898.3 ms | 29.31 ms | 19.39 ms |   2.97 MB |
| FluentInitializationFromAssembly | .NET Framework 4.8 | 897.8 ms | 70.98 ms | 46.95 ms |   4.55 MB |
| XmlInitialization                | .NET Framework 4.8 | 771.8 ms | 41.03 ms | 27.14 ms |   2.16 MB |
| XmlInitializationFromAssembly    | .NET Framework 4.8 | 766.4 ms | 32.20 ms | 21.30 ms |   2.15 MB |
| ByCodeInitialization             | .NET Framework 4.8 | 624.4 ms | 10.24 ms |  6.77 ms |   2.03 MB |
| ByCodeInitializationFromAssembly | .NET Framework 4.8 | 633.2 ms | 25.34 ms | 16.76 ms |   2.03 MB |
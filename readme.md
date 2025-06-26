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

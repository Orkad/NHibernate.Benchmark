using BenchmarkDotNet.Running;
using NHibernate.Benchmark.Benchmarks;

//new InitializationBenchmark().XmlInitializationFromAssembly();

BenchmarkRunner.Run<InitializationBenchmark>();
using BenchmarkDotNet.Running;
using NHibernate.Benchmark.Benchmarks;
using NHibernate.Benchmark.Helpers;

BenchmarkRunner.Run<InitializationBenchmark>();
using BenchmarkDotNet.Running;
using NHibernate.Benchmark.Benchmarks;
using NHibernate.Benchmark.Helpers;

//new InitializationBenchmark()
//{
//    Strategy = MappingStrategy.ByCode // Change to Xml or Fluent to test other strategies
//}.CreateSessionFactory(); // Initialize the benchmark

BenchmarkRunner.Run<InitializationBenchmark>();
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using NHibernate.Benchmark.AuthorWork.Mappings.ByCode;
using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Data.Common;
using System.Reflection;

namespace NHibernate.Benchmark;

[SimpleJob(
    RunStrategy.Monitoring,
    runtimeMoniker: RuntimeMoniker.Net80,
    iterationCount: 30)]
[MemoryDiagnoser]
public class TrackingBenchmark
{
    private static readonly Assembly assembly = typeof(Person).Assembly;

    private ISessionFactory sessionFactory;
    private DbConnection connection;
    private ISession session;

    [Params(
        2, 4, 8, 16, 64, 128, 258, 512, 1024, 2048, 4096, 8192, 16384, 
        32768, 65536, 131072, 262144, 524288, 1048576)]
    public int ElementsCount { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var cfg = new Configuration();
        cfg.DataBaseIntegration(db =>
        {
            db.Dialect<Dialect.SQLiteDialect>();
            db.Driver<Driver.SQLite20Driver>();
            db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
            db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
            db.LogSqlInConsole = false;
        });
        var mapper = new ModelMapper();
        mapper.AddMapping<PersonMapping>();
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        sessionFactory = cfg.BuildSessionFactory();
        connection = sessionFactory.OpenSession().Connection;
        new SchemaExport(cfg).Create(false, true, connection);
        using var statelessSession = sessionFactory.OpenStatelessSession(connection);
        for (int i = 0; i < ElementsCount; i++)
        {
            var person = new Person { Id = i, Name = $"Person {i}" };
            statelessSession.Insert(person);
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        session = sessionFactory.WithOptions().Connection(connection).OpenSession();
        _ = session.Query<Person>().Take(ElementsCount).ToList();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        session.Dispose();
        session = null;
    }

    [Benchmark]
    public void SessionTracking()
    {
        session.Flush();
    }
}

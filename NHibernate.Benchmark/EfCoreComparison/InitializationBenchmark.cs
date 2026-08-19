using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using FluentNHibernate.Cfg;
using NHibernate.Benchmark.AuthorWork.Mappings.ByCode;
using NHibernate.Benchmark.AuthorWork.Mappings.Fluent;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using EfCore = NHibernate.Benchmark.AuthorWork.EfCore;
using NHPerson = NHibernate.Benchmark.AuthorWork.Models.Person;

namespace NHibernate.Benchmark.EfCoreComparison;

// Compares session-factory build cost (NHibernate, Fluent mapping as the baseline, ByCode
// kept alongside for reference) against EF Core's first-use model build cost, all against a
// fresh SQLite in-memory schema. net10.0-only: EF Core's current packages don't support
// net48. See ../InitializationBenchmark.cs for the single-ORM comparison across mapping
// styles this suite doesn't repeat.
[SimpleJob(
    RunStrategy.ColdStart,
    runtimeMoniker: RuntimeMoniker.Net10_0,
    launchCount: 30,
    iterationCount: 1,
    invocationCount: 1)]
[MemoryDiagnoser]
public class InitializationBenchmark
{
    private static Configuration CreateBaseConfiguration()
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
        return cfg;
    }

    private static ISessionFactory UseConfiguration(Configuration cfg)
    {
        var sessionFactory = cfg.BuildSessionFactory();
        using var session = sessionFactory.OpenSession();
        new SchemaExport(cfg).Create(false, true, session.Connection);
        _ = session.Get<NHPerson>(1);
        return sessionFactory;
    }

    [Benchmark(Baseline = true)]
    public ISessionFactory NHibernateFluentInitialization()
    {
        var cfg = Fluently.Configure(CreateBaseConfiguration())
            .Mappings(m => m.FluentMappings.Add<PersonMap>())
            .BuildConfiguration();
        return UseConfiguration(cfg);
    }

    [Benchmark]
    public ISessionFactory NHibernateByCodeInitialization()
    {
        var cfg = CreateBaseConfiguration();
        var mapper = new ModelMapper();
        mapper.AddMapping<PersonMapping>();
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return UseConfiguration(cfg);
    }

    [Benchmark]
    public EfCore.PersonContext EfCoreInitialization()
    {
        var connection = EfCore.PersonContext.CreateOpenInMemoryConnection();
        var context = new EfCore.PersonContext(connection);
        context.Database.EnsureCreated();
        _ = context.People.Find(1);
        return context;
    }
}

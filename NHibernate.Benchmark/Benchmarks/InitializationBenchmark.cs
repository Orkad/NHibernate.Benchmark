using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Benchmark.Mappings.ByCode;
using NHibernate.Benchmark.Mappings.Fluent;
using NHibernate.Benchmark.Models;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace NHibernate.Benchmark.Benchmarks;


[SimpleJob(RunStrategy.ColdStart, launchCount: 3, iterationCount: 1)]
[MemoryDiagnoser]
public class InitializationBenchmark
{
    private static readonly Assembly assembly = typeof(Person).Assembly;
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

    private static ISessionFactory UseConfiguration(Configuration configuration)
    {
        var sf = configuration.BuildSessionFactory();
        using var session = sf.OpenSession();
        new SchemaExport(configuration).Create(false, true, session.Connection);
        _ = session.Get<Person>(1);
        return sf;
    }

    [Benchmark]
    public ISessionFactory FluentInitialization()
    {
        var cfg = CreateBaseConfiguration();
        cfg = Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.Add<PersonMap>())
            .BuildConfiguration();
        return UseConfiguration(cfg);
    }

    [Benchmark(Baseline = true)]
    public ISessionFactory FluentInitializationFromAssembly()
    {
        var cfg = CreateBaseConfiguration();

        var sf = Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
            .BuildSessionFactory();
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory XmlInitialization()
    {
        var cfg = CreateBaseConfiguration();

        cfg.AddFile("Mappings/Xml/Person.hbm.xml");
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory XmlInitializationFromAssembly()
    {
        var cfg = CreateBaseConfiguration();

        cfg.AddAssembly(assembly);
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory ByCodeInitialization()
    {
        var cfg = CreateBaseConfiguration();

        var mapper = new ModelMapper();
        mapper.AddMapping<PersonMapping>();
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return UseConfiguration(cfg);
    }

    [Benchmark]
    public ISessionFactory ByCodeInitializationFromAssembly()
    {
        var cfg = CreateBaseConfiguration();

        var mapper = new ModelMapper();
        mapper.AddMappings(assembly.GetExportedTypes());
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return UseConfiguration(cfg);
    }
}

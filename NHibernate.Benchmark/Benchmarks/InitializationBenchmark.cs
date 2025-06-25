using BenchmarkDotNet.Attributes;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Benchmark.Helpers;
using NHibernate.Benchmark.Mappings.ByCode;
using NHibernate.Benchmark.Mappings.Fluent;
using NHibernate.Benchmark.Models;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Benchmark.Benchmarks;


[ShortRunJob]
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

    [Benchmark]
    public ISessionFactory FluentInitialization()
    {
        var cfg = CreateBaseConfiguration();
        return Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.Add<PersonMap>())
            .BuildSessionFactory();
    }

    [Benchmark]
    public ISessionFactory FluentInitializationFromAssembly()
    {
        var cfg = CreateBaseConfiguration();

        return Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PersonMap>())
            .BuildSessionFactory();
    }

    [Benchmark]
    public ISessionFactory XmlInitialization()
    {
        var cfg = CreateBaseConfiguration();

        cfg.AddFile("Mappings/Xml/Person.hbm.xml");
        return cfg.BuildSessionFactory();
    }

    [Benchmark]
    public ISessionFactory XmlInitializationFromAssembly()
    {
        var cfg = CreateBaseConfiguration();

        cfg.AddAssembly(typeof(Person).Assembly);
        return cfg.BuildSessionFactory();
    }

    [Benchmark]
    public ISessionFactory ByCodeInitialization()
    {
        var cfg = CreateBaseConfiguration();

        var mapper = new ModelMapper();
        mapper.AddMapping<PersonMapping>();
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return cfg.BuildSessionFactory();
    }
}

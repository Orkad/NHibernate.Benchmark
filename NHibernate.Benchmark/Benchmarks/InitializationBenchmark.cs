using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
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


[SimpleJob(RunStrategy.ColdStart, runtimeMoniker: RuntimeMoniker.Net48, launchCount: 3, iterationCount: 1)]
[SimpleJob(RunStrategy.ColdStart, runtimeMoniker: RuntimeMoniker.Net80, launchCount: 3, iterationCount: 1)]
[MemoryDiagnoser]
public class InitializationBenchmark
{
    private static readonly Assembly assembly = typeof(Person).Assembly;
    private Configuration cfg;

    [IterationSetup]
    public void CreateBaseConfiguration()
    {
        cfg = new Configuration();
        cfg.DataBaseIntegration(db =>
        {
            db.Dialect<Dialect.SQLiteDialect>();
            db.Driver<Driver.SQLite20Driver>();
            db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
            db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
            db.LogSqlInConsole = false;
        });
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
        cfg = Fluently.Configure(cfg)
            .Mappings(m =>
            {
                m.FluentMappings.Add<PersonMap>();
                m.FluentMappings.Add<AuthorMap>();
                m.FluentMappings.Add<WorkMap>();
                m.FluentMappings.Add<BookMap>();
                m.FluentMappings.Add<SongMap>();
            })
            .BuildConfiguration();
        return UseConfiguration(cfg);
    }

    [Benchmark(Baseline = true)]
    public ISessionFactory FluentInitializationFromAssembly()
    {
        var sf = Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
            .BuildSessionFactory();
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory XmlInitialization()
    {
        cfg.AddFile("Mappings/Xml/Person.hbm.xml");
        cfg.AddFile("Mappings/Xml/Author.hbm.xml");
        cfg.AddFile("Mappings/Xml/Work.hbm.xml");
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory XmlInitializationFromAssembly()
    {
        cfg.AddAssembly(assembly);
        return UseConfiguration(cfg);

    }

    [Benchmark]
    public ISessionFactory ByCodeInitialization()
    {
        var mapper = new ModelMapper();
        mapper.AddMapping<PersonMapping>();
        mapper.AddMapping<AuthorMapping>();
        mapper.AddMapping<WorkMapping>();
        mapper.AddMapping<BookMapping>();
        mapper.AddMapping<SongMapping>();
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return UseConfiguration(cfg);
    }

    [Benchmark]
    public ISessionFactory ByCodeInitializationFromAssembly()
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(assembly.GetExportedTypes());
        cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return UseConfiguration(cfg);
    }
}

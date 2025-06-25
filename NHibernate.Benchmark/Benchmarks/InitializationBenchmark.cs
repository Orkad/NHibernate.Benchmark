using BenchmarkDotNet.Attributes;
using FluentNHibernate.Cfg;
using NHibernate.Benchmark.Helpers;
using NHibernate.Benchmark.Mappings.ByCode;
using NHibernate.Benchmark.Mappings.Fluent;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Benchmark.Benchmarks;

[SimpleJob(iterationCount: 3)]
[MemoryDiagnoser]
public class InitializationBenchmark
{
    [Params(MappingStrategy.Xml, MappingStrategy.ByCode, MappingStrategy.Fluent)]
    public MappingStrategy Strategy { get; set; }

    [Benchmark]
    public ISessionFactory CreateSessionFactory()
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

        switch (Strategy)
        {
            case MappingStrategy.Xml:
                cfg.AddFile("Mappings/Xml/Person.hbm.xml");
                break;

            case MappingStrategy.ByCode:
                var mapper = new ModelMapper();
                mapper.AddMapping<PersonMapping>(); // suffix Mapping
                cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
                break;

            case MappingStrategy.Fluent:
                return Fluently.Configure(cfg)
                    .Mappings(m => m.FluentMappings.Add<PersonMap>()) // suffix Map
                    .ExposeConfiguration(c => new SchemaExport(c).Create(false, true))
                    .BuildSessionFactory();
        }

        // Pour XML et ByCode : création manuelle du schéma
        new SchemaExport(cfg).Create(false, true);
        return cfg.BuildSessionFactory();
    }
}

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using NHibernate.Benchmark.AuthorWork.Mappings.ByCode;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Data.Common;
using EfPersonContext = NHibernate.Benchmark.AuthorWork.EfCore.PersonContext;
using EfPerson = NHibernate.Benchmark.AuthorWork.EfCore.Models.Person;
using NHPerson = NHibernate.Benchmark.AuthorWork.Models.Person;

namespace NHibernate.Benchmark.EfCoreComparison;

// Compares session.Flush() (NHibernate) against DbContext.SaveChanges() (EF Core) cost once
// ElementsCount entities are tracked with no pending changes, isolating pure change-tracking
// overhead the same way ../TrackingBenchmark.cs does for NHibernate alone. net10.0-only: EF
// Core's current packages don't support net48.
[SimpleJob(
    RunStrategy.Monitoring,
    runtimeMoniker: RuntimeMoniker.Net10_0,
    iterationCount: 30)]
[MemoryDiagnoser]
public class TrackingBenchmark
{
    private ISessionFactory sessionFactory;
    private DbConnection nhibernateConnection;
    private ISession session;

    private DbConnection efConnection;
    private EfPersonContext context;

    [Params(
        2, 4, 8, 16, 32, 64, 128, 258, 512, 1024, 2048, 4096, 8192, 16384,
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
        nhibernateConnection = sessionFactory.OpenSession().Connection;
        new SchemaExport(cfg).Create(false, true, nhibernateConnection);
        using (var statelessSession = sessionFactory.OpenStatelessSession(nhibernateConnection))
        {
            for (int i = 0; i < ElementsCount; i++)
            {
                statelessSession.Insert(new NHPerson { Id = i, FirstName = $"Person {i}" });
            }
        }

        efConnection = EfPersonContext.CreateOpenInMemoryConnection();
        using (var seedContext = new EfPersonContext(efConnection))
        {
            seedContext.Database.EnsureCreated();
            for (int i = 0; i < ElementsCount; i++)
            {
                seedContext.People.Add(new EfPerson { Id = i, FirstName = $"Person {i}" });
            }
            seedContext.SaveChanges();
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        session = sessionFactory.WithOptions().Connection(nhibernateConnection).OpenSession();
        _ = session.Query<NHPerson>().Take(ElementsCount).ToList();

        context = new EfPersonContext(efConnection);
        _ = context.People.Take(ElementsCount).ToList();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        session.Dispose();
        session = null;
        context.Dispose();
        context = null;
    }

    [Benchmark(Baseline = true)]
    public void NHibernateFlush()
    {
        session.Flush();
    }

    [Benchmark]
    public void EfCoreSaveChanges()
    {
        context.SaveChanges();
    }
}

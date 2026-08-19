using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using NHibernate.Benchmark.AuthorWork.Mappings.ByCode;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Data.Common;
using EfPersonContext = NHibernate.Benchmark.AuthorWork.EfCore.PersonContext;
using EfPerson = NHibernate.Benchmark.AuthorWork.EfCore.Models.Person;
using NHPerson = NHibernate.Benchmark.AuthorWork.Models.Person;

namespace NHibernate.Benchmark.EfCoreComparison;

public class PersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

// Compares fetching full Person entities (tracked and read-only) against a full-field LINQ
// projection, NHibernate vs EF Core, across the same ElementsCount row counts as
// ../ProjectionBenchmark.cs. Each ORM gets its own in-memory SQLite database, seeded with
// identically-seeded Bogus data so row content matches between the two. net10.0-only: EF
// Core's current packages don't support net48.
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
[MinColumn, MaxColumn]
public class ProjectionBenchmark
{
    private ISessionFactory sessionFactory;
    private DbConnection nhibernateConnection;
    private ISession session;

    private DbConnection efConnection;
    private EfPersonContext context;

    [Params(1, 5, 10, 50, 100, 500, 1000, 5000)]
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
        Bogus.Randomizer.Seed = new Random(8675309);
        using (var statelessSession = sessionFactory.OpenStatelessSession(nhibernateConnection))
        {
            var nhFaker = new Bogus.Faker<NHPerson>()
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Address, f => f.Address.StreetAddress())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.State, f => f.Address.State())
                .RuleFor(p => p.ZipCode, f => f.Address.ZipCode());
            for (int i = 0; i < ElementsCount; i++)
            {
                statelessSession.Insert(nhFaker.Generate());
            }
        }

        efConnection = EfPersonContext.CreateOpenInMemoryConnection();
        using (var seedContext = new EfPersonContext(efConnection))
        {
            seedContext.Database.EnsureCreated();
            Bogus.Randomizer.Seed = new Random(8675309);
            var efFaker = new Bogus.Faker<EfPerson>()
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Address, f => f.Address.StreetAddress())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.State, f => f.Address.State())
                .RuleFor(p => p.ZipCode, f => f.Address.ZipCode());
            for (int i = 0; i < ElementsCount; i++)
            {
                seedContext.People.Add(efFaker.Generate());
            }
            seedContext.SaveChanges();
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        session = sessionFactory.WithOptions().Connection(nhibernateConnection).OpenSession();
        context = new EfPersonContext(efConnection);
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
    public IList<NHPerson> NHibernateFullEntity()
    {
        return session.Query<NHPerson>().ToList();
    }

    [Benchmark]
    public IList<NHPerson> NHibernateFullEntityNoTracking()
    {
        return session.Query<NHPerson>().WithOptions(o => o.SetReadOnly(true)).ToList();
    }

    [Benchmark]
    public IList<PersonDto> NHibernateProjection()
    {
        return session.Query<NHPerson>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            City = p.City,
            State = p.State,
            ZipCode = p.ZipCode,
        }).ToList();
    }

    [Benchmark]
    public List<EfPerson> EfCoreFullEntity()
    {
        return context.People.ToList();
    }

    [Benchmark]
    public List<EfPerson> EfCoreFullEntityNoTracking()
    {
        return context.People.AsNoTracking().ToList();
    }

    [Benchmark]
    public List<PersonDto> EfCoreProjection()
    {
        return context.People.Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            City = p.City,
            State = p.State,
            ZipCode = p.ZipCode,
        }).ToList();
    }
}

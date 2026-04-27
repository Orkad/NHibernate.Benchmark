using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using NHibernate.Benchmark.AuthorWork.Mappings.ByCode;
using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Transform;
using System.Data.Common;

namespace NHibernate.Benchmark;

public class PersonDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[MinColumn, MaxColumn]
public class ProjectionBenchmark
{
    private ISessionFactory sessionFactory;
    private DbConnection connection;
    private ISession session;

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
        connection = sessionFactory.OpenSession().Connection;
        new SchemaExport(cfg).Create(false, true, connection);
        using var statelessSession = sessionFactory.OpenStatelessSession(connection);
        Bogus.Randomizer.Seed = new Random(8675309);
        var faker = new Bogus.Faker<Person>()
            .RuleFor(p => p.Id, f => f.IndexFaker)
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Address, f => f.Address.StreetAddress())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.State, f => f.Address.State())
            .RuleFor(p => p.ZipCode, f => f.Address.ZipCode());
        for (int i = 0; i < ElementsCount; i++)
        {
            statelessSession.Insert(faker.Generate());
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        session = sessionFactory.WithOptions().Connection(connection).OpenSession();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        session.Dispose();
        session = null;
    }

    //[Benchmark]
    //public IList<Person> QueryOverNoProjection()
    //{
    //    return session.QueryOver<Person>().List();
    //}

    //[Benchmark]
    //public IList<PersonDto> QueryOverProjection()
    //{
    //    PersonDto personAlias = null;
    //    return session.QueryOver<Person>()
    //        .SelectList(list => list
    //            .Select(Projections.Property<Person>(p => p.Id).WithAlias(() => personAlias.Id))
    //            .Select(Projections.Property<Person>(p => p.FirstName).WithAlias(() => personAlias.FirstName))
    //            .Select(Projections.Property<Person>(p => p.LastName).WithAlias(() => personAlias.LastName))
    //        )
    //        .TransformUsing(Transformers.AliasToBean<PersonDto>())
    //        .List<PersonDto>();
    //}

    [Benchmark]
    public IList<Person> FullEntity()
    {
        return session.Query<Person>().ToList();
    }

    [Benchmark]
    public IList<Person> FullEntityNoTracking()
    {
        return session.Query<Person>().WithOptions(o => o.SetReadOnly(true)).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection1Field()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection2Fields()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection3Fields()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection4Fields()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection5Fields()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            City = p.City,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> Projection6Fields()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            City = p.City,
            State = p.State,
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> ProjectionFull()
    {
        return session.Query<Person>().Select(p => new PersonDto
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

    //[Benchmark]
    //public IList<Person> HqlNoProjection()
    //{
    //    return session.CreateQuery("from Person")
    //        .List<Person>();
    //}

    //[Benchmark]
    //public IList<PersonDto> HqlProjection()
    //{
    //    return session.CreateQuery("select p.Id as Id, p.FirstName as FirstName, p.LastName as LastName from Person p")
    //    .SetResultTransformer(Transformers.AliasToBean<PersonDto>())
    //    .List<PersonDto>();
    //}

    //[Benchmark]
    //public IList<Person> SqlNoProjection()
    //{
    //    return session.CreateSQLQuery("SELECT * FROM Person")
    //        .AddEntity(typeof(Person))
    //        .List<Person>();
    //}

    //[Benchmark]
    //public IList<PersonDto> SqlProjection()
    //{
    //    return session.CreateSQLQuery("SELECT Id, FirstName, LastName FROM Person")
    //        .SetResultTransformer(Transformers.AliasToBean<PersonDto>())
    //        .List<PersonDto>();
    //}
}

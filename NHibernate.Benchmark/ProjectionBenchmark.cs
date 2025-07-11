using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
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
    public string Name { get; set; }
}

[SimpleJob(RunStrategy.ColdStart, runtimeMoniker: RuntimeMoniker.Net80, iterationCount: 1, launchCount: 10)]
[MemoryDiagnoser]
public class ProjectionBenchmark
{
    private ISessionFactory sessionFactory;
    private DbConnection connection;
    private ISession session;

    [Params(8, 64, 512, 4096, 32768)]
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
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        session.Dispose();
        session = null;
    }

    [Benchmark]
    public IList<Person> QueryOverNoProjection()
    {
        return session.QueryOver<Person>().List();
    }

    [Benchmark]
    public IList<PersonDto> QueryOverProjection()
    {
        PersonDto personAlias = null;
        return session.QueryOver<Person>()
            .SelectList(list => list
                .Select(Projections.Property<Person>(p => p.Id).WithAlias(() => personAlias.Id))
                .Select(Projections.Property<Person>(p => p.Name).WithAlias(() => personAlias.Name))
            )
            .TransformUsing(Transformers.AliasToBean<PersonDto>())
            .List<PersonDto>();
    }

    [Benchmark]
    public IList<Person> LinqNoProjection()
    {
        return session.Query<Person>().ToList();
    }

    [Benchmark]
    public IList<PersonDto> LinqProjection()
    {
        return session.Query<Person>().Select(p => new PersonDto
        {
            Id = p.Id,
            Name = p.Name,
        }).ToList();
    }

    [Benchmark]
    public IList<Person> HqlNoProjection()
    {
        return session.CreateQuery("from Person")
            .List<Person>();
    }

    [Benchmark]
    public IList<PersonDto> HqlProjection()
    {
        return session.CreateQuery("select p.Id as Id, p.Name as Name from Person p")
        .SetResultTransformer(Transformers.AliasToBean<PersonDto>())
        .List<PersonDto>();
    }

    [Benchmark]
    public IList<Person> SqlNoProjection()
    {
        return session.CreateSQLQuery("SELECT * FROM Person")
            .AddEntity(typeof(Person))
            .List<Person>();
    }

    [Benchmark]
    public IList<PersonDto> SqlProjection()
    {
        return session.CreateSQLQuery("SELECT Id, Name FROM Person")
            .SetResultTransformer(Transformers.AliasToBean<PersonDto>())
            .List<PersonDto>();
    }
}

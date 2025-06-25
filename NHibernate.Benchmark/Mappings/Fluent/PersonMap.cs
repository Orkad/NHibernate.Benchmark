using FluentNHibernate.Mapping;
using NHibernate.Benchmark.Models;

namespace NHibernate.Benchmark.Mappings.Fluent;

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Table("Person");
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.Name);
    }
}

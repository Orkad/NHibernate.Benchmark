using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Table("Person");
        Id(x => x.Id).GeneratedBy.Native();
        Map(x => x.Name);
    }
}

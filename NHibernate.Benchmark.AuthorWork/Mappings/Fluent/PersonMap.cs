using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Table("Person");
        Id(x => x.Id).GeneratedBy.Native();
        Map(x => x.FirstName);
        Map(x => x.LastName);
        Map(x => x.Address);
        Map(x => x.City);
        Map(x => x.State);
        Map(x => x.ZipCode);
    }
}

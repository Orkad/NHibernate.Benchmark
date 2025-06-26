using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.AuthorWork.Mappings.ByCode;

public class PersonMapping : ClassMapping<Person>
{
    public PersonMapping()
    {
        Table("Person");
        Id(x => x.Id, m => m.Generator(Generators.Native));
        Property(x => x.Name);
    }
}

using NHibernate.Benchmark.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.Mappings.ByCode;

public class PersonMapping : ClassMapping<Person>
{
    public PersonMapping()
    {
        Table("Person");
        Id(x => x.Id, m => m.Generator(Generators.Assigned));
        Property(x => x.Name);
    }
}

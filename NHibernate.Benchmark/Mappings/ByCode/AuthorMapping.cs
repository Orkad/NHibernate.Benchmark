using NHibernate.Benchmark.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.Mappings.ByCode;

public class AuthorMapping : ClassMapping<Author>
{
    public AuthorMapping()
    {
        Table("Author");

        Id(x => x.Id, m =>
        {
            m.Column("Id");
            m.Generator(Generators.Assigned);
        });

        Property(x => x.Alias, m => m.Column("Alias"));

        OneToOne(x => x.Person, m =>
        {
            m.Constrained(true); // correspond à <one-to-one constrained="true"/>
        });
    }
}
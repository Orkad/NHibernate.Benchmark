using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.AuthorWork.Mappings.ByCode;

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
            m.Constrained(true);
        });

        Set(x => x.Works, m =>
        {
            m.Table("AuthorWork");
            m.Key(k => k.Column("AuthorId"));
            m.Inverse(true);
            m.Lazy(CollectionLazy.Lazy);
        },
        r => r.ManyToMany(m => m.Column("WorkId")));
    }
}

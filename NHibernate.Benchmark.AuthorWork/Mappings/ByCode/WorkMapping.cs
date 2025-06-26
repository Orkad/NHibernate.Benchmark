using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.AuthorWork.Mappings.ByCode;

public class WorkMapping : ClassMapping<Work>
{
    public WorkMapping()
    {
        Table("Work");

        Id(x => x.Id, m =>
        {
            m.Column("Id");
            m.Generator(Generators.Native);
        });

        DiscriminatorValue("W");
        
        Discriminator(d =>
        {
            d.Column("Type");
            d.Type(NHibernateUtil.Character);
        });

        Property(x => x.Title);

        Set(x => x.Authors, m =>
        {
            m.Table("AuthorWork");
            m.Key(k =>
            {
                k.Column("WorkId");
                k.NotNullable(true);
            });
            m.Lazy(CollectionLazy.Lazy);
        },
        r => r.ManyToMany(m =>
        {
            m.Class(typeof(Author));
            m.Column(c =>
            {
                c.Name("AuthorId");
                c.NotNullable(true);
            });
        }));
    }
}

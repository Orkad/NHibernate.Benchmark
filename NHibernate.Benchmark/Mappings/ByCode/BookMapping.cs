using NHibernate.Benchmark.Models;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.Mappings.ByCode;

public class BookMapping : SubclassMapping<Book>
{
    public BookMapping()
    {
        DiscriminatorValue("B");

        Property(x => x.Text, m => m.Column("Text"));
    }
}

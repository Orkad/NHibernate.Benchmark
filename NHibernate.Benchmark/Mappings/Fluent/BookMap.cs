using FluentNHibernate.Mapping;
using NHibernate.Benchmark.Models;

namespace NHibernate.Benchmark.Mappings.Fluent;

public class BookMap : SubclassMap<Book>
{
    public BookMap()
    {
        DiscriminatorValue("B");
        Map(x => x.Text).Column("Text");
    }
}

using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;

public class BookMap : SubclassMap<Book>
{
    public BookMap()
    {
        DiscriminatorValue("B");
        Map(x => x.Text).Column("Text");
    }
}

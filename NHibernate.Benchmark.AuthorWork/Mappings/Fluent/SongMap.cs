using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;

public class SongMap : SubclassMap<Song>
{
    public SongMap()
    {
        DiscriminatorValue("S");
        Map(x => x.Tempo).Column("Tempo");
        Map(x => x.Genre).Column("Genre");
    }
}
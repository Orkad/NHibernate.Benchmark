using NHibernate.Benchmark.Models;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.Mappings.ByCode;

public class SongMapping : SubclassMapping<Song>
{
    public SongMapping()
    {
        DiscriminatorValue("S");

        Property(x => x.Tempo, m => m.Column("Tempo"));
        Property(x => x.Genre, m => m.Column("Genre"));
    }
}
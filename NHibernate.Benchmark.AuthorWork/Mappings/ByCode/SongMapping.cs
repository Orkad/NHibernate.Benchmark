﻿using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.AuthorWork.Mappings.ByCode;

public class SongMapping : SubclassMapping<Song>
{
    public SongMapping()
    {
        DiscriminatorValue("S");

        Property(x => x.Tempo, m => m.Column("Tempo"));
        Property(x => x.Genre, m => m.Column("Genre"));
    }
}
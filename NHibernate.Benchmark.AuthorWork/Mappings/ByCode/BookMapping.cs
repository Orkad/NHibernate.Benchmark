﻿using NHibernate.Benchmark.AuthorWork.Models;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Benchmark.AuthorWork.Mappings.ByCode;

public class BookMapping : SubclassMapping<Book>
{
    public BookMapping()
    {
        DiscriminatorValue("B");

        Property(x => x.Text, m => m.Column("Text"));
    }
}

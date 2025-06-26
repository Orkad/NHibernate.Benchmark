using FluentNHibernate.Mapping;
using NHibernate.Benchmark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Benchmark.Mappings.Fluent;
public class AuthorMap : ClassMap<Author>
{
    public AuthorMap()
    {
        Table("Author");
        Id(x => x.Id).GeneratedBy.Assigned();
        Map(x => x.Alias);
        HasOne(x => x.Person).Constrained();
    }
}

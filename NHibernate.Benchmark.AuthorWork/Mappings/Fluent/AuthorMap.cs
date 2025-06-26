using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;
public class AuthorMap : ClassMap<Author>
{
    public AuthorMap()
    {
        Table("Author");

        Id(x => x.Id, "Id")
            .GeneratedBy.Assigned(); // <generator class="assigned"/>

        Map(x => x.Alias, "Alias");

        HasOne(x => x.Person)
            .Constrained(); // <one-to-one constrained="true"/>

        HasManyToMany(x => x.Works)
            .Table("AuthorWork")
            .ParentKeyColumn("AuthorId")
            .ChildKeyColumn("WorkId")
            .Inverse()       // <set inverse="true"/>
            .LazyLoad();     // lazy="true"
    }
}

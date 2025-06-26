using FluentNHibernate.Mapping;
using NHibernate.Benchmark.AuthorWork.Models;

namespace NHibernate.Benchmark.AuthorWork.Mappings.Fluent;

public class WorkMap : ClassMap<Work>
{
    public WorkMap()
    {
        Table("Work");
        DiscriminateSubClassesOnColumn("Type", 'W');

        Id(x => x.Id, "Id")
            .GeneratedBy.Native();

        Map(x => x.Title);

        HasManyToMany(x => x.Authors)
            .Table("AuthorWork")
            .ParentKeyColumn("WorkId")
            .ChildKeyColumn("AuthorId")
            .LazyLoad();
    }
}

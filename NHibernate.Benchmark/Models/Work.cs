namespace NHibernate.Benchmark.Models;

public class Work
{
    public virtual int Id { get; set; }
    public virtual string Title { get; set; }
    public virtual ISet<Author> Authors { get; set; }
}

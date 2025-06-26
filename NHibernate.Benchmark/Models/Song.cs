namespace NHibernate.Benchmark.Models;

public class Song : Work
{
    public virtual float Tempo { get; set; }
    public virtual string Genre { get; set; }
}

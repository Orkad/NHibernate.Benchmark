namespace NHibernate.Benchmark.AuthorWork.Models;

public class Person
{
    public virtual int Id { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Address { get; set; }
    public virtual string City { get; set; }
    public virtual string State { get; set; }
    public virtual string ZipCode { get; set; }
}

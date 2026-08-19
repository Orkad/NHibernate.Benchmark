using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NHibernate.Benchmark.AuthorWork.EfCore.Models;
using System.Data.Common;

namespace NHibernate.Benchmark.AuthorWork.EfCore;

public class PersonContext : DbContext
{
    private readonly DbConnection connection;

    public DbSet<Person> People { get; set; }

    public PersonContext(DbConnection connection)
    {
        this.connection = connection;
    }

    // The SQLite in-memory database is destroyed once its connection closes, so callers
    // must keep this connection open for as long as they want the database to survive
    // (mirroring how the NHibernate benchmarks keep their SQLite connection open).
    public static DbConnection CreateOpenInMemoryConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        return connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");
            entity.HasKey(p => p.Id);
        });
    }
}

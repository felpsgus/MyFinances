using Microsoft.EntityFrameworkCore;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Context;

public class MyFinancesDbContext : DbContext
{
    public MyFinancesDbContext(DbContextOptions<MyFinancesDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<Namespace> Namespaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFinancesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
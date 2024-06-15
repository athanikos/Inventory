using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Expressions.Entities;

namespace Inventory.Expressions;

public class ExpressionDbContext : DbContext
{    
    public ExpressionDbContext(DbContextOptions
    options) :
    base(options)
    { }

    public DbSet<SimpleExpression> Parameters { get; set; }

    protected override void OnModelCreating
    (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Expressions");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var config =
                        modelBuilder.Entity<SimpleExpression>();
        config.ToTable("Expression").HasKey(p => p.Id);

    }

}


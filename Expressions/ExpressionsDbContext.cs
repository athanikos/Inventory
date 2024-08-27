using Expressions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Expressions;

public class ExpressionsDbContext : DbContext
{

    public ExpressionsDbContext()  { }

    public ExpressionsDbContext(DbContextOptions<ExpressionsDbContext>    options) :
           base(options)  { }
           
    public DbSet<InventoryExpression> InventoryExpressions { get; set; }
    public DbSet<ProductExpression> ProductExpressions { get; set; }
    public DbSet<BooleanExpression> BooleanExpressions { get; set; }

    protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Expressions");

                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
                        modelBuilder.Entity<InventoryExpression>().ToTable("InventoryExpression");
                        modelBuilder.Entity<ProductExpression>().ToTable("ProductExpression");
                        modelBuilder.Entity<BooleanExpression>().ToTable("BooleanExpression");
    }

}


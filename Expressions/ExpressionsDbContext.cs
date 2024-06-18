using Expressions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Expressions;

public class ExpressionsDbContext : DbContext
{

    public ExpressionsDbContext()
    { }


    public ExpressionsDbContext(
        DbContextOptions<ExpressionsDbContext>    options) :
            base(options)  { }
           

         public DbSet<InventoryExpression> InventoryExpressions { get; set; }
         public DbSet<ProductExpression> ProductExpressions { get; set; }



         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Expressions");

                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                        var config = 
                        modelBuilder.Entity<InventoryExpression>();
                        config.ToTable("InventoryExpression").HasKey(p=>p.Id);

                        modelBuilder.Entity<ProductExpression>();
                        config.ToTable("ProductExpression").HasKey(p => p.Id);



         }

}


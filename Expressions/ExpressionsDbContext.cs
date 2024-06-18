using Expressions.Entities;
using Inventory.Expressions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Expressions;

public  class ExpressionsDbContext : DbContext
{
    public ExpressionsDbContext(DbContextOptions<ExpressionsDbContext>
            options) :
            base(options)  { }
           

         public DbSet<MultipleProductExpression> MultipleProductExpressions { get; set; }
         public DbSet<ProductExpression> SingleProductExpressions { get; set; }



         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Expressions");

                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                        var config = 
                        modelBuilder.Entity<MultipleProductExpression>();
                        config.ToTable("MultipleProductExpression").HasKey(p=>p.Id);

                        modelBuilder.Entity<ProductExpression>();
                        config.ToTable("ProductExpression").HasKey(p => p.Id);



    }

}


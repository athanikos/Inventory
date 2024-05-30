using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Products;

public  class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) :
            base(options)    
        { }

       
       public  DbSet<Product> Products;    


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                    base.OnModelCreating(modelBuilder);

                    modelBuilder.Entity<Product>()
                    .HasMany(e => e.Categories)
                    .WithMany(e => e.Products)
                    .UsingEntity<ProductCategory>();

                    modelBuilder.Entity<Product>()
                     .HasMany(e => e.Metrics)
                     .WithMany(e => e.Products)
                     .UsingEntity<ProductMetric>();

            }


}


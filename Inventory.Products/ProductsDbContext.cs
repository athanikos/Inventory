using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;


namespace Inventory.Products;

public class ProductsDbContext : DbContext
{

        public ProductsDbContext(DbContextOptions
            <ProductsDbContext> options) :
            base(options)  { }
           
        public   DbSet<Product> Products { get; set; }
        public   DbSet<Inventory.Products.Entities.Inventory> Inventories { get; set; }


    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
        {
                    base.OnModelCreating(modelBuilder);
                    modelBuilder.HasDefaultSchema("Products");

                    var config = modelBuilder.Entity<Product>();
                    modelBuilder.Entity<Product>().ToTable("Product");

                    modelBuilder.Entity<Inventory.Products.Entities.Inventory>()
                    .HasMany(e => e.Products);

                    modelBuilder.Entity<Inventory.Products.Entities.Inventory>().ToTable("Inventory");


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


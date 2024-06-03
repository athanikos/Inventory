using Entities = Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Products;

public class ProductsDbContext : DbContext
{

        public ProductsDbContext(DbContextOptions
            <ProductsDbContext> options) :
            base(options)  { }
           
        public   DbSet<Entities.Product> Products { get; set; }
        public   DbSet<Entities.Inventory> Inventories { get; set; }
        public   DbSet<Entities.Category> Categories { get; set; }
        public   DbSet<Entities.Metric> Metrics { get; set; }

        public DbSet<Entities.Transaction> Transactions { get; set; }
        public DbSet<Entities.TransactionItem> TransactionItems { get; set; }

    protected override void OnModelCreating
            (ModelBuilder modelBuilder)
            {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Products");

                        var config = modelBuilder.Entity<Entities.Product>();
                        modelBuilder.Entity<Entities.Product>().ToTable("Product");

                        modelBuilder.Entity<Entities.Inventory>()
                        .HasMany(e => e.Products);

                        modelBuilder.Entity<Entities.Inventory>().ToTable("Inventory");


                        modelBuilder.Entity<Entities.Category>().ToTable("Category");

                        modelBuilder.Entity<Entities.Product>()
                                                .HasMany(e => e.Categories)
                                                .WithMany(e => e.Products)
                                                .UsingEntity<Entities.ProductCategory>();

                        modelBuilder.Entity<Entities.Product>()
                                    .HasMany(e => e.Metrics)
                                     .WithMany(e => e.Products)
                                     .UsingEntity<Entities.ProductMetric>();

                        modelBuilder.Entity<Entities.Source>()
                          .HasMany(e => e.Metrics);
                   



        }



}


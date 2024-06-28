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
        public   DbSet<Entities.Source> Sources { get; set; }
        public   DbSet<Entities.ProductCategory> ProductCategories { get; set; }
        public   DbSet<Entities.ProductMetric> ProductMetrics { get; set; }
        public   DbSet<Entities.InventoryMetric> InventoryMetrics { get; set; }
    
        protected override void OnModelCreating
            (ModelBuilder modelBuilder)
            {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Products");

                        modelBuilder.Entity<Entities.Product>().ToTable("Product");

                        modelBuilder.Entity<Entities.Inventory>()
                        .HasMany(e => e.Products);

                        modelBuilder.Entity<Entities.Inventory>().ToTable("Inventory");
                        modelBuilder.Entity<Entities.Source>().ToTable("Source");
                        modelBuilder.Entity<Entities.Category>().ToTable("Category");

                        modelBuilder.Entity<Entities.Product>()
                                                .HasMany(e => e.Categories)
                                                .WithMany(e => e.Products)
                                                .UsingEntity<Entities.ProductCategory>();

                        modelBuilder.Entity<Entities.Product>()
                                    .HasMany(e => e.Metrics)
                                    .WithMany(e => e.Products)
                                     .UsingEntity<Entities.ProductMetric>()
                                     .HasKey(p => new { p.MetricId, p.ProductId, p.EffectiveDate});

                        modelBuilder.Entity<Entities.Inventory>()
                        .HasMany(e => e.Metrics)
                        .WithMany(e => e.Inventories)
                        .UsingEntity<Entities.InventoryMetric>()
                        .HasKey(p => new { p.MetricId, p.InventoryId, p.EffectiveDate });




                        modelBuilder.Entity<Entities.Source>()
                                          .HasMany(e => e.Metrics);
                   



        }

     protected override void ConfigureConventions(
     ModelConfigurationBuilder configurationBuilder)
        {
        configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
        }



}


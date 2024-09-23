using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
namespace Inventory.Products;

public class ProductsDbContext : DbContext
{

    public ProductsDbContext(DbContextOptions
        <ProductsDbContext> options) :
        base(options)
    { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Entities.Inventory> Inventories { get; set; }
    public DbSet<Entities.Category> Categories { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<Entities.Source> Sources { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductMetric> ProductMetrics { get; set; }
    public DbSet<QuantityMetric> QuantityMetrics { get; set; }
    public DbSet<InventoryMetric> InventoryMetrics { get; set; }

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

        modelBuilder.Entity<Entities.Category>().HasKey(e => e.Id);

        modelBuilder.Entity<QuantityMetric>().ToTable("QuantityMetric");


        modelBuilder.Entity<QuantityMetric>()
        .HasKey(p => new { p.ProductId, p.EffectiveDate });

        modelBuilder.Entity<Product>().HasMany(e => e.Categories)
                                      .WithMany(e => e.Products)
                                      .UsingEntity<ProductCategory>();

        modelBuilder.Entity<Metric>()
          .HasIndex(u => u.Code)
          .IsUnique();

        modelBuilder.Entity<Product>()
          .HasIndex(u => u.Code)
          .IsUnique();


        modelBuilder.Entity<Product>()
          .HasMany(e => e.Metrics)
          .WithMany(e => e.Products)
          .UsingEntity<ProductMetric>()
          .HasKey(p => new { p.MetricId, p.ProductId, p.EffectiveDate });

        modelBuilder.Entity<Entities.Inventory>()
            .HasMany(e => e.Metrics)
            .WithMany(e => e.Inventories)
            .UsingEntity<InventoryMetric>()
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


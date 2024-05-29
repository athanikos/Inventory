    using Microsoft.EntityFrameworkCore;
    namespace Inventory.Products;

    public  class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) :
            base(options)    
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
        .HasMany(o => o.ProductMetrics);

        }


    }


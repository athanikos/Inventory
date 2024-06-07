using Microsoft.EntityFrameworkCore;

namespace Inventory.Prices;

internal class PricesDbContext : DbContext
{

    internal PricesDbContext(DbContextOptions
            <PricesDbContext> options) :
            base(options)  { }
           
        internal    DbSet<Entities.Parameters> Parameters { get; set; }
     
         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Prices");

                        var config = modelBuilder.Entity<Entities.Parameters>();
                        modelBuilder.Entity<Entities.Parameters>().ToTable("Parameters");

         }

}


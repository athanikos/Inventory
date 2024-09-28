using Inventory.Defaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Defaults;

public class DefaultsDbContext(
    DbContextOptions
        <DefaultsDbContext> options) : DbContext(options)
{
    public DbSet<Default> Defaults { get; set; }
 
    
    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Defaults");
        modelBuilder.Entity<Default>().ToTable("Default");

    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 6);
    }



}
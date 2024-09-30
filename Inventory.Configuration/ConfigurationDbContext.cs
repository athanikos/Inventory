using Inventory.Defaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Defaults;

public class ConfigurationDbContext(
    DbContextOptions
        <ConfigurationDbContext> options) : DbContext(options)
{
    public DbSet<Configuration> Configurations { get; set; }
 
    
    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Configurations");
        modelBuilder.Entity<Configuration>().ToTable("Configuration");

        modelBuilder.Entity<Configuration>().ToTable("Configuration").HasKey(o => o.Type);


    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 6);
    }



}
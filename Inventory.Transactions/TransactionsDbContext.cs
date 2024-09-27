using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Inventory.Transactions.Entities.Generic;

namespace Inventory.Transactions;

public class TransactionsDbContext(
    DbContextOptions
        <TransactionsDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Transaction> Transactions { get; set; }

    public DbSet<Entities.TransactionSectionGroup> TransactionSectionGroups { get; set; }
    public DbSet<Entities.TransactionSection> TransactionSections { get; set; }


    public DbSet<Template> Templates { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Entities.Value> Values { get; set; }

    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Transactions");
  
        modelBuilder.Entity<Entities.Transaction>().ToTable("Transaction");
        modelBuilder.Entity<Entities.TransactionSection>().ToTable("TransactionSection");
        modelBuilder.Entity<Entities.TransactionSectionGroup>().ToTable("TransactionSectionGroup");

        modelBuilder.Entity<Entities.Value>().ToTable("Value");
        modelBuilder.Entity<Template>().ToTable("Template");
        modelBuilder.Entity<Section>().ToTable("Section");
        modelBuilder.Entity<Field>().ToTable("Field");
        
        modelBuilder.Entity<Template>()
        .HasMany(e => e.Sections);
        modelBuilder.Entity<Section>()
        .HasMany(e => e.Fields);
        modelBuilder.Entity<Section>().HasKey(e => e.Id);
        modelBuilder.Entity<Template>().HasKey(e => e.Id);
        modelBuilder.Entity<Entities.TransactionSection>().HasKey(e => e.Id);

        modelBuilder.Entity<Field>().HasMany(e => e.Values);

        modelBuilder.Entity<Field>().HasKey(e => e.Id);
        modelBuilder.Entity < Entities.Value>().HasKey(e => e.Id);

        modelBuilder.Entity<Entities.TransactionSection>().HasKey(e => e.Id);



        modelBuilder.Entity<Entities.Transaction>()
                                    .HasMany(e => e.TransactionSections)
                                    .WithOne(a => a.Transaction)
                                    .HasForeignKey(e => e.TransactionId);

        modelBuilder.Entity<Entities.TransactionSection>()
                                    .HasMany(e => e.SectionGroups)
                                    .WithOne(a => a.TransactionSection)
                                    .HasForeignKey(e => e.TransactionSectionId);

        modelBuilder.Entity<Entities.TransactionSectionGroup>()
                             .HasMany(e => e.Values)
                             .WithOne(a => a.TransactionSectionGroup)
                             .HasForeignKey(e => e.TransactionSectionGroupId);
    }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
    }


}


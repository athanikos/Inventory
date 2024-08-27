using Microsoft.EntityFrameworkCore;
namespace Inventory.Transactions;

public class TransactionsDbContext : DbContext
{
    public TransactionsDbContext(DbContextOptions
        <TransactionsDbContext> options) :
        base(options) { }

    public DbSet<Entities.Transaction> Transactions { get; set; }
    public DbSet<Entities.Entity> Entities  { get; set; }
    public DbSet<Entities.Template> Templates { get; set; }
    public DbSet<Entities.Section> Sections { get; set; }
    public DbSet<Entities.Field> Fields { get; set; }
    public DbSet<Entities.Value> Values { get; set; }

    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Transactions");

        modelBuilder.Entity<Entities.Transaction>().ToTable("Transaction");
        modelBuilder.Entity<Entities.Entity>().ToTable("Entity");
        modelBuilder.Entity<Entities.Template>().ToTable("Template");
        modelBuilder.Entity<Entities.Section>().ToTable("Section");
        modelBuilder.Entity<Entities.Field>().ToTable("Field");
        modelBuilder.Entity<Entities.Value>().ToTable("Value");

        modelBuilder.Entity<Entities.Template>()
        .HasMany(e => e.Sections);

        modelBuilder.Entity<Entities.Section>()
        .HasMany(e => e.Fields);

        modelBuilder.Entity<Entities.Section>().HasKey(e => e.Id);
        
        modelBuilder.Entity<Entities.Template>().HasKey(e => e.Id);  

        modelBuilder.Entity<Entities.Field>()
                                               .HasMany(e => e.FieldValues)
                                               .WithOne(e => e.Field);

        modelBuilder.Entity<Entities.Field>().HasKey(e => e.Id);

        modelBuilder.Entity < Entities.Value>().HasKey(e => e.Id);


        modelBuilder.Entity<Entities.Transaction>()
                                           .HasMany(e => e.Values)
                                            .WithOne(a => a.Transaction)
                                            .HasForeignKey(e => e.TransactionId);
      
        modelBuilder.Entity<Entities.Entity>()
                                   .HasMany(e => e.Values)
                                   .WithOne(a => a.Entity)
                                   .HasForeignKey(e => e.EntityId);

    }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
    }
}


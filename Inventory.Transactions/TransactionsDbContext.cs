using Microsoft.EntityFrameworkCore;
namespace Inventory.Transactions;

public class TransactionsDbContext : DbContext
{

    public TransactionsDbContext(DbContextOptions
        <TransactionsDbContext> options) :
        base(options)
    { }

    public DbSet<Entities.Transaction> Transactions { get; set; }
    public DbSet<Entities.TransactionItem> TransactionItems { get; set; }
    public DbSet<Entities.TransactionItemTemplate> Templates { get; set; }
    public DbSet<Entities.TransactionItemFieldValue> Values { get; set; }



    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Transactions");

        modelBuilder.Entity<Entities.TransactionItemTemplate>().ToTable("TransactionItemTemplate");

        modelBuilder.Entity<Entities.TransactionItemTemplate>()
        .HasMany(e => e.TemplateFields);


        modelBuilder.Entity<Entities.TransactionItemTemplate>().HasKey(e => e.Id);  

        modelBuilder.Entity<Entities.TransactionItemTemplateField>()
                                               .HasMany(e => e.FieldValues)
                                               .WithOne(e => e.Field);

        modelBuilder.Entity<Entities.TransactionItemTemplateField>().HasKey(e => e.Id);


        modelBuilder.Entity < Entities.TransactionItemFieldValue>().HasKey(e => e.Id);



    }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
    }



}


using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace Inventory.Transactions;

public class TransactionsDbContext : DbContext
{

    public TransactionsDbContext(DbContextOptions
        <TransactionsDbContext> options) :
        base(options)
    { }

    public DbSet<Entities.Transaction> Transactions { get; set; }
    public DbSet<Entities.TransactionItem> TransactionItems { get; set; }

    protected override void OnModelCreating
        (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Transactions");






    }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
    }



}


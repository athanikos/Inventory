using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Prices;

public  class NotifierDbContext : DbContext
{
    public NotifierDbContext() { } // This one

    public NotifierDbContext(DbContextOptions
            options) :
            base(options)  { }
           
        public     DbSet<Entities.NotifierParameter> Parameters { get; set; }
     
         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Notifier");
                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                        var config = 
                        modelBuilder.Entity<Entities.NotifierParameter>();
                        config.ToTable("NotifierParameter").HasKey(p=>p.Id);

         }

}


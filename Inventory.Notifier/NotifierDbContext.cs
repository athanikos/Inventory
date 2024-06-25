using Inventory.Notifications.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Prices;

public  class NotifierDbContext : DbContext
{
    public NotifierDbContext() { } // This one

    public NotifierDbContext(DbContextOptions
            options) :
            base(options)  { }
           
        public     DbSet<Notification> Notifications { get; set; }
     
         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Notifications");
                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                        var config = 
                        modelBuilder.Entity<Notification>();
                        config.ToTable("Notification").HasKey(p=>p.Id);

         }

}


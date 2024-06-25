using Inventory.Notifications.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Notifications;

public  class NotifierDbContext : DbContext
{
 
    public NotifierDbContext(DbContextOptions<NotifierDbContext>
            options) :
            base(options)  { }

    public NotifierDbContext() { } // This one


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


﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Prices;

public  class PricesDbContext : DbContext
{
        public PricesDbContext(DbContextOptions<PricesDbContext>
            options) :
            base(options)  { }
           
        public     DbSet<Entities.PricesParameter> Parameters { get; set; }
     
         protected override void OnModelCreating
         (ModelBuilder modelBuilder)
         {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.HasDefaultSchema("Prices");
                        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                        var config = 
                        modelBuilder.Entity<Entities.PricesParameter>();
                        config.ToTable("PricesParameter").HasKey(p=>p.Id);

         }

}


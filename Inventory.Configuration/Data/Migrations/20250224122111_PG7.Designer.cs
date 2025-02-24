﻿// <auto-generated />
using System;
using Inventory.Defaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Defaults.Data.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20250224122111_PG7")]
    partial class PG7
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Configurations")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Inventory.Defaults.Entities.Configuration", b =>
                {
                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid?>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Type");

                    b.ToTable("Configuration", "Configurations");
                });
#pragma warning restore 612, 618
        }
    }
}

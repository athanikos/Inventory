﻿// <auto-generated />
using System;
using Inventory.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inventory.Notifications.Data.Migrations
{
    [DbContext(typeof(NotifierDbContext))]
    [Migration("20240625173713_iniial")]
    partial class iniial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Notifications")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inventory.Notifications.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BooleanExpressionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("ExpressionValue")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("NotifyEveryMinutes")
                        .HasColumnType("int");

                    b.Property<int>("NotifyTimes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Notification", "Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Inventory.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    partial class ProductsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Products")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Inventory.Products.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FatherId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Category", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.Inventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Inventory", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.InventoryMetric", b =>
                {
                    b.Property<Guid>("MetricId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InventoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InventoryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MetricCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.HasKey("MetricId", "InventoryId", "EffectiveDate");

                    b.HasIndex("InventoryId");

                    b.ToTable("InventoryMetrics", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.Metric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Metrics", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("InventoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.ToTable("Product", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.ProductCategory", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCategories", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.ProductMetric", b =>
                {
                    b.Property<Guid>("MetricId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MetricCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.HasKey("MetricId", "ProductId", "EffectiveDate");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductMetrics", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.QuantityMetric", b =>
                {
                    b.Property<Guid>("MetricId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("MetricCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.HasKey("MetricId", "ProductId", "EffectiveDate");

                    b.ToTable("QuantityMetrics", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Source", "Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.InventoryMetric", b =>
                {
                    b.HasOne("Inventory.Products.Entities.Inventory", null)
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Products.Entities.Metric", null)
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Products.Entities.Metric", b =>
                {
                    b.HasOne("Inventory.Products.Entities.Source", null)
                        .WithMany("Metrics")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Products.Entities.Product", b =>
                {
                    b.HasOne("Inventory.Products.Entities.Inventory", null)
                        .WithMany("Products")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Products.Entities.ProductCategory", b =>
                {
                    b.HasOne("Inventory.Products.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Products.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Products.Entities.ProductMetric", b =>
                {
                    b.HasOne("Inventory.Products.Entities.Metric", null)
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Products.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Products.Entities.Inventory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Inventory.Products.Entities.Source", b =>
                {
                    b.Navigation("Metrics");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Inventory.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    [DbContext(typeof(TransactionsDbContext))]
    [Migration("20240829132514_PG9")]
    partial class PG9
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Transactions")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Inventory.Transactions.Entities.Field", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Expression")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Field", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SectionType")
                        .HasColumnType("integer");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("Section", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Template", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Transaction", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<int>("TransactionSectionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionSection", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSectionGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("GroupValue")
                        .HasColumnType("integer");

                    b.Property<Guid>("TransactionSectionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransactionSectionId");

                    b.ToTable("TransactionSectionGroup", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Value", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FieldId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TransactionSectionGroupId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("TransactionSectionGroupId");

                    b.ToTable("Value", "Transactions");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Field", b =>
                {
                    b.HasOne("Inventory.Transactions.Entities.Section", null)
                        .WithMany("Fields")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Section", b =>
                {
                    b.HasOne("Inventory.Transactions.Entities.Template", null)
                        .WithMany("Sections")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSection", b =>
                {
                    b.HasOne("Inventory.Transactions.Entities.Transaction", "Transaction")
                        .WithMany("TransactionSections")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSectionGroup", b =>
                {
                    b.HasOne("Inventory.Transactions.Entities.TransactionSection", "TransactionSection")
                        .WithMany("SectionGroups")
                        .HasForeignKey("TransactionSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransactionSection");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Value", b =>
                {
                    b.HasOne("Inventory.Transactions.Entities.Field", "Field")
                        .WithMany("Values")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Transactions.Entities.TransactionSectionGroup", "TransactionSectionGroup")
                        .WithMany("Values")
                        .HasForeignKey("TransactionSectionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("TransactionSectionGroup");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Field", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Section", b =>
                {
                    b.Navigation("Fields");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Template", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.Transaction", b =>
                {
                    b.Navigation("TransactionSections");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSection", b =>
                {
                    b.Navigation("SectionGroups");
                });

            modelBuilder.Entity("Inventory.Transactions.Entities.TransactionSectionGroup", b =>
                {
                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}

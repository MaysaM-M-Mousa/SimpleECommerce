﻿// <auto-generated />
using System;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    partial class InventoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BuildingBlocks.Application.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HandlerType")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MessageId", "HandlerType")
                        .IsUnique();

                    b.ToTable("InboxMessages");
                });

            modelBuilder.Entity("Inventory.Domain.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The new branch IPhone from Apple!",
                            Name = "IPhone 15",
                            Price = 1200m,
                            StockQuantity = 15
                        },
                        new
                        {
                            Id = 2,
                            Description = "High-performance gaming laptop from Asus",
                            Name = "Asus ROG Strix G16",
                            Price = 2000m,
                            StockQuantity = 150
                        },
                        new
                        {
                            Id = 3,
                            Description = "Ultra FHD Samsung TV",
                            Name = "Samsung TV",
                            Price = 3200m,
                            StockQuantity = 200
                        },
                        new
                        {
                            Id = 4,
                            Description = "Premium noise-canceling headphones",
                            Name = "Sony WH-1000XM5",
                            Price = 300m,
                            StockQuantity = 40
                        },
                        new
                        {
                            Id = 5,
                            Description = "Compact and powerful smart speaker",
                            Name = "Amazon Echo Dot 5th Gen",
                            Price = 150m,
                            StockQuantity = 15
                        });
                });

            modelBuilder.Entity("Inventory.Domain.Products.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("Inventory.Domain.Products.Reservation", b =>
                {
                    b.HasOne("Inventory.Domain.Products.Product", null)
                        .WithMany("Reservations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Domain.Products.Product", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}

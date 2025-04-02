﻿// <auto-generated />
using System;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BuildingBlocks.Application.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("HandlerType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MessageId", "HandlerType")
                        .IsUnique();

                    b.ToTable("InboxMessages");
                });

            modelBuilder.Entity("BuildingBlocks.Application.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProcessedOnUtc");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("Inventory.Application.Products.ReserveStock.Saga.ReserveStocksSagaState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.HasKey("CorrelationId");

                    b.ToTable("ReserveStocksSagaStates");
                });

            modelBuilder.Entity("Inventory.Domain.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer");

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

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("Inventory.Application.Products.ReserveStock.Saga.ReserveStocksSagaState", b =>
                {
                    b.OwnsOne("Inventory.Application.Products.ReserveStock.Saga.ReservationDetails", "ReservationDetails", b1 =>
                        {
                            b1.Property<Guid>("ReserveStocksSagaStateCorrelationId")
                                .HasColumnType("uuid");

                            b1.HasKey("ReserveStocksSagaStateCorrelationId");

                            b1.ToTable("ReserveStocksSagaStates");

                            b1.ToJson("ReservationDetails");

                            b1.WithOwner()
                                .HasForeignKey("ReserveStocksSagaStateCorrelationId");

                            b1.OwnsMany("Inventory.Application.Products.ReserveStock.Saga.ProductQuantity", "ProductsToRelease", b2 =>
                                {
                                    b2.Property<Guid>("ReservationDetailsReserveStocksSagaStateCorrelationId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<int>("ProductId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("Quantity")
                                        .HasColumnType("integer");

                                    b2.HasKey("ReservationDetailsReserveStocksSagaStateCorrelationId", "__synthesizedOrdinal");

                                    b2.ToTable("ReserveStocksSagaStates");

                                    b2.WithOwner()
                                        .HasForeignKey("ReservationDetailsReserveStocksSagaStateCorrelationId");
                                });

                            b1.OwnsMany("Inventory.Application.Products.ReserveStock.Saga.ProductQuantity", "ProductsToReserve", b2 =>
                                {
                                    b2.Property<Guid>("ReservationDetailsReserveStocksSagaStateCorrelationId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<int>("ProductId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("Quantity")
                                        .HasColumnType("integer");

                                    b2.HasKey("ReservationDetailsReserveStocksSagaStateCorrelationId", "__synthesizedOrdinal");

                                    b2.ToTable("ReserveStocksSagaStates");

                                    b2.WithOwner()
                                        .HasForeignKey("ReservationDetailsReserveStocksSagaStateCorrelationId");
                                });

                            b1.OwnsMany("Inventory.Application.Products.ReserveStock.Saga.ProductQuantity", "ReservedProducts", b2 =>
                                {
                                    b2.Property<Guid>("ReservationDetailsReserveStocksSagaStateCorrelationId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<int>("ProductId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("Quantity")
                                        .HasColumnType("integer");

                                    b2.HasKey("ReservationDetailsReserveStocksSagaStateCorrelationId", "__synthesizedOrdinal");

                                    b2.ToTable("ReserveStocksSagaStates");

                                    b2.WithOwner()
                                        .HasForeignKey("ReservationDetailsReserveStocksSagaStateCorrelationId");
                                });

                            b1.Navigation("ProductsToRelease");

                            b1.Navigation("ProductsToReserve");

                            b1.Navigation("ReservedProducts");
                        });

                    b.Navigation("ReservationDetails")
                        .IsRequired();
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

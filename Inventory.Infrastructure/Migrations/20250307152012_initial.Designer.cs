﻿// <auto-generated />
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    [Migration("20250307152012_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

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

                    b.Property<int>("Quantity")
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
                            Quantity = 15
                        },
                        new
                        {
                            Id = 2,
                            Description = "High-performance gaming laptop from Asus",
                            Name = "Asus ROG Strix G16",
                            Price = 2000m,
                            Quantity = 150
                        },
                        new
                        {
                            Id = 3,
                            Description = "Ultra FHD Samsung TV",
                            Name = "Samsung TV",
                            Price = 3200m,
                            Quantity = 200
                        },
                        new
                        {
                            Id = 4,
                            Description = "Premium noise-canceling headphones",
                            Name = "Sony WH-1000XM5",
                            Price = 300m,
                            Quantity = 40
                        },
                        new
                        {
                            Id = 5,
                            Description = "Compact and powerful smart speaker",
                            Name = "Amazon Echo Dot 5th Gen",
                            Price = 150m,
                            Quantity = 15
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

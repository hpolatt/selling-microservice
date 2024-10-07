﻿// <auto-generated />
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CatalogService.Api.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20241007095510_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("catalog_brand_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("catalog_item_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("catalog_type_hilo")
                .IncrementsBy(10);

            modelBuilder.Entity("CatalogService.Api.Core.Domain.CatalogBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "catalog_brand_hilo");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("CatalogBrands", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Brand = "Azure"
                        },
                        new
                        {
                            Id = 2,
                            Brand = ".NET"
                        },
                        new
                        {
                            Id = 3,
                            Brand = "SQL Server"
                        },
                        new
                        {
                            Id = 4,
                            Brand = "Other"
                        },
                        new
                        {
                            Id = 5,
                            Brand = "CatalogBrandTestOne"
                        },
                        new
                        {
                            Id = 6,
                            Brand = "CatalogBrandTestTwo"
                        });
                });

            modelBuilder.Entity("CatalogService.Api.Core.Domain.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "catalog_item_hilo");

                    b.Property<int>("AvailableStock")
                        .HasColumnType("int");

                    b.Property<int>("CatalogBrandId")
                        .HasColumnType("int");

                    b.Property<int>("CatalogTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("OnReorder")
                        .HasColumnType("bit");

                    b.Property<string>("PictureFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CatalogBrandId");

                    b.HasIndex("CatalogTypeId");

                    b.ToTable("CatalogItems", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AvailableStock = 100,
                            CatalogBrandId = 1,
                            CatalogTypeId = 1,
                            Description = ".NET Bot Black Hoodie and more",
                            Name = ".NET Bot Black Hoodie",
                            OnReorder = false,
                            PictureFileName = "1.png",
                            Price = 19.5m
                        },
                        new
                        {
                            Id = 2,
                            AvailableStock = 89,
                            CatalogBrandId = 1,
                            CatalogTypeId = 1,
                            Description = ".NET Black & White Mug",
                            Name = ".NET Black & White Mug",
                            OnReorder = true,
                            PictureFileName = "2.png",
                            Price = 8.50m
                        },
                        new
                        {
                            Id = 3,
                            AvailableStock = 56,
                            CatalogBrandId = 1,
                            CatalogTypeId = 2,
                            Description = "Prism White T-Shirt",
                            Name = "Prism White T-Shirt",
                            OnReorder = false,
                            PictureFileName = "3.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 4,
                            AvailableStock = 120,
                            CatalogBrandId = 1,
                            CatalogTypeId = 1,
                            Description = ".NET Foundation T-shirt",
                            Name = ".NET Foundation T-shirt",
                            OnReorder = false,
                            PictureFileName = "4.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 5,
                            AvailableStock = 55,
                            CatalogBrandId = 2,
                            CatalogTypeId = 3,
                            Description = "Roslyn Red Sheet",
                            Name = "Roslyn Red Sheet",
                            OnReorder = false,
                            PictureFileName = "5.png",
                            Price = 8.5m
                        },
                        new
                        {
                            Id = 6,
                            AvailableStock = 17,
                            CatalogBrandId = 1,
                            CatalogTypeId = 1,
                            Description = ".NET Blue Hoodie",
                            Name = ".NET Blue Hoodie",
                            OnReorder = false,
                            PictureFileName = "6.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 7,
                            AvailableStock = 8,
                            CatalogBrandId = 2,
                            CatalogTypeId = 2,
                            Description = "Roslyn Red T-Shirt",
                            Name = "Roslyn Red T-Shirt",
                            OnReorder = false,
                            PictureFileName = "7.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 8,
                            AvailableStock = 34,
                            CatalogBrandId = 2,
                            CatalogTypeId = 2,
                            Description = "Kudu Purple Hoodie",
                            Name = "Kudu Purple Hoodie",
                            OnReorder = false,
                            PictureFileName = "8.png",
                            Price = 8.5m
                        },
                        new
                        {
                            Id = 9,
                            AvailableStock = 76,
                            CatalogBrandId = 2,
                            CatalogTypeId = 2,
                            Description = "Cup<T> White Mug",
                            Name = "Cup<T> White Mug",
                            OnReorder = false,
                            PictureFileName = "9.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 10,
                            AvailableStock = 11,
                            CatalogBrandId = 1,
                            CatalogTypeId = 3,
                            Description = ".NET Foundation Sheet",
                            Name = ".NET Foundation Sheet",
                            OnReorder = false,
                            PictureFileName = "10.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 11,
                            AvailableStock = 3,
                            CatalogBrandId = 1,
                            CatalogTypeId = 3,
                            Description = "Cup<T> Sheet",
                            Name = "Cup<T> Sheet",
                            OnReorder = false,
                            PictureFileName = "11.png",
                            Price = 8.5m
                        },
                        new
                        {
                            Id = 12,
                            AvailableStock = 0,
                            CatalogBrandId = 2,
                            CatalogTypeId = 2,
                            Description = "Prism White TShirt",
                            Name = "Prism White TShirt",
                            OnReorder = false,
                            PictureFileName = "12.png",
                            Price = 12m
                        },
                        new
                        {
                            Id = 13,
                            AvailableStock = 0,
                            CatalogBrandId = 2,
                            CatalogTypeId = 2,
                            Description = "De los Palotes, pepito",
                            Name = "De los Palotes",
                            OnReorder = false,
                            PictureFileName = "12.png",
                            Price = 12m
                        });
                });

            modelBuilder.Entity("CatalogService.Api.Core.Domain.CatalogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "catalog_type_hilo");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("CatalogTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Mug"
                        },
                        new
                        {
                            Id = 2,
                            Type = "T-Shirt"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Sheet"
                        },
                        new
                        {
                            Id = 4,
                            Type = "USB Memory"
                        },
                        new
                        {
                            Id = 5,
                            Type = "CatalogTypeTestOne"
                        },
                        new
                        {
                            Id = 6,
                            Type = "CatalogTypeTestTwo"
                        });
                });

            modelBuilder.Entity("CatalogService.Api.Core.Domain.CatalogItem", b =>
                {
                    b.HasOne("CatalogService.Api.Core.Domain.CatalogBrand", "CatalogBrand")
                        .WithMany()
                        .HasForeignKey("CatalogBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatalogService.Api.Core.Domain.CatalogType", "CatalogType")
                        .WithMany()
                        .HasForeignKey("CatalogTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatalogBrand");

                    b.Navigation("CatalogType");
                });
#pragma warning restore 612, 618
        }
    }
}

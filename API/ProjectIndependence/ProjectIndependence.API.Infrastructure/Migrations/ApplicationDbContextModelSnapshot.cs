﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectIndependence.API.Infrastructure.Data;

#nullable disable

namespace ProjectIndependence.API.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BillingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("SalesQuotationLineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Tax")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SalesQuotationLineId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("QuotationDate")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("SalesQuotations");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotationLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SalesQuotationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("SalesQuotationId");

                    b.ToTable("GetSalesQuotationLines");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Products.Product", b =>
                {
                    b.HasOne("ProjectIndependence.API.Core.Entities.Sales.SalesQuotationLine", null)
                        .WithMany("Products")
                        .HasForeignKey("SalesQuotationLineId");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotation", b =>
                {
                    b.HasOne("ProjectIndependence.API.Core.Entities.Customers.Customer", "Customer")
                        .WithMany("SalesQuotations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotationLine", b =>
                {
                    b.HasOne("ProjectIndependence.API.Core.Entities.Sales.SalesQuotation", null)
                        .WithMany("SalesQuotationLines")
                        .HasForeignKey("SalesQuotationId");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Customers.Customer", b =>
                {
                    b.Navigation("SalesQuotations");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotation", b =>
                {
                    b.Navigation("SalesQuotationLines");
                });

            modelBuilder.Entity("ProjectIndependence.API.Core.Entities.Sales.SalesQuotationLine", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using LuxeIQ.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LuxeIQ.Migrations
{
    [DbContext(typeof(LuxeIQContext))]
    [Migration("20240222160529_user_table_mobile_and_activated_column_modified")]
    partial class user_table_mobile_and_activated_column_modified
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LuxeIQ.Models.ManufacturerTerritories", b =>
                {
                    b.Property<long>("territoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("territoryId"));

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<long>("manufacturerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("repCode")
                        .HasColumnType("bigint");

                    b.Property<string>("salesAgency")
                        .HasColumnType("text");

                    b.Property<string>("salesRegion")
                        .HasColumnType("text");

                    b.Property<string>("salesTerritory")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("territoryId");

                    b.HasIndex("manufacturerId");

                    b.ToTable("ManufacturerTerritories", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.Manufacturers", b =>
                {
                    b.Property<long>("manufacturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("manufacturerId"));

                    b.Property<string>("address1")
                        .HasColumnType("text");

                    b.Property<string>("address2")
                        .HasColumnType("text");

                    b.Property<string>("businessName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("contactEmail")
                        .HasColumnType("text");

                    b.Property<string>("contactName")
                        .HasColumnType("text");

                    b.Property<string>("corporateAdmin")
                        .HasColumnType("text");

                    b.Property<string>("corporateAdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("otherAdmin")
                        .HasColumnType("text");

                    b.Property<string>("otherAdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<string>("product_attributes")
                        .HasColumnType("text");

                    b.Property<string>("salesAdmin")
                        .HasColumnType("text");

                    b.Property<string>("salesAdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("zipcode")
                        .HasColumnType("text");

                    b.HasKey("manufacturerId");

                    b.ToTable("Manufacturers", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.Products", b =>
                {
                    b.Property<long>("productId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("productId"));

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<long>("manufacturerId")
                        .HasColumnType("bigint");

                    b.Property<string>("productAttributes")
                        .HasColumnType("text");

                    b.Property<string>("tableName")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("productId");

                    b.HasIndex("manufacturerId");

                    b.ToTable("Products", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.SalesRepAgency", b =>
                {
                    b.Property<long>("salesRepAgencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("salesRepAgencyId"));

                    b.Property<string>("address1")
                        .HasColumnType("text");

                    b.Property<string>("address2")
                        .HasColumnType("text");

                    b.Property<string>("administrator")
                        .HasColumnType("text");

                    b.Property<string>("administratorMail")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("salesRepAgencyName")
                        .HasColumnType("text");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<string>("territoryName")
                        .HasColumnType("text");

                    b.Property<long?>("territoryNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("zipcode")
                        .HasColumnType("text");

                    b.HasKey("salesRepAgencyId");

                    b.ToTable("SalesRepAgency", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.Users", b =>
                {
                    b.Property<long>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("userId"));

                    b.Property<long?>("ManufacturerId")
                        .HasColumnType("bigint");

                    b.Property<string>("activated")
                        .HasColumnType("text");

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("mobile")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<long?>("salesRepAgencyId")
                        .HasColumnType("bigint");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("userType")
                        .HasColumnType("text");

                    b.Property<string>("zipCode")
                        .HasColumnType("text");

                    b.HasKey("userId");

                    b.ToTable("Users", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.WholesalerHQ", b =>
                {
                    b.Property<long>("wholesalerHQId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("wholesalerHQId"));

                    b.Property<int?>("accountNo")
                        .HasColumnType("integer");

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("customer")
                        .HasColumnType("text");

                    b.Property<string>("fax")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<int?>("salesRegion")
                        .HasColumnType("integer");

                    b.Property<int?>("salesTerritory")
                        .HasColumnType("integer");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<long>("wholesalerId")
                        .HasColumnType("bigint");

                    b.Property<string>("zipcode")
                        .HasColumnType("text");

                    b.HasKey("wholesalerHQId");

                    b.HasIndex("wholesalerId");

                    b.ToTable("WholesalerHQ", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.WholesalerShowrooms", b =>
                {
                    b.Property<long>("showroomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("showroomId"));

                    b.Property<string>("address1")
                        .HasColumnType("text");

                    b.Property<string>("address2")
                        .HasColumnType("text");

                    b.Property<string>("branchNumber")
                        .HasColumnType("text");

                    b.Property<string>("businessName")
                        .HasColumnType("text");

                    b.Property<string>("buyingMultiplier")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("contactMail")
                        .HasColumnType("text");

                    b.Property<string>("contactName")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("manufacturerAccountNo")
                        .HasColumnType("text");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("salesAgency")
                        .HasColumnType("text");

                    b.Property<string>("salesRep")
                        .HasColumnType("text");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<string>("territoryName")
                        .HasColumnType("text");

                    b.Property<long?>("territoryNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("wholesalerAccountNo")
                        .HasColumnType("text");

                    b.Property<long>("wholesalerId")
                        .HasColumnType("bigint");

                    b.Property<string>("zipcode")
                        .HasColumnType("text");

                    b.HasKey("showroomId");

                    b.HasIndex("wholesalerId");

                    b.ToTable("WholesalerShowrooms", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.Wholesalers", b =>
                {
                    b.Property<long>("wholesalerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("wholesalerId"));

                    b.Property<string>("address1")
                        .HasColumnType("text");

                    b.Property<string>("address2")
                        .HasColumnType("text");

                    b.Property<string>("businessName")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<long>("manufacturerId")
                        .HasColumnType("bigint");

                    b.Property<string>("purchasingMultiplier")
                        .HasColumnType("text");

                    b.Property<string>("state")
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("zipcode")
                        .HasColumnType("text");

                    b.HasKey("wholesalerId");

                    b.HasIndex("manufacturerId");

                    b.ToTable("Wholesalers", "public");
                });

            modelBuilder.Entity("LuxeIQ.Models.ManufacturerTerritories", b =>
                {
                    b.HasOne("LuxeIQ.Models.Manufacturers", "Manufacturers")
                        .WithMany()
                        .HasForeignKey("manufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturers");
                });

            modelBuilder.Entity("LuxeIQ.Models.Products", b =>
                {
                    b.HasOne("LuxeIQ.Models.Manufacturers", "Manufacturers")
                        .WithMany()
                        .HasForeignKey("manufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturers");
                });

            modelBuilder.Entity("LuxeIQ.Models.WholesalerHQ", b =>
                {
                    b.HasOne("LuxeIQ.Models.Wholesalers", "Wholesalers")
                        .WithMany()
                        .HasForeignKey("wholesalerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wholesalers");
                });

            modelBuilder.Entity("LuxeIQ.Models.WholesalerShowrooms", b =>
                {
                    b.HasOne("LuxeIQ.Models.Wholesalers", "Wholesalers")
                        .WithMany()
                        .HasForeignKey("wholesalerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wholesalers");
                });

            modelBuilder.Entity("LuxeIQ.Models.Wholesalers", b =>
                {
                    b.HasOne("LuxeIQ.Models.Manufacturers", "Manufacturers")
                        .WithMany()
                        .HasForeignKey("manufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturers");
                });
#pragma warning restore 612, 618
        }
    }
}
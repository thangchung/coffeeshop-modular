﻿// <auto-generated />
using System;
using CoffeeShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeeShop.Infrastructure.Data.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.5.22302.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoffeeShop.Barista.Domain.BaristaItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("item_name");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer")
                        .HasColumnName("item_type");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time_up");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_barista_orders");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_barista_orders_id");

                    b.ToTable("barista_orders", "barista");
                });

            modelBuilder.Entity("CoffeeShop.Domain.LineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("IsBaristaOrder")
                        .HasColumnType("boolean")
                        .HasColumnName("is_barista_order");

                    b.Property<int>("ItemStatus")
                        .HasColumnType("integer")
                        .HasColumnName("item_status");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer")
                        .HasColumnName("item_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_line_items");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_line_items_id");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_line_items_order_id");

                    b.ToTable("line_items", "order");
                });

            modelBuilder.Entity("CoffeeShop.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("Location")
                        .HasColumnType("integer")
                        .HasColumnName("location");

                    b.Property<Guid>("LoyaltyMemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("loyalty_member_id");

                    b.Property<int>("OrderSource")
                        .HasColumnType("integer")
                        .HasColumnName("order_source");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("integer")
                        .HasColumnName("order_status");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_orders_id");

                    b.ToTable("orders", "order");
                });

            modelBuilder.Entity("CoffeeShop.Kitchen.Domain.KitchenOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("item_name");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer")
                        .HasColumnName("item_type");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time_up");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_kitchen_orders");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_kitchen_orders_id");

                    b.ToTable("kitchen_orders", "kitchen");
                });

            modelBuilder.Entity("CoffeeShop.Domain.LineItem", b =>
                {
                    b.HasOne("CoffeeShop.Domain.Order", "Order")
                        .WithMany("LineItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_line_items_orders_order_temp_id");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("CoffeeShop.Domain.Order", b =>
                {
                    b.Navigation("LineItems");
                });
#pragma warning restore 612, 618
        }
    }
}

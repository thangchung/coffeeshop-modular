using CoffeeShop.Barista.Domain;
using CoffeeShop.Domain;
using CoffeeShop.Kitchen.Domain;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;

namespace CoffeeShop.Infrastructure.Data;

public class MainDbContext : AppDbContextBase
{
    public MainDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BaristaItem> BaristaItems { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<LineItem> LineItems { get; set; } = default!;
    public DbSet<KitchenOrder> KitchenOrders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

        // BaristaItems
        modelBuilder.Entity<BaristaItem>().ToTable("barista_orders", "barista");
        modelBuilder.Entity<BaristaItem>().HasKey(x => x.Id);
        modelBuilder.Entity<BaristaItem>().Property(x => x.Id).HasColumnType("uuid")
            .HasDefaultValueSql(Consts.UuidAlgorithm);

        modelBuilder.Entity<BaristaItem>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

        modelBuilder.Entity<BaristaItem>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<BaristaItem>().Ignore(x => x.DomainEvents);

        modelBuilder.Entity<BaristaItem>().Property(x => x.ItemType).IsRequired();
        modelBuilder.Entity<BaristaItem>().Property(x => x.ItemName).IsRequired();

        // Orders
        modelBuilder.Entity<Order>().ToTable("orders", "order");
        modelBuilder.Entity<Order>().HasKey(x => x.Id);
        modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("uuid")
            .HasDefaultValueSql(Consts.UuidAlgorithm);

        modelBuilder.Entity<Order>().Property(x => x.LoyaltyMemberId).HasColumnType("uuid");
        modelBuilder.Entity<Order>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

        modelBuilder.Entity<Order>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<Order>().Ignore(x => x.DomainEvents);

        modelBuilder.Entity<Order>().Property(x => x.OrderSource).IsRequired();
        modelBuilder.Entity<Order>().Property(x => x.OrderStatus).IsRequired();
        modelBuilder.Entity<Order>().Property(x => x.Location).IsRequired();

        modelBuilder.Entity<LineItem>().ToTable("line_items", "order");
        modelBuilder.Entity<LineItem>().HasKey(x => x.Id);
        modelBuilder.Entity<LineItem>().Property(x => x.Id).HasColumnType("uuid")
            .HasDefaultValueSql(Consts.UuidAlgorithm);

        modelBuilder.Entity<LineItem>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

        modelBuilder.Entity<LineItem>().HasIndex(x => x.Id).IsUnique();

        // KitchenOrders
        modelBuilder.Entity<KitchenOrder>().ToTable("kitchen_orders", "kitchen");
        modelBuilder.Entity<KitchenOrder>().HasKey(x => x.Id);
        modelBuilder.Entity<KitchenOrder>().Property(x => x.Id).HasColumnType("uuid")
            .HasDefaultValueSql(Consts.UuidAlgorithm);

        modelBuilder.Entity<KitchenOrder>().Property(x => x.OrderId).HasColumnType("uuid");
        modelBuilder.Entity<KitchenOrder>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

        modelBuilder.Entity<KitchenOrder>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<KitchenOrder>().Ignore(x => x.DomainEvents);

        modelBuilder.Entity<KitchenOrder>().Property(x => x.ItemType).IsRequired();
        modelBuilder.Entity<KitchenOrder>().Property(x => x.ItemName).IsRequired();

        // relationships
        modelBuilder.Entity<Order>()
            .HasMany(x => x.LineItems);
    }
}

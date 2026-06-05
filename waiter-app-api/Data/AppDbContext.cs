using Microsoft.EntityFrameworkCore;
using WaiterApp.Models;

namespace WaiterApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Icon).IsRequired().HasMaxLength(50);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
            entity.Property(p => p.ImagePath).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Price).HasPrecision(18, 2);

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId);
        });

        // Ingredient
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Icon).IsRequired().HasMaxLength(50);

            entity.HasOne(i => i.Product)
                  .WithMany(p => p.Ingredients)
                  .HasForeignKey(i => i.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Table).IsRequired().HasMaxLength(50);
            entity.Property(o => o.Status).IsRequired().HasMaxLength(20);
            entity.Property(o => o.CreatedAt).IsRequired();

            entity.HasIndex(o => o.CreatedAt);
            entity.HasIndex(o => o.Status);
        });

        // OrderItem (join table com payload)
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => new { oi.OrderId, oi.ProductId });
            entity.Property(oi => oi.Quantity).IsRequired();

            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

using Microsoft.EntityFrameworkCore;
using WebLab2.Entities;

namespace WebLab2.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired(); 

        modelBuilder.Entity<Product>()
            .ToTable(b => b.HasCheckConstraint("CK_Product_Price", "[Price] >= 0.01 AND [Price] <= 10000"));

        modelBuilder.Entity<User>()
            .ToTable(b => b.HasCheckConstraint("CK_User_Email", "[Email] LIKE '%@%.%'"));
    }
}

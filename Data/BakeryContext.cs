using bakery.Models;
using Microsoft.EntityFrameworkCore;
namespace bakery.Data;
public class BakeryContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data source=Bakery.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new Product[]
        {
            new Product(){ Id = 1, Description = "Bangel Recipe", ImageName = "bangel.jfif",Name = "Bangel",Price = 12.99m}
        });
        base.OnModelCreating(modelBuilder);
    }
}
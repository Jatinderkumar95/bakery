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
}
namespace Sales.Api.Data;

using Microsoft.EntityFrameworkCore;
using Sales.Api.Models;

public class SalesDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("sales");
    }

    public static void SeedDatabase()
    {
        var context = new SalesDbContext();

        context.ProductPrices.Add(new ProductPrice { Id = 1, Price = new decimal(1291.61), ProductId = "1" });
        context.ProductPrices.Add(new ProductPrice { Id = 2, Price = new decimal(1697.71), ProductId = "2" });
        context.ProductPrices.Add(new ProductPrice { Id = 3, Price = new decimal(169.11), ProductId = "3" });

        context.SaveChanges();
    }

    public DbSet<ProductPrice> ProductPrices { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }
}
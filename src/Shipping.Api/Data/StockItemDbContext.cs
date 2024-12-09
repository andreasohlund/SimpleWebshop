namespace Shipping.Api.Data;

using Microsoft.EntityFrameworkCore;
using Shipping.Api.Models;

public class StockItemDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("shipping");
    }

    public DbSet<StockItem> StockItems { get; set; }

    public static void SeedDatabase()
    {
        var context = new StockItemDbContext();

        context.StockItems.Add(new StockItem { Id = 1, InStock = true, ProductId = 1 });
        context.StockItems.Add(new StockItem { Id = 2, InStock = true, ProductId = 2 });
        context.StockItems.Add(new StockItem { Id = 3, InStock = false, ProductId = 3 });

        context.SaveChanges();
    }
}
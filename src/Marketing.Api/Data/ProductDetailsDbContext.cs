namespace Marketing.Api.Data;

using Marketing.Api.Models;
using Microsoft.EntityFrameworkCore;

public class ProductDetailsDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("marketing");
    }

    public DbSet<ProductDetails> ProductDetails { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    internal static void SeedDatabase()
    {
        var context = new ProductDetailsDbContext();
        context.ProductDetails.Add(new ProductDetails
        {
            ProductId = "1",
            Name = "Apple iPhone 14 512GB Starlight",
            Description = "Diagonal Size\r\n6.1 \"\r\nInternal Memory Capacity\r\n512 GB\r\nMobile Broadband Generation\r\n5G\r\nWaterprotected/Shockproof\r\nWaterprotected\r\nIP Class\r\nIP68\r\nColour\r\nStarlight",
            ImageUrl = "https://cf-images.dustin.eu/cdn-cgi/image/format=auto,quality=75,width=640,,fit=contain/image/d200001268522/apple-iphone-14-128gb-stj%C3%A4rnglans.jpg"
        });

        context.ProductDetails.Add(new ProductDetails
        {
            ProductId = "2",
            Name = "Galaxy Z Fold4 5G Enterprise Edition \r\n256GB Dual-SIM Black",
            Description = "Diagonal Size\r\n7.6 \"\r\nInternal Memory Capacity\r\n256 GB\r\nMobile Broadband Generation\r\n5G\r\nSIM Card Slot Qty\r\nDual-SIM\r\nWaterprotected/Shockproof\r\nWaterprotected\r\nColour\r\nBlack",
            ImageUrl = "https://cf-images.dustin.eu/cdn-cgi/image/format=auto,quality=75,width=640,,fit=contain/image/d200001001788376/samsung-galaxy-z-fold4-5g-enterprise-edition-256gb-dual-sim-black.png"
        });

        context.ProductDetails.Add(new ProductDetails
        {
            ProductId = "3",
            Name = "NOKIA MOBIRA CITYMAN 200",
            Description = "Brick Mobile Cell Phone Vintage Retro Collectable",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4e/Mobira_Cityman_200.jpg?20080801232340"
        });
            
        context.SaveChanges();
    }
}
﻿namespace Sales.Api.Data
{
    using Microsoft.EntityFrameworkCore;
    using Sales.Api.Models;

    public class SalesDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("sales");
        }

        internal static void SeedDatabase()
        {
            var context = new SalesDbContext();

            context.ProductPrices.Add(new ProductPrice { Id = 1, Price = new decimal(1095.00), ProductId = 1 });
            context.ProductPrices.Add(new ProductPrice { Id = 2, Price = new decimal(949.00), ProductId = 2 });
            context.ProductPrices.Add(new ProductPrice { Id = 3, Price = new decimal(950.00), ProductId = 3 });

            context.SaveChanges();
        }

        public DbSet<ProductPrice> ProductPrices { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
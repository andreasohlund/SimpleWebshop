using ITOps.Shared;
using Shipping.Api;
using Shipping.Api.Data;

Console.Title = "Shipping";

StockItemDbContext.SeedDatabase();

using var host = Host.CreateDefaultBuilder(args)
    .UseEShopNServiceBusEndpoint("Shipping.Api")
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
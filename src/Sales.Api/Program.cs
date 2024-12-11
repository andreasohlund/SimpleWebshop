using ITOps.Shared;
using Sales.Api;
using Sales.Api.Data;

Console.Title = "Sales";

SalesDbContext.SeedDatabase();

using var host = Host.CreateDefaultBuilder(args)
    .UseEShopNServiceBusEndpoint("Sales.Api")
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
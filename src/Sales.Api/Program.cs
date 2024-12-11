using ITOps.Shared;
using Sales.Api;
using Sales.Api.Data;

Console.Title = "Sales";

SalesDbContext.SeedDatabase();

using var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Sales.Api"))
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
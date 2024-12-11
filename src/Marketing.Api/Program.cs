using ITOps.Shared;
using Marketing.Api;
using Marketing.Api.Data;

Console.Title = "Marketing";

ProductDetailsDbContext.SeedDatabase();

using var host = Host.CreateDefaultBuilder(args)
    .UseEShopNServiceBusEndpoint("Marketing.Api")
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
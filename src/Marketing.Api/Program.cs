using ITOps.Shared;
using Marketing.Api;
using Marketing.Api.Data;

Console.Title = "Marketing";

ProductDetailsDbContext.SeedDatabase();

using var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Marketing.Api"))
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
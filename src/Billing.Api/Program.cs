using Billing.Api;
using ITOps.Shared;

Console.Title = "Billing";

using var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Billing.Api"))
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
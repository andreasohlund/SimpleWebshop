using Billing.Api;
using ITOps.Shared;

Console.Title = "Billing";

using var host = Host.CreateDefaultBuilder(args)
    .UseEShopNServiceBusEndpoint("Billing.Api")
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();
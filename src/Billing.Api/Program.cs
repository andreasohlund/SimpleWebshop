using Billing.Api;
using ITOps.Shared;

Console.Title = "Billing";

using var host = CreateHostBuilder(args).Build();
await host.StartAsync();

Console.WriteLine("Press any key to shutdown");
Console.ReadKey();
await host.StopAsync();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Billing.Api"))
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
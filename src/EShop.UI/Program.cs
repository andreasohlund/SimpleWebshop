namespace EShop.UI;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ITOps.Shared;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "WebHost";

        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseNServiceBus(_ => EShopEndpointConfiguration.Create("EShop.UI"))
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
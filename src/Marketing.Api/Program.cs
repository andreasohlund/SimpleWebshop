namespace Marketing.Api;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ITOps.Shared;
using Marketing.Api.Data;

static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Marketing";

        ProductDetailsDbContext.SeedDatabase();

        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        Console.WriteLine("Press any key to shutdown");
        Console.ReadKey();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Marketing.Api"))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
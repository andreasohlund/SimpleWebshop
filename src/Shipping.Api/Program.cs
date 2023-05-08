﻿namespace Shipping.Api;

using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ITOps.Shared;
using Shipping.Api.Data;

static class Program
{
    public static async Task Main(string[] args)
    {
        StockItemDbContext.SeedDatabase();

        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        Console.WriteLine("Press any key to shutdown");
        Console.ReadKey();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseNServiceBus(_ => EShopEndpointConfiguration.Create("Shipping.Api"))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}

namespace Billing.Api;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ITOps.Shared;

static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        Console.WriteLine("Press any key to shutdown");
        Console.ReadKey();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseNServiceBus(c =>
            {
                var endpointConfiguration = new EndpointConfiguration("Billing.Api"); ;

                endpointConfiguration.ApplyCommonNServiceBusConfiguration();

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
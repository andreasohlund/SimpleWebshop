namespace Sales.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using ITOps.Shared;
    using Sales.Api.Data;

    static class Program
    {
        public static async Task Main(string[] args)
        {
            SalesDbContext.SeedDatabase();

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
                    var endpointConfiguration = new EndpointConfiguration("Sales.Api");

                    endpointConfiguration.ApplyCommonNServiceBusConfiguration();

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
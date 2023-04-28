namespace Shipping.Api
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using NServiceBus.MessageMutator;
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
                .UseNServiceBus(c =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Shipping.Api");

                    endpointConfiguration.ApplyCommonNServiceBusConfiguration();

                    // Remove assembly information to be able to reuse message schema from different endpoints w/o sharing messages assembly
                    endpointConfiguration.RegisterMessageMutator(new RemoveAssemblyInfoFromMessageMutator());

                    // Configure saga audit plugin
                    endpointConfiguration.AuditSagaStateChanges("Particular.ServiceControl");
                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}

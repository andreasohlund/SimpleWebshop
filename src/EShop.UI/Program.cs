namespace EShop.UI;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ITOps.Shared;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
          .UseNServiceBus(c =>
          {
              var endpointConfiguration = new EndpointConfiguration("EShop.UI");

              endpointConfiguration.ApplyCommonNServiceBusConfiguration();
              endpointConfiguration.PurgeOnStartup(true);

              return endpointConfiguration;
          }).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
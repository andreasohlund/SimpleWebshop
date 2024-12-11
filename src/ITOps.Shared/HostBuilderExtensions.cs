using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ITOps.Shared;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseEShopNServiceBusEndpoint(this IHostBuilder builder, string endpointName, bool enableMonitoring = true)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.AddFilter("System", LogLevel.Warning);
            logging.AddFilter("Microsoft", LogLevel.Warning);
            logging.AddFilter("NServiceBus", LogLevel.Warning);
        });
        builder.UseNServiceBus(_ => EShopEndpointConfiguration.Create(endpointName, enableMonitoring));

        return builder;
    }
}
namespace ITOps.Shared;

using Marketing.Internal;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Transport;
using Sales.Internal;

public static class EShopEndpointConfiguration
{
    public static EndpointConfiguration Create(string endpointName, bool enableMonitoring = true)
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        // Transport configuration
        var connectionString = Environment.GetEnvironmentVariable("SimpleEShopConnectionString");
        var transport = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));

        //var transport = endpointConfiguration.UseTransport(new LearningTransport());
        ConfigureRouting(transport);
        
        // Persistence Configuration
        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.EnableInstallers();

        // JSON Serializer
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

        if (enableMonitoring)
        {
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            // Enable Metrics Collection and Reporting
            endpointConfiguration
                .EnableMetrics()
                .SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(5));

            // Enable endpoint hearbeat reporting
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl", TimeSpan.FromSeconds(30));

            // Configure saga audit plugin
            endpointConfiguration.AuditSagaStateChanges("Particular.ServiceControl");
        }

        // Remove assembly information to be able to reuse message schema from different endpoints w/o sharing messages assembly
        endpointConfiguration.RegisterMessageMutator(new RemoveAssemblyInfoFromMessageMutator());

        return endpointConfiguration;
    }

    static void ConfigureRouting<T>(RoutingSettings<T> routing)
        where T : TransportDefinition
    {
        routing.RouteToEndpoint(typeof(PlaceOrder), "Sales.Api");
        routing.RouteToEndpoint(typeof(CancelOrder), "Sales.Api");
        routing.RouteToEndpoint(typeof(StoreOrder), "Sales.Api");
        routing.RouteToEndpoint(typeof(AcceptOrder), "Sales.Api");
        routing.RouteToEndpoint(typeof(RecordConsumerBehavior), "Marketing.Api");

        // For transports that do not support publish/subcribe natively, e.g. MSMQ, SqlTransport, call RegisterPublisher
    }
}
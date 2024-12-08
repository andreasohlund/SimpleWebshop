﻿namespace ITOps.Shared;

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
        var transport = endpointConfiguration.UseTransport(new LearningTransport());
        //var transport = endpointConfiguration.UseTransport(new AzureServiceBusTransport(Environment.GetEnvironmentVariable("SimpleEShopConnectionString")));

        ConfigureRouting(transport);

        // Persistence Configuration
        endpointConfiguration.UsePersistence<LearningPersistence>();

        //endpointConfiguration.UsePersistence<AzureTablePersistence>()
        //    .ConnectionString(Environment.GetEnvironmentVariable("SimpleEShopStorageConnectionString"));

        endpointConfiguration.EnableInstallers();

        // JSON Serializer
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        if (enableMonitoring)
        {
            endpointConfiguration.ConnectToServicePlatform(new ServicePlatformConnectionConfiguration
            {
                Heartbeats = new()
                {
                    Enabled = true,
                    HeartbeatsQueue = "Particular.ServiceControl",
                },
                CustomChecks = new()
                {
                    Enabled = true,
                    CustomChecksQueue = "Particular.ServiceControl"
                },
                ErrorQueue = "error",
                SagaAudit = new()
                {
                    Enabled = true,
                    SagaAuditQueue = "audit"
                },
                MessageAudit = new()
                {
                    Enabled = true,
                    AuditQueue = "audit"
                },

                Metrics = new()
                {
                    Enabled = true,
                    MetricsQueue = "Particular.Monitoring",
                    Interval = TimeSpan.FromSeconds(1)
                }
            });
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
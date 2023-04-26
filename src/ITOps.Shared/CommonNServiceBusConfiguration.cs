namespace ITOps.Shared
{
    using Marketing.Internal;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Transport;
    using Sales.Internal;
    using System;

    public static class CommonNServiceBusConfiguration
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CommonNServiceBusConfiguration));

        public static void ApplyCommonNServiceBusConfiguration(this EndpointConfiguration endpointConfiguration, bool enableMonitoring = true)
        {
            // Transport configuration
            var rabbitMqConnectionString = Environment.GetEnvironmentVariable("NetCoreDemoRabbitMQTransport");

            if (string.IsNullOrEmpty(rabbitMqConnectionString))
            {
                log.Info("Using Learning Transport");
                var transport = endpointConfiguration.UseTransport(new LearningTransport());
                ConfigureRouting(transport);
                // Persistence Configuration
                endpointConfiguration.UsePersistence<LearningPersistence>();
            }
            else
            {
                log.Info("Using RabbitMQ Transport");
                var transport = endpointConfiguration.UseTransport(new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), rabbitMqConnectionString));

                ConfigureRouting(transport);

                // Persistence Configuration
                endpointConfiguration.UsePersistence<LearningPersistence>(); //TODO
            }

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
            }
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
}
namespace Shipping.Api.MessageHandlers;

using Shipping.Internal;
using Billing.Events;
using NServiceBus;
using Sales.Events;

public class OrderShipmentSaga(ILogger<OrderShipmentSaga> logger) : Saga<OrderShipmentSaga.State>,
    IAmStartedByMessages<OrderBilled>,
    IAmStartedByMessages<OrderAccepted>,
    IAmStartedByMessages<RegisterShippingDetails>
{
    public Task Handle(RegisterShippingDetails message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Shipping provider for  '{message.OrderId}' has been set to '{message.ShippingOption}'");
        Data.ShippingProvider = message.ShippingOption;
        CompleteSagaIfAllDetailsArePresent();
        return Task.CompletedTask;
    }

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order '{message.OrderId}' has been accepted. Prepare inventory ready for shipping");
        Data.IsOrderAccepted = true;
        CompleteSagaIfAllDetailsArePresent();
        return Task.CompletedTask;
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order '{message.OrderId}' has been billed.");
        Data.IsOrderBilled = true;
        CompleteSagaIfAllDetailsArePresent();
        return Task.CompletedTask;
    }

    void CompleteSagaIfAllDetailsArePresent()
    {
        if (!Data.IsOrderBilled || !Data.IsOrderAccepted || string.IsNullOrEmpty(Data.ShippingProvider))
        {
            return;
        }

        logger.LogInformation(
            $"Order '{Data.OrderId}' is ready to ship via {Data.ShippingProvider} as both OrderAccepted and OrderBilled events has been received.");

        MarkAsComplete();
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderShipmentSaga.State> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId)
            .ToMessage<RegisterShippingDetails>(message => message.OrderId)
            .ToMessage<OrderAccepted>(message => message.OrderId);
    }

    public class State : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderAccepted { get; set; }
        public bool IsOrderBilled { get; set; }
        public string ShippingProvider { get; set; }
    }
}
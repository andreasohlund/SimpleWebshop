namespace Shipping.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using Sales.Events;

public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicy.State>,
    IAmStartedByMessages<OrderBilled>,
    IAmStartedByMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order '{message.OrderId}' has been accepted. Prepare inventory ready for shipping");
        Data.IsOrderAccepted = true;
        CompleteSagaIfBothEventsReceived();
        return Task.CompletedTask;
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order '{message.OrderId}' has been billed.");
        Data.IsOrderBilled = true;
        CompleteSagaIfBothEventsReceived();
        return Task.CompletedTask;
    }

    void CompleteSagaIfBothEventsReceived()
    {
        if (!Data.IsOrderBilled || !Data.IsOrderAccepted)
        {
            return;
        }

        logger.LogInformation(
            $"Order '{Data.OrderId}' is ready to ship as both OrderAccepted and OrderBilled events has been received.");

        MarkAsComplete();
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicy.State> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId)
            .ToMessage<OrderAccepted>(message => message.OrderId);
    }

    public class State : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderAccepted { get; set; }
        public bool IsOrderBilled { get; set; }
    }
}
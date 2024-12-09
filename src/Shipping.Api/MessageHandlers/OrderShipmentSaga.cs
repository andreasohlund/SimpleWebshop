namespace Shipping.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using Sales.Events;

public class OrderShipmentSaga(ILogger<OrderShipmentSaga> logger) : Saga<OrderShipmentSaga.State>,
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

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderShipmentSaga.State> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId)
            .ToMessage<OrderAccepted>(message => message.OrderId);
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

    public class State : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderAccepted { get; set; }
        public bool IsOrderBilled { get; set; }
    }
}
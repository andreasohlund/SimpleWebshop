namespace Shipping.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Events;

public class OrderShipmentSaga : Saga<OrderShipmentSaga.SagaData>,
    IAmStartedByMessages<OrderBilled>,
    IAmStartedByMessages<OrderAccepted>
{
    static readonly ILog log = LogManager.GetLogger<OrderShipmentSaga>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.Info($"Order '{message.OrderId}' has been accepted. Prepare inventory ready for shipping");
        Data.IsOrderAccepted = true;
        CompleteSagaIfBothEventsReceived();
        return Task.CompletedTask;
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        log.Info($"Order '{message.OrderId}' has been billed.");
        Data.IsOrderBilled = true;
        CompleteSagaIfBothEventsReceived();
        return Task.CompletedTask;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderShipmentSaga.SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId)
            .ToMessage<OrderAccepted>(message => message.OrderId);
    }

    public void CompleteSagaIfBothEventsReceived()
    {
        if (Data.IsOrderBilled && Data.IsOrderAccepted)
        {
            log.Info(
                $"Order '{Data.OrderId}' is ready to ship as both OrderAccepted and OrderBilled events has been received.");
            MarkAsComplete();
        }
    }


    public class SagaData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderAccepted { get; set; }
        public bool IsOrderBilled { get; set; }
    }
}
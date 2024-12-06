namespace Billing.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using Sales.Events;

public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) : IHandleMessages<OrderAccepted>
{
    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        // Simulate some work
        await Task.Delay(Random.Shared.Next(25, 50), context.CancellationToken);

        logger.LogInformation($"Order '{message.OrderId}' has been accepted, make sure the payment goes through.");

        await ThisIsntGoingToScaleWell();

        await context.Publish(new OrderBilled
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId
        });
    }

    Task ThisIsntGoingToScaleWell()
    {
        return Task.Delay(Random.Shared.Next(250, 350));
    }
}
namespace Billing.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using Sales.Events;

public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) : IHandleMessages<OrderAccepted>
{
    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order '{message.OrderId}' has been accepted, making sure the payment goes through.");
        
        // simulate performing the payment
        await Task.Delay(Random.Shared.Next(250, 350), context.CancellationToken);

        await context.Publish(new OrderBilled
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId
        });
    }
}
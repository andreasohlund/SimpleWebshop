namespace Billing.Api.MessageHandlers;

using Billing.Events;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Events;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    public OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger)
    {
        this.logger = logger;
    }

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        // Simulate some work
        await Task.Delay(random.Next(25, 50), context.CancellationToken);

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
        return Task.Delay(random.Next(250, 350));
    }

    readonly ILogger logger;

    static readonly Random random = new Random();
}
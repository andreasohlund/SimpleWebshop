namespace Marketing.Api.MessageHandlers;

using Marketing.Api.Data;
using Marketing.Api.Models;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Events;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger, ProductDetailsDbContext dbContext)
    : IHandleMessages<OrderPlaced>
{
    public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Storing what products just got recently bought.");

        await dbContext.OrderDetails.AddAsync(new OrderDetails
        {
            ProductId = message.ProductId,
            OrderId = message.OrderId
        }, context.CancellationToken);

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
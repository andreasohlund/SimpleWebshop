namespace Marketing.Api.MessageHandlers;

using Marketing.Api.Data;
using Marketing.Api.Models;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Events;

public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    readonly ProductDetailsDbContext dbContext;
    readonly ILogger logger;

    public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger, ProductDetailsDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

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
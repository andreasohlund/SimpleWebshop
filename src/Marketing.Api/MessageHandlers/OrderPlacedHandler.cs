namespace Marketing.Api.MessageHandlers;

using Marketing.Api.Data;
using Marketing.Api.Models;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Events;

public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();
    readonly ProductDetailsDbContext dbContext;

    public OrderPlacedHandler(ProductDetailsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        log.Info("Storing what products just got recently bought.");

        await dbContext.OrderDetails.AddAsync(new OrderDetails
        {
            ProductId = message.ProductId,
            OrderId = message.OrderId
        }, context.CancellationToken);

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
namespace Sales.Api.MessageHandlers;

using NServiceBus;
using Sales.Api.Data;
using Sales.Internal;

public class CancelOrderHandler(SalesDbContext dbContext) : IHandleMessages<CancelOrder>
{
    public async Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        // Find Order and update the database.
        var order = dbContext.OrderDetails.First(m => m.OrderId == message.OrderId);
        
        order.IsOrderCancelled = true;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
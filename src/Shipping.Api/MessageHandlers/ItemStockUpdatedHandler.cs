namespace Shipping.Api.MessageHandlers;

using NServiceBus;
using NServiceBus.Logging;
using Shipping.Api.Data;
using Warehouse.Azure;

public class ItemStockUpdatedHandler(ILogger<ItemStockUpdatedHandler> logger, StockItemDbContext dbContext)
    : IHandleMessages<ItemStockUpdated>
{
    public async Task Handle(ItemStockUpdated message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Product Id: '{message.ProductId}', Availability: {message.IsAvailable}");

        var stockItem = dbContext.StockItems.First(x => x.ProductId == message.ProductId);
        stockItem.InStock = message.IsAvailable;

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
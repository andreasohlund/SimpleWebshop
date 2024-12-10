namespace Sales.Api.MessageHandlers;

using NServiceBus;
using Sales.Api.Data;
using Sales.Api.Models;
using Sales.Events;
using Sales.Internal;

public class StoreOrderHandler(SalesDbContext dbContext) : IHandleMessages<StoreOrder>
{
    public async Task Handle(StoreOrder message, IMessageHandlerContext context)
    {
        await dbContext.OrderDetails.AddAsync(new OrderDetail
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId,
            OrderPlacedOn = message.OrderPlacedOn,
            IsOrderAccepted = false,
            Price = GetPriceFor(message.ProductId)
        }, context.CancellationToken);

        await dbContext.SaveChangesAsync(context.CancellationToken);

        // Publish event
        await context.Publish(new OrderPlaced
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId
        });
    }

    decimal GetPriceFor(int productId)
    {
        var price = dbContext.ProductPrices.Where(p => p.ProductId == productId)
            .Select(productPrice => productPrice.Price).First();
        return price;
    }
}
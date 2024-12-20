﻿namespace Sales.Api.MessageHandlers;

using NServiceBus;
using Sales.Api.Data;
using Sales.Events;
using Sales.Internal;

public class AcceptOrderHandler(SalesDbContext dbContext) : IHandleMessages<AcceptOrder>
{
    public async Task Handle(AcceptOrder message, IMessageHandlerContext context)
    {
        // Find Order and update the database.
        var order = dbContext.OrderDetails.First(m => m.OrderId == message.OrderId);
        order.IsOrderAccepted = true;
        await dbContext.SaveChangesAsync(context.CancellationToken);

        // Publish event
        await context.Publish(new OrderAccepted
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId
        });
    }
}
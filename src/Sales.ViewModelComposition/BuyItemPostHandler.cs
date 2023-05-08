namespace Sales.ViewModelComposition;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Sales.Internal;
using ServiceComposer.AspNetCore;

public class BuyItemPostHandler : ICompositionRequestsHandler
{
    readonly IMessageSession session;

    public BuyItemPostHandler(IMessageSession messageSession)
    {
        session = messageSession;
    }

    [HttpPost("/products/buyitem/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var productId = (string)request.HttpContext.GetRouteData().Values["id"];

        var orderId = Guid.NewGuid().ToString();

        await session.Send(new PlaceOrder
        {
            OrderId = "EShop-" + orderId,
            ProductId = int.Parse(productId)
        });
    }
}
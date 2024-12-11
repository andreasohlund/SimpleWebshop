namespace Sales.ViewModelComposition;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Sales.Internal;
using ServiceComposer.AspNetCore;

public class BuyItemPostHandler(IMessageSession messageSession) : ICompositionRequestsHandler
{
    [HttpPost("/products/buyitem/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var productId = (string)request.HttpContext.GetRouteData().Values["id"];

        var orderId = Guid.NewGuid().ToString();

        await messageSession.Send(new PlaceOrder
        {
            OrderId = "EShop-" + orderId,
            ProductId = productId
        });
    }
}
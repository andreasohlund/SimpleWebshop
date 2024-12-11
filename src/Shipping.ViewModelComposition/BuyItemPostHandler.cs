using NServiceBus;
using Shipping.Internal;

namespace Shipping.ViewModelComposition;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceComposer.AspNetCore;

public class BuyItemPostHandler(IMessageSession messageSession) : ICompositionRequestsHandler
{
    [HttpPost("/products/buyitem/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var orderId = request.HttpContext.Request.Form["order-id"];
        var shippingOption = request.HttpContext.Request.Form["shipping-option"];
        
        await messageSession.Send(new RegisterShippingDetails
        {
            OrderId = orderId,
            ShippingOption = shippingOption
        });
    }
}
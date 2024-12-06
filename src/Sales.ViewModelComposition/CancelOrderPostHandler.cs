namespace Sales.ViewModelComposition;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Sales.Internal;
using ServiceComposer.AspNetCore;

public class CancelOrderPostHandler(IMessageSession messageSession) : ICompositionRequestsHandler
{
    [HttpPost("/orders/cancelorder/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var orderId = (string)request.HttpContext.GetRouteData().Values["id"];

        await messageSession.Send(new CancelOrder
        {
            OrderId = orderId
        });
    }
}
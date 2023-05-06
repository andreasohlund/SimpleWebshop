namespace Sales.ViewModelComposition;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Sales.Internal;
using ServiceComposer.AspNetCore;

public class CancelOrderPostHandler : ICompositionRequestsHandler
{
    readonly IMessageSession session;

    public CancelOrderPostHandler(IMessageSession messageSession)
    {
        session = messageSession;
    }

    [HttpPost("/orders/cancelorder/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var orderId = (string)request.HttpContext.GetRouteData().Values["id"];

        await session.Send(new CancelOrder
        {
            OrderId = orderId
        });
    }
}
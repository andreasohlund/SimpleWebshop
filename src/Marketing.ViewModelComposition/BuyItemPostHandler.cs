namespace Marketing.ViewModelComposition;

using Marketing.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
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

        await session.Send(new RecordConsumerBehavior { ProductId = int.Parse(productId) });
    }
}
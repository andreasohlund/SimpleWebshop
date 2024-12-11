using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceComposer.AspNetCore;

namespace Shipping.ViewModelComposition;

public class ShippingOptionsGetHandler : ICompositionRequestsHandler
{
    [HttpGet("/products/details/{id}")]
    public Task Handle(HttpRequest request)
    {
        var vm = request.GetComposedResponseModel();

        vm.ShippingOptions = new List<string> { { "post-mord" }, { "bring" } };
        
        return Task.CompletedTask;
    }
}
namespace Sales.ViewModelComposition;

using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;

public class ProductDetailsGetHandler(HttpClient httpClient) : ICompositionRequestsHandler
{
    [HttpGet("/products/details/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var id = request.HttpContext.GetRouteData().Values["id"];

        var url = $"http://localhost:50687/product/{id}";
        var response = await httpClient.GetAsync(url);

        dynamic productDetails = await response.Content.AsExpando();

        var vm = request.GetComposedResponseModel();

        vm.Price = productDetails.Price;
    }
}
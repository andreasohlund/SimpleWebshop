namespace Marketing.ViewModelComposition;

using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;
using System.Net.Http;

public class ProductDetailsGetHandler : ICompositionRequestsHandler
{
    readonly HttpClient httpClient;

    public ProductDetailsGetHandler(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpGet("/products/details/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var id = request.HttpContext.GetRouteData().Values["id"];

        var url = $"http://localhost:50688/product/{id}";
        var response = await httpClient.GetAsync(url);

        dynamic productDetails = await response.Content.AsExpando();

        var vm = request.GetComposedResponseModel();
        
        vm.ProductName = productDetails.Name;
        vm.ProductDescription = productDetails.Description;
        vm.ProductId = productDetails.ProductId;
        vm.ImageUrl = productDetails.ImageUrl;
    }
}
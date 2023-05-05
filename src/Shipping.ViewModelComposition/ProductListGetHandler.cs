namespace Shipping.ViewModelComposition;

using System.Net.Http;
using ITOps.ViewModelComposition;
using Marketing.Events.ViewModelComposition;
using Microsoft.AspNetCore.Mvc;
using ServiceComposer.AspNetCore;

internal class ProductListGetHandler : ICompositionEventsSubscriber
{
    private readonly HttpClient httpClient;

    public ProductListGetHandler(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpGet("/products")]
    public void Subscribe(ICompositionEventsPublisher publisher)
    {
        publisher.Subscribe<ProductsLoaded>(async (@event, request) =>
        {
            var productIds = string.Join(",", @event.AvailableProductsViewModel.Keys);

            var url = $"http://localhost:50686/product?productIds={productIds}";
            var response = await httpClient.GetAsync(url);

            dynamic[] productStockList = await response.Content.AsExpandoArray();

            foreach (dynamic productStockStatus in productStockList)
            {
                @event.AvailableProductsViewModel[productStockStatus.Id].InStock = productStockStatus.InStock;
            }
        });
    }
}
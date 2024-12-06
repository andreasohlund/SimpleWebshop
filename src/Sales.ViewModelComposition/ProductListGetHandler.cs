namespace Sales.ViewModelComposition;

using ITOps.ViewModelComposition;
using Marketing.Events.ViewModelComposition;
using Microsoft.AspNetCore.Mvc;
using ServiceComposer.AspNetCore;

public class ProductListGetHandler(HttpClient httpClient) : ICompositionEventsSubscriber
{
    [HttpGet("/products")]
    public void Subscribe(ICompositionEventsPublisher publisher)
    {
        publisher.Subscribe<ProductsLoaded>(async (@event, request) =>
        {
            var productIds = string.Join(",", @event.AvailableProductsViewModel.Keys);

            var url = $"http://localhost:50687/product?productIds={productIds}";
            var response = await httpClient.GetAsync(url);

            dynamic[] productPrices = await response.Content.AsExpandoArray();

            foreach (dynamic productPrice in productPrices)
            {
                @event.AvailableProductsViewModel[productPrice.Id].Price = productPrice.Price;
            }
        });
    }
}
namespace Marketing.ViewModelComposition;

using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Mvc;
using Sales.Events.ViewModelComposition;
using ServiceComposer.AspNetCore;

public class OrdersListGetHandler(HttpClient httpClient) : ICompositionEventsSubscriber
{
    [HttpGet("/orders")]
    public void Subscribe(ICompositionEventsPublisher publisher)
    {
        publisher.Subscribe<OrdersLoaded>(async (@event, request) =>
        {
            var orderIds = string.Join(",", @event.OrdersViewModel.Keys);

            var url = $"http://localhost:50688/product/order?orderIds={orderIds}";
            var response = await httpClient.GetAsync(url);
            dynamic productList = await response.Content.AsExpandoArray();

            foreach (dynamic order in @event.OrdersViewModel.Values)
            {
                var product = ((IEnumerable<dynamic>) productList).Single(p => p.ProductId == order.ProductId);

                order.Name = product.Name;
                order.ImageUrl = product.ImageUrl;
            }
        });
    }
}
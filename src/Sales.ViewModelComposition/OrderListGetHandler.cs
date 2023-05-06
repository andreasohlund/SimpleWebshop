using EShop.Messages.ViewModelCompositionEvents;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceComposer.AspNetCore;
using System.Dynamic;

namespace Sales.ViewModelComposition;

class OrderListGetHandler : ICompositionRequestsHandler
{
    readonly HttpClient httpClient;

    public OrderListGetHandler(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpGet("/orders")]
    public async Task Handle(HttpRequest request)
    {
        //invoke Sales back-end API to retrieve the currently placed orders
        var url = $"http://localhost:50687/order/";
        var response = await httpClient.GetAsync(url);

        dynamic orders = await response.Content.AsExpandoArray();
        var ordersViewModel = MapToViewModelDictionary(orders);

        var context = request.GetCompositionContext();
        var vm = request.GetComposedResponseModel();

        await context.RaiseEvent(new OrdersLoaded
        {
            OrdersViewModel = ordersViewModel
        });

        vm.Orders = ordersViewModel.Values;
    }

    IDictionary<dynamic, dynamic> MapToViewModelDictionary(dynamic[] orders)
    {
        var dictionary = new Dictionary<dynamic, dynamic>();

        foreach (var order in orders)
        {
            dynamic orderDetailObject = new ExpandoObject();
            orderDetailObject.OrderId = order.OrderId;
            orderDetailObject.Price = order.Price;
            orderDetailObject.OrderPlacedOn = order.OrderPlacedOn;
            orderDetailObject.IsOrderAccepted = order.IsOrderAccepted;
            orderDetailObject.IsOrderCancelled = order.IsOrderCancelled;
            orderDetailObject.ProductId = order.ProductId;
            dictionary[order.OrderId] = orderDetailObject;
        }

        return dictionary;
    }
}
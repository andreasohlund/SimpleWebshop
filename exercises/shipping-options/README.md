# Add support for shipping options to be selected

## UI

In Details.cshtml add:

### Add dropdown

```
    <p>
        <select name="shipping-option">
            @foreach (var shippingOption in Model.ShippingOptions)
            {
                <option value="@shippingOption">@shippingOption</option>
            }
        </select>
    </p>
```
if the product is in stock

### Populate view model

Add a new reqest handler to the Shipping.ViewModelComposition project:

```
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
```

Run the UI to see that it works.

## Send a command to register shipping options

Teacher: Who should sent this command? What are the downsides of pushing this via an event? Refer to Putting your events on a diet talk

### Add command

1. Add a new class to Shipping.Internal:

```
namespace Shipping.Internal;

public class RegisterShippingDetails : ICommand
{
    public string OrderId { get; set; }
    public string ShippingOption { get; set; }
}
```

1. Add routing to ITOps.Shared, EShopEndpointConfiguration=>ConfigureRouting

```
routing.RouteToEndpoint(typeof(RegisterShippingDetails).Assembly, "Shipping.Api");
```

1. Send the command from a new post handler in Shipping.ViewModelComposition

```
public class BuyItemPostHandler(IMessageSession messageSession) : ICompositionRequestsHandler
{
    [HttpPost("/products/buyitem/{id}")]
    public async Task Handle(HttpRequest request)
    {
        var orderId = request.HttpContext.Request.Form["order-id"];
        var shippingOption = request.HttpContext.Request.Form["shipping-option"];
        
        await messageSession.Send(new RegisterShippingDetails
        {
            OrderId = orderId,
            ShippingOption = shippingOption
        });
    }
}
```

## Make the order id avaiable to all post handlers

### Add it to the view model

In Sales.ViewModelComposition modify the ProductDetailsGetHandler by adding

```
vm.OrderId = Guid.NewGuid().ToString();
```

### Add a form field


In Details.cshtml add:

`<input type="hidden" name="order-id" value="@Model.OrderId"/>`

to the form.

### Get order id from form

Update all BuyItemPostHandler to:

`var orderId = request.HttpContext.Request.Form["order-id"];`

## Handle the command

### Add a new handler to the shipping policy

1. In ShippingPolicy add:

```    
public Task Handle(RegisterShippingDetails message, IMessageHandlerContext context)
{
    logger.LogInformation($"Shipping provider for '{message.OrderId}' has been set to '{message.ShippingOption}'");
    Data.ShippingProvider = message.ShippingOption;
    CompleteSagaIfAllDetailsArePresent();
    return Task.CompletedTask;
}

```

1. Make it start the saga
1. Add the mapping
1. Modify the complete criteria:

```
void CompleteSagaIfAllDetailsArePresent()
{
    if (!Data.IsOrderBilled || !Data.IsOrderAccepted || string.IsNullOrEmpty(Data.ShippingProvider))
    {
        return;
    }

    logger.LogInformation(
        $"Order '{Data.OrderId}' is ready to ship with {Data.ShippingProvider} as both OrderAccepted and OrderBilled events has been received.");

    MarkAsComplete();
}
```
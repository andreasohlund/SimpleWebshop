namespace Shipping.Internal;

public class RegisterShippingDetails :ICommand
{
    public string OrderId { get; set; }
    public string ShippingOption { get; set; }
}
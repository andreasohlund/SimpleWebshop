namespace Sales.Events;

using NServiceBus;

public class OrderAccepted : IEvent
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
}
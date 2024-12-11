namespace Billing.Events;

using NServiceBus;

public class OrderBilled : IEvent
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
}
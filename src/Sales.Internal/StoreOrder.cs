namespace Sales.Internal;

using System;
using NServiceBus;

public class StoreOrder : ICommand
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public DateTime OrderPlacedOn { get; set; }
}
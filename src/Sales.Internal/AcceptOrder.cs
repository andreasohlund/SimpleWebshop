﻿namespace Sales.Internal;

using NServiceBus;

public class AcceptOrder : ICommand
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
}
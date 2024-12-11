﻿namespace Warehouse.Azure
{
    using NServiceBus;

    public class ItemStockUpdated : IEvent
    {
        public string ProductId { get; set; }
        public bool IsAvailable { get; set; }
    }
}

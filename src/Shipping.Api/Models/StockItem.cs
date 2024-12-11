namespace Shipping.Api.Models;

public class StockItem
{
    public int Id { get; set; }
    public string ProductId { get; set; }
    public bool InStock { get; set; }
}
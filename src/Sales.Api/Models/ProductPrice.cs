namespace Sales.Api.Models;

public class ProductPrice
{
    public int Id { get; set; }
    public string ProductId { get; set; }
    public decimal Price { get; set; }
}
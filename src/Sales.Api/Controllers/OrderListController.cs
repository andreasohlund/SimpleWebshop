namespace Sales.Api.Controllers;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sales.Api.Data;

[Route("order")]
public class OrderListController(SalesDbContext context) : Controller
{
    public IActionResult Get()
    {
        var item = context.OrderDetails.OrderByDescending(u => u.OrderPlacedOn).Take(10).ToList();
        return new ObjectResult(item);
    }
}
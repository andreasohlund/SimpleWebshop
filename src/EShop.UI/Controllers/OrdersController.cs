namespace EShop.UI.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("orders")]
public class OrdersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("cancelorder/{id}")]
    public IActionResult CancelOrder(string id)
    {
        return View();
    }
}
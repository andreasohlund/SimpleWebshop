namespace EShop.UI.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("orders")]

public class OrdersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("cancel/{id}")]
    public IActionResult CancelOrder(int id)
    {
        return View();
    }
}
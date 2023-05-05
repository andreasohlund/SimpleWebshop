namespace EShop.UI.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("products")]
public class ProductsController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(int id)
    {
        return View();
    }

    //public IActionResult BuyItem(int id)
    //{
    //    return View();
    //}
}
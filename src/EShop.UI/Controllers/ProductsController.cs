namespace EShop.UI.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        [HttpGet("/products")]
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult BuyItem(int id)
        {
            return View();
        }
    }
}
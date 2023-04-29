namespace EShop.UI.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CancelOrder(int id)
        {
            return View();
        }
    }
}
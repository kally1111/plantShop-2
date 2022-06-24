using Microsoft.AspNetCore.Mvc;


namespace PlantShop.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

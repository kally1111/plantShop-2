using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlantShop.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }
    }
}

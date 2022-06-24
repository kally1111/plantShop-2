using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantShop.DataAccess;
using PlantShop.Models;
using PlantShop.Services;
using System.Linq;


namespace PlantShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService shopService;
        private readonly PlantShopDbContext db;
        public ShopController(IShopService shopService, PlantShopDbContext db)
        {
            this.shopService = shopService;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View( this.shopService.Index());
        }
        [Authorize]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public IActionResult Create(ShopViewModel shop)
        {
            if (ModelState.IsValid)
            {
               this.shopService.Create(shop);
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        public IActionResult Details(int id)
        {
            return View(this.shopService.Details(id));
        }
        [Authorize]

        public IActionResult UpdateData(string searchString)
        {
            ViewData["FilterShop"] = searchString.ToLower();
            return View(this.shopService.UpdateData(searchString));
        }
        [Authorize]

        public IActionResult DetailedInfo(int id)
        {
            return View(this.shopService.DetailedInfo(id));
        }
        [Authorize]

        public IActionResult Change(ShopViewModel shop)
        {
                this.shopService.Change(shop);
                return RedirectToAction(nameof(Index));
          
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
                return View(this.shopService.Delete(id));
        }
        [Authorize]

        public IActionResult ConfermedDelete(int id)
        {
            int employee = this.db.Employees.Where(p => p.ShopId == id).Count();
            if ( employee == 0)
            {
                this.shopService.ConfermedDelete(id);
                return RedirectToAction(nameof(Index));
            }
            this.shopService.DeleteDenied(id);
            return View(nameof(DeleteDenied));
        }
        public IActionResult DeleteDenied(int id)
        {
            return View(this.shopService.DeleteDenied(id));
        }
    }
}

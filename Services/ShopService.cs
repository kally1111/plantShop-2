using Microsoft.AspNetCore.Hosting;
using PlantShop.Data;
using PlantShop.DataAccess;
using PlantShop.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace PlantShop.Services
{
    public class ShopService : IShopService
    {
        private readonly PlantShopDbContext db;
        private readonly IFileService fileService;

        public ShopService(PlantShopDbContext db, IFileService fileService)
        {
            this.db = db;
            this.fileService=  fileService;
        }
        public List<ShopViewModel> Index()
        {
            return db.Shopes.ToList().Where(s => s.Id != 1).Select(s => MapShop(s)).ToList();

        }

        public void Create(ShopViewModel shop)
        {
            db.Add(MapShop(shop));
            db.SaveChanges();
        }

        public ShopViewModel Details(int id)
        {
            return MapShop(this.db.Shopes.FirstOrDefault(s => s.Id == id));
        }
        public List<ShopViewModel> UpdateData(string searchString)
        {

            var query = this.db.Shopes.ToList()
                .Where(s => s.ShopName.Contains(searchString)).Select(s => MapShop(s))
                .OrderBy(s => s.ShopId).ToList();
            return query;
        }
        public ShopViewModel DetailedInfo(int id)
        {

            return MapShop(this.db.Shopes
                .FirstOrDefault(pl => pl.Id == id)
                );
        }
        public void Change(ShopViewModel shop)
        {
            if (shop.Photo != null && shop.PhotoPath != null)
            {
                this.fileService.DeleteFile(shop.PhotoPath);
            }
            string uniqueFileName = this.fileService.Upload(shop.Photo);
            var shopToChange = db.Shopes.FirstOrDefault(x => x.Id == shop.ShopId);
            shopToChange.ShopName = shop.ShopName;
            shopToChange.City = shop.City;
            shopToChange.Address = shop.Address;
            shopToChange.PhotoPath = uniqueFileName;
            shopToChange.PhoneNumber = shop.PhoneNumber;
            shopToChange.Email = shop.Email;

            db.SaveChanges();
        }
        public ShopViewModel Delete(int id)
        {
          
            return MapShop(this.db.Shopes.Include(s => s.ShopPlant).Include(s => s.Employees)
                .FirstOrDefault(s => s.Id == id));
        }
        public void ConfermedDelete(int id)
        { 
           
                var shopToDelete = this.db.Shopes
                    .FirstOrDefault(p => p.Id == id);
            if (shopToDelete.PhotoPath != null)
            {
                this.fileService.DeleteFile(shopToDelete.PhotoPath);
            }
            db.Remove(shopToDelete);
                db.SaveChanges();
        }
        public ShopViewModel DeleteDenied(int id)
        {

            return MapShop(this.db.Shopes
                .FirstOrDefault(s => s.Id == id));
        }
        private ShopViewModel MapShop(Shop shop)
        {
            if (shop == null)
            {
                return new ShopViewModel();
            }
            return new ShopViewModel
            {
                ShopId=shop.Id,
                ShopName = shop.ShopName,
                City = shop.City,
                Address = shop.Address,
                PhoneNumber = shop.PhoneNumber,
                PhotoPath = shop.PhotoPath

            };
        }
        private Shop MapShop(ShopViewModel shop)
        {
            string uniqueFileName = this.fileService.Upload(shop.Photo);
            return new Shop
            {
                Id=shop.ShopId,
                ShopName = shop.ShopName,
                City = shop.City,
                Address = shop.Address,
                PhotoPath = uniqueFileName,
                PhoneNumber = shop.PhoneNumber,
                Email = shop.Email,
                
            };
        }   

    }
}

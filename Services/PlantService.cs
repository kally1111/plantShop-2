using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlantShop.Data;
using PlantShop.DataAccess;
using PlantShop.Models.PlantVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantShop.Services
{
    public class PlantService : IPlantService
    {
        private readonly PlantShopDbContext db;
        private readonly IFileService fileService;
        public PlantService(PlantShopDbContext db, IFileService fileService)
        {
            this.db = db;
            this.fileService = fileService;
        }

        public GetPlantViewModel Get(GetPlantViewModel getPlantViewModel, int page)
        {

            int plantsPerPage = 2;
            int start = (page - 1) * plantsPerPage;
            var query = db.Plants.OrderBy(p => p.Id).Include(s => s.ShopPlant).ThenInclude(sh=>sh.Shop)
                .Skip(start).Take(plantsPerPage);
            var count = db.Plants.Count();
            string order = getPlantViewModel.OrderBy;
            if (getPlantViewModel.OrderBy != null ||
                getPlantViewModel.SortByTypeOfPlant != null || getPlantViewModel.SortByPlaceToPlant != null)
            {
                if(getPlantViewModel.OrderBy != null) { 
                if (getPlantViewModel.SortByPlaceToPlant == null && getPlantViewModel.SortByTypeOfPlant == null)
                {
                    switch (getPlantViewModel.OrderBy)
                    {
                        case "orderByName":
                            query = db.Plants.OrderBy(p => p.PlantName).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop)
                     .Skip(start).Take(plantsPerPage); break;
                        case "orderByPrice":
                            query = db.Plants.OrderBy(p => p.Price).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop)
                     .Skip(start).Take(plantsPerPage); break;

                    }
                }
                else if(getPlantViewModel.SortByPlaceToPlant != null || getPlantViewModel.SortByTypeOfPlant != null)
                {
                    switch (getPlantViewModel.OrderBy)
                    {
                        case "orderByName":
                            query = db.Plants.OrderBy(p => p.PlantName).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop);
                            break;
                        case "orderByPrice":
                            query = db.Plants.OrderBy(p => p.Price).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop);
                            break;

                    }
                    if (getPlantViewModel.SortByPlaceToPlant != null && getPlantViewModel.SortByTypeOfPlant != null)
                    {
                        query = query
                            .Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                            .Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant)
                           .Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                        count = db.Plants.Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                            .Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant).Count();
                    }
                    else if (getPlantViewModel.SortByPlaceToPlant != null)
                    {
                        query = query
                                .Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                               .Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                        count = count = db.Plants.Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                            .Count();

                    }
                    else if (getPlantViewModel.SortByTypeOfPlant != null)
                    {
                        query = query
                                .Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant)
                               .Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                        count = db.Plants
                                .Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant).Count();

                    }
                }
            }
                if(getPlantViewModel.OrderBy == null) 
                { 
                if (
                    getPlantViewModel.SortByTypeOfPlant != null && getPlantViewModel.SortByPlaceToPlant != null)
                {
                    query = db.Plants.Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant)
                            .Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                           .OrderBy(p => p.Id).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                    count = db.Plants.Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant)
                            .Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant).Count();
                }
                else if (getPlantViewModel.SortByTypeOfPlant != null)
                {
                    query = db.Plants.Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant)
                        .OrderBy(p => p.Id).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                    count = db.Plants.Where(p => p.TypeOfPlant == getPlantViewModel.SortByTypeOfPlant).Count();
                }
                else if (getPlantViewModel.SortByPlaceToPlant != null)
                {
                    query = db.Plants.Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant)
                                    .OrderBy(p => p.Id).Include(s => s.ShopPlant).ThenInclude(sh => sh.Shop).Skip(start).Take(plantsPerPage);
                    count = db.Plants.Where(p => p.PlaceToPlant == getPlantViewModel.SortByPlaceToPlant).Count();
                }
            }
            }
            int pageCount;
            if (count != 0)
            {
                pageCount = Convert.ToInt32(Math.Ceiling(count / (double)plantsPerPage));
            }
            else
            {
                pageCount = 1;
            }
            getPlantViewModel.PageCount = pageCount;
            getPlantViewModel.CurrentPage = page;
            getPlantViewModel.Query = query;
            return getPlantViewModel;
        }

        public void Create(CreatePlantViewModel plant)
        {

            db.Add(MapCreatePlant(plant));
            db.SaveChanges();
            foreach (var shop in plant.SelectedShops)
            {

                ShopPlant shopPlant = new ShopPlant();
                shopPlant.PlantId = db.Plants.Max(p => p.Id);
                shopPlant.ShopId = shop;
                db.Add(shopPlant);
                db.SaveChanges();
            }

        }

        public InfoPlantViewModel InfoPlant(int id)
        {
         return  MapInfoPlant(this.db.Plants.Include(x => x.ShopPlant).ThenInclude(sp=>sp.Shop)
                .FirstOrDefault(p => p.Id == id));
        }
       
        public void Change(DetailedInfoPlantViewModel plant)
        {
        
            var plantToDelete = this.db.Plants.Include(x => x.ShopPlant)
               .FirstOrDefault(p => p.Id == plant.PlantId);
            if (plantToDelete.PhotoPath != null)
            {
                this.fileService.DeleteFile(plantToDelete.PhotoPath);
            }
            db.Remove(plantToDelete);
            db.SaveChanges();

            db.Add(MapDetailedInfoPlant(plant));
            db.SaveChanges();
            if (plant.SelectedShops != null)
            {
                foreach (var shop in plant.SelectedShops)
                {

                    ShopPlant shopPlant = new ShopPlant();
                    shopPlant.PlantId = db.Plants.Max(p => p.Id);
                    shopPlant.ShopId = shop;
                    db.Add(shopPlant);
                    db.SaveChanges();
                }
            }
        }

        public List<UpdateDataPVM> UpdateData(string searchString)
        {

            var query = this.db.Plants.Include(x=>x.ShopPlant).ThenInclude(s=>s.Shop).ToList()
                .Where(p => p.PlantName.Contains(searchString)).Select(p => MapUpdatePlant(p))
                .OrderBy(p => p.PlantId).ToList();
            return query;
        }

        public DetailedInfoPlantViewModel DetailedInfo(int id)
        {

            return MapDetailedInfoPlant(this.db.Plants.Include(x => x.ShopPlant).ThenInclude(s => s.Shop)
                .FirstOrDefault(pl => pl.Id == id)
                );
        }
        public InfoPlantViewModel Delete(int id)
        {
            return MapInfoPlant(this.db.Plants.Include(x => x.ShopPlant).ThenInclude(s => s.Shop)
                .FirstOrDefault(p => p.Id == id));
        }
        public void ConfermedDelete(int id)
        {
            var plantToDelete=this.db.Plants.Include(x => x.ShopPlant).ThenInclude(s => s.Shop)
                .FirstOrDefault(p => p.Id ==id);
            if (plantToDelete.PhotoPath != null)
            {
                this.fileService.DeleteFile(plantToDelete.PhotoPath);
            }
            db.Remove(plantToDelete);
            db.SaveChanges();
        }

        public GetByShopPVM GetByShop(int id, int page)
        {
            GetByShopPVM getByShopPVM = new GetByShopPVM();
            int plantsPerPage = 2;
            int pageCount;
            int start = (page - 1) * plantsPerPage;
            var query = db.Shopes.Where(s => s.Id == id).Include(s => s.ShopPlant).ThenInclude(sp => sp.Plant);
           
            getByShopPVM.Query = query;
            return getByShopPVM;
        }
        private UpdateDataPVM MapUpdatePlant(Plant plant)
        {
            if (plant == null)
            {
                return new UpdateDataPVM();
            }
            return new UpdateDataPVM
            {
                PlantId = plant.Id,
                PlantName = plant.PlantName,
                Price = plant.Price,
                PhotoPath = plant.PhotoPath,
                TypeOfPlant = plant.TypeOfPlant,
            
                Shops = plant.ShopPlant.Select(s => s.Shop).ToList(),
            };

        }
        private InfoPlantViewModel MapInfoPlant(Plant plant)
        {

            if (plant == null)
            {
                return new InfoPlantViewModel();
            }
            return new InfoPlantViewModel
            {
                PlantId = plant.Id,
                PlantName = plant.PlantName,
                Price = plant.Price,
                Description = plant.Description,
                PhotoPath = plant.PhotoPath,
                TypeOfPlant = plant.TypeOfPlant,
                PlaceToPlant = plant.PlaceToPlant,
                Shops=plant.ShopPlant.Select(s=>s.Shop).ToList(),


                ShopName = plant.ShopPlant.Select(s => s.Shop.ShopName).ToList()
                 
              };
        }
        private Plant MapCreatePlant(CreatePlantViewModel plant)
        {
            IFormFile fileName = plant.Photo;
            string uniqueFileName = this.fileService.Upload(fileName);

            return new Plant
            {
                PlantName = plant.PlantName,
                Price = plant.Price,
                Description = plant.Description,
                PhotoPath = uniqueFileName,
                TypeOfPlant = plant.TypeOfPlant,
                PlaceToPlant = plant.PlaceToPlant,
            };
        }
        private DetailedInfoPlantViewModel MapDetailedInfoPlant(Plant plant)
        {
            if (plant == null)
            {
                return new DetailedInfoPlantViewModel();
            }
            return new DetailedInfoPlantViewModel
            {
                PlantId = plant.Id,
                PlantName = plant.PlantName,
                Price = plant.Price,
                Description = plant.Description,
                PhotoPath = plant.PhotoPath,
                TypeOfPlant = plant.TypeOfPlant,
                PlaceToPlant = plant.PlaceToPlant,
                Shops = plant.ShopPlant.Select(s => s.Shop).ToList(),
               // ShopName = plant.ShopPlant.Select(s => s.Shop.ShopName).ToList()


            };
        }
        private Plant MapDetailedInfoPlant(DetailedInfoPlantViewModel plant)
        {
            string uniqueFileName = this.fileService.Upload(plant.Photo);

            return new Plant
            {
                PlantName = plant.PlantName,
                Price = plant.Price,
                Description = plant.Description,
                PhotoPath = uniqueFileName,
                TypeOfPlant = plant.TypeOfPlant,
                PlaceToPlant = plant.PlaceToPlant,
            };
        }

    }
}


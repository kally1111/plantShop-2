using PlantShop.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PlantShop.Models.PlantVM
{
    public class GetByShopPVM
    {

        [Display(Name = "Plant Name")]
        public string PlantName { get; set; }
        public double? Price { get; set; }     
        public string ShopName { get; set; }
        public IQueryable<Shop> Query { get; set; }
    }
}

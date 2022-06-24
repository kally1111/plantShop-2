using Microsoft.AspNetCore.Http;
using PlantShop.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlantShop.Models.PlantVM
{
    public class CreatePlantViewModel
    {

     
        [Required]
        [StringLength((20), ErrorMessage = "Name cannot be longer than 20 characters")]
        [Display(Name = "Plant Name")]
        public string PlantName { get; set; }
        public double? Price { get; set; }
        [StringLength((500), ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }
        [Display(Name = "Type of plant")]
        public string TypeOfPlant { get; set; }
        [Display(Name = "Place to plant")]
        public string PlaceToPlant { get; set; }
        public IFormFile Photo { get; set; }
        public List<int> SelectedShops { get; set; }
       
    }
}

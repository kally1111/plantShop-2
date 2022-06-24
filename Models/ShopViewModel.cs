using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PlantShop.Models
{
    public class ShopViewModel
    {
        public int ShopId { get; set; }
        [Display(Name ="Shop Name")]
        public string ShopName { get; set; }
        [StringLength((20), ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^0[7-9][0-9]{8}$", ErrorMessage = "Invalid phone number")]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }
    }
}

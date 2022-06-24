using System.ComponentModel.DataAnnotations.Schema;


namespace PlantShop.Data
{
    public class Customer:People
    {
        [Column(TypeName = "nvarchar(20)")]

        public string City { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string Address { get; set; }
    }
}

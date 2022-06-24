


namespace PlantShop.Data
{
    public class ShopPlant
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}

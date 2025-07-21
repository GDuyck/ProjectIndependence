using ProjectIndependence.API.Core.Entities.Base;

namespace ProjectIndependence.API.Core.Entities.Products
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }
    }
}
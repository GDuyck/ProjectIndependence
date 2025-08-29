using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Core.Entities.Products
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TaxRate Tax { get; internal set; }

        private Product() { }

        public Product(Guid id, string name, decimal price, int tax)
        {
            Id = id;
            Name = name;
            Price = price;
            Tax = new TaxRate(tax);
        }
    }
}
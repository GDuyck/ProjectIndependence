using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Core.Entities.Products
{
    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public TaxRate Tax { get; private set; }

        public Product(string name, decimal price, int tax)
        {
            Name = name;
            Price = price;
            Tax = new TaxRate(tax);
        }
    }
}
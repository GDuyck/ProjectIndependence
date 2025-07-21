using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Products;

namespace ProjectIndependence.API.Core.Entities.Sales
{
    public class SalesQuotationLine : EntityBase
    {
        public ICollection<Product> Products { get; set; } = [];
        public decimal Total { get; set; }
    }
}
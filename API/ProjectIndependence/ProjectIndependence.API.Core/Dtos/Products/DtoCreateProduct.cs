using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Products;

namespace ProjectIndependence.API.Core.Dtos.Products
{
    public record DtoCreateProduct : DtoBase
    {
        // Core info
        public string Name { get; set; }
        public  string ProductCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // Classification for later

        // Pricing
        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }
    }
}
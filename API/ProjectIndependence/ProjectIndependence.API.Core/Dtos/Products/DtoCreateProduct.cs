using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Products;

namespace ProjectIndependence.API.Core.Dtos.Products
{
    public record DtoCreateProduct : DtoBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }
    }
}
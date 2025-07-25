using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Dtos.Products
{
    public record DtoProduct : DtoBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }

        public DtoProduct(Product product)
        {
            Name = product.Name;
            Price = product.Price;
            Tax = product.Tax;
        }
    }
}
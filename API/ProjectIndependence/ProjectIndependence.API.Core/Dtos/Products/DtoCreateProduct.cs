using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Dtos.Products
{
    public record DtoCreateProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }

        public static Product CreateProductEntity(DtoCreateProduct product)
        {
            Product newProduct = new();
            newProduct.Name = product.Name;
            newProduct.Price = product.Price;
            newProduct.Tax = product.Tax;

            return newProduct;
        }
    }
}

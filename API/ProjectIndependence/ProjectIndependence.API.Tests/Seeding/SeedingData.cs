using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Seeding
{
    public static class SeedingData
    {
        public static List<Product> ProductsToSeed()
        {
            return new List<Product>
            {
                new Product(Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"), "Test product 1",20,21),
                new Product(Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),"Test product 2",40,12)
            };
        }
    }
}

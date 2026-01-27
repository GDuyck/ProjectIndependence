using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Tests.Seeding
{
    public static class SeedingData
    {
        public static IEnumerable<Product> ProductsToSeed()
        {
            return new List<Product>
            {
                new Product(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Seed Product A",
                    "SPA-001",
                    "First seeded product",
                    true,
                    19.99m,
                    10.00m,
                    21,
                    100,
                    "Seeder"),

                new Product(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Seed Product B",
                    "SPB-002",
                    "Second seeded product",
                    true,
                    49.50m,
                    25.00m,
                    21,
                    50,
                    "Seeder"),

                new Product(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Seed Product C",
                    "SPC-003",
                    "Third seeded product",
                    false,
                    9.99m,
                    4.00m,
                    6,
                    0,
                    "Seeder")
            };
        }

        public static IEnumerable<ProductPriceChange> ProductPriceChangesToSeed()
        {
            return new List<ProductPriceChange>
            {
                new ProductPriceChange(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    oldRetailPrice: 17.99m,
                    newRetailPrice: 19.99m,
                    oldCostPrice: 9.00m,
                    newCostPrice: 10.00m,
                    reasonForPriceChange: "Annual supplier increase",
                    changedBy: "Seeder"
                ),

                new ProductPriceChange(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    oldRetailPrice: 45.00m,
                    newRetailPrice: 49.50m,
                    oldCostPrice: 22.00m,
                    newCostPrice: 25.00m,
                    reasonForPriceChange: "Repricing to match market",
                    changedBy: "Seeder"
                ),

                new ProductPriceChange(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    oldRetailPrice: 8.99m,
                    newRetailPrice: 9.99m,
                    oldCostPrice: 3.50m,
                    newCostPrice: 4.00m,
                    reasonForPriceChange: "Minor cost adjustment",
                    changedBy: "Seeder"
                )
            };
        }

        public static Product SingleProduct()
        {
            return new Product(
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                "Single Seed",
                "SS-100",
                "Single seeded product",
                true,
                5.00m,
                2.50m,
                21,
                5,
                "Seeder");
        }

        // add seeding date for productpricechanges
        public static async Task SeedProducts(ApplicationDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(ProductsToSeed());
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
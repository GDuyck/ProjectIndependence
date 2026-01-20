using ProjectIndependence.API.Core.Entities.Products;

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
                10,
                5,
                "Seeder");
        }
    }
}
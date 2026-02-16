namespace ProjectIndependence.API.Tests.Integration.Common
{
    public static class PutProductData
    {
        public static IEnumerable<object[]> InvalidPutProductData =>
            new List<object[]>
            {
                new object[]
                {
                    "", // Empty product Name
                    "Another updated product",
                    21
                },
                new object[]
                {
                    "Updated Product Name",
                    "", // Empty description
                    21
                },
                new object[]
                {
                    "Updated Product Name",
                    "Updated product description",
                    18 // Incorrect tax
                }
            };
    }
}
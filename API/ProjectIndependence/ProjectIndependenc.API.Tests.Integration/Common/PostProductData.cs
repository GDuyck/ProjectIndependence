using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    public static class PostProductData
    {
        public static IEnumerable<object[]> InvalidProductData =>
            new List<object[]>
            {
                new object[]
                {
                    "", // Empty product code
                    "Another new product",
                    "This is another new product for this test",
                    true,
                    20m,
                    9m,
                    12,
                    20,
                    "testuser"
                },
                new object[]
                {
                    "NPROD001",
                    "", // Empty product name
                    "Discription here",
                    true,
                    21m,
                    10m,
                    21,
                    25,
                    "Testuser"
                },
                new object[]
                {
                    "NPROD002",
                    "Product Name",
                    "Random product description",
                    true,
                    25m,
                    12m,
                    18, // Incorrect tax
                    30,
                    "Testuser"
                },
                new object[]
                {
                    "NPROD003",
                    "Name of product",
                    "", // Empty description
                    true,
                    30m,
                    15m,
                    6,
                    35,
                    "Testing user"
                }
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Errors
{
    public static class ValidationErrors
    {
        // General
        public const string Name = "A name is required";
        public const string IdsNotMatchingTitle = "ID's don't match";
        public const string IdsNotMatching = "The ID in the URL doesn't match the ID in the body";
        public const string NotFoundTitle = "Not found";

        // Products
        public const string ProductPrice = "The product requires a price";
        public const string ProductPriceNotZero = "The price of the product must be higher than 0";
        public const string ProductTax = "The product must have a tax";
        public const string ProductNotFound = "No product found with the id ";
    }
}

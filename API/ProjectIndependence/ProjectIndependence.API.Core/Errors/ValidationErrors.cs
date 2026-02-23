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
        public const string Description = "You have to give a description";
        public const string IdsNotMatchingTitle = "ID's don't match";
        public const string IdsNotMatching = "The ID in the URL doesn't match the ID in the body";
        public const string NotFoundTitle = "Not found";

        // Products
        public const string ProductCodeEmpty = "The productcode can not be empty";
        public const string ProductCodeLength = "The productcode must be between 3 and 20 characters";
        public const string ProductCodeAlphanumeric = "The productcode can only contain letts, numbers and - or _";
        public const string ProductRetailPrice = "The retail price must be greater than 0";
        public const string ProductRetailPriceBiggerThanCostPrice = "The retail price must be higher than or equal to the cost price";
        public const string ProductCostPrice = "The costprice must be greater than 0";
        public const string ProductCostPriceLesserThan = "The cost price must be lower than or equal to the retail price";
        public const string ProductPriceNotZero = "The price of the product must be higher than 0";
        public const string ProductTax = "The product must have a tax";
        public const string ProductStock = "The stock can not be empty";
        public const string ProductReorderLevel = "The reorder level must be 0 or higher";
        public const string ProductNotFound = "No product found with the id ";

        // Product price history
        public const string ProductPriceChangeReason = "A reason for the price change is required";
        public const string ProductPriceChangeReasonLength = "The reason for the price change cannot exceed 500 characters";
    }
}

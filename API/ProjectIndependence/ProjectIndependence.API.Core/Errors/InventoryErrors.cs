namespace ProjectIndependence.API.Core.Errors
{
    public static class InventoryErrors
    {
        public static readonly Error InsufficientStock =
            new("INV-001", "Insufficient stock available");

        public static readonly Error InvalidQuantity =
            new("INV-002", "Quantity must be greater than 0");

        public static readonly Error QuantityExceedsAvailable =
            new("INV-003", "Quantity exceeds the available stock");

        public static readonly Error QuantityExceedsReserve = 
            new("INV-004", "Quantity exceeds the reserved stock");

        public static readonly Error ProductIdEmpty =
            new("INV-005", "ProductId cannot be empty");

        public static readonly Error NoReference = 
            new("INV-006", "Reference is required for tracking");
    }
}
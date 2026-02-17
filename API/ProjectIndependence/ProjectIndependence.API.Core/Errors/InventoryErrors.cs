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
    }
}
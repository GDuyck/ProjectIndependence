namespace ProjectIndependence.API.Application.Products.Queries.GetProductDetail
{
    public class ProductDetailItem
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string ProductCode { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }

        public decimal RetailPrice { get; init; }
        public decimal CostPrice { get; init; }
        public int Tax { get; init; }

        public int QuantityOnHand { get; init; }
        public int QuantityReserved { get; init; }
        public int AvailableStock { get; init; }
        public int ReorderLevel { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public string CreatedBy { get; init; }
    }
}
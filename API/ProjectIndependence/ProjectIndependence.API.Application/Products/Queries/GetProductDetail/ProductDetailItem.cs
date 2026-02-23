namespace ProjectIndependence.API.Application.Products.Queries.GetProductDetail
{
    public class ProductDetailItem
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int Tax { get; set; }

        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int AvailableStock { get; set; }
        public int ReorderLevel { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
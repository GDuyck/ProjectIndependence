namespace ProjectIndependence.API.Application.Products.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int Tax { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
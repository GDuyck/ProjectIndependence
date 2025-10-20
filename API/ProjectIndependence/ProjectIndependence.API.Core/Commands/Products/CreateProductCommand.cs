namespace ProjectIndependence.API.Core.Commands.Products
{
    public class CreateProductCommand
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int Tax { get; set; }
        public int Stock { get; set; }
        public string CreatedBy { get; set; }
    }
}
namespace ProjectIndependence.API.Core.Commands.Products
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
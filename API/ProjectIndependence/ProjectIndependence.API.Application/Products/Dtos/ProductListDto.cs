namespace ProjectIndependence.API.Application.Products.Dtos
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
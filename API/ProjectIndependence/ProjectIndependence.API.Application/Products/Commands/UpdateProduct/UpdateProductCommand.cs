namespace ProjectIndependence.API.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public int Tax { get; set; }
    }
}
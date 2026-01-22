namespace ProjectIndependence.API.Application.Products.Commands.AdjustProductStock
{
    public class AdjustProductStockCommand
    {
        public Guid Id { get; set; }
        public int QuantityChange { get; set; }
    }
}
using ProjectIndependence.API.Core.Entities.Base;

namespace ProjectIndependence.API.Core.Entities.Products
{
    public class ProductPriceChange : EntityBase
    {
        // Corresponding product
        public Guid ProductId { get; private set; }
        public Product Product { get; set; }

        // Price changes
        public decimal OldRetailPrice { get; private set; }

        public decimal NewRetailPrice { get; private set; }
        public decimal OldCostPrice { get; private set; }
        public decimal NewCostPrice { get; private set; }
        public string ReasonForPriceChange { get; private set; }
        public string ChangedBy { get; private set; }
        public DateTime ChangedAt { get; private set; }

        private ProductPriceChange()
        {
        }

        public ProductPriceChange(
            Guid prodcutId,
            decimal oldRetailPrice,
            decimal newRetailPrice,
            decimal oldCostPrice,
            decimal newCostPrice,
            string reasonForPriceChange,
            string changedBy
            )
        {
            ProductId = prodcutId;
            OldRetailPrice = oldRetailPrice;
            NewRetailPrice = newRetailPrice;
            OldCostPrice = oldCostPrice;
            NewCostPrice = newCostPrice;
            ReasonForPriceChange = reasonForPriceChange;
            ChangedBy = changedBy;
            ChangedAt = DateTime.UtcNow;
        }
    }
}
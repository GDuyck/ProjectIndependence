using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Core.Entities.Inventories
{
    public class Inventory : EntityBase
    {
        public Guid ProductId { get; private set; }
        public int QuantityOnHand { get; private set; }
        public int QuantityReserved { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public int AvailableStock => QuantityOnHand - QuantityReserved;

        public Inventory(Guid productId, int quantityOnHand, string createdBy)
        {
            ProductId = productId;
            QuantityOnHand = quantityOnHand;
            QuantityReserved = 0;
            CreatedAt = DateTime.Now;
            CreatedBy = createdBy;
        }

        public void IncreaseStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            QuantityOnHand += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void DecreaseStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > AvailableStock)
                throw new InventoryException(InventoryErrors.QuantityExceedsAvailable);

            QuantityOnHand -= quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReserveStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > AvailableStock)
                throw new InventoryException(InventoryErrors.QuantityExceedsAvailable);

            QuantityReserved += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReleaseReservedStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > QuantityReserved)
                throw new InventoryException(InventoryErrors.QuantityExceedsReserve);

            QuantityReserved -= quantity;
            UpdatedAt = DateTime.Now;
        }

        private static void EnsurePositiveQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new InventoryException(InventoryErrors.InvalidQuantity);
        }
    }
}
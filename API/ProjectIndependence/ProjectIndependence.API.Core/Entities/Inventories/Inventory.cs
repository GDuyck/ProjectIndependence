using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Enums;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Core.Entities.Inventories
{
    public class Inventory : EntityBase
    {
        private readonly List<InventoryMovement> _movements = new();

        public Guid ProductId { get; private set; }
        public int QuantityOnHand { get; private set; }
        public int QuantityReserved { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public int AvailableStock => QuantityOnHand - QuantityReserved;

        public IReadOnlyCollection<InventoryMovement> Movements => _movements;

        public Inventory(Guid productId, int quantityOnHand, string createdBy)
        {
            EnsurePositiveQuantity(quantityOnHand);

            ProductId = productId;
            QuantityOnHand = quantityOnHand;
            QuantityReserved = 0;
            CreatedAt = DateTime.Now;
            CreatedBy = createdBy;
        }

        public void IncreaseStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            AddMovement(quantity, InventoryMovementType.Purchase, "Stock increase");

            QuantityOnHand += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void DecreaseStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > AvailableStock)
                throw new InventoryException(InventoryErrors.QuantityExceedsAvailable);

            AddMovement(-quantity, InventoryMovementType.Sale, "Stock Decrease");

            QuantityOnHand -= quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReserveStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > AvailableStock)
                throw new InventoryException(InventoryErrors.QuantityExceedsAvailable);

            AddMovement(-quantity, InventoryMovementType.Reservation, "Stock reservation");

            QuantityReserved += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReleaseReservedStock(int quantity)
        {
            EnsurePositiveQuantity(quantity);

            if (quantity > QuantityReserved)
                throw new InventoryException(InventoryErrors.QuantityExceedsReserve);

            AddMovement(quantity, InventoryMovementType.Release, "Release reserved stock");

            QuantityReserved -= quantity;
            UpdatedAt = DateTime.Now;
        }

        private static void EnsurePositiveQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new InventoryException(InventoryErrors.InvalidQuantity);
        }

        private void AddMovement(int quantityChange, InventoryMovementType type, string reference)
        {
            var movement = new InventoryMovement(ProductId, quantityChange, type, reference);
            _movements.Add(movement);
        }
    }
}
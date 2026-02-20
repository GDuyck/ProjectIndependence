using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Enums;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Core.Entities.Inventories
{
    public class InventoryMovement : EntityBase
    {
        public Guid ProductId { get; private set; }
        public Product Product { get; set; } = null!;
        public int QuantityChange { get; private set; }
        public InventoryMovementType Type { get; private set; }
        public string Reference { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private InventoryMovement()
        {
        }

        public InventoryMovement(Guid productId, int quantityChange, InventoryMovementType type, string reference)
        {
            if (productId == Guid.Empty)
                throw new InventoryException(Errors.InventoryErrors.ProductIdEmpty);

            if (string.IsNullOrWhiteSpace(reference))
                throw new InventoryException(Errors.InventoryErrors.NoReference);

            if (quantityChange == 0)
                throw new InventoryException(Errors.InventoryErrors.InvalidQuantity);

            ProductId = productId;
            QuantityChange = quantityChange;
            Type = type;
            Reference = reference;
            CreatedAt = DateTime.Now;
        }
    }
}
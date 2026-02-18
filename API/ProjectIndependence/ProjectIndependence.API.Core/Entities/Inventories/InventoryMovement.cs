using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Core.Entities.Inventories
{
    public class InventoryMovement : EntityBase
    {
        public Guid ProductId { get; private set; }
        public int QuantityChange { get; private set; }
        public InventoryMovementType Type { get; private set; }
        public string Reference { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public InventoryMovement(Guid productId, int quantityChange, InventoryMovementType type, string reference)
        {
            ProductId = productId;
            QuantityChange = quantityChange;
            Type = type;
            Reference = reference;
            CreatedAt = DateTime.Now;
        }
    }
}

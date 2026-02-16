using ProjectIndependence.API.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ProjectIndependence.API.Core.Entities.Inventory
{
    public class Inventory : EntityBase
    {
        public Guid ProductId { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }

        public void IncreaseStock(int quantity)
        {
            QuantityOnHand += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity > QuantityOnHand)
            {
                throw new InvalidOperationException("Cannot decrease stock below zero.");
            }
            QuantityOnHand -= quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReserveStock(int quantity)
        {
            if (quantity > QuantityOnHand - QuantityReserved)
            {
                throw new InvalidOperationException("Not enough stock available to reserve.");
            }
            QuantityReserved += quantity;
            UpdatedAt = DateTime.Now;
        }

        public void ReleaseReservedStock(int quantity)
        {
            if (quantity > QuantityReserved)
            {
                throw new InvalidOperationException("Cannot release more reserved stock than currently reserved.");
            }
            QuantityReserved -= quantity;
            UpdatedAt = DateTime.Now;
        }
    }
}

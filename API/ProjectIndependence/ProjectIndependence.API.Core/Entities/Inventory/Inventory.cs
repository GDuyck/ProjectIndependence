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
    }
}

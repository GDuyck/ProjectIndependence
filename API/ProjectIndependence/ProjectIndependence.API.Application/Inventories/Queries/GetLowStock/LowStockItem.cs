using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Inventories.Queries.GetLowStock
{
    public class LowStockItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int AvailableStock { get; set; }
    }
}

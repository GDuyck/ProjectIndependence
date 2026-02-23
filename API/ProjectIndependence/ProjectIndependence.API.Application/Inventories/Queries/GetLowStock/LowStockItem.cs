using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Inventories.Queries.GetLowStock
{
    public class LowStockItem
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public string ProductCode { get; init; }
        public int QuantityOnHand { get; init; }
        public int QuantityReserved { get; init; }
        public int AvailableStock { get; init; }
        public int ReorderLevel { get; init; }
    }
}

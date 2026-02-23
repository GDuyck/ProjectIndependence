using ProjectIndependence.API.Application.Inventories.Queries.GetLowStock;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Interfaces.Inventories
{
    public interface IInventoryQueries
    {
        Task<IReadOnlyList<LowStockItem>> GetLowStockItemsAsync(CancellationToken cancellationToken = default);

    }
}

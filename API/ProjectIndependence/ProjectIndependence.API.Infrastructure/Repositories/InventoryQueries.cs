using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces.Inventories;
using ProjectIndependence.API.Application.Inventories.Queries.GetLowStock;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Infrastructure.Repositories
{
    public class InventoryQueries : IInventoryQueries
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public InventoryQueries(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IReadOnlyList<LowStockItem>> GetLowStockItemsAsync(CancellationToken cancellationToken = default)
        {
            var lowStockItems = await _applicationDbContext.Inventories
                .AsNoTracking()
                .Where(i => (i.QuantityOnHand - i.QuantityReserved) <= i.ReorderLevel)
                .Join(_applicationDbContext.Products,
                inventory => inventory.ProductId,
                product => product.Id,
                (inventory, product) => new LowStockItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductCode = product.ProductCode,
                    QuantityOnHand = inventory.QuantityOnHand,
                    QuantityReserved = inventory.QuantityReserved,
                    AvailableStock = inventory.QuantityOnHand - inventory.QuantityReserved,
                    ReorderLevel = inventory.ReorderLevel
                })
                .ToListAsync(cancellationToken);
            
            return lowStockItems;
        }
    }
}
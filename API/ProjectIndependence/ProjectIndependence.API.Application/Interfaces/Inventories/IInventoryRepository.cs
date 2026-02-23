using ProjectIndependence.API.Application.Inventories.Queries.GetLowStock;
using ProjectIndependence.API.Core.Entities.Inventories;

namespace ProjectIndependence.API.Application.Interfaces.Inventories
{
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Inventory> GetInventoryByProductIdReadOnlyAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Inventory> CreateInventoryAsync(Inventory inventory, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Inventory>> InventoryListAsync(CancellationToken cancellationToken = default);
        Task<int> GetQuantityOnHandByProductId(Guid productId, CancellationToken cancellationToken = default);
        Task<int> GetAvailableStockByProductId(Guid productId, CancellationToken cancellationToken = default);
        Task<int> GetReservedStockByProductId(Guid productId, CancellationToken cancellationToken = default);
    }
}
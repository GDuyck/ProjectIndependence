using ProjectIndependence.API.Core.Entities.Inventories;

namespace ProjectIndependence.API.Application.Interfaces.Inventories
{
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Inventory> GetInventoryByProductIdReadOnlyAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Inventory> CreateInventoryAsync(Inventory inventory, CancellationToken cancellationToken = default);

        Task<Inventory> UpdateInventoryAsync(Inventory inventory, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Inventory>> InventoryListAsync(CancellationToken cancellationToken = default);
    }
}
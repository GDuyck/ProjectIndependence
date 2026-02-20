using ProjectIndependence.API.Core.Entities.Inventories;

namespace ProjectIndependence.API.Application.Interfaces.Inventories
{
    public interface IInventoryMovementRepository
    {
        Task<IReadOnlyList<InventoryMovement>> GetInventoryMovementListByProductIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
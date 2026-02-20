using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces.Inventories;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Infrastructure.Repositories
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public InventoryMovementRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IReadOnlyList<InventoryMovement>> GetInventoryMovementListByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var inventoryMovementsList = await _applicationDbContext.InventoryMovements
                .AsNoTracking()
                .Where(im => im.ProductId == productId)
                .OrderBy(im => im.CreatedAt)
                .ToListAsync(cancellationToken);

            return inventoryMovementsList;
        }
    }
}

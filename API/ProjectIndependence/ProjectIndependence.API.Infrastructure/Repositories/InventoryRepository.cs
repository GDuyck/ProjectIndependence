using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces.Inventories;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public InventoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Inventory> GetInventoryByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var inventory = await _applicationDbContext.Inventories
                .Include(i => i.Movements)
                .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);

            return inventory;
        }

        public async Task<Inventory> GetInventoryByProductIdReadOnlyAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var inventory = await _applicationDbContext.Inventories
                .AsNoTracking()
                .Include(i => i.Movements)
                .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);

            return inventory;
        }

        public async Task<IReadOnlyList<Inventory>> InventoryListAsync(CancellationToken cancellationToken = default)
        {
            var inventoryList = await _applicationDbContext.Inventories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return inventoryList;
        }
        public async Task<Inventory> CreateInventoryAsync(Inventory inventory, CancellationToken cancellationToken = default)
        {
            var createdInventory = await _applicationDbContext.Inventories
                .AddAsync(inventory, cancellationToken);

            return createdInventory.Entity;
        }
    }
}

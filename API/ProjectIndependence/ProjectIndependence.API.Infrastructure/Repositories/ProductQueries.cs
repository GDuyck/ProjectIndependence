using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Queries.GetProductDetail;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Infrastructure.Repositories
{
    public class ProductQueries : IProductQueries
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductQueries(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDetailItem?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var productDetail = await _applicationDbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Join(_applicationDbContext.Inventories,
                product => product.Id,
                inventory => inventory.ProductId,
                (product, inventory) => new ProductDetailItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    ProductCode = product.ProductCode,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    RetailPrice = product.RetailPrice,
                    CostPrice = product.CostPrice,
                    Tax = product.Tax,
                    QuantityOnHand = inventory.QuantityOnHand,
                    QuantityReserved = inventory.QuantityReserved,
                    AvailableStock = inventory.QuantityOnHand - inventory.QuantityReserved,
                    ReorderLevel = inventory.ReorderLevel,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    CreatedBy = product.CreatedBy
                })
                .FirstOrDefaultAsync(cancellationToken);

            return productDetail;
        }
    }
}

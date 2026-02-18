using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Interfaces.Products
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Product> GetProductByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Product>> GetProductListAsync(CancellationToken cancellationToken);
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken);
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken);
        Task<IReadOnlyList<ProductPriceChange>> GetProductPriceChangeByProductIdListAsync(Guid id, CancellationToken cancellationToken);
        Task<ProductPriceChange> GetProductPriceChangeDetailAsync(Guid id, Guid productId, CancellationToken cancellationToken);

    }
}

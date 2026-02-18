using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Interfaces.Products
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Product>> GetProductListAsync(CancellationToken cancellationToken);
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken);
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductListAsync(CancellationToken cancellationToken)
        {
            var products = await _applicationDbContext.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return products;
        }

        public async Task<Product> GetProductByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product;
        }

        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken)
        {
            await _applicationDbContext.Products.AddAsync(product, cancellationToken);

            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
            _applicationDbContext.Products.Update(product);

            return product;
        }

        public async Task<IReadOnlyList<ProductPriceChange>> GetProductPriceChangeByProductIdListAsync(Guid id, CancellationToken cancellationToken)
        {
            var productPriceChanges = await _applicationDbContext.ProductPriceChanges
                .AsNoTracking()
                .Where(pc => pc.ProductId == id)
                .ToListAsync();

            return productPriceChanges;
        }

        public async Task<ProductPriceChange> GetProductPriceChangeDetailAsync(Guid id, Guid productId, CancellationToken cancellationToken)
        {
            var productPriceChangeDetail = await _applicationDbContext.ProductPriceChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(pc => pc.Id == id & pc.ProductId == productId);

            return productPriceChangeDetail;
        }

        public async Task<bool> ProductExists(Guid id, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Products
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}
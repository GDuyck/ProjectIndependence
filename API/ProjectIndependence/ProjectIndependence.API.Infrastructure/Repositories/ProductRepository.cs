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
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductListAsync(CancellationToken cancellationToken)
        {
            var products = await _applicationDbContext.Products
                .AsNoTracking()
                .ToListAsync();

            return products;
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
    }
}
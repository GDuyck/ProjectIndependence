using Mapster;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Products.Dtos;

namespace ProjectIndependence.API.Core.Products.Queries
{
    public class ProductListQueryHandler : IQueryHandler<ProductListQuery, List<ProductListDto>>
    {
        private readonly IProductRepository _productRepository;

        public ProductListQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductListDto>> HandleAsync(ProductListQuery query, CancellationToken cancellationToken = default)
        {
            var entities = await _productRepository.GetAllAsync();

            var products = entities
                .AsQueryable()
                .ProjectToType<ProductListDto>()
                .ToList();

            return products;
        }
    }
}
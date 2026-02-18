using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Queries.ProductList
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
            var productEnities = await _productRepository.GetProductListAsync(cancellationToken);

            var products = productEnities.Adapt<List<ProductListDto>>();

            return products;
        }
    }
}
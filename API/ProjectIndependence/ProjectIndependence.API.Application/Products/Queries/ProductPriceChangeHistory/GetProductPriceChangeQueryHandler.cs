using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory
{
    public class GetProductPriceChangeQueryHandler : IQueryHandler<GetProductPriceChangeQuery, ProductPriceChangeHistoryDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductPriceChangeQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductPriceChangeHistoryDto> HandleAsync(GetProductPriceChangeQuery query, CancellationToken cancellationToken = default)
        {
            var productPriceChange = await _productRepository.GetProductPriceChangeDetailAsync(query.Id, query.ProductId, cancellationToken);

            var dto = productPriceChange.Adapt<ProductPriceChangeHistoryDto>();

            return dto;
        }
    }
}
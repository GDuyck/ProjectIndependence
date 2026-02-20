using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory
{
    public class GetProductPriceChangeHistoryListQueryHandler : IQueryHandler<GetProductPriceChangeHistoryListQuery, List<ProductPriceChangeHistoryListDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductPriceChangeHistoryListQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductPriceChangeHistoryListDto>> HandleAsync(GetProductPriceChangeHistoryListQuery query, CancellationToken cancellationToken = default)
        {
            var priceChangeHistory = await _productRepository.GetProductPriceChangeByProductIdListAsync(query.ProductId, cancellationToken);

            var dto = priceChangeHistory.Adapt<List<ProductPriceChangeHistoryListDto>>();

            return dto;
        }
    }
}
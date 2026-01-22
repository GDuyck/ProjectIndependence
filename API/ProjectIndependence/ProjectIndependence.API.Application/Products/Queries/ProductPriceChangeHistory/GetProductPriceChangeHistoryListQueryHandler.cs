using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory
{
    internal class GetProductPriceChangeHistoryListQueryHandler : IQueryHandler<GetProductPriceChangeHistoryListQuery, List<ProductPriceChangeHistoryListDto>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetProductPriceChangeHistoryListQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductPriceChangeHistoryListDto>> HandleAsync(GetProductPriceChangeHistoryListQuery query, CancellationToken cancellationToken = default)
        {
            var priceChangeHistory = await _applicationDbContext.ProductPriceChanges
                .AsNoTracking()
                .Where(p => p.ProductId == query.ProductId)
                .ProjectToType<ProductPriceChangeHistoryListDto>()
                .ToListAsync();

            return priceChangeHistory;
        }
    }
}
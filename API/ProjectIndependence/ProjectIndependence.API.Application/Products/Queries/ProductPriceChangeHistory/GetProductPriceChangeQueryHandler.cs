using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory
{
    public class GetProductPriceChangeQueryHandler : IQueryHandler<GetProductPriceChangeQuery, ProductPriceChangeHistoryDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetProductPriceChangeQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductPriceChangeHistoryDto> HandleAsync(GetProductPriceChangeQuery query, CancellationToken cancellationToken = default)
        {
            var productPriceChange = await _applicationDbContext.ProductPriceChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(ppc => ppc.Id == query.Id && ppc.ProductId == query.ProductId);

            var dto = productPriceChange.Adapt<ProductPriceChangeHistoryDto>();

            return dto;
        }
    }
}
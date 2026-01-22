using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetProductByIdQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default)
        {
            var productEnity = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            var productDto = productEnity.Adapt<ProductDto>();

            return productDto;
        }
    }
}
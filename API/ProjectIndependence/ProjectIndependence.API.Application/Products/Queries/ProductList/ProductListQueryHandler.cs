using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Queries.ProductList
{
    public class ProductListQueryHandler : IQueryHandler<ProductListQuery, List<ProductListDto>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductListQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductListDto>> HandleAsync(ProductListQuery query, CancellationToken cancellationToken = default)
        {
            //var entities = await _productRepository.GetAllAsync();

            //var products = entities
            //    .AsQueryable()
            //    .ProjectToType<ProductListDto>()
            //    .ToList();

            var products = await _applicationDbContext.Products
                .AsNoTracking()
                .ProjectToType<ProductListDto>()
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
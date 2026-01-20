using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Exceptions;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

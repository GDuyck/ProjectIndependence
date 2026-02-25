using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Products.Queries.GetProductDetail
{
    public class GetProductDetailByIdQueryHandler : IQueryHandler<GetProductDetailByIdQuery, ProductDetailDto>
    {
        private readonly IProductQueries _productQueries;

        public GetProductDetailByIdQueryHandler(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<ProductDetailDto> HandleAsync(GetProductDetailByIdQuery query, CancellationToken cancellationToken = default)
        {
            var productDetail = await _productQueries.GetProductDetailAsync(query.Id, cancellationToken);

            var productDetailDto = productDetail.Adapt<ProductDetailDto>();

            return productDetailDto;
        }
    }
}

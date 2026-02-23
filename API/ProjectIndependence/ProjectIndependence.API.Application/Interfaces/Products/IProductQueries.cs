using ProjectIndependence.API.Application.Products.Queries.GetProductDetail;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Interfaces.Products
{
    public interface IProductQueries
    {
        Task<ProductDetailItem?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}

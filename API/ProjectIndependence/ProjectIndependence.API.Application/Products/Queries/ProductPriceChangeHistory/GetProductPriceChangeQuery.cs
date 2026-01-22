using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory
{
    public record GetProductPriceChangeQuery(Guid Id, Guid ProductId);
}

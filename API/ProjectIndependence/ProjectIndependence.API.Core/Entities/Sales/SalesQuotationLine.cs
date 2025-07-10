using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Entities.Sales
{
    public class SalesQuotationLine : EntityBase
    {
        public ICollection<Product> Products { get; set; } = [];
        public decimal Total { get; set; }
    }
}

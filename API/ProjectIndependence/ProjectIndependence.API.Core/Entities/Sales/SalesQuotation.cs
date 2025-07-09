using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Entities.Sales
{
    public class SalesQuotation : EntityBase
    {
        public Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public DateOnly QuotationDate { get; set; }
        public required string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<SalesQuotationLine> SalesQuotationLines { get; set; } = [];
    }
}

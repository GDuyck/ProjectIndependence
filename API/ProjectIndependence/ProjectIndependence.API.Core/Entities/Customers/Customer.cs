using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Entities.Customers
{
    public class Customer : EntityBase
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string BillingAddress { get; set; }
        public required string ShippingAddress { get; set; }
        public ICollection<SalesQuotation> SalesQuotations { get; set; } = [];
    }
}

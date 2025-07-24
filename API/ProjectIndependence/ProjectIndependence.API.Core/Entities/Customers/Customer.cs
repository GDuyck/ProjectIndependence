using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Sales;

namespace ProjectIndependence.API.Core.Entities.Customers
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? BillingAddress { get; set; }
        public string? ShippingAddress { get; set; }
        public ICollection<SalesQuotation> SalesQuotations { get; set; } = [];
    }
}
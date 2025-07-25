﻿using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Customers;

namespace ProjectIndependence.API.Core.Entities.Sales
{
    public class SalesQuotation : EntityBase
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateOnly QuotationDate { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<SalesQuotationLine> SalesQuotationLines { get; set; } = [];
    }
}
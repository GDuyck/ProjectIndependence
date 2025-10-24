using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Products.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommand
    {
        public Guid Id { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
    }
}

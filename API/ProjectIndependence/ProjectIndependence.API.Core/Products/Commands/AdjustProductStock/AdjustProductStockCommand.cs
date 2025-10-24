using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Products.Commands.AdjustProductStock
{
    public class AdjustProductStockCommand
    {
        public Guid Id { get; set; }
        public int QuantityChange { get; set; }
    }
}

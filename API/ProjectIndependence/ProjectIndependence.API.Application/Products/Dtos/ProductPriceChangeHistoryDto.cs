using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Application.Products.Dtos
{
    public class ProductPriceChangeHistoryDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal OldRetailPrice { get; set; }
        public decimal NewRetailPrice { get; set; }
        public decimal OldCostPrice { get; set; }
        public decimal NewCostPrice { get; set; }
        public string ReasonForPriceChange { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Sales;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Tests.Servicebuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Sales
{
    internal class SalesQuotationLineRepositoryTest
    {
        private readonly ISalesQuotationLineRepository salesQuotationLineRepository;
        private readonly ServiceProvider serviceProvider;
        private readonly List<SalesQuotationLine> mockSalesQuotationLine;

        public SalesQuotationLineRepositoryTest()
        {
            mockSalesQuotationLine = new List<SalesQuotationLine>
            {
                new SalesQuotationLine
                {
                    Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f611"),
                    Total = 123
                },
                new SalesQuotationLine
                {
                    Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f612"),
                    Total = 1234
                }
            };

            serviceProvider = CreateServiceProvider.CreateProvider(mockSalesQuotationLine);

            salesQuotationLineRepository = serviceProvider.GetRequiredService<ISalesQuotationLineRepository>();
        }
    }
}

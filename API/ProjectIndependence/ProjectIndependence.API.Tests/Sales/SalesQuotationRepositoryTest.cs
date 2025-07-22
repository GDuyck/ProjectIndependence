using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Sales;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Tests.Servicebuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Sales
{
    public class SalesQuotationRepositoryTest
    {
        private readonly ISalesQuotationRepository salesQuotationRepository;
        private readonly ServiceProvider serviceProvider;
        private readonly List<SalesQuotation> mockSalesQuotation;
        public SalesQuotationRepositoryTest()
        {
            mockSalesQuotation = new List<SalesQuotation>
            {
                new SalesQuotation
                {
                    Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f614"),
                    QuotationDate = DateOnly.MinValue,
                    Status = "Being processed",
                    TotalPrice = 2500
                },
                new SalesQuotation
                {
                    Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f615"),
                    QuotationDate = DateOnly.MaxValue,
                    Status = "OK",
                    TotalPrice = 7845
                }
            };

            serviceProvider = CreateServiceProvider.CreateProvider(mockSalesQuotation);

            salesQuotationRepository = serviceProvider.GetRequiredService<ISalesQuotationRepository>();
        }
    }
}

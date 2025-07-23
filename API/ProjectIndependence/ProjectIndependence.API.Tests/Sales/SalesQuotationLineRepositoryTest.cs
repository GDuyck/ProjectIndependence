using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Customers;
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
    public class SalesQuotationLineRepositoryTest
    {
        private readonly ISalesQuotationLineRepository salesQuotationLineRepository;
        private readonly ServiceProvider serviceProvider;
        private readonly List<SalesQuotationLine> mockSalesQuotationLines;

        public SalesQuotationLineRepositoryTest()
        {
            mockSalesQuotationLines = new List<SalesQuotationLine>
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

            serviceProvider = CreateServiceProvider.CreateProvider(mockSalesQuotationLines);

            salesQuotationLineRepository = serviceProvider.GetRequiredService<ISalesQuotationLineRepository>();
        }

        [Fact]
        public async Task SalesQuotationLineRepository_GetAllAsync_ReturnsAllLinesAndIsNotNull()
        {
            // ACT
            var result = await salesQuotationLineRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(mockSalesQuotationLines.Count, result.Count());
        }

        [Fact]
        public async Task SalesQuotationLineRepository_GetById_ReturnsQuotationLineWithAValidIdAsync()
        {
            // ARRANGE
            var testId = mockSalesQuotationLines[0].Id;

            // ACT
            var getByIdResult = await salesQuotationLineRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.NotNull(getByIdResult);
            Assert.Equal(testId, getByIdResult.Id);
        }

        [Fact]
        public async Task SalesQuotationLineRepository_GetById_ReturnsNullWithInvalidId()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed129aaab");

            // ACT
            var result = await salesQuotationLineRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task SalesQuotationLineRepository_AddAsync_AddsLinetAndReturnsTheAddedLine()
        {
            // ARRANGE
            var newSalesQuotationLine = new SalesQuotationLine
            {
                Id = new Guid(),
                Total = 999
            };

            // ACT
            var result = await salesQuotationLineRepository.AddAsync(newSalesQuotationLine);

            // ASSERT
            Assert.Equal(newSalesQuotationLine.Total, result.Total);
            Assert.Equal(newSalesQuotationLine.Id, result.Id);
        }

        [Fact]
        public async Task SalesQuotationLineRepository_UpdateAsync_ReturnQuotationLineWithUpdatedValues()
        {
            // ARRANGE
            decimal newTotal = 1487;

            // ACT
            var customerToUpdate = await salesQuotationLineRepository.GetByIdAsync(mockSalesQuotationLines[0].Id);

            customerToUpdate.Total = newTotal;

            var result = await salesQuotationLineRepository.UpdateAsync(customerToUpdate);

            // ASSERT
            Assert.Equal(newTotal, result.Total);
            Assert.Equal(customerToUpdate.Id, result.Id);
        }

        [Fact]
        public async Task SalesQuotationLineRepository_DeleteAsync_ReturnsTrueIfQuotationLineExistsAndGetsDeleted()
        {
            // ARRANGE
            var testId = mockSalesQuotationLines[0].Id;

            // ACT
            var result = await salesQuotationLineRepository.DeleteAsync(testId);
            var notExistingLine = await salesQuotationLineRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.True(result);
            Assert.Null(notExistingLine);
        }

        [Fact]
        public async Task SalesQuotationLineRepository_DeleteAsync_ReturnsFalseIfQuotationLineDoesNotExist()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa");

            // ACT
            var result = await salesQuotationLineRepository.DeleteAsync(testId);

            // ASSERT
            Assert.False(result);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Sales;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Infrastructure.Repositories.Sales;
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
        private readonly List<SalesQuotation> mockSalesQuotations;
        public SalesQuotationRepositoryTest()
        {
            mockSalesQuotations = new List<SalesQuotation>
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

            serviceProvider = CreateServiceProvider.CreateProvider(mockSalesQuotations);

            salesQuotationRepository = serviceProvider.GetRequiredService<ISalesQuotationRepository>();
        }

        [Fact]
        public async Task SalesQuotationRepository_GetAllAsync_ReturnsAllQuotationsAndIsNotNull()
        {
            // ACT
            var result = await salesQuotationRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(mockSalesQuotations.Count, result.Count());
        }

        [Fact]
        public async Task SalesQuotationRepository_GetById_ReturnsQuotationWithAValidIdAsync()
        {
            // ARRANGE
            var testId = mockSalesQuotations[0].Id;

            // ACT
            var getByIdResult = await salesQuotationRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.NotNull(getByIdResult);
            Assert.Equal(testId, getByIdResult.Id);
        }

        [Fact]
        public async Task SalesQuotationRepository_GetById_ReturnsNullWithInvalidId()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed129aaab");

            // ACT
            var result = await salesQuotationRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task SalesQuotationRepository_AddAsync_AddsQuotationtAndReturnsTheAddedQuotation()
        {
            // ARRANGE
            var newSalesQuotation = new SalesQuotation
            {
                Id = new Guid(),
                TotalPrice = 148795,
                Status = "ok"
            };

            // ACT
            var result = await salesQuotationRepository.AddAsync(newSalesQuotation);

            // ASSERT
            Assert.Equal(newSalesQuotation.TotalPrice, result.TotalPrice);
            Assert.Equal(newSalesQuotation.Id, result.Id);
        }

        [Fact]
        public async Task SalesQuotationRepository_UpdateAsync_ReturnQuotationWithUpdatedValues()
        {
            // ARRANGE
            decimal newTotal = 1487;

            // ACT
            var quotationToUpdate = await salesQuotationRepository.GetByIdAsync(mockSalesQuotations[0].Id);

            quotationToUpdate.TotalPrice = newTotal;

            var result = await salesQuotationRepository.UpdateAsync(quotationToUpdate);

            // ASSERT
            Assert.Equal(newTotal, result.TotalPrice);
            Assert.Equal(quotationToUpdate.Id, result.Id);
        }

        [Fact]
        public async Task SalesQuotationRepository_DeleteAsync_ReturnsTrueIfQuotationExistsAndGetsDeleted()
        {
            // ARRANGE
            var testId = mockSalesQuotations[0].Id;

            // ACT
            var result = await salesQuotationRepository.DeleteAsync(testId);
            var notExistingQuotation = await salesQuotationRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.True(result);
            Assert.Null(notExistingQuotation);
        }

        [Fact]
        public async Task SalesQuotationRepository_DeleteAsync_ReturnsFalseIfQuotationDoesNotExist()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa");

            // ACT
            var result = await salesQuotationRepository.DeleteAsync(testId);

            // ASSERT
            Assert.False(result);
        }
    }
}

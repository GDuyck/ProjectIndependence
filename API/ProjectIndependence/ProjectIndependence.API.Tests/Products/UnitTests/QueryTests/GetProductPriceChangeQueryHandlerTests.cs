using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.QueryTests
{
    public class GetProductPriceChangeQueryHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public GetProductPriceChangeQueryHandlerTests(TestServiceProviderFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task GetProductPriceChangeQueryHandler_WithCorrectIdAndProductId_ReturnsPriceChangeDetailDto()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var productForId = dbContext.Products.FirstOrDefault();

            var productPriceChangeForId = dbContext.ProductPriceChanges
                .FirstOrDefault(p => p.ProductId == productForId.Id);

            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetProductPriceChangeQuery, ProductPriceChangeHistoryDto>>();

            var query = new GetProductPriceChangeQuery(productPriceChangeForId.Id, productForId.Id);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productPriceChangeForId.Id, result.Id);
            Assert.Equal(productForId.Id, result.ProductId);
            Assert.Equal(productPriceChangeForId.ReasonForPriceChange, result.ReasonForPriceChange);
        }
    }
}
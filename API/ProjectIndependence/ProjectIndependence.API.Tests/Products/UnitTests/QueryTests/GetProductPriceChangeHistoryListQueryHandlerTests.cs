using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Application.Products.Queries.ProductPriceChangeHistory;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.QueryTests
{
    public class GetProductPriceChangeHistoryListQueryHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public GetProductPriceChangeHistoryListQueryHandlerTests(TestServiceProviderFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task GetProductPriceChangeHistoryListQuery_WithCorrectProductId_ShowsListOfPriceChangesForProduct()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetProductPriceChangeHistoryListQuery, List<ProductPriceChangeHistoryListDto>>>();

            var productId = SeedingData.ProductsToSeed().FirstOrDefault();

            var priceChangeCount = SeedingData.ProductPriceChangesToSeed().Count(pc => pc.ProductId == productId.Id);

            var query = new GetProductPriceChangeHistoryListQuery(productId.Id);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(priceChangeCount, result.Count);
        }
    }
}
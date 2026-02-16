using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Application.Products.Queries.ProductList;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.QueryTests
{
    public class ProductListQueryTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public ProductListQueryTests(TestServiceProviderFixture fixture)
        {
            _testFixture = fixture;
        }

        [Fact]
        public async Task ProductListQueryHandler_ReturnsAllProducts()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<ProductListQuery, List<ProductListDto>>>();

            var query = new ProductListQuery();

            var productCount = SeedingData.ProductsToSeed().Count();

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productCount, result.Count);
        }
    }
}
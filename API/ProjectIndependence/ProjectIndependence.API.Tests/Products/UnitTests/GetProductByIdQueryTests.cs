using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Application.Products.Queries.GetProductById;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests
{
    public class GetProductByIdQueryTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public GetProductByIdQueryTests(TestServiceProviderFixture fixture)
        {
            _testFixture = fixture;
        }

        [Fact]
        public async Task GetProductByIdQuery_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetProductByIdQuery, ProductDto>>();

            var productToQuery = SeedingData.ProductsToSeed().FirstOrDefault();

            var query = new GetProductByIdQuery(productToQuery.Id);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productToQuery.Id, result.Id);
        }
    }
}
using FluentAssertions;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Inventories
{
    [Collection("IntegrationTests")]
    public class InventoryQueriesTests(SqlServerContainerFixture sqlServerContainerFixture) : IntegrationTestBase(sqlServerContainerFixture)
    {
        [Fact]
        public async Task GetLowStockAsync_ShouldReturnAListOfLowStockItems()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: false);
            var repository = new InventoryQueries(context);

            var product1 = new Product(Guid.NewGuid(), "New Product 1", "NP01", "First Test product", true, 100m, 30m, 21, "Testuser");
            var product2 = new Product(Guid.NewGuid(), "New Product 2", "NP02", "Second Test product", true, 150m, 50m, 21, "Testuser");
            var product3 = new Product(Guid.NewGuid(), "New Product 3", "NP03", "Third Test product", true, 200m, 70m, 21, "Testuser");

            await context.Products.AddRangeAsync(product1, product2, product3);

            var inventory1 = new Inventory(product1.Id, 5, 10, product1.CreatedBy); // Low stock
            var inventory2 = new Inventory(product2.Id, 25, 5, product2.CreatedBy); // Not low stock
            var inventory3 = new Inventory(product3.Id, 5, 5, product3.CreatedBy); // low stock equal to reorder level

            await context.Inventories.AddRangeAsync(inventory1, inventory2, inventory3);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetLowStockItemsAsync(TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);

            var productIds = result.Select(i => i.ProductId).ToList();
            productIds.Should().Contain(product1.Id);
            productIds.Should().Contain(product3.Id);
            productIds.Should().NotContain(product2.Id);

            result.First(r => r.ProductId == product1.Id).ProductName.Should().Be(product1.Name);
        }
    }
}
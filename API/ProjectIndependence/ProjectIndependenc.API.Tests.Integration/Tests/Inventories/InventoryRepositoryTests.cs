using FluentAssertions;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Inventories
{
    [Collection("IntegrationTests")]
    public class InventoryRepositoryTests(SqlServerContainerFixture sqlServerContainerFixture) : IntegrationTestBase(sqlServerContainerFixture)
    {
        [Fact]
        public async Task GetInventoryByProductIdAsync_WithDataInDatabase_ShouldReturnInventory()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryRepository(context);

            var inventory = context.Inventories.First();

            // Act
            var result = await repository.GetInventoryByProductIdAsync(inventory.ProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result!.ProductId.Should().Be(inventory.ProductId);
        }

        [Fact]
        public async Task GetInventoryByProductIdAsync_WithNoDataInDatabase_ShouldReturnNull()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: false);
            var repository = new InventoryRepository(context);

            // Act
            var result = await repository.GetInventoryByProductIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task InventoryListAsync_WithDataInDatabase_ShouldReturnInventoryList()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryRepository(context);

            var inventories = context.Inventories.ToList();

            var firstInventory = inventories.FirstOrDefault();

            // Act
            var result = await repository.InventoryListAsync(TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(inventories.Count);
            result.FirstOrDefault()!.ProductId.Should().Be(firstInventory!.ProductId);
        }

        [Fact]
        public async Task InventoryListAsync_WithNoDataInDatabase_ShouldReturnEmptyList()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: false);
            var repository = new InventoryRepository(context);

            // Act
            var result = await repository.InventoryListAsync(TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Fact]
        public async Task CreateInventoryAsync_WithCorrectInput_ShouldReturnCreatedInventory()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: false);
            var repository = new InventoryRepository(context);

            var newInventory = new Inventory
                (
                    Guid.NewGuid(),
                    10,
                    2,
                    "testUser"
                );

            // Act
            var result = await repository.CreateInventoryAsync(newInventory, TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result!.ProductId.Should().Be(newInventory.ProductId);
            result!.QuantityOnHand.Should().Be(newInventory.QuantityOnHand);
        }

        [Fact]
        public async Task GetQuantityOnHandByProductId_WithValidId_ShouldReturnCorrectQuantity()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryRepository(context);
            var inventory = context.Inventories.First();

            // Act
            var result = await repository.GetQuantityOnHandByProductId(inventory.ProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().Be(inventory.QuantityOnHand);
        }

        [Fact]
        public async Task GetAvailableStockByProductId_WithValidId_ShouldReturnCorrectAvailableStock()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryRepository(context);
            var inventory = context.Inventories.First();

            var availableStock = inventory.QuantityOnHand - inventory.QuantityReserved;

            // Act
            var result = await repository.GetAvailableStockByProductId(inventory.ProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().Be(availableStock);
        }

        [Fact]
        public async Task GetReservedStockByProductId_WithValidId_ShouldReturnCorrectReservedStock()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryRepository(context);
            var inventory = context.Inventories.First();

            // Act
            var result = await repository.GetReservedStockByProductId(inventory.ProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().Be(inventory.QuantityReserved);
        }

        [Fact]
        public async Task GetLowStockAsync_ShouldReturnAListOfLowStockItems() 
        {
            // Arrange
            await using var context = CreateDbContext(seedData: false);
            var repository = new InventoryRepository(context);

            var product1 = new Product(Guid.NewGuid(), "New Product 1", "NP01", "First Test product", true, 100m, 30m, 21, 10, "Testuser");
            var product2 = new Product(Guid.NewGuid(), "New Product 2", "NP02", "Second Test product", true, 150m, 50m, 21, 20, "Testuser");
            var product3 = new Product(Guid.NewGuid(), "New Product 3", "NP03", "Third Test product", true, 200m, 70m, 21, 30, "Testuser");

            await context.Products.AddRangeAsync(product1, product2, product3);

            var inventory1 = new Inventory(product1.Id, 5, 10, product1.CreatedBy); // Low stock
            var inventory2 = new Inventory(product2.Id, 25, 5, product2.CreatedBy); // Not low stock
            var inventory3 = new Inventory(product3.Id, 5, 5, product3.CreatedBy); // low stock equal to reorder level

            await context.Inventories.AddRangeAsync(inventory1, inventory2, inventory3);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetLowStocksAsync(TestContext.Current.CancellationToken);

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
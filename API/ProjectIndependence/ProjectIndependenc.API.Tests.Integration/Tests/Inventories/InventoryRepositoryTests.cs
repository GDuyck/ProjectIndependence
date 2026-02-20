using FluentAssertions;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using System.Net.WebSockets;
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
                    "testUser"
                );

            // Act
            var result = await repository.CreateInventoryAsync(newInventory, TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result!.ProductId.Should().Be(newInventory.ProductId);
            result!.QuantityOnHand.Should().Be(newInventory.QuantityOnHand);
        }
    }
}
using FluentAssertions;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using System;
using System.Collections.Generic;
using System.Text;
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
            var result =  await repository.GetInventoryByProductIdAsync(inventory.ProductId);

            // Assert
            result.Should().NotBeNull();
            result!.ProductId.Should().Be(inventory.ProductId);
        }
    }
}

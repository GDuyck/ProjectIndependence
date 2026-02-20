using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Inventories
{
    [Collection("IntegrationTests")]
    public class InventoryMovementRepositoryTests(SqlServerContainerFixture sqlServerContainerFixture) : IntegrationTestBase(sqlServerContainerFixture)
    {
        [Fact]
        public async Task GetInventoryMovementListByProductIdAsync_WithDataInDatabase_ShouldReturnInventoryMovementList()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryMovementRepository(context);

            var existingInventoryMovement = await context.InventoryMovements
                .AsNoTracking()
                .FirstAsync(cancellationToken: TestContext.Current.CancellationToken);

            var expectedCount = await context.InventoryMovements
                .CountAsync(im => im.ProductId == existingInventoryMovement.ProductId, cancellationToken: TestContext.Current.CancellationToken);

            // Act
            var result = await repository.GetInventoryMovementListByProductIdAsync(existingInventoryMovement.ProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(expectedCount);
            result.Should().OnlyContain(im => im.ProductId == existingInventoryMovement.ProductId);
            result.Should().BeInDescendingOrder(im => im.CreatedAt);
        }

        [Fact]
        public async Task GetInventoryMovementListByProductIdAsync_WithInvalidProductId_ShouldReturnEmptyList()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new InventoryMovementRepository(context);

            // Act
            var result = await repository.GetInventoryMovementListByProductIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

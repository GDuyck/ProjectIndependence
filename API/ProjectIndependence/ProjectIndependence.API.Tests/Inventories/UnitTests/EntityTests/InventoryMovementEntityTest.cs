using FluentAssertions;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Core.Enums;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Tests.Inventories.UnitTests.EntityTests
{
    public class InventoryMovementEntityTest
    {
        [Fact]
        public void InventoryMovementConstructor_WithValidInput_ShouldCreateInventoryMovement()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var quantity = 10;
            var moventType = InventoryMovementType.Purchase;
            var reference = "Test Reference";

            // Act
            var movement = new InventoryMovement(productId, quantity, moventType, reference);

            // Assert
            movement.Should().NotBeNull();
            movement.ProductId.Should().Be(productId);
            movement.QuantityChange.Should().Be(quantity);
            movement.Type.Should().Be(moventType);
            movement.Reference.Should().Be(reference);
        }

        [Fact]
        public void InventoryMovementConstructor_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var quantity = 10;
            var moventType = InventoryMovementType.Purchase;
            var reference = "Test Reference";

            // Act
            var action = () => new InventoryMovement(Guid.Empty, quantity, moventType, reference);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.ProductIdEmpty.Code);
        }

        [Fact]
        public void InventoryMovementConstructor_WithQuantityZero_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var moventType = InventoryMovementType.Purchase;
            var reference = "Test Reference";

            // Act
            var action = () => new InventoryMovement(productId, 0, moventType, reference);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.InvalidQuantity.Code);
        }

        [Fact]
        public void InventoryMovementConstructor_WithEmptyReference_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var quantity = 10;
            var moventType = InventoryMovementType.Purchase;

            // Act
            var action = () => new InventoryMovement(productId, quantity, moventType, string.Empty);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.NoReference.Code);
        }
    }
}
using FluentAssertions;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Tests.Inventories.UnitTests.EntityTests
{
    public class InventoryEntityTest
    {
        [Fact]
        public void Inventory_IncreaseStock_ShouldIncreaseStock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");

            // Act
            inventory.IncreaseStock(5);

            // Assert
            Assert.Equal(15, inventory.QuantityOnHand);
        }

        [Fact]
        public void Inventory_IncreaseStockWithNegativeQuantity_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act
            var action = () => inventory.IncreaseStock(-3);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.InvalidQuantity.Code);
        }

        [Fact]
        public void Inventory_DecreaseStock_ShouldDecreaseStock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");

            // Act
            inventory.DecreaseStock(3);

            // Assert
            Assert.Equal(7, inventory.QuantityOnHand);
        }

        [Fact]
        public void Inventroy_DecreaseStockWithNegativeQuantity_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act
            var action = () => inventory.DecreaseStock(-2);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.InvalidQuantity.Code);
        }

        [Fact]
        public void Inventory_DecreaseStockWithQuantityHigherThanAvailable_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act
            var action = () => inventory.DecreaseStock(15);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.QuantityExceedsAvailable.Code);
        }

        [Fact]
        public void Inventory_Reservestock_ShouldIncreaseReservestock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");

            // Act
            inventory.ReserveStock(4);

            // Assert
            Assert.Equal(4, inventory.QuantityReserved);
        }

        public void Inventory_ReservestockWithNegativeQuantity_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act
            var action = () => inventory.ReserveStock(-1);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.InvalidQuantity.Code);
        }

        [Fact]
        public void Inventory_ReservestockWithQuantityHigherThanAvailable_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act
            var action = () => inventory.ReserveStock(12);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.QuantityExceedsAvailable.Code);
        }

        [Fact]
        public void Inventory_ReleaseReservedStock_ShouldDecreaseReservestock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            inventory.ReserveStock(5);

            // Act
            inventory.ReleaseReservedStock(2);

            // Assert
            Assert.Equal(3, inventory.QuantityReserved);
        }

        [Fact]
        public void Inventory_ReleaseReservedStockWithNegativeQuantity_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            inventory.ReserveStock(5);
            // Act
            var action = () => inventory.ReleaseReservedStock(-2);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.InvalidQuantity.Code);
        }

        [Fact]
        public void Inventory_ReleaseReservedStockWithQuantityHigherThanReserved_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            inventory.ReserveStock(5);
            // Act
            var action = () => inventory.ReleaseReservedStock(7);

            // Assert
            var exception = action.Should().Throw<InventoryException>().Which;
            exception.Code.Should().Be(InventoryErrors.QuantityExceedsReserve.Code);
        }
    }
}
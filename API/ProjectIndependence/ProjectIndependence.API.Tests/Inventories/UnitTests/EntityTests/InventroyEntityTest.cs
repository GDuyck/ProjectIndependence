using ProjectIndependence.API.Core.Entities.Inventories;
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
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.IncreaseStock(-3));
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
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.DecreaseStock(-2));
        }

        [Fact]
        public void Inventory_DecreaseStockWithQuantityHigherThanAvailable_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.DecreaseStock(15));
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
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.ReserveStock(-1));
        }

        [Fact]
        public void Inventory_ReservestockWithQuantityHigherThanAvailable_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.ReserveStock(12));
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
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.ReleaseReservedStock(-2));
        }

        [Fact]
        public void Inventory_ReleaseReservedStockWithQuantityHigherThanReserved_ThrowsException()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          "Tester");
            inventory.ReserveStock(5);
            // Act & Assert
            Assert.Throws<InventoryException>(() => inventory.ReleaseReservedStock(7));
        }
    }
}
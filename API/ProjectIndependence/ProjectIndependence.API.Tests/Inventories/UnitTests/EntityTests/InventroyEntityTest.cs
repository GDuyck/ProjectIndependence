using ProjectIndependence.API.Core.Entities.Inventories;

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
                                          0,
                                          "Tester");

            // Act
            inventory.IncreaseStock(5);

            // Assert
            Assert.Equal(15, inventory.QuantityOnHand);
        }

        [Fact]
        public void Inventory_DecreaseStock_ShouldDecreaseStock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          0,
                                          "Tester");

            // Act
            inventory.DecreaseStock(3);

            // Assert
            Assert.Equal(7, inventory.QuantityOnHand);
        }

        [Fact]
        public void Inventory_Reservestock_ShouldIncreaseReservestock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          0,
                                          "Tester");

            // Act
            inventory.ReserveStock(4);

            // Assert
            Assert.Equal(4, inventory.QuantityReserved);
        }

        [Fact]
        public void Inventory_ReleaseReservedStock_ShouldDecreaseReservestock()
        {
            // Arrange
            var inventory = new Inventory(Guid.NewGuid(),
                                          10,
                                          0,
                                          "Tester");
            inventory.ReserveStock(5);

            // Act
            inventory.ReleaseReservedStock(2);

            // Assert
            Assert.Equal(3, inventory.QuantityReserved);
        }
    }
}
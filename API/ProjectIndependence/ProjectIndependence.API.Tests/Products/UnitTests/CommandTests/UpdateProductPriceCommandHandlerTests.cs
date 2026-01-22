using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.CommandTests
{
    public class UpdateProductPriceCommandHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public UpdateProductPriceCommandHandlerTests(TestServiceProviderFixture fixture)
        {
            _testFixture = fixture;
        }

        [Fact]
        public async Task UpdateProductPriceCommandHandler_WithCorrectInput_GivesUpdatedProductDtoAndCreatesPriceChange()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdateProductPriceCommand, ProductDto>>();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var productToUpdate = SeedingData.ProductsToSeed().FirstOrDefault();

            var command = new UpdateProductPriceCommand
            {
                Id = productToUpdate.Id,
                RetailPrice = 25.99m,
                CostPrice = 15.99m,
                ReasonForPriceChange = "Updated the price for a test",
                UpatedBy = "Tester"
            };

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.RetailPrice, result.RetailPrice);
            Assert.Equal(command.CostPrice, result.CostPrice);
            Assert.NotEqual(productToUpdate.RetailPrice, result.RetailPrice);
            Assert.NotEqual(productToUpdate.CostPrice, result.CostPrice);

            // Assert with DB
            var updatedProductInDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productToUpdate.Id);
            var productPriceChangeInDb = await dbContext.ProductPriceChanges
                .Where(p => p.ProductId == productToUpdate.Id)
                .OrderByDescending(p => p.ChangedAt)
                .FirstOrDefaultAsync();

            Assert.Equal(result.Id, updatedProductInDb.Id);
            Assert.Equal(result.RetailPrice, updatedProductInDb.RetailPrice);
            Assert.Equal(result.Id, productPriceChangeInDb.ProductId);
            Assert.Equal(command.ReasonForPriceChange, productPriceChangeInDb.ReasonForPriceChange);
        }
    }
}
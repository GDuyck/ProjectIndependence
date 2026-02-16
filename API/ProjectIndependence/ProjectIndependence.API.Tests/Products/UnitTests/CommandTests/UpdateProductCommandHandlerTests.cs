using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.CommandTests
{
    public class UpdateProductCommandHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public UpdateProductCommandHandlerTests(TestServiceProviderFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task UpdateProductCommandHandler_WithCorrectInput_ShouldUpdateAndReturnDto()
        {
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdateProductCommand, ProductDto>>();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var productToUpdate = SeedingData.ProductsToSeed().FirstOrDefault();

            var updateCommand = new UpdateProductCommand
            {
                Id = productToUpdate.Id,
                Name = "Updated Product Name",
                Description = "Updated Product Description"
            };

            // Act
            var result = await handler.HandleAsync(updateCommand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateCommand.Id, result.Id);
            Assert.NotEqual(productToUpdate.Name, result.Name);

            // Assert DB
            var productInDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productToUpdate.Id);
            Assert.Equal(updateCommand.Name, productInDb.Name);
            Assert.Equal(updateCommand.Description, productInDb.Description);
        }
    }
}
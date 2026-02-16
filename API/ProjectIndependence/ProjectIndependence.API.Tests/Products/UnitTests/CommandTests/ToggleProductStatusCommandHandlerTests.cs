using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Seeding;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products.UnitTests.CommandTests
{
    public class ToggleProductStatusCommandHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _fixture;

        public ToggleProductStatusCommandHandlerTests(TestServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ProductStatusCommandHandler_WithCorrectId_ChangesProductStatus()
        {
            // Arrange
            using var serviceProvider = _fixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<ToggleProductStatusCommand, ProductDto>>();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var productToChangeStatus = SeedingData.ProductsToSeed().FirstOrDefault();

            var command = new ToggleProductStatusCommand(productToChangeStatus.Id);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(productToChangeStatus.IsActive, result.IsActive);

            // Assert with from database
            var productFromDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productToChangeStatus.Id);
            Assert.Equal(result.IsActive, productFromDb.IsActive);
            Assert.Equal(result.Id, productFromDb.Id);
        }
    }
}
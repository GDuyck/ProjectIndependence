using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products
{
    public class CreateProductCommandHandlerTests : IClassFixture<TestServiceProviderFixture>
    {
        private readonly TestServiceProviderFixture _testFixture;

        public CreateProductCommandHandlerTests(TestServiceProviderFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task CreateProductCommandHandler_WithCorrectInput_ShouldCreateAndReturnDto()
        {
            using var scope = _testFixture.ServiceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateProductCommand, ProductDto>>();

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                ProductCode = "TP001",
                Description = "A test product",
                IsActive = true,
                RetailPrice = 99.99m,
                CostPrice = 50.00m,
                Tax = 21,
                Stock = 10,
                CreatedBy = "Tester"
            };

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
        }
    }
}
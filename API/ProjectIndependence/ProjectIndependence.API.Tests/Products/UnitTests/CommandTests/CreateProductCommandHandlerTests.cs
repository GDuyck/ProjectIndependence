using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Servicebuilder;
using ProjectIndependence.API.Tests.Seeding;

namespace ProjectIndependence.API.Tests.Products.UnitTests.CommandTests
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
            // Arrange
            using var serviceProvider = _testFixture.CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateProductCommand, ProductDto>>();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var singleProduct = SeedingData.SingleProduct();

            var command = new CreateProductCommand
            {
                Name = singleProduct.Name,
                ProductCode = singleProduct.ProductCode,
                Description = singleProduct.Description,
                IsActive = singleProduct.IsActive,
                RetailPrice = singleProduct.RetailPrice,
                CostPrice = singleProduct.CostPrice,
                Tax = singleProduct.Tax,
                Stock = singleProduct.Stock,
                CreatedBy = singleProduct.CreatedBy
            };

            // Act
            var result = await handler.HandleAsync(command);

            // Assert - dto
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);

            var count = await dbContext.Products.CountAsync();

            // Assert - database
            var productInDb = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == singleProduct.ProductCode);
            Assert.Equal(command.Stock, productInDb.Stock);
        }
    }
}
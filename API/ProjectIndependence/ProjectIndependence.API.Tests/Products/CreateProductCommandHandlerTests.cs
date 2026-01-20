using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Extensions;
using Xunit;

namespace ProjectIndependence.API.Tests.Products
{
    public class CreateProductCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCreateAndReturnDto()
        {
            // Register Mapster mappings used by the handler
            MapsterConfig.RegisterMappings();

            // Use unique in-memory database per test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            await using var context = new ApplicationDbContext(options);

            var handler = new CreateProductCommandHandler(context);

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

            var saved = await context.Products.FindAsync(result.Id);
            Assert.NotNull(saved);
            Assert.Equal(command.ProductCode, saved.ProductCode);
        }
    }
}
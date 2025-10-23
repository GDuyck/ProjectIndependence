using FluentAssertions;
using Mapster;
using Moq;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Products.Commands.CreateProduct;

namespace ProjectIndependence.API.Tests.Products
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            Extensions.MapsterConfig.RegisterMappings();

            _mockRepo = new Mock<IProductRepository>();
            _handler = new CreateProductCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldCreateAndReturnDto()
        {
            // ARRANGE
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

            var savedEntity = command.Adapt<Product>();

            _mockRepo
                .Setup(mr => mr.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(savedEntity);

            // ACT
            var result = await _handler.HandleAsync(command);

            // ASSERT
            _mockRepo.Verify(mr => mr.AddAsync(It.IsAny<Product>()), Times.Once());

            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
        }
    }
}
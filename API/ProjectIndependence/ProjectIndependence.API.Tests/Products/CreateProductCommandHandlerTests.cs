//using FluentAssertions;
//using Mapster;
//using Moq;
//using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
//using ProjectIndependence.API.Core.Entities.Products;

//namespace ProjectIndependence.API.Tests.Products
//{
//    public class CreateProductCommandHandlerTests
//    {
//        private readonly CreateProductCommandHandler _handler;

//        public CreateProductCommandHandlerTests()
//        {
//            Extensions.MapsterConfig.RegisterMappings();
//        }

//        [Fact]
//        public async Task HandleAsync_ShouldCreateAndReturnDto()
//        {
//            // ARRANGE
//            var command = new CreateProductCommand
//            {
//                Name = "Test Product",
//                ProductCode = "TP001",
//                Description = "A test product",
//                IsActive = true,
//                RetailPrice = 99.99m,
//                CostPrice = 50.00m,
//                Tax = 21,
//                Stock = 10,
//                CreatedBy = "Tester"
//            };

//            var savedEntity = command.Adapt<Product>();

//            _mockRepo
//                .Setup(mr => mr.AddAsync(It.IsAny<Product>()))
//                .ReturnsAsync(savedEntity);

//            // ACT
//            var result = await _handler.HandleAsync(command);

//            // ASSERT
//            _mockRepo.Verify(mr => mr.AddAsync(It.IsAny<Product>()), Times.Once());

//            result.Should().NotBeNull();
//            result.Name.Should().Be(command.Name);
//        }
//    }
//}
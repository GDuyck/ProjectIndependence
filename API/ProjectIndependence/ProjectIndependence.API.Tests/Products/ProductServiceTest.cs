using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductServiceTest : IClassFixture<TestServiceProviderFixture>
    {
        private readonly Mock<IProductRepository> mockProductRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly List<Product> _products;

        public ProductServiceTest(TestServiceProviderFixture fixture)
        {
            _productService = fixture.ServiceProvider.GetRequiredService<IProductService>();
        }

        [Fact]
        public async Task ProductService_GetByIdAsync_ProvidesProductDtoAndIsNotNullAndIsTypeOfDtoProduct()
        {
            // ARRANGE
            var requestedProductDto =
                    new DtoProduct
                    {
                        Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),
                        Name = "Test product 2",
                        Price = 40,
                        Tax = 12
                    };

            // ACT
            var result = await _productService.GetByIdAsync(requestedProductDto.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(requestedProductDto.Name, result.Name);
            Assert.IsType<DtoProduct>(result);
        }

        [Fact]
        public async Task ProductService_GetByIdAsync_ReturnsNullWithInvalidId()
        {
            // ARRANGE
            var invalidId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed1290122");

            // ACT
            var result = await _productService.GetByIdAsync(invalidId);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task ProductService_GetAllAsync_ReturnsListOfProductDtos()
        {
            // ACT
            var result = await _productService.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<DtoProduct>(result.ToList()[0]);
        }

        [Fact]
        public async Task ProductService_AddAsync_ReturnNewDtoProduct()
        {
            // ARRANGE
            var requestDto = new DtoCreateProduct
            {
                Name = "New product number 3",
                Price = 123,
                Tax = 6
            };

            // ACT
            var result = await _productService.AddAsync(requestDto);

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<DtoProduct>(result);
        }

        [Fact]
        public async Task ProductService_UpdateAsync_ReturnsUpdatedDtoProduct()
        {
            // ARRANGE
            var requestDto = new DtoCreateProduct
            {
                Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"),
                Name = "Test product 1 is now and updated version with new price",
                Price = 200,
                Tax = 21,
            };

            // ACT
            var result = await _productService.UpdateAsync(requestDto);
            // Get by id to check if updates are done
            var getByIdResult = await _productService.GetByIdAsync(requestDto.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<DtoProduct>(result);
            Assert.Equal(requestDto.Name, getByIdResult.Name);
            Assert.Equal(requestDto.Name, result.Name);
            Assert.Equal(requestDto.Price, getByIdResult.Price);
            Assert.Equal(requestDto.Price, result.Price);
        }

        [Fact]
        public async Task ProductService_DeleteAsync_ReturnsTrueWithValidId()
        {
            // ARRANGE
            var validId = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622");

            // ACT
            var result = await _productService.DeleteAsync(validId);

            // ASSERT
            Assert.True(result);
        }

        [Fact]
        public async Task ProductService_DeleteAsync_ReturnsFalseWithInvalidId()
        {
            // ARRANGE
            var invalidId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed1290133");

            // ACT
            var result = await _productService.DeleteAsync(invalidId);

            // ASSERT
            Assert.False(result);
        }
    }
}
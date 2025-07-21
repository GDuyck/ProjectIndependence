using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductRepositoryTest
    {
        private readonly IProductRepository productRepository;
        private readonly ServiceProvider serviceProvider;
        private readonly List<Product> mockProducts;

        public ProductRepositoryTest()
        {
            mockProducts = new List<Product>
            {
                    new Product
                    {
                        Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"),
                        Name = "Test product 1",
                        Price = 20,
                        Tax = 21
                    },
                    new Product
                    {
                        Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),
                        Name = "Test product 2",
                        Price = 40,
                        Tax = 12
                    }
            };

            serviceProvider = CreateServiceProvider.CreateProvider<Product, IProductRepository, ProductRepository>(mockProducts);

            productRepository = serviceProvider.GetRequiredService<IProductRepository>();
        }

        [Fact]
        public async Task ProductRepository_GetAllAsync_ReturnsAllProductInDatabase()
        {
            // ACT
            var result = await productRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(mockProducts.Count, result.Count());
        }

        [Fact]
        public async Task ProductRepository_GetById_ReturnsProductWithAValidIdAsync()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa");

            // ACT
            var getByIdResult = await productRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.NotNull(getByIdResult);
            Assert.Equal(testId, getByIdResult.Id);
        }

        [Fact]
        public async Task ProductRepository_GetById_ReturnsNullWithInvalidId()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab");

            // ACT
            var result = await productRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task ProductRepository_AddAsync_AddProductAndReturnsTheAddedProduct()
        {
            // ARRANGE
            var newProduct = new Product
            {
                Id = new Guid(),
                Name = "New Product 3",
                Price = 24,
                Tax = 6
            };

            // ACT
            var result = await productRepository.AddAsync(newProduct);

            // ASSERT
            Assert.Equal(newProduct.Name, result.Name);
            Assert.Equal(newProduct.Id, result.Id);
        }

        [Fact]
        public async Task ProductRepository_UpdateAsync_ReturnProductWithUpdatedValues()
        {
            // ARRANGE
            string newProductName = "Product formerly known as Test Product 1";

            // ACT
            var productToUpdate = await productRepository.GetByIdAsync(Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"));

            productToUpdate.Name = newProductName;

            var result = await productRepository.UpdateAsync(productToUpdate);

            // ASSERT
            Assert.Equal(newProductName, result.Name);
            Assert.Equal(productToUpdate.Id, result.Id);
        }

        [Fact]
        public async Task ProductRepository_DeleteAsync_ReturnsTrueIfProductExistsAndGetsDeleted()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa");

            // ACT
            var result = await productRepository.DeleteAsync(testId);

            // ASSERT
            Assert.True(result);
        }

        [Fact]
        public async Task ProductRepository_DeleteAsync_ReturnsFalseIfProductDoesNotExist()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab");

            // ACT
            var result = await productRepository.DeleteAsync(testId);

            // ASSERT
            Assert.False(result);
        }
    }
}
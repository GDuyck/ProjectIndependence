using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Moq;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Tests.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductRepositoryTest
    {
        private readonly IProductRepository productRepository;
        private readonly ServiceProvider serviceProvider;
        private readonly List<Product> mockProducts;

        public ProductRepositoryTest()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase( Guid.NewGuid().ToString() ) );
            services.AddScoped<IProductRepository, ProductRepository>();

            serviceProvider = services.BuildServiceProvider();

            productRepository = serviceProvider.GetRequiredService<IProductRepository>();
        }

        [Fact]
        public async Task ProductRepository_GetAllAsync_ReturnsAllProductInDatabase()
        {
            // ACT
            var result = await productRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ProductRepository_AddAsync_AddProductAndReturnsTheAddedProductAndIsNotNull()
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
            Assert.NotNull(result);
            Assert.Equal(newProduct.Name, result.Name);
            Assert.Equal(newProduct.Id, result.Id);
        }

        [Fact]
        public async Task ProductRepository_GetById_ReturnsProductWithAValidIdAsync()
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
            await productRepository.AddAsync(newProduct);
            var getByIdResult = await productRepository.GetByIdAsync(newProduct.Id);

            // ASSERT
            Assert.Equal(newProduct.Id, getByIdResult.Id);
            Assert.Equal(newProduct.Name, getByIdResult.Name);
        }
    }
}

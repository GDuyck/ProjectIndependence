using FluentAssertions;
using Microsoft.AspNetCore.Http;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus;
using ProjectIndependence.API.Application.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Infrastructure.Repositories;
using ProjectIndependence.API.Tests.Integration.Common;
using ProjectIndependence.API.Tests.Integration.Seeding;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Products
{
    public class ProductControllerTests(SqlServerContainerFixture sqlServerContainerFixture) : IntegrationTestBase(sqlServerContainerFixture)
    {
        #region GetProducts

        [Fact]
        public async Task GetProducts_WithDataInDatabase_returns200OKWithProducts()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var request = "/api/products";

            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductListDto>>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.Equal(SeedingData.ProductsToSeed().Count(), result!.Data!.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_WithNoDataInDatabase_Returns200WithEmptyList()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: false);
            var client = factory.CreateClient();
            var request = "/api/products";

            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductListDto>>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.Empty(result.Data!);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion GetProducts

        #region GetProductById

        [Fact]
        public async Task GetProductById_WthValidId_Returns200OKWithProduct()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var validProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var validProductId = validProduct!.Id;
            var request = $"/api/products/{validProductId}";

            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(validProductId, result.Data!.Id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_WithInvalidId_Returns404NotFound()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var invalidProductId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}";

            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion GetProductById

        #region GetProductPriceChangeHistory

        [Fact]
        public async Task GetProductPriceRangeHistoryListQuery_WithValidProductId_Returns200OKWithPriceChangeHistoryList()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var validProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var validProductId = validProduct!.Id;
            var request = $"/api/products/{validProductId}/pricechanges";

            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(validProductId, result.Data!.FirstOrDefault()!.ProductId);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPriceChangeHistoryQuery_WithValidProductIdAndHistoryId_Returns200OKWithPriceChangeHistory()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var validProductPriceChange = SeedingData.ProductPriceChangesToSeed().FirstOrDefault();
            var validProductId = validProductPriceChange!.ProductId;

            var validHistoryResponse = await client.GetAsync($"/api/products/{validProductId}/pricechanges", cancellationToken: TestContext.Current.CancellationToken);
            validHistoryResponse.EnsureSuccessStatusCode();

            var validHistoryResult = await validHistoryResponse.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>(cancellationToken: TestContext.Current.CancellationToken);

            var validHistoryId = validHistoryResult!.Data!.FirstOrDefault()!.Id;

            var request = $"/api/products/{validProductId}/pricechanges/{validHistoryId}";
            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);
            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductPriceChangeHistoryDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(validProductId, result.Data!.ProductId);
            Assert.Equal(validHistoryId, result.Data.Id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductPriceChangeHistoryListQuery_WithoutDataInDataBase_Returns200OKWithEmptyList()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: false);
            var client = factory.CreateClient();
            var someProductId = Guid.NewGuid();
            var request = $"/api/products/{someProductId}/pricechanges";
            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);
            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data!);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductPriceChangeHistoryQuery_WithInvalidProductIdOrHistoryId_Returns404NotFound()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var invalidProductId = Guid.NewGuid();
            var invalidHistoryId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}/pricechanges/{invalidHistoryId}";
            // Act
            var response = await client.GetAsync(request, cancellationToken: TestContext.Current.CancellationToken);
            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductPriceChangeHistoryDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion GetProductPriceChangeHistory

        #region PostCreateProduct

        [Fact]
        public async Task PostProductAsync_WithValidInput_Returns200OkWithNewProduct()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var request = "/api/products";
            var newProduct = new CreateProductCommand
            {
                ProductCode = "NEWPROD001",
                Name = "New Product",
                Description = "This is a new product.",
                IsActive = true,
                RetailPrice = 29.99m,
                CostPrice = 15.00m,
                Tax = 21,
                Stock = 100,
                CreatedBy = "testuser"
            };

            // Act
            var response = await client.PostAsJsonAsync(request, newProduct, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data!.Id);
            Assert.Equal(newProduct.ProductCode, result.Data!.ProductCode);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(PostProductData.InvalidProductData),
            MemberType = typeof(PostProductData)
        )]
        public async Task PostProductAsync_WithInvalidInput_Returns400BadRequest(
            string productCode, string name, string description, bool isActive,
            decimal retailPrice, decimal costPrice, int tax, int stock, string createdBy)
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var request = "/api/products";
            var newWrongProduct = new CreateProductCommand
            {
                ProductCode = productCode,
                Name = name,
                Description = description,
                IsActive = isActive,
                RetailPrice = retailPrice,
                CostPrice = costPrice,
                Tax = tax,
                Stock = stock,
                CreatedBy = createdBy
            };

            // Act
            var response = await client.PostAsJsonAsync(request, newWrongProduct, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion PostCreateProduct

        #region PUT

        [Fact]
        public async Task PutProductAsync_WithValidInput_Returns200OkWithUpdatedProduct()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var request = $"/api/products/{existingProductId}";
            var updatedProduct = new UpdateProductCommand
            {
                Id = existingProductId,
                Name = "Updated product",
                Description = "This is an updated product.",
                Tax = 0
            };

            // Act
            var response = await client.PutAsJsonAsync(request, updatedProduct, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(existingProductId, result.Data!.Id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(existingProduct!.Name, result.Data!.Name);
            Assert.NotEqual(existingProduct.Description, result.Data!.Description);
        }

        [Fact]
        public async Task PutProductAsync_WithInvalidIdInRequestOrBody_Returns400BadRequest()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var invalidProductId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}";
            var updatedProduct = new UpdateProductCommand
            {
                Id = existingProductId,
                Name = "Updated product",
                Description = "This is an updated product.",
                Tax = 0
            };

            // Act
            var response = await client.PutAsJsonAsync(request, updatedProduct, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(PutProductData.InvalidPutProductData),
            MemberType = typeof(PutProductData)
        )]
        public async Task PutProductAsync_WithInvalidInput_Returns400BadRequest(string name, string description, int tax)
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var request = $"/api/products/{existingProductId}";
            var updatedWrongProduct = new UpdateProductCommand
            {
                Id = existingProductId,
                Name = name,
                Description = description,
                Tax = tax
            };

            // Act
            var response = await client.PutAsJsonAsync(request, updatedWrongProduct, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion PUT

        #region PatchProduct

        [Fact]
        public async Task UpdatePrice_WithValidInput_Returns200OkWithProductWithUpdatedPrice()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var request = $"/api/products/{existingProductId}/price";
            var newPrice = existingProduct.RetailPrice + 10;
            var updatePrice = new UpdateProductPriceCommand
            {
                Id = existingProductId,
                RetailPrice = newPrice,
                CostPrice = existingProduct.CostPrice,
                ReasonForPriceChange = "Price increased due to higher costs.",
                UpatedBy = "testuser"
            };

            // Act
            var response = await client.PatchAsJsonAsync(request, updatePrice, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(existingProductId, result.Data!.Id);
            Assert.Equal(newPrice, result.Data!.RetailPrice);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePrice_WithMismatchingIds_Returns400BadRequest()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var invalidProductId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}/price";
            var updatePrice = new UpdateProductPriceCommand
            {
                Id = existingProductId,
                RetailPrice = existingProduct.RetailPrice + 10,
                CostPrice = existingProduct.CostPrice,
                ReasonForPriceChange = "Price increased due to higher costs.",
                UpatedBy = "testuser"
            };

            // Act
            var response = await client.PatchAsJsonAsync(request, updatePrice, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.NotNull(response);
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePrice_WithInvalidInput_Returns400BadRequest()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var request = $"/api/products/{existingProductId}/price";
            var updatePrice = new UpdateProductPriceCommand
            {
                Id = existingProductId,
                RetailPrice = -10, // Invalid retail price
                CostPrice = existingProduct.CostPrice,
                ReasonForPriceChange = "Invalid price change.",
                UpatedBy = "testuser"
            };
            // Act
            var response = await client.PatchAsJsonAsync(request, updatePrice, cancellationToken: TestContext.Current.CancellationToken);
            // Assert
            Assert.NotNull(response);
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ToggleProductStatus_WithValidInput_Returns200OkWithUpdatedProduct()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var request = $"/api/products/{existingProductId}/status";
            var toggleStatus = new ToggleProductStatusCommand(existingProductId);

            // Act
            var response = await client.PatchAsJsonAsync(request, toggleStatus, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(existingProductId, result.Data!.Id);
            Assert.NotEqual(existingProduct.IsActive, result.Data!.IsActive);
        }

        [Fact]
        public async Task ToggleProductStatus_WithMismatchingIds_Returns400BadRequest()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;
            var invalidProductId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}/status";
            var toggleStatus = new ToggleProductStatusCommand(existingProductId);

            // Act
            var response = await client.PatchAsJsonAsync(request, toggleStatus, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.NotNull(response);
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ToggleProductStatus_WithBothInvalidIds_Returns404NotFound()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var invalidProductId = Guid.NewGuid();
            var request = $"/api/products/{invalidProductId}/status";
            var toggleStatus = new ToggleProductStatusCommand(invalidProductId);

            // Act
            var response = await client.PatchAsJsonAsync(request, toggleStatus, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.NotNull(response);
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion PatchProduct

        #region Repository

        [Fact]
        public async Task ProductRepository_ProductExists_WithValidId_ReturnsTrue()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new ProductRepository(context);

            var existingProduct = SeedingData.ProductsToSeed().FirstOrDefault();
            var existingProductId = existingProduct!.Id;

            // Act
            var result = await repository.ProductExists(existingProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProductRepository_ProductExists_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            await using var context = CreateDbContext(seedData: true);
            var repository = new ProductRepository(context);
            var invalidProductId = Guid.NewGuid();

            // Act
            var result = await repository.ProductExists(invalidProductId, TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        #endregion Repository
    }
}
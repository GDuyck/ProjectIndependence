using Microsoft.AspNetCore.Http;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Tests.Integration.Common;
using ProjectIndependence.API.Tests.Seeding;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Products
{
    public class ProductControllerTests : IntegrationTestBase
    {
        public ProductControllerTests(SqlServerContainerFixture sqlServerContainerFixture) : base(sqlServerContainerFixture)
        {
        }

        #region GetProducts

        [Fact]
        public async Task GetProducts_WithDataInDatabase_returns200OKWithProducts()
        {
            // Arrange
            await using var factory = CreateFactory(seedData: true);
            var client = factory.CreateClient();
            var request = "/api/products";

            // Act
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductListDto>>>();

            Assert.NotNull(result);
            Assert.True(result!.Success);
            Assert.Equal(SeedingData.ProductsToSeed().Count(), result.Data.Count);
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
            var response = await client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductListDto>>>();

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
            var response = await client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>();

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
            var response = await client.GetAsync(request);

            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductDto>>();

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
            var response = await client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>();

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

            var validHistoryResponse = await client.GetAsync($"/api/products/{validProductId}/pricechanges");
            validHistoryResponse.EnsureSuccessStatusCode();

            var validHistoryResult = await validHistoryResponse.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>();

            var validHistoryId = validHistoryResult!.Data!.FirstOrDefault()!.Id;

            var request = $"/api/products/{validProductId}/pricechanges/{validHistoryId}";
            // Act
            var response = await client.GetAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductPriceChangeHistoryDto>>();
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
            var response = await client.GetAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<ProductPriceChangeHistoryListDto>>>();
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
            var response = await client.GetAsync(request);
            // Assert
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ProductPriceChangeHistoryDto>>();
            Assert.NotNull(result);
            Assert.False(result!.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion GetProductPriceChangeHistory
    }
}
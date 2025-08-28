using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Tests.Integration;
using ProjectIndependence.API.Tests.Seeding;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductIntegrationTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public ProductIntegrationTests(CustomWebApplicationFactory factory): base(factory) { }

        [Fact]
        public async Task GetAll_ReturnOkWithProducts()
        {
            // ACT
            var response = await _httpClient.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DtoProduct[]>>();

            // ASSERT
            response.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ReturnsOkWithProduct()
        {
            // ARRANGE
            var productToLookUp = SeedingData.ProductsToSeed()[0]; // First product in the list

            // ACT
            var response = await _httpClient.GetAsync($"api/products/{productToLookUp.Id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DtoProduct>>();

            // ASSERT
            response.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(productToLookUp.Id);
            result.Data.Name.Should().Be(productToLookUp.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // ARRANGE
            var wrongId = Guid.NewGuid();

            // ACT
            var response = await _httpClient.GetAsync($"api/products/{wrongId}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // ASSERT
            response.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // Deserialize error
            var problemDetail = (result.Error as JsonElement?)?.Deserialize<ProblemDetails>();

            problemDetail.Should().NotBeNull();
            problemDetail.Title.Should().Be(ValidationErrors.NotFoundTitle);
            problemDetail.Detail.Should().Be(ValidationErrors.ProductNotFound + wrongId);
        }

        [Fact]
        public async Task PostAsync_WithValidInput_ReturnsCreatedWithNewProduct()
        {
            // ARRANGE
            var newProduct = new DtoCreateProduct
            {
                Name = "New Product",
                Price = 750,
                Tax = 21
            };

            var newProductJson = JsonSerializer.Serialize(newProduct);

            var contentToSend = new StringContent(newProductJson, Encoding.UTF8, "application/json");

            // ACT
            var response = await _httpClient.PostAsync("api/products", contentToSend);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DtoProduct>>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Name.Should().Be(newProduct.Name);
            result.Data.Price.Should().Be(newProduct.Price);
            result.Data.Tax.Should().Be(newProduct.Tax);
        }

        [Fact]
        public async Task Update_WithValidContent_ReturnsOkWithUpdatedProduct()
        {
            // ARRANGE
            string newName = "Product with updated name";

            var oldProduct = SeedingData.ProductsToSeed()[0];

            var updatedProduct = new Product
            {
                Id = oldProduct.Id,
                Name = newName,
                Price = oldProduct.Price,
                Tax = oldProduct.Tax
            };

            var updateProductJson = JsonSerializer.Serialize(updatedProduct);

            var contentToSend = new StringContent(updateProductJson, Encoding.UTF8, "application/json");

            // ACT
            var response = await _httpClient.PutAsync($"api/products/{oldProduct.Id}", contentToSend);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DtoProduct>>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Data.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Name.Should().Be(updatedProduct.Name);
            result.Data.Price.Should().Be(updatedProduct.Price);
            result.Data.Tax.Should().Be(updatedProduct.Tax);
            result.Data.Id.Should().Be(oldProduct.Id);
        }
    }
}
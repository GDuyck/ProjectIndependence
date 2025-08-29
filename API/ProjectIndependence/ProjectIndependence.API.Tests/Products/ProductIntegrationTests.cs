using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Tests.Integration;
using ProjectIndependence.API.Tests.Seeding;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductIntegrationTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {
        public ProductIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

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
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();

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

            var mapper = new Mapper();

            var iWantToCheckSomething = mapper.From(newProduct).AdaptToType<Product>();

            // ACT
            var response = await _httpClient.PostAsJsonAsync("api/products", newProduct);
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
        public async Task PostAsync_WithInvalidInput_ReturnsBadRequestWithErrorMessage()
        {
            // ARRANGE
            var newProduct = new DtoCreateProduct
            {
                Name = "",
                Price = 0,
                Tax = 0
            };

            // ACT
            var response = await _httpClient.PostAsJsonAsync("api/products", newProduct);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // Deserialize error
            var problemDetail = (result.Error as JsonElement?)?.Deserialize<ValidationProblemDetails>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();

            problemDetail.Should().NotBeNull();
            problemDetail.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain(ValidationErrors.Name);
            problemDetail.Errors.Should().ContainKey("Price")
                .WhoseValue.Should().Contain(ValidationErrors.ProductPriceNotZero);
            problemDetail.Errors.Should().ContainKey("Tax")
                .WhoseValue.Should().Contain(ValidationErrors.ProductTax);
        }

        [Fact]
        public async Task Update_WithValidContent_ReturnsOkWithUpdatedProduct()
        {
            // ARRANGE
            string newName = "Product with updated name";

            var oldProduct = SeedingData.ProductsToSeed()[0];

            //var updatedProduct = new Product(oldProduct.Id, newName, oldProduct.Price, oldProduct.Tax);
            var updatedProduct = new DtoCreateProduct
            {
                Id = oldProduct.Id,
                Name = newName,
                Price = oldProduct.Price,
                Tax = oldProduct.Tax
            };

            // ACT
            var response = await _httpClient.PutAsJsonAsync($"api/products/{oldProduct.Id}", updatedProduct);
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

        [Fact]
        public async Task UpdateAsync_WithInvalidInput_ReturnsBadRequestWithErrorMessage()
        {
            // ARRANGE
            var oldProduct = SeedingData.ProductsToSeed()[0];

            var newProduct = new DtoCreateProduct
            {
                Id = oldProduct.Id,
                Name = "",
                Price = 0,
                Tax = 0
            };

            // ACT
            var response = await _httpClient.PutAsJsonAsync($"api/products/{oldProduct.Id}", newProduct);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // Deserialize error
            var problemDetail = (result.Error as JsonElement?)?.Deserialize<ValidationProblemDetails>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();

            problemDetail.Should().NotBeNull();
            problemDetail.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain(ValidationErrors.Name);
            problemDetail.Errors.Should().ContainKey("Price")
                .WhoseValue.Should().Contain(ValidationErrors.ProductPriceNotZero);
            problemDetail.Errors.Should().ContainKey("Tax")
                .WhoseValue.Should().Contain(ValidationErrors.ProductTax);
        }

        [Fact]
        public async Task UpdateAsync_WithNotMatchingIds_ReturnsNotFoundWithErrorMessage()
        {
            // ARRANGE
            string newName = "Product with new name";

            var oldProduct = SeedingData.ProductsToSeed()[0];

            //var updatedProduct = new Product(Guid.NewGuid(), newName, oldProduct.Price, oldProduct.Tax);
            var updatedProduct = new DtoCreateProduct
            {
                Id = Guid.NewGuid(),
                Name = newName,
                Price = oldProduct.Price,
                Tax = oldProduct.Tax
            };

            // ACT
            var response = await _httpClient.PutAsJsonAsync($"api/products/{oldProduct.Id}", updatedProduct);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // Deserialize error
            var problemDetail = (result.Error as JsonElement?)?.Deserialize<ProblemDetails>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();

            problemDetail.Should().NotBeNull();
            problemDetail.Title.Should().Be(ValidationErrors.IdsNotMatchingTitle);
            problemDetail.Detail.Should().Be(ValidationErrors.IdsNotMatching);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnsOkWithDeletedMessage()
        {
            // ARRANGE
            var productIdToDelete = SeedingData.ProductsToSeed()[0].Id;

            // ACT
            var response = await _httpClient.DeleteAsync($"api/products/{productIdToDelete}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("The product has been successfully deleted");
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ReturnsNotFoundWithMessage()
        {
            // ARRANGE
            var productIdToDelete = Guid.NewGuid();

            // ACT
            var response = await _httpClient.DeleteAsync($"api/products/{productIdToDelete}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            // Deserialize error
            var problemDetail = (result.Error as JsonElement?)?.Deserialize<ProblemDetails>();

            // ASSERT
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();

            problemDetail.Should().NotBeNull();
            problemDetail.Title.Should().Be(ValidationErrors.NotFoundTitle);
            problemDetail.Detail.Should().Be(ValidationErrors.ProductNotFound + productIdToDelete);
        }
    }
}
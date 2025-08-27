using FluentAssertions;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Tests.Integration;
using ProjectIndependence.API.Tests.Seeding;
using System.Net.Http.Json;
using Xunit;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public ProductIntegrationTests(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
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
        }
    }
}
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Tests.Integration;
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
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
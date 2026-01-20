using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Infrastructure.Data;
using Xunit;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductIntegrationTests : IClassFixture<ProjectIndependence.API.Tests.Integration.CustomWebApplicationFactory>
    {
        private readonly ProjectIndependence.API.Tests.Integration.CustomWebApplicationFactory _factory;

        public ProductIntegrationTests(ProjectIndependence.API.Tests.Integration.CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Post_CreateProduct_ReturnsCreatedAndPersists()
        {
            // Arrange
            var client = _factory.CreateClient();

            var command = new CreateProductCommand
            {
                Name = "Integration Product",
                ProductCode = "IP-001",
                Description = "Created during integration test",
                IsActive = true,
                RetailPrice = 12.34m,
                CostPrice = 6.50m,
                Tax = 21,
                Stock = 20,
                CreatedBy = "IntegrationTest"
            };

            var json = JsonSerializer.Serialize(command, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Products", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductDto>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal(command.Name, apiResponse.Data.Name);

            // Verify persisted in database
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var saved = await db.Products.FindAsync(apiResponse.Data.Id);

            Assert.NotNull(saved);
            Assert.Equal(command.ProductCode, saved.ProductCode);
        }
    }
}

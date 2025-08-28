using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Tests.Integration
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected readonly HttpClient _httpClient;
        private readonly IServiceScope _serviceScope;
        protected readonly ApplicationDbContext _applicationDbContext;

        protected IntegrationTestBase(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
            _serviceScope = factory.Services.CreateScope();
            _applicationDbContext = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public async Task InitializeAsync()
        {
            // Reset db
            await _applicationDbContext.Database.EnsureDeletedAsync();
            await _applicationDbContext.Database.MigrateAsync();

            // Reseed

            _applicationDbContext.Products.AddRange(
                new Product
                {
                    Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"),
                    Name = "Test product 1",
                    Price = 20,
                    Tax = 21,
                },
                new Product
                {
                    Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),
                    Name = "Test product 2",
                    Price = 40,
                    Tax = 12
                }
             );

            await _applicationDbContext.SaveChangesAsync();
        }

        public Task DisposeAsync()
        {
            _serviceScope.Dispose();
            return Task.CompletedTask;
        }
    }
}
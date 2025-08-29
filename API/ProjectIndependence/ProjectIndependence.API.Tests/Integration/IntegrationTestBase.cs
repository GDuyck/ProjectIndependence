using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Seeding;

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
                SeedingData.ProductsToSeed()
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
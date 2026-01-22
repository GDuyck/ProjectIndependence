using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Application.Extensions;
using ProjectIndependence.API.Extensions;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Extensions;
using ProjectIndependence.API.Tests.Seeding;

namespace ProjectIndependence.API.Tests.Servicebuilder
{
    public class TestServiceProviderFixture : IDisposable
    {
        public ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructureServicesWithoutDb();
            services.AddMapster();
            MapsterConfig.RegisterMappings();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection), ServiceLifetime.Transient);

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext).GetAwaiter().GetResult();

            return serviceProvider;
        }

        public void Dispose()
        {
        }

        private static async Task SeedDatabase(ApplicationDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(SeedingData.ProductsToSeed());
                dbContext.ProductPriceChanges.AddRange(SeedingData.ProductPriceChangesToSeed());
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
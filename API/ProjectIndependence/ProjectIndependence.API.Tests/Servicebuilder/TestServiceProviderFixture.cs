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
        private readonly SqliteConnection _connection;
        public ServiceProvider ServiceProvider { get; }

        public TestServiceProviderFixture()
        {
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructureServicesWithoutDb();
            services.AddMapster();
            MapsterConfig.RegisterMappings();

            // Add sqlite for in memory db
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_connection), ServiceLifetime.Transient);

            ServiceProvider = services.BuildServiceProvider();

            // Initialize database
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _connection.Close();
            (ServiceProvider as IDisposable)?.Dispose();
        }

        private static async Task SeedDatabase(ApplicationDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(SeedingData.ProductsToSeed());
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
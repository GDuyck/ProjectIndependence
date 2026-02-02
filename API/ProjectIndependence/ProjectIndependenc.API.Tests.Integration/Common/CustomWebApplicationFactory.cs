using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Seeding;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;
        private readonly bool _seedData;
        private readonly string _databaseName;

        public CustomWebApplicationFactory(string connectionString, bool seedData)
        {
            _connectionString = connectionString;
            _seedData = seedData;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Remove the existing ApplicationDbContext registration
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                // Register dbcontext with testcontainers
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(_connectionString));

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                if (_seedData)
                {
                    SeedingData.SeedProducts(dbContext);
                }
            });
        }
    }
}
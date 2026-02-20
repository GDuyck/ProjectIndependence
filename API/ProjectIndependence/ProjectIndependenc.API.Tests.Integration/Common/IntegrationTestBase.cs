using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Tests.Integration.Seeding;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    [Collection("IntegrationTests")]
    public abstract class IntegrationTestBase
    {
        protected readonly SqlServerContainerFixture _sqlServerContainerFixture;

        protected IntegrationTestBase(SqlServerContainerFixture sqlServerContainerFixture)
        {
            _sqlServerContainerFixture = sqlServerContainerFixture;
        }

        protected CustomWebApplicationFactory CreateFactory(bool seedData = false)
        {
            var connectionString = TestDatebase.CreateConnectionString(
                _sqlServerContainerFixture.Container.GetConnectionString());

            return new CustomWebApplicationFactory(
                connectionString,
                seedData);
        }

        protected ApplicationDbContext CreateDbContext(bool seedData = false)
        {
            var connectionString = TestDatebase.CreateConnectionString(
                _sqlServerContainerFixture.Container.GetConnectionString());

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.Migrate();

            if (seedData)
                SeedingData.SeedProducts(context);

            return context;

        }
    }
}
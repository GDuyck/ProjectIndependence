using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Infrastructure.Data;
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
    }
}
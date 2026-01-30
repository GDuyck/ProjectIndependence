using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Infrastructure.Data;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected HttpClient Client { get; private set; } = null!;
        private CustomWebApplicationFactory _factory = null!;
        private string _datebaseName = null!;

        protected async Task<HttpClient> CreateClientAsync(bool seedData)
        {
            _datebaseName = $"TestDb_{Guid.NewGuid():N}";

            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = "localhost,1433",
                InitialCatalog = _datebaseName,
                UserID = "sa",
                Password = "L1mb0-m@N",
                TrustServerCertificate = true,
            }.ConnectionString;

            _factory = new CustomWebApplicationFactory(connectionString, seedData);
            Client = _factory.CreateClient();
            return Client;
        }

        public async Task DisposeAsync()
        {
            if (_factory != null)
            {
                using var scope = _factory.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await dbContext.Database.EnsureDeletedAsync();
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
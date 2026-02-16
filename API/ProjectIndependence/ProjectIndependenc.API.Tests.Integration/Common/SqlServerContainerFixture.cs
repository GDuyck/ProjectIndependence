using Testcontainers.MsSql;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    public class SqlServerContainerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; }

        public SqlServerContainerFixture()
        {
            Container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("L1mb0-m@N")
                .Build();
        }

        async ValueTask IAsyncLifetime.InitializeAsync()
        {
            await Container.StartAsync();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await Container.DisposeAsync();
        }
    }
}
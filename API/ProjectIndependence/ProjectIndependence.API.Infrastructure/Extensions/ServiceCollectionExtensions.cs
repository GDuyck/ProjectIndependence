using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultDatabase");

            // Add dbContext
            if (environment.IsEnvironment("Test"))
                connectionString = configuration.GetConnectionString("TestDatabase");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            return services;
        }

        public static IServiceCollection AddInfrastructureServicesWithoutDb(this IServiceCollection services)
        {
            return services;
        }
    }
}
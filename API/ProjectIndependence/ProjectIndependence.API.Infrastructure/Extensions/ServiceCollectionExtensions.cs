using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Inventories;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories;

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

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();
            services.AddScoped<IProductQueries, ProductQueries>();
            services.AddScoped<IInventoryQueries, InventoryQueries>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServicesWithoutDb(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
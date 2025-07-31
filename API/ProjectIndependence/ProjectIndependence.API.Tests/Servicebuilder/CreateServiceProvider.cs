using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Services.Products;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Customers;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Infrastructure.Repositories.Sales;

namespace ProjectIndependence.API.Tests.Servicebuilder
{
    public static class CreateServiceProvider
    {
        private static IServiceProvider? _provider;
        public static IServiceProvider Instance => _provider ??= Build();

        public static ServiceProvider CreateProvider<TEntitiy>(List<TEntitiy> entities)
            where TEntitiy : EntityBase
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISalesQuotationRepository, SalesQuotationRepository>();
            services.AddScoped<ISalesQuotationLineRepository, SalesQuotationLineRepository>();

            var serviceProvider = services.BuildServiceProvider();

            var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            appDbContext.Set<TEntitiy>().AddRange(entities);

            appDbContext.SaveChanges();

            return serviceProvider;
        }

        private static IServiceProvider Build()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISalesQuotationRepository, SalesQuotationRepository>();
            services.AddScoped<ISalesQuotationLineRepository, SalesQuotationLineRepository>();

            services.AddMapster();

            services.AddScoped<IProductService, ProductService>();

            var serviceProvider = services.BuildServiceProvider();

            var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            appDbContext.Set<Product>()
                .AddRange(
                        new Product
                        {
                            Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"),
                            Name = "Test product 1",
                            Price = 20,
                            Tax = 21,
                        },
                        new Product
                        {
                            Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),
                            Name = "Test product 2",
                            Price = 40,
                            Tax = 12
                        }
                );

            appDbContext.SaveChanges();

            return serviceProvider;
        }
    }
}
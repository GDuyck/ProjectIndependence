using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Base;
using ProjectIndependence.API.Infrastructure.Repositories.Customers;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Infrastructure.Repositories.Sales;

namespace ProjectIndependence.API.Tests.Servicebuilder
{
    public static class CreateServiceProvider
    {
        public static ServiceProvider CreateProvider<TEntitiy>(List<TEntitiy> entities)
            where TEntitiy : EntityBase
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISalesQuotationRepository, ISalesQuotationRepository>();
            services.AddScoped<ISalesQuotationLineRepository, SalesQuotationLineRepository>();

            var serviceProvider = services.BuildServiceProvider();

            var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            appDbContext.Set<TEntitiy>().AddRange(entities);

            appDbContext.SaveChanges();

            return serviceProvider;
        }
    }
}
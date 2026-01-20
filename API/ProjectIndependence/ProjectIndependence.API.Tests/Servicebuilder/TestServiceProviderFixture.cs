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
using ProjectIndependence.API.Tests.Seeding;

namespace ProjectIndependence.API.Tests.Servicebuilder
{
    public class TestServiceProviderFixture : IDisposable
    {
        public IServiceProvider ServiceProvider;

        public static ServiceProvider CreateProvider<TEntitiy>(List<TEntitiy> entities)
            where TEntitiy : EntityBase
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            var serviceProvider = services.BuildServiceProvider();

            var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            appDbContext.Set<TEntitiy>().AddRange(entities);

            appDbContext.SaveChanges();

            return serviceProvider;
        }

        public TestServiceProviderFixture()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Transient);

            services.AddMapster();

            ServiceProvider = services.BuildServiceProvider();

            var appDbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            appDbContext.SaveChanges();
        }

        public void Dispose()
        {
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}
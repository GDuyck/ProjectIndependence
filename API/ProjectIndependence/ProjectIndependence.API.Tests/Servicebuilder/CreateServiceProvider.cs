using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Base;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Servicebuilder
{
    public class CreateServiceProvider
    {
        public static ServiceProvider CreateProvider<TEntitiy, TRepositoryInterface, TRepository>(List<TEntitiy> entities)
            where TEntitiy : EntityBase
            where TRepositoryInterface : IBaseRepository<TEntitiy>
            where TRepository : BaseRepository<TEntitiy>
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IProductRepository, ProductRepository>();

            var serviceProvider = services.BuildServiceProvider();

            var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();


            foreach (var entity in entities)
                appDbContext.Set<TEntitiy>().Add(entity);

            appDbContext.SaveChanges();

            return serviceProvider;
        }
    }
}

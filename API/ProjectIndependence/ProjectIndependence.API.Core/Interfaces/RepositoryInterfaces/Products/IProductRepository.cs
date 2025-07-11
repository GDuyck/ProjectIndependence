using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products
{
    public interface IProductRepository : IBaseRepository<Product>
    {
    }
}

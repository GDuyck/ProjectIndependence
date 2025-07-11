using ProjectIndependence.API.Core.Entities.Customers;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
    }
}

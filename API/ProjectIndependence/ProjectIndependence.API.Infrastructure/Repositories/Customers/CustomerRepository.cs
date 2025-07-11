using ProjectIndependence.API.Core.Entities.Customers;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Infrastructure.Repositories.Customers
{
    public class CustomerRepository(ApplicationDbContext dbContext) : BaseRepository<Customer>(dbContext), ICustomerRepository
    {
    }
}

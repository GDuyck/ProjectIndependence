using ProjectIndependence.API.Core.Entities.Sales;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Infrastructure.Repositories.Sales
{
    public class SalesQuotationRepository(ApplicationDbContext dbContext) : BaseRepository<SalesQuotation>(dbContext), ISalesQuotationRepository
    {
    }
}

using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

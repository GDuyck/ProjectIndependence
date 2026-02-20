using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

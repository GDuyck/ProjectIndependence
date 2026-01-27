using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus
{
    public class ToggleProductStatusCommandHandler : ICommandHandler<ToggleProductStatusCommand, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ToggleProductStatusCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(ToggleProductStatusCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            product.ToggleStatus();

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
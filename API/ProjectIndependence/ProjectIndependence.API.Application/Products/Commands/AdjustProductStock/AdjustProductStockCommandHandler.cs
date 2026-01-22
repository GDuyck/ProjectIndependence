using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Commands.AdjustProductStock
{
    public class AdjustProductStockCommandHandler : ICommandHandler<AdjustProductStockCommand, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AdjustProductStockCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(AdjustProductStockCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id);

            product.Stock += command.QuantityChange;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var updateProductDto = product.Adapt<ProductDto>();

            return updateProductDto;
        }
    }
}
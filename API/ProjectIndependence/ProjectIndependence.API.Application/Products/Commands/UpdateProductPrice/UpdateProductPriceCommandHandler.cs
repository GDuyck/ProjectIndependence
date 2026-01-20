using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UpdateProductPriceCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductPriceCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(product => product.Id == command.Id, cancellationToken);

            product.UpdatePrice(command.RetailPrice);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
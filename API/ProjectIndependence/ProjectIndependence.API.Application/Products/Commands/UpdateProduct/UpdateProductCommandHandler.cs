using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UpdateProductCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            product.UpdateProduct(command.Name, command.ProductCode, command.Description);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
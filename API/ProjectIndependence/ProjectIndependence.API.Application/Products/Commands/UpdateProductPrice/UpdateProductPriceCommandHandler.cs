using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
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

            var oldCostPrice = product.CostPrice;
            var oldRetailPrice = product.RetailPrice;

            product.UpdatePrice(command.RetailPrice, command.CostPrice);


            var ProductPriceChange = new ProductPriceChange
            (
                product.Id,
                oldRetailPrice,
                command.RetailPrice,
                oldCostPrice,
                command.CostPrice,
                command.ReasonForPriceChange,
                command.UpatedBy
            );

            await _applicationDbContext.ProductPriceChanges.AddAsync(ProductPriceChange, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
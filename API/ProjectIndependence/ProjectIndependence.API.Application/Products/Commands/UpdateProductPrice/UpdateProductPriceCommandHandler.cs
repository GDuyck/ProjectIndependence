using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //var product = await _productRepository.GetByIdAsync(command.Id);

            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(product => product.Id == command.Id);

            product.RetailPrice = command.RetailPrice;

            product.UpdatedAt = DateTime.Now;

            var updatedProduct = await _productRepository.UpdateAsync(product);

            var updatedProductDto = updatedProduct.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}

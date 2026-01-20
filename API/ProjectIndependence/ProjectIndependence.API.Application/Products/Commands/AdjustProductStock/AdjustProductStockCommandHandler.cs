using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //var product = await _productRepository.GetByIdAsync(command.Id);

            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id);

            product.Stock += command.QuantityChange;

            var updatedProduct = await _productRepository.UpdateAsync(product);

            var updateProductDto = updatedProduct.Adapt<ProductDto>();

            return updateProductDto;
        }
    }
}

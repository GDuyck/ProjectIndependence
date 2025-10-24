using Mapster;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Products.Commands.AdjustProductStock
{
    public class AdjustProductStockCommandHandler : ICommandHandler<AdjustProductStockCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public AdjustProductStockCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(AdjustProductStockCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);

            product.Stock += command.QuantityChange;

            var updatedProduct = await _productRepository.UpdateAsync(product);

            var updateProductDto = updatedProduct.Adapt<ProductDto>();

            return updateProductDto;
        }
    }
}

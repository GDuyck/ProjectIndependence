using Mapster;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Products.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductPriceCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductPriceCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);

            product.RetailPrice = command.RetailPrice;

            product.UpdatedAt = DateTime.Now;

            var updatedProduct = await _productRepository.UpdateAsync(product);

            var updatedProductDto = updatedProduct.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}

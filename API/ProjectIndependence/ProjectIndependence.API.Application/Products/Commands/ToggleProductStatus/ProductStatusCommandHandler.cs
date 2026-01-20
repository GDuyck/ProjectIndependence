using Mapster;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;

namespace ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus
{
    public class ProductStatusCommandHandler : ICommandHandler<ProductStatusCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public ProductStatusCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(ProductStatusCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);

            product.ToggleStatus();

            var updatedProduct = await _productRepository.UpdateAsync(product);

            var updatedProductDto = updatedProduct.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
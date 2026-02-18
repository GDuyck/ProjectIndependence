using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public UpdateProductPriceCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductPriceCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, cancellationToken);

            product.UpdatePrice(command.RetailPrice, command.CostPrice, command.ReasonForPriceChange);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Commands.ToggleProductStatus
{
    public class ToggleProductStatusCommandHandler : ICommandHandler<ToggleProductStatusCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ToggleProductStatusCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> HandleAsync(ToggleProductStatusCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, cancellationToken);

            if (product is null)
                return null;

            product.ToggleStatus();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, cancellationToken);

            product.UpdateProduct(command.Name, command.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedProductDto = product.Adapt<ProductDto>();

            return updatedProductDto;
        }
    }
}
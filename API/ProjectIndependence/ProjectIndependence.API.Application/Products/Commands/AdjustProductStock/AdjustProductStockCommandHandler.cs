using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Commands.AdjustProductStock
{
    public class AdjustProductStockCommandHandler : ICommandHandler<AdjustProductStockCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdjustProductStockCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(AdjustProductStockCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, cancellationToken);

            product.Stock += command.QuantityChange;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updateProductDto = product.Adapt<ProductDto>();

            return updateProductDto;
        }
    }
}
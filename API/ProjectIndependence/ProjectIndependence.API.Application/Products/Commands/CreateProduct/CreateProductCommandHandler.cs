using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetProductListAsync(cancellationToken);
            var exists = products.Any(p => p.ProductCode == command.ProductCode);

            if (exists)
                throw new ProductCodeAlreadyExistsException(command.ProductCode);

            var newProduct = command.Adapt<Product>();

            var createdProduct = await _productRepository.CreateProductAsync(newProduct, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = createdProduct.Adapt<ProductDto>();

            return dto;
        }
    }
}
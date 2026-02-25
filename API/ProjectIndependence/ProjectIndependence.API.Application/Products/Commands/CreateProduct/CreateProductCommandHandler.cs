using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Inventories;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Inventories;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Exceptions;

namespace ProjectIndependence.API.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetProductListAsync(cancellationToken);
            var exists = products.Any(p => p.ProductCode == command.ProductCode);

            if (exists)
                throw new ProductCodeAlreadyExistsException(command.ProductCode);

            var newProduct = command.Adapt<Product>();

            var createdProduct = await _productRepository.CreateProductAsync(newProduct, cancellationToken);

            // Create inventory
            var inventory = new Inventory(createdProduct.Id, command.InitialStock, command.ReorderLevel, command.CreatedBy);

            await _inventoryRepository.CreateInventoryAsync(inventory, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = createdProduct.Adapt<ProductDto>();

            return dto;
        }
    }
}
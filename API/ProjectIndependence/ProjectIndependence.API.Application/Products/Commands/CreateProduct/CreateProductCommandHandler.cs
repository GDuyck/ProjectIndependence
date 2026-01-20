using Mapster;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;

namespace ProjectIndependence.API.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var newProduct = command.Adapt<Product>();

            var entity = await _repository.AddAsync(newProduct);

            var dto = entity.Adapt<ProductDto>();

            return dto;
        }
    }
}
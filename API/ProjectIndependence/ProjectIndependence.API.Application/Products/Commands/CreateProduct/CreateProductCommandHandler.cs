using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateProductCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var newProduct = command.Adapt<Product>();

            await _applicationDbContext.Products.AddAsync(newProduct, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var dto = newProduct.Adapt<ProductDto>();

            return dto;
        }
    }
}
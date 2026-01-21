using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Exceptions;
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
            var exists = await _applicationDbContext.Products
                .AsNoTracking()
                .AnyAsync(p => p.ProductCode == command.ProductCode, cancellationToken);

            if (exists)
                throw new ProductCodeAlreadyExistsException(command.ProductCode);

            var newProduct = command.Adapt<Product>();

            await _applicationDbContext.Products.AddAsync(newProduct, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var dto = newProduct.Adapt<ProductDto>();

            return dto;
        }
    }
}
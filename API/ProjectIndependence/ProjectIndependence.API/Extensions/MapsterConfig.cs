using Mapster;
using ProjectIndependence.API.Application.Products.Commands.CreateProduct;
using ProjectIndependence.API.Application.Products.Dtos;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Extensions
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateProductCommand, Product>.NewConfig()
                .Map(dest => dest.Tax, src => new TaxRate(src.Tax))
                .ConstructUsing(command => new Product
                (
                    Guid.NewGuid(),
                    command.Name,
                    command.ProductCode,
                    command.Description,
                    command.IsActive,
                    command.RetailPrice,
                    command.CostPrice,
                    0,
                    command.Stock,
                    command.CreatedBy
                ));

            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                .Map(dest => dest.Tax, src => src.Tax.Value);
        }
    }
}
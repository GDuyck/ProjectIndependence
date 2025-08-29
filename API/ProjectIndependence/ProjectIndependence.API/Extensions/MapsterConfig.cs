using Mapster;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Extensions
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<DtoCreateProduct, Product>.NewConfig()
                .ConstructUsing(dto => new Product
                (
                    dto.Id != Guid.Empty ? dto.Id : Guid.Empty,
                    dto.Name,
                    dto.Price,
                    dto.Tax
                ));
        }
    }
}

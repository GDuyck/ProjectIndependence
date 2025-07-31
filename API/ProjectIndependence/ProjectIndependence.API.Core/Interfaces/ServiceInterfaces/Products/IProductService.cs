using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Base;

namespace ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products
{
    public interface IProductService : IBaseService<DtoProduct, DtoCreateProduct, Product>
    {
    }
}
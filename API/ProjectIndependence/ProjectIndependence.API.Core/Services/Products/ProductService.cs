using MapsterMapper;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Services.Products
{
    public class ProductService : BaseService<DtoProduct, DtoCreateProduct, Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

    }
}

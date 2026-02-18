using Mapster;
using ProjectIndependence.API.Application.Interfaces;
using ProjectIndependence.API.Application.Interfaces.Products;
using ProjectIndependence.API.Application.Products.Dtos;

namespace ProjectIndependence.API.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default)
        {
            var productEnity = await _productRepository.GetProductByIdReadOnlyAsync(query.Id, cancellationToken);

            var productDto = productEnity.Adapt<ProductDto>();

            return productDto;
        }
    }
}
using MapsterMapper;
using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Base;
using System.Runtime.CompilerServices;

namespace ProjectIndependence.API.Core.Services.BaseService
{
    public class BaseService<TResponseDto, TRequestDto, TEntity, > : IBaseService<TResponseDto, TRequestDto, TEntity>
        where TResponseDto : DtoBase
        where TRequestDto : DtoBase
        where TEntity : EntityBase
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            var dto = _mapper.From(entity).AdaptToType<TResponseDto>();

            return dto;
        }
    }
}
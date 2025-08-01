using MapsterMapper;
using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Base;
using System.Collections;

namespace ProjectIndependence.API.Core.Services.BaseService
{
    public class BaseService<TResponseDto, TRequestDto, TEntity> : IBaseService<TResponseDto, TRequestDto, TEntity>
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

        public virtual async Task<IEnumerable<TResponseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            var dtoList = _mapper.From(entities).AdaptToType<IEnumerable<TResponseDto>>();

            return dtoList;
        }

        public async Task<TResponseDto> AddAsync(TRequestDto requestDto)
        {
            var entity = _mapper.From(requestDto).AdaptToType<TEntity>();

            var result = await _repository.AddAsync(entity);

            var resultDto = _mapper.From(result).AdaptToType<TResponseDto>();

            return resultDto;
        }

        public async Task<TResponseDto> UpdateAsync(TRequestDto requestDto)
        {
            var entity = _mapper.From(requestDto).AdaptToType<TEntity>();

            var result = await _repository.UpdateAsync(entity);

            var resultDto = _mapper.From(result).AdaptToType<TResponseDto>();

            return resultDto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id);

            return result;
        }
    }
}
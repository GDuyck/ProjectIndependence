using ProjectIndependence.API.Core.Dtos.Base;
using ProjectIndependence.API.Core.Entities.Base;

namespace ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Base
{
    public interface IBaseService<TResponseDto, TRequestDto, TEntity>
        where TResponseDto : DtoBase
        where TRequestDto : DtoBase
        where TEntity : EntityBase
    {
        Task<TResponseDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TResponseDto>> GetAllAsync();
        Task<TResponseDto> AddAsync(TRequestDto requestDto);
        Task<TResponseDto> UpdateAsync(TRequestDto requestDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
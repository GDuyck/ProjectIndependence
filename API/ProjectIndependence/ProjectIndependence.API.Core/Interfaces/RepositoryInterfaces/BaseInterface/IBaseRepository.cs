using ProjectIndependence.API.Core.Entities.Base;

namespace ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface
{
    public interface IBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
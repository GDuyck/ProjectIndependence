using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.BaseInterface;
using ProjectIndependence.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Infrastructure.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await FetchAll().ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
        }

        private IQueryable<TEntity> FetchAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }
    }
}

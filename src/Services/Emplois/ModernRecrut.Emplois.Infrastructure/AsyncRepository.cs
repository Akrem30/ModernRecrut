using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.Core.Interfaces;
using ModernRecrut.Emplois.Core.Entites;
using ModernRecrut.Emplois.Infrastructure.Data;

namespace ModernRecrut.Emplois.Infrastructure
{
    public class AsyncRepository<TBaseEntity> : IAsyncRepository<TBaseEntity> where TBaseEntity : Entity
    {
        protected readonly EmploiContext _dbContext;

        public AsyncRepository(EmploiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TBaseEntity entity)
        {
            await _dbContext.Set<TBaseEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TBaseEntity entity)
        {
            _dbContext.Set<TBaseEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(TBaseEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<TBaseEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TBaseEntity>().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<TBaseEntity>> ListAsync()
        {
            return await _dbContext.Set<TBaseEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TBaseEntity>> ListAsync(System.Linq.Expressions.Expression<Func<TBaseEntity, bool>> predicate)
        {
            return await _dbContext.Set<TBaseEntity>()
                .Where(predicate)
                .ToListAsync();
        }
    }
}

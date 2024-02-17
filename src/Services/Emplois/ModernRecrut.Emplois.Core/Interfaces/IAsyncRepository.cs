using System.Linq.Expressions;
using ModernRecrut.Emplois.Core.Entites;

namespace ModernRecrut.Emplois.Core.Interfaces
{
    public interface IAsyncRepository<TBaseEntity> where TBaseEntity : Entity
    {
        Task<TBaseEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TBaseEntity>> ListAsync();
        Task<IEnumerable<TBaseEntity>> ListAsync(Expression<Func<TBaseEntity, bool>> predicate);
        Task AddAsync(TBaseEntity entity);
        Task DeleteAsync(TBaseEntity entity);
        Task EditAsync(TBaseEntity entity);
    }
}

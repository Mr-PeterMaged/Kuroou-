
using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;

namespace Ecommerce.Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec);
        Task<int> CountAsync(ISpecification<TEntity> spec);
		void Add(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);

	}
}

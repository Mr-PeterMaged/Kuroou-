
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructre.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
		public async Task<int> CountAsync(ISpecification<TEntity> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}

		public async Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
		public void Add(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
		}

		public void Update(TEntity entity)
		{
			_context.Set<TEntity>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(TEntity entity)
		{
			_context.Set<TEntity>().Remove(entity);
		}
		private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}

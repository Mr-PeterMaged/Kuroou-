
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
		Task<int> Complete();
	}
}

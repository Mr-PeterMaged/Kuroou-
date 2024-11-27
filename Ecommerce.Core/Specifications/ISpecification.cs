
using Ecommerce.Core.Entities;
using System.Linq.Expressions;

namespace Ecommerce.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; }
		public Expression<Func<T, object>> OrderBy { get; }
		public Expression<Func<T, object>> OrderByDesceding { get; }
		public int Take { get; }
		public int Skip { get; }
		public bool IsPaginationEnabled { get; }
	}
}

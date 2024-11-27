
using Ecommerce.Core.Entities;
using System.Linq.Expressions;

namespace Ecommerce.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();
		public Expression<Func<T, object>> OrderBy { get; private set; }

		public Expression<Func<T, object>> OrderByDesceding { get; private set; }

		public int Take { get; private set; }

		public int Skip { get; private set; }

		public bool IsPaginationEnabled { get; private set; }

		public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

		protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
		{
			OrderBy = orderByExpression;
		}

		protected void AddOrderByDesceding(Expression<Func<T, object>> orderByExpression)
		{
			OrderByDesceding = orderByExpression;
		}

		protected void ApplyPagination(int take, int skip)
		{
			Take = take;
			Skip = skip;
			IsPaginationEnabled = true;
		}
	}
}

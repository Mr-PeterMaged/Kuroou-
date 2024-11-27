
using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructre.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

			if (spec.IsPaginationEnabled)
			{
				query = query.Skip(spec.Skip).Take(spec.Take);
			}

			if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

			if (spec.OrderByDesceding != null)
			{
				query = query.OrderByDescending(spec.OrderByDesceding);
			}

			query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}

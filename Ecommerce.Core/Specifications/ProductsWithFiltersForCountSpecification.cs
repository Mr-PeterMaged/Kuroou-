
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Specifications
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecification<Product>
	{
        public ProductsWithFiltersForCountSpecification(string? sort,
														int? brandId,
														int? typeId,
														string search)
			: base(x =>
			(string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search)) &&
			(!brandId.HasValue || x.ProductBrandId == brandId) &&
			(!typeId.HasValue || x.ProductTypeId == typeId))
		{
            
        }
    }
}

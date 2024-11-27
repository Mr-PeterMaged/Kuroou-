
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(string? sort,
													   int? brandId,
													   int? typeId,
													   int pageIndex,
													   int pageSize,
													   string search)
            :base(x =>
			(string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search)) &&
			(!brandId.HasValue || x.ProductBrandId == brandId) &&
			(!typeId.HasValue || x.ProductTypeId == typeId))
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
			AddOrderBy(x => x.Name);

			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
				{
					case "priceAsc":
						AddOrderBy(x => x.Price);
						break;

					case "priceDesc":
						AddOrderByDesceding(x => x.Price);
						break;

					default:
						AddOrderBy(x => x.Name);
						break;

				}
			}
			ApplyPagination(pageSize, (pageIndex -1 ) * pageSize);
		}
        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}

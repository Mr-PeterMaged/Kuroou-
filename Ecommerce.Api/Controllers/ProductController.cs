using AutoMapper;
using Ecommerce.Api.Dtos;
using Ecommerce.Api.Helpers;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class ProductController : BaseController
	{
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductType> _typeRepository;
        private readonly IMapper _mapper;
        public ProductController(IGenericRepository<Product> productRepository,
                                 IGenericRepository<ProductBrand> brandRepository,
								 IGenericRepository<ProductType> typeRepository,
								 IMapper mapper)
        {
            _productRepository = productRepository;
			_brandRepository = brandRepository;
            _typeRepository = typeRepository;
			_mapper = mapper;
        }
		[Cached(600)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product =  await _productRepository.GetEntityWithSpec(spec);
            var productsToReturn = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(productsToReturn);
        }
		[Cached(600)]
		[HttpGet]
        public async Task<IActionResult> GetProducts(string? sort,
                                                     int? brandId,
                                                     int? typeId,
                                                     int pageIndex,
                                                     int pageSize,
                                                     string? search)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(sort, brandId, typeId, pageIndex, pageSize, search);
            var countSpec = new ProductsWithFiltersForCountSpecification(sort, brandId, typeId, search);

            var products = await _productRepository.ListAsync(spec);
            var count = await _productRepository.CountAsync(countSpec);
            var productsToReturn = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var response = new Pagination<ProductToReturnDto>(pageIndex, pageSize, count, productsToReturn);
            return Ok(response);
        }
		[Cached(600)]
		[HttpGet("get-brands")]
		public async Task<IActionResult> GetBrands()
        {
            var brands = await _brandRepository.ListAllAsync();
            return Ok(brands);
        }
		[Cached(600)]
		[HttpGet("get-types")]
		public async Task<IActionResult> GetTypes()
		{
			var types = await _typeRepository.ListAllAsync();
			return Ok(types);
		}

	}
}


using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructre.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _storeContext;
        public ProductRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<IReadOnlyList<Product>> GetAllProducts()
        {
            var products = await _storeContext.Product.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _storeContext.Product.FindAsync(id);
            return product;
        }
    }
}

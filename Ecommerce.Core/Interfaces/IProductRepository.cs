
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllProducts();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateAsync(Product product, List<ProductCategory> productCategories);
        Task<IEnumerable<Product>> SearchAsync(string? name, long? categoryId, decimal? minPrice, decimal? maxPrice);
        Task<Product?> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(long id);
        Task<bool> IsAvailableAsync(long productId, int requestedQty);
        Task DecreaseStockAsync(long productId, int quantity);
        Task IncreaseStockAsync(long productId, int quantity);
    }
}

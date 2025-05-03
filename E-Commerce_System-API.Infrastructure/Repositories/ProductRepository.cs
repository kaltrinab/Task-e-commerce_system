using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using E_Commerce_System_API.Application.Interfaces;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDapperContext _context;
        private readonly INotificationService _notificationService;

        public ProductRepository(IDapperContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public async Task<int> CreateAsync(Product product, List<ProductCategory> productCategories)
        {
            using var conn = _context.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            var addProductQuery = @"INSERT INTO Products (Name, Description, SKU, Price, Stock, CreatedDate)
                VALUES (@Name, @Description, @SKU, @Price, @Stock, @CreatedDate);
                SELECT last_insert_rowid();";

            var productId = await conn.ExecuteScalarAsync<int>(addProductQuery, product, transaction);

            var categoryQuery = @"INSERT INTO ProductCategories (ProductId, CategoryId)
                        VALUES (@ProductId, @CategoryId);";

            foreach (var category in productCategories)
            {
                await conn.ExecuteAsync(categoryQuery, new
                {
                    ProductId = productId,
                    category.CategoryId
                }, transaction);
            }

            transaction.Commit();
            return productId;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var deleteProductQuery = "DELETE FROM Products WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            var result = await conn.ExecuteAsync(deleteProductQuery, new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var getProductsQuery = "SELECT * FROM Products";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Product>(getProductsQuery);
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            var getproductQuery = "SELECT * FROM Products WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Product>(getproductQuery, new { Id = id });
        }

        public async Task<bool> IsAvailableAsync(long productId, int requestedQty)
        {
            var stockQuery = "SELECT Stock FROM Products WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            var stock = await conn.ExecuteScalarAsync<int?>(stockQuery, new { Id = productId });
            return stock.HasValue && stock.Value >= requestedQty;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string? name, long? categoryId, decimal? minPrice, decimal? maxPrice)
        {

            var searchQuery = @"SELECT p.* FROM Products p
                    LEFT JOIN ProductCategories pc ON p.Id = pc.ProductId
                    WHERE 1=1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                searchQuery += " AND p.Name LIKE @Name";
                parameters.Add("Name", $"%{name}%");
            }

            if (categoryId.HasValue)
            {
                searchQuery += " AND pc.CategoryId = @CategoryId";
                parameters.Add("CategoryId", categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                searchQuery += " AND p.Price >= @MinPrice";
                parameters.Add("MinPrice", minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                searchQuery += " AND p.Price <= @MaxPrice";
                parameters.Add("MaxPrice", maxPrice.Value);
            }

            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Product>(searchQuery, parameters);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var updateproductQuery = @"UPDATE Products SET Name = @Name, Description = @Description,
                    SKU = @SKU, Price = @Price, Stock = @Stock
                    WHERE Id = @Id";

            using var conn = _context.CreateConnection();
            var result = await conn.ExecuteAsync(updateproductQuery, product);
            return result > 0;
        }

        public async Task DecreaseStockAsync(long productId, int quantity)
        {
            var decreasestockquery = @"UPDATE Products SET Stock = Stock - @Quantity WHERE Id = @ProductId AND Stock >= @Quantity";
            using var conn = _context.CreateConnection();

            var affected = await conn.ExecuteAsync(decreasestockquery, new { ProductId = productId, Quantity = quantity });
            if (affected == 0)
                throw new Exception($"Stock deduction failed for product ID {productId}.");

            var stock = await conn.ExecuteScalarAsync<int>("SELECT Stock FROM Products WHERE Id = @Id", new { Id = productId });
            if (stock < 5)
                await _notificationService.NotifyLowStockAsync(productId, stock);
        }

        public async Task IncreaseStockAsync(long productId, int quantity)
        {
            var increqasestockquery = "UPDATE Products SET Stock = Stock + @Quantity WHERE Id = @ProductId";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(increqasestockquery, new { ProductId = productId, Quantity = quantity });
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Application.DTOs.Products;
using E_Commerce_System_API.Application.Interfaces;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<IEnumerable<Product>> GetAllAsync() => _productRepository.GetAllAsync();
        public Task<Product?> GetByIdAsync(int id) => _productRepository.GetByIdAsync(id);
        public Task<IEnumerable<Product>> SearchAsync(string? name, int? categoryId, decimal? min, decimal? max)
            => _productRepository.SearchAsync(name, categoryId, min, max);

        public Task<int> CreateAsync(ProductDto dto)
        {
            var entity = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = dto.SKU,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedDate = dto.CreatedDate
            };

            return _productRepository.CreateAsync(entity, dto.Categories);
        }

        public async Task<bool> UpdateAsync(ProductDto dto)
        {
            var existing = await _productRepository.GetByIdAsync(dto.Id);
            if (existing is null) return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.SKU = dto.SKU;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;

            return await _productRepository.UpdateAsync(existing);
        }
        public Task<bool> DeleteAsync(int id) => _productRepository.DeleteAsync(id);
    }
}

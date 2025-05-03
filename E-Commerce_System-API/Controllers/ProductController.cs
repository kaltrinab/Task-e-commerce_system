using E_Commerce_System_API.Application.DTOs.Products;
using E_Commerce_System_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productService.GetAllAsync());


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _productService.CreateAsync(productDto);
            return Ok("Product created successfully!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id) return BadRequest("Product Id is not the same!");
            var success = await _productService.UpdateAsync(productDto);
            return Ok("Product updated successfully!");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteProduct = await _productService.DeleteAsync(id);
            return deleteProduct ? Ok("Product is deleted!") : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? categoryId, [FromQuery] decimal? min, [FromQuery] decimal? max)
        {
            var result = await _productService.SearchAsync(name, categoryId, min, max);
            return Ok(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Application.DTOs.Products
{
    public class ProductDto
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public string SKU { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public List<ProductCategory> Categories { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Domain.Entities
{
    public class Product
    {
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

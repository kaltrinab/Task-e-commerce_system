using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Application.DTOs.Orders
{
    public class OrderDto
    {
        [Required]
        public long UserId { get; set; }
        public List<OrderProductsDto> Products { get; set; }
    }
    public class OrderProductsDto
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

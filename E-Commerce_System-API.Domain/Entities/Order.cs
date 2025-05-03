using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Domain.Entities
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate {  get; set; }
        public string Status { get; set; }

        public List<OrderProductDetail> OrderProducts { get; set; } 
    }
    public class OrderProductDetail
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

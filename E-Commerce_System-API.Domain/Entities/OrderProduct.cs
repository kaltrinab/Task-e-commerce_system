using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Domain.Entities
{
    public class OrderProduct
    {
        [Key]
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }


    }
}

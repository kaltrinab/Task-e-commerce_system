using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Domain.Entities
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

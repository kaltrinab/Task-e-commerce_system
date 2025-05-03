using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Domain.Entities
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        // public int PersonId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Role {  get; set; }

    }
}

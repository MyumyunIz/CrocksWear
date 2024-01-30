using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public List<Order> Orders { get; set; }
        

        public User() {
            Orders = new List<Order>();
        }

        public User(string name, string email, string password)
        {
            this.Name= name;
            this.Email= email;
            this.Password= password;

            Orders = new List<Order>();

        }
    }
}
